using System;
using Server;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an elemental corpse")]
    public class StaticElemental : BaseCreature
    {
        [Constructable]
        public StaticElemental() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "static elemental";
            Body = 14;
            Hue = 33;

            Blessed = true;
            Hidden = true;
            CantWalk = true;
        }

        public override void GenerateLoot()
        {
        }

        public StaticElemental(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}