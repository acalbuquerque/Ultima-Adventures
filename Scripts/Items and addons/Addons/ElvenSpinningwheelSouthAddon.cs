using System;
using Server;
using Server.Mobiles;
namespace Server.Items
{
	public class ElvenSpinningwheelSouthAddon : BaseAddon, ISpinningWheel
	{
		public override BaseAddonDeed Deed{ get{ return new ElvenSpinningwheelSouthDeed(); } }

		[Constructable]
		public ElvenSpinningwheelSouthAddon()
		{
            Name = "roda de fiar";
            AddComponent( new AddonComponent( 0x2DDA ), 0, 0, 0 );
		}

		public ElvenSpinningwheelSouthAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}

		private Timer m_Timer;
        private Point3D m_startLoc;

        public override void OnComponentLoaded( AddonComponent c )
		{
			switch ( c.ItemID )
			{
				case 0x1016:
				case 0x101A:
				case 0x101D:
				case 0x10A5: --c.ItemID; break;
			}
		}

		public bool Spinning{ get{ return m_Timer != null; } }

		public void BeginSpin( SpinCallback callback, Mobile from, Item yarn )
		{
            PlayerMobile pm = from as PlayerMobile;
            pm.SendMessage(55, "Voc� n�o deve se mover enquanto transforma o(s) item(s). Caso contr�rio, falhar� na transforma��o!");

            m_Timer = new SpinTimer( this, callback, from, yarn );
			m_Timer.Start();

            m_startLoc = pm.Location;

            foreach ( AddonComponent c in Components )
			{
				switch ( c.ItemID )
				{
					case 0x1015:
					case 0x1019:
					case 0x101C:
					case 0x10A4: ++c.ItemID; break;
				}
			}
		}

		public void EndSpin( SpinCallback callback, Mobile from, Item yarn )
		{
            PlayerMobile pm = from as PlayerMobile;
            if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = null;

			foreach ( AddonComponent c in Components )
			{
				switch ( c.ItemID )
				{
					case 0x1016:
					case 0x101A:
					case 0x101D:
					case 0x10A5: --c.ItemID; break;
				}
			}

			if ( callback != null )
                callback(this, from, yarn, m_startLoc);
        }

		private class SpinTimer : Timer
		{
			private ElvenSpinningwheelSouthAddon m_Wheel;
			private SpinCallback m_Callback;
			private Mobile m_From;
			private Item m_Yarn;

			public SpinTimer( ElvenSpinningwheelSouthAddon wheel, SpinCallback callback, Mobile from, Item yarn ) : base(TimeSpan.FromSeconds((yarn.Amount >= 30) ? 45 : (int)(1.5 * yarn.Amount)))
            {
				m_Wheel = wheel;
				m_Callback = callback;
				m_From = from;
				m_Yarn = yarn;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Wheel.EndSpin( m_Callback, m_From, m_Yarn );
			}
		}
	}

	public class ElvenSpinningwheelSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new ElvenSpinningwheelSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072878; } } // spinning wheel (south)

		[Constructable]
		public ElvenSpinningwheelSouthDeed()
		{
		}

		public ElvenSpinningwheelSouthDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}