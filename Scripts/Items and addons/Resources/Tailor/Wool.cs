using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
namespace Server.Items
{
	public class Wool : Item, IDyable
	{
        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ArtificialColored
        {
            get { return m_colored; }
            set { m_colored = value; InvalidateProperties(); }
        }

        [Constructable]
		public Wool() : this( 1 )
		{
            Name = "fardo(s) de lã fina";
            Hue = 946;
        }

		[Constructable]
		public Wool( int amount ) : base( 0xDF8 )
		{
			Stackable = true;
			Weight = 3.0;
			Amount = amount;
            Hue = 946;
            Name = "fardo(s) de lã fina";
        }

		public Wool( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

			if(m_colored)
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
            m_colored = reader.ReadBool();
            Name = "fardo(s) de lã fina";
        }
		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

            ArtificialColored = true;

            return ArtificialColored;
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 502655 ); // What spinning wheel do you wish to spin this on?
				from.Target = new PickWheelTarget( this );
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
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
                    Wool bYarn = yarn as Wool;
                    BaseClothMaterial item = new LightYarn(yarn.Amount * 4);
                    item.Hue = yarn.Hue;
                    item.ArtificialColored = bYarn.ArtificialColored;
                    item.Resource = CraftResource.Wool;
                    yarn.Delete();

                    from.AddToBackpack(item);
                    from.SendMessage(55, "Você coloca a(s) bola(s) de lã na mochila.");
                    //from.SendLocalizedMessage(1010574); // You put a ball of yarn in your backpack.
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
			private Wool m_Wool;

			public PickWheelTarget( Wool wool ) : base( 3, false, TargetFlags.None )
			{
				m_Wool = wool;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Wool.Deleted )
					return;

				ISpinningWheel wheel = targeted as ISpinningWheel;

				if ( wheel == null && targeted is AddonComponent )
					wheel = ((AddonComponent)targeted).Addon as ISpinningWheel;

				if ( wheel is Item )
				{
					Item item = (Item)wheel;

					if ( !m_Wool.IsChildOf( from.Backpack ) )
					{
						from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
					}
					else if ( wheel.Spinning )
					{
						from.SendLocalizedMessage( 502656 ); // That spinning wheel is being used.
					}
					else
					{
						if ( m_Wool is TaintedWool )	wheel.BeginSpin( new SpinCallback( TaintedWool.OnSpun ), from, m_Wool );
						else wheel.BeginSpin( new SpinCallback( Wool.OnSpun ), from, m_Wool );
					}
				}
				else
				{
					from.SendLocalizedMessage( 502658 ); // Use that on a spinning wheel.
				}
			}
		}
	}
	public class TaintedWool : Wool
	{

        [Constructable]
		public TaintedWool() : this( 1 )
		{
            Name = "fardo(s) de lã grossa";
        }
		
		[Constructable]
		public TaintedWool( int amount ) : base( 0x101F )
		{
			Stackable = true;
			Weight = 4.0;
			Amount = amount;
			Hue = 914;
            Name = "fardo(s) de lã grossa";
        }

		public TaintedWool( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

            //Wool t = base as Wool;
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

            //ToolbarHelper.Infos = this;

            int version = reader.ReadInt();

            Name = "fardo(s) de lã grossa";
        }
		
		new public static void OnSpun( ISpinningWheel wheel, Mobile from, Item yarn, Point3D originLoc)
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
					Wool bYarn = yarn as Wool;

                    BaseClothMaterial item = new DarkYarn(yarn.Amount * 3);
                    item.Hue = yarn.Hue;
					item.ArtificialColored = bYarn.ArtificialColored;
                    item.Resource = CraftResource.Wool;
                    yarn.Delete();

                    from.AddToBackpack(item);
                    from.SendMessage(55, "Você coloca a(s) bola(s) de lã na mochila.");
                    //from.SendLocalizedMessage(1010574); // You put a ball of yarn in your backpack.
                }
                else
                {
                    yarn.Amount -= ((yarn.Amount * 0.1) < 1) ? 1 : (int)(yarn.Amount * 0.1);
                    from.SendMessage(33, "Você perdeu uma pequena quantidade de material quando falhou na transformação.");
                }
            }
        }
	}
}