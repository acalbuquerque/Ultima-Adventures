using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[Flipable( 0x1030, 0x1031 )]
	public class JointingPlane : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefCarpentry.CraftSystem; } }

		[Constructable]
		public JointingPlane() : base( 0x1030 )
		{
			Weight = 2.0;
            Name = "plaina";
        }

		[Constructable]
		public JointingPlane( int uses ) : base( uses, 0x1030 )
		{
			Weight = 2.0;
            Name = "plaina";
        }

		public JointingPlane( Serial serial ) : base( serial )
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

			if ( Weight == 1.0 )
				Weight = 2.0;
		}
	}
}