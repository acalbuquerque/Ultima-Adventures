using System;
using System.Collections;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Second
{
	public class ProtectionSpell : MagerySpell
	{
		private static Hashtable m_Registry = new Hashtable();
		public static Hashtable Registry { get { return m_Registry; } }

		private static SpellInfo m_Info = new SpellInfo(
				"Protection", "Uus Sanct",
				236,
				9011,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public ProtectionSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast(Mobile caster)
		{
			if ( Core.AOS )
				return true;

			if ( m_Registry.ContainsKey( Caster ) )
			{
                Caster.SendMessage(55, "O alvo já está sob efeito do feitiço.");
                //Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( DefensiveSpell ) ) )
			{
                Caster.SendMessage(55, "O feitiço não pode ser aplicado nesse momento.");
                //Caster.SendLocalizedMessage( 1005385 ); // The spell will not adhere to you at this time.
				return false;
			}

			return true;
		}

		private static Hashtable m_Table = new Hashtable();

		public static void Toggle( Mobile caster, Mobile target ) // TODO: COOP3R - Remove part of this nonsense
		{
			/* Players under the protection spell effect can no longer have their spells "disrupted" when hit.
			 * Players under the protection spell have decreased physical resistance stat value (-15 + (Inscription/20),
			 * a decreased "resisting spells" skill value by -35 + (Inscription/20),
			 * and a slower casting speed modifier (technically, a negative "faster cast speed") of 2 points.
			 * The protection spell has an indefinite duration, becoming active when cast, and deactivated when re-cast.
			 * Reactive Armor, Protection, and Magic Reflection will stay on�even after logging out,
			 * even after dying�until you �turn them off� by casting them again.
			 */

			//object[] mods = (object[])m_Table[target];
            ResistanceMod[] mods = (ResistanceMod[])m_Table[target];
            if ( mods == null )
			{
                target.SendMessage("Add Prot");
                target.PlaySound( 0x1E9 );
				target.FixedParticles( 0x375A, 9, 20, 5016, Server.Items.CharacterDatabase.GetMySpellHue( caster, 0 ), 0, EffectLayer.Waist );

                /*				mods = new object[2]
                                    {
                                                        new ResistanceMod( ResistanceType.Physical, -15 + Math.Min( (int)(caster.Skills[SkillName.Inscribe].Value / 20), 15 ) ),
                                                        new DefaultSkillMod( SkillName.MagicResist, true, -35 + Math.Min( (int)(caster.Skills[SkillName.Inscribe].Value / 20), 35 ) )
                                    };*/

                mods = new ResistanceMod[5]
                            {
                                new ResistanceMod( ResistanceType.Physical, -8 ),
                                new ResistanceMod( ResistanceType.Fire, -8 ),
                                new ResistanceMod( ResistanceType.Cold, -8 ),
                                new ResistanceMod( ResistanceType.Poison, -8 ),
                                new ResistanceMod( ResistanceType.Energy, -8 )
                            };

                m_Table[target] = mods;

                for (int i = 0; i < mods.Length; ++i)
                    target.AddResistanceMod(mods[i]);
                
				string args = "";
                args = String.Format("{0}\t{1}\t{2}\t{3}\t{4}", 8, 8, 8, 8, 8);

                /*                m_Table[target] = mods;
                                Registry[target] = 100.0;

                                target.AddResistanceMod((ResistanceMod)mods[0]);*/
                //target.AddSkillMod((SkillMod)mods[1]);

                //int physloss = -15 + (int)(caster.Skills[SkillName.Inscribe].Value / 20);
                //int resistloss = -35 + (int)(caster.Skills[SkillName.Inscribe].Value / 20);
                //string args = String.Format("{0}", physloss); // String.Format("{0}\t{1}", physloss, resistloss);
                TimeSpan length = SpellHelper.NMSGetDuration(caster, target, true);
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Protection, 1075814, length, target, args.ToString()));
                //BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Protection, 1075814, 1075815, args.ToString()));

			}
			else
			{
                target.SendMessage("Remove Prot");
                target.PlaySound( 0x1ED );
				target.FixedParticles( 0x375A, 9, 20, 5016, Server.Items.CharacterDatabase.GetMySpellHue( caster, 0 ), 0, EffectLayer.Waist );

				m_Table.Remove(target);
				//Registry.Remove(target);

				//target.RemoveResistanceMod((ResistanceMod)mods[0]);
                //target.RemoveSkillMod((SkillMod)mods[1]);

                for(int i = 0; i < mods.Length; ++i )
                    target.RemoveResistanceMod(mods[i]);

                BuffInfo.RemoveBuff(target, BuffIcon.Protection);
			}
		}

		public static void EndProtection( Mobile m )
		{
			if ( m_Table.Contains( m ) )
			{
				object[] mods = (object[]) m_Table[ m ];

				m_Table.Remove( m );
				Registry.Remove( m );

				m.RemoveResistanceMod( (ResistanceMod) mods[ 0 ] );
                //m.RemoveSkillMod( (SkillMod) mods[ 1 ] );
                m.PlaySound(0x1ED);
                BuffInfo.RemoveBuff( m, BuffIcon.Protection );
			}
		}

		public override void OnCast()
		{
			if ( Core.AOS )
			{
				if ( CheckSequence() )
					Toggle( Caster, Caster );

				FinishSequence();
			}
			else
			{
				if ( m_Registry.ContainsKey( Caster ) )
				{
                    Caster.SendMessage(55, "O alvo já está sob efeito do feitiço.");//Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
                }
				else if ( !Caster.CanBeginAction( typeof( DefensiveSpell ) ) )
				{
                    Caster.SendMessage(55, "O feitiço não pode ser aplicado nesse momento.");//Caster.SendLocalizedMessage( 1005385 ); // The spell will not adhere to you at this time.
                }
				else if ( CheckSequence() )
				{
					if ( Caster.BeginAction( typeof( DefensiveSpell ) ) )
					{
						double value = (int)(Caster.Skills[SkillName.Inscribe].Value); // inscript give benefits
                        value /= 4;

						if ( value < 0 )
							value = 0;
						else if ( value > 30 )
							value = 30.0;

						Registry.Add( Caster, value );
						new InternalTimer( Caster ).Start();

						Caster.FixedParticles( 0x375A, 9, 20, 5016, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, EffectLayer.Waist );
						Caster.PlaySound( 0x1ED );
					}
					else
					{
                        Caster.SendMessage(55, "O feitiço não pode ser aplicado nesse momento.");//Caster.SendLocalizedMessage( 1005385 ); // The spell will not adhere to you at this time.
                    }
				}

				FinishSequence();
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Caster;

			public InternalTimer( Mobile caster ) : base( TimeSpan.FromSeconds( 0 ) )
			{
                double val = caster.Skills[SkillName.Magery].Value * 2.0;
				if ( val < 15 ) // min secs
					val = 15;
				else if ( val > 30 )
					val = 30; // max secs

				m_Caster = caster;
				Delay = TimeSpan.FromSeconds( val );
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
                m_Caster.SendMessage("Remove Prottt");
                ProtectionSpell.Registry.Remove( m_Caster );
				DefensiveSpell.Nullify( m_Caster );
			}
		}
	}
}
