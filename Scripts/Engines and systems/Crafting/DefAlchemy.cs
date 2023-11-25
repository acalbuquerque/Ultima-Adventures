using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefAlchemy : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Alchemy;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044001; } // <CENTER>ALCHEMY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefAlchemy();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefAlchemy() : base( 1, 1, 1.25 )// base( 1, 1, 3.1 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x242 );
		}

		private static Type typeofPotion = typeof( BasePotion );

		public static bool IsPotion( Type type )
		{
			return typeofPotion.IsAssignableFrom( type );
		}

		private static Type typeofLiquid = typeof( BaseLiquid );

		public static bool IsLiquid( Type type )
		{
			return typeofLiquid.IsAssignableFrom( type );
		}

		private static Type typeofMixture = typeof( BaseMixture );

		public static bool IsMixture( Type type )
		{
			return typeofMixture.IsAssignableFrom( type );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			Server.Gumps.MReagentGump.XReagentGump( from );

			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( IsLiquid( item.ItemType ) || IsMixture( item.ItemType ) )
				{
					from.AddToBackpack( new Bottle() );
					from.AddToBackpack( new Jar() );
					return 500287; // You fail to create a useful potion.
				}
				else if ( IsPotion( item.ItemType ) )
				{
					from.AddToBackpack( new Bottle() );
					return 500287; // You fail to create a useful potion.
				}
				else
				{
					return 1044043; // You failed to create the item, and some of your materials are lost.
				}
			}
			else
			{
				if ( IsLiquid( item.ItemType ) || IsMixture( item.ItemType ) )
				{
					from.AddToBackpack( new Bottle() );
				}

				from.PlaySound( 0x240 ); // Sound of a filling bottle

				if (item is LesserPoisonPotion && from is PlayerMobile)
				{
					if (Utility.RandomBool())
					from.CheckSkill( SkillName.Poisoning, 0, 40 );
				}
				else if (item is PoisonPotion && from is PlayerMobile)
				{
					if (Utility.RandomBool())
					from.CheckSkill( SkillName.Poisoning, 0, 75 );
				}
				else if (item is GreaterPoisonPotion && from is PlayerMobile)
				{
					if (Utility.RandomBool())
					from.CheckSkill( SkillName.Poisoning, 0, 95 );
				}
				else if (item is DeadlyPoisonPotion && from is PlayerMobile)
				{
					if (Utility.RandomBool())
					from.CheckSkill( SkillName.Poisoning, 0, 115 );
				}
				else if (item is LethalPoisonPotion && from is PlayerMobile)
				{
					if (Utility.RandomBool())
					from.CheckSkill( SkillName.Poisoning, 0, 125 );
				}

				if ( IsPotion( item.ItemType ) )
				{
					if ( quality == -1 )
						return 1048136; // You create the potion and pour it into a keg.
					else
						return 500279; // You pour the potion into a bottle...
				}
				else
				{
					return 1044154; // You create the item.
				}
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

			// Agility Potion
			index = AddCraft( typeof( AgilityPotion ), "Poções", "agilidade", 35.0, 65.0, typeof( Bloodmoss ), 1044354, 1, 1044362 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
			index = AddCraft( typeof( GreaterAgilityPotion ), "Poções", "agilidade maior", 55.0, 85.0, typeof( Bloodmoss ), 1044354, 3, 1044362 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

            // Cure Potion
            index = AddCraft(typeof(LesserCurePotion), "Poções", "cura menor", 10.0, 45.0, typeof(Garlic), 1044355, 2, 1044363);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(CurePotion), "Poções", "cura", 30.0, 75.0, typeof(Garlic), 1044355, 3, 1044363);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterCurePotion), "Poções", "cura maior", 60.0, 90.0, typeof(Garlic), 1044355, 6, 1044363);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Explosion Potion
            index = AddCraft(typeof(LesserExplosionPotion), "Poções", "explosão menor", 15.0, 55.0, typeof(SulfurousAsh), 1044359, 3, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(ExplosionPotion), "Poções", "explosão", 40.0, 85.0, typeof(SulfurousAsh), 1044359, 5, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterExplosionPotion), "Poções", "explosão maior", 65.0, 95.0, typeof(SulfurousAsh), 1044359, 10, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Heal Potion
            index = AddCraft(typeof(LesserHealPotion), "Poções", "vida menor", 15.0, 45.0, typeof(Ginseng), 1044356, 2, 1044364);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(HealPotion), "Poções", "vida", 35.0, 70.0, typeof(Ginseng), 1044356, 4, 1044364);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterHealPotion), "Poções", "vida maior", 55.0, 95.0, typeof(Ginseng), 1044356, 7, 1044364);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Nightsight Potion
            index = AddCraft(typeof(NightSightPotion), "Poções", "visão noturna", 5.0, 30.0, typeof(SpidersSilk), 1044360, 1, 1044368);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Refresh Potion
            index = AddCraft(typeof(RefreshPotion), "Poções", "refresh", 10, 45.0, typeof(BlackPearl), 1044353, 2, 1044361);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(TotalRefreshPotion), "Poções", "refresh, total", 45.0, 80.0, typeof(BlackPearl), 1044353, 5, 1044361);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Strength Potion
            index = AddCraft(typeof(StrengthPotion), "Poções", "força", 25.0, 65.0, typeof(MandrakeRoot), 1044357, 2, 1044365);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterStrengthPotion), "Poções", "força maior", 45.0, 90.0, typeof(MandrakeRoot), 1044357, 5, 1044365);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Poison Potion
            index = AddCraft(typeof(LesserPoisonPotion), "Poções", "veneno menor", 15.0, 50.0, typeof(Nightshade), 1044358, 1, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(PoisonPotion), "Poções", "veneno", 25.0, 70.0, typeof(Nightshade), 1044358, 2, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterPoisonPotion), "Poções", "veneno maior", 40.0, 80.0, typeof(Nightshade), 1044358, 4, 1044366);
            AddRes(index, typeof(NoxCrystal), 1023982, 1, 1017346);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(DeadlyPoisonPotion), "Poções", "veneno mortal", 65.0, 95.0, typeof(Nightshade), 1044358, 8, 1044366);
            AddRes(index, typeof(NoxCrystal), 1023982, 2, 1017346);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(LethalPoisonPotion), "Poções", "veneno letal", 80.0, 110.0, typeof(Nightshade), 1044358, 12, 1044366);
            AddRes(index, typeof(NoxCrystal), 1023982, 3, 1017346);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            /*            // Conflagration Potions
                        index = AddCraft( typeof(ConflagrationPotion), "Potions", "conflagration", 55.0, 105.0, typeof(GraveDust), 1023983, 5, 1044253 );
                        AddRes( index, typeof(Bottle), 1044529, 1, 500315 );
                        index = AddCraft( typeof(GreaterConflagrationPotion), "Potions", "conflagration, greater", 65.0, 115.0, typeof(GraveDust), 1023983, 10, 1044253 );
                        AddRes( index, typeof(Bottle), 1044529, 1, 500315 );

                        // Confusion Blast Potions
                        index = AddCraft( typeof(ConfusionBlastPotion), "Potions", "confusion blast", 55.0, 105.0, typeof(PigIron), 1023978, 5, 1044253 );
                        AddRes( index, typeof(Bottle), 1044529, 1, 500315 );
                        index = AddCraft( typeof(GreaterConfusionBlastPotion), "Potions", "confusion blast, greater", 65.0, 115.0, typeof(PigIron), 1023978, 10, 1044253 );
                        AddRes( index, typeof(Bottle), 1044529, 1, 500315 );

                        // Frostbite Potions
                        index = AddCraft( typeof(FrostbitePotion), "Potions", "frostbite", 55.0, 105.0, typeof(MoonCrystal), "moon crystal", 5, 1042081 );
                        AddRes( index, typeof(Bottle), 1044529, 1, 500315 );
                        index = AddCraft( typeof(GreaterFrostbitePotion), "Potions", "frostbite, greater", 65.0, 115.0, typeof(MoonCrystal), "moon crystal", 10, 1042081 );
                        AddRes( index, typeof(Bottle), 1044529, 1, 500315 );

                        // SMOKE BOMB
                        index = AddCraft( typeof( SmokeBomb ), "Potions", "smoke bomb", 90.0, 120.0, typeof( Eggs ), 1044477, 1, 1044253 );
                        AddRes( index, typeof ( Ginseng ), 1044356, 3, 1044364 );*/

            // -------------------------------------------------------------------------------------------------------------------------------------------
            // ELIXIR

            /*index = AddCraft( typeof( ElixirAlchemy ), "Elixirs", "alchemy", 60.0, 120.0, typeof( MoonCrystal ), "moon crystal", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirAnatomy ), "Elixirs", "anatomy", 60.0, 120.0, typeof( SwampBerries ), "swamp berries", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BatWing ), "bat wing", 1, 1042081 );
				AddRes( index, typeof ( Ruby ), "ruby", 1, 1042081 );
			index = AddCraft( typeof( ElixirAnimalLore ), "Elixirs", "animal lore", 60.0, 120.0, typeof( RedLotus ), "red lotus", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Ginseng ), "ginseng", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirAnimalTaming ), "Elixirs", "animal taming", 60.0, 120.0, typeof( BeetleShell ), "beetle shell", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( PigIron ), "pig iron", 1, 1042081 );
				AddRes( index, typeof ( Tourmaline ), "tourmaline", 1, 1042081 );
			index = AddCraft( typeof( ElixirArchery ), "Elixirs", "archery", 60.0, 120.0, typeof( FairyEgg ), "fairy egg", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SpidersSilk ), "spider silk", 1, 1042081 );
				AddRes( index, typeof ( Amethyst ), "amethyst", 1, 1042081 );
			index = AddCraft( typeof( ElixirArmsLore ), "Elixirs", "arms lore", 60.0, 120.0, typeof( ButterflyWings ), "butterfly wings", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Emerald ), "emerald", 1, 1042081 );
			index = AddCraft( typeof( ElixirBegging ), "Elixirs", "begging", 60.0, 120.0, typeof( FairyEgg ), "fairy egg", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( PigIron ), "pig iron", 1, 1042081 );
				AddRes( index, typeof ( Tourmaline ), "tourmaline", 1, 1042081 );
			index = AddCraft( typeof( ElixirBlacksmith ), "Elixirs", "blacksmithing", 60.0, 120.0, typeof( GargoyleEar ), "gargoyle ear", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( GraveDust ), "grave dust", 1, 1042081 );
				AddRes( index, typeof ( Diamond ), "diamond", 1, 1042081 );
			index = AddCraft( typeof( ElixirCamping ), "Elixirs", "camping", 60.0, 120.0, typeof( RedLotus ), "red lotus", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( DaemonBlood ), "daemon blood", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirCarpentry ), "Elixirs", "carpentry", 60.0, 120.0, typeof( RedLotus ), "red lotus", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BlackPearl ), "black pearl", 1, 1042081 );
				AddRes( index, typeof ( Amethyst ), "amethyst", 1, 1042081 );
			index = AddCraft( typeof( ElixirCartography ), "Elixirs", "cartography", 60.0, 120.0, typeof( SeaSalt ), "sea salt", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( DaemonBlood ), "daemon blood", 1, 1042081 );
				AddRes( index, typeof ( Ruby ), "ruby", 1, 1042081 );
			index = AddCraft( typeof( ElixirCooking ), "Elixirs", "cooking", 60.0, 120.0, typeof( SeaSalt ), "sea salt", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BatWing ), "bat wing", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirDetectHidden ), "Elixirs", "detection", 60.0, 120.0, typeof( PixieSkull ), "pixie skull", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( NoxCrystal ), "nox crystal", 1, 1042081 );
				AddRes( index, typeof ( Emerald ), "emerald", 1, 1042081 );
			index = AddCraft( typeof( ElixirDiscordance ), "Elixirs", "discordance", 60.0, 120.0, typeof( GargoyleEar ), "gargoyle ear", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BlackPearl ), "black pearl", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirEvalInt ), "Elixirs", "intelligence evaluation", 60.0, 120.0, typeof( ButterflyWings ), "butterfly wings", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Bloodmoss ), "bloodmoss", 1, 1042081 );
				AddRes( index, typeof ( StarSapphire ), "star sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirFencing ), "Elixirs", "fencing", 60.0, 120.0, typeof( SwampBerries ), "swamp berries", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Bloodmoss ), "bloodmoss", 1, 1042081 );
				AddRes( index, typeof ( Citrine ), "citrine", 1, 1042081 );
			index = AddCraft( typeof( ElixirFishing ), "Elixirs", "fishing", 60.0, 120.0, typeof( SeaSalt ), "sea salt", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( GraveDust ), "grave dust", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirFletching ), "Elixirs", "fletching", 60.0, 120.0, typeof( EyeOfToad ), "eye of toad", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Ginseng ), "ginseng", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirFocus ), "Elixirs", "focus", 60.0, 120.0, typeof( SwampBerries ), "swamp berries", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Tourmaline ), "tourmaline", 1, 1042081 );
			index = AddCraft( typeof( ElixirForensics ), "Elixirs", "forensics", 60.0, 120.0, typeof( FairyEgg ), "fairy egg", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BlackPearl ), "black pearl", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirHealing ), "Elixirs", "of the healer", 60.0, 120.0, typeof( SilverWidow ), "silver widow", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirHerding ), "Elixirs", "herding", 60.0, 120.0, typeof( PixieSkull ), "pixie skull", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirHiding ), "Elixirs", "hiding", 60.0, 120.0, typeof( MoonCrystal ), "moon crystal", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BatWing ), "bat wing", 1, 1042081 );
				AddRes( index, typeof ( Amethyst ), "amethyst", 1, 1042081 );
			index = AddCraft( typeof( ElixirInscribe ), "Elixirs", "inscription", 60.0, 120.0, typeof( EyeOfToad ), "eye of toad", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( PigIron ), "pig iron", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirItemID ), "Elixirs", "item identifying", 60.0, 120.0, typeof( EyeOfToad ), "eye of toad", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirLockpicking ), "Elixirs", "lockpicking", 60.0, 120.0, typeof( ButterflyWings ), "butterfly wings", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( MandrakeRoot ), "mandrake root", 1, 1042081 );
				AddRes( index, typeof ( Tourmaline ), "tourmaline", 1, 1042081 );
			index = AddCraft( typeof( ElixirLumberjacking ), "Elixirs", "lumberjacking", 60.0, 120.0, typeof( GargoyleEar ), "gargoyle ear", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Diamond ), "diamond", 1, 1042081 );
			index = AddCraft( typeof( ElixirMacing ), "Elixirs", "mace fighting", 60.0, 120.0, typeof( SilverWidow ), "silver widow", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( PigIron ), "pig iron", 1, 1042081 );
				AddRes( index, typeof ( Diamond ), "diamond", 1, 1042081 );
			index = AddCraft( typeof( ElixirMagicResist ), "Elixirs", "magic resistance", 60.0, 120.0, typeof( BeetleShell ), "beetle shell", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirMeditation ), "Elixirs", "meditating", 60.0, 120.0, typeof( BeetleShell ), "beetle shell", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Ginseng ), "ginseng", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirMining ), "Elixirs", "mining", 60.0, 120.0, typeof( Brimstone ), "brimstone", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BatWing ), "bat wing", 1, 1042081 );
				AddRes( index, typeof ( Diamond ), "diamond", 1, 1042081 );
			index = AddCraft( typeof( ElixirMusicianship ), "Elixirs", "musicianship", 60.0, 120.0, typeof( ButterflyWings ), "butterfly wings", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Nightshade ), "nightshade", 1, 1042081 );
				AddRes( index, typeof ( StarSapphire ), "star sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirParry ), "Elixirs", "parrying", 60.0, 120.0, typeof( Brimstone ), "brimstone", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( NoxCrystal ), "nox crystal", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirPeacemaking ), "Elixirs", "peacemaking", 60.0, 120.0, typeof( PixieSkull ), "pixie skull", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirPoisoning ), "Elixirs", "poisoning", 60.0, 120.0, typeof( SeaSalt ), "sea salt", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BlackPearl ), "black pearl", 1, 1042081 );
				AddRes( index, typeof ( StarSapphire ), "star sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirProvocation ), "Elixirs", "provocation", 60.0, 120.0, typeof( EyeOfToad ), "eye of toad", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Ginseng ), "ginseng", 1, 1042081 );
				AddRes( index, typeof ( Emerald ), "emerald", 1, 1042081 );
			index = AddCraft( typeof( ElixirRemoveTrap ), "Elixirs", "removing trap", 60.0, 120.0, typeof( SilverWidow ), "silver widow", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( BatWing ), "bat wing", 1, 1042081 );
				AddRes( index, typeof ( Emerald ), "emerald", 1, 1042081 );
			index = AddCraft( typeof( ElixirSnooping ), "Elixirs", "snooping", 60.0, 120.0, typeof( ButterflyWings ), "butterfly wings", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Nightshade ), "nightshade", 1, 1042081 );
				AddRes( index, typeof ( Amber ), "amber", 1, 1042081 );
			index = AddCraft( typeof( ElixirSpiritSpeak ), "Elixirs", "spirit speaking", 60.0, 120.0, typeof( SwampBerries ), "swamp berries", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( GraveDust ), "grave dust", 1, 1042081 );
				AddRes( index, typeof ( Emerald ), "emerald", 1, 1042081 );
			index = AddCraft( typeof( ElixirStealing ), "Elixirs", "stealing", 60.0, 120.0, typeof( MoonCrystal ), "moon crystal", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Sapphire ), "sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirStealth ), "Elixirs", "stealth", 60.0, 120.0, typeof( PixieSkull ), "pixie skull", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
				AddRes( index, typeof ( Citrine ), "citrine", 1, 1042081 );
			index = AddCraft( typeof( ElixirSwords ), "Elixirs", "sword fighting", 60.0, 120.0, typeof( FairyEgg ), "fairy egg", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( NoxCrystal ), "nox crystal", 1, 1042081 );
				AddRes( index, typeof ( Citrine ), "citrine", 1, 1042081 );
			index = AddCraft( typeof( ElixirTactics ), "Elixirs", "tactics", 60.0, 120.0, typeof( GargoyleEar ), "gargoyle ear", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SpidersSilk ), "spider silk", 1, 1042081 );
				AddRes( index, typeof ( StarSapphire ), "star sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirTailoring ), "Elixirs", "tailoring", 60.0, 120.0, typeof( Brimstone ), "brimstone", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Emerald ), "emerald", 1, 1042081 );
			index = AddCraft( typeof( ElixirTasteID ), "Elixirs", "taste identification", 60.0, 120.0, typeof( BeetleShell ), "beetle shell", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Diamond ), "diamond", 1, 1042081 );
			index = AddCraft( typeof( ElixirTinkering ), "Elixirs", "tinkering", 60.0, 120.0, typeof( ButterflyWings ), "butterfly wings", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
				AddRes( index, typeof ( Tourmaline ), "tourmaline", 1, 1042081 );
			index = AddCraft( typeof( ElixirTracking ), "Elixirs", "tracking", 60.0, 120.0, typeof( SilverWidow ), "silver widow", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( MandrakeRoot ), "mandrake root", 1, 1042081 );
				AddRes( index, typeof ( Tourmaline ), "tourmaline", 1, 1042081 );
			index = AddCraft( typeof( ElixirVeterinary ), "Elixirs", "veterinary", 60.0, 120.0, typeof( RedLotus ), "red lotus", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( DaemonBlood ), "daemon blood", 1, 1042081 );
				AddRes( index, typeof ( StarSapphire ), "star sapphire", 1, 1042081 );
			index = AddCraft( typeof( ElixirWrestling ), "Elixirs", "wrestling", 60.0, 120.0, typeof( MoonCrystal ), "moon crystal", 3, 1042081 );
				AddRes( index, typeof ( Bottle ), "empty bottle", 1, 1042081 );
				AddRes( index, typeof ( SpidersSilk ), "spider silk", 1, 1042081 );
				AddRes( index, typeof ( StarSapphire ), "star sapphire", 1, 1042081 );*/

            // -------------------------------------------------------------------------------------------------------------------------------------------
            // MIXTURES

            /*index = AddCraft(typeof(OilTransmutation), "Óleos", "Óleo de Transmutação", 70.0, 120.0, typeof(RedLotus), "lotus vermelha", 2, 1042081);
            AddRes(index, typeof(Bottle), "Garrafa Vazia", 1, 1042081);
            AddRes(index, typeof(DaemonBlood), "daemon blood", 1, 1042081);
            AddRes(index, typeof(BlackPearl), "black pearl", 3, 1042081);*/

            /*index = AddCraft( typeof( LiquidFire ), "Mixtures", "liquid, fire", 50.0, 120.0, typeof( Brimstone ), "brimstone", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( ExplosionPotion ), "regular explosion potion", 1, 1042081 );
				AddRes( index, typeof ( BlackPearl ), "black pearl", 1, 1042081 );
			index = AddCraft( typeof( LiquidGoo ), "Mixtures", "liquid, goo", 50.0, 120.0, typeof( SwampBerries ), "swamp berries", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( HealPotion ), "regular heal potion", 1, 1042081 );
				AddRes( index, typeof ( DaemonBlood ), "daemon blood", 1, 1042081 );
			index = AddCraft( typeof( LiquidIce ), "Mixtures", "liquid, ice", 50.0, 120.0, typeof( MoonCrystal ), "moon crystal", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( CurePotion ), "regular cure potion", 1, 1042081 );
				AddRes( index, typeof ( GraveDust ), "grave dust", 1, 1042081 );
			index = AddCraft( typeof( LiquidPain ), "Mixtures", "liquid, pain", 50.0, 120.0, typeof( GargoyleEar ), "gargoyle ear", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( NightSightPotion ), "night sight potion", 1, 1042081 );
				AddRes( index, typeof ( Garlic ), "garlic", 1, 1042081 );
			index = AddCraft( typeof( LiquidRot ), "Mixtures", "liquid, rot", 50.0, 120.0, typeof( PixieSkull ), "pixie skull", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( PoisonPotion ), "regular poison potion", 1, 1042081 );
				AddRes( index, typeof ( NoxCrystal ), "nox crystal", 1, 1042081 );

			index = AddCraft( typeof( MixtureSlime ), "Mixtures", "slime", 50.0, 120.0, typeof( BeetleShell ), "beetle shell", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( StrengthPotion ), "regular strength potion", 1, 1042081 );
				AddRes( index, typeof ( Ginseng ), "ginseng", 1, 1042081 );
			index = AddCraft( typeof( MixtureDiseasedSlime ), "Mixtures", "slime, diseased", 50.0, 120.0, typeof( EyeOfToad ), "eye of toad", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( ConfusionBlastPotion ), "regular confusion blast potion", 1, 1042081 );
				AddRes( index, typeof ( Nightshade ), "nightshade", 1, 1042081 );
			index = AddCraft( typeof( MixtureFireSlime ), "Mixtures", "slime, fire", 50.0, 120.0, typeof( RedLotus ), "red lotus", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( ConflagrationPotion ), "regular conflagration potion", 1, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulfurous ash", 1, 1042081 );
			index = AddCraft( typeof( MixtureIceSlime ), "Mixtures", "slime, ice", 50.0, 120.0, typeof( SeaSalt ), "sea salt", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( AgilityPotion ), "regular agility potion", 1, 1042081 );
				AddRes( index, typeof ( Bloodmoss ), "bloodmoss", 1, 1042081 );
			index = AddCraft( typeof( MixtureRadiatedSlime ), "Mixtures", "slime, irradiated", 50.0, 120.0, typeof( FairyEgg ), "fairy egg", 2, 1042081 );
				AddRes( index, typeof ( Jar ), "empty jar", 1, 1042081 );
				AddRes( index, typeof ( RefreshPotion ), "regular refresh potion", 1, 1042081 );
				AddRes( index, typeof ( PigIron ), "pig iron", 1, 1042081 );

			index = AddCraft( typeof( BottleOfAcid ), "Mixtures", "bottle of acid", 95, 120.0, typeof( Brimstone ), "brimstone", 2, 1042081 );
				AddRes( index, typeof ( SulfurousAsh ), "sulferous ash", 2, 1042081 );
				AddRes( index, typeof ( SeaSalt ), "sea salt", 2, 1042081 );

			index = AddCraft( typeof( BottleOil ), "Mixtures", "technomancer oil", 95, 120.0, typeof( PigIron ), "pig iron", 20, 1042081 );
				AddRes( index, typeof ( LiquidGoo ), "liquid goo", 10, 1042081 );
				AddRes( index, typeof ( MixtureSlime ), "slime", 2, 1042081 );*/

            // -------------------------------------------------------------------------------------------------------------------------------------------
            //Transmutations - Created by Krystofer Robin (Kelton Grespair)
            /*index = AddCraft( typeof(DullCopperIngot), "Transmutações", "Cobre Rústico", 50, 60, typeof( IronIngot), "lingote de ferro", 10, 1042081);
			AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(CopperIngot), "Transmutações", "Cobre", 60, 70, typeof(DullCopperIngot), "lingote de cobre rústico", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft(typeof(ShadowIronIngot), "Transmutações", "Ferro Negro", 70, 80, typeof(CopperIngot), "lingote de cobre", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(BronzeIngot), "Transmutações", "Bronze", 80, 90, typeof(ShadowIronIngot), "lingote de ferro negro", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(GoldIngot), "Transmutações", "Dourado", 90, 100, typeof( BronzeIngot), "lingote de bronze", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(AgapiteIngot), "Transmutações", "Agapite", 100, 120, typeof( GoldIngot), "lingote de dourado", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(VeriteIngot), "Transmutações", "Verite", 110, 120, typeof( AgapiteIngot), "lingote de agapite", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);

            index = AddCraft( typeof(SpinedLeather), "Transmutações", "Couro Spined", 60, 80, typeof( Leather), "couro normal", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(HornedLeather), "Transmutações", "Couro Horned", 70, 100, typeof( SpinedLeather), "couro spined", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(BarbedLeather), "Transmutações", "Couro Barbed", 80, 120, typeof( HornedLeather), "couro horned", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);

            index = AddCraft( typeof(AshBoard), "Transmutações", "Carvalho Cinza", 30, 50, typeof( BaseWoodBoard), "tábua de Madeira", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(EbonyBoard), "Transmutações", "Ébano", 40, 60, typeof(AshBoard), "tábua de Carvalho Cinza", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(GoldenOakBoard), "Transmutações", "Ipê-Amarelo", 50, 70, typeof( EbonyBoard), "tábua de Ébano", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(CherryBoard), "Transmutações", "Cerejeira", 60, 80, typeof(GoldenOakBoard), "tábua de Ipê-Amarelo", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);
            index = AddCraft( typeof(RosewoodBoard), "Transmutações", "Pau-Brasil", 70, 90, typeof( CherryBoard), "tábua de Cerejeira", 10, 1042081);
            AddRes(index, typeof(OilTransmutation), "Óleo de Transmutação", 1, 1148260);*/
            /*index = AddCraft( typeof(MahoganyBoard), "Transmutations", "Mahogany", 80, 100, typeof( HickoryBoard), "hickory board", 10, 1042081);
			index = AddCraft( typeof(DriftwoodBoard), "Transmutations", "Driftwood", 90, 110, typeof( MahoganyBoard), "mahogany board", 10, 1042081);
			index = AddCraft( typeof(OakBoard), "Transmutations", "Oak", 100, 120, typeof( DriftwoodBoard), "driftwood board", 10, 1042081);*/

            /*index = AddCraft( typeof(PowerCrystal), "Transmutações", "Power Crystal", 95, 120, typeof( ArcaneGem), "arcane gem", 10, 1042081);
				AddRes( index, typeof (BottleOil), "technomancer oil", 10, 1042081);
				AddRes( index, typeof ( NoxCrystal), "nox crystal", 50, 1042081);*/


            // Hair Potion
            index = AddCraft( typeof( HairOilPotion ), "Cosmético", "poção de corte de cabelo", 80, 110.0, typeof( PixieSkull ), "pixie skull", 2, 1042081 );
			AddRes( index, typeof ( Bottle ), "empty bottle", 1, 500315 );

			index = AddCraft( typeof( HairDyePotion ), "Cosmético", "tinta de cabelo", 80, 110.0, typeof( FairyEgg ), "fairy egg", 3, 1042081 );
			AddRes( index, typeof ( Bottle ), "empty bottle", 1, 500315 );
		}
	}
}