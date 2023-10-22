using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0xF95, 0xF96, 0xF97, 0xF98, 0xF99, 0xF9A, 0xF9B, 0xF9C )]
	public class BoltOfCloth : Item, IScissorable, ICommodity /*IDyable*/

    {
		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }

        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ArtificialColored
        {
            get { return m_colored; }
            set {   m_colored = value;                      
                    InvalidateProperties(); }
        }

        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }

        }
        [Constructable]
		public BoltOfCloth() : this( 1 )
		{
            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

		[Constructable]
		public BoltOfCloth( int amount ) : base( 0xF95 )
		{
			Stackable = true;
			Weight = 5.0;
			Amount = amount;

            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

		public BoltOfCloth( Serial serial ) : base( serial )
		{
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
            list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Utilize uma tesoura para transformar em tecido.", "#ffe066")); // PARENTHESIS
        }

        public override int LabelNumber
        {
            get
            {
                if (m_Resource == CraftResource.Cotton)
                    return 1101200;
                else if (m_Resource == CraftResource.Flax)
                    return 1101201;
                else if (m_Resource == CraftResource.Silk)
                    return 1101202;
                else if (m_Resource == CraftResource.Wool)
                    return 1101209;

                return 1101203;
            }
        }

        /*        public bool Dye( Mobile from, DyeTub sender )
                {
                    if ( Deleted ) return false;

                    Hue = sender.DyedHue;

                    Colored = true;

                    return Colored;
                }*/

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

		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) ) return false;

			Cloth item = new Cloth();

            if (CraftResource.Wool == m_Resource)
            {
                item = new WoolCloth();
            }
            else if (CraftResource.Cotton == m_Resource)
            {
                item = new CottonCloth();
            }
            else if (CraftResource.Flax == m_Resource)
            {
                item = new FlaxCloth();
            }
            else if (CraftResource.Silk == m_Resource)
            {
                item = new SilkCloth();
            }

            item.ArtificialColored = m_colored;
            item.Resource = m_Resource;

            base.ScissorHelper( from, item, 15 );

			return true;
		}

		public override void OnSingleClick( Mobile from )
		{
			int number = (Amount == 1) ? 1049122 : 1049121;

			from.Send( new MessageLocalized( Serial, ItemID, MessageType.Label, 0x3B2, 3, number, "", (Amount * 15).ToString() ) );
		}
	}
    //
    public class WoolBoltOfCloth : BoltOfCloth

    {
        [Constructable]
        public WoolBoltOfCloth() : this(1)
        {
            Resource = CraftResource.Wool;
        }

        [Constructable]
        public WoolBoltOfCloth(int amount) : base(amount)
        {
            Resource = CraftResource.Wool;
        }

        public WoolBoltOfCloth(Serial serial) : base(serial)
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
    /*public class FlaxBoltOfCloth : BoltOfCloth

    {
        [Constructable]
        public FlaxBoltOfCloth() : this(1)
        {
        }

        [Constructable]
        public FlaxBoltOfCloth(int amount) : base(amount)
        {
        }

        public FlaxBoltOfCloth(Serial serial) : base(serial)
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
    public class SilkkBoltOfCloth : BoltOfCloth

    {
        [Constructable]
        public SilkkBoltOfCloth() : this(1)
        {
        }

        [Constructable]
        public SilkkBoltOfCloth(int amount) : base(amount)
        {
        }

        public SilkkBoltOfCloth(Serial serial) : base(serial)
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
    }*/
}