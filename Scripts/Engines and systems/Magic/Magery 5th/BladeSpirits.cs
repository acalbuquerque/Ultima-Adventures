using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class BladeSpiritsSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Blade Spirits", "In Jux Hur Ylem", 
				266,
				9040,
				false,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.Nightshade
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public BladeSpiritsSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override TimeSpan GetCastDelay()
		{
			if ( Core.AOS )
				return TimeSpan.FromTicks( base.GetCastDelay().Ticks * ((Core.SE) ? 3 : 5) );

			return base.GetCastDelay() + TimeSpan.FromSeconds( 6.0 );
		}

		public override bool CheckCast(Mobile caster)
		{
			if ( !base.CheckCast( caster ) )
				return false;

			if( Caster.Followers > Caster.FollowersMax )
			{
                Caster.SendMessage(55, "Você já possui muitos seguidores para usar essa magia.");
                Caster.PlaySound(Caster.Female ? 812 : 1086);
                Caster.Say("*oops*");
                //Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

			int nBenefit = 0;
/*			if ( Caster is PlayerMobile )
			{
				nBenefit = (int)(Caster.Skills[SkillName.Magery].Value / 2);
			}*/

			if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
				DoFizzle();
                Caster.SendMessage(55, "Esta localização está bloqueada.");
                Caster.PlaySound(Caster.Female ? 812 : 1086);
                Caster.Say("*oops*");
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				TimeSpan duration = SpellHelper.NMSGetDuration(Caster, Caster, false);
				BaseCreature.Summon( new BladeSpirits(), false, Caster, new Point3D( p ), 0x212, duration );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private BladeSpiritsSpell m_Owner;

			public InternalTarget( BladeSpiritsSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o );
			}

			protected override void OnTargetOutOfLOS( Mobile from, object o )
			{
                from.SendMessage(55, "O alvo não pode ser visto.");
				from.Target = new InternalTarget( m_Owner );
				from.Target.BeginTimeout( from, TimeoutTime - DateTime.UtcNow );
				m_Owner = null;
			}

			protected override void OnTargetFinish( Mobile from )
			{
				if ( m_Owner != null )
					m_Owner.FinishSequence();
			}
		}
	}
}