using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x0FBF, 0x0FC0 )]
	public class ScribesPen : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefInscription.CraftSystem; } }

		public override int LabelNumber{ get{ return 1044168; } } // scribe's pen

		public override int Hue{ get { return 0x60; } }

		[Constructable]
		public ScribesPen() : base( 0x2051 )
		{
			Name = "caneta de escriba";
			Weight = 1.0;
			Hue = 0x60;
		}

		[Constructable]
		public ScribesPen( int uses ) : base( uses, 0x0FBF )
		{
            Name = "caneta de escriba";
            Weight = 1.0;
            Hue = 0x60;
        }

		public ScribesPen( Serial serial ) : base( serial )
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

			if ( Weight == 2.0 )
				Weight = 1.0;

            Name = "caneta de escriba";
            ItemID = 0x2051;
			Hue = 0x60;
		}
	}
}