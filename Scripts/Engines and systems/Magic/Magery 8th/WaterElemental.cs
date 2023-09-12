using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Eighth
{
	public class WaterElementalSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Water Elemental", "Kal Vas Xen An Flam",
				269,
				9070,
				false,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public WaterElementalSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast(Mobile caster)
		{
			if ( !base.CheckCast( caster ) )
				return false;

            if (Caster.Followers >= Caster.FollowersMax)
            {
                DoFizzle();
                Caster.SendMessage(55, "Você já tem muitos seguidores para invocar um novo servo.");
                return false;
            }

            return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
                TimeSpan duration = TimeSpan.FromSeconds((Caster.Skills[SkillName.Magery].Fixed * 0.1) * NMSUtils.getBonusIncriptBenefit(Caster));
                Caster.SendMessage(55, "O seu feitiço terá a duração de aproximadamente " + duration + "s.");
                // Supers has 50% chance to summon Greater Elemental
                if ((Caster.Skills[SkillName.Magery].Value >= 100 && Caster.Skills[SkillName.EvalInt].Value >= 100) &&
                    Utility.RandomMinMax(0, 1) != 0)
                    SpellHelper.Summon( new SummonedWaterElementalGreater(), Caster, 0x217, duration, false, false );
				else
					SpellHelper.Summon( new SummonedWaterElemental(), Caster, 0x217, duration, false, false );
			}

			FinishSequence();
		}
	}
}
