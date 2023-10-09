using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x4F52, 0x5173 )]
	public class WoodworkingTools : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefCarpentry.CraftSystem; } }

		[Constructable]
		public WoodworkingTools() : base( 0x4F52 )
		{
			Name = "Kit de Ferrramentas";
			Weight = 5.0;
		}

		[Constructable]
		public WoodworkingTools( int uses ) : base( uses, 0x4F52 )
		{
			Name = "Kit de Ferrramentas";
			Weight = 5.0;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Ferrramentas de Carpintaria");
            //list.Add( 1049644, "Com este kit é possível criar todos os ítens de carpintaria.");
        } 

		public WoodworkingTools( Serial serial ) : base( serial )
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