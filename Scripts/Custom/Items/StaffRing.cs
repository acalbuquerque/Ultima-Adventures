// By Nerun

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{

    public class StaffRing : BaseRing
    {
        [Constructable]
        public StaffRing() : base(0x108a)
        {
            Hue = 2369;
            Weight = 1.0;
            Name = "The Staff Ring";
            Attributes.NightSight = 1;
            Attributes.AttackChance = 20;
            Attributes.LowerRegCost = 100;
            Attributes.LowerManaCost = 100;
            Attributes.RegenHits = 12;
            Attributes.RegenStam = 24;
            Attributes.RegenMana = 18;
            Attributes.SpellDamage = 30;
            Attributes.CastRecovery = 6;
            Attributes.CastSpeed = 4;
            LootType = LootType.Blessed;
        }

        public StaffRing(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.Counselor)
            {
                from.SendMessage("Você não é membro do do Time de Staffs e não deveria possuir este item. Reporte-se a um staff imediatamente para evitar problemas.");
                this.Delete();
                return;
            }
            else
            {
                base.OnEquip(from);
            }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.Counselor)
            {
                from.SendMessage("Você não é membro do do Time de Staffs e não deveria possuir este item. Reporte-se a um staff imediatamente para evitar problemas.");
                this.Delete();
            }
            else
            {
                if (from.AccessLevel == AccessLevel.Counselor)
                {
                    Hue = 2256;
                }
                else if (from.AccessLevel == AccessLevel.GameMaster)
                {
                    Hue = 2262;
                }
                else if (from.AccessLevel == AccessLevel.Seer)
                {
                    Hue = 2250;
                }
                else if (from.AccessLevel == AccessLevel.Administrator)
                {
                    Hue = 2253;
                }
                else
                {
                    Hue = 2232;
                }
            }
            return true;
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
}