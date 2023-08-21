using System;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Sixth
{
	public class DispelSpell : MagerySpell
	{

        private static SpellInfo m_Info = new SpellInfo(
				"Dispel", "An Ort",
				218,
				9002,
				Reagent.Garlic,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public DispelSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public class InternalTarget : Target
		{
			private DispelSpell m_Owner;

			public InternalTarget( DispelSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile && o is BaseCreature )
				{
					Mobile m = (Mobile)o;
					BaseCreature bc = m as BaseCreature;

                    int caosMomentum = Utility.RandomMinMax(0, 10);
                    double percentageLimit = NMSUtils.getSummonDispelPercentage(bc, caosMomentum);
                    double dispelChance = NMSUtils.getDispelChance(from, bc, caosMomentum);

                    if (!from.CanSee(m))
                    {
                        from.SendMessage(55, "O alvo não pode ser visto.");
                    }
                    else if (bc == null || !bc.IsDispellable || (bc is BaseVendor) || o is PlayerMobile)
                    {
                        from.SendMessage(55, "Não é possível dissipar " + m.Name);
                        //from.SendLocalizedMessage( 1005049 ); // That cannot be dispelled.
                    }
                    else if (bc.IsDispellable || bc.ControlSlots == 666 || m_Owner.CheckHSequence(m) ) // FOR SPECIAL SUMMONED WIZARD CREATURES
                    {
                        SpellHelper.Turn(from, m);

                        if (dispelChance >= percentageLimit)
                        {
                            Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, Server.Items.CharacterDatabase.GetMySpellHue(from, 0), 0, 5042, 0);
                            Effects.PlaySound(m, m.Map, 0x201);

                            m.Delete();
                        }
                        else
                        {
                            if (bc.Hits < bc.HitsMax * 0.2) // if creature health is less than 20%
                            {
                                if (dispelChance >= (bc.Hits / 10))
                                {
                                    from.SendMessage(20, bc.Name + " já estava sem forças e foi dissipado!");
                                    Effects.SendLocationParticles(EffectItem.Create(bc.Location, bc.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, Server.Items.CharacterDatabase.GetMySpellHue(from, 0), 0, 5042, 0);
                                    Effects.PlaySound(bc, bc.Map, 0x201);
                                    bc.Delete();
                                }
                            }
                            else
                            {
                                from.DoHarmful(bc);
                                bc.FixedEffect(0x3779, 10, 20, Server.Items.CharacterDatabase.GetMySpellHue(from, 0), 0);
                                from.SendMessage(33, "Você conseguiu irritar " + bc.Name);
                            }
                        }
                    }
                    else
                    {
                        from.SendMessage(55, "Algo de estranho aconteceu neste feitiço.");
                    }

                }
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}