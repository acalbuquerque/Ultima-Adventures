using System;
using Server;
using Server.Network;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class FishScanner : Item
    {
        private int m_Charges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        [Constructable]
        public FishScanner() : base(0x540A)
        {
            Name = "Scanner de Peixes";
            Weight = 5;
            Hue = 1366;
            ItemID = Utility.RandomMinMax(0x540A, 0x540B);
            Charges = 10;//Utility.RandomMinMax(10, 15);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (m_Charges > 1) { list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor(m_Charges.ToString() + " Cargas Restantes", "#8be4fc") ); }
            else { list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("1 Carga Restante", "#8be4fc")); }

            list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Skills: fishing + TasteID", "#8be4fc")); 
            list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Escaneia as propriedades mágicas de um peixe", "#ffe066"));
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage(55, "O item precisa estar na sua mochila.");
                return;
            }
            else
            {
                from.SendMessage(55, "Selecione o peixe a ser escaneado.");
                from.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private FishScanner m_Tool;

            public InternalTarget(FishScanner tool) : base(2, false, TargetFlags.None)
            {
                m_Tool = tool;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseMagicFish)
                {
                    BaseMagicFish m_Fish = (BaseMagicFish)targeted;

                    if (m_Fish.Deleted)
                        return;

                    if (m_Fish.RevealProps) 
                    {
                        from.SendMessage(55, "Este peixe teve suas propriedades mágicas identificadas.");
                        return;
                    }

                    if (!m_Fish.IsChildOf(from.Backpack))
                    {
                        from.SendMessage(55, "O peixe precisa estar na sua mochila.");
                        return;
                    }

                    if (80 >= from.Skills[SkillName.TasteID].Value && 90 >= from.Skills[SkillName.Fishing].Value)
                    {
                        from.SendMessage(55, "Apenas Mestres em Pesca e Peritos em Identificação do Paladar sabem como utilizar este equipamento.");
                        return;
                    }
                    else 
                    {
                        m_Fish.RevealProps = true;
                        from.SendMessage(55, "Você identificou as propriedades do peixe mágico.");
                        from.PlaySound(0x3BD);
                        m_Tool.ConsumeCharge(from);
                    }
                }
                else
                {
                    from.SendMessage(55, "Você só pode usar isso em peixe mágico.");
                }
            }
        }

        public void ConsumeCharge(Mobile from)
        {
            --Charges;

            if (Charges == 0)
            {
                from.SendMessage(55, "O scanner parece estar quebrado.");
                Item MyJunk = new SpaceJunkA();
                MyJunk.Hue = 751;
                MyJunk.ItemID = this.ItemID;
                MyJunk.Name = /*Server.Items.SpaceJunk.RandomCondition() + */"scanner quebrado de peixes";
                MyJunk.Weight = this.Weight;
                from.AddToBackpack(MyJunk);
                this.Delete();
            }
        }

        public FishScanner(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Charges = (int)reader.ReadInt();
        }
    }
}