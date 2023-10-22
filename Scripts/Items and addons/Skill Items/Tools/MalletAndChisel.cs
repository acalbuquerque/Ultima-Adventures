using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class MalletAndChisel : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefMasonry.CraftSystem; } }

		[Constructable]
		public MalletAndChisel() : base( 0x12B3 )
		{
			Weight = 2.0;
            Name = "martelo e cinzel";
        }

		[Constructable]
		public MalletAndChisel( int uses ) : base( uses, 0x12B3 )
		{
            Weight = 2.0;
            Name = "martelo e cinzel";
        }

		public MalletAndChisel( Serial serial ) : base( serial )
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