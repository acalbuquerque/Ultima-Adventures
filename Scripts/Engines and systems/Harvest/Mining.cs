using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Misc;

namespace Server.Engines.Harvest
{
	public class Mining : HarvestSystem
	{
		private static Mining m_System;

		public static Mining System
		{
			get
			{
				if ( m_System == null )
					m_System = new Mining();

				return m_System;
			}
		}

		private HarvestDefinition m_OreAndStone, m_Sand;

		public HarvestDefinition OreAndStone
		{
			get{ return m_OreAndStone; }
		}

		public HarvestDefinition Sand
		{
			get{ return m_Sand; }
		}

		public Mining()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region Mining for ore and stone
			HarvestDefinition oreAndStone = m_OreAndStone = new HarvestDefinition();

			// Resource banks are every 8x8 tiles
			oreAndStone.BankWidth = 3;
			oreAndStone.BankHeight = 3;

			// Every bank holds from 5-30 ore
			oreAndStone.MinTotal = 5;
			oreAndStone.MaxTotal = 30;

			// A resource bank will respawn its content every 10 to 30 minutes
			oreAndStone.MinRespawn = TimeSpan.FromMinutes( 15.0 );
			oreAndStone.MaxRespawn = TimeSpan.FromMinutes( 30.0 );

			// Skill checking is done on the Mining skill
			oreAndStone.Skill = SkillName.Mining;

			// Set the list of harvestable tiles
			oreAndStone.Tiles = m_MountainAndCaveTiles;

			// Players must be within 2 tiles to harvest
			oreAndStone.MaxRange = 2;

			// One ore per harvest action
			oreAndStone.ConsumedPerHarvest = 1;
			oreAndStone.ConsumedPerFeluccaHarvest = 1;

			// The digging effect
			oreAndStone.EffectActions = new int[]{ 11 };
			oreAndStone.EffectSounds = new int[]{ 0x125, 0x126 };
			oreAndStone.EffectCounts = new int[]{ 1 };
			oreAndStone.EffectDelay = TimeSpan.FromSeconds( 1.5 );
			oreAndStone.EffectSoundDelay = TimeSpan.FromSeconds( 0.7 );

			oreAndStone.NoResourcesMessage = 503040; // There is no metal here to mine.
			oreAndStone.DoubleHarvestMessage = 503042; // Someone has gotten to the metal before you.
			oreAndStone.TimedOutOfRangeMessage = 503041; // You have moved too far away to continue mining.
			oreAndStone.OutOfRangeMessage = 500446; // That is too far away.
			oreAndStone.FailMessage = 503043; // You loosen some rocks but fail to find any useable ore.
			oreAndStone.PackFullMessage = 1010481; // Your backpack is full, so the ore you mined is lost.
			oreAndStone.ToolBrokeMessage = 1044038; // You have worn out your tool!

			res = new HarvestResource[]
				{
					new HarvestResource( 00.0, 00.0, 120.0, "Voc� encontrou alguns min�rios de ferro.", typeof( IronOre ), typeof( Granite ) ),
					new HarvestResource( 65.0, 50.0, 120.0, "Voc� encontrou alguns min�rios de cobre r�stico.", typeof( DullCopperOre ), typeof( DullCopperGranite ), typeof( DullCopperElemental ) ),
					new HarvestResource( 70.0, 55.0, 120.0, "Voc� encontrou alguns min�rios de cobre.", typeof( CopperOre ), typeof( CopperGranite ), typeof( CopperElemental ) ),
					new HarvestResource( 75.0, 60.0, 120.0, "Voc� encontrou alguns min�rios de bronze.", typeof( BronzeOre ), typeof( BronzeGranite ), typeof( BronzeElemental ) ),
                    new HarvestResource( 80.0, 65.0, 120.0, "Voc� encontrou alguns min�rios de ferro negro.", typeof( ShadowIronOre ), typeof( ShadowIronGranite ), typeof( ShadowIronElemental ) ),
                    new HarvestResource( 85.0, 70.0, 120.0, "Voc� encontrou alguns min�rios de platina.", typeof( PlatinumOre ), typeof( PlatinumGranite ), typeof( EarthElemental ) ),
                    new HarvestResource( 85.0, 75.0, 120.0, "Voc� encontrou alguns min�rios de dourado.", typeof( GoldOre ), typeof( GoldGranite ), typeof( GoldenElemental ) ),
					new HarvestResource( 90.0, 80.0, 120.0, "Voc� encontrou alguns min�rios de agapite.", typeof( AgapiteOre ), typeof( AgapiteGranite ), typeof( AgapiteElemental ) ),
					new HarvestResource( 95.0, 85.0, 120.0, "Voc� encontrou alguns min�rios de verite.", typeof( VeriteOre ), typeof( VeriteGranite ), typeof( VeriteElemental ) ),
					new HarvestResource( 95.0, 85.0, 120.0, "Voc� encontrou alguns min�rios de valorite.", typeof( ValoriteOre ), typeof( ValoriteGranite ), typeof( ValoriteElemental ) ),
                    new HarvestResource( 100.0, 90.0, 120.0, "Voc� encontrou alguns min�rios de tit�nio.", typeof( TitaniumOre ), typeof( TitaniumGranite ), typeof( EarthElemental ) ),
                    new HarvestResource( 100.0, 90.0, 120.0, "Voc� encontrou alguns min�rios de ros�nio.", typeof( RoseniumOre ), typeof( RoseniumGranite ), typeof( EarthElemental ) ),
                    //new HarvestResource( 120.0, 100.0, 140.0, "Voc� encontrou alguns min�rios de dwarven.", typeof( DwarvenOre ), typeof( DwarvenGranite ), typeof( EarthElemental ) )
				};
			// the sum chance Needs to be 100%
			veins = new HarvestVein[]
				{
					new HarvestVein( 25.0, 0.0, res[0], null   ), // Iron
					new HarvestVein( 15.0, 0.5, res[1], res[0] ), // Dull Copper
					new HarvestVein( 13.0, 0.5, res[2], res[0] ), // Copper
					new HarvestVein( 10.0, 0.5, res[3], res[0] ), // Bronze
					new HarvestVein( 9.0, 0.5, res[4], res[0] ), // Shadow Iron
					new HarvestVein( 6.25, 0.5, res[5], res[0] ), // Platinum
					new HarvestVein( 6.25, 0.5, res[6], res[0] ), // Gold
					new HarvestVein( 4.5, 0.3, res[7], res[0] ), // Agapite
					new HarvestVein( 3.5, 0.2, res[8], res[0] ), // Verite
					new HarvestVein( 3.5, 0.2, res[9], res[0] ), // Valorite
					new HarvestVein( 2.0, 0.1, res[10], res[0] ), // Titanium
					new HarvestVein( 2.0, 0.1, res[11], res[0] ),  // Rosenium
                    //new HarvestVein( 70.0, 0.5, res[12], res[0] )  // Dwarven
				};

			oreAndStone.Resources = res;
			oreAndStone.Veins = veins;

            //PlayerMobile pm = from as PlayerMobile;
            //TreasureMap map = new TreasureMap(1, pm.Map, pm.Location, pm.X, pm.Y);

            oreAndStone.BonusResources = new BonusHarvestResource[]
            {
                    new BonusHarvestResource( 0, 89.75, null, null ),	//Nothing
					new BonusHarvestResource( 60, 5, 1074542, typeof( BlankScroll ) ),
                    new BonusHarvestResource( 60, 1, 1074542, typeof( LocalMap ) ),
                    new BonusHarvestResource( 60, 1, 1074542, typeof( IndecipherableMap ) ),
                    new BonusHarvestResource( 60, 1, 1074542, typeof( BlankMap ) ),
                    new BonusHarvestResource( 70, .5, 1074542, typeof( Amber ) ),
                    new BonusHarvestResource( 75, .5, 1074542, typeof( Amethyst ) ),
                    new BonusHarvestResource( 75, .5, 1074542, typeof( Citrine ) ),
                    new BonusHarvestResource( 80, .1, 1074542, typeof( Diamond ) ),
                    new BonusHarvestResource( 85, .1, 1074542, typeof( Emerald ) ),
                    new BonusHarvestResource( 85, .1, 1074542, typeof( Ruby ) ),
                    new BonusHarvestResource( 85, .1, 1074542, typeof( Sapphire ) ),
                    new BonusHarvestResource( 90, .05, 1074542, typeof( StarSapphire ) ),
                    new BonusHarvestResource( 90, .05, 1074542, typeof( Tourmaline ) ),
                    new BonusHarvestResource( 100, .05, 1072562, typeof( BlueDiamond ) ),
                    new BonusHarvestResource( 100, .05, 1072567, typeof( DarkSapphire ) ),
                    new BonusHarvestResource( 100, .05, 1072570, typeof( EcruCitrine ) ),
                    new BonusHarvestResource( 100, .05, 1072564, typeof( FireRuby ) ),
                    new BonusHarvestResource( 100, .05, 1072566, typeof( PerfectEmerald ) )
                //new BonusHarvestResource( 100, .1, 1072568, typeof( Turquoise ) ),

            };

            oreAndStone.RaceBonus = false;//Core.ML;
			oreAndStone.RandomizeVeins = true;//Core.ML;

			Definitions.Add( oreAndStone );
			#endregion

			#region Mining for sand
			HarvestDefinition sand = m_Sand = new HarvestDefinition();

			// Resource banks are every 3x3 tiles
			sand.BankWidth = 3;
			sand.BankHeight = 3;

			// Every bank holds from 9 to 18 sand
			sand.MinTotal = 9;
			sand.MaxTotal = 18;

			// A resource bank will respawn its content every 10 to 20 minutes
			sand.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			sand.MaxRespawn = TimeSpan.FromMinutes( 20.0 );

			// Skill checking is done on the Mining skill
			sand.Skill = SkillName.Mining;

			// Set the list of harvestable tiles
			sand.Tiles = m_SandTiles;

			// Players must be within 2 tiles to harvest
			sand.MaxRange = 2;

			// One sand per harvest action
			sand.ConsumedPerHarvest = 1;
			sand.ConsumedPerFeluccaHarvest = 1;

			// The digging effect
			sand.EffectActions = new int[]{ 11 };
			sand.EffectSounds = new int[]{ 0x125, 0x126 };
			sand.EffectCounts = new int[]{ 6 };
			sand.EffectDelay = TimeSpan.FromSeconds( 1.5 );
			sand.EffectSoundDelay = TimeSpan.FromSeconds( 0.7 );

			sand.NoResourcesMessage = 1044629; // There is no sand here to mine.
			sand.DoubleHarvestMessage = 1044629; // There is no sand here to mine.
			sand.TimedOutOfRangeMessage = 503041; // You have moved too far away to continue mining.
			sand.OutOfRangeMessage = 500446; // That is too far away.
			sand.FailMessage = 1044630; // You dig for a while but fail to find any of sufficient quality for glassblowing.
			sand.PackFullMessage = 1044632; // Your backpack can't hold the sand, and it is lost!
			sand.ToolBrokeMessage = 1044038; // You have worn out your tool!

			res = new HarvestResource[]
				{
                    new HarvestResource( 50.0, 30.0, 120.0, 1044631, typeof( Sand ) )
				};

			veins = new HarvestVein[]
				{
					new HarvestVein( 80.0, 0.0, res[0], null )
				};

			sand.Resources = res;
			sand.Veins = veins;

			Definitions.Add( sand );
			#endregion
		}

		public override Type GetResourceType( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource )
		{
			if ( def == m_OreAndStone )
			{
                #region Void Pool Items
                /*                HarvestMap hmap = HarvestMap.CheckMapOnHarvest(from, loc, def);

                                if (hmap != null && hmap.Resource >= CraftResource.Iron && hmap.Resource <= CraftResource.Dwarven)
                                {
                                    hmap.UsesRemaining--;
                                    hmap.InvalidateProperties();

                                    CraftResourceInfo info = CraftResources.GetInfo(hmap.Resource);

                                    if (info != null)
                                        return info.ResourceTypes[1];
                                }*/
                #endregion
                PlayerMobile pm = from as PlayerMobile;
				if (pm != null && 
					pm.StoneMining && 
					pm.ToggleMiningStone && 
					from.Skills[SkillName.Mining].Base >= 100.0 && 0.5 > Utility.RandomDouble())
				{
                    //from.SendMessage(20, "Mining =>" + resource.Types[1]);
                    return resource.Types[1];
                }

                return resource.Types[0];
			}

            return base.GetResourceType( from, tool, def, map, loc, resource );
		}

		public override bool CheckHarvest( Mobile from, Item tool )
		{
			if ( !base.CheckHarvest( from, tool ) )
				return false;

			if ( from.Mounted )
			{
				from.SendLocalizedMessage( 501864 ); // You can't mine while riding.
				return false;
			}
			else if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				from.SendLocalizedMessage( 501865 ); // You can't mine while polymorphed.
				return false;
			}

			return true;
		}

		public override void SendSuccessTo( Mobile from, Item item, HarvestResource resource )
		{
			if ( item is BaseGranite )
				from.SendLocalizedMessage( 1044606 ); // You carefully extract some workable stone from the ore vein!
			else
				base.SendSuccessTo( from, item, resource );
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				return false;

			if ( def == m_Sand && !(from is PlayerMobile && from.Skills[SkillName.Mining].Base >= 100.0 && ((PlayerMobile)from).SandMining) )
			{
				OnBadHarvestTarget( from, tool, toHarvest );
				return false;
			}
			else if ( from.Mounted )
			{
				from.SendLocalizedMessage( 501864 ); // You can't mine while riding.
				return false;
			}
			else if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				from.SendLocalizedMessage( 501865 ); // You can't mine while polymorphed.
				return false;
			}

			return true;
		}

		public override HarvestVein MutateVein( Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein )
		{
			if ( tool is GargoylesPickaxe && def == m_OreAndStone )
			{
				int veinIndex = Array.IndexOf( def.Veins, vein );

				if ( veinIndex >= 0 && veinIndex < (def.Veins.Length - 1) )
					return def.Veins[veinIndex + 1];
			}
			else if ( tool is OreShovel && def == m_OreAndStone ) // WIZARD ADDED
			{
				int veinIndex = Array.IndexOf( def.Veins, vein );
				return def.Veins[0];
			} 

			return base.MutateVein( from, tool, def, bank, toHarvest, vein );
		}

		private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				 0, -1,
				 0,  1,
				 1, -1,
				 1,  0,
				 1,  1
			};

		public override void OnHarvestFinished( Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested )
		{
			if ( tool is GargoylesPickaxe && def == m_OreAndStone && 0.1 > Utility.RandomDouble() )
			{
				HarvestResource res = vein.PrimaryResource;

				if ( res == resource && res.Types.Length >= 3 )
				{
					try
					{
						Map map = from.Map;

						if ( map == null )
							return;

						BaseCreature spawned = Activator.CreateInstance( res.Types[2], new object[]{ 25 } ) as BaseCreature;

						if ( spawned != null )
						{
							int offset = Utility.Random( 8 ) * 2;

							for ( int i = 0; i < m_Offsets.Length; i += 2 )
							{
								int x = from.X + m_Offsets[(offset + i) % m_Offsets.Length];
								int y = from.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

								if ( map.CanSpawnMobile( x, y, from.Z ) )
								{
									spawned.OnBeforeSpawn( new Point3D( x, y, from.Z ), map );
									spawned.MoveToWorld( new Point3D( x, y, from.Z ), map );
									spawned.Combatant = from;
									return;
								}
								else
								{
									int z = map.GetAverageZ( x, y );

									if ( map.CanSpawnMobile( x, y, z ) )
									{
										spawned.OnBeforeSpawn( new Point3D( x, y, z ), map );
										spawned.MoveToWorld( new Point3D( x, y, z ), map );
										spawned.Combatant = from;
										return;
									}
								}
							}

							spawned.OnBeforeSpawn( from.Location, from.Map );
							spawned.MoveToWorld( from.Location, from.Map );
							spawned.Combatant = from;
						}
					}
					catch
					{
					}
				}
			}
		}

		public override bool BeginHarvesting( Mobile from, Item tool )
		{
			if ( !base.BeginHarvesting( from, tool ) )
				return false;

			from.SendLocalizedMessage( 503033 ); // Where do you wish to dig?
			return true;
		}

		public override void OnHarvestStarted( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			base.OnHarvestStarted( from, tool, def, toHarvest );

			if ( Core.ML )
				from.RevealingAction();
		}

		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
			if ( toHarvest is LandTarget )
				from.SendLocalizedMessage( 501862 ); // You can't mine there.
			else
				from.SendLocalizedMessage( 501863 ); // You can't mine that.
		}

		#region Tile lists
		public static int[] m_MountainAndCaveTiles = new int[]
			{
				220, 221, 222, 223, 224, 225, 226, 227, 228, 229,
				230, 231, 236, 237, 238, 239, 240, 241, 242, 243,
				244, 245, 246, 247, 252, 253, 254, 255, 256, 257,
				258, 259, 260, 261, 262, 263, 268, 269, 270, 271,
				272, 273, 274, 275, 276, 277, 278, 279, 286, 287,
				288, 289, 290, 291, 292, 293, 294, 296, 296, 297,
				321, 322, 323, 324, 467, 468, 469, 470, 471, 472,
				473, 474, 476, 477, 478, 479, 480, 481, 482, 483,
				484, 485, 486, 487, 492, 493, 494, 495, 543, 544,
				545, 546, 547, 548, 549, 550, 551, 552, 553, 554,
				555, 556, 557, 558, 559, 560, 561, 562, 563, 564,
				565, 566, 567, 568, 569, 570, 571, 572, 573, 574,
				575, 576, 577, 578, 579, 581, 582, 583, 584, 585,
				586, 587, 588, 589, 590, 591, 592, 593, 594, 595,
				596, 597, 598, 599, 600, 601, 610, 611, 612, 613,

				1010, 1741, 1742, 1743, 1744, 1745, 1746, 1747, 1748, 1749,
				1750, 1751, 1752, 1753, 1754, 1755, 1756, 1757, 1771, 1772,
				1773, 1774, 1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782,
				1783, 1784, 1785, 1786, 1787, 1788, 1789, 1790, 1801, 1802,
				1803, 1804, 1805, 1806, 1807, 1808, 1809, 1811, 1812, 1813,
				1814, 1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823,
				1824, 1831, 1832, 1833, 1834, 1835, 1836, 1837, 1838, 1839,
				1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848, 1849,
				1850, 1851, 1852, 1853, 1854, 1861, 1862, 1863, 1864, 1865,
				1866, 1867, 1868, 1869, 1870, 1871, 1872, 1873, 1874, 1875,
				1876, 1877, 1878, 1879, 1880, 1881, 1882, 1883, 1884, 1981,
				1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991,
				1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999, 2000, 2001,
				2002, 2003, 2004, 2028, 2029, 2030, 2031, 2032, 2033, 2100,
				2101, 2102, 2103, 2104, 2105,

				0x453B, 0x453C, 0x453D, 0x453E, 0x453F, 0x4540, 0x4541,
				0x4542, 0x4543, 0x4544,	0x4545, 0x4546, 0x4547, 0x4548,
				0x4549, 0x454A, 0x454B, 0x454C, 0x454D, 0x454E,	0x454F//, 0x8E0, 0x8E3, 0x8E1, 0x8E5, 0x8E8
            };

		public static int[] m_SandTiles = new int[]
			{
				22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
				32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
				42, 43, 44, 45, 46, 47, 48, 49, 50, 51,
				52, 53, 54, 55, 56, 57, 58, 59, 60, 61,
				62, 68, 69, 70, 71, 72, 73, 74, 75,

				286, 287, 288, 289, 290, 291, 292, 293, 294, 295,
				296, 297, 298, 299, 300, 301, 402, 424, 425, 426,
				427, 441, 442, 443, 444, 445, 446, 447, 448, 449,
				450, 451, 452, 453, 454, 455, 456, 457, 458, 459,
				460, 461, 462, 463, 464, 465, 642, 643, 644, 645,
				650, 651, 652, 653, 654, 655, 656, 657, 821, 822,
				823, 824, 825, 826, 827, 828, 833, 834, 835, 836,
				845, 846, 847, 848, 849, 850, 851, 852, 857, 858,
				859, 860, 951, 952, 953, 954, 955, 956, 957, 958,
				967, 968, 969, 970,

				1447, 1448, 1449, 1450, 1451, 1452, 1453, 1454, 1455,
				1456, 1457, 1458, 1611, 1612, 1613, 1614, 1615, 1616,
				1617, 1618, 1623, 1624, 1625, 1626, 1635, 1636, 1637,
				1638, 1639, 1640, 1641, 1642, 1647, 1648, 1649, 1650
			};
		#endregion
	}
}