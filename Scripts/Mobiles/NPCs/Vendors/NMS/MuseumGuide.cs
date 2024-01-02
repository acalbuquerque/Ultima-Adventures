using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Misc;
using Server.Mobiles;
using System.Collections;
using Server.Gumps;
using System.Text;
namespace Server.Mobiles
{
    public class MuseumGuide : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.LibrariansGuild; } }

        [Constructable]
        public MuseumGuide() : base("O guia do Museu")
        {
            Job = JobFragment.scholar;
            Karma = Utility.RandomMinMax(15, -45);
            SetSkill(SkillName.Inscribe, 50.0, 85.0);
            SetSkill(SkillName.MagicResist, 65.0, 85.0);
            SetSkill(SkillName.Wrestling, 65.0, 85.0);
            SetSkill(SkillName.ItemID, 65.0, 100.0);
            SetSkill(SkillName.ArmsLore, 65.0, 88.0);
            SetSkill(SkillName.Blacksmith, 90.0, 100.0);
            SetSkill(SkillName.Mining, 90.0, 100.0);

            SpeechHue = Server.Misc.RandomThings.GetSpeechHue();

            //CanTeach = false;
            Blessed = true;
            Female = false;
            FacialHairItemID = 0x204C; // BEARD
            HairItemID = 0x203C; // LONG HAIR
            FacialHairHue = 0x467;
            HairHue = 0x467;
        }

        public MuseumGuide(Serial serial) : base(serial)
        {
        }

        public override void InitSBInfo()
        {
            if (NpcGuild == NpcGuild.MinersGuild) { m_SBInfos.Add(new NMS_SBMinerBSMuseumGuide()); }
            else { m_SBInfos.Add(new NMS_SBMuseumGuide());  }
            //m_SBInfos.Add(new SBBuyArtifacts());
        }

        public override VendorShoeType ShoeType
        {
            get { return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
            int clothHue = Utility.RandomMinMax(0, 12);
            AddItem(new Server.Items.Robe(Server.Misc.RandomThings.GetRandomColor(clothHue)));

            this.Blessed = true;
            this.Female = false;
            this.FacialHairHue = 0x467;
            this.FacialHairItemID = 0x204C; // BEARD
            this.HairHue = 0x467;
            this.HairItemID = 0x203C; // LONG HAIR
        }

        /*public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold)
            {
                int halfDroppedCoins = dropped.Amount / 2;
                string sMessage = "";
                this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
            }

            return base.OnDragDrop(from, dropped);
        }*/

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

        private class FixEntry : ContextMenuEntry
        {
            private MuseumGuide m_Sage;
            private Mobile m_From;

            public FixEntry(MuseumGuide Sage, Mobile from) : base(6120, 1)
            {
                m_Sage = Sage;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Sage.BeginRepair(m_From);
            }
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive && !from.Blessed)
            {
                list.Add(new FixEntry(this, from));
            }

            base.AddCustomContextEntries(from, list);
        }

        public void BeginRepair(Mobile from)
        {
            if (Deleted || !from.Alive)
                return;
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
    }

    public class MinerBSMuseumGuide : MuseumGuide
    {
        [Constructable]
        public MinerBSMuseumGuide() : this(1)
        { }

        public MinerBSMuseumGuide(Serial serial) : base(serial)
        {
        }
        public override NpcGuild NpcGuild { get { return NpcGuild.MinersGuild; } }

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
    }
}