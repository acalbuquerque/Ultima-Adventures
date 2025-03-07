using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseOre : Item, ICommodity
	{
        public override double DefaultWeight
        {
            get { return 4.0; }
        }

        private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}

		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }

		public abstract BaseIngot GetIngot();

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Resource = (CraftResource)reader.ReadInt();
					break;
				}
				case 0:
				{
					OreInfo info;

					switch ( reader.ReadInt() )
					{
						case 0: info = OreInfo.Iron; break;
						case 1: info = OreInfo.DullCopper; break;
						case 2: info = OreInfo.Copper; break;
						case 3: info = OreInfo.Bronze; break;
                        case 4: info = OreInfo.ShadowIron; break;
                        case 5: info = OreInfo.Platinum; break;
                        case 6: info = OreInfo.Gold; break;
						case 7: info = OreInfo.Agapite; break;
						case 8: info = OreInfo.Verite; break;
						case 9: info = OreInfo.Valorite; break;
                        case 10: info = OreInfo.Titanium; break;
                        case 11: info = OreInfo.Rosenium; break;
                        case 12: info = OreInfo.Nepturite; break;
						case 13: info = OreInfo.Obsidian; break;
						case 14: info = OreInfo.Mithril; break;
						case 15: info = OreInfo.Xormite; break;
						case 16: info = OreInfo.Dwarven; break;

						default: info = null; break;
					}

					m_Resource = CraftResources.GetFromOreInfo( info );
					break;
				}
			}
		}

		public BaseOre( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseOre( CraftResource resource, int amount ) : base( 0x19B9 )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
		}

		public BaseOre( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1026583 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1026583 ); // ore
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !CraftResources.IsStandard( m_Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( m_Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( m_Resource ) );
			}
		}

		public override int LabelNumber
		{
			get
			{
				if (m_Resource >= CraftResource.DullCopper && m_Resource <= CraftResource.Valorite)
					return 1042845 + (int)(m_Resource - CraftResource.DullCopper);
				//else if (m_Resource == CraftResource.Titanium)
					//return 6661002;

                return 1042853; // iron ore;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;
			
			if ( RootParent is BaseCreature )
			{
				from.SendLocalizedMessage( 500447 ); // That is not accessible
				return;
			}
			else if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.SendLocalizedMessage( 501971 ); // Select the forge on which to smelt the ore, or another pile of ore with which to combine it.
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 501976 ); // The ore is too far away.
			}
		}

		private class InternalTarget : Target
		{
			private BaseOre m_Ore;

			public InternalTarget( BaseOre ore ) :  base ( 2, false, TargetFlags.None )
			{
				m_Ore = ore;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Ore.Deleted )
					return;

				if ( !from.InRange( m_Ore.GetWorldLocation(), 2 ) )
				{
					from.SendLocalizedMessage( 501976 ); // The ore is too far away.
					return;
				}
				
				#region Combine Ore
				if ( targeted is BaseOre )
				{
					BaseOre ore = (BaseOre)targeted;
					if ( !ore.Movable )
						return;
					else if ( m_Ore == ore )
					{
						from.SendLocalizedMessage( 501972 ); // Select another pile or ore with which to combine this.
						from.Target = new InternalTarget( ore );
						return;
					}
					else if ( ore.Resource != m_Ore.Resource )
					{
						from.SendLocalizedMessage( 501979 ); // You cannot combine ores of different metals.
						return;
					}

					int worth = ore.Amount;
					if ( ore.ItemID == 0x19B9 )
						worth *= 8;
					else if ( ore.ItemID == 0x19B7 )
						worth *= 2;
					else 
						worth *= 4;
					int sourceWorth = m_Ore.Amount;
					if ( m_Ore.ItemID == 0x19B9 )
						sourceWorth *= 8;
					else if ( m_Ore.ItemID == 0x19B7 )
						sourceWorth *= 2;
					else
						sourceWorth *= 4;
					worth += sourceWorth;

					int plusWeight = 0;
					int newID = ore.ItemID;
					if ( ore.DefaultWeight != m_Ore.DefaultWeight )
					{
						if ( ore.ItemID == 0x19B7 || m_Ore.ItemID == 0x19B7 )
						{
							newID = 0x19B7;
						}
						else if ( ore.ItemID == 0x19B9 )
						{
							newID = m_Ore.ItemID;
							plusWeight = ore.Amount * 2;
						}
						else
						{
							plusWeight = m_Ore.Amount * 2;
						}
					}

					if ( (ore.ItemID == 0x19B9 && worth > 120000) || (( ore.ItemID == 0x19B8 || ore.ItemID == 0x19BA ) && worth > 60000) || (ore.ItemID == 0x19B7 && worth > 30000))
					{
						from.SendLocalizedMessage( 1062844 ); // There is too much ore to combine.
						return;
					}
					else if ( ore.RootParent is Mobile && (plusWeight + ((Mobile)ore.RootParent).Backpack.TotalWeight) > ((Mobile)ore.RootParent).Backpack.MaxWeight )
					{ 
						from.SendLocalizedMessage( 501978 ); // The weight is too great to combine in a container.
						return;
					}

					ore.ItemID = newID;
					if ( ore.ItemID == 0x19B9 )
					{
						ore.Amount = worth / 8;
						m_Ore.Delete();
					}
					else if ( ore.ItemID == 0x19B7 )
					{
						ore.Amount = worth / 2;
						m_Ore.Delete();
					}
					else
					{
						ore.Amount = worth / 4;
						m_Ore.Delete();
					}	
					return;
				}
				#endregion

				if ( Server.Engines.Craft.DefBlacksmithy.IsForge( targeted ) )
				{
					double difficulty;

					switch ( m_Ore.Resource )
					{
						default: difficulty = 50.0; break;
						case CraftResource.DullCopper: difficulty = 65.0; break;
                        case CraftResource.Copper: difficulty = 70.0; break;
                        case CraftResource.Bronze: difficulty = 75.0; break;
                        case CraftResource.ShadowIron: difficulty = 80.0; break;
                        case CraftResource.Platinum: difficulty = 85.0; break;
                        case CraftResource.Gold: difficulty = 85.0; break;
						case CraftResource.Agapite: difficulty = 90.0; break;
						case CraftResource.Verite: difficulty = 95.0; break;
						case CraftResource.Valorite: difficulty = 95.0; break;
                        case CraftResource.Titanium: difficulty = 100.0; break;
                        case CraftResource.Rosenium: difficulty = 100.0; break;
                        case CraftResource.Nepturite: difficulty = 105.0; break;
						case CraftResource.Obsidian: difficulty = 105.0; break;
						case CraftResource.Mithril: difficulty = 110.0; break;
						case CraftResource.Xormite: difficulty = 110.0; break;
						case CraftResource.Dwarven: difficulty = 120.0; break;
                    }

					double minSkill = difficulty - 10.0;
					double maxSkill = difficulty + 10.0;
					
					if ( difficulty > 50.0 && difficulty > from.Skills[SkillName.Mining].Value )
					{
                        from.SendMessage(55, "Voc� n�o sabe como derreter este min�rio.");
                        //from.SendLocalizedMessage( 501986 ); // You have no idea how to smelt this strange ore!
						return;
					}
					
					if ( m_Ore.Amount <= 1 && m_Ore.ItemID == 0x19B7 )
					{
                        from.SendMessage(55, "Voc� h� min�rio suficiente para fazer um lingote.");
                        //from.SendLocalizedMessage( 501987 ); // There is not enough metal-bearing ore in this pile to make an ingot.
						return;
					}

					if ( from.CheckTargetSkill( SkillName.Mining, targeted, minSkill, maxSkill ) )
					{
						if ( m_Ore.Amount <= 0 )
						{
                            from.SendMessage(55, "Voc� h� min�rio suficiente para fazer um lingote.");
                            //from.SendLocalizedMessage( 501987 ); // There is not enough metal-bearing ore in this pile to make an ingot.
						}
						else
						{
							int amount = m_Ore.Amount;
							if ( m_Ore.Amount > 30000 )
								amount = 30000;

							BaseIngot ingot = m_Ore.GetIngot();
							
							if ( m_Ore.ItemID == 0x19B7 )
							{
								if ( m_Ore.Amount % 2 == 0 )
								{
									amount /= 2;
									m_Ore.Delete();
								}
								else
								{
									amount /= 2;
									m_Ore.Amount = 1;
								}
							}
								
							else if ( m_Ore.ItemID == 0x19B9 )
							{
								amount *= 2;
								m_Ore.Delete();
							}
							
							else
							{
								amount /= 1;
								m_Ore.Delete();
							}

							ingot.Amount = amount;
							from.AddToBackpack( ingot );
							from.PlaySound( 0x208 );
                            from.SendMessage(55, "Voc� fundiu o min�rio removendo as impurezas e colocou o metal na mochila.");
                            //from.SendLocalizedMessage( 501988 ); // You smelt the ore removing the impurities and put the metal in your backpack.
						}
					}
					else if ( m_Ore.Amount < 2 && m_Ore.ItemID == 0x19B9 )
					{
						from.SendLocalizedMessage( 501990 ); // You burn away the impurities but are left with less useable metal.
						m_Ore.ItemID = 0x19B8;
						from.PlaySound( 0x208 );
					}
					else if ( m_Ore.Amount < 2 && m_Ore.ItemID == 0x19B8 || m_Ore.ItemID == 0x19BA )
					{
						from.SendLocalizedMessage( 501990 ); // You burn away the impurities but are left with less useable metal.
						m_Ore.ItemID = 0x19B7;
						from.PlaySound( 0x208 );
					}
					else
					{
						from.SendLocalizedMessage( 501990 ); // You burn away the impurities but are left with less useable metal.
						m_Ore.Amount /= 2;
						from.PlaySound( 0x208 );
					}
				}
			}
		}
	}

	public class IronOre : BaseOre
	{
		[Constructable]
		public IronOre() : this( 1 )
		{
		}

		[Constructable]
		public IronOre( int amount ) : base( CraftResource.Iron, amount )
		{
		}

		public IronOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4.0; } // Density of real iron (~8g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new IronIngot();
		}
	}

	public class DullCopperOre : BaseOre
	{
		[Constructable]
		public DullCopperOre() : this( 1 )
		{
		}

		[Constructable]
		public DullCopperOre( int amount ) : base( CraftResource.DullCopper, amount )
		{
		}

		public DullCopperOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4.5; } // Density of real copper (~9g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new DullCopperIngot();
		}
	}

	public class ShadowIronOre : BaseOre
	{
		[Constructable]
		public ShadowIronOre() : this( 1 )
		{
		}

		[Constructable]
		public ShadowIronOre( int amount ) : base( CraftResource.ShadowIron, amount )
		{
		}

		public ShadowIronOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4; } // Density of real iron (~8g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new ShadowIronIngot();
		}
	}

	public class CopperOre : BaseOre
	{
		[Constructable]
		public CopperOre() : this( 1 )
		{
		}

		[Constructable]
		public CopperOre( int amount ) : base( CraftResource.Copper, amount )
		{
		}

		public CopperOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4.5; } // Density of real copper (~9g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new CopperIngot();
		}
	}

	public class BronzeOre : BaseOre
	{
		[Constructable]
		public BronzeOre() : this( 1 )
		{
		}

		[Constructable]
		public BronzeOre( int amount ) : base( CraftResource.Bronze, amount )
		{
		}

		public BronzeOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4.5; } // Density of real bronze (~9g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new BronzeIngot();
		}
	}

	public class GoldOre : BaseOre
	{
		[Constructable]
		public GoldOre() : this( 1 )
		{
		}

		[Constructable]
		public GoldOre( int amount ) : base( CraftResource.Gold, amount )
		{
		}

		public GoldOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 9.5; } // Density of real copper (~19g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new GoldIngot();
		}
	}

	public class AgapiteOre : BaseOre
	{
		[Constructable]
		public AgapiteOre() : this( 1 )
		{
		}

		[Constructable]
		public AgapiteOre( int amount ) : base( CraftResource.Agapite, amount )
		{
		}

		public AgapiteOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4; } // Density of real iron (~8g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new AgapiteIngot();
		}
	}

	public class VeriteOre : BaseOre
	{
		[Constructable]
		public VeriteOre() : this( 1 )
		{
		}

		[Constructable]
		public VeriteOre( int amount ) : base( CraftResource.Verite, amount )
		{
		}

		public VeriteOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4; } // Density of real iron (~8g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new VeriteIngot();
		}
	}

	public class ValoriteOre : BaseOre
	{
		[Constructable]
		public ValoriteOre() : this( 1 )
		{

		}

		[Constructable]
		public ValoriteOre( int amount ) : base( CraftResource.Valorite, amount )
		{
		}

		public ValoriteOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 4; } // Density of real iron (~8g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new ValoriteIngot();
		}
	}

    public class TitaniumOre : BaseOre
    {
        public override int Hue { get { return (Server.Misc.MaterialInfo.GetMaterialColor("titanium", "classic", 0)); } }
        //protected override CraftResource DefaultResource { get { return CraftResource.Titanium;  } }

        [Constructable]
        public TitaniumOre() : this(1)
        {
        }

        [Constructable]
        public TitaniumOre(int amount) : base(CraftResource.Titanium, amount)
        {
        }

        public TitaniumOre(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get { return 2.25; } // Density of real titanium (~4.5g/cm�) / 2;
        }

        public virtual int GetLabelNumber()
        {
            return 6661002;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new TitaniumIngot();
        }
    }

    public class RoseniumOre : BaseOre
    {
        public override int Hue { get { return (Server.Misc.MaterialInfo.GetMaterialColor("rosenium", "classic", 0)); } }
        //protected override CraftResource DefaultResource { get { return CraftResource.Titanium;  } }
        [Constructable]
        public RoseniumOre() : this(1)
        {
        }

        [Constructable]
        public RoseniumOre(int amount) : base(CraftResource.Rosenium, amount)
        {
        }

        public RoseniumOre(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get { return 2.25; } // Density of real titanium (~4.5g/cm�) / 2;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new RoseniumIngot();
        }
    }

    public class PlatinumOre : BaseOre
    {
        public override int Hue { get { return (Server.Misc.MaterialInfo.GetMaterialColor("platinum", "classic", 0)); } }
        //protected override CraftResource DefaultResource { get { return CraftResource.Titanium;  } }
        [Constructable]
        public PlatinumOre() : this(1)
        {
        }

        [Constructable]
        public PlatinumOre(int amount) : base(CraftResource.Platinum, amount)
        {
        }

        public PlatinumOre(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight
        {
            get { return 10.5; } // Density of real platinum (~21.5g/cm�) / 2;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override BaseIngot GetIngot()
        {
            return new PlatinumIngot();
        }
    }

    public class ObsidianOre : BaseOre
	{
		[Constructable]
		public ObsidianOre() : this( 1 )
		{
		}

		[Constructable]
		public ObsidianOre( int amount ) : base( CraftResource.Obsidian, amount )
		{
		}

		public ObsidianOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 1.5; } // Density of real obsidian (~3.0g/cm�) / 2;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new ObsidianIngot();
		}
	}

	public class MithrilOre : BaseOre
	{
		[Constructable]
		public MithrilOre() : this( 1 )
		{
		}

		[Constructable]
		public MithrilOre( int amount ) : base( CraftResource.Mithril, amount )
		{
		}

		public MithrilOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 1.0; } // Density of mithril is the lowest possible;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new MithrilIngot();
		}
	}

	public class DwarvenOre : BaseOre
	{
		[Constructable]
		public DwarvenOre() : this( 1 )
		{
		}

		[Constructable]
		public DwarvenOre( int amount ) : base( CraftResource.Dwarven, amount )
		{
		}

		public DwarvenOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 5.0; } // DwarvenOre needs to have a medium density;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new DwarvenIngot();
		}
	}

	public class XormiteOre : BaseOre
	{
		[Constructable]
		public XormiteOre() : this( 1 )
		{
		}

		[Constructable]
		public XormiteOre( int amount ) : base( CraftResource.Xormite, amount )
		{
		}

		public XormiteOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 8.0; } // DwarvenOre needs to have a high density;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new XormiteIngot();
		}
	}

	public class NepturiteOre : BaseOre
	{
		[Constructable]
		public NepturiteOre() : this( 1 )
		{
		}

		[Constructable]
		public NepturiteOre( int amount ) : base( CraftResource.Nepturite, amount )
		{
		}

		public NepturiteOre( Serial serial ) : base( serial )
		{
		}

        public override double DefaultWeight
        {
            get { return 2.0; } // Nepturite is low (base) density;
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override BaseIngot GetIngot()
		{
			return new NepturiteIngot();
		}
	}
}