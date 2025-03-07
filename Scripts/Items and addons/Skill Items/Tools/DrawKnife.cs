using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class DrawKnife : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefCarpentry.CraftSystem; } }

		[Constructable]
		public DrawKnife() : base( 0x10E4 )
		{
            Name = "faca de tanoeiro";
            Weight = 2.0;
		}

		[Constructable]
		public DrawKnife( int uses ) : base( uses, 0x10E4 )
		{
            Name = "faca de tanoeiro";
            Weight = 2.0;
		}

		public DrawKnife( Serial serial ) : base( serial )
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