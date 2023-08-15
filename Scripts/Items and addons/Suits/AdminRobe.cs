using System;
using Server;

namespace Server.Items
{
	public class AdminRobe : BaseSuit
	{
		[Constructable]
        public AdminRobe(Mobile from) : base(AccessLevel.Administrator, 0x0, 0x204F) // Blank hue
        {
            Hue = 2259;
            Weight = 1.0;
            LootType = LootType.Blessed;
            changeColor(from);
        }

		public AdminRobe( Serial serial ) : base( serial )
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

        private void changeColor(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Administrator)
            {
                Hue = 2253;
            }
            else
            {
                Hue = 2232;
            }
        }
    }
}