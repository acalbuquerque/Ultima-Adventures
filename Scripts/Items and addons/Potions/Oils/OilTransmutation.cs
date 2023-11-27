using Server;
using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;
using Server.Misc;

namespace Server.Items
{
    public class OilTransmutation : Item
    {
        [Constructable]
        public OilTransmutation() : this(1)
        {
        }

        [Constructable]
        public OilTransmutation(int amount) : base(0x1FDD)
        {
            Weight = 0.01;
            Stackable = true;
            Amount = amount;
            Hue = 2170;
            Name = "Óleo de Transmutação";
        }

        /*public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Rub On Metal To Change It");
        }*/

        public override void OnDoubleClick(Mobile from)
        {
        }

        public OilTransmutation(Serial serial) : base(serial)
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
            ItemID = 0x1FDD;
        }
    }
}