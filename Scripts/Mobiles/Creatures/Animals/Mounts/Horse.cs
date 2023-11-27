using System;
using Server.Items;
using Server.Mobiles;


namespace Server.Mobiles
{
	[CorpseName( "corpo de cavalo" )]
	[TypeAlias( "Server.Mobiles.BrownHorse", "Server.Mobiles.DirtyHorse", "Server.Mobiles.GrayHorse", "Server.Mobiles.TanHorse" )]
	public class Horse : BaseMount
	{
		private static int[] m_IDs = new int[]
			{
				0xC8, 0x3E9F,
				0xE2, 0x3EA0,
				0xE4, 0x3EA1,
				0xE2, 0x3EA0
			};

        private int m_OriginHue;
        private int m_OriginScale;

        private bool m_BardingExceptional;
        private Mobile m_BardingCrafter;
        private int m_BardingHP;
        private bool m_HasBarding;
        private CraftResource m_BardingResource;

        [CommandProperty(AccessLevel.GameMaster)]
        public int OriginalHue
        {
            get { return m_OriginHue; }
            set { m_OriginHue = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int OriginalScale
        {
            get { return m_OriginScale; }
            set { m_OriginScale = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardingCrafter
        {
            get { return m_BardingCrafter; }
            set { m_BardingCrafter = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardingExceptional
        {
            get { return m_BardingExceptional; }
            set { m_BardingExceptional = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BardingHP
        {
            get { return m_BardingHP; }
            set { m_BardingHP = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasBarding
        {
            get { return m_HasBarding; }
            set
            {
                m_HasBarding = value;

                if (m_HasBarding)
                {
                    Hue = CraftResources.GetHue(m_BardingResource);
                }
                else
                {
                    setupOriginAttrs(OriginalScale);

                    Hue = m_OriginHue;
                    BardingExceptional = false;
					BardingCrafter = null;
					BardingHP = 0;
					BardingResource = CraftResource.None; 
                }

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource BardingResource
        {
            get { return m_BardingResource; }
            set
            {
                m_BardingResource = value;

                if (m_HasBarding)
                    Hue = CraftResources.GetHue(value);

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BardingMaxHP
        {
            get { return m_BardingExceptional ? 2500 : 1000; }
        }

        [Constructable]
		public Horse() : this( "cavalo" )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            string resourceName = CraftResources.GetName(m_BardingResource);

            base.AddNameProperties(list);

            if (m_BardingResource != CraftResource.None)
            {
                if (string.IsNullOrEmpty(resourceName) || resourceName.ToLower() == "none" || resourceName.ToLower() == "normal")
                {
                    resourceName = "";
                }

                if (resourceName != "")
                {
                    list.Add(1053099, "Armadura: " + ItemNameHue.UnifiedItemProps.SetColor(resourceName, "#8be4fc"));
                }
            }
        }

        private void setupOriginAttrs(int scale) 
		{
            if (scale < 1) scale = 1;

            double scalar = (scale / 100) + 1;

            int random = Utility.Random(4);
            Body = m_IDs[random * 2];
            ItemID = m_IDs[random * 2 + 1];

            SetStr( (int)(32 * scalar), 85);
            SetDex( (int)(56 * scalar), 100);
            SetInt( (int)(15 * scalar), 25);

            SetHits( (int)(40 * scalar), 95);
            SetMana(0);

            SetDamage( (int)(5 * scalar), 12);

            SetDamageType(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Physical, 20, 35);

            SetSkill(SkillName.MagicResist, 25.1, 45.0);
            SetSkill(SkillName.Tactics, 39.3, 54.0);
            SetSkill(SkillName.Wrestling, 39.3, 54.0);
        }

		[Constructable]
		public Horse( string name ) : base( name, 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
            int scale = 1; // %

            Hue = 0;
            BaseSoundID = 0xA8;
            Karma = 0;
            Fame = 300;
            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 49.1;

            if (Utility.RandomDouble() > 0.40) // 40% chance
            {
			    Name = "corcel";
                Fame = 500;
                MinTameSkill = 70.1;

                scale = 20;

                Hue = Utility.RandomList( 0x780, 0x781, 0x782, 0x783, 0x8FD, 0x8FE, 0x8FF, 0x900, 0x901, 0x902, 0x903, 0x904, 0x905, 0x906, 0x907, 0x908, Utility.RandomNeutralHue() );

                if ( Utility.RandomMinMax( 0, 100 ) <= 50 ) // 2% chance
			    {
				    MinTameSkill = 80.1;
				    Fame = 600;
				    Hue = 0x9C2;
                    Name = "alazao";

                    scale = 30;

                    if ( Utility.RandomMinMax( 1, 2 ) == 1 ) // 50% chance
                    { 
                        Hue = 0x497; Name = "corcel negro"; 
                    }
			    }
		    }
            m_OriginScale = scale;
            setupOriginAttrs(scale);
            m_OriginHue = Hue;
        }

		public override int Meat{ get{ return 3; } }
		public override int Hides{ get{ return 10; } }
		public override int Furs{ get{ return Utility.RandomList( 0, 0, 0, 5 ); } }
		public override FurType FurType{ get{ return FurType.Regular; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override bool OnBeforeDeath()
		{
			Server.Items.HorseArmor.DropArmor( this );
			return base.OnBeforeDeath();
		}

		public Horse( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

            writer.Write((bool)m_BardingExceptional);
            writer.Write((Mobile)m_BardingCrafter);
            writer.Write((bool)m_HasBarding);
            writer.Write((int)m_BardingHP);
            writer.Write((int)m_BardingResource);

            writer.Write((int)m_OriginHue);
            writer.Write((int)m_OriginScale);
            
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_BardingExceptional = reader.ReadBool();
                        m_BardingCrafter = reader.ReadMobile();
                        m_HasBarding = reader.ReadBool();
                        m_BardingHP = reader.ReadInt();
                        m_BardingResource = (CraftResource)reader.ReadInt();

                        m_OriginHue = reader.ReadInt();
                        m_OriginScale = reader.ReadInt();
                        
                        break;
                    }
            }

            if (Hue == 0 && !m_HasBarding)
                Hue = m_OriginHue;

            if (BaseSoundID == -1)
                BaseSoundID = 0x16A;

            if ( !Server.Misc.MyServerSettings.ClientVersion() && Body == 587 && ItemID == 587 )
			{
				Body = 0xE2;
				ItemID = 0x3EA0;
			}
		}
	}
}