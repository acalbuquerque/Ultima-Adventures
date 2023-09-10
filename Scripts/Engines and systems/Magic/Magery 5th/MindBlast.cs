using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Fifth
{
	public class MindBlastSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mind Blast", "Por Corp Wis",
				218,
				Core.AOS ? 9002 : 9032,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.Nightshade,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public MindBlastSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
			if ( Core.AOS )
				m_Info.LeftHandEffect = m_Info.RightHandEffect = 9002;
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		private void AosDelay_Callback( object state )
		{
			object[] states = (object[])state;
			Mobile caster = (Mobile)states[0];
			Mobile target = (Mobile)states[1];
			Mobile defender = (Mobile)states[2];
			int damage = (int)states[3];

			if ( caster.HarmfulCheck( defender ) )
			{
				SpellHelper.Damage( this, target, Utility.RandomMinMax( damage, damage + 4 ), 0, 0, 100, 0, 0 );

				target.FixedParticles( 0x374A, 10, 15, 5038, Server.Items.CharacterDatabase.GetMySpellHue( caster, 1181 ), 2, EffectLayer.Head );
				target.PlaySound( 0x213 );
			}
		}

		public override bool DelayedDamage{ get{ return !Core.AOS; } }

		private double calcDamage(Mobile from, Mobile target) 
		{
            int fromStat = from.Int, targetStat = target.Int, damage = 0;
            // Algorithm: (fromStat - targetStat) ?? 3

            if (targetStat < fromStat)
                damage = ((fromStat - targetStat) > 3) ? (fromStat - targetStat) : 3;
            else
                damage = ((targetStat - fromStat) > 3) ? (targetStat - fromStat) : 3;

            double finalDamage = (GetDamageScalar(target) * (damage)) - Utility.RandomMinMax(0, 3);

            if (finalDamage > 42)
                finalDamage = 42;

            if (CheckResisted(target))
            {
                finalDamage /= 2;
                target.SendMessage(55, "Você resiste aos efeitos da magia."); // You feel yourself resisting magical energy.
            }

            from.FixedParticles(0x374A, 10, 15, 2038, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 1181), 2, EffectLayer.Head);
            target.FixedParticles(0x374A, 10, 15, 5038, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 1181), 2, EffectLayer.Head);
            target.PlaySound(0x213);

			return finalDamage;
        }

		public void Target( Mobile m )
		{
			int nBenefit = 0;
/*			if ( Caster is PlayerMobile ) // WIZARD
			{
				nBenefit = (int)(Caster.Skills[SkillName.Magery].Value / 5);
			}*/

			if ( !Caster.CanSee( m ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( Core.AOS )
			{
                if ( Caster.CanBeHarmful( m ) && CheckSequence() )
				{
					Mobile from = Caster, target = m;

					SpellHelper.Turn( from, target );

					SpellHelper.NMSCheckReflect( (int)this.Circle, ref from, ref target );

					int damage = (int)calcDamage(from, target) + nBenefit;

                    Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ),
						new TimerStateCallback( AosDelay_Callback ),
						new object[]{ Caster, target, m, damage } );
				}
			}
			else if ( CheckHSequence( m ) )
			{
                Mobile from = Caster, target = m;

				SpellHelper.Turn( from, target );

				SpellHelper.NMSCheckReflect( (int)this.Circle, ref from, ref target );

                double damage = calcDamage(from, target);

                SpellHelper.Damage( this, target, damage, 0, 0, 100, 0, 0 );
			}

			FinishSequence();
		}

		public override double GetSlayerDamageScalar( Mobile target )
		{
			return 1.0; //This spell isn't affected by slayer spellbooks
		}

		private class InternalTarget : Target
		{
			private MindBlastSpell m_Owner;

			public InternalTarget( MindBlastSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
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