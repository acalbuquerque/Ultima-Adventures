using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Fourth
{
	public class ArchCureSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Arch Cure", "Vas An Nox",
				215,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

		public ArchCureSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		// Archcure is now 1/4th of a second faster
		public override TimeSpan CastDelayBase{ get{ return base.CastDelayBase - TimeSpan.FromSeconds( 0.25 ); } }

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				List<Mobile> targets = new List<Mobile>();

				Map map = Caster.Map;
				Mobile m_directtarget = p as Mobile;

				if ( map != null )
				{
					//you can target directly someone/something and become criminal if it's a criminal action
					 /*if ( m_directtarget != null )
						targets.Add ( m_directtarget );*/

					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p ), 2 );

					foreach ( Mobile m in eable )
					{
						if (m.Poisoned) 
						{
                            targets.Add(m);
                        }
/*                        if (Caster.CanBeBeneficial(m, false) && (m != m_directtarget && m is PlayerMobile) || (m == Caster && m != m_directtarget) || IsAllyTo(Caster, m))
                        {
                            
                        }*/
						// Archcure area effect won't cure aggressors or victims, nor murderers, criminals or monsters 
						// plus Arch Cure Area will NEVER work on summons/pets if you are in Felucca facet
						// red players can cure only themselves and guildies with arch cure area.

						/*if ( map.Rules == MapRules.FeluccaRules )
							{
								if ( Caster.CanBeBeneficial( m, false ) && ( !Core.AOS || !IsAggressor( m ) && !IsAggressed( m ) && (( IsInnocentTo ( Caster, m ) && IsInnocentTo ( m, Caster ) ) || ( IsAllyTo ( Caster, m ) )) && m != m_directtarget && m is PlayerMobile || m == Caster && m != m_directtarget ))
									targets.Add( m );
							}
						else if ( Caster.CanBeBeneficial( m, false ) && ( !Core.AOS || !IsAggressor( m ) && !IsAggressed( m ) && (( IsInnocentTo ( Caster, m ) && IsInnocentTo ( m, Caster ) ) || ( IsAllyTo ( Caster, m ) )) && m != m_directtarget || m == Caster && m != m_directtarget ))
							targets.Add( m );*/
					}

					eable.Free();
				}
				Effects.PlaySound( p, Caster.Map, 0x299 );
                //Caster.SendMessage(20, "targets.Count -> " + targets.Count);

				// TODO - Maybe we can limit that only >=100 mage can cure more than 5 ?

                if ( targets.Count > 0 )
				{
					int cured = 0;

					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile m = targets[i];

						Caster.DoBeneficial( m );

						Poison poison = m.Poison;

						if ( poison != null )
						{
                            int chanceToCure = (int)NMSUtils.getBeneficialMageryInscribePercentage(Caster);
                            chanceToCure -= (poison.Level >= 4) ? poison.Level * 2 : poison.Level; // lethal poison double the reduce the chance to cure
                            if (chanceToCure < 0) chanceToCure = 0;

                            //Caster.SendMessage(22, "chanceToCure -> " + chanceToCure);
                            if (chanceToCure >= Utility.RandomMinMax(poison.Level * 2, 100) && m.CurePoison(Caster)) // success
							{
								++cured;
                                m.PlaySound(0x1E0);
                                m.FixedParticles(0x373A, 10, 15, 5012, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
							}
							else 
							{
                                m.PlaySound(342);
                                m.FixedParticles(0x374A, 10, 15, 5028, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
                            }
						}
					}

					if ( cured > 0 && cured == targets.Count)
					{
                        Misc.Titles.AwardKarma(Caster, (10 * cured), true);
                        Caster.PlaySound(Caster.Female ? 783 : 1054); Caster.Say("*woohoo!*");
                        Caster.SendMessage(2253, "Você curou todos os venenos próximos do alvo!");
                        Caster.FixedParticles(0x376A, 9, 32, 5030, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
                    }
					else if (cured > 0)
					{
                        Misc.Titles.AwardKarma(Caster, (10 * cured), true);
                        Caster.SendMessage(55, "Você curou alguns dos venenos próximos do alvo!");
                    }
                    else
                        Caster.SendMessage(33, "Você não conseguiu curar nenhum dos venenos próximos do alvo!");
                    //Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!
                }
			}

			FinishSequence();
		}

		private bool IsAggressor( Mobile m )
		{
			foreach ( AggressorInfo info in Caster.Aggressors )
			{
				if ( m == info.Attacker && !info.Expired )
					return true;
			}

			return false;
		}

		private bool IsAggressed( Mobile m )
		{
			foreach ( AggressorInfo info in Caster.Aggressed )
			{
				if ( m == info.Defender && !info.Expired )
					return true;
			}

			return false;
		}

		private static bool IsInnocentTo( Mobile from, Mobile to )
		{
			return ( Notoriety.Compute( from, (Mobile)to ) == Notoriety.Innocent );
		}
		
		private static bool IsAllyTo( Mobile from, Mobile to )
		{
			return ( Notoriety.Compute( from, (Mobile)to ) == Notoriety.Ally );
		}

		private class InternalTarget : Target
		{
			private ArchCureSpell m_Owner;

			public InternalTarget( ArchCureSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
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