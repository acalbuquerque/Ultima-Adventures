using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Second
{
	public class AgilitySpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Agility", "Ex Uus",
				212,
				9061,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public AgilitySpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( CheckBSequence( m ) )
			{
				int percentage = (int)(SpellHelper.GetOffsetScalar( Caster, m, false )*100);
/*                Caster.SendMessage(55, "percentual - " + percentage);
                if (Caster.RawInt < 60)
				{
					percentage -= Utility.RandomMinMax(5, 7);
				}
				else if (Caster.RawInt >= 60 && Caster.RawInt < 80)
				{
					percentage -= Utility.RandomMinMax(3, 6);
				}
				else if (Caster.RawInt >= 80 && Caster.RawInt < 100)
                {
					percentage -= 2;
                }

				if (percentage < 0)
					percentage = 1;*/
                TimeSpan length = SpellHelper.NMSGetDuration( Caster, m, true );
                //Caster.SendMessage(35, "percentual final - " + percentage);

                SpellHelper.Turn(Caster, m);
                SpellHelper.AddStatBonus(Caster, m, StatType.Dex);
                m.FixedParticles(0x375A, 10, 15, 5010, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.Waist);
                m.PlaySound(0x1e7);
                BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Agility, 1075841, length, m, percentage.ToString() ) );
			}

			FinishSequence();
		}

		public class InternalTarget : Target
		{
			private AgilitySpell m_Owner;

			public InternalTarget( AgilitySpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
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