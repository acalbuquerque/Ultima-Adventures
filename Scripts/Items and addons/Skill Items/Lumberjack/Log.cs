using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseLog : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get { return m_Resource; }
			set { m_Resource = value; InvalidateProperties(); }
		}

		int ICommodity.DescriptionNumber { get { return CraftResources.IsStandard( m_Resource ) ? LabelNumber : 1075062 + ( (int)m_Resource - (int)CraftResource.RegularWood ); } }
		bool ICommodity.IsDeedable { get { return true; } }
		[Constructable]
		public BaseLog() : this( 1 )
		{
            Name = "Tora(s)";
        }

		[Constructable]
		public BaseLog( int amount ) : this( CraftResource.RegularWood, amount )
		{
            Name = "Tora(s)";
        }

		public abstract BaseWoodBoard GetLog();

		[Constructable]
		public BaseLog( CraftResource resource ) : this( resource, 1 )
		{
            Name = "Tora(s)";
        }

		[Constructable]
		public BaseLog( CraftResource resource, int amount ) : base( 0x1BE0 )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
            Name = "Tora(s)";
            m_Resource = resource;
			Hue = CraftResources.GetHue( resource );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !CraftResources.IsStandard( m_Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( m_Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( m_Resource ) );
			}
		}

		public BaseLog( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int)m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Resource = (CraftResource)reader.ReadInt();
						break;
					}
			}

			if ( version == 0 )
				m_Resource = CraftResource.RegularWood;

			ItemID = 0x1BE0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;
			
			if ( RootParent is BaseCreature )
			{
				from.SendLocalizedMessage( 500447 ); // That is not accessible
				return;
			}
			else if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.SendMessage("Selecione a serraria na qual deseja cortar as toras.");
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendMessage( "As toras est�o muito longe." );
			}
		}

		private class InternalTarget : Target
		{
			private BaseLog m_Log;

			public InternalTarget( BaseLog log ) :  base ( 2, false, TargetFlags.None )
			{
				m_Log = log;
			}

			private bool IsMill( object obj )
			{
				if ( obj is Item )
				{
					Item saw = (Item)obj;

					if ( saw.Name == "serraria" && 
						( saw.ItemID == 1928 || saw.ItemID == 4525 || saw.ItemID == 7130 || saw.ItemID == 4530 || saw.ItemID == 7127 )
					   )
						return true;
					else
						return false;
				}

				return false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Log.Deleted )
					return;

				if ( !from.InRange( m_Log.GetWorldLocation(), 2 ) )
				{
					from.SendMessage("As toras est�o muito longe.");
					return;
				}

				if ( IsMill( targeted ) )
				{
					double difficulty;

					switch ( m_Log.Resource )
					{
						default: difficulty = 40.0; break;
						case CraftResource.AshTree: difficulty = 60.0; break;
						case CraftResource.EbonyTree: difficulty = 70.0; break;
                        case CraftResource.ElvenTree: difficulty = 80.0; break;
                        case CraftResource.GoldenOakTree: difficulty = 85.0; break;
                        case CraftResource.CherryTree: difficulty = 90.0; break;
                        case CraftResource.RosewoodTree: difficulty = 95.0; break;
						case CraftResource.HickoryTree: difficulty = 100.0; break;
						/*case CraftResource.MahoganyTree: difficulty = 80.0; break;
						case CraftResource.DriftwoodTree: difficulty = 80.0; break;
						case CraftResource.OakTree: difficulty = 85.0; break;
						case CraftResource.PineTree: difficulty = 90.0; break;
						case CraftResource.GhostTree: difficulty = 90.0; break;*/
						
						/*case CraftResource.WalnutTree: difficulty = 99.0; break;
						case CraftResource.PetrifiedTree: difficulty = 99.9; break;*/
						
					}

					double minSkill = difficulty - 10.0;
					double maxSkill = difficulty + 10.0;
					
					if ( difficulty > 50.0 && difficulty > from.Skills[SkillName.Lumberjacking].Value )
					{
						from.SendMessage(55,"Voc� n�o tem ideia de como cortar e trabalhar esse tipo de madeira!");
						return;
					}

					if ( from.CheckTargetSkill( SkillName.Lumberjacking, targeted, minSkill, maxSkill ) )
					{
						if ( m_Log.Amount <= 0 )
						{
							from.SendMessage(55,"N�o h� madeira suficiente nesta pilha para fazer uma t�bua.");
						}
						else
						{
							int amount = m_Log.Amount;
							BaseWoodBoard wood = m_Log.GetLog();
							m_Log.Delete();
							wood.Amount = amount;
							from.AddToBackpack( wood );
							from.PlaySound( 0x21C );
							from.SendMessage( 55, "Voc� corta as toras e coloca algumas t�buas na mochila.");
						}
					}
					else
					{
						int amount = m_Log.Amount;
						int lose = Utility.RandomMinMax( 1, amount );

						if ( amount < 2 || lose == amount )
						{
							m_Log.Delete();
							from.SendMessage(55, "Voc� tenta cortar as toras, mas estraga toda a madeira.");
						}
						else
						{
							m_Log.Amount = amount - lose;
							from.SendMessage(55, "Voc� tenta cortar as toras, mas estraga um pouco da madeira.");
						}

						from.PlaySound( 0x21C );
					}
				}
				else
				{
					from.SendMessage(55, "Isso n�o � uma serraria");
				}
			}
		}
	}

	public class Log : BaseLog
	{
		[Constructable]
		public Log() : this( 1 )
		{
		}
		[Constructable]
		public Log( int amount ) : base( CraftResource.RegularWood, amount )
		{
		}
		public Log( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            Weight = 3.0;
        }
		public override BaseWoodBoard GetLog()
		{
			return new Board();
		}
	}
	public class AshLog : BaseLog
	{
		[Constructable]
		public AshLog() : this(1)
		{
		}
		[Constructable]
		public AshLog(int amount) : base(CraftResource.AshTree, amount)
		{
		}
		public AshLog(Serial serial) : base(serial)
		{
		}
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
            Weight = 3.0;
        }
		public override BaseWoodBoard GetLog()
		{
			return new AshBoard();
		}
	}
    public class EbonyLog : BaseLog
    {
        [Constructable]
        public EbonyLog() : this(1)
        {
        }
        [Constructable]
        public EbonyLog(int amount) : base(CraftResource.EbonyTree, amount)
        {
        }
        public EbonyLog(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Weight = 4.0;
        }
        public override BaseWoodBoard GetLog()
        {
            return new EbonyBoard();
        }
    }
    public class ElvenLog : BaseLog
    {
        [Constructable]
        public ElvenLog() : this(1)
        {
        }
        [Constructable]
        public ElvenLog(int amount) : base(CraftResource.ElvenTree, amount)
        {
        }
        public ElvenLog(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Weight = 3.0;
        }
        public override BaseWoodBoard GetLog()
        {
            return new ElvenBoard();
        }
    }

	public class GoldenOakLog : BaseLog
	{
		[Constructable]
		public GoldenOakLog() : this( 1 )
		{
		}
		[Constructable]
		public GoldenOakLog( int amount ) : base( CraftResource.GoldenOakTree, amount )
		{
		}
		public GoldenOakLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            Weight = 5.0;
        }
		public override BaseWoodBoard GetLog()
		{
			return new GoldenOakBoard();
		}
	}
    public class CherryLog : BaseLog
    {
        [Constructable]
        public CherryLog() : this(1)
        {
        }
        [Constructable]
        public CherryLog(int amount) : base(CraftResource.CherryTree, amount)
        {
        }
        public CherryLog(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Weight = 4.0;
        }
        public override BaseWoodBoard GetLog()
        {
            return new CherryBoard();
        }
    }
    public class RosewoodLog : BaseLog
    {
        [Constructable]
        public RosewoodLog() : this(1)
        {
        }
        [Constructable]
        public RosewoodLog(int amount) : base(CraftResource.RosewoodTree, amount)
        {
        }
        public RosewoodLog(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Weight = 5.0;
        }
        public override BaseWoodBoard GetLog()
        {
            return new RosewoodBoard();
        }
    }
    public class HickoryLog : BaseLog
    {
        [Constructable]
        public HickoryLog() : this(1)
        {
        }
        [Constructable]
        public HickoryLog(int amount) : base(CraftResource.HickoryTree, amount)
        {
        }
        public HickoryLog(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Weight = 4.0;
        }
        public override BaseWoodBoard GetLog()
        {
            return new HickoryBoard();
        }
    }
    /*public class PetrifiedLog : BaseLog
    {
        [Constructable]
        public PetrifiedLog() : this(1)
        {
        }
        [Constructable]
        public PetrifiedLog(int amount) : base(CraftResource.PetrifiedTree, amount)
        {
        }
        public PetrifiedLog(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
        public override BaseWoodBoard GetLog()
        {
            return new PetrifiedBoard();
        }
    }*/
    /*public class MahoganyLog : BaseLog
	{
		[Constructable]
		public MahoganyLog() : this( 1 )
		{
		}
		[Constructable]
		public MahoganyLog( int amount ) : base( CraftResource.MahoganyTree, amount )
		{
		}
		public MahoganyLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override BaseWoodBoard GetLog()
		{
			return new MahoganyBoard();
		}
	}*/
    /*public class OakLog : BaseLog
	{
		[Constructable]
		public OakLog() : this( 1 )
		{
		}
		[Constructable]
		public OakLog( int amount ) : base( CraftResource.OakTree, amount )
		{
		}
		public OakLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override BaseWoodBoard GetLog()
		{
			return new OakBoard();
		}
	}*/
    /*public class PineLog : BaseLog
	{
		[Constructable]
		public PineLog() : this( 1 )
		{
		}
		[Constructable]
		public PineLog( int amount ) : base( CraftResource.PineTree, amount )
		{
		}
		public PineLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override BaseWoodBoard GetLog()
		{
			return new PineBoard();
		}
	}*/

    /*public class WalnutLog : BaseLog
	{
		[Constructable]
		public WalnutLog() : this( 1 )
		{
		}
		[Constructable]
		public WalnutLog( int amount ) : base( CraftResource.WalnutTree, amount )
		{
		}
		public WalnutLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override BaseWoodBoard GetLog()
		{
			return new WalnutBoard();
		}
	}*/
    /*public class DriftwoodLog : BaseLog
	{
		[Constructable]
		public DriftwoodLog() : this( 1 )
		{
		}
		[Constructable]
		public DriftwoodLog( int amount ) : base( CraftResource.DriftwoodTree, amount )
		{
		}
		public DriftwoodLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override BaseWoodBoard GetLog()
		{
			return new DriftwoodBoard();
		}
	}*/
    /*public class GhostLog : BaseLog
	{
		[Constructable]
		public GhostLog() : this( 1 )
		{
		}
		[Constructable]
		public GhostLog( int amount ) : base( CraftResource.GhostTree, amount )
		{
		}
		public GhostLog( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		public override BaseWoodBoard GetLog()
		{
			return new GhostBoard();
		}
	}*/


}