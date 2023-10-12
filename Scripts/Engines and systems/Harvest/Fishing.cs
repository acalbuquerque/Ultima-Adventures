using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;
using System.Collections;
using Server.Multis;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Misc;

namespace Server.Engines.Harvest
{
	public class Fishing : HarvestSystem
	{
		private static Fishing m_System;

		public static Fishing System
		{
			get
			{
				if ( m_System == null )
					m_System = new Fishing();

				return m_System;
			}
		}

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get{ return m_Definition; }
		}

		public Fishing()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region Fishing
			HarvestDefinition fish = m_Definition = new HarvestDefinition();

			// Resource banks are every 5x5 tiles
			fish.BankWidth = 5;
			fish.BankHeight = 5;

			// Every bank holds from 4 to 16 fish
			fish.MinTotal = 3;
			fish.MaxTotal = 12;

			// A resource bank will respawn its content every 10 to 20 minutes
			fish.MinRespawn = TimeSpan.FromMinutes( 15.0 );
			fish.MaxRespawn = TimeSpan.FromMinutes( 30.0 );

			// Skill checking is done on the Fishing skill
			fish.Skill = SkillName.Fishing;

			// Set the list of harvestable tiles
			fish.Tiles = m_WaterTiles;
			fish.RangedTiles = true;

			// Players must be within 4 tiles to harvest
			fish.MaxRange = 4;

			// One fish per harvest action
			fish.ConsumedPerHarvest = 1;
			fish.ConsumedPerFeluccaHarvest = 1;

			// The fishing
			fish.EffectActions = new int[]{ 12 };
			fish.EffectSounds = new int[0];
			fish.EffectCounts = new int[]{ 1 };
			fish.EffectDelay = TimeSpan.FromSeconds(3);
			fish.EffectSoundDelay = TimeSpan.FromSeconds( 1.5 );

			fish.NoResourcesMessage = 503172; // The fish don't seem to be biting here.
			fish.FailMessage = 503171; // You fish a while, but fail to catch anything.
			fish.TimedOutOfRangeMessage = 500976; // You need to be closer to the water to fish!
			fish.OutOfRangeMessage = 500976; // You need to be closer to the water to fish!
			fish.PackFullMessage = 503204; // You do not have room in your backpack for this.
			fish.ToolBrokeMessage = 503174; // You broke your fishing pole.

			res = new HarvestResource[]
				{
					new HarvestResource( 00.0, 00.0, 100.0, "Você pescou um peixe comum!", typeof( Fish ) ),
                    new HarvestResource( 85.0, 75.0, 120.0, "Você pescou um peixe gigante!", typeof( BigFish ) )
                };

            // the sum chance Needs to be 100%
            veins = new HarvestVein[]
				{
					new HarvestVein( 90.0, 0.0, res[0], null ),
                    new HarvestVein( 10.0, 0.5, res[1], res[0] )
                };

			fish.Resources = res;
			fish.Veins = veins;

            fish.BonusResources = new BonusHarvestResource[]
			{
				new BonusHarvestResource( 0, 99.0, null, null ),
				new BonusHarvestResource( 75.0, 1.0, "Eba! Você encontrou uma pérola negra.", typeof( BlackPearl ) ),
            };

			fish.RaceBonus = false;
            fish.RandomizeVeins = true;

			Definitions.Add( fish );
			#endregion
		}

        public override Type GetResourceType(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            if (def == m_Definition)
            {
                PlayerMobile pm = from as PlayerMobile;
/*                if (pm != null &&
                    pm.StoneMining &&
                    pm.ToggleMiningStone &&
                    from.Skills[SkillName.Mining].Base >= 100.0 && 0.5 > Utility.RandomDouble())
                {
                    //from.SendMessage(20, "Mining =>" + resource.Types[1]);
                    return resource.Types[1];
                }*/

                return resource.Types[0];
            }

            return base.GetResourceType(from, tool, def, map, loc, resource);
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            PlayerMobile pm = from as PlayerMobile;
            if (!base.CheckHarvest(from, tool))
                return false;

            if (from.Mounted)
            {
                pm.SendMessage(55, "Não é possível pescar em cima de um cavalo."); // You can't fish while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                pm.SendMessage(55, "Não é possível pescar enquanto está sob efeito de polimorfismo.");//from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }

            return true;
        }

        public override void SendSuccessTo(Mobile from, Item item, HarvestResource resource)
        {
            if (item is PearlSkull)
            {
                from.SendMessage(55, "Hmmmm...o crânio de alguém está preso no seu gancho!");
            }
            else if (item is NewFish)
            {
                from.SendMessage(55, "Você tirou um peixe exótico dessas águas!");
            }
            else if (item is RustyJunk)
            {
                from.SendMessage(55, "Você pescou um lixo enferrujado!");
            }
            else if (item is HighSeasRelic)
            {
                from.SendMessage(55, "Você pescou algo especial do naufrágio próximo a você!");
            }
            else if (item is WoodenChest || item is MetalGoldenChest || item is SunkenChest)
            {
                from.SendMessage(55, "Você tira um baú pesado dessas águas profundas!");
            }
            else
            {
                int number;
                string name;

                if (item is BaseMagicFish)
                {
                    number = 1008124;
                    name = "peixe mágico";
                }
                else if (item is Fish)
                {
                    number = 1008124;
					name = "peixe comum";//item.ItemData.Name;
                }
                else if (item is BaseShoes)
                {
                    number = 1008124;
                    name = "Calçados";//item.ItemData.Name;
                }
                else if (item is TreasureMap)
                {
                    number = 1008125;
                    name = "um pergaminho encharcado";
                }
                else if (item is MessageInABottle)
                {
                    number = 1008125;
                    name = "uma garrafa com pedido de socorro (SOS)";
                }
                else if (item is FishingNet || item is SpecialFishingNet || item is FabledFishingNet || item is NeptunesFishingNet)
                {
                    number = 1008125;
                    name = "uma rede de pesca";
                }
                else
                {
                    number = 1043297;

                    if (item.Name != null && item.Name != "")
                        name = item.Name;
                    else if ((item.ItemData.Flags & TileFlag.ArticleA) != 0)
                        name = "" + item.ItemData.Name;
                    else if ((item.ItemData.Flags & TileFlag.ArticleAn) != 0)
                        name = "" + item.ItemData.Name;
                    else
                        name = item.ItemData.Name;
                }

                // THIS HOPEFULLY FIXES THE ~1_val~a fish ISSUE
                NetState ns = from.NetState;

                if (ns == null)
                    return;

                if (number == 1043297 || ns.HighSeas)
                    from.SendLocalizedMessage(number, name);
                else
                    from.SendLocalizedMessage(number, true, name);
            }
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (from.Mounted)
            {
                pm.SendMessage(55, "Não é possível pescar em cima de um cavalo."); //from.SendLocalizedMessage(500971); // You can't fish while riding!
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                pm.SendMessage(55, "Não é possível pescar enquanto está sob efeito de polimorfismo.");//from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }

            return true;
        }

        public override HarvestVein MutateVein(Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein)
        {
/*            if (tool is GargoylesPickaxe && def == m_OreAndStone)
            {
                int veinIndex = Array.IndexOf(def.Veins, vein);

                if (veinIndex >= 0 && veinIndex < (def.Veins.Length - 1))
                    return def.Veins[veinIndex + 1];
            }
            else if (tool is OreShovel && def == m_OreAndStone) // WIZARD ADDED
            {
                int veinIndex = Array.IndexOf(def.Veins, vein);
                return def.Veins[0];
            }*/

            return base.MutateVein(from, tool, def, bank, toHarvest, vein);
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

        public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
        {
            from.RevealingAction();
            base.OnHarvestFinished(from, tool, def, vein, bank, resource, harvested);
        }

        public override bool BeginHarvesting(Mobile from, Item tool)
        {
            if (from.FindItemOnLayer(Layer.OneHanded) == null || tool != from.FindItemOnLayer(Layer.OneHanded))
            {
                from.SendMessage(55, "Você precisa estar segurando sua vara de pesca para pescar.");
                return false;
            }
            else if (!base.BeginHarvesting(from, tool))
                return false;

            from.SendMessage(55, "Onde você deseja pescar?");
            //from.SendLocalizedMessage(500974); // What water do you want to fish in?
            return true;
        }

        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);
            from.RevealingAction();

			int tileID;
			Map map;
			Point3D loc;

			if (GetHarvestDetails(from, tool, toHarvest, out tileID, out map, out loc))

				Timer.DelayCall(TimeSpan.FromSeconds(1.5),
				delegate
				{
					if (Core.ML)
						from.RevealingAction();

					Effects.SendLocationEffect(loc, map, 0x352D, 16, 4);
					Effects.PlaySound(loc, map, 0x364);
				});
		}

        public override void OnBadHarvestTarget(Mobile from, Item tool, object toHarvest)
        {
            from.SendMessage(55, "Não é possível pescar ali.");
/*            if (toHarvest is LandTarget)
                from.SendMessage(55, "Não é possível pescar ali.");*/
        }

        public static void FishingSkill(Mobile from, int c)
        {
            while (c > 0)
            {
                c--;
                from.CheckSkill(SkillName.Fishing, 0, 120);
            }
        }

		public override void OnConcurrentHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
		{
            from.SendMessage(55, "Você já está pescando!");
            //from.SendLocalizedMessage(500972); // You are already fishing.
		}

		private class MutateEntry
		{
			public string m_group;
			public double m_ReqSkill, m_MinSkill, m_MaxSkill;
			public bool m_DeepWater;
			public Type[] m_Types;

			public MutateEntry(string group, double reqSkill, double minSkill, double maxSkill, bool deepWater, params Type[] types)
			{
				m_group = group;
                m_ReqSkill = reqSkill;
				m_MinSkill = minSkill;
				m_MaxSkill = maxSkill;
				m_DeepWater = deepWater;
				m_Types = types;
			}
		}

		private static MutateEntry[] m_MutateTable = new MutateEntry[]
		{
            /*new MutateEntry("grupo1", 70.0, 70.0, 75.0, false, typeof( CorpseSailor ), typeof( RustyJunk ), typeof( WetClothes ) ), //min 0,02% - max 10%
            new MutateEntry("grupo2", 75.0, 75.0, 80, false, typeof( FishingNet ), typeof( SpecialSeaweed ) , typeof( BlackPearl ) ), //min 0,02% - max 9%
            new MutateEntry("grupo3", 80.0, 80.0, 85.0, false, typeof( PrizedFish ), typeof( InvisibleFish ), typeof( PoisonFish ) ), //min 0,02% - max 8%
            new MutateEntry("grupo4", 85.0, 85.0, 90.0, false, typeof( WondrousFish ), typeof( StaminaFish ), typeof( HealFish ), typeof( ManaFish ) ), //min 0,02% - max 7%
            new MutateEntry("grupo5", 90.0, 90.0, 95.0, false, typeof( TrulyRareFish )*//*, typeof( PeculiarFish )*//* ), //min 0,02% - max 6%
            new MutateEntry("grupo6", 95.0, 95.0, 100.0, false, typeof( NewFish ), typeof( PearlSkull ) ), //min 0,02% - max 5%
			new MutateEntry("grupo7", 100.0, 100.0,  105.0, false,typeof( SunkenBag )), //min 0,02% - max 4%
            new MutateEntry("grupo8", 105.0,  105.0,  110.0,  true, typeof( SpecialFishingNet ), typeof( NeptunesFishingNet ), typeof( FabledFishingNet ) ), //min 0,2% - max 3%*/
            new MutateEntry("grupo9", 100.0, 20.0,  21.0, true, typeof( MessageInABottle ) ) //min 0,02% - max 1%
        };

		public override Type MutateType(Type type, Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
		{
			bool deepWater = false;
            PlayerMobile pm = (PlayerMobile)from;

            if (Server.Misc.Worlds.IsOnBoat(from) && !Server.Misc.Worlds.BoatToCloseToTown(from))
				deepWater = true;

			double skillBase = from.Skills[SkillName.Fishing].Base;
			double skillValue = from.Skills[SkillName.Fishing].Value;

            for (int i = 0; i < m_MutateTable.Length; ++i)
			{
				MutateEntry entry = m_MutateTable[i];

				if (!deepWater && entry.m_DeepWater)
					continue;

				if (skillBase >= entry.m_ReqSkill)
                { //min 0,02%

                    from.SendMessage(33, "*** NOME: " + entry.m_group);

                    double chance = ( (skillValue - entry.m_MinSkill) / (entry.m_MaxSkill - entry.m_MinSkill) ) / 100;
                    double random = Utility.RandomDouble();

                    from.SendMessage(65, "*** CHANCE: " + chance + "*** RANDOM: " + random);
                    
					if (chance >= random) 
					{
                        pm.PlaySound(pm.Female ? 783 : 1054);
                        pm.Say("*woohoo!*");
                        from.SendMessage(55, "Que sorte! Você pescou algo inesperado e colocou em sua mochila!");
                        return entry.m_Types[Utility.Random(entry.m_Types.Length)];
                    }
				}
			}

			return type;
		}

		private static Map SafeMap(Map map)
		{
			if (map == null || map == Map.Internal)
				return Map.Trammel;

			return map;
		}

		/*public static bool IsNearHugeShipWreck( Mobile from )
		{
			if ( from.InRange( new Point3D(578, 1370, -5), 36 ) && from.Map == Map.Ilshenar )
				return true;
			else if ( from.InRange( new Point3D(946, 821, -5), 36 ) && from.Map == Map.TerMur )
				return true;
			else if ( from.InRange( new Point3D(969, 217, -5), 36 ) && from.Map == Map.TerMur )
				return true;
			else if ( from.InRange( new Point3D(322, 661, -5), 36 ) && from.Map == Map.TerMur )
				return true;
			else if ( from.InRange( new Point3D(760, 587, -5), 36 ) && from.Map == Map.Tokuno )
				return true;
			else if ( from.InRange( new Point3D(200, 1056, -5), 36 ) && from.Map == Map.Tokuno )
				return true;
			else if ( from.InRange( new Point3D(1232, 387, -5), 36 ) && from.Map == Map.Tokuno )
				return true;
			else if ( from.InRange( new Point3D(528, 233, -5), 36 ) && from.Map == Map.Tokuno )
				return true;
			else if ( from.InRange( new Point3D(504, 1931, -5), 36 ) && from.Map == Map.Malas )
				return true;
			else if ( from.InRange( new Point3D(1472, 1776, -5), 36 ) && from.Map == Map.Malas )
				return true;
			else if ( from.InRange( new Point3D(1560, 579, -5), 36 ) && from.Map == Map.Malas )
				return true;
			else if ( from.InRange( new Point3D(1328, 144, -5), 36 ) && from.Map == Map.Malas )
				return true;
			else if ( from.InRange( new Point3D(2312, 2299, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(2497, 3217, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(576, 3523, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(4352, 3768, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(4824, 1627, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(3208, 216, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(1112, 619, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(521, 2153, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(2920, 1643, -5), 36 ) && from.Map == Map.Felucca )
				return true;
			else if ( from.InRange( new Point3D(320, 2288, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(3343, 1842, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(3214, 938, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(4520, 1128, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(4760, 2307, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(3551, 2952, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(1271, 2651, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(744, 1304, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(735, 555, -5), 36 ) && from.Map == Map.Trammel )
				return true;
			else if ( from.InRange( new Point3D(1824, 440, -5), 36 ) && from.Map == Map.Trammel )
				return true;

			return false;
		}*/


		/*public static bool IsNearSpaceCrash( Mobile from )
		{
			if ( from.X >= 457 && from.X <= 494 && from.Y >= 1785 && from.Y <= 1821 && from.Map == Map.Trammel )
				return true;


			if ( from.X >= 4430 && from.X <= 4501 && from.Y >= 589 && from.Y <= 661 && from.Map == Map.Trammel )
				return true;


			return false;
		}*/


		/*public static bool IsNearUnderwaterRuins( Mobile from )
		{
			if ( from.X >= 4342 && from.X <= 4420 && from.Y >= 2766 && from.Y <= 2845 && from.Map == Map.Trammel )
				return true;


			if ( from.X >= 175 && from.X <= 243 && from.Y >= 2316 && from.Y <= 2344 && from.Map == Map.Trammel )
				return true;


			if ( from.X >= 3664 && from.X <= 3737 && from.Y >= 2522 && from.Y <= 2594 && from.Map == Map.Trammel )
				return true;


			if ( from.X >= 1668 && from.X <= 1734 && from.Y >= 1309 && from.Y <= 1376 && from.Map == Map.Felucca )
				return true;


			if ( from.X >= 1573 && from.X <= 1634 && from.Y >= 3261 && from.Y <= 3326 && from.Map == Map.Felucca )
				return true;


			return false;
		}*/


		/*public static void FishUpFromSpaceship( Mobile from )
		{
			int nGuild = 0;


			PlayerMobile pc = (PlayerMobile)from;
			if ( pc.NpcGuild != NpcGuild.FishermensGuild )
			{
				nGuild = (int)(from.Skills[SkillName.Fishing].Value/4);
			}


			int nChance = (int)(from.Skills[SkillName.Fishing].Value/4) + nGuild;


			if ( nChance > Utility.Random(100) )
			{
				Item preLoot = Server.Items.DungeonLoot.RandomSpaceCrash();
				from.AddToBackpack ( preLoot );
				from.SendMessage("You fish up something from the wreckage below.");
			}
		}*/


		/*public static void FishUpFromRuins( Mobile from )
		{
			int nGuild = 1;


			PlayerMobile pc = (PlayerMobile)from;
			if ( pc.NpcGuild != NpcGuild.FishermensGuild )
			{
				nGuild = (int)(from.Skills[SkillName.Fishing].Value/25);
			}


			Item preLoot = new RustyJunk();


			int goldBoost = (int)(from.Skills[SkillName.Fishing].Value * nGuild);
			switch ( Utility.Random( 18 ) )
			{
				case 0: preLoot = new DDRelicLeather(); DDRelicLeather RL0 = (DDRelicLeather)preLoot; RL0.RelicGoldValue = RL0.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 1: preLoot = new DDRelicOrbs(); DDRelicOrbs RL1 = (DDRelicOrbs)preLoot; RL1.RelicGoldValue = RL1.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 2: preLoot = new DDRelicPainting(); DDRelicPainting RL2 = (DDRelicPainting)preLoot; RL2.RelicGoldValue = RL2.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 3: preLoot = new DDRelicRugAddonDeed(); DDRelicRugAddonDeed RL3 = (DDRelicRugAddonDeed)preLoot; RL3.RelicGoldValue = RL3.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 4: preLoot = new DDRelicScrolls(); DDRelicScrolls RL4 = (DDRelicScrolls)preLoot; RL4.RelicGoldValue = RL4.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 5: preLoot = new DDRelicStatue(); DDRelicStatue RL5 = (DDRelicStatue)preLoot; RL5.RelicGoldValue = RL5.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 6: preLoot = new DDRelicVase(); DDRelicVase RL6 = (DDRelicVase)preLoot; RL6.RelicGoldValue = RL6.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 7: preLoot = new DDRelicWeapon(); DDRelicWeapon RL7 = (DDRelicWeapon)preLoot; RL7.RelicGoldValue = RL7.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 8: preLoot = new DDRelicArmor(); DDRelicArmor RL8 = (DDRelicArmor)preLoot; RL8.RelicGoldValue = RL8.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 9: preLoot = new DDRelicArts(); DDRelicArts RL9 = (DDRelicArts)preLoot; RL9.RelicGoldValue = RL9.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 10: preLoot = new DDRelicBanner(); DDRelicBanner RL10 = (DDRelicBanner)preLoot; RL10.RelicGoldValue = RL10.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 11: preLoot = new DDRelicBearRugsAddonDeed(); DDRelicBearRugsAddonDeed RL11 = (DDRelicBearRugsAddonDeed)preLoot; RL11.RelicGoldValue = RL11.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 12: preLoot = new DDRelicBook(); DDRelicBook RL12 = (DDRelicBook)preLoot; RL12.RelicGoldValue = RL12.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 13: preLoot = new DDRelicCloth(); DDRelicCloth RL13 = (DDRelicCloth)preLoot; RL13.RelicGoldValue = RL13.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 14: preLoot = new DDRelicFur(); DDRelicFur RL14 = (DDRelicFur)preLoot; RL14.RelicGoldValue = RL14.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 15: preLoot = new DDRelicInstrument(); DDRelicInstrument RL15 = (DDRelicInstrument)preLoot; RL15.RelicGoldValue = RL15.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 16: preLoot = new DDRelicJewels(); DDRelicJewels RL16 = (DDRelicJewels)preLoot; RL16.RelicGoldValue = RL16.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
				case 17: 
					switch ( Utility.Random( 3 ) )
					{
						case 0: preLoot = new DDRelicClock1(); DDRelicClock1 RL17 = (DDRelicClock1)preLoot; RL17.RelicGoldValue = RL17.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 1: preLoot = new DDRelicClock2(); DDRelicClock2 RL18 = (DDRelicClock2)preLoot; RL18.RelicGoldValue = RL18.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 2: preLoot = new DDRelicClock3(); DDRelicClock3 RL19 = (DDRelicClock3)preLoot; RL19.RelicGoldValue = RL19.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
					}
				break;
			}


			from.AddToBackpack ( preLoot );
			from.SendMessage("You fish up something from the ruins below.");
		}*/


		/*public static void FishUpFromMajorWreck( Mobile from )
		{
			string ship = Server.Misc.RandomThings.GetRandomShipName( "", 0 );


			int nWave = Utility.Random( 7 );
			int nGuild = 0;

			PlayerMobile pc = (PlayerMobile)from;
			if ( pc.NpcGuild != NpcGuild.FishermensGuild )
			{
				nWave = Utility.Random( 8 );
				nGuild = (int)(from.Skills[SkillName.Fishing].Value/4);
			}

			int mLevel = (int)(from.Skills[SkillName.Fishing].Value/10) + 1;

			Item preLoot = new RustyJunk();
			int nChance = (int)(from.Skills[SkillName.Fishing].Value/4) + nGuild;

			switch ( nWave )
			{
				case 0: // Body parts
				{
					if ( nChance > Utility.Random(100) )
						preLoot = new RustyJunk();
					else
					{
						int[] list = new int[]
							{
								0x1CDD, 0x1CE5, // arm
								0x1CE0, 0x1CE8, // torso
								0x1CE1, 0x1CE9, // head
								0x1CE2, 0x1CEC, // leg
								0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3, 0x1AE4, // skulls
								0x1B09, 0x1B0A, 0x1B0B, 0x1B0C, 0x1B0D, 0x1B0E, 0x1B0F, 0x1B10, // bone piles
								0x1B15, 0x1B16 // pelvis bones
							};

						preLoot = new ShipwreckedItem( Utility.RandomList( list ), ship );
						preLoot.Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );
					}
					break;
				}
				case 1: // Paintings and portraits
				{
					if ( nChance > Utility.Random(100) )
					{
						preLoot = new DDRelicPainting();
						preLoot.Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );
						preLoot.Name = preLoot.Name + " (covered in muck)";
					}
					else
						preLoot = new ShipwreckedItem( Utility.Random( 0xE9F, 10 ), ship );

					break;
				}
				case 2: // Misc
				{
					if ( nChance > Utility.Random(100) )
					{
						switch ( Utility.Random( 4 ) )
						{
							case 0: preLoot = new DDRelicArts(); break;
							case 1: preLoot = new DDRelicDrink(); break;
							case 2: preLoot = new DDRelicInstrument(); break;
							case 3: preLoot = new DDRelicJewels(); break;
						}
					}
					else
					{
						preLoot = new ShipwreckedItem( Utility.Random( 0x13A4, 11 ), ship );
						preLoot.Hue = RandomThings.GetRandomColor(0);
					}
					break;
				}
				case 3: // Shells
				{
					if ( nChance > Utility.Random(100) )
						preLoot = new NewFish();
					else
						preLoot = new ShipwreckedItem( Utility.Random( 0xFC4, 9 ), ship );
					break;
				}
				case 4:	// Hats
				{
					if ( Utility.RandomBool() )
					{
						preLoot = new SkullCap();
						if ( Utility.Random(4) == 1 )
						{
							preLoot.Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );
							preLoot.Name = "soggy skullcap";
						}
						else
						{
							preLoot.Hue = RandomThings.GetRandomColor(0);
							preLoot.Name = "skullcap";
						}
					}
					else
					{
						preLoot = new TricorneHat();
						if ( Utility.Random(4) == 1 )
						{
							preLoot.Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );
							preLoot.Name = "soggy pirate hat";
						}
						else
						{
							preLoot.Hue = RandomThings.GetRandomColor(0);
							preLoot.Name = "pirate hat";
						}
					}
					break;
				}
				case 5: // Sea Relic
				{
					int[] list = new int[]
						{
							0x1EB5, // unfinished barrel
							0xA2A, // stool
							0xC1F, // broken clock
							0x1047, 0x1048, // globe
							0x1EB1, 0x1EB2, 0x1EB3, 0x1EB4 // barrel staves
						};

					if ( Utility.Random( list.Length + 1 ) == 0 )
						preLoot = new Candelabra();
					else
						preLoot = new ShipwreckedItem( Utility.RandomList( list ), ship );

					break;
				}
				case 6:	// Boots
				{
					preLoot = new ThighBoots(); preLoot.Name = "boots";
					switch ( Utility.Random( 4 ) )
					{
						case 1: preLoot = new Sandals(); preLoot.Name = "sandals"; break;
						case 2: preLoot = new Shoes(); preLoot.Name = "shoes"; break;
						case 3: preLoot = new Boots(); preLoot.Name = "boots"; break;
					}
					if ( Utility.Random(2) == 1 )
					{
						preLoot.Hue = Utility.RandomList( 0xB97, 0xB98, 0xB99, 0xB9A, 0xB88 );
						preLoot.Name = "soggy " + preLoot.Name;
					}
					break;
				}
				case 7:	// Random Relic
				{
					int goldBoost = (int)from.Skills[SkillName.Fishing].Value;
					switch ( Utility.Random( 18 ) )
					{
						case 0: preLoot = new DDRelicLeather(); DDRelicLeather RL0 = (DDRelicLeather)preLoot; RL0.RelicGoldValue = RL0.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 1: preLoot = new DDRelicOrbs(); DDRelicOrbs RL1 = (DDRelicOrbs)preLoot; RL1.RelicGoldValue = RL1.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 2: preLoot = new DDRelicPainting(); DDRelicPainting RL2 = (DDRelicPainting)preLoot; RL2.RelicGoldValue = RL2.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 3: preLoot = new DDRelicRugAddonDeed(); DDRelicRugAddonDeed RL3 = (DDRelicRugAddonDeed)preLoot; RL3.RelicGoldValue = RL3.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 4: preLoot = new DDRelicScrolls(); DDRelicScrolls RL4 = (DDRelicScrolls)preLoot; RL4.RelicGoldValue = RL4.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 5: preLoot = new DDRelicStatue(); DDRelicStatue RL5 = (DDRelicStatue)preLoot; RL5.RelicGoldValue = RL5.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 6: preLoot = new DDRelicVase(); DDRelicVase RL6 = (DDRelicVase)preLoot; RL6.RelicGoldValue = RL6.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 7: preLoot = new DDRelicWeapon(); DDRelicWeapon RL7 = (DDRelicWeapon)preLoot; RL7.RelicGoldValue = RL7.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 8: preLoot = new DDRelicArmor(); DDRelicArmor RL8 = (DDRelicArmor)preLoot; RL8.RelicGoldValue = RL8.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 9: preLoot = new DDRelicArts(); DDRelicArts RL9 = (DDRelicArts)preLoot; RL9.RelicGoldValue = RL9.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 10: preLoot = new DDRelicBanner(); DDRelicBanner RL10 = (DDRelicBanner)preLoot; RL10.RelicGoldValue = RL10.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 11: preLoot = new DDRelicBearRugsAddonDeed(); DDRelicBearRugsAddonDeed RL11 = (DDRelicBearRugsAddonDeed)preLoot; RL11.RelicGoldValue = RL11.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 12: preLoot = new DDRelicBook(); DDRelicBook RL12 = (DDRelicBook)preLoot; RL12.RelicGoldValue = RL12.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 13: preLoot = new DDRelicCloth(); DDRelicCloth RL13 = (DDRelicCloth)preLoot; RL13.RelicGoldValue = RL13.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 14: preLoot = new DDRelicFur(); DDRelicFur RL14 = (DDRelicFur)preLoot; RL14.RelicGoldValue = RL14.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 15: preLoot = new DDRelicInstrument(); DDRelicInstrument RL15 = (DDRelicInstrument)preLoot; RL15.RelicGoldValue = RL15.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 16: preLoot = new DDRelicJewels(); DDRelicJewels RL16 = (DDRelicJewels)preLoot; RL16.RelicGoldValue = RL16.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
						case 17: 
							switch ( Utility.Random( 3 ) )
							{
								case 0: preLoot = new DDRelicClock1(); DDRelicClock1 RL17 = (DDRelicClock1)preLoot; RL17.RelicGoldValue = RL17.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
								case 1: preLoot = new DDRelicClock2(); DDRelicClock2 RL18 = (DDRelicClock2)preLoot; RL18.RelicGoldValue = RL18.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
								case 2: preLoot = new DDRelicClock3(); DDRelicClock3 RL19 = (DDRelicClock3)preLoot; RL19.RelicGoldValue = RL19.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
							}
						break;
					}
					break;
				}
			}

			from.AddToBackpack ( preLoot );
			from.SendMessage("You fish up something from the wreckage below.");
		}*/

		public override bool CheckResources(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed)
		{
			Container pack = from.Backpack;
            PlayerMobile pm = (PlayerMobile)from;
            if (pack != null)
			{
				List<SOS> messages = pack.FindItemsByType<SOS>();

				for (int i = 0; i < messages.Count; ++i)
				{
					SOS sos = messages[i];

					if (from.Map == sos.TargetMap && from.InRange(sos.TargetLocation, 60)) 
					{
                        pm.SendMessage(55, "Você encontrou destroços de um naufrágio!");
                        return true;
                    }
				}
			}

			return base.CheckResources(from, tool, def, map, loc, timed);
		}

		private static void GetRandomAOSStats(out int attributeCount, out int min, out int max, int level)
		{
			int rnd = Utility.Random(15);
			attributeCount = Utility.RandomMinMax(1, level);
			min = level * 3; max = level * 7;
		}

		public override Item Construct(Type type, Mobile from)
		{
/*			if (type == typeof(TreasureMap))
			{
				int level = Utility.RandomMinMax(1, 3);

				return new TreasureMap(level, from.Map, from.Location, from.X, from.Y);
			}*/
            
            if (type == typeof(MessageInABottle))
			{
				return new MessageInABottle(from.Map, 0, from.Location, from.X, from.Y);
			}
            Container pack = from.Backpack;

			if (pack != null)
			{
				List<SOS> messages = pack.FindItemsByType<SOS>();

				for (int i = 0; i < messages.Count; ++i)
				{
					SOS sos = messages[i];

					if (from.Map == sos.TargetMap && from.InRange(sos.TargetLocation, 60)) // WIZARD ADDED FOR MULTI-FACET SOS
					{
						int nWave = Utility.Random(8);
						int nGuild = 0;

						PlayerMobile pm = (PlayerMobile)from;

						if (pm.NpcGuild != NpcGuild.FishermensGuild)
						{
							nWave = Utility.Random(11);
							nGuild = (int)(from.Skills[SkillName.Fishing].Value / 4);
						}

						int mLevel = (int)(from.Skills[SkillName.Fishing].Value / 10) + 1;

						Item preLoot = null;
						int nChance = (int)(from.Skills[SkillName.Fishing].Value / 4) + nGuild;
                        //pm.SendMessage(66, "*** nWAVE: "+ nWave + " - Chance: "+ nChance);
                        #region LOOT in treasure
                        switch (nWave)
						{
							case 1: // Body parts
								{
									if (nChance > Utility.Random(100))
										preLoot = new RustyJunk();
									else
									{
										int[] list = new int[]
											{
													0x1CDD, 0x1CE5, // arm
													0x1CE0, 0x1CE8, // torso
													0x1CE1, 0x1CE9, // head
													0x1CE2, 0x1CEC, // leg
													0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3, 0x1AE4, // skulls
													0x1B09, 0x1B0A, 0x1B0B, 0x1B0C, 0x1B0D, 0x1B0E, 0x1B0F, 0x1B10, // bone piles
													0x1B15, 0x1B16 // pelvis bones
											};

										preLoot = new ShipwreckedItem(Utility.RandomList(list), sos.ShipName);
										preLoot.Hue = Utility.RandomList(0xB97, 0xB98, 0xB99, 0xB9A, 0xB88);
									}
									break;
								}
							case 2: // Paintings and portraits
								{
									if (nChance > Utility.Random(100))
									{
										preLoot = new DDRelicPainting();
										preLoot.Hue = Utility.RandomList(0xB97, 0xB98, 0xB99, 0xB9A, 0xB88);
										preLoot.Name = preLoot.Name + " (coberto de lodo)";
									}
									else
										preLoot = new ShipwreckedItem(Utility.Random(0xE9F, 10), sos.ShipName);

									break;
								}
							case 3: // Misc
								{
									if (nChance > Utility.Random(100))
									{
										switch (Utility.Random(4))
										{
											case 0: preLoot = new DDRelicArts(); break;
											case 1: preLoot = new DDRelicDrink(); break;
											case 2: preLoot = new DDRelicInstrument(); break;
											case 3: preLoot = new DDRelicJewels(); break;
										}
									}
									else
									{
										if (Utility.Random(20) == 1)
										{
											preLoot = new ShipwreckedItem(Utility.RandomList(0x4FE8, 0x4FE9), sos.ShipName);
										}
										else
										{
											preLoot = new ShipwreckedItem(Utility.Random(0x13A4, 11), sos.ShipName);
											preLoot.Hue = RandomThings.GetRandomColor(0);
										}
									}
									break;
								}
							case 4: // Shells
								{
									if (nChance > Utility.Random(100))
										preLoot = new NewFish();
									else
										preLoot = new ShipwreckedItem(Utility.Random(0xFC4, 9), sos.ShipName);
									break;
								}
							case 5: // Hats
								{
									if (nChance > Utility.Random(100))
									{
										preLoot = new MagicHat();
										string sAdj = "magical";
										switch (Utility.RandomMinMax(1, 7))
										{
											case 1: sAdj = "magical "; break;
											case 2: sAdj = "magic "; break;
											case 3: sAdj = "mystical "; break;
											case 4: sAdj = "enchanted "; break;
											case 5: sAdj = "mysterious "; break;
											case 6: sAdj = "mythical "; break;
											case 7: sAdj = "unusual "; break;
										}
										if (Utility.RandomBool()) { preLoot.Name = sAdj + "skullcap"; preLoot.ItemID = 5444; }
										else { preLoot.Name = sAdj + "pirate hat"; preLoot.ItemID = 5915; }

										int attributeCount;
										int min, max;
										GetRandomAOSStats(out attributeCount, out min, out max, mLevel);
										BaseRunicTool.ApplyAttributesTo((BaseJewel)preLoot, attributeCount, min, max);
									}
									else
									{
										if (Utility.RandomBool())
										{
											preLoot = new SkullCap();
											if (Utility.Random(4) == 1)
											{
												preLoot.Hue = Utility.RandomList(0xB97, 0xB98, 0xB99, 0xB9A, 0xB88);
												preLoot.Name = "soggy skullcap";
											}
											else
											{
												preLoot.Hue = RandomThings.GetRandomColor(0);
												preLoot.Name = "skullcap";
											}
										}
										else
										{
											preLoot = new TricorneHat();
											if (Utility.Random(4) == 1)
											{
												preLoot.Hue = Utility.RandomList(0xB97, 0xB98, 0xB99, 0xB9A, 0xB88);
												preLoot.Name = "soggy pirate hat";
											}
											else
											{
												preLoot.Hue = RandomThings.GetRandomColor(0);
												preLoot.Name = "pirate hat";
											}
										}
									}
									break;
								}
							case 6: // Sea Relic
								{
									if (nChance > Utility.Random(100))
									{
										preLoot = new HighSeasRelic();
										HighSeasRelic relic = (HighSeasRelic)preLoot;
										relic.RelicOrigin = "Resgatado do Naufrágio: [ " + sos.ShipName + " ]";
										int nGold = (int)(from.Skills[SkillName.Fishing].Value / 20);
										relic.RelicGoldValue = relic.RelicGoldValue * nGold;
									}
									else
									{
										int[] list = new int[]
											{
													0x1EB5, // unfinished barrel
													0xA2A, // stool
													0xC1F, // broken clock
													0x1047, 0x1048, // globe
													0x1EB1, 0x1EB2, 0x1EB3, 0x1EB4 // barrel staves
											};

										if (Utility.Random(list.Length + 1) == 0)
											preLoot = new Candelabra();
										else
											preLoot = new ShipwreckedItem(Utility.RandomList(list), sos.ShipName);
									}
									break;
								}
							case 7: // Boots
								{
									if (nChance > Utility.Random(100))
									{
										preLoot = new MagicBoots();
										int attributeCount;
										int min, max;
										GetRandomAOSStats(out attributeCount, out min, out max, mLevel);
										BaseRunicTool.ApplyAttributesTo((BaseJewel)preLoot, attributeCount, min, max);
									}
									else
									{
										preLoot = new ThighBoots(); preLoot.Name = "botas";
										switch (Utility.Random(4))
										{
											case 1: preLoot = new Sandals(); preLoot.Name = "sandálias"; break;
											case 2: preLoot = new Shoes(); preLoot.Name = "sapatos"; break;
											case 3: preLoot = new Boots(); preLoot.Name = "botas"; break;
										}
										if (Utility.Random(2) == 1)
										{
											preLoot.Hue = Utility.RandomList(0xB97, 0xB98, 0xB99, 0xB9A, 0xB88);
											preLoot.Name = preLoot.Name + "encharcada(o)s";
										}
									}
									break;
								}
							case 8: // Random Relic
								{
									int goldBoost = (int)from.Skills[SkillName.Fishing].Value;
									switch (Utility.Random(20))
									{
										case 0: preLoot = new DDRelicLeather(); DDRelicLeather RL0 = (DDRelicLeather)preLoot; RL0.RelicGoldValue = RL0.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 1: preLoot = new DDRelicOrbs(); DDRelicOrbs RL1 = (DDRelicOrbs)preLoot; RL1.RelicGoldValue = RL1.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 2: preLoot = new DDRelicPainting(); DDRelicPainting RL2 = (DDRelicPainting)preLoot; RL2.RelicGoldValue = RL2.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 3: preLoot = new DDRelicRugAddonDeed(); DDRelicRugAddonDeed RL3 = (DDRelicRugAddonDeed)preLoot; RL3.RelicGoldValue = RL3.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 4: preLoot = new DDRelicScrolls(); DDRelicScrolls RL4 = (DDRelicScrolls)preLoot; RL4.RelicGoldValue = RL4.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 5: preLoot = new DDRelicStatue(); DDRelicStatue RL5 = (DDRelicStatue)preLoot; RL5.RelicGoldValue = RL5.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 6: preLoot = new DDRelicVase(); DDRelicVase RL6 = (DDRelicVase)preLoot; RL6.RelicGoldValue = RL6.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 7: preLoot = new DDRelicWeapon(); DDRelicWeapon RL7 = (DDRelicWeapon)preLoot; RL7.RelicGoldValue = RL7.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 8: preLoot = new DDRelicArmor(); DDRelicArmor RL8 = (DDRelicArmor)preLoot; RL8.RelicGoldValue = RL8.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 9: preLoot = new DDRelicArts(); DDRelicArts RL9 = (DDRelicArts)preLoot; RL9.RelicGoldValue = RL9.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 10: preLoot = new DDRelicBanner(); DDRelicBanner RL10 = (DDRelicBanner)preLoot; RL10.RelicGoldValue = RL10.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 11: preLoot = new DDRelicBearRugsAddonDeed(); DDRelicBearRugsAddonDeed RL11 = (DDRelicBearRugsAddonDeed)preLoot; RL11.RelicGoldValue = RL11.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 12: preLoot = new DDRelicBook(); DDRelicBook RL12 = (DDRelicBook)preLoot; RL12.RelicGoldValue = RL12.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 13: preLoot = new DDRelicCloth(); DDRelicCloth RL13 = (DDRelicCloth)preLoot; RL13.RelicGoldValue = RL13.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 14: preLoot = new DDRelicFur(); DDRelicFur RL14 = (DDRelicFur)preLoot; RL14.RelicGoldValue = RL14.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 15: preLoot = new DDRelicInstrument(); DDRelicInstrument RL15 = (DDRelicInstrument)preLoot; RL15.RelicGoldValue = RL15.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 16: preLoot = new DDRelicJewels(); DDRelicJewels RL16 = (DDRelicJewels)preLoot; RL16.RelicGoldValue = RL16.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
										case 17:
											switch (Utility.Random(3))
											{
												case 0: preLoot = new DDRelicClock1(); DDRelicClock1 RL17 = (DDRelicClock1)preLoot; RL17.RelicGoldValue = RL17.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
												case 1: preLoot = new DDRelicClock2(); DDRelicClock2 RL18 = (DDRelicClock2)preLoot; RL18.RelicGoldValue = RL18.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
												case 2: preLoot = new DDRelicClock3(); DDRelicClock3 RL19 = (DDRelicClock3)preLoot; RL19.RelicGoldValue = RL19.RelicGoldValue + goldBoost; preLoot.InvalidateProperties(); break;
											}
											break;
										case 18:
											switch (Utility.Random(8))
											{
												case 0: preLoot = new TreasurePile01AddonDeed(); break;
												case 1: preLoot = new TreasurePileAddonDeed(); break;
												case 2: preLoot = new TreasurePile2AddonDeed(); break;
												case 3: preLoot = new TreasurePile2AddonDeed(); break;
												case 4: preLoot = new TreasurePile03AddonDeed(); break;
												case 5: preLoot = new TreasurePile3AddonDeed(); break;
												case 6: preLoot = new TreasurePile04AddonDeed(); break;
												case 7: preLoot = new TreasurePile05AddonDeed(); break;
											}
											break;
										case 19:
											switch (Utility.Random(8))
											{
												case 0: preLoot = new GalleonAddonDeed(); break;
												case 1: preLoot = new PirateShipAddonDeed(); break;
												case 2: preLoot = new VikingBoatAddonDeed(); break;
												case 3: preLoot = new VikingBoatSailAddonDeed(); break;
											}
											break;
									}
									break;
								}
							case 9: // Container with Items
								{
									int golden = (int)(from.Skills[SkillName.Fishing].Value / 10) + 3;
									if (Utility.RandomBool())
									{
										switch (Utility.Random(2))
										{
											case 0: preLoot = new CorpseSailor(golden); break;
											case 1: preLoot = new SunkenBag(golden); break;
										}
									}
									else
									{
										preLoot = new LootChest(golden);
										preLoot.Hue = 0;
										switch (Utility.Random(7))
										{
											case 0: preLoot.ItemID = Utility.RandomList(0x4FDB, 0x4FDC); preLoot.Name = "estante do capitão do mar"; break;
											case 1: preLoot.ItemID = Utility.RandomList(0x4FF4, 0x4FF5); preLoot.Name = "baú com cracas"; break;
											case 2: preLoot.ItemID = Utility.RandomList(0x4FEA, 0x4FEB); preLoot.Name = "baú real"; break;
											case 3: preLoot.ItemID = Utility.RandomList(0x4FEC, 0x4FED); preLoot.Name = "gabinete real"; break;
											case 4: preLoot.ItemID = Utility.RandomList(0x4FEE, 0x4FEF); preLoot.Name = "gabinete real"; break;
											case 5: preLoot.ItemID = Utility.RandomList(0x4FF0, 0x4FF0); preLoot.Name = "armário real"; break;
											case 6: preLoot.ItemID = Utility.RandomList(0x4FF2, 0x4FF3); preLoot.Name = "baú real"; break;
										}
									}
									break;
								}
						}
                        #endregion

                        if (preLoot != null)
						{
							if (preLoot is IShipwreckedItem)
								((IShipwreckedItem)preLoot).IsShipwreckedItem = true;

                            pm.SendMessage(55, "Você encontrou destroços de um naufrágio!");
                            return preLoot;
						}

						int IamAncient = 0;
						if (sos.IsAncient)
							IamAncient = 1;

						SunkenChest chest = new SunkenChest(sos.Level, from, IamAncient);
						FishingSkill(from, 10);
						LoggingFunctions.LogQuestSea(from, sos.Level, sos.ShipName);

						// ------------------------------------------------------------------------

						BaseCreature creature = new DeepSeaSerpent();
						string Screature = "Serpente Marinha";

						int monster = Utility.RandomMinMax(16, 19);

						if (sos.Level < 2) { monster = Utility.RandomMinMax(1, 4); }
						else if (sos.Level == 2) { monster = Utility.RandomMinMax(5, 9); }
						else if (sos.Level == 3) { monster = Utility.RandomMinMax(10, 11); }
						else if (sos.Level == 4) { monster = Utility.RandomMinMax(12, 15); }

						switch (monster)
						{
							case 1: creature = new WaterNaga(); Screature = "Naga d´agua"; break; // 1
							case 2: creature = new GiantEel(); Screature = "Enguia de Gigante"; break; // 1
							case 3: creature = new SeaSerpent(); Screature = "Serpente Marinha"; break; // 1
							case 4: creature = new Shark(); Screature = "Tubarão-Martelo"; break; // 1
							case 5: creature = new SeaDrake(); Screature = "Draconete Marinho"; break; // 2
							case 6: creature = new DeepSeaSerpent(); Screature = "Grande Serpente Marinha"; break; // 2
							case 7: creature = new GreatWhite(); Screature = "Tubarão-Branco"; break; // 2
							case 8: creature = new SeaHag(); Screature = "Bruxa do mar"; break; // 2
							case 9: creature = new EyeOfTheDeep(); Screature = "Observador Marinho"; break; // 2
							case 10: creature = new SeaDragon(); Screature = "Dragão Marinho"; break; // 3
							case 11: creature = new DemonOfTheSea(); Screature = "Demônio Marinho"; break; // 3
							case 12: creature = new SeaGiant(); Screature = "Kaiju Marinho"; break; // 4
							case 13: creature = new StormGiant(); Screature = "Kaiju Marinho"; break; // 4
							case 14: creature = new GiantSquid(); Screature = "Lula Gigante"; break; // 4
							case 15: creature = new SeaHagGreater(); Screature = "Bruxa Marinha"; break; // 4
							case 16: creature = new Calamari(); Screature = "Calamari Gigante"; break; // 5
							case 17: creature = new Kraken(); Screature = "Kraken"; break; // 5
							case 18: creature = new Megalodon(); Screature = "Megalodon"; break; // 5
							case 19: creature = new DeepSeaDragon(); Screature = "Leviatã"; break; // 5
						}

						int x = from.X, y = from.Y;

						Map map = from.Map;

						for (int c = 0; map != null && c < 20; ++c)
						{
							int tx = from.X - 10 + Utility.Random(21);
							int ty = from.Y - 10 + Utility.Random(21);

							LandTile t = map.Tiles.GetLandTile(tx, ty);

							if (t.Z == -5 && Server.Misc.Worlds.IsWaterTile(t.ID, 0) && !Spells.SpellHelper.CheckMulti(new Point3D(tx, ty, -5), map))
							{
								x = tx;
								y = ty;
								break;
							}
						}
                        
						pm.SendMessage(66, "Você encontra um monstro marinho que guarda o naufrágio.");
                        
						creature.MoveToWorld(new Point3D(x, y, -5), map);
						creature.Home = creature.Location;
						creature.WhisperHue = 999;
						creature.Name = Screature;
						creature.RangeHome = 10;
						creature.PackItem(chest);

						// ------------------------------------------------------------------------

						sos.Delete();

						switch (Utility.Random(9))
						{
							case 0: preLoot = ArtifactBuilder.CreateArtifact("driftwood"); break;
							case 1: preLoot = ArtifactBuilder.CreateArtifact("kelp"); break;
							case 2: preLoot = ArtifactBuilder.CreateArtifact("barnacle"); break;
							case 3: preLoot = ArtifactBuilder.CreateArtifact("bronzed"); break;
							case 4: preLoot = ArtifactBuilder.CreateArtifact("neptune"); break;
							case 5: preLoot = new ShipwreckedItem(Utility.Random(0xE9F, 10), sos.ShipName); break;
							case 6: preLoot = new ShipwreckedItem(Utility.Random(0x13A4, 11), sos.ShipName); break;
							case 7: preLoot = new ShipwreckedItem(Utility.Random(0xFC4, 9), sos.ShipName); break;
							case 8:
								preLoot = new HighSeasRelic();
								HighSeasRelic relic = (HighSeasRelic)preLoot;
								relic.RelicOrigin = "Resgatado do Naufrágio: [ " + sos.ShipName + " ]";
								int nGold = (int)(from.Skills[SkillName.Fishing].Value / 20);
								relic.RelicGoldValue = relic.RelicGoldValue * nGold;
								break;
						}

						from.SendMessage(55, "Seu gancho pescou uma criatura no naufrágio abaixo!");

						return preLoot;
					}
				}
			}

			return base.Construct(type, from);
		}

		public override bool Give(Mobile m, Item item, bool placeAtFeet)
		{
			if (item is TreasureMap || item is MessageInABottle || item is FishingNet || item is SpecialFishingNet || item is FabledFishingNet || item is NeptunesFishingNet)
			{
				BaseCreature serp = new DeepSeaSerpent();

				switch (Utility.Random(5))
				{
					case 0: serp = new GiantEel(); break;
					case 1: serp = new GiantSquid(); break;
					case 2: serp = new SeaSerpent(); break;
					case 3: serp = new DeepSeaSerpent(); break;
					case 4: serp = new RottingSquid(); break;
				}

				int x = m.X, y = m.Y;

				Map map = m.Map;

				for (int i = 0; map != null && i < 20; ++i)
				{
					int tx = m.X - 10 + Utility.Random(21);
					int ty = m.Y - 10 + Utility.Random(21);

					LandTile t = map.Tiles.GetLandTile(tx, ty);

					if (t.Z == -5 && Server.Misc.Worlds.IsWaterTile(t.ID, 0) && !Spells.SpellHelper.CheckMulti(new Point3D(tx, ty, -5), map))
					{
						x = tx;
						y = ty;
						break;
					}
				}

				serp.MoveToWorld(new Point3D(x, y, -5), map);

				serp.Home = serp.Location;
				serp.RangeHome = 10;
				serp.WhisperHue = 999;

				serp.PackItem(item);

                PlayerMobile pm = (PlayerMobile)m;

				if (item is TreasureMap || item is MessageInABottle)
				{

                    m.SendMessage(55, "Um monstro marinho surge e engole o tesouro que você pescou!");
                }
				else 
				{
                    m.SendMessage(55, "Um monstro marinho foi atraido pela sua rede de pesca!");
                }
                
                pm.PlaySound(pm.Female ? 0x32E : 0x440);
                pm.Say("*MINHA NOSSA!*");
                //m.SendLocalizedMessage(503170); // Uh oh! That doesn't look like a fish!

                return true; // we don't want to give the item to the player, it's on the serpent
			}

			if (item is WoodenChest || item is MetalGoldenChest || item is SunkenChest)
				placeAtFeet = true;

			if (item is NewFish)
			{
				int nFishingSkill = (int)(m.Skills[SkillName.Fishing].Value / 6);
				NewFish fishy = (NewFish)item;
				fishy.FishGoldValue = fishy.FishGoldValue * nFishingSkill;
			}

			return base.Give(m, item, placeAtFeet);
		}

		public override object GetLock(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
		{
			return this;
		}

		#region Tile lists
		public static int[] m_WaterTiles = new int[]
			{
				0xA8, 0xA9, 0xAA, 0xAB, 0x136, 0x137, 0x40A8, 0x40A9, 0x40AA, 0x40AB, 0x4136, 0x4137, 
				0x5559, 0x5796, 0x5797, 0x5798, 0x5799, 0x579A, 0x579B, 0x579C, 0x579D, 0x579E, 0x579F, 0x57A0, 0x57A1, 0x57A2, 0x57A3, 0x57A4, 0x57A5, 0x57A6, 
				0x57A7, 0x57A8, 0x57A9, 0x57AA, 0x57AB, 0x57AC, 0x57AD, 0x57AE, 0x57AF, 0x57B0, 0x57B1, 0x57B2, 0x57BB, 0x57BC, 0x746E, 0x746F, 0x7470, 0x7471, 0x7472, 0x7473, 
				0x7474, 0x7475, 0x7476, 0x7477, 0x7478, 0x7479, 0x747A, 0x747B, 0x747C, 0x747D, 0x747E, 0x747F, 0x7480, 0x7481, 0x7482, 0x7483, 0x7484, 0x7485, 0x7494, 0x7495, 
				0x7496, 0x7497, 0x7498, 0x749A, 0x749B, 0x749C, 0x749D, 0x749E, 0x74A0, 0x74A1, 0x74A2, 0x74A3, 0x74A4, 0x74A6, 0x74A7, 0x74A8, 0x74A9, 0x74AA, 0x74AB, 0x74B8, 
				0x74B9, 0x74BA, 0x74BB, 0x74BD, 0x74BE, 0x74BF, 0x74C0, 0x74C2, 0x74C3, 0x74C4, 0x74C5, 0x74C7, 0x74C8, 0x74C9, 0x74CA, 0x74D2, 0x7529, 0x752A, 0x752B, 0x752C, 
				0x7531, 0x7532, 0x7533, 0x7534, 0x7535, 0x7536, 0x7537, 0x7538, 0x753D, 0x753E, 0x753F, 0x7540, 0x7541, 0x95F0, 0x95F1, 0x95F2, 0x95F3, 0x95F4, 0x95F5, 0x95F6, 
				0x95F7, 0x95F8, 0x95F9, 0x95FA, 0x95FB, 0x95FC, 0x95FD, 0x95FE, 0x95FF, 0x9600, 0x9601, 0x9602, 0x9603, 0x9604, 0x9605, 0x9606, 0x9607, 0x9608, 0x9609, 0x960A, 
				0x960B, 0x960C, 0x960D, 0x960E, 0x960F, 0x9610, 0x9611, 0x9612, 0x9613, 0x9614, 0x9615, 0x9616, 0x9617, 0x9618, 0x9619, 0x961A, 0x961B, 0x961C, 0x961D, 0x961E, 
				0x961F, 0x9620, 0x9621, 0x9622, 0x9623, 0x9624, 0x9633, 0x9634, 0x9635, 0x9636, 0x9637, 0x9638, 0x9639, 0x963A, 0x963B, 0x963C, 0x963D, 0x963F, 0x9640, 0x9641, 
				0x9642, 0x9643, 0x9644, 0x9645, 0x9646, 0x9647, 0x9648, 0x9649, 0x964A, 0x9657, 0x9658, 0x9659, 0x965A, 0x965B, 0x965C, 0x965D, 0x965E, 0x965F, 0x9660, 0x9661, 
				0x9662, 0x9663, 0x9664, 0x9665, 0x9666, 0x9667, 0x9668, 0x9669, 0x966A, 0x966B, 0x966C, 0x966D, 0x966E, 0x966F
			};
        #endregion
    }
}
