using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Misc;

namespace Server.Spells.Sixth
{
	public class MarkSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Mark", "Kal Por Ylem",
				218,
				9002,
				Reagent.BlackPearl,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

		public MarkSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public override bool CheckCast(Mobile caster)
		{
			if ( !base.CheckCast( caster ) )
				return false;

			return SpellHelper.CheckTravel( Caster, TravelCheckType.Mark );
		}

		public void Target( RecallRune rune )
		{
			Region reg = Region.Find( Caster.Location, Caster.Map );
					
			if ( !Caster.CanSee( rune ) )
			{
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
			else if ( reg.IsPartOf( typeof( PirateRegion ) ) )
			{
                Caster.SendMessage(55, "Estas águas são muito agitadas para lançar este feitiço.");
			}
			else if ( Worlds.RegionAllowedTeleport( Caster.Map, Caster.Location, Caster.X, Caster.Y ) == false || !SpellHelper.CheckTravel(Caster, TravelCheckType.Mark))
			{
				Caster.SendMessage(55, "Esse feitiço parece não funcionar neste lugar.");
			}
			else if ( SpellHelper.CheckMulti( Caster.Location, Caster.Map, !Core.AOS ) )
			{
                Caster.SendMessage(55, "Esse local está bloqueado para lançcar este feitiço.");
			}
			else if ( !rune.IsChildOf( Caster.Backpack ) )
			{
                Caster.PlaySound(Caster.Female ? 812 : 1086);
                Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "*Oops*");
                Caster.SendMessage(55, "Você deve ter esta runa em sua mochila para marcá-la.");
            }
			else if ( CheckSequence() )
			{
				rune.Mark( Caster );

				Caster.PlaySound( 0x1FA );
				Effects.SendLocationEffect( Caster, Caster.Map, 14201, 16, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private MarkSpell m_Owner;

			public InternalTarget( MarkSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					m_Owner.Target( (RecallRune) o );
				}
				else
				{
                    from.SendMessage(55, "Você deve ter utilizar uma runa em sua mochila para utilizar este feitiço.");
                    //from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501797, from.Name, "" ) ); // I cannot mark that object.
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}