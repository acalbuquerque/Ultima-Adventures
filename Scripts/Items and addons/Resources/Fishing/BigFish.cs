using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class BigFish : Item, ICarvable
	{
		private Mobile m_Fisher;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Fisher
		{
			get{ return m_Fisher; }
			set{ m_Fisher = value; InvalidateProperties(); }
		}

		public void Carve( Mobile from, Item item )
		{
			base.ScissorHelper( from, new RawFishSteak(), (int)Weight * 2 , false );
		}

		public override int LabelNumber{ get{ return 1041112; } } // a big fish

		[Constructable]
		public BigFish() : base( 0x09CC )
		{

            int[] list = new int[]
                {
                    0x847, 0x58C, 0x9CC, 60, 1154, 1161
				};

            Weight = Utility.RandomMinMax( 4, 24 );    //TODO: Find correct formula.  max on OSI currently 200, OSI dev says it's not 200 as max, and ~ 1/1,000,000 chance to get highest
			Hue = Utility.RandomList(list);
			Name = "Peixe Gigante";
        }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( Weight >= 20 )
			{
				if ( m_Fisher != null )
					list.Add( 1070857, m_Fisher.Name ); // Caught by ~1_fisherman~

				list.Add( 1070858, ((int)Weight).ToString() ); // ~1_weight~ stones
			}
		}

		public BigFish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (Mobile) m_Fisher );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Fisher = reader.ReadMobile();
					break;
				}
				case 0:
				{
					Weight = Utility.RandomMinMax( 4, 24 );
					break;
				}
			}
		}
	}
}