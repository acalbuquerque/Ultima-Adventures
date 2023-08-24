using System;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells.First
{
	public class HealSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Heal", "In Mani",
				224,
				9061,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.SpidersSilk
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public HealSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
                Caster.SendMessage(55, "O alvo n�o pode ser visto.");
            }
			else if ( m.IsDeadBondedPet || m is BaseCreature && ((BaseCreature)m).IsAnimatedDead)
			{
                Caster.SendMessage(55, "Voc� n�o pode curar aquilo que j� est� morto.");
                //Caster.SendLocalizedMessage( 1060177 ); // You cannot heal a creature that is already dead!
			}
			else if ( m is PlayerMobile && m.FindItemOnLayer( Layer.Ring ) != null && m.FindItemOnLayer( Layer.Ring ) is OneRing)
			{
				Caster.SendMessage(33, "O UM ANEL desfez o feiti�o e te diz para n�o fazer isso... " );
                DoFizzle();
                return;
			}
			else if ( m is Golem )
			{
                DoFizzle();
                Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "* N�o sei como curar isso *"); // You cannot heal that.
			}
			else if ( (m.Poisoned && m.Poison.Level >= 3) || Server.Items.MortalStrike.IsWounded( m ) )
			{
                Caster.SendMessage(33, ((Caster == m) ? "Voc� sente o veneno penetrar mais em suas veias." : "O seu alvo est� mortalmente envenenado e n�o poder� ser curado com esse feiti�o!"));
                //Caster.LocalOverheadMessage( MessageType.Regular, 0x22, (Caster == m) ? 1005000 : 1010398 );
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				int toHeal = (int)NMSUtils.getBeneficialMageryInscribePercentage(Caster) / 3;

				/*toHeal = Caster.Skills.Magery.Fixed / 120;
				toHeal += Utility.RandomMinMax( 1, 4 );
				toHeal = Server.Misc.MyServerSettings.PlayerLevelMod( toHeal, Caster );*/

                if ( Caster != m )
					toHeal = (int)(toHeal * 1.2); // 20% more heal points if is another person.

				//m.Heal( toHeal, Caster );
				SpellHelper.Heal( toHeal, m, Caster );

				m.FixedParticles( 0x376A, 9, 32, 5005, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, EffectLayer.Waist );
				m.PlaySound( 0x1F2 );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private HealSpell m_Owner;

			public InternalTarget( HealSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}