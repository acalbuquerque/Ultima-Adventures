using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Sixth
{
	public class InvisibilitySpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Invisibility", "An Lor Xen",
				206,
				9002,
				Reagent.Bloodmoss,
				Reagent.Nightshade
			);

		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public InvisibilitySpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( m is Mobiles.BaseVendor || m is Mobiles.PlayerVendor || m is Mobiles.PlayerBarkeeper || m.AccessLevel > Caster.AccessLevel )
			{
                Caster.SendMessage(55, "Este feitiço não funciona neste alvo.");
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				Effects.SendLocationParticles( EffectItem.Create( new Point3D( m.X, m.Y, m.Z + 16 ), Caster.Map, EffectItem.DefaultDuration ), 0x376A, 10, 15, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, 5045, 0 );
				m.PlaySound( 0x3C4 );

				m.Hidden = true;
				m.Combatant = null;
				m.Warmode = false;

				RemoveTimer( m );

				int nBenefit = 0;
				if ( Caster is PlayerMobile ) // WIZARD
				{
                    nBenefit = (int)(Caster.Skills[SkillName.Magery].Value * 0.25);

                    foreach ( Mobile pet in World.Mobiles.Values )
					{
						if ( pet is BaseCreature )
						{
							BaseCreature bc = (BaseCreature)pet;
							if ( bc.Controlled && bc.ControlMaster == m )
								pet.Hidden = true;
						}
					}
				}

				TimeSpan duration = TimeSpan.FromSeconds( (5 + (Caster.Skills.Inscribe.Fixed * 0.1)) + nBenefit );
                Caster.SendMessage(55, "O seu feitiço terá a duração de aproximadamente " + duration + "s.");
                Timer t = new InternalTimer( m, duration );

				BuffInfo.RemoveBuff( m, BuffIcon.HidingAndOrStealth );
				BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Invisibility, 1075825, duration, m ) );	//Invisibility/Invisible

				m_Table[m] = t;

				t.Start();
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool HasTimer( Mobile m )
		{
			return m_Table[m] != null;
		}

		public static void RemoveTimer( Mobile m )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
			{
				t.Stop();
				m_Table.Remove( m );
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile;

			public InternalTimer( Mobile m, TimeSpan duration ) : base( duration )
			{
				Priority = TimerPriority.OneSecond;
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				m_Mobile.RevealingAction();
				RemoveTimer( m_Mobile );
			}
		}

		public class InternalTarget : Target
		{
			private InvisibilitySpell m_Owner;

			public InternalTarget( InvisibilitySpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
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