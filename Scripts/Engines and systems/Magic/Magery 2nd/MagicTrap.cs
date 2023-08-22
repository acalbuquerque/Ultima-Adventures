using System;
using Server; 
using Server.Targeting;
using Server.Network;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Misc;

namespace Server.Spells.Second
{
	public class MagicTrapSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Magic Trap", "In Jux",
				212,
				9001,
				Reagent.Garlic,
				Reagent.SpidersSilk,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public MagicTrapSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( TrapableContainer item )
		{
			if ( !Caster.CanSee( item ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( item.TrapType != TrapType.None && item.TrapType != TrapType.MagicTrap )
			{
				base.DoFizzle();
			}
			else if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, item );

				item.TrapType = TrapType.MagicTrap;
				//item.TrapPower = Core.AOS ? Utility.RandomMinMax( 10, 50 ) : 1;
				double power = (int)(Caster.Skills[SkillName.Magery].Value) / 3;
				int powerDamage = (int)Math.Floor((power * NMSUtils.getDamageEvalBenefit(Caster)) / 3) + 1;

                item.TrapPower = powerDamage;
				item.TrapLevel = (int)(Caster.Skills[SkillName.Magery].Value / 3);

                if ( item.TrapLevel > 60 ){ item.TrapLevel = 60; }
				if ( item.TrapLevel < 0 ){ item.TrapLevel = 0; }

				Point3D loc = item.GetWorldLocation();

				Effects.SendLocationParticles( EffectItem.Create( new Point3D( loc.X + 1, loc.Y, loc.Z ), item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 9502 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( loc.X, loc.Y - 1, loc.Z ), item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 9502 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( loc.X - 1, loc.Y, loc.Z ), item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 9502 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( loc.X, loc.Y + 1, loc.Z ), item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 9502 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( loc.X, loc.Y,     loc.Z ), item.Map, EffectItem.DefaultDuration ), 0, 0, 0, 5014 );

				Effects.PlaySound( loc, item.Map, 0x1EF );
			}

			FinishSequence();
		}

		public void MTarget( IPoint3D p )
		{
            Point3D loc = new Point3D(p.X, p.Y, p.Z);

			if (!Caster.CanSee(p))
			{
				Caster.SendMessage(55, "O alvo não pode ser visto.");
			}
			else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				int traps = 0;

				foreach (Item m in Caster.GetItemsInRange(10))
				{
					if (m is SpellTrap)
						++traps;
				}

				if (traps > 2)
				{
					Caster.SendMessage(55, "Existem muitas armadilhas mágicas na área!");
				}
				else if (!Caster.Region.AllowHarmful(Caster, Caster))
				{
					Caster.SendMessage(55, "Não parece ser uma boa ideia fazer isso aqui.");
					return;
				}
				else
				{ // Runas no chao
					SpellHelper.Turn(Caster, p);
					SpellHelper.GetSurfaceTop(ref p);

					NMSUtils.makeCriminalAction(Caster, true);

                    double power = (int)(Caster.Skills[SkillName.Magery].Value) / 3;
					int powerDamage = (int)Math.Floor((power * NMSUtils.getDamageEvalBenefit(Caster)) / 5) + 1;
					int TrapPower = powerDamage;
					SpellTrap mtrap = new SpellTrap(Caster, TrapPower);
					mtrap.Map = Caster.Map;
					mtrap.Location = loc;

					Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 10, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, 9502, 0);
					Effects.PlaySound(loc, Caster.Map, 0x1EF);
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private MagicTrapSpell m_Owner;

			public InternalTarget( MagicTrapSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is TrapableContainer )
				{
					m_Owner.Target( (TrapableContainer)o );
				}
				else if ( o is IPoint3D )
				{
					m_Owner.MTarget( (IPoint3D)o );
				}
				else
				{
					from.SendMessage(55, "Você não pode criar uma armadilha aqui." );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}