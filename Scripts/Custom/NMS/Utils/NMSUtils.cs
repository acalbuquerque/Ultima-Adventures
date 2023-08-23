using System;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Misc;

namespace Server
{
    public static class NMSUtils
    {
        public static double getDamageEvalBenefit(Mobile Caster) 
        {
            //double magery = Caster.Skills.Magery.Value;
            double eval = Caster.Skills.EvalInt.Value;
            double value = ((eval * 3) / 100) + 1;
            return value;
        }

        public static double getBonusIncriptBenefit(Mobile Caster)
        {
            //double magery = Caster.Skills.Magery.Value;
            double inscr = Caster.Skills.Inscribe.Value;
            double value = ((inscr * 3) / 100) + 1;
            return value;
        }

        public static double getBeneficialMageryInscribePercentage(Mobile Caster) 
        {
            double magery = Caster.Skills.Magery.Value;
            double inscribe = Caster.Skills.Inscribe.Value;

            double maxPercent = inscribe / 3;
            if (maxPercent <= 1)
                maxPercent = 1;

            double influence = (maxPercent / 100) + 1;
            double points = (magery * influence) / 3;

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

        //towns
        public static void makeCriminalAction(Mobile caster, bool status)
        {
            caster.CriminalAction(status);
            caster.SendMessage(55, "Você cometeu um ato criminoso.");
            Misc.Titles.AwardKarma(caster, -30, true);
        }
    }
}