using Server;
using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;
using Server.Misc;

namespace Server.Items
{
	public class OilWood : Item
	{
		[Constructable]
		public OilWood() : this( 1 )
		{
		}

		[Constructable]
		public OilWood( int amount ) : base( 0x1FDD )
		{
			Weight = 0.01;
			Stackable = true;
			Amount = amount;

			switch ( Utility.RandomMinMax( 0, 6 ) )
			{
				case 0:		Name = "óleo mutagênico ( Carvalho cinza )";	Hue = MaterialInfo.GetMaterialColor( "ash", "", 0 ); break;
				case 1:		Name = "óleo mutagênico ( Cerejeira )";		Hue = MaterialInfo.GetMaterialColor( "cherry", "", 0 ); break;
				case 2:		Name = "óleo mutagênico ( Ipê-amarelo )";	Hue = MaterialInfo.GetMaterialColor( "golden oak", "", 0 ); break;
				case 3:		Name = "óleo mutagênico ( Ébano )";			Hue = MaterialInfo.GetMaterialColor( "ebony", "", 0 ); break;
				case 4:		Name = "óleo mutagênico ( Nogueira Branca )"; Hue = MaterialInfo.GetMaterialColor( "hickory", "", 0 ); break;
				case 5:		Name = "óleo mutagênico ( Pau-Brasil )";		Hue = MaterialInfo.GetMaterialColor( "rosewood", "", 0 ); break;
                case 6:		Name = "óleo mutagênico ( Élfica )";			Hue = MaterialInfo.GetMaterialColor( "elven", "", 0); break;
            }
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			list.Add( 1070722, "Esfregue em armas ou armaduras de madeira");
		}

		public override void OnDoubleClick( Mobile from )
		{
			Target t;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( from.Skills[SkillName.Carpentry].Base >= 100 || from.Skills[SkillName.Fletching].Base >= 100 )
			{
				from.SendMessage(55, "Onde você deseja aplicar esse óleo?" );
				t = new OilTarget( this );
				from.Target = t;
			}
			else
			{
				from.SendMessage(55, "Somente um grande mestre carpinteiro ou grande mestre flecheiro pode usar este óleo.");
			}
		}

		private class OilTarget : Target
		{
			private OilWood m_Oil;

			public OilTarget( OilWood tube ) : base( 1, false, TargetFlags.None )
			{
				m_Oil = tube;
			}
			
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				Item iOil = targeted as Item;

				if ( from.Backpack.FindItemByType( typeof ( OilCloth ) ) == null )
				{
					from.SendMessage(55, "Você precisa de um pano para óleos para aplicar isso.");
				}
				else if ( iOil is BaseWeapon )
				{
					BaseWeapon xOil = (BaseWeapon)iOil;

					if ( !iOil.IsChildOf( from.Backpack ) )
					{
						from.SendMessage(55, "Você só pode usar este óleo nos itens da sua mochila.");
					}
					else if ( iOil.IsChildOf( from.Backpack ) && MaterialInfo.IsWoodenItem( iOil ) )
					{
						
						if ( m_Oil.Hue == MaterialInfo.GetMaterialColor("ash", "", 0) ) { xOil.Resource = CraftResource.AshTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("ebony", "", 0) ) { xOil.Resource = CraftResource.EbonyTree; }
						else if ( m_Oil.Hue == MaterialInfo.GetMaterialColor("golden oak", "", 0) ) { xOil.Resource = CraftResource.GoldenOakTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("cherry", "", 0) ) { xOil.Resource = CraftResource.CherryTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("rosewood", "", 0) ) { xOil.Resource = CraftResource.RosewoodTree; }
                        else if ( m_Oil.Hue == MaterialInfo.GetMaterialColor("hickory", "", 0) ) { xOil.Resource = CraftResource.HickoryTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("elven", "", 0) ) { xOil.Resource = CraftResource.ElvenTree; }
                        /*if ( m_Oil.Name == "oil of wood polish ( oak )" ) { xOil.Resource = CraftResource.OakTree; }*/
                        /*else if ( m_Oil.Name == "oil of wood polish ( walnut )" ) { xOil.Resource = CraftResource.WalnutTree; }*/
                        /*else if ( m_Oil.Name == "oil of wood polish ( pine )" ) { xOil.Resource = CraftResource.PineTree; }*/
                        /*else if ( m_Oil.Name == "oil of wood polish ( mahogany )" ) { xOil.Resource = CraftResource.MahoganyTree; }
						else if ( m_Oil.Name == "oil of wood polish ( driftwood )" ) { xOil.Resource = CraftResource.DriftwoodTree; }*/

                        from.RevealingAction();
						from.PlaySound( 0x23E );
						from.AddToBackpack( new Bottle() );
						m_Oil.Consume();
						from.Backpack.FindItemByType( typeof ( OilCloth ) ).Delete();
					}
					else
					{
						from.SendMessage(55, "Você não pode esfregar esse óleo nisso.");
					}
				}
				else if ( iOil is BaseArmor )
				{
					BaseArmor xOil = (BaseArmor)iOil;

					if ( !iOil.IsChildOf( from.Backpack ) )
					{
						from.SendMessage(55, "Você só pode usar este óleo nos itens da sua mochila.");
					}
					else if ( iOil.IsChildOf( from.Backpack ) && MaterialInfo.IsWoodenItem( iOil ) )
					{
                        if (m_Oil.Hue == MaterialInfo.GetMaterialColor("ash", "", 0)) { xOil.Resource = CraftResource.AshTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("ebony", "", 0)) { xOil.Resource = CraftResource.EbonyTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("golden oak", "", 0)) { xOil.Resource = CraftResource.GoldenOakTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("cherry", "", 0)) { xOil.Resource = CraftResource.CherryTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("rosewood", "", 0)) { xOil.Resource = CraftResource.RosewoodTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("hickory", "", 0)) { xOil.Resource = CraftResource.HickoryTree; }
                        else if (m_Oil.Hue == MaterialInfo.GetMaterialColor("elven", "", 0)) { xOil.Resource = CraftResource.ElvenTree; }
                        /*if ( m_Oil.Name == "oil of wood polish ( oak )" ) { xOil.Resource = CraftResource.OakTree; }*/
                        /*else if ( m_Oil.Name == "oil of wood polish ( walnut )" ) { xOil.Resource = CraftResource.WalnutTree; }*/
                        /*else if ( m_Oil.Name == "oil of wood polish ( pine )" ) { xOil.Resource = CraftResource.PineTree; }*/
                        /*else if ( m_Oil.Name == "oil of wood polish ( mahogany )" ) { xOil.Resource = CraftResource.MahoganyTree; }
						else if ( m_Oil.Name == "oil of wood polish ( driftwood )" ) { xOil.Resource = CraftResource.DriftwoodTree; }*/

                        from.RevealingAction();
						from.PlaySound( 0x23E );
						from.AddToBackpack( new Bottle() );
						m_Oil.Consume();
						from.Backpack.FindItemByType( typeof ( OilCloth ) ).Delete();
					}
					else
					{
                        from.SendMessage(55, "Você não pode esfregar esse óleo nisso.");
                    }
				}
				else
				{
                    from.SendMessage(55, "Você não pode esfregar esse óleo nisso.");
                }
			}
		}

		public OilWood( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( ( int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1FDD;
		}
	}
}