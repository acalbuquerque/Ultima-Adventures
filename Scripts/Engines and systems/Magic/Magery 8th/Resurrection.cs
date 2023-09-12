using System;
using Server.Targeting;
using Server.Network;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.Spells.Eighth
{
    public class ResurrectionSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Resurrection", "An Corp",
                245,
                9062,
                Reagent.Bloodmoss,
                Reagent.Garlic,
                Reagent.Ginseng
            );
 
        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
 
        public ResurrectionSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
        {
        }
 
        public override void OnCast()
        {
            Caster.Target = new InternalTarget( this );
        }
 
        public void Target( Mobile m )
        {
			if ( m == null || Caster == null )
			return;

            if ( !Caster.CanSee( m ) )
            {
                Caster.SendMessage(55, "O alvo não pode ser visto.");
            }
            else if ( m == Caster && CheckBSequence( m, true ) )
            {

                if ( m is PlayerMobile && ((PlayerMobile)m).SoulBound )
                {
                    Caster.SendMessage(55, "Este feitiço não funciona em você mesmo.");
                    return; // no orbs for soulbound players
                }

				if (m.Backpack == null)
					return;

				ArrayList targets = new ArrayList();
				foreach ( Item item in m.Backpack.Items )
					if ( item is SoulOrb )
					{
						SoulOrb myOrb = (SoulOrb)item;
						if ( myOrb.m_Owner == m )
						{
						targets.Add( item );
						}
					}

				for ( int i = 0; i < targets.Count; ++i )
				{
					Item item = ( Item )targets[ i ];
					item.Delete();
				}

				m.PlaySound( 0x214 );
				m.FixedEffect( 0x376A, 10, 16, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );
				m.SendMessage(55, "Você invoca um orbe mágico para proteger sua alma.");
				SoulOrb iOrb = new SoulOrb();
				iOrb.m_Owner = m;
				m.AddToBackpack( iOrb );
				Server.Items.SoulOrb.OnSummoned( m, iOrb );
            }
            else if ( !Caster.Alive )
            {
                Caster.SendMessage(55, "Você deve estar vivo para usar este feitiço.");
            }
            else if (m is PlayerMobile && m.Alive)
            {
                Caster.SendMessage(55, "O alvo não está morto para ser ressuscitado.");
            }
            else if ( !Caster.InRange( m, 4 ) )
            {
                Caster.SendMessage(55, "O alvo precisa estar mais próximo para ser ressuscitado.");
            }
            else if ( m.Map == null || !m.Map.CanFit( m.Location, 16, false, false ) )
            {
                Caster.SendMessage(55, "O alvo não pode ser ressuscitado neste local.");
                m.SendMessage(55, "Você não pode ser ressuscitado aqui!");
            }
            else if ( m is PlayerMobile && CheckBSequence( m, true ) )
            {

                if ( m is PlayerMobile && ((PlayerMobile)m).SoulBound )
                {
                    ((PlayerMobile)m).ResetPlayer( m );
					Caster.SendMessage(55, "Essa pessoa escolheu levar uma vida difícil. (SoulBound)");
                    FinishSequence();
                    return;
                }
                SpellHelper.Turn( Caster, m );
 
                m.PlaySound( 0x214 );
                m.FixedEffect( 0x376A, 10, 16, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );
 
                m.CloseGump( typeof( ResurrectGump ) );
                m.SendGump( new ResurrectGump( m, Caster ) );
            }
            else if (m is BaseCreature && ((BaseCreature)m).GetMaster() != null && CheckBSequence( m, true ) )
			{
				BaseCreature pet = (BaseCreature)m;
				Mobile master = pet.GetMaster();
				SpellHelper.Turn( Caster, m );

				m.PlaySound( 0x214 );
				m.FixedEffect( 0x376A, 10, 16, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0 );

				master.CloseGump(typeof(PetResurrectGump));
				master.SendGump(new PetResurrectGump(master, pet));
			}
            FinishSequence();
        }

        public void ItemTarget( Item hench )
        {
			if ( hench is HenchmanFighterItem && CheckSequence() )
			{
				HenchmanFighterItem friend = (HenchmanFighterItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "fighter henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					Caster.PlaySound( 0x214 );
				}
				else
				{
                    Caster.SendMessage(55, "Eles não estão mortos.");
				}
			}
			else if ( hench is HenchmanWizardItem && CheckSequence() )
			{
				HenchmanWizardItem friend = (HenchmanWizardItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "wizard henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					Caster.PlaySound( 0x214 );
				}
				else
				{
                    Caster.SendMessage(55, "Eles não estão mortos.");
                }
			}
			else if ( hench is HenchmanArcherItem && CheckSequence() )
			{
				HenchmanArcherItem friend = (HenchmanArcherItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "archer henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					Caster.PlaySound( 0x214 );
				}
				else
				{
                    Caster.SendMessage(55, "Eles não estão mortos.");
                }
			}
			else if (hench is HenchmanMonsterItem && CheckSequence() )
			{
				HenchmanMonsterItem friend = (HenchmanMonsterItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "creature henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					Caster.PlaySound( 0x214 );
				}
				else
				{
                    Caster.SendMessage(55, "Eles não estão mortos.");
                }
			}
			else
			{
				Caster.SendMessage(55, "Este feitiço não pareceu funcionar.");
			}
            FinishSequence();
		}
 
        private class InternalTarget : Target
        {
            private ResurrectionSpell m_Owner;
 
            public InternalTarget( ResurrectionSpell owner ) : base(Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
            {
                m_Owner = owner;
            }
 
            protected override void OnTarget( Mobile from, object o )
            {
                if ( o is Mobile )
                {
                    m_Owner.Target( (Mobile)o );
                }
                else if ( o is Item )
                {
                    m_Owner.ItemTarget( (Item)o );
                }
            }
 
            protected override void OnTargetFinish( Mobile from )
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
