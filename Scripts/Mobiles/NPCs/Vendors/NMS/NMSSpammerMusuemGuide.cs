using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    public class NMSSpammerMuseumGuide : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public NMSSpammerMuseumGuide()
        {
        }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo("Guia de Minérios", typeof(LearnMetalBook), 8, 50, 0x4C5B, 0));
                Add(new GenericBuyInfo("Guia de Granitos", typeof(LearnGraniteBook), 8, 50, 0x4C5C, 0));

                Add(new GenericBuyInfo(typeof(IronIngot), 15, 150, 0x1BF2, 0));
                Add(new GenericBuyInfo("CR Ingots", typeof(DullCopperIngot), 25, 100, 0x1BF2, 2741));
                
                Add(new GenericBuyInfo("Cobre Ingots", typeof(CopperIngot), 35, 70, 0x1BF2, 2840));
                Add(new GenericBuyInfo("Ferro Negro Ingots", typeof(ShadowIronIngot), 30, 80, 0x1BF2, 2739));
                if (Utility.RandomBool())
                    Add(new GenericBuyInfo("Bronze Ingots", typeof(BronzeIngot), 40, 50, 0x1BF2, 2236));
                if (Utility.RandomDouble() > 0.60)
                    Add(new GenericBuyInfo("Dourado ingots", typeof(GoldIngot), 55, 30, 0x1BF2, 2843));
                if (Utility.RandomDouble() > 0.70)
                    Add(new GenericBuyInfo("Agapite Ingots", typeof(AgapiteIngot), 85, 25, 0x1BF2, 2794));
                if (Utility.RandomDouble() > 0.80)
                    Add(new GenericBuyInfo("Verite Ingots", typeof(VeriteIngot), 125, 20, 0x1BF2, 2141));
                if (Utility.RandomDouble() > 0.90)
                    Add(new GenericBuyInfo("Valorite Ingots", typeof(ValoriteIngot), 250, 15, 0x1BF2, 2397));
                if (Utility.RandomDouble() > 0.95)
                    Add(new GenericBuyInfo("Rosenium Ingots", typeof(RoseniumIngot), 500, 5, 0x1BF2, 236));
                if (Utility.RandomDouble() > 0.95)
                    Add(new GenericBuyInfo("Titanium Ingots", typeof(TitaniumIngot), 500, 5, 0x1BF2, 1381));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(IronIngot), 5);
                Add(typeof(DullCopperIngot), 8);
                Add(typeof(ShadowIronIngot), 10);
                Add(typeof(CopperIngot), 12);
                Add(typeof(BronzeIngot), 14);
                Add(typeof(GoldIngot), 16);
                Add(typeof(AgapiteIngot), 19);
                Add(typeof(VeriteIngot), 25);
                Add(typeof(ValoriteIngot), 45);
                Add(typeof(RoseniumIngot), 100);
                Add(typeof(TitaniumIngot), 100);
            }
        }
    }
}
