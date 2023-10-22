using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseIngot : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}

		public override double DefaultWeight
		{
			get { return 0.1; }
		}
		
		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Resource );
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
				case 0:
				{
					OreInfo info;

					switch ( reader.ReadInt() )
					{
                            case 0: info = OreInfo.Iron; break;
                            case 1: info = OreInfo.DullCopper; break;
                            case 2: info = OreInfo.Copper; break;
                            case 3: info = OreInfo.Bronze; break;
                            case 4: info = OreInfo.ShadowIron; break;
                            case 5: info = OreInfo.Platinum; break;
                            case 6: info = OreInfo.Gold; break;
                            case 7: info = OreInfo.Agapite; break;
                            case 8: info = OreInfo.Verite; break;
                            case 9: info = OreInfo.Valorite; break;
                            case 10: info = OreInfo.Titanium; break;
                            case 11: info = OreInfo.Rosenium; break;
                            case 12: info = OreInfo.Nepturite; break;
                            case 13: info = OreInfo.Obsidian; break;
                            case 14: info = OreInfo.Mithril; break;
                            case 15: info = OreInfo.Xormite; break;
                            case 16: info = OreInfo.Dwarven; break;
                            default: info = null; break;
					}

					m_Resource = CraftResources.GetFromOreInfo( info );
					break;
				}
			}
		}

		public BaseIngot( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseIngot( CraftResource resource, int amount ) : base( 0x1BF2 )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );

			m_Resource = resource;
		}

		public BaseIngot( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1027154 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1027154 ); // ingots
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

		public override int LabelNumber
		{
			get
			{
				if ( m_Resource >= CraftResource.DullCopper && m_Resource <= CraftResource.Valorite )
					return 1042684 + (int)(m_Resource - CraftResource.DullCopper);

				else if (m_Resource == CraftResource.Titanium)
                    return 6661001;
                else if (m_Resource == CraftResource.Rosenium)
                    return 6662001;
                else if (m_Resource == CraftResource.Platinum)
                    return 6663001;

                else if( m_Resource == CraftResource.Steel )
					return 1036159;

                else if( m_Resource == CraftResource.Brass )
					return 1036160;

                else if( m_Resource == CraftResource.Mithril )
					return 1036158;

                else if( m_Resource == CraftResource.Obsidian )
					return 1036168;

                else if( m_Resource == CraftResource.Nepturite )
					return 1036176;

                else if( m_Resource == CraftResource.Xormite )
					return 1034443;

                else if( m_Resource == CraftResource.Dwarven )
					return 1036187;

				return 1042692;
			}
		}
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class IronIngot : BaseIngot
	{
		[Constructable]
		public IronIngot() : this( 1 )
		{
		}

		[Constructable]
		public IronIngot( int amount ) : base( CraftResource.Iron, amount )
		{
		}

		public IronIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class DullCopperIngot : BaseIngot
	{
		[Constructable]
		public DullCopperIngot() : this( 1 )
		{
		}

		[Constructable]
		public DullCopperIngot( int amount ) : base( CraftResource.DullCopper, amount )
		{
		}

		public DullCopperIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class ShadowIronIngot : BaseIngot
	{
		[Constructable]
		public ShadowIronIngot() : this( 1 )
		{
		}

		[Constructable]
		public ShadowIronIngot( int amount ) : base( CraftResource.ShadowIron, amount )
		{
		}

		public ShadowIronIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class CopperIngot : BaseIngot
	{
		[Constructable]
		public CopperIngot() : this( 1 )
		{
		}

		[Constructable]
		public CopperIngot( int amount ) : base( CraftResource.Copper, amount )
		{
		}

		public CopperIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class BronzeIngot : BaseIngot
	{
		[Constructable]
		public BronzeIngot() : this( 1 )
		{
		}

		[Constructable]
		public BronzeIngot( int amount ) : base( CraftResource.Bronze, amount )
		{
		}

		public BronzeIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class GoldIngot : BaseIngot
	{
		[Constructable]
		public GoldIngot() : this( 1 )
		{
		}

		[Constructable]
		public GoldIngot( int amount ) : base( CraftResource.Gold, amount )
		{
		}

		public GoldIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class AgapiteIngot : BaseIngot
	{
		[Constructable]
		public AgapiteIngot() : this( 1 )
		{
		}

		[Constructable]
		public AgapiteIngot( int amount ) : base( CraftResource.Agapite, amount )
		{
		}

		public AgapiteIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class VeriteIngot : BaseIngot
	{
		[Constructable]
		public VeriteIngot() : this( 1 )
		{
		}

		[Constructable]
		public VeriteIngot( int amount ) : base( CraftResource.Verite, amount )
		{
		}

		public VeriteIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class ValoriteIngot : BaseIngot
	{
		[Constructable]
		public ValoriteIngot() : this( 1 )
		{
		}

		[Constructable]
		public ValoriteIngot( int amount ) : base( CraftResource.Valorite, amount )
		{
		}

		public ValoriteIngot( Serial serial ) : base( serial )
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class TitaniumIngot : BaseIngot
    {
        //protected override CraftResource DefaultResource { get { return CraftResource.Titanium; } }
        public override int Hue { get { return (Server.Misc.MaterialInfo.GetMaterialColor("titanium", "classic", 0)); } }
        
        [Constructable]
        public TitaniumIngot() : this(1)
        {
        }

        [Constructable]
        public TitaniumIngot(int amount) : base(CraftResource.Titanium, amount)
        {
        }

        public TitaniumIngot(Serial serial) : base(serial)
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
    }

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class RoseniumIngot : BaseIngot
    {
        //protected override CraftResource DefaultResource { get { return CraftResource.Titanium; } }
        public override int Hue { get { return (Server.Misc.MaterialInfo.GetMaterialColor("rosenium", "classic", 0)); } }

        [Constructable]
        public RoseniumIngot() : this(1)
        {
        }

        [Constructable]
        public RoseniumIngot(int amount) : base(CraftResource.Rosenium, amount)
        {
        }

        public RoseniumIngot(Serial serial) : base(serial)
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
    }

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class PlatinumIngot : BaseIngot
    {
        //protected override CraftResource DefaultResource { get { return CraftResource.Titanium; } }
        public override int Hue { get { return (Server.Misc.MaterialInfo.GetMaterialColor("platinum", "classic", 0)); } }

        [Constructable]
        public PlatinumIngot() : this(1)
        {
        }

        [Constructable]
        public PlatinumIngot(int amount) : base(CraftResource.Platinum, amount)
        {
        }

        public PlatinumIngot(Serial serial) : base(serial)
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
    }

    [FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class SteelIngot : BaseIngot
	{
		[Constructable]
		public SteelIngot() : this( 1 )
		{
		}

		[Constructable]
		public SteelIngot( int amount ) : base( CraftResource.Steel, amount )
		{
		}

		public SteelIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class BrassIngot : BaseIngot
	{
		[Constructable]
		public BrassIngot() : this( 1 )
		{
		}

		[Constructable]
		public BrassIngot( int amount ) : base( CraftResource.Brass, amount )
		{
		}

		public BrassIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class MithrilIngot : BaseIngot
	{
		[Constructable]
		public MithrilIngot() : this( 1 )
		{
		}

		[Constructable]
		public MithrilIngot( int amount ) : base( CraftResource.Mithril, amount )
		{
		}

		public MithrilIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class DwarvenIngot : BaseIngot
	{
		[Constructable]
		public DwarvenIngot() : this( 1 )
		{
		}

		[Constructable]
		public DwarvenIngot( int amount ) : base( CraftResource.Dwarven, amount )
		{
		}

		public DwarvenIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class XormiteIngot : BaseIngot
	{
		[Constructable]
		public XormiteIngot() : this( 1 )
		{
		}

		[Constructable]
		public XormiteIngot( int amount ) : base( CraftResource.Xormite, amount )
		{
		}

		public XormiteIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class ObsidianIngot : BaseIngot
	{
		[Constructable]
		public ObsidianIngot() : this( 1 )
		{
		}

		[Constructable]
		public ObsidianIngot( int amount ) : base( CraftResource.Obsidian, amount )
		{
		}

		public ObsidianIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class NepturiteIngot : BaseIngot
	{
		[Constructable]
		public NepturiteIngot() : this( 1 )
		{
		}

		[Constructable]
		public NepturiteIngot( int amount ) : base( CraftResource.Nepturite, amount )
		{
		}

		public NepturiteIngot( Serial serial ) : base( serial )
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