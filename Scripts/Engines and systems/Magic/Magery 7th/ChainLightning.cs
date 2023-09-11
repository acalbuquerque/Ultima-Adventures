using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Regions;
using Server.Mobiles;

namespace Server.Spells.Seventh
{
	public class ChainLightningSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Chain Lightning", "Vas Ort Grav",
				209,
				9022,
				false,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public ChainLightningSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool DelayedDamage{ get{ return true; } }

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				SpellHelper.Turn( Caster, p );

				if ( p is Item )
					p = ((Item)p).GetWorldLocation();

				ArrayList targets = new ArrayList();

				Map map = Caster.Map;

				bool playerVsPlayer = false;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p ), 5 );

					foreach ( Mobile m in eable )
					{
						Mobile pet = m;

						if ( Caster.Region == m.Region && Caster != m )
						{
							//if ( m is BaseCreature )
								//petOwner = ((BaseCreature)m).GetMaster();

                            targets.Add(m);
                            if (m.Player)
                                playerVsPlayer = true;
                        }
					}

					eable.Free();
				}

				double damage;

				int nBenefit = 0;
				if ( Caster is PlayerMobile ) // WIZARD
				{
					//nBenefit = CalculateMobileBenefit(Caster, 6, 3);
				}

				damage = GetNMSDamage(38, 2, 5, Caster.Player && playerVsPlayer) + nBenefit;

				if ( targets.Count > 0 )
				{
					if ( targets.Count > 1 )
						damage /= targets.Count;

					if (damage < 12)
						damage = 12;

                    for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile m = (Mobile)targets[i];

						Region house = m.Region;

						double toDeal = damage;

						if ( CheckResisted( m ) )
						{
							toDeal *= 0.5;
                            m.SendMessage(55, "Sua aura mágica lhe ajudou a resistir metade do dano desse feitiço.");
						}

						if( !(house is Regions.HouseRegion) )
						{
							if (m is PlayerMobile && m.FindItemOnLayer(Layer.Ring) != null && m.FindItemOnLayer(Layer.Ring) is OneRing) 
							{
                                m.SendMessage(33, "O UM ANEL te protegeu de ser revelado e absorveu metade do dano recebido.");
                                toDeal *= 0.5;
                            }
							else
							{
								m.RevealingAction();
                            }

                            Caster.DoHarmful(m);
                            SpellHelper.Damage(this, m, toDeal, 0, 0, 0, 0, 100);

                            if ( Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ) > 0 )
							{
								Point3D blast = new Point3D( ( m.X ), ( m.Y ), m.Z+10 );
								Effects.SendLocationEffect( blast, m.Map, 0x2A4E, 30, 10, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );
								m.PlaySound( 0x029 );
							}
							else
							{
								m.BoltEffect( 0 );
							}
                                                          
                        }
					}
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ChainLightningSpell m_Owner;

			public InternalTarget( ChainLightningSpell owner ) : base( 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D p = o as IPoint3D;

				if ( p != null )
					m_Owner.Target( p );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}