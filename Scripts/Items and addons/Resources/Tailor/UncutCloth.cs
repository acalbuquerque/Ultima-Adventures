using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x1765, 0x1767 )]
	public class UncutCloth : Item, IScissorable, IDyable, ICommodity
	{
		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }

		public override double DefaultWeight
		{
			get { return 0.3; }
		}

        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ArtificialColored
        {
            get { return m_colored; }
            set { m_colored = value; InvalidateProperties(); }
        }

        [Constructable]
		public UncutCloth() : this( 1 )
		{
            //Name = "tecido(s) de poliéster";
        }

		[Constructable]
		public UncutCloth( int amount ) : base( 0x1767 )
		{
			Stackable = true;
			Amount = amount;
            //Name = "tecido(s) de poliéster";
        }

		public UncutCloth( Serial serial ) : base( serial )
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

            Hue = sender.DyedHue;
            ArtificialColored = true;
            return ArtificialColored;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

            writer.Write(m_colored);
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            m_colored = reader.ReadBool();

            //Name = "tecido(s) de poliéster";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            //list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor("Poliéster", "#8be4fc"));

            if (m_colored)
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Colorido Artificalmente", "#8be4fc"));

            list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Utilize uma tesoura para transformar em bandagens.", "#ffe066")); // PARENTHESIS
        }

        public override void OnSingleClick( Mobile from )
		{
			int number = (Amount == 1) ? 1049124 : 1049123;

			from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", Amount.ToString() ) );
		}
		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) ) return false;

			base.ScissorHelper( from, new Bandage(), 1 );

			return true;
		}
	}
}