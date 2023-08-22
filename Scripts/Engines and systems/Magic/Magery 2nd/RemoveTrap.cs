using System;
using Server.Targeting;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using System.Collections;

namespace Server.Spells.Second
{
	public class RemoveTrapSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Remove Trap", "An Jux",
				212,
				9001,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public RemoveTrapSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( 95, "Selecione uma armadilha ou você mesmo para invocar um amuleto de proteção." );
		}

		public void Target( TrapableContainer item )
		{
			if ( !Caster.CanSee( item ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( CheckSequence() )
			{
				int mageLvl = (int)((Caster.Skills[SkillName.Magery].Value) / 3) - Utility.RandomMinMax(0, 1); // 50% chance to be weaker than the top
                Point3D loc = item.GetWorldLocation();
                if ( mageLvl >= item.TrapLevel)
				{
					Effects.SendLocationParticles( EffectItem.Create( loc, item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, 5015, 0 );
					Effects.PlaySound( loc, item.Map, 0x1F0 );
                    Effects.PlaySound(loc, item.Map, 61);

                    Caster.SendMessage(55, "Todas as armadilhas aqui foram desativadas.");

					item.TrapType = TrapType.None;
					item.TrapPower = 0;
					item.TrapLevel = 0;
				}
				else
				{
                    Caster.LocalOverheadMessage(MessageType.Emote, 55, true, "* aff! *");
                    Caster.SendMessage(55, "Essa armadilha parece complicada demais para ser desfeita por sua magia.");
                    Effects.PlaySound(loc, item.Map, 10);
                    //base.DoFizzle();
                }
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private RemoveTrapSpell m_Owner;

			public InternalTarget( RemoveTrapSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is TrapableContainer )
				{
					m_Owner.Target( (TrapableContainer)o );
				}
				else if ( from == o )
				{
					if ( m_Owner.CheckSequence() )
					{
						ArrayList targets = new ArrayList();
						foreach ( Item item in World.Items.Values )
						if ( item is TrapWand )
						{
							TrapWand myWand = (TrapWand)item;
							if ( myWand.owner == from )
							{
								targets.Add( item );
							}
						}
						for ( int i = 0; i < targets.Count; ++i )
						{
							Item item = ( Item )targets[ i ];
							item.Delete();
						}

						from.PlaySound( 0x1ED );
						from.FixedParticles( 0x376A, 9, 32, 5008, Server.Items.CharacterDatabase.GetMySpellHue( from, 0 ), 0, EffectLayer.Waist );
						from.SendMessage(2253, "Você invoca um orbe mágico em sua mochila.");
						Item iWand = new TrapWand(from);
						int nPower = (int)(NMSUtils.getBeneficialMageryInscribePercentage(from) + 30);//(int)(from.Skills[SkillName.Magery].Value / 2 ) + 25;

                        if (nPower > 75){nPower = 75;}
						TrapWand xWand = (TrapWand)iWand;
						xWand.WandPower = nPower;
						from.AddToBackpack( xWand );
					}
					m_Owner.FinishSequence();
				}
				else
				{
					from.SendMessage( 55, "Este feitiço não tem efeito sobre isso!");
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}