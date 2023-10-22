using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public delegate void SpinCallback( ISpinningWheel sender, Mobile from, Item yarn, Point3D originLoc );

	public interface ISpinningWheel
	{
		bool Spinning{ get; }
		void BeginSpin( SpinCallback callback, Mobile from, Item yarn );
	}

	public class SpinningwheelEastAddon : BaseAddon, ISpinningWheel
	{
		public override BaseAddonDeed Deed{ get{ return new SpinningwheelEastDeed(); } }

		[Constructable]
		public SpinningwheelEastAddon()
		{
            Name = "roda de fiar";
            AddComponent( new AddonComponent( 0x1019 ), 0, 0, 0 );
		}

		public SpinningwheelEastAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
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

		public void BeginSpin( SpinCallback callback, Mobile from, Item yarn)
		{
            PlayerMobile pm = from as PlayerMobile;
            pm.SendMessage(55, "Você não deve se mover enquanto transforma o(s) item(s). Caso contrário, falhará na transformação!");

            m_Timer = new SpinTimer( this, callback, from, yarn );
			m_Timer.Start();

			m_startLoc = pm.Location;

            foreach (AddonComponent c in Components)
			{
                switch (c.ItemID)
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
				callback( this, from, yarn, m_startLoc);
		}

		private class SpinTimer : Timer
		{
			private SpinningwheelEastAddon m_Wheel;
			private SpinCallback m_Callback;
			private Mobile m_From;
			private Item m_Yarn;
			private Point3D m_originLoc;

			public SpinTimer( SpinningwheelEastAddon wheel, SpinCallback callback, Mobile from, Item yarn ) : base( TimeSpan.FromSeconds( (yarn.Amount >= 30) ? 45 : (int)(1.5 * yarn.Amount) ) )
			{
                PlayerMobile pm = from as PlayerMobile;

                m_Wheel = wheel;
				m_Callback = callback;
				m_From = from;
				m_Yarn = yarn;
				m_originLoc = pm.Location;
                Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				m_Wheel.EndSpin( m_Callback, m_From, m_Yarn );
			}
		}
	}

	public class SpinningwheelEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SpinningwheelEastAddon(); } }
		public override int LabelNumber{ get{ return 1044341; } } // spining wheel (east)

		[Constructable]
		public SpinningwheelEastDeed()
		{
		}

		public SpinningwheelEastDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}