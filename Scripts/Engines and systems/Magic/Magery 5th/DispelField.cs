using System;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Misc;
using System.Collections.Generic;

namespace Server.Spells.Fifth
{
	public class DispelFieldSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Dispel Field", "An Grav",
				206,
				9002,
				Reagent.BlackPearl,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh,
				Reagent.Garlic
			);

		public override SpellCircle Circle { get { return SpellCircle.Fifth; } }

		public DispelFieldSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target(IPoint3D p)
		{
			if (!Caster.CanSee(p))
			{
				Caster.SendMessage(55, "O alvo não pode ser visto.");
			}
			else if (CheckSequence())
			{
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // adding fields
                List<Item> items = new List<Item>();

                Map map = Caster.Map;
                IPooledEnumerable eable = map.GetItemsInRange(new Point3D(p), 2);
                foreach (Item i in eable)
                    if (i.GetType().IsDefined(typeof(DispellableFieldAttribute), false))
                        items.Add(i);
                eable.Free();

                // Dispeling all fields in range
                foreach (Item targ in items)
                {
                    if (targ == null)
                        continue;

                    Effects.SendLocationParticles(EffectItem.Create(targ.Location, targ.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, 5042, 0);
                    Effects.PlaySound(targ.GetWorldLocation(), targ.Map, 0x201);
                    targ.Delete();

                    if (targ is Moongate && ((Moongate)targ).Dispellable)
                    {
                        Moongate gate = targ as Moongate;
                        destroyGateTargetFrom(gate);
                    }
                }
            }
            FinishSequence();
        }

        public void TargetItem( Item item )
		{
			Type t = item.GetType();

			if ( !Caster.CanSee( item ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( !t.IsDefined( typeof( DispellableFieldAttribute ), false ) )
			{
                Caster.PlaySound(Caster.Female ? 799 : 1071);
                Caster.Say("*huh?*");
                Caster.SendMessage(55, "Isso não pode ser dissipado."); 
			}
			else if ( item is Moongate && !((Moongate)item).Dispellable )
			{
                Caster.SendMessage(55, "Essa magia é muito caótica para o seu feitiço."); 
                Caster.PlaySound(Caster.Female ? 812 : 1086);
                Caster.Say("*oops*");
            }
			else if (CheckSequence() )
			{
                SpellHelper.Turn(Caster, item);

                // adding fields
                List<Item> fields = new List<Item>();
                
                Map map = item.Map;
                IPooledEnumerable eable = map.GetItemsInRange(new Point3D(item.GetWorldLocation()), 2);
                foreach (Item i in eable)
                    if (i.GetType().IsDefined(typeof(DispellableFieldAttribute), false))
                        fields.Add(i);
                eable.Free();

                // Dispeling all fields in range
                foreach (Item targ in fields)
                {
                    if (targ == null)
                        continue;

                    Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, 5042, 0);
                    Effects.PlaySound(targ.GetWorldLocation(), targ.Map, 0x201);
                    targ.Delete();

					if (item is Moongate && ((Moongate)item).Dispellable) 
					{
						Moongate gate = item as Moongate;
                        destroyGateTargetFrom(gate);
                    }
                }
			}

			FinishSequence();
		}

		private void destroyGateTargetFrom(Moongate gate) {
            List<Item> gatesDest = new List<Item>();
            Point3D m_MoonDest = gate.Target;
            IPooledEnumerable neable = gate.TargetMap.GetItemsInRange(m_MoonDest, 0);

            foreach (Item i in neable)
                if (i.GetType().IsDefined(typeof(DispellableFieldAttribute), false))
                    gatesDest.Add(i);
            neable.Free();

            foreach (Item mgDest in gatesDest)
            {
                if (mgDest != null)
                {
                    mgDest.Delete();
                }
            }
        }

		private class InternalTarget : Target
		{
			private DispelFieldSpell m_Owner;

			public InternalTarget( DispelFieldSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if (o is Item)
				{
					m_Owner.TargetItem((Item)o);
				}
				else if (o is IPoint3D) 
				{
                    IPoint3D p = o as IPoint3D;

                    if (p != null)
                        m_Owner.Target(p);
                }
				else
				{
					m_Owner.Caster.SendLocalizedMessage(1005049); // That cannot be dispelled.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}