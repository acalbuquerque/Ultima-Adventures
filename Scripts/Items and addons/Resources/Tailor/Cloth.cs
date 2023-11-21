using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x1766, 0x1768 )]
	public class Cloth : Item, IScissorable, ICommodity, IDyable

    {
		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }

        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ArtificialColored
        {
            get { return m_colored; }
            set { m_colored = value; InvalidateProperties(); }
        }

        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }

        }

        public override double DefaultWeight
		{
			get { return 0.5; }
		}


		[Constructable]
		public Cloth() : this( 1 )
		{
            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

		[Constructable]
		public Cloth( int amount ) : base( 0x1766 )
		{
			Stackable = true;
			Amount = amount;

            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

		public Cloth( Serial serial ) : base( serial )
		{
            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_Resource != CraftResource.None)
            {
                string resourceName = CraftResources.GetName(m_Resource);
                if (string.IsNullOrEmpty(resourceName) || resourceName.ToLower() == "none" || resourceName.ToLower() == "normal")
                {
                    resourceName = "";
                }

                if (resourceName != "")
                {
                    list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor(resourceName, "#8be4fc"));
                }
            }

            if (m_colored)
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Colorido Artificalmente", "#8be4fc"));

            if (m_Resource != CraftResource.Cotton)
                list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Utilize uma tesoura para transformar em bandagens.", "#ffe066")); // PARENTHESIS
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            if (m_Resource == CraftResource.Cotton || m_Resource == CraftResource.Poliester)
            {
                Hue = sender.DyedHue;
                ArtificialColored = true;
                return ArtificialColored;
            }
            else 
            {
                from.SendMessage(55, "Você não pode pintar esse tipo de tecido.");
                return false;
            }

        }

        public override int LabelNumber
        {
            get
            {
                if (m_Resource == CraftResource.Cotton)
                    return 1101204;
                else if (m_Resource == CraftResource.Flax)
                    return 1101205;
                else if (m_Resource == CraftResource.Silk)
                    return 1101206;
                else if (m_Resource == CraftResource.Wool)
                    return 1101208;

                return 1101207;
            }
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

            writer.Write(m_colored);

            writer.Write((int)m_Resource);
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            Name = "tecido(s) cortado(s)";

            switch (version)
            {
                case 1:
                    {
                        m_colored = reader.ReadBool();
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }

            if (version == 0)
                m_Resource = CraftResource.Cotton;
        }

		public override void OnSingleClick( Mobile from )
		{
			int number = (Amount == 1) ? 1049124 : 1049123;

			from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", Amount.ToString() ) );
		}
		
		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) ) return false;

            if (CraftResource.Cotton == m_Resource)
            {
                base.ScissorHelper(from, new Bandage(), 2);
                from.SendMessage(55, "Você corta o tecido e transforma-o em bandagens.");
                return true;
            }
            else 
            {
                from.SendMessage(55, "Apenas tecido do tipo algodão pode ser transformado em bandagens!");
                return false;
            }
		}
	}

    public class CottonCloth : Cloth

    {
        [Constructable]
        public CottonCloth() : this(1)
        {
            Resource = CraftResource.Cotton;
        }

        [Constructable]
        public CottonCloth(int amount) : base(amount)
        {
            Resource = CraftResource.Cotton;
        }

        public CottonCloth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    public class FlaxCloth : Cloth

    {
        [Constructable]
        public FlaxCloth() : this(1)
        {
            Resource = CraftResource.Flax;
            Hue = 1382;
        }

        [Constructable]
        public FlaxCloth(int amount) : base(amount)
        {
            Resource = CraftResource.Flax;
            Hue = 1382;
        }

        public FlaxCloth(Serial serial) : base(serial)
        {
            Hue = 1382;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hue = 1382;
        }
    }
    public class SilkCloth : Cloth

    {
        [Constructable]
        public SilkCloth() : this(1)
        {
            Resource = CraftResource.Silk;
            Hue = 2173;
            ItemID = 0x1767;
        }

        [Constructable]
        public SilkCloth(int amount) : base(amount)
        {
            Resource = CraftResource.Silk;
            Hue = 2173;
            ItemID = 0x1767;
        }

        public SilkCloth(Serial serial) : base(serial)
        {
            Hue = 2173;
            ItemID = 0x1767;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hue = 2173;
            ItemID = 0x1767;
        }
    }
    public class PoliesterCloth : Cloth

    {
        [Constructable]
        public PoliesterCloth() : this(1)
        {
            Resource = CraftResource.Poliester;

            ItemID = 0x1767;
        }

        [Constructable]
        public PoliesterCloth(int amount) : base(amount)
        {
            Resource = CraftResource.Poliester;

            ItemID = 0x1767;
        }

        public PoliesterCloth(Serial serial) : base(serial)
        {
            ItemID = 0x1767;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            ItemID = 0x1767;
        }
    }
    public class WoolCloth : Cloth

    {
        [Constructable]
        public WoolCloth() : this(1)
        {
            Resource = CraftResource.Wool;
        }

        [Constructable]
        public WoolCloth(int amount) : base(amount)
        {
            Resource = CraftResource.Wool;
        }

        public WoolCloth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}