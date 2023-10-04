using System;

namespace Server.Items
{
	[Furniture]
	public class ElegantLowTable : Item
	{
		[Constructable]
		public ElegantLowTable() : base(0x2819)
		{
            Name = "Mesa de Centro Elegante";
            Weight = 5.0;
        }

		public ElegantLowTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}
	}

	[Furniture]
	public class PlainLowTable : Item
	{
		[Constructable]
		public PlainLowTable() : base(0x281A)
		{
            Name = "Mesa de Centro Simples";
            Weight = 5.0;
        }

		public PlainLowTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}
	}

	[Furniture]
	[Flipable(0xB90,0xB7D)]
	public class LargeTable : Item
	{
		[Constructable]
		public LargeTable() : base(0xB90)
		{
            Name = "mesa grande";
            Weight = 5.0;
        }

		public LargeTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB35,0xB34)]
	public class Nightstand : Item
	{
		[Constructable]
		public Nightstand() : base(0xB35)
		{
            Name = "mesa de cabeceira";
            Weight = 3.0;
        }

		public Nightstand(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB8F,0xB7C)]
	public class YewWoodTable : Item
	{
		[Constructable]
		public YewWoodTable() : base(0xB8F)
		{
            Name = "mesa simples";
            Weight = 5.0;
        }

		public YewWoodTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 4.0 )
				Weight = 5.0;
		}
	}

    [Furniture]
    //[Flipable(0xB40)]
    public class PlainLargeTable : Item
    {
        [Constructable]
        public PlainLargeTable() : base(0xB40)
        {
            Name = "mesa larga";
            Weight = 5.0;
        }

        public PlainLargeTable(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Weight == 4.0)
                Weight = 5.0;
        }
    }
}