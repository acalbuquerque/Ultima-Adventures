using System;
using Server.Network;
using Server.Multis;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Regions;
using Server.Mobiles;

namespace Server.Spells.Seventh
{
	public class GateTravelSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Gate Travel", "Vas Rel Por",
				263,
				9032,
				Reagent.BlackPearl,
				Reagent.MandrakeRoot,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Seventh; } }

		private RunebookEntry m_Entry;

		public GateTravelSpell( Mobile caster, Item scroll ) : this( caster, scroll, null )
		{
		}

		public GateTravelSpell( Mobile caster, Item scroll, RunebookEntry entry ) : base( caster, scroll, m_Info )
		{
			m_Entry = entry;
		}

		public override void OnCast()
		{
			if ( m_Entry == null )
				Caster.Target = new InternalTarget( this );
			else
				Effect( m_Entry.Location, m_Entry.Map, true );
		}

		public override bool CheckCast(Mobile caster)
		{
			return SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom );
		}

		private bool GateExistsAt(Map map, Point3D loc )
		{
			bool _gateFound = false;

			IPooledEnumerable eable = map.GetItemsInRange( loc, 0 );
			foreach ( Item item in eable )
			{
				if ( item is Moongate || item is PublicMoongate )
				{
					_gateFound = true;
					break;
				}
			}
			eable.Free();

			return _gateFound;
		}

		public void Effect( Point3D loc, Map map, bool checkMulti )
		{
			if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom ) || !SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.GateTo))
			{
                Caster.SendMessage(55, "Algo de estranho aconteceu e bloqueou o uso deste feitiço.");
            }
			else if ( Worlds.AllowEscape( Caster, Caster.Map, Caster.Location, Caster.X, Caster.Y ) == false || 
				Worlds.RegionAllowedRecall(Caster.Map, Caster.Location, Caster.X, Caster.Y) == false)
			{
				Caster.SendMessage(55, "Esse feitiço parece não funcionar neste lugar.");
			}
			else if ( Worlds.RegionAllowedTeleport( map, loc, loc.X, loc.Y ) == false )
			{
				Caster.SendMessage(55, "O destino parece magicamente inacessível.");
			}
			else if ( !map.CanSpawnMobile( loc.X, loc.Y, loc.Z ) || (checkMulti && SpellHelper.CheckMulti(loc, map)))
			{
                Caster.SendMessage(55, "Esse local está bloqueado para o uso de teletransporte!");
            }
			else if ( Core.SE && ( GateExistsAt( map, loc ) || GateExistsAt( Caster.Map, Caster.Location ) ) ) // SE restricted stacking gates
			{
                Caster.SendMessage(55, "Já existe um portal neste local.");
			}
			else if ( CheckSequence() )
			{
                Caster.SendMessage(55, "Você abriu um portal mágico para outro lugar.");

				Effects.PlaySound( Caster.Location, Caster.Map, 0x20E );
				InternalItem firstGate = new InternalItem( loc, map );
				firstGate.Hue = Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0, true );
				if ( AetherGlobe.EvilChamp == Caster || AetherGlobe.GoodChamp == Caster )
					firstGate.ChampGate = true;
				firstGate.MoveToWorld( Caster.Location, Caster.Map );

				if ( Worlds.RegionAllowedTeleport( Caster.Map, Caster.Location, Caster.X, Caster.Y ) == true )
				{
					Effects.PlaySound( loc, map, 0x20E );
					InternalItem secondGate = new InternalItem( Caster.Location, Caster.Map );
					secondGate.Hue = Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0, true );
					if ( AetherGlobe.EvilChamp == Caster || AetherGlobe.GoodChamp == Caster )
						secondGate.ChampGate = true;
					secondGate.MoveToWorld( loc, map );
				}
			}

			FinishSequence();
		}

		[DispellableField]
		private class InternalItem : Moongate
		{
			public override bool ShowFeluccaWarning{ get { return false; } } // WIZARD

			public InternalItem( Point3D target, Map map ) : base( target, map )
			{
				Map = map;

				Dispellable = true;

				InternalTimer t = new InternalTimer( this );
				t.Start();
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				Delete();
			}

			private class InternalTimer : Timer
			{
				private Item m_Item;

				public InternalTimer( Item item ) : base( TimeSpan.FromSeconds( 30.0 ) )
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

		private class InternalTarget : Target
		{
			private GateTravelSpell m_Owner;

			public InternalTarget( GateTravelSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
                owner.Caster.SendMessage(55, "Selecione um destino válido.");
            }

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					RecallRune rune = (RecallRune)o;

					if ( rune.Marked )
						m_Owner.Effect( rune.Target, rune.TargetMap, true );
					else
                        from.SendMessage(55, "Essa runa ainda não está marcada.");
				}
				else if ( o is Runebook )
				{
					RunebookEntry e = ((Runebook)o).Default;

					if ( e != null )
						m_Owner.Effect( e.Location, e.Map, true );
					else
                        from.SendMessage(55, "O destino não está marcado corretamente.");
				}
				else if ( o is HouseRaffleDeed && ((HouseRaffleDeed)o).ValidLocation() )
				{
					HouseRaffleDeed deed = (HouseRaffleDeed)o;

					m_Owner.Effect( deed.PlotLocation, deed.PlotFacet, true );
				}
				else
				{
					from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501030, from.Name, "" ) ); // I can not gate travel from that object.
				}
			}
			
			protected override void OnNonlocalTarget( Mobile from, object o )
			{
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
