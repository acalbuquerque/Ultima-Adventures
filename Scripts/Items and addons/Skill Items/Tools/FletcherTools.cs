using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class FletcherTools : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefBowFletching.CraftSystem; } }

		[Constructable]
		public FletcherTools() : base( 0x1F2C )
		{
            Name = "Kit de Ferrramentas";
            Weight = 5.0;
        }

		[Constructable]
		public FletcherTools( int uses ) : base( uses, 0x1F2C )
		{
            Name = "Kit de Ferrramentas";
            Weight = 5.0;
        }

		public FletcherTools( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Ferrramentas de Arcos e Flechas");
            //list.Add( 1049644, "Com este kit é possível criar todos os ítens de carpintaria.");
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
				Weight = 5.0;

			ItemID = 0x1F2C;
            Name = "Kit de Ferrramentas";
        }
	}
}