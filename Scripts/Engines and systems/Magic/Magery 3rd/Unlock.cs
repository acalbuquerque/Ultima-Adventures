using System;
using Server.Targeting;
using Server.Network;
using Server.Items;

namespace Server.Spells.Third
{
	public class UnlockSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Unlock Spell", "Ex Por",
				215,
				9001,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Third; } }

		public UnlockSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		private class InternalTarget : Target
		{
			private UnlockSpell m_Owner;

			public InternalTarget( UnlockSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D loc = o as IPoint3D;

				if ( loc == null )
					return;

				if ( m_Owner.CheckSequence() ) {
					SpellHelper.Turn( from, o );

					Effects.SendLocationParticles( EffectItem.Create( new Point3D( loc ), from.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, Server.Items.CharacterDatabase.GetMySpellHue( from, 0 ), 0, 5024, 0 );

					Effects.PlaySound( loc, from.Map, 0x1FF );

					if ( o is Mobile )
					{
                        from.PlaySound(from.Female ? 812 : 1086);
                        from.Say("*oops*");
                        from.SendMessage(55, "Você não tem o que destrancar.");
                        //from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
					}
					else if ( o is BaseHouseDoor )  // house door check
					{
                        from.PlaySound(from.Female ? 812 : 1086);
                        from.Say("*oops*");
                        from.SendMessage(55, "Este feitiço é para destrancar baús, caixas, cofres e algumas portas.");
					}
					else if ( o is BookBox )  // cursed box of books
					{
                        from.PlaySound(from.Female ? 812 : 1086);
                        from.Say("*oops*");
                        from.SendMessage(55, "Este feitiço nunca será capaz de destrancar uma caixa amaldiçoada.");
					}
					else if ( o is UnidentifiedArtifact || o is UnidentifiedItem || o is CurseItem )
					{
                        from.PlaySound(from.Female ? 812 : 1086);
                        from.Say("*oops*");
                        from.SendMessage(55, "Este feitiço não serve para destrancar isto.");
					}
					else if ( o is BaseDoor )
					{
						if (Server.Items.DoorType.IsDungeonDoor((BaseDoor)o))
						{
							if (((BaseDoor)o).Locked == false)
							{
								//from.SendMessage(55, "Isso não precisava ser desbloqueado.");
								from.PlaySound(from.Female ? 812 : 1086);
								from.Say("*oops*");
								from.LocalOverheadMessage(MessageType.Regular, 55, false, "Isso não precisava ser destrancado."); // That did not need to be unlocked.
							}
							else
							{
								if (((BaseDoor)o).KeyValue <= 65 && Utility.RandomDouble() < (double)((100 - ((BaseDoor)o).KeyValue) / 100))
								{
									((BaseDoor)o).Locked = false;
									((BaseDoor)o).KeyValue = 0;
									Server.Items.DoorType.UnlockDoors((BaseDoor)o);
									from.SendMessage(2253, "Você ouve a fechadura abrir.");
									from.PlaySound(from.Female ? 779 : 1050);
									from.Say("*ah ha!*");
								}
								else
								{
									from.SendMessage(55, "A fechadura é muito complexa para este feitiço.");
									from.PlaySound(from.Female ? 811 : 1085);
									from.Say("*oooh*");
								}

							}
						}
						else {
                            from.PlaySound(from.Female ? 812 : 1086);
                            from.Say("*oops*");
                            from.LocalOverheadMessage(MessageType.Regular, 55, false, "Isso não precisava ser destrancado.");
                            //from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
                        }
                    }
					else if ( !( o is LockableContainer ) )
					{
                        from.SendMessage(55, "Você não pode destrancar isso!");
                        from.PlaySound(from.Female ? 812 : 1086);
                        from.Say("*oops*");
                        //from.SendLocalizedMessage( 501666 ); // You can't unlock that!
                    }
					else {
						LockableContainer cont = (LockableContainer)o;

						if (Multis.BaseHouse.CheckSecured(cont))
						{
                            from.SendMessage(55, "Você não pode usar magia em um item seguro.");
                            from.PlaySound(from.Female ? 811 : 1085);
                            from.Say("*oooh*");
						}
						else if (!cont.Locked || cont.LockLevel == 0) 
						{
                            from.PlaySound(from.Female ? 812 : 1086);
                            from.Say("*oops*");
                            from.SendMessage(55, "Este baú não parece estar trancado ou não possuir um nível de trava.");
                            from.LocalOverheadMessage(MessageType.Regular, 55, false, "* Aff! Isso parece já estar destrancado! *");
                        }
                        else if ( this is TreasureMapChest || this is ParagonChest || this is PirateChest)
						{
                            from.PlaySound(from.Female ? 778 : 1049);
                            from.Say("*ah!*");
                            from.SendMessage(55, "Uma forte aura mágica neste baú impede o funcionamento do seu feitiço mas talvez um ladrão possa abri-lo");
						}
						else {
                            // Max lvl gonna be 76 (120x120 supers), SO THIEF (lockpick) IS SPECIAL AND IS THE ONLY ABLE THE UNLOCK +75 lvl locked container
                            int level = (int)NMSUtils.getBeneficialMageryInscribePercentage(from) + 20; 

                            if (level >= cont.RequiredSkill && !(cont is TreasureMapChest && ((TreasureMapChest)cont).Level > 2))
							{
								cont.Locked = false;
								from.SendMessage(2253, "Você ouve a fechadura abrir.");
								from.PlaySound(from.Female ? 783 : 1054);
								from.Say("*woohoo!*");

								if (cont.LockLevel == -255)
									cont.LockLevel = cont.RequiredSkill - 10;
							}
							else 
							{
                                from.PlaySound(from.Female ? 799 : 1071);
                                from.Say("*huh?*");
                                from.SendMessage(55, "Esta fechadura parece ser muito complexa para o seu feitiço.");
                                //from.LocalOverheadMessage(MessageType.Regular, 55, false, "Esta fechadura parece ser muito complexa para o seu feitiço."); // My spell does not seem to have an effect on that lock.
                            }
						}		
					}
				}

				m_Owner.FinishSequence();
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}