using System;
using System.Collections.Generic;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Fourth
{
	public class ManaDrainSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mana Drain", "Ort Rel",
				215,
				9031,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public ManaDrainSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		private static Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();

		private void AosDelay_Callback( object state )
		{
			object[] states = (object[])state;

			Mobile m = (Mobile)states[0];
			int mana = (int)states[1];

			if ( m.Alive && !m.IsDeadBondedPet )
			{
				m.Mana += mana;

				m.FixedEffect( 0x3779, 10, 25, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );
				m.PlaySound( 0x28E );
			}

			m_Table.Remove( m );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
                Caster.SendMessage(55, "O alvo n�o pode ser visto.");
            }
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				if ( m.Spell != null )
					m.Spell.OnCasterHurt();

				m.Paralyzed = false;
				double magebonus = (Caster.Skills.Magery.Value * NMSUtils.getDamageEvalBenefit(Caster));
                int toDrain = (int)((magebonus / 10)*1.25);
                if (toDrain < 0)
                    toDrain = 0;
                else if (toDrain > m.Mana)
                    toDrain = m.Mana;

                if (m_Table.ContainsKey(m))
                    toDrain = 0;

                m.FixedParticles(0x3789, 10, 25, 5032, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Head);
                m.PlaySound(0x1F8);

                if (toDrain > 0)
                {
                    //Caster.SendMessage(33, "==> " + toDrain);
                    m.Mana -= toDrain;
					int seconds = (int)(5 * NMSUtils.getDamageEvalBenefit(Caster));
                    Caster.SendMessage(33, "==> " + NMSUtils.getDamageEvalBenefit(Caster));
                    Caster.SendMessage(22, "====> " + seconds);
                    m_Table[m] = Timer.DelayCall(TimeSpan.FromSeconds(seconds), new TimerStateCallback(AosDelay_Callback), new object[] { m, toDrain });
                }
/*                if ( Core.AOS )
				{
                    Caster.SendMessage(33, "AOS");
                    int toDrain = 40 + (int)(GetDamageSkill( Caster ) - GetResistSkill( m ));

					if (Caster is PlayerMobile && ((PlayerMobile)Caster).Sorcerer() )
						toDrain = (int)((double)toDrain*1.25);

					if ( toDrain < 0 )
						toDrain = 0;
					else if ( toDrain > m.Mana )
						toDrain = m.Mana;

					if ( m_Table.ContainsKey( m ) )
						toDrain = 0;

					m.FixedParticles( 0x3789, 10, 25, 5032, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, EffectLayer.Head );
					m.PlaySound( 0x1F8 );

					if ( toDrain > 0 )
					{
						m.Mana -= toDrain;

						m_Table[m] = Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( AosDelay_Callback ), new object[]{ m, toDrain } );
					}
				}
				else
				{
                    Caster.SendMessage(22, "!AOS");
                    if ( CheckResisted( m ) )
						m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
					else if ( m.Mana >= 100 )
						m.Mana -= Utility.Random( 1, 100 );
					else
						m.Mana -= Utility.Random( 1, m.Mana );

					m.FixedParticles( 0x374A, 10, 15, 5032, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, EffectLayer.Head );
					m.PlaySound( 0x1F8 );
				}*/

				HarmfulSpell( m );
			}

			FinishSequence();
		}

/*		public override double GetResistPercent( Mobile target )
		{
			return 99.0;
		}*/

		private class InternalTarget : Target
		{
			private ManaDrainSpell m_Owner;

			public InternalTarget( ManaDrainSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
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
