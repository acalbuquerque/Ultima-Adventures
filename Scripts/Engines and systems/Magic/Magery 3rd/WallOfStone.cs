using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using System.Collections.Generic;

namespace Server.Spells.Third
{
	public class WallOfStoneSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Wall of Stone", "In Sanct Ylem",
				227,
				9011,
				false,
				Reagent.Bloodmoss,
				Reagent.Garlic
			);

		public override SpellCircle Circle { get { return SpellCircle.Third; } }

		public WallOfStoneSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( SpellHelper.CheckTown( p, Caster ) && CheckSequence() )
			{
				SpellHelper.Turn( Caster, p );

				SpellHelper.GetSurfaceTop( ref p );

				int dx = Caster.Location.X - p.X;
				int dy = Caster.Location.Y - p.Y;
				int rx = (dx - dy) * 44;
				int ry = (dx + dy) * 44;

				bool eastToWest;

				if ( rx >= 0 && ry >= 0 )
				{
					eastToWest = false;
				}
				else if ( rx >= 0 )
				{
					eastToWest = true;
				}
				else if ( ry >= 0 )
				{
					eastToWest = true;
				}
				else
				{
					eastToWest = false;
				}

				Effects.PlaySound( p, Caster.Map, 0x1F6 );

				for ( int i = -2; i <= 2; ++i )
				{
					Point3D loc = new Point3D( eastToWest ? p.X + i : p.X, eastToWest ? p.Y : p.Y + i, p.Z );
					//bool canFit = SpellHelper.AdjustField( ref loc, Caster.Map, 22, true );

                    IPooledEnumerable eable = Caster.Map.GetMobilesInRange(loc, 0);
                    bool canFit = true;

                    foreach (Mobile m in eable)
                    {
                        if (m.AccessLevel != AccessLevel.Player || !m.Alive)
                            continue;

                        if (m.Location.Z - loc.Z < 18 && m.Location.Z - loc.Z > -10)
                        {
                            //The whole field counts as a harmful action, not just the target
                            //Caster.DoHarmful(m);
                            //Make a hole in the wall if a mobile is there
                            canFit = false;
                            break;
                        }
                    }
                    eable.Free();


                    if ( !canFit )
						continue;

                    //remove existing wall items
                    List<Item> itemsFound = new List<Item>();

                    foreach (Item item in Caster.Map.GetItemsInRange(loc, 0))
                    {
                        if (item is InternalItem) itemsFound.Add(item);
                    }

                    eable.Free();

                    for (int j = itemsFound.Count - 1; j >= 0; --j) itemsFound[j].Delete();

                    Item wall = new InternalItem( loc, Caster.Map, Caster );

					Effects.SendLocationParticles(wall, 0x376A, 9, 10, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, 5025, 0 );
				}
			}

			FinishSequence();
		}

		[DispellableField]
		public class InternalItem : Item
		{
			private Timer m_Timer;
			private DateTime m_End;
			private Mobile m_Caster;

			public override bool BlocksFit{ get{ return true; } }

			public InternalItem( Point3D loc, Map map, Mobile caster ) : base( 0x80 )
			{
				Visible = false;
				Movable = false;

				MoveToWorld( loc, map );

				m_Caster = caster;

				if ( caster.InLOS( this ) )
					Visible = true;
				else
					Delete();

				if ( Deleted )
					return;

				int nBenefit = 0;
				if ( caster is PlayerMobile ) // WIZARD
				{
					nBenefit = (int)(caster.Skills[SkillName.Inscribe].Value / 4);
				}

				m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 5.0 + nBenefit ) );
				m_Timer.Start();

				m_End = DateTime.Now + TimeSpan.FromSeconds( 5.0 );
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );

				writer.Write( (int) 1 ); // version

				writer.WriteDeltaTime( m_End );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				int version = reader.ReadInt();

				switch ( version )
				{
					case 1:
					{
						m_End = reader.ReadDeltaTime();

						m_Timer = new InternalTimer( this, m_End - DateTime.UtcNow );
						m_Timer.Start();

						break;
					}
					case 0:
					{
						TimeSpan duration = TimeSpan.FromSeconds( 10.0 );

						m_Timer = new InternalTimer( this, duration );
						m_Timer.Start();

						m_End = DateTime.UtcNow + duration;

						break;
					}
				}
			}

			public override bool OnMoveOver( Mobile m )
			{
				int noto;

				if ( m is PlayerMobile )
				{
					noto = Notoriety.Compute( m_Caster, m );
					if ( noto == Notoriety.Enemy || noto == Notoriety.Ally )
						return false;
				}
				return base.OnMoveOver( m );
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if ( m_Timer != null )
					m_Timer.Stop();
			}

			private class InternalTimer : Timer
			{
				private InternalItem m_Item;

				public InternalTimer( InternalItem item, TimeSpan duration ) : base( duration )
				{
					Priority = TimerPriority.OneSecond;
					m_Item = item;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}
		}

		public class InternalTarget : Target
		{
			private WallOfStoneSpell m_Owner;

			public InternalTarget( WallOfStoneSpell owner ) : base( Core.ML ? 10 : 12, true, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is IPoint3D )
					m_Owner.Target( (IPoint3D)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}