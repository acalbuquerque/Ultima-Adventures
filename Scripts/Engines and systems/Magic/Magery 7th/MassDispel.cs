using System;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Seventh
{
	public class MassDispelSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mass Dispel", "Vas An Ort",
				263,
				9002,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.BlackPearl,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		public MassDispelSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( IPoint3D p )
		{
			if ( !Caster.CanSee( p ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( CheckSequence() )
			{
                int caosMomentum = Utility.RandomMinMax(0, 10);
                SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				List<Mobile> targets = new List<Mobile>();
                List<Item> fields = new List<Item>();

                Map map = Caster.Map;
				// Mapping all targets and fields
				if ( map != null )
				{
					IPooledEnumerable eable = map.GetMobilesInRange( new Point3D( p ), 8 );

                    foreach ( Mobile m in eable )
					{
						if ( m is BaseCreature )
						{
							BaseCreature mn = m as BaseCreature;
							if ( mn.ControlSlots == 666 )
								targets.Add( m );
						}

						if ( m is BaseCreature && (m as BaseCreature).IsDispellable && Caster.CanBeHarmful( m, false ) )
							targets.Add( m );
					}

					eable.Free();

                    eable = map.GetItemsInRange(new Point3D(p), 4);
                    foreach (Item i in eable)
                        if (i.GetType().IsDefined(typeof(DispellableFieldAttribute), false))
                            fields.Add(i);
                    eable.Free();
                }

				// Dispeling all fields in range
                foreach (Item targ in fields)
                {
                    if (targ == null)
                        continue;

                    Effects.SendLocationParticles(EffectItem.Create(targ.Location, targ.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, 5042);
                    Effects.PlaySound(targ.GetWorldLocation(), targ.Map, 0x201);
                    targ.Delete();
                }

				// Dispeling all targets in range
                foreach (Mobile m in targets)
				{
                    BaseCreature bc = m as BaseCreature;
                    if (bc == null)
                        continue;

                    double percentageLimit = NMSUtils.getSummonDispelPercentage(bc, caosMomentum);
                    double dispelChance =  NMSUtils.getDispelChance(Caster, bc, caosMomentum);

                    if (bc == null || !bc.IsDispellable || (bc is BaseVendor) || bc is PlayerMobile)
                    {
                        Caster.SendMessage(55, "Não é possível dissipar " + m.Name);
                        //from.SendLocalizedMessage( 1005049 ); // That cannot be dispelled.
                    }
					else 
					{
                        if (dispelChance >= percentageLimit)
                        {
                            Caster.SendMessage(55, bc.Name + " foi dissipado!");
                            Effects.SendLocationParticles(EffectItem.Create(bc.Location, bc.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, 5042, 0);
                            Effects.PlaySound(bc, bc.Map, 0x201);
                            bc.Delete();
                        }
                        else
                        {
                            if (bc.Hits < bc.HitsMax * 0.2) // if creature health is less than 20%
                            {
                                if (dispelChance >= (bc.Hits / 10))
                                {
                                    Caster.SendMessage(20, bc.Name + " já estava sem forças e foi dissipado!");
                                    Effects.SendLocationParticles(EffectItem.Create(bc.Location, bc.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, 5042, 0);
                                    Effects.PlaySound(bc, bc.Map, 0x201);
                                    bc.Delete();
                                }
                            }
                            else
                            {
                                Caster.DoHarmful(bc);
                                bc.FixedEffect(0x3779, 10, 20, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0);
                                Caster.SendMessage(33, "Você conseguiu irritar " + bc.Name);
                            }

                        }
                    }
				}
			}

			FinishSequence();
		}

		private void dispelTarget() {
		
		}

		private class InternalTarget : Target
		{
			private MassDispelSpell m_Owner;

			public InternalTarget( MassDispelSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
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