using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	public class Cotton : Item/*, IDyable*/
	{
        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ArtificialColored
        {
            get { return m_colored; }
            set { m_colored = value; InvalidateProperties(); }
        }

        [Constructable]
		public Cotton() : this( 1 )
		{
            Name = "fardo(s) de algodão";
        }

		[Constructable]
		public Cotton( int amount ) : base( 0xDF9 )
		{
			Stackable = true;
			Weight = 4.0;
			Amount = amount;
			Name = "fardo(s) de algodão";
        }

		public Cotton( Serial serial ) : base( serial )
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

            Name = "fardo(s) de algodão";
            m_colored = reader.ReadBool();
        }
/*		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

            ArtificialColored = true;

            return ArtificialColored;
        }*/

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

		public static void OnSpun( ISpinningWheel wheel, Mobile from, Item yarn, Point3D originLoc )
		{
            bool stopped = false;
            PlayerMobile pm = from as PlayerMobile;
            //pm.SendMessage(33, "Origin X: " + originLoc.X + " - Origin Y: " + originLoc.Y);

            if ( yarn != null )
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
                    Cotton bYarn = yarn as Cotton;
                    CottonSpoolOfThread item = new CottonSpoolOfThread((yarn.Amount * 6));
					item.Hue = yarn.Hue;
                    item.ArtificialColored = bYarn.ArtificialColored;

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
			private Cotton m_Cotton;

			public PickWheelTarget( Cotton cotton ) : base( 3, false, TargetFlags.None )
			{
				m_Cotton = cotton;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Cotton.Deleted )
					return;

				ISpinningWheel wheel = targeted as ISpinningWheel;

				if ( wheel == null && targeted is AddonComponent )
					wheel = ((AddonComponent)targeted).Addon as ISpinningWheel;

				if ( wheel is Item )
				{
					Item item = (Item)wheel;

					if ( !m_Cotton.IsChildOf( from.Backpack ) )
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
                        wheel.BeginSpin( new SpinCallback( Cotton.OnSpun ), from, m_Cotton );
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