using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;

namespace Server.Mobiles
{
    public class SpammerMuseumGuide : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public virtual bool IsInvulnerable { get { return true; } }
        public override NpcGuild NpcGuild { get { return NpcGuild.MinersGuild; } }

        [Constructable]
        public SpammerMuseumGuide() : base("[ Guia ]")
        {
            Job = JobFragment.scholar;
            Sophistication = Sophistication.High;
            Attitude = Attitude.Kindly;

            SpeechHue = Server.Misc.RandomThings.GetSpeechHue();

            //SpeechHue = Utility.RandomDyedHue();
            Blessed = true;
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new NMSSpammerMuseumGuide());
        }


        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (InRange(m, 4) && !InRange(oldLocation, 4))
            {
                if (m is PlayerMobile && !m.Hidden)
                {
                    switch (Utility.Random(4))
                    {
                        case 0: Yell("* BEMVINDO AO MUSEU * Informações aqui!"); break;
                        case 1: Yell("Para mais informações, fale comigo!!  Minérios, lingotes... "); break;
                        case 2: Yell("Deseja alguma informação, " + m.Name + "? Também compro e vendo minérios!"); break;
                        case 3: Yell("COMPRO E VENDO!!! Deseja algo?!"); break;
                    }

                }
            }

        }

        public override VendorShoeType ShoeType
        {
            get { return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new Server.Items.Robe(Utility.RandomYellowHue()));
            AddItem(new Server.Items.Bonnet(Utility.RandomYellowHue()));
        }

        public SpammerMuseumGuide(Serial serial) : base(serial)
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
        }

        ///////////////////////////////////////////////////////////////////////////
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new SpeechGumpEntry(from, this));
        }

        public class SpeechGumpEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public SpeechGumpEntry(Mobile from, Mobile giver) : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(SpeechGump)))
                    {
                        mobile.SendGump(new SpeechGump("* AVISO IMPORTANTE *", SpeechFunctions.SpeechText(m_Giver.Name, m_Mobile.Name, "MuseumGuide")));

                    }
                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////
    }
}