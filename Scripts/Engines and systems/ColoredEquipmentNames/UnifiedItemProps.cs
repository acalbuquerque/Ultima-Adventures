using System;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace ItemNameHue
{
    public class UnifiedItemProps
    {
		public static string GetArmorItemValue(Item item)
		{
			int rarityValue = 0, rarityProps = 0;
			ItemTier.GetItemTier(item, out rarityValue, out rarityProps);

			if (rarityProps >= 5)
                return "<BASEFONT COLOR=#fc893d>";
			else if (rarityProps >= 3)
                return "<BASEFONT COLOR=#b997fc>";
            else if (rarityProps >= 1)
                return "<BASEFONT COLOR=#9cf77e>";
            

			return "<BASEFONT COLOR=#D6D6D6>";
		}

		public static string RarityNameMod(Item item, string orig)
		{
			return (string)(GetArmorItemValue(item) + orig + "<BASEFONT COLOR=#FFFFFF>");
		}

        public static string SetColor(string text, string color)
        {
            return String.Format("<BASEFONT COLOR={0}>{1}</BASEFONT>", color, text);
        }
    }
}
