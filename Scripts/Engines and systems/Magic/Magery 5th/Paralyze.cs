using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells.Chivalry;

namespace Server.Spells.Fifth
{
	public class ParalyzeSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Paralyze", "An Ex Por",
				218,
				9012,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public ParalyzeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if (Core.AOS && (m.Frozen || m.Paralyzed || (m.Spell != null && m.Spell.IsCasting && !(m.Spell is PaladinSpell))))
			{
				Caster.SendLocalizedMessage(1061923); // The target is already frozen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				double duration;

				int nBenefit = 0;
				/*if (Caster is PlayerMobile) // WIZARD
				{
					nBenefit = (int)(Caster.Skills[SkillName.Magery].Value / 2);
				}*/

				// Algorithm: ((10% of eval) + 5) seconds [- 50% if resisted]

				duration = 5.0 + (Caster.Skills[SkillName.EvalInt].Value * 0.1) + nBenefit;

				if (CheckResisted(m)) 
				{
                    duration *= 0.5;
                    m.SendMessage(55, "Sua aura mágica lhe ajudou a resistir ao feitiço pela metade. (" + duration + "s)");
                    m.FixedEffect(0x37B9, 10, 5);
                    m.FixedParticles(0x374A, 10, 30, 5013, Server.Items.CharacterDatabase.GetMySpellHue(m, 0), 2, EffectLayer.Waist);
                    Caster.SendMessage(55, "O oponente reisitiu ao feitiço pela metade. (" + duration + "s)");
                }
                    
				
                m.Paralyze( TimeSpan.FromSeconds( duration ) );

				m.PlaySound( 0x204 );
				m.FixedEffect( 0x376A, 6, 1, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );

				HarmfulSpell( m );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private ParalyzeSpell m_Owner;

			public InternalTarget( ParalyzeSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}