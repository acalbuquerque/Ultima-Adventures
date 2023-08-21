using System;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server
{
    public static class NMSUtils
    {
        public static double getBeneficialMageryInscribePercentage(Mobile Caster) 
        {
            double magery = Caster.Skills.Magery.Value;
            double inscribe = Caster.Skills.Inscribe.Value;

            double maxPercent = inscribe / 4;
            if (maxPercent <= 1)
                maxPercent = 1;

            double influence = (maxPercent / 100) + 1;
            double points = (magery * influence) / 4;

            return points;
        }

        public static double getDispelChance(Mobile caster, BaseCreature bc, int caosBonus)
        {
            double mageInscribPoints = (caster.Skills.Magery.Value + caster.Skills.Inscribe.Value);
            double superScale = (((mageInscribPoints - 200) / 100) + 1);
            double casterPower = (((mageInscribPoints) / 10) + (caster.RawInt / 10)) * superScale;
            double dispelChance = (casterPower - getSummonDispelDifficulty(bc)) + caosBonus; //adding  caos momentum to help dispel chance
            if (dispelChance < 0) { dispelChance = 0; }
            return dispelChance;
        }

        public static double getSummonDispelDifficulty(BaseCreature bc) 
        { 
            return (bc.DispelDifficulty + bc.DispelFocus) / 10;
        }

        public static int getSummonDispelPercentage(BaseCreature bc, int caosBonus)
        {
            return Utility.RandomMinMax((int)getSummonDispelDifficulty(bc), (int)(bc.DispelDifficulty / 2)) + caosBonus;
        }

    }
}