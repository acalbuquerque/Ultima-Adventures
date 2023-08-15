using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    [FlipableAttribute(0x2D25, 0x2D31)]
    public class GodStaff : BaseStaff
    {
        //public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Block; } }
        //public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ForceOfNature; } }
        //public override WeaponAbility ThirdAbility{ get{ return WeaponAbility.MagicProtection; } }
        //public override WeaponAbility FourthAbility{ get{ return WeaponAbility.FreezeStrike; } }
        //public override WeaponAbility FifthAbility{ get{ return WeaponAbility.PsychicAttack; } }

        //public override int AosStrengthReq{ get{ return 150; } }
        public override int AosMinDamage { get { return 80; } }
        public override int AosMaxDamage { get { return 120; } }
        public override int AosSpeed { get { return 50; } }
        public override float MlSpeed { get { return 2.5f; } }

        public override int OldStrengthReq { get { return 150; } }
        public override int OldMinDamage { get { return 80; } }
        public override int OldMaxDamage { get { return 120; } }
        public override int OldSpeed { get { return 50; } }

        public override int InitMinHits { get { return 40; } }
        public override int InitMaxHits { get { return 80; } }

        public Mobile StaffOwner;
        public string StaffName;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Staff_Owner { get { return StaffOwner; } set { StaffOwner = value; } }

        [CommandProperty(AccessLevel.Owner)]
        public string Staff_Name { get { return StaffName; } set { StaffName = value; InvalidateProperties(); } }


        [Constructable]
        public GodStaff(Mobile from) : base(0x2D25)
        {
            LootType = LootType.Blessed;

            this.StaffOwner = from;
            string StaffName = "Belongs to " + StaffOwner.Name + "";

            EngravedText = StaffName;
            //
            Name = "God staff";
            Weight = 3.0;
            LootType = LootType.Blessed;
            AccuracyLevel = WeaponAccuracyLevel.Supremely;
            DurabilityLevel = WeaponDurabilityLevel.Indestructible;
            DamageLevel = WeaponDamageLevel.Vanq;
            Attributes.AttackChance = 10;
            WeaponAttributes.HitLeechHits = 50;
            Resource = CraftResource.RegularWood;
            AosElementDamages.Physical = 100;
            Attributes.SpellChanneling = 1;
            Attributes.SpellDamage = 50;
            Attributes.CastRecovery = 2;
            Attributes.CastSpeed = 2;
            Attributes.LowerManaCost = 40;
            Attributes.LowerRegCost = 100;
            WeaponAttributes.LowerStatReq = 150;
            this.Hue = 2254; //0x8D6; // special red
        }

        public GodStaff(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((Mobile)StaffOwner);
            writer.Write(StaffName);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            StaffOwner = reader.ReadMobile();
            StaffName = reader.ReadString();

            EngravedText = StaffName;
            LootType = LootType.Blessed;
            int version = reader.ReadEncodedInt();
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            this.AosElementDamages.Physical = 0;
            this.AosElementDamages.Fire = 0;
            this.AosElementDamages.Cold = 0;
            this.AosElementDamages.Poison = 0;
            this.AosElementDamages.Energy = 0;

            this.InvalidateProperties();
        }

        public override bool OnEquip(Mobile from)
        {
            //if ( this.StaffOwner == from )
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                base.OnEquip(from);
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Emote, 0xB1F, true, "** arrgh **");
                from.SendMessage("Você não deveria possuir este item. Reporte-se a um staff imediatamente para evitar problemas.");
                BrokenGear broke = new BrokenGear();
                broke.ItemID = this.ItemID;
                broke.Hue = 0x47E;
                broke.Name = "a broken staff";
                broke.Weight = this.Weight;
                from.AddToBackpack(broke);
                this.Delete();
                return false;
            }
            return true;
        }
    }
}