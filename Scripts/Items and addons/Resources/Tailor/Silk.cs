using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
namespace Server.Items
{
    public class Silk : BaseClothMaterial
    {
        [Constructable]
        public Silk() : this(1)
        {
            Name = "bola(s) de seda";
            Hue = 2173;
            Resource = CraftResource.Silk;
        }

        [Constructable]
        public Silk(int amount) : base(0xE1F, amount)
        {
            Name = "bola(s) de seda";
            Hue = 2173;
            Resource = CraftResource.Silk;
            Weight = 2.0;
        }

        public Silk(Serial serial) : base(serial)
        {
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

            Name = "bola(s) de seda";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendMessage(55, "Você precisa de uma roda de fiar para transformar isso.");
                //from.SendLocalizedMessage( 502655 ); // What spinning wheel do you wish to spin this on?
                from.Target = new PickWheelTarget(this);
            }
            else
            {
                from.SendMessage(55, "Isso precisa estar na sua mochila.");
                //from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
            }
        }

        public static void OnSpun(ISpinningWheel wheel, Mobile from, Item yarn, Point3D originLoc)
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
                    SilkSpoolOfThread item = new SilkSpoolOfThread((yarn.Amount * 2));
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
            private Silk m_LightYarnUnraveled;

            public PickWheelTarget(Silk silk) : base(3, false, TargetFlags.None)
            {
                m_LightYarnUnraveled = silk;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_LightYarnUnraveled.Deleted)
                    return;

                ISpinningWheel wheel = targeted as ISpinningWheel;

                if (wheel == null && targeted is AddonComponent)
                    wheel = ((AddonComponent)targeted).Addon as ISpinningWheel;

                if (wheel is Item)
                {
                    Item item = (Item)wheel;

                    if (!m_LightYarnUnraveled.IsChildOf(from.Backpack))
                    {
                        from.SendMessage(55, "Isso precisa estar na sua mochila.");
                        //from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
                    }
                    else if (wheel.Spinning)
                    {
                        from.SendMessage(55, "Essa roda de fiar já está em uso. Aguarde acabar.");
                        //from.SendLocalizedMessage( 502656 ); // That spinning wheel is being used.
                    }
                    else
                    {
                        wheel.BeginSpin(new SpinCallback(Silk.OnSpun), from, m_LightYarnUnraveled);
                    }
                }
                else
                {
                    from.SendMessage(55, "Você precisa de uma roda de fiar para transformar isso.");
                    //from.SendLocalizedMessage( 502658 ); // Use that on a spinning wheel.
                }
            }
        }
    }
}

    