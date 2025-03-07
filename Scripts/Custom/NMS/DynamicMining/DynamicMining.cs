﻿/*
 * 
 * By Gargouille
 * Date: 21/08/2013
 * 
 * 
 */

using System;
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Misc;
using Server.Regions;

namespace Server.Engines.Harvest
{
	public class DynamicMining : HarvestSystem
	{

/*        private static DynamicMining m_System;

        public static DynamicMining System
        {
            get
            {
                if (m_System == null)
                    m_System = new DynamicMining();

                return m_System;
            }
        }*/

        public static HarvestSystem GetSystem(Item axe)
		{
			Map map;
			Point3D loc;

            Mobile m = (Mobile)axe.RootParentEntity;
            object root = axe.RootParent;

			if ( root == null )
			{
				map = axe.Map;
				loc = axe.Location;
			}
			else
			{
				map = ((IEntity)root).Map;
				loc = ((IEntity)root).Location;
			}
			
			IPooledEnumerable eable = map.GetMobilesInRange(loc, 5);
			
			foreach ( Mobile mob in eable )
			{
				if(mob is MineSpirit)//find a mine spot
				{
					MineSpirit mine = (MineSpirit)mob;
                    //m.SendMessage(35, "dist-> " + mine.GetDistanceToSqrt(loc));
                    //m.SendMessage(55, "Você sente que está próximo de um veio de minério.");
                    if (mine.GetDistanceToSqrt(loc) <= mine.Range)
					{
                        return mine.HarvestSystem; //return its system
					}
				}
			}
			
			return null;//Nothing to harvest
		}
		
		#region As Mining
		private HarvestDefinition m_Ore;

		public HarvestDefinition Ore
		{
			get{ return m_Ore; }
		}

		public DynamicMining()
		{
			#region Mining for ore
			HarvestDefinition ore = m_Ore = new HarvestDefinition();

			// Resource banks are every 3x3 tiles
			ore.BankWidth = 3;
			ore.BankHeight = 3;

			// Every bank holds from 4 to 28 ore
			ore.MinTotal = 4;
			ore.MaxTotal = 28;

			// A resource bank will respawn its content every 10 to 30 minutes
			ore.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			ore.MaxRespawn = TimeSpan.FromMinutes( 30 );

			// Skill checking is done on the Mining skill
			ore.Skill = SkillName.Mining;

			// Set the list of harvestable tiles
			ore.Tiles = m_MountainAndCaveTiles;

			// Players must be within 2 tiles to harvest
			ore.MaxRange = 2;

			// One ore per harvest action
			ore.ConsumedPerHarvest = 1;
			ore.ConsumedPerFeluccaHarvest = 1;

			// The digging effect
			ore.EffectActions = new int[]{ 11 };
			ore.EffectSounds = new int[]{ 0x125, 0x126 };
			ore.EffectCounts = new int[]{ 1 };
			ore.EffectDelay = TimeSpan.FromSeconds( 1.5 );
			ore.EffectSoundDelay = TimeSpan.FromSeconds( 0.7 );

			ore.NoResourcesMessage = 503040; // There is no metal here to mine.
			ore.DoubleHarvestMessage = 503042; // Someone has gotten to the metal before you.
			ore.TimedOutOfRangeMessage = 503041; // You have moved too far away to continue mining.
			ore.OutOfRangeMessage = 500446; // That is too far away.
			ore.FailMessage = 503043; // You loosen some rocks but fail to find any useable ore.
			ore.PackFullMessage = 1010481; // Your backpack is full, so the ore you mined is lost.
			ore.ToolBrokeMessage = 1044038; // You have worn out your tool!

			if ( Core.ML )
			{
				ore.BonusResources = new BonusHarvestResource[]
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
			}

			ore.RaceBonus = false;//Core.ML;
			ore.RandomizeVeins = true;

			Definitions.Add( ore );
			#endregion
		}

		public override Type GetResourceType( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource )
		{
			if ( def == m_Ore )
			{
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
			
			base.SendSuccessTo( from, item, resource );
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
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

		public override HarvestVein MutateVein( Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein )
		{
			return vein;
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
		private static int[] m_MountainAndCaveTiles = new int[]
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

		#endregion
		
		#endregion
	}
}