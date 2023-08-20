using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Second
{
	public class CureSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Cure", "An Nox",
				212,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public CureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				Poison p = m.Poison;

				if ( p != null )
				{
                    int chanceToCure = 0;
                    int totalSkills = (int)(Caster.Skills[SkillName.Magery].Value + Caster.Skills[SkillName.Inscribe].Value);

                    if (totalSkills >= 240)
                    {
                        chanceToCure = 60;
                    }
                    else if ((Caster.Skills[SkillName.Magery].Value >= 120 && (Caster.Skills[SkillName.Inscribe].Value >= 100)))
                    {
                        chanceToCure = 50;
                    }
                    else if ((Caster.Skills[SkillName.Magery].Value >= 100 && (Caster.Skills[SkillName.Inscribe].Value >= 80)))
                    {
                        chanceToCure = 40;
                    }
                    else if ((Caster.Skills[SkillName.Magery].Value >= 80 && (Caster.Skills[SkillName.Inscribe].Value >= 60)))
                    {
                        chanceToCure = 30;
                    }
                    else
                    {
                        chanceToCure = 20;
                    }
                    chanceToCure -= (poison.Level * 2);
                    if (chanceToCure < 0) chanceToCure = 0;

                    if ( chanceToCure >= Utility.Random( 100 ) )
					{
						if ( m.CurePoison( Caster ) )
						{
							if ( Caster != m )
                                m.SendMessage(2253, "Você curou o veneno do alvo!");//Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!

                            m.SendMessage(2253, "Você curou o veneno!");
                            //m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
                        }
					}
					else
					{
                        m.SendMessage(2253, "Você falhou em curar o veneno!"); //m.SendLocalizedMessage( 1010060 ); // You have failed to cure your target!
					}
				}

				m.FixedParticles( 0x373A, 10, 15, 5012, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, EffectLayer.Waist );
				m.PlaySound( 0x1E0 );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private CureSpell m_Owner;

			public InternalTarget( CureSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}