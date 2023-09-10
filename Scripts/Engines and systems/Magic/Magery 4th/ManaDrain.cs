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
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );

				if ( m.Spell != null )
					m.Spell.OnCasterHurt();

				m.Paralyzed = false;
				double magebonus = (Caster.Skills.Magery.Value * NMSUtils.getDamageEvalBenefit(Caster));
                int toDrain = (int)((magebonus / 10)*1.5);
				if (toDrain < 0)
					toDrain = 0;
				else if (m is PlayerMobile && toDrain > (m.ManaMax * 0.3))
					toDrain = (int)(m.ManaMax * 0.3); // limit mana drain to max 30% of PLAYER defender.
                else if (m is BaseCreature && toDrain > (m.ManaMax * 0.5))
                    toDrain = (int)(m.ManaMax * 0.5); // limit mana drain to max 50% of Mobs defender.
                else if (toDrain > m.Mana)
					toDrain = m.Mana;

                if (m_Table.ContainsKey(m))
                    toDrain = 0;

                m.FixedParticles(0x3789, 10, 25, 5032, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Head);
                m.PlaySound(0x1F8);

                if (toDrain > 0)
                {
                    m.Mana -= toDrain;
					int seconds = (int)(5 * NMSUtils.getDamageEvalBenefit(Caster)) + Utility.RandomMinMax(1, 3); ;
                    Caster.SendMessage(55, "Você retirou " + toDrain + " pontos de mana do seu oponente e os efeitos do feitiço irão durar " + seconds + "s");
                    m.SendMessage(33, "Você perdeu " + toDrain + " pontos de mana e os efeitos desse feitiço irão durar " + seconds + "s");
                    m_Table[m] = Timer.DelayCall(TimeSpan.FromSeconds(seconds), new TimerStateCallback(AosDelay_Callback), new object[] { m, toDrain });
                }

				HarmfulSpell( m );
			}

			FinishSequence();
		}

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
