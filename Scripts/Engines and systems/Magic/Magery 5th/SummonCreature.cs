using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class SummonCreatureSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Creature", "Kal Xen",
				16,
				false,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public SummonCreatureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		// NOTE: Creature list based on 1hr of summon/release on OSI.

		private static Type[] m_Types = new Type[]
			{
				typeof( GrizzlyBear ),
				typeof( BlackBear ),
				typeof( Walrus ),
				typeof( Chicken ),
				typeof( GiantSerpent ),
				typeof( Alligator ),
				typeof( GreyWolf ),
				typeof( Slime ),
				typeof( Eagle ),
				typeof( Gorilla ),
				typeof( SnowLeopard ),
				typeof( Pig ),
				typeof( Hind ),
				typeof( Rabbit ),
                typeof( Dog ),
                typeof( WildCat ),
                typeof( Sheep )
            };

        private static Type[] m_SpecialTypes = new Type[]
            {
                typeof( PolarBear ),
                typeof( Horse ),
				typeof( Scorpion ),
                typeof( GiantSpider ),
                typeof( DireWolf ),
                typeof( DireBear ),
                typeof( RidableLlama ) 
            };

        public override bool CheckCast(Mobile caster)
		{
			if ( !base.CheckCast( caster ) )
				return false;

            if (Caster.Followers > Caster.FollowersMax / 2)
            {
                Caster.SendMessage(55, "Você já possui muitos seguidores para usar essa magia.");
                Caster.PlaySound(Caster.Female ? 812 : 1086);
                Caster.Say("*oops*");
                return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				try
				{
                    BaseCreature creature = (BaseCreature)Activator.CreateInstance(m_Types[Utility.Random(m_Types.Length)]);
                    if (Caster.Skills.Magery.Value >= 80)
					{
                        creature = (BaseCreature)Activator.CreateInstance(m_SpecialTypes[Utility.Random(m_SpecialTypes.Length)]);
                    }				
					//creature.ControlSlots = 2;

					int nBenefit = 0;
					if ( Caster is PlayerMobile ) // WIZARD
					{
						nBenefit = (int)(Caster.Skills[SkillName.AnimalLore].Value * 0.25);
					}

					TimeSpan duration;

                    duration = TimeSpan.FromSeconds(((Caster.Skills.Magery.Fixed) * 0.1) + nBenefit);
                    Caster.SendMessage(55, "O seu feitiço terá a duração de aproximadamente " + duration + "s.");
                    SpellHelper.Summon( creature, Caster, 0x215, duration, false, false );
				}
				catch
				{
				}
			}

			FinishSequence();
		}

		public override TimeSpan GetCastDelay()
		{
			if ( Core.AOS )
				return TimeSpan.FromTicks( base.GetCastDelay().Ticks * 3 );

			return base.GetCastDelay() + TimeSpan.FromSeconds( 3.0 );
		}
	}
}