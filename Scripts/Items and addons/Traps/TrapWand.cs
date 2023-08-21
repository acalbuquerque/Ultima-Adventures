using System;
using Server;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using Server.Spells;
using Server.Network;
using Server.Multis;
using System.Collections;

namespace Server.Items 
{
	public class TrapWand : Item
	{
		public int WandPower;

		[CommandProperty(AccessLevel.Owner)]
		public int Wand_Power { get { return WandPower; } set { WandPower = value; InvalidateProperties(); } }

		public Mobile owner;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return owner; }
			set{ owner = value; }
		}

		[Constructable]
		public TrapWand() : this( null )
		{
		}
		
		[Constructable]
		public TrapWand( Mobile from ) : base( 0x4FD6 )
		{
			this.owner = from;
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = Server.Misc.RandomThings.GetRandomColor(0);
			Light = LightType.Circle150;
			RenameWand();
			ItemRemovalTimer thisTimer = new ItemRemovalTimer( this );
			thisTimer.Start();
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Evita armadilhas em paredes e pisos em " + WandPower + "%");
			list.Add( 1049644, "Deve estar na mochila e dura 10 minutos"); // PARENTHESIS
        }

		private void RenameWand()
		{
			if ( owner != null )
			{
				this.Name = "orbe anti-armadilha de " + owner.Name;
			}
			else
			{
				this.Name = "orbe anti-armadilha";
			}		
		}

		public TrapWand( Serial serial ) : base( serial )
		{ 
		} 
		
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
			writer.Write( (Mobile)owner);
            writer.Write( WandPower );
			RenameWand();
		} 
		
		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
			owner = reader.ReadMobile();
            WandPower = reader.ReadInt();
			this.Delete(); // none when the world starts 
			RenameWand();
		}

		public class ItemRemovalTimer : Timer 
		{ 
			private Item i_item; 
			public ItemRemovalTimer( Item item ) : base( TimeSpan.FromMinutes( 10.0 ) ) 
			{ 
				Priority = TimerPriority.OneSecond; 
				i_item = item; 
			} 

			protected override void OnTick() 
			{ 
				if (( i_item != null ) && ( !i_item.Deleted ))
				{
					TrapWand wands = (TrapWand)i_item;
					Mobile from = wands.owner;
					from.LocalOverheadMessage(Network.MessageType.Emote, 0x3B2, false, "* Seu orbe anti-armadilha desapareceu. *");
					from.PlaySound( 0x1F0 );
					i_item.Delete();
				}
			} 
		} 
	}
}