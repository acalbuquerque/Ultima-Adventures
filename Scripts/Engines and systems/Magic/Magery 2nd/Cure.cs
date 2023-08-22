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
					int chanceToCure = (int)NMSUtils.getBeneficialMageryInscribePercentage(Caster);
                    chanceToCure -= p.Level;
                    if (chanceToCure < 0) chanceToCure = 0;

                    if ((m.Poisoned && m.Poison.Level >= 4) || Server.Items.MortalStrike.IsWounded(m))
                    {
                        m.LocalOverheadMessage(MessageType.Emote, 33, true, "* Ouch! *");
                        m.PlaySound(343);
                        m.FixedParticles(0x374A, 10, 15, 5021, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
                        Caster.SendMessage(33, ((Caster == m) ? "Você está mortalmente envenenado e não poderá se curar com esse feitiço simples!" : "O seu alvo está mortalmente envenenado e não poderá ser curado com esse feitiço simples!"));
                    }
                    else if (chanceToCure <= Utility.RandomMinMax(p.Level * 2, 100))
					{
                        m.PlaySound(342);
                        m.SendMessage(33, "Você falhou em curar o veneno!"); //m.SendLocalizedMessage( 1010060 ); // You have failed to cure your target!
                        m.FixedParticles(0x374A, 10, 15, 5028, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
                    }
                    else
					{
                        m.CurePoison(Caster);
                        m.PlaySound(0x1E0);

                        Misc.Titles.AwardKarma(Caster, 10, true);
                        Caster.SendMessage(2253, ((Caster == m) ? "Você curou o veneno!" : "Você curou o veneno do alvo!"));
                        m.FixedParticles(0x373A, 10, 15, 5012, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
                        //m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
                    }
                }
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