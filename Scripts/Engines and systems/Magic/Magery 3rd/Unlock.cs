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
                        from.SendMessage(55, "Voc� n�o tem o que destrancar.");
                        //from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
					}
					else if ( o is BaseHouseDoor )  // house door check
					{
						from.SendMessage(55, "Este feiti�o � para destravar certos recipientes e outros tipos de portas.");
					}
					else if ( o is BookBox )  // cursed box of books
					{
						from.SendMessage(55, "Este feiti�o nunca pode desbloquear esta caixa amaldi�oada.");
					}
					else if ( o is UnidentifiedArtifact || o is UnidentifiedItem || o is CurseItem )
					{
						from.SendMessage(55, "Este feiti�o � usado para desbloquear qualquer cont�iner.");
					}
					else if ( o is BaseDoor )
					{
						if ( Server.Items.DoorType.IsDungeonDoor( (BaseDoor)o ) )
						{
							if (((BaseDoor)o).Locked == false) {
                                //from.SendMessage(55, "Isso n�o precisava ser desbloqueado.");
                                from.LocalOverheadMessage(MessageType.Regular, 55, false, "Isso n�o precisava ser desbloqueado."); // That did not need to be unlocked.
                            }
							else
							{
								if (((BaseDoor)o).KeyValue <= 65 && Utility.RandomDouble() < (double)( (100- ((BaseDoor)o).KeyValue) / 100) )
								{	
									((BaseDoor)o).Locked = false;
									((BaseDoor)o).KeyValue = 0;
									Server.Items.DoorType.UnlockDoors( (BaseDoor)o );
                                    from.SendMessage(2253, "Voc� ouve a fechadura abrir.");
                                }
								else 
									from.SendMessage(55, "A fechadura � muito complexa para um feiti�o simples.");
							}
						}
						else
                            from.LocalOverheadMessage(MessageType.Regular, 55, false, "Isso n�o precisava ser desbloqueado.");
                        //from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
					}
					else if ( !( o is LockableContainer ) )
					{
                        from.SendMessage(55, "Voc� n�o pode desbloquear isso!");
                        //from.SendLocalizedMessage( 501666 ); // You can't unlock that!
					}
					else {
						LockableContainer cont = (LockableContainer)o;

						if ( Multis.BaseHouse.CheckSecured( cont ) ) 
							from.SendLocalizedMessage( 55, "Voc� n�o pode lan�ar isso em um item seguro."); // You cannot cast this on a secure item.
						else if ( !cont.Locked )
                            from.LocalOverheadMessage(MessageType.Regular, 55, false, "Isso n�o precisava ser desbloqueado.");
                        else if ( cont.LockLevel == 0 )
                            from.SendMessage(55, "Voc� n�o pode desbloquear isso!");
                        else if ( this is TreasureMapChest || this is ParagonChest )
						{
							from.SendMessage( 55, "Uma aura m�gica neste ba� de tesouro impede o funcionamento do seu feiti�o." );
						}
						else if ( this is PirateChest )
						{
							from.SendMessage(55, "Isso parece estar protegido da magia, mas talvez um ladr�o possa abri-lo.");
						}
						else {
							int level = (int)(from.Skills[SkillName.Magery].Value) + 20; // WIZARD CHANGED FOR WANDS AND SUCH

							if ( level > 50 ){ level = 50; } // WIZARD ADDED FOR A MAXIMUM SO THIEF IS SPECIAL

							if ( level >= cont.RequiredSkill && !(cont is TreasureMapChest && ((TreasureMapChest)cont).Level > 2) ) {
								cont.Locked = false;
                                from.SendMessage(2253, "Voc� ouve a fechadura abrir.");
                                if ( cont.LockLevel == -255 )
									cont.LockLevel = cont.RequiredSkill - 10;
							}
							else
								from.LocalOverheadMessage( MessageType.Regular, 55, false, "Meu feiti�o n�o parece ter efeito nessa fechadura."); // My spell does not seem to have an effect on that lock.
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