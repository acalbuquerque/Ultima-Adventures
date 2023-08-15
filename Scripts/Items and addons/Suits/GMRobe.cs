using System;
using Server;

namespace Server.Items
{
	public class GMRobe : BaseSuit
	{
		[Constructable]
		public GMRobe() : base( AccessLevel.GameMaster, 0x26, 0x204F )
		{
            Hue = 2262;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

		public GMRobe( Serial serial ) : base( serial )
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