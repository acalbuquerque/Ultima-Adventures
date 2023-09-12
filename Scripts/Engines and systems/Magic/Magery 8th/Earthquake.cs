using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Eighth
{
	public class EarthquakeSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Earthquake", "In Vas Por",
				233,
				9012,
				false,
				Reagent.Bloodmoss,
				Reagent.Ginseng,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Eighth; } }

		public EarthquakeSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool DelayedDamage{ get{ return !Core.AOS; } }

		public override void OnCast()
		{
			if ( SpellHelper.CheckTown( Caster, Caster ) && CheckSequence() )
			{
				List<Mobile> targets = new List<Mobile>();

				Map map = Caster.Map;
                bool playerVsPlayer = false;
                if ( map != null )
					foreach ( Mobile m in Caster.GetMobilesInRange( 1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0) ) )
						if ( Caster.Region == m.Region && Caster != m && SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) && (!Core.AOS || Caster.InLOS( m )) )
						{
                            targets.Add(m);
                            if (m.Player)
                                playerVsPlayer = true;
                        }
                            
				Caster.PlaySound( 0x220 );

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = targets[i];

					double damage;

					int nBenefit = 0;
					if ( Caster is PlayerMobile ) // WIZARD
					{
						//nBenefit = (int)(Caster.Skills[SkillName.Magery].Value / 5);
					}

                    damage = GetNMSDamage(35, 1, 6, Caster.Player && playerVsPlayer) + nBenefit;

                    //damage = m.Hits / 2;

					if ( !m.Player )
						damage = Math.Max( Math.Min( damage, damage * 2), 15 );
						damage += Utility.RandomMinMax( 0, 10 );

					damage = damage + nBenefit;

                    if (CheckResisted(m))
                    {
                        damage *= 0.5;
                        m.SendMessage(55, "Sua aura mágica lhe ajudou a resistir metade do dano desse feitiço.");
                    }

                    if (m is PlayerMobile && m.FindItemOnLayer(Layer.Ring) != null && m.FindItemOnLayer(Layer.Ring) is OneRing)
                    {
                        m.SendMessage(33, "O UM ANEL te protegeu de ser revelado.");
                    }
                    else
                    {
                        m.RevealingAction();
                    }

                    Caster.DoHarmful( m );
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, (int)damage, 100, 0, 0, 0, 0 );
				}
			}

			FinishSequence();
		}
	}
}