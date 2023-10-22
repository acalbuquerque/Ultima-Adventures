using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
namespace Server.Items
{
	public class Flax : Item
	{
        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Colored
        {
            get { return m_colored; }
            set { m_colored = value; InvalidateProperties(); }
        }

        [Constructable]
		public Flax() : this( 1 )
		{
            Name = "chumaço(s) de linho";
            Hue = 1382;
        }

		[Constructable]
		public Flax( int amount ) : base( 0x1A9C )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
            Name = "chumaço(s) de linho";
            Hue = 1382;
        }

		public Flax( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (m_colored)
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Colorido Artificalmente", "#8be4fc"));
            list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Utilize uma roda de fiar para transformar.", "#ffe066")); // PARENTHESIS
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
            Name = "chumaço(s) de linho";
            m_colored = reader.ReadBool();
        }
		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
                from.SendMessage(55, "Você precisa de uma roda de fiar para transformar isso.");
                //from.SendLocalizedMessage( 502655 ); // What spinning wheel do you wish to spin this on?
				from.Target = new PickWheelTarget( this );
			}
			else
			{
                from.SendMessage(55, "Isso precisa estar na sua mochila.");
                //from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public static void OnSpun( ISpinningWheel wheel, Mobile from, Item yarn, Point3D originLoc)
		{
            bool stopped = false;
            PlayerMobile pm = from as PlayerMobile;
            //pm.SendMessage(33, "Origin X:" + originLoc.X + "Origin Y: " + originLoc.Y);

            if (yarn != null)
            {

                for (int i = 0; i < yarn.Amount; i++)
                {
                    Point3D atualLoc = pm.Location; //check if player moved, if so, stop
                    //pm.SendMessage(35, "atual X: " + atualLoc.X + " - atual Y: " + atualLoc.Y);

                    if (originLoc.X != atualLoc.X || originLoc.Y != atualLoc.Y)
                    {
                        stopped = true;
                        pm.SendMessage(55, "Você se moveu e parou de transformar o(s) item(s).");

                        break;
                    }
                }

                if (!stopped)
                {
                    FlaxSpoolOfThread item = new FlaxSpoolOfThread((yarn.Amount * 4));
                    item.Hue = yarn.Hue;
                    yarn.Delete();

                    from.AddToBackpack(item);
                    from.SendMessage(55, "Você coloca os carretéis de linha na mochila.");
                    //from.SendLocalizedMessage( 1010577 ); // You put the spools of thread in your backpack.
                }
                else
                {
                    yarn.Amount -= ((yarn.Amount * 0.1) < 1) ? 1 : (int)(yarn.Amount * 0.1);
                    from.SendMessage(33, "Você perdeu uma pequena quantidade de material quando falhou na transformação.");
                }

            }
        }

		private class PickWheelTarget : Target
		{
			private Flax m_Flax;

			public PickWheelTarget( Flax flax ) : base( 3, false, TargetFlags.None )
			{
				m_Flax = flax;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Flax.Deleted )
					return;

				ISpinningWheel wheel = targeted as ISpinningWheel;

				if ( wheel == null && targeted is AddonComponent )
					wheel = ((AddonComponent)targeted).Addon as ISpinningWheel;

				if ( wheel is Item )
				{
					Item item = (Item)wheel;

					if ( !m_Flax.IsChildOf( from.Backpack ) )
					{
                        from.SendMessage(55, "Isso precisa estar na sua mochila.");
                        //from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
					}
					else if ( wheel.Spinning )
					{
                        from.SendMessage(55, "Essa roda de tecer já está em uso. Aguarde acabar.");
                        //from.SendLocalizedMessage( 502656 ); // That spinning wheel is being used.
					}
					else
					{
						wheel.BeginSpin( new SpinCallback( Flax.OnSpun ), from, m_Flax );
					}
				}
				else
				{
                    from.SendMessage(55, "Você precisa de uma roda de tecer para transformar isso.");
                    //from.SendLocalizedMessage( 502658 ); // Use that on a spinning wheel.
				}
			}
		}
	}
}