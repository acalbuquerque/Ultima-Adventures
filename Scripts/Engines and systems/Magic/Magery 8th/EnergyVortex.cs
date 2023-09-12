using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Eighth
{
	public class EnergyVortexSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Elemental Vortex", "Vas Corp Por",
				260,
				9032,
				false,
				Reagent.Bloodmoss,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.Nightshade
			);

		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public EnergyVortexSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			Map map = Caster.Map;

			SpellHelper.GetSurfaceTop( ref p );

			if ( map == null || !map.CanSpawnMobile( p.X, p.Y, p.Z ) )
			{
                Caster.SendMessage(55, "O local está bloquado para este feitiço.");
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
                TimeSpan duration = TimeSpan.FromSeconds((Caster.Skills[SkillName.Magery].Fixed * 0.1) * NMSUtils.getBonusIncriptBenefit(Caster));
                Caster.SendMessage(55, "O seu feitiço terá a duração de aproximadamente " + duration + "s.");

                BaseCreature.Summon( new EnergyVortex(), false, Caster, new Point3D( p ), 0x212, duration );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private EnergyVortexSpell m_Owner;

			public InternalTarget( EnergyVortexSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
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
                //from.SendLocalizedMessage( 501943 ); // Target cannot be seen. Try again.
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