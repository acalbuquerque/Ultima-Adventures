using Server;
using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class HorseArmor : Item
	{
		public string ArmorMaterial;

		[CommandProperty(AccessLevel.Owner)]
		public string Armor_Material { get { return ArmorMaterial; } set { ArmorMaterial = value; InvalidateProperties(); } }

        private bool m_Exceptional;
        private Mobile m_Crafter;
        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter { get { return m_Crafter; } set { m_Crafter = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Exceptional { get { return m_Exceptional; } set { m_Exceptional = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource { 
			get { return m_Resource; } 
			set { m_Resource = value; Hue = CraftResources.GetHue(value); InvalidateProperties(); } 
		}

        [Constructable]
		public HorseArmor() : base( 0x040A )
		{
			Weight = 25.0;
			Name = "Armadura de Cavalo";
			Hue = MaterialInfo.GetMaterialColor( "silver", "classic", 0 );

            string resourceName = CraftResources.GetName(m_Resource);
			if (!string.IsNullOrEmpty(resourceName) || resourceName.ToLower() != "none" || resourceName.ToLower() != "normal")
			{
                Hue = MaterialInfo.GetMaterialColor(resourceName.ToLower(), "classic", 0);
				Armor_Material = resourceName;
            }          
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
            string resourceName = CraftResources.GetName(m_Resource);

            base.AddNameProperties( list );

            if (m_Resource != CraftResource.None)
            {
                if (string.IsNullOrEmpty(resourceName) || resourceName.ToLower() == "none" || resourceName.ToLower() == "normal")
                {
                    resourceName = "";
                }

                if (resourceName != "")
                {
                    list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor(resourceName, "#8be4fc"));
                }
            }

            if (m_Exceptional && m_Crafter != null)
			{
                list.Add(1050043, ItemNameHue.UnifiedItemProps.SetColor(m_Crafter.Name, "#8be4fc"));// crafted by ~1_NAME~
                list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor("Excepcional", "#ffe066"));
            }

            list.Add(ItemNameHue.UnifiedItemProps.SetColor("Este item é considerado armadura", "#8be4fc"));
        }

		public override void OnDoubleClick( Mobile from )
		{
			Target t;

			if ( !IsChildOf( from.Backpack ) )
			{
                from.SendMessage(55, "Este item precisa estar na sua mochila.");
			}
			else
			{
                //from.BeginTarget(6, false, TargetFlags.None, new TargetCallback(OnTarget));
                from.SendMessage( "Qual montaria equina você deseja colocar esta armadura?" );
				t = new HorseTarget( this, ArmorMaterial );
				from.Target = t;
			}
		}

		private class HorseTarget : Target
		{
			private HorseArmor m_HorseArmor;
			private string m_ArmorMaterial;

			public HorseTarget( HorseArmor armor, string metal ) : base( 8, false, TargetFlags.None )
			{
                m_HorseArmor = armor;
				m_ArmorMaterial = metal;
			}

			protected override void OnTarget(Mobile from, object obj)
			{
				if (obj is Mobile)
				{
					Mobile mArmor = obj as Mobile;

					if (mArmor is BaseCreature)
					{
						BaseCreature bcArmor = (BaseCreature)obj;

						if ((bcArmor is Horse /*|| bcArmor is ZebraRiding || bcArmor is Zebra || bcArmor is FireSteed || bcArmor is Nightmare || bcArmor is AncientNightmareRiding*/) && bcArmor.ControlMaster == from && bcArmor is BaseMount)
						{
							BaseMount bmArmor = (BaseMount)bcArmor;

							Horse pet = bmArmor as Horse;

							if (pet == null || pet.HasBarding)
							{
								from.SendMessage(55, "Esta montaria já esta equipada com armadura.");
							}
							else
							{
								if (MyServerSettings.ClientVersion())
								{
									bmArmor.Body = 587;
									bmArmor.ItemID = 587;
								}
								else
								{
									bmArmor.Body = 0xE2;
									bmArmor.ItemID = 0x3EA0;
								}

								int mod = 5;

								bmArmor.SetStr(bmArmor.RawStr + mod);
								bmArmor.SetDex(bmArmor.RawDex + mod);
								bmArmor.SetInt(bmArmor.RawInt + mod);
								bmArmor.SetHits(bmArmor.HitsMax + mod);
								bmArmor.SetDamage(bmArmor.DamageMin + mod, bmArmor.DamageMax + mod);
								bmArmor.SetResistance(ResistanceType.Physical, bmArmor.PhysicalResistance + mod);

								bmArmor.SetSkill(SkillName.MagicResist, bmArmor.Skills[SkillName.MagicResist].Base + mod);
								bmArmor.SetSkill(SkillName.Tactics, bmArmor.Skills[SkillName.Tactics].Base + mod);
								bmArmor.SetSkill(SkillName.Wrestling, bmArmor.Skills[SkillName.Wrestling].Base + mod);

								pet.BardingExceptional = m_HorseArmor.Exceptional;
								pet.BardingCrafter = m_HorseArmor.Crafter;
								pet.BardingHP = pet.BardingMaxHP;
								pet.BardingResource = m_HorseArmor.Resource;
								pet.HasBarding = true;
								pet.Hue = m_HorseArmor.Hue;

								from.RevealingAction();
								from.PlaySound(0x0AA);

								m_HorseArmor.Consume();
								from.SendMessage(55, "Você coloca a armadura na sua montaria. Para remover a armadura, utilize uma lâmina na montaria.");
							}
						}
						else
						{
							from.SendMessage(55, "Esta armadura somente pode ser utilizada em cavalos de sua propriedade.");
						}
					}
				}
				else 
				{
                    from.SendMessage(55, "Apenas montarias equinas podem ser equipadas com esta armadura.");
                }
            }
        }

		public static void DropArmor( BaseCreature bc )
		{
            BaseMount bmArmor = (BaseMount)bc;
            Horse pet = bmArmor as Horse;
			
            HorseArmor armor = new HorseArmor();

            if (pet != null && pet.HasBarding)
			{
				armor.Hue = bc.Hue;
				armor.Crafter = pet.BardingCrafter;
				armor.Exceptional = pet.BardingExceptional;
				armor.Resource = pet.BardingResource;

				if (armor.Resource != null)
				{
					bc.AddItem(armor);
				}
			}
			else 
			{
                armor.Delete();
            }
		}

        public static void DropArmorInBackPack(BaseCreature bc, Mobile from)
        {
            BaseMount bmArmor = (BaseMount)bc;
            Horse pet = bmArmor as Horse;

            HorseArmor armor = new HorseArmor();

            if (pet != null && pet.HasBarding)
            {
                armor.Hue = bc.Hue;
                armor.Crafter = pet.BardingCrafter;
                armor.Exceptional = pet.BardingExceptional;
                armor.Resource = pet.BardingResource;

                if (armor.Resource != null)
                {
                    //pet.Hue = pet.OriginalHue;
                    from.AddToBackpack(armor);
                    from.SendMessage(55, "Você remove a armadura da montaria e coloca em sua mochila.");
                }
            }
            else
            {
                armor.Delete();
                from.SendMessage(55, "Esta montaria não possui armadura para ser retirada.");
            }
        }

        public HorseArmor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( ( int) 0 ); // version
            writer.Write( ArmorMaterial );

            writer.Write((bool)m_Exceptional);
            writer.Write((Mobile)m_Crafter);
            writer.Write((int)m_Resource);
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            ArmorMaterial = reader.ReadString();

            m_Exceptional = reader.ReadBool();
            m_Crafter = reader.ReadMobile();
            m_Resource = (CraftResource)reader.ReadInt();
        }
	}
}