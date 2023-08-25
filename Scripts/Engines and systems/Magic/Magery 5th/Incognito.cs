using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Targeting;

namespace Server.Spells.Fifth
{
	public class IncognitoSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Incognito", "Kal In Ex",
				206,
				9002,
				Reagent.Bloodmoss,
				Reagent.Garlic,
				Reagent.Nightshade
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
        public static bool originalGenderFemale = false;
        public static int originalHue = 0;
        public static int originalHairHue = 0;
        public static int originalFacialHairHue = 0;
        public IncognitoSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

        public override void OnCast()
        {
			originalGenderFemale = Caster.Female;
			originalHairHue = Caster.HairHue;
            originalFacialHairHue = Caster.FacialHairHue;
			originalHue = Caster.Hue;
            Caster.SendMessage(55, "Quem você deseja usar como disfarce?");
            Caster.Target = new InternalTarget(this);
        }

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendMessage(55, "O alvo não pode ser visto.");
			}
			else if (!Caster.CanBeginAction(typeof(IncognitoSpell)))
			{
				Caster.SendMessage(55, "Este feitiço já atua sobre você!");
			}
			else if (Caster.BodyMod == 183 || Caster.BodyMod == 184)
			{
				Caster.SendMessage(55, "Você não pode usar esse feitiço enquanto veste uma pintura corporal.");
			}
			else if (DisguiseTimers.IsDisguised(Caster))
			{
				Caster.SendMessage(55, "Você não pode usar esse feitiço quando já está disfarçado.");
			}
			else if (!Caster.CanBeginAction(typeof(PolymorphSpell)) || Caster.IsBodyMod)
			{
				DoFizzle();
			}
			else if ((m is PlayerMobile || m is BaseCreature) && (m.Body.Type == Caster.Body.Type) && CheckBSequence(m))
			{
				if (Caster.BeginAction(typeof(IncognitoSpell)))
				{
					DisguiseTimers.StopTimer(Caster);
					PlayerMobile pm = Caster as PlayerMobile;

					if (pm != null && pm.Race != null)
					{
						pm.HueMod = m.HueMod;//Caster.Race.RandomSkinHue();
						pm.NameMod = m.Name;//Caster.Female ? NameList.RandomName("female") : NameList.RandomName("male");
						pm.Title = "[fake]";
                        pm.SetHairMods(m.HairItemID, m.FacialHairItemID);
                        pm.Female = m.Female;
                        pm.HairHue = m.HairHue;
						pm.Hue = m.Hue;
						pm.FacialHairHue = m.FacialHairHue;
					}

					Caster.FixedParticles(0x373A, 10, 15, 5036, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Head);
					Caster.PlaySound(0x3BD);

					BaseArmor.ValidateMobile(Caster);
					BaseClothing.ValidateMobile(Caster);

					StopTimer(Caster);

					TimeSpan length = SpellHelper.NMSGetDuration(Caster, Caster, false);//TimeSpan.FromSeconds(timeVal);

					Timer t = new InternalTimer(Caster, length);

					m_Timers[Caster] = t;

					t.Start();

					BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Incognito, 1075819, length, Caster));
				}
				else
				{
					Caster.SendMessage(55, "Você já está sobre efeito do feitiço!");
				}
			}
			else 
			{
                DoFizzle();
                Caster.SendMessage(55, "Não é possível usar este feitiço neste alvo!");
            }
            FinishSequence();
        }

		public override bool CheckCast(Mobile caster)
		{
			if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) )
			{
                Caster.SendMessage(55, "Este feitiço já atua sobre você!");
				return false;
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
                Caster.SendMessage(55, "Você não pode usar esse feitiço enquanto veste uma pintura corporal.");
				return false;
			}

			return true;
		}

		/*public override void OnCast()
		{
			if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) )
			{
                Caster.SendMessage(55, "Este feitiço já atua sobre você!");
            }
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
                Caster.SendMessage(55, "Você não pode usar esse feitiço enquanto veste uma pintura corporal.");
            }
			else if ( DisguiseTimers.IsDisguised( Caster ) )
			{
                Caster.SendMessage(55, "Você não pode usar esse feitiço quando já está disfarçado."); 
				//Caster.SendLocalizedMessage( 1061631 ); // You can't do that while disguised.
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) || Caster.IsBodyMod )
			{
				DoFizzle();
			}
			else if ( CheckSequence() )
			{
				if ( Caster.BeginAction( typeof( IncognitoSpell ) ) )
				{
					DisguiseTimers.StopTimer( Caster );

					Caster.HueMod = Caster.Race.RandomSkinHue();
					Caster.NameMod = Caster.Female ? NameList.RandomName( "female" ) : NameList.RandomName( "male" );

					PlayerMobile pm = Caster as PlayerMobile;

					if ( pm != null && pm.Race != null )
					{
						pm.SetHairMods( pm.Race.RandomHair( pm.Female ), pm.Race.RandomFacialHair( pm.Female ) );
						pm.HairHue = pm.Race.RandomHairHue();
						pm.FacialHairHue = pm.Race.RandomHairHue();
					}

					Caster.FixedParticles( 0x373A, 10, 15, 5036, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, EffectLayer.Head );
					Caster.PlaySound( 0x3BD );

					BaseArmor.ValidateMobile( Caster );
					BaseClothing.ValidateMobile( Caster );

					StopTimer( Caster );


					int timeVal = SpellHelper.NMSGetDuration(Caster, Caster, false);//((6 * Caster.Skills.Magery.Fixed) / 50) + 1;

                    if ( timeVal > 120 )
						timeVal = 120;

					TimeSpan length = TimeSpan.FromSeconds( timeVal );

					Timer t = new InternalTimer( Caster, length );

					m_Timers[Caster] = t;

					t.Start();

					BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.Incognito, 1075819, length, Caster ) );

				}
				else
				{
                    Caster.SendMessage(55, "Você já está disfarçado!");
                    //Caster.SendLocalizedMessage( 1079022 ); // You're already incognitoed!
				}
			}

			FinishSequence();
		}*/

		public static Hashtable m_Timers = new Hashtable();

		public static bool StopTimer( Mobile m )
		{
			Timer t = (Timer)m_Timers[m];

			if ( t != null )
			{
				t.Stop();
				m_Timers.Remove( m );
				BuffInfo.RemoveBuff( m, BuffIcon.Incognito );
            }

			return ( t != null );
		}

		private static int[] m_HairIDs = new int[]
			{
				0x2044, 0x2045, 0x2046,
				0x203C, 0x203B, 0x203D,
				0x2047, 0x2048, 0x2049,
				0x204A, 0x0000
			};

		private static int[] m_BeardIDs = new int[]
			{
				0x203E, 0x203F, 0x2040,
				0x2041, 0x204B, 0x204C,
				0x204D, 0x0000
			};

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;

			public InternalTimer( Mobile owner, TimeSpan length ) : base( length )
			{
				m_Owner = owner;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( IncognitoSpell ) ) )
				{
					if ( m_Owner is PlayerMobile )
						((PlayerMobile)m_Owner).SetHairMods( -1, -1 );

					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.NameMod = null;
                    m_Owner.Title = null;
                    m_Owner.HairHue = originalHairHue;
                    m_Owner.FacialHairHue = originalFacialHairHue;
                    m_Owner.Female = originalGenderFemale;
					m_Owner.Hue = originalHue;
                    m_Owner.EndAction( typeof( IncognitoSpell ) );

					BaseArmor.ValidateMobile( m_Owner );
					BaseClothing.ValidateMobile( m_Owner );

                    m_Owner.PlaySound(0x1EA);
                    m_Owner.FixedParticles(0x376A, 9, 32, 5008, Server.Items.CharacterDatabase.GetMySpellHue(m_Owner, 0), 0, EffectLayer.Waist);

                    m_Owner.PlaySound(m_Owner.Female ? 812 : 1086);
                    m_Owner.Say("*oops*");
                }
			}
		}

        private class InternalTarget : Target
        {
            private IncognitoSpell m_Owner;
            public InternalTarget(IncognitoSpell owner) : base(Core.ML ? 10 : 12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
