using System;
using Server;
using Server.Misc;

namespace Server.Items
{
	public class HighSeasRelic : Item
	{
		public int RelicGoldValue;
		public int RelicFlipID1;
		public int RelicFlipID2;
		public string RelicOrigin;

		[CommandProperty(AccessLevel.Owner)]
		public int Relic_Value { get { return RelicGoldValue; } set { RelicGoldValue = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Relic_FlipID1 { get { return RelicFlipID1; } set { RelicFlipID1 = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Relic_FlipID2 { get { return RelicFlipID2; } set { RelicFlipID2 = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Relic_Origin { get { return RelicOrigin; } set { RelicOrigin = value; InvalidateProperties(); } }

		[Constructable]
		public HighSeasRelic() : base( 0x41FD )
		{
			Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );

			switch ( Utility.RandomMinMax( 0, 20 ) )
			{
				case 0:	Name = "�ncora";				ItemID = 0x897;		Weight = 100.0;		RelicGoldValue = Utility.RandomMinMax( 80, 200 );
					RelicFlipID1 = 0x897;	RelicFlipID2 = 0x898;	Server.Misc.MaterialInfo.ColorPlainMetal( this );	break;
				case 1:	Name = "barril arruinado";			ItemID = 0x1EB5;	Weight = 20.0;		RelicGoldValue = Utility.RandomMinMax( 20, 80 );
					RelicFlipID1 = 0x1EB5;	RelicFlipID2 = 0x1EB5;	break;
				case 2:	Name = "bala de canh�o";		ItemID = 0x0E73;	Weight = 10.0;		RelicGoldValue = Utility.RandomMinMax( 10, 20 );
					RelicFlipID1 = 0x0E73;	RelicFlipID2 = 0x0E73;	break;
				case 3:	Name = "balas de canh�o";		ItemID = 0x0E74;	Weight = 50.0;		RelicGoldValue = Utility.RandomMinMax( 20, 40 );
					RelicFlipID1 = 0x0E74;	RelicFlipID2 = 0x0E74;	break;
				case 4:	Name = "rel�gio quebrado";			ItemID = 0x0C1F;	Weight = 2.0;		RelicGoldValue = Utility.RandomMinMax( 20, 40 );
					RelicFlipID1 = 0x0C1F;	RelicFlipID2 = 0x0C1F;	break;
				case 5:	Name = "miniatura de navio";			ItemID = 0x14F3;	Weight = 5.0;		RelicGoldValue = Utility.RandomMinMax( 60, 130 );
					RelicFlipID1 = 0x14F3;	RelicFlipID2 = 0x14F4;	break;
				case 6:	Name = "lagosta empalhada";		ItemID = 0x46BC;	Weight = 2.0;		RelicGoldValue = Utility.RandomMinMax( 40, 80 );
					RelicFlipID1 = 0x46BC;	RelicFlipID2 = 0x46BD;	break;
				case 7:	Name = "carangueijo empalhado";			ItemID = 0x46BA;	Weight = 2.0;		RelicGoldValue = Utility.RandomMinMax( 40, 80 );
					RelicFlipID1 = 0x46BA;	RelicFlipID2 = 0x46BB;	break;
				case 8:	Name = "luneta arruinada";		ItemID = 0x14F5;	Weight = 1.0;		RelicGoldValue = Utility.RandomMinMax( 20, 40 );
					RelicFlipID1 = 0x14F5;	RelicFlipID2 = 0x14F6;	break;
				case 9:	Name = "corda encharcada";			ItemID = 0x14F8;	Weight = 10.0;		RelicGoldValue = Utility.RandomMinMax( 10, 30 );
					RelicFlipID1 = 0x14F8;	RelicFlipID2 = 0x14FA;	break;
				case 10: Name = "chicote de flagela��o usado";	ItemID = 0x166E;	Weight = 2.0;		RelicGoldValue = Utility.RandomMinMax( 10, 30 );
					RelicFlipID1 = 0x166E;	RelicFlipID2 = 0x166E;	break;
				case 11: Name = "ampulheta rachada";	ItemID = 0x1810;	Weight = 2.0;		RelicGoldValue = Utility.RandomMinMax( 20, 40 );
					RelicFlipID1 = 0x1810;	RelicFlipID2 = 0x1813;	break;
				case 12: Name = "barril de rum";			ItemID = 0x1AD6;	Weight = 20.0;		RelicGoldValue = Utility.RandomMinMax( 50, 120 );
					RelicFlipID1 = 0x1AD6;	RelicFlipID2 = 0x1AD7;	Hue = 0x96D;	break;
				case 13: Name = "remos empenados";			ItemID = 0x1E2A;	Weight = 10.0;		RelicGoldValue = Utility.RandomMinMax( 40, 100 );
					RelicFlipID1 = 0x1E2A;	RelicFlipID2 = 0x1E2B;	break;
				case 14: Name = "rede de pesca arruinada"; 	ItemID = 0x1EA3;	Weight = 10.0;		RelicGoldValue = Utility.RandomMinMax( 30, 90 );
					RelicFlipID1 = 0x1EA3;	RelicFlipID2 = 0x1EA4;	break;
				case 15: Name = "rede de pesca podre"; 	ItemID = 0x1EA5;	Weight = 10.0;		RelicGoldValue = Utility.RandomMinMax( 30, 90 );
					RelicFlipID1 = 0x1EA5;	RelicFlipID2 = 0x1EA6;	break;
				case 16: Name = "garrafa de rum";		ItemID = 0xE26;		Weight = 1.0;		RelicGoldValue = Utility.RandomMinMax( 10, 30 );
					RelicFlipID1 = 0xE26;	RelicFlipID2 = 0xEFE;	break;
				case 17: Name = "globo quebrado";			ItemID = 0x1047;	Weight = 1.0;		RelicGoldValue = Utility.RandomMinMax( 20, 40 );
					RelicFlipID1 = 0x1047;	RelicFlipID2 = 0x1048;	break;
				case 18: Name = "sextante quebrado";		ItemID = 0x1057;	Weight = 1.0;		RelicGoldValue = Utility.RandomMinMax( 10, 30 );
					RelicFlipID1 = 0x1057;	RelicFlipID2 = 0x1058;	break;
				case 19: Name = "chap�u de pirata rasgado";	ItemID = 0x171B;	Weight = 1.0;		RelicGoldValue = Utility.RandomMinMax( 10, 30 );
					RelicFlipID1 = 0x171B;	RelicFlipID2 = 0x171B;	break;
				case 20: Name = "o di�rio do capit�o";
							Hue = RandomThings.GetRandomColor(0);
							ItemID = Utility.RandomList( 0xFBD, 0xFBE, 0xFEF, 0xFF0, 0xFF1, 0xFF2, 0x42BF, 0xE3B, 0xEFA, 0x2253, 0x2254, 0x42BF );
							Weight = 1.0;
							RelicGoldValue = Utility.RandomMinMax( 50, 130 );
							RelicFlipID1 = ItemID;	RelicFlipID2 = ItemID;	break;
			}


			string boat = RandomThings.GetRandomShipName( "", 0 );
			RelicOrigin = "Resgatado do Naufr�gio: [ " + boat + " ]";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, ItemNameHue.UnifiedItemProps.SetColor(RelicOrigin, "#8be4fc"));
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage( 55,"Identifique o item para descobrir o valor." );
				from.SendMessage(55, "Isso deve estar na sua mochila para virar.");
			}
			else
			{
				if ( this.ItemID == RelicFlipID1 ){ this.ItemID = RelicFlipID2; } else { this.ItemID = RelicFlipID1; }
			}
		}

		public HighSeasRelic(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
            writer.Write( (int) 0 ); // version
            writer.Write( RelicGoldValue );
            writer.Write( RelicFlipID1 );
            writer.Write( RelicFlipID2 );
            writer.Write( RelicOrigin );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
            int version = reader.ReadInt();
            RelicGoldValue = reader.ReadInt();
            RelicFlipID1 = reader.ReadInt();
            RelicFlipID2 = reader.ReadInt();
			RelicOrigin = reader.ReadString();
		}
	}
}