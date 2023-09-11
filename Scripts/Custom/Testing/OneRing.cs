using System;
using Server;
using System.Collections;
using Server.OneTime;
using Server.Mobiles;

namespace Server.Items
{
	public class OneRing : GoldRing, IOneTime
	{
		public static Mobile wearer;

		private int m_OneTimeType;
        public int OneTimeType
        {
            get{ return m_OneTimeType; }
            set{ m_OneTimeType = value; }
        }

		private int m_Tick;
		[CommandProperty( AccessLevel.GameMaster )]
        public int Tick
        {
            get{ return m_Tick; }
            set{ m_Tick = value; }
        }

		[Constructable]
		public OneRing()
		{
			Name = "O Um Anel";
			Hue = 0x21;
			ItemID = 0x4CF8;
			m_Tick = 0;
			m_OneTimeType = 3;
		}


        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "brilha com energia misteriosa");
        }

		public override void OnAfterSpawn()
		{
			ArrayList targets = new ArrayList();
			foreach ( Item item in World.Items.Values )
			if ( item is OneRing && item != this)
			{
				targets.Add( item );
			}
			for ( int i = 0; i < targets.Count; ++i )
			{
				Item item = ( Item )targets[ i ];
				item.Delete();
			}
		}

		public override bool OnEquip( Mobile from )
		{
			if (!(from is PlayerMobile) || !((PlayerMobile)from).Avatar)
			{
				from.SendMessage(55, "O anel n�o faz nada por voc�, ent�o voc� o tira.");
				return false;
			}

			if (wearer == null || (wearer != null && wearer != from))
				wearer = from;

/*			if ( ((PlayerMobile)from).GetFlag( PlayerFlag.JustWoreRing ))
			{
				from.SendMessage("O anel ainda n�o permite que voc� o use novamente.");
				return false;
			}*/

			if (!from.Hidden)
				from.Hidden = true;

			return true;
		}

		public override void OnRemoved( IEntity parent )
		{
			PlayerMobile from = null;

			if (parent is PlayerMobile)
				from = parent as PlayerMobile;
			else
				return;

			//from.SetFlag( PlayerFlag.JustWoreRing, true );

			//Timer.DelayCall( TimeSpan.FromMinutes( 3 ), new TimerStateCallback ( EquipAgain ), new object[]{ from }  );

		}

		public void EquipAgain( object state )
		{
			if (this.Deleted || this == null)
				return;
				
			object[] states = (object[])state;

			PlayerMobile from = (PlayerMobile)states[0];
			//from.SetFlag( PlayerFlag.JustWoreRing, false );
		}

		public void OneTimeTick()
        {
			if (m_Tick >= 5)
			{
				if (wearer != null)
				{
					if (wearer.FindItemOnLayer(Layer.Ring) != null && wearer.FindItemOnLayer(Layer.Ring) is OneRing)
						wearer.Hits -= 6;// + (wearer.Hits/30);
				}

				m_Tick = 0;
			}

			m_Tick ++;
		}


		public OneRing( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );


			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			m_OneTimeType = 3;


			int version = reader.ReadInt();
		}
	}
}