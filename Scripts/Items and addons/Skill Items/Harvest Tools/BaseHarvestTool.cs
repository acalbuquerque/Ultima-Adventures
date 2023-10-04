using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Craft;
using Server.Engines.Harvest;
using Server.ContextMenus;
using Server.Misc;
using System.Globalization;

namespace Server.Items
{
	public interface IUsesRemaining
	{
		int UsesRemaining{ get; set; }
		bool ShowUsesRemaining{ get; set; }
	}

	public abstract class BaseHarvestTool : Item, IUsesRemaining, ICraftable
	{
		private Mobile m_Crafter;
		private ToolQuality m_Quality;
		private int m_UsesRemaining;
        private CraftResource m_Resource;

        [CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get{ return m_Crafter; }
			set{ m_Crafter = value; InvalidateProperties(); }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { /*UnscaleDurability();*/ m_Resource = value; Hue = CraftResources.GetHue(m_Resource); InvalidateProperties(); /*ScaleDurability();*/ }
        }

        [CommandProperty( AccessLevel.GameMaster )]
		public ToolQuality Quality
		{
			get{ return m_Quality; }
			set{ UnscaleUses(); m_Quality = value; InvalidateProperties(); ScaleUses(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		public void ScaleUses()
		{
			m_UsesRemaining = (m_UsesRemaining * GetUsesScalar()) / 100;
			InvalidateProperties();
		}

		public void UnscaleUses()
		{
			m_UsesRemaining = (m_UsesRemaining * 100) / GetUsesScalar();
		}

		public int GetUsesScalar()
		{
			if ( m_Quality == ToolQuality.Exceptional )
				return 200;

			return 100;
		}

		public bool ShowUsesRemaining{ get{ return true; } set{} }

		public abstract HarvestSystem HarvestSystem{ get; }

		public BaseHarvestTool( int itemID ) : this( 50, itemID )
		{
		}
        public virtual CraftResource DefaultResource { get { return CraftResource.Iron; } }
        public BaseHarvestTool( int usesRemaining, int itemID ) : base( itemID )
		{
			m_UsesRemaining = usesRemaining;
			m_Quality = ToolQuality.Regular;
		}

		public override void GetProperties( ObjectPropertyList list )
		{

            #region [Item Name Color]

            base.GetProperties(list);

            string resourceName = CraftResources.GetName(m_Resource);
            // Makers mark not displayed on OSI - FUCKOF OSI!
            if ( m_Crafter != null )
                list.Add(1050043, ItemNameHue.UnifiedItemProps.SetColor(m_Crafter.Name, "#8be4fc"));
            //list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Quality == ToolQuality.Exceptional )
                list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor("Excepcional", "#ffe066"));
            //list.Add( 1060636 ); // exceptional

            if (resourceName != "")
            {
                //list.Add(1053099, ItemNameHue.UnifiedItemProps.RarityNameMod(this, ((m_Quality == ArmorQuality.Exceptional) ? "Exceptional " : "") + "{0}\t{1}"), resourceName, GetNameString());
                list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor(resourceName, "#8be4fc"));
            }

            list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
            #endregion
        }

        public virtual void DisplayDurabilityTo( Mobile m )
		{
			LabelToAffix( m, 1017323, AffixType.Append, ": " + m_UsesRemaining.ToString() ); // Durability
		}

		public override void OnSingleClick( Mobile from )
		{
			DisplayDurabilityTo( from );

			base.OnSingleClick( from );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (from is PlayerMobile && ((PlayerMobile)from).GetFlag( PlayerFlag.IsAutomated ))
			{
				from.SendMessage(55, "Você não pode usar isso enquanto executa uma ação automatizada.");
				return;
			}
			if ( IsChildOf( from.Backpack ) || Parent == from )
				HarvestSystem.BeginHarvesting( from, this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			AddContextMenuEntries( from, this, list, HarvestSystem );
		}

		public static void AddContextMenuEntries( Mobile from, Item item, List<ContextMenuEntry> list, HarvestSystem system )
		{
			if ( system != Mining.System )
				return;

			if ( !item.IsChildOf( from.Backpack ) && item.Parent != from )
				return;

			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			ContextMenuEntry miningEntry = new ContextMenuEntry( pm.ToggleMiningStone ? 6179 : 6178 );
			miningEntry.Color = 0x421F;
			list.Add( miningEntry );

			list.Add( new ToggleMiningStoneEntry( pm, false, 6176 ) );
			list.Add( new ToggleMiningStoneEntry( pm, true, 6177 ) );
		}

		private class ToggleMiningStoneEntry : ContextMenuEntry
		{
			private PlayerMobile m_Mobile;
			private bool m_Value;

			public ToggleMiningStoneEntry( PlayerMobile mobile, bool value, int number ) : base( number )
			{
				m_Mobile = mobile;
				m_Value = value;

				bool stoneMining = ( mobile.StoneMining && mobile.Skills[SkillName.Mining].Base >= 100.0 );

				if ( mobile.ToggleMiningStone == value || ( value && !stoneMining ) )
					this.Flags |= CMEFlags.Disabled;
			}

			public override void OnClick()
			{
				bool oldValue = m_Mobile.ToggleMiningStone;

				if ( m_Value )
				{
					if ( oldValue )
					{
						m_Mobile.SendLocalizedMessage( 1054023 ); // You are already set to mine both ore and stone!
					}
					else if ( !m_Mobile.StoneMining || m_Mobile.Skills[SkillName.Mining].Base < 100.0 )
					{
						m_Mobile.SendLocalizedMessage( 1054024 ); // You have not learned how to mine stone or you do not have enough skill!
					}
					else
					{
						m_Mobile.ToggleMiningStone = true;
						m_Mobile.SendLocalizedMessage( 1054022 ); // You are now set to mine both ore and stone.
					}
				}
				else
				{
					if ( oldValue )
					{
						m_Mobile.ToggleMiningStone = false;
						m_Mobile.SendLocalizedMessage( 1054020 ); // You are now set to mine only ore.
					}
					else
					{
						m_Mobile.SendLocalizedMessage( 1054021 ); // You are already set to mine only ore!
					}
				}
			}
		}

		public BaseHarvestTool( Serial serial ) : base( serial )
		{
		}

        #region Serialization/Deserialization
        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            Quality = 0x00000008,
            Crafter = 0x00000200,
            Resource = 0x00800000,
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

            writer.Write(m_Crafter);
            writer.WriteEncodedInt((int)m_Quality);
            writer.WriteEncodedInt((int)m_Resource);
            writer.WriteEncodedInt((int)UsesRemaining);
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
            m_Crafter = reader.ReadMobile();
            m_Quality = (ToolQuality)reader.ReadEncodedInt();
            m_Resource = (CraftResource)reader.ReadEncodedInt();
            UsesRemaining = reader.ReadEncodedInt();
        }
        #endregion

        #region ICraftable Members

        public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Quality = (ToolQuality)quality;

			if ( makersMark )
				Crafter = from;

			return quality;
		}

		#endregion
	}
}