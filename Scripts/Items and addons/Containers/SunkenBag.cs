using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Misc;

namespace Server.Items
{
	public class SunkenBag : LockableContainer
	{
		[Constructable]
		public SunkenBag() : this( 0 )
		{
		}

		[Constructable]
		public SunkenBag( int level ) : base( 0xe40 )
		{
			if ( level < 1 ){ level = Utility.RandomMinMax( 1, 4 ); }
			Movable = true;
			Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );

			ItemID = 0xE76;
			GumpID = 0x3D;
			switch ( Utility.RandomMinMax( 0, 2 ) )
			{
				case 0: Name = "bolsa"; break;
				case 1:	Name = "saco"; break;
				case 2:	Name = "pochete"; break;
			}

			if ( Utility.Random( 2 ) == 1 )
			{
				ItemID = Utility.RandomList( 0xE75, 0x53D5 );
				GumpID = 0x3C;
				switch ( Utility.RandomMinMax( 0, 2 ) )
				{
					case 0: Name = "pacote"; break;
					case 1:	Name = "mochila"; break;
					case 2:	Name = "sacola"; break;
				}
			}

			string sAdjective = "molhado(a)";

			switch ( Utility.RandomMinMax( 0, 4 ) )
			{
				case 0: sAdjective = "encharcado(a)"; break;
				case 1:	sAdjective = "molhado(a)"; break;
				case 2:	sAdjective = "ensopado(a)"; break;
				case 3:	sAdjective = "úmido(a)"; break;
				case 4:	sAdjective = "alagado(a)"; break;
			}

			string sSack = ContainerFunctions.GetOwner( "SunkenBag" );

			Name = Name + " " + sAdjective + " de " + sSack;

			TrapType = TrapType.None;
			TrapPower = 0;
			TrapLevel = 0;
			Locked = false;
            LockLevel = 0;
			MaxLockLevel = 0;
			RequiredSkill = 0;
			Weight = 11.0 + (double)level;

			if ( Weight > 10 ){ Movable = false; } // DON'T WANT THEM TO MOVE IT UNTIL THEY OPEN IT FIRST
		}

		public override void Open( Mobile from )
		{
			if ( this.Weight > 10 )
			{
				Movable = true;
				int FillMeUpLevel = (int)(this.Weight - 11);
				this.Weight = 2.0;

				if ( GetPlayerInfo.LuckyPlayer( from.Luck, from ) )
				{
					FillMeUpLevel = FillMeUpLevel + Utility.RandomMinMax( 1, 2 );
				}

				ContainerFunctions.FillTheContainer( FillMeUpLevel, this, from );
			}

			base.Open( from );
		}

		public SunkenBag( Serial serial ) : base( serial )
		{
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
	}
}