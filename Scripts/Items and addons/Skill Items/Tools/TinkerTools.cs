using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[Flipable( 0x1EB8, 0x1EB9 )]
	public class TinkerTools : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefTinkering.CraftSystem; } }

		[Constructable]
		public TinkerTools() : base( 0x1EB8 )
		{
            Name = "Kit de Ferrramentas";
            Weight = 5.0;
        }

		[Constructable]
		public TinkerTools( int uses ) : base( uses, 0x1EB8 )
		{
            Name = "Kit de Ferrramentas";
            Weight = 5.0;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Ferrramentas de Inventor");
            //list.Add( 1049644, "Com este kit � poss�vel criar todos os �tens de carpintaria.");
        }

        public TinkerTools( Serial serial ) : base( serial )
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

	public class TinkersTools : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefTinkering.CraftSystem; } }

		[Constructable]
		public TinkersTools()
			: base(0x1EBC)
		{
            Name = "Kit de Ferrramentas";
            Weight = 5.0;
        }

		[Constructable]
		public TinkersTools(int uses)
			: base(uses, 0x1EBC)
		{
            Name = "Kit de Ferrramentas";
            Weight = 5.0;
        }

		public TinkersTools(Serial serial)
			: base(serial)
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Ferrramentas de Inventor");
            //list.Add( 1049644, "Com este kit � poss�vel criar todos os �tens de carpintaria.");
        }

        public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}