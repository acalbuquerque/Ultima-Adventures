using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class Scorp : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefCarpentry.CraftSystem; } }

		[Constructable]
		public Scorp() : base( 0x10E7 )
		{
			Weight = 1.0;
            Name = "scorps tool";
        }

		[Constructable]
		public Scorp( int uses ) : base( uses, 0x10E7 )
		{
			Weight = 1.0;
            Name = "scorps tool";
        }

		public Scorp( Serial serial ) : base( serial )
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