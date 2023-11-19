using System;
using Server.Items;

namespace Server.Engines.Craft
{
	public class DefTailoring : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tailoring; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044005; } // <CENTER>TAILORING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefTailoring();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefTailoring() : base( 1, 1, 1.25 )// base( 1, 1, 4.5 )
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

		public static bool IsNonColorable(Type type)
		{
			for (int i = 0; i < m_TailorNonColorables.Length; ++i)
			{
				if (m_TailorNonColorables[i] == type)
				{
					return true;
				}
			}

			return false;
		}

		private static Type[] m_TailorNonColorables = new Type[]
			{
				//typeof( OrcHelm )
			};

		private static Type[] m_TailorColorables = new Type[]
			{
				typeof( GozaMatEastDeed ), typeof( GozaMatSouthDeed ),
				typeof( SquareGozaMatEastDeed ), typeof( SquareGozaMatSouthDeed ),
				typeof( BrocadeGozaMatEastDeed ), typeof( BrocadeGozaMatSouthDeed ),
				typeof( BrocadeSquareGozaMatEastDeed ), typeof( BrocadeSquareGozaMatSouthDeed )
			};

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			if ( type != typeof( CottonCloth ) && type != typeof( PoliesterCloth ) )
				return false;

			type = item.ItemType;

			bool contains = false;

			for ( int i = 0; !contains && i < m_TailorColorables.Length; ++i )
				contains = ( m_TailorColorables[i] == type );

			return contains;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x248 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				if ( quality == 0 )
					return 502785; // You were barely able to make this item.  It's quality is below average.
				else if ( makersMark && quality == 2 )
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( quality == 2 )
					return 1044155; // You create an exceptional quality item.
				else				
					return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

			#region Hats
			AddCraft( typeof( SkullCap ), "Máscara/Chapéu/Boné", 1025444, 0.0, 25.0, typeof( CottonCloth ), 1044286, 2, 1044287 );
			AddCraft( typeof( Bandana ), "Máscara/Chapéu/Boné", 1025440, 0.0, 25.0, typeof( CottonCloth ), 1044286, 2, 1044287 );
			AddCraft( typeof( FloppyHat ), "Máscara/Chapéu/Boné", 1025907, 6.2, 31.2, typeof( CottonCloth ), 1044286, 11, 1044287 );
			AddCraft( typeof( Cap ), "Máscara/Chapéu/Boné", 1025909, 6.2, 31.2, typeof( CottonCloth ), 1044286, 11, 1044287 );
			AddCraft( typeof( WideBrimHat ), "Máscara/Chapéu/Boné", 1025908, 6.2, 31.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( StrawHat ), "Máscara/Chapéu/Boné", 1025911, 6.2, 31.2, typeof( CottonCloth ), 1044286, 10, 1044287 );
			AddCraft( typeof( TallStrawHat ), "Máscara/Chapéu/Boné", 1025910, 6.7, 31.7, typeof( CottonCloth ), 1044286, 13, 1044287 );
			AddCraft( typeof( WizardsHat ), "Máscara/Chapéu/Boné", 1025912, 7.2, 32.2, typeof( CottonCloth ), 1044286, 15, 1044287 );
			AddCraft( typeof( WitchHat ), "Máscara/Chapéu/Boné", "witch hat", 7.2, 32.2, typeof( CottonCloth ), 1044286, 15, 1044287 );
			AddCraft( typeof( Bonnet ), "Máscara/Chapéu/Boné", 1025913, 6.2, 31.2, typeof( CottonCloth ), 1044286, 11, 1044287 );
			AddCraft( typeof( FeatheredHat ), "Máscara/Chapéu/Boné", 1025914, 6.2, 31.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( TricorneHat ), "Máscara/Chapéu/Boné", 1025915, 6.2, 31.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( PirateHat ), "Máscara/Chapéu/Boné", "pirate hat", 6.2, 31.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( JesterHat ), "Máscara/Chapéu/Boné", 1025916, 7.2, 32.2, typeof( CottonCloth ), 1044286, 15, 1044287 );
			AddCraft( typeof( ClothHood ), "Máscara/Chapéu/Boné", "cloth hood", 7.2, 32.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( FancyHood ), "Máscara/Chapéu/Boné", "fancy hood", 7.2, 32.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( ClothCowl ), "Máscara/Chapéu/Boné", "cloth cowl", 7.2, 32.2, typeof( CottonCloth ), 1044286, 12, 1044287 );
            AddCraft(typeof(WizardHood), "Máscara/Chapéu/Boné", "wizard hood", 7.2, 32.2, typeof(CottonCloth), 1044286, 12, 1044287);

            AddCraft(typeof(FurCap), "Máscara/Chapéu/Boné", "fur cap", 0.0, 25.0, typeof(Furs), "Fur", 2, 1042081);
            AddCraft(typeof(WhiteFurCap), "Máscara/Chapéu/Boné", "white fur cap", 12.0, 48.0, typeof(FursWhite), "White Fur", 9, 1042081);

            AddCraft( typeof( FlowerGarland ), "Máscara/Chapéu/Boné", 1028965, 10.0, 35.0, typeof( CottonCloth ), 1044286, 5, 1044287 );
			AddCraft( typeof( ClothNinjaHood ), "Máscara/Chapéu/Boné", 1030202, 80.0, 105.0, typeof( CottonCloth ), 1044286, 13, 1044287 );
			AddCraft( typeof( Kasa ), "Máscara/Chapéu/Boné", 1030211, 60.0, 85.0, typeof( CottonCloth ), 1044286, 12, 1044287 );

            
            index = AddCraft(typeof(DeadMask), "Máscara/Chapéu/Boné", "mask of the dead", 7.2, 32.2, typeof(CottonCloth), 1044286, 12, 1044287);
            AddRes(index, typeof(PolishedSkull), "Polished Skull", 1, 1049063);
            #endregion

            #region Shirts
            AddCraft( typeof( Doublet ), "Robes/Camisas/Capas", 1028059, 0, 25.0, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( Shirt ), "Robes/Camisas/Capas", 1025399, 20.7, 45.7, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( BeggarVest ), "Robes/Camisas/Capas", "beggar vest", 20.7, 45.7, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( RoyalVest ), "Robes/Camisas/Capas", "royal vest", 20.7, 45.7, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( RusticVest ), "Robes/Camisas/Capas", "rustic vest", 20.7, 45.7, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( Tunic ), "Robes/Camisas/Capas", 1028097, 00.0, 25.0, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( Surcoat ), "Robes/Camisas/Capas", 1028189, 8.2, 33.2, typeof( CottonCloth ), 1044286, 14, 1044287 );
			AddCraft( typeof( PlainDress ), "Robes/Camisas/Capas", 1027937, 12.4, 37.4, typeof( CottonCloth ), 1044286, 10, 1044287 );
			AddCraft( typeof( FancyDress ), "Robes/Camisas/Capas", 1027935, 33.1, 58.1, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( GildedDress ), "Robes/Camisas/Capas", 1028973, 37.5, 62.5, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( Cloak ), "Robes/Camisas/Capas", 1025397, 41.4, 66.4, typeof( CottonCloth ), 1044286, 14, 1044287 );
			AddCraft( typeof( RoyalCape ), "Robes/Camisas/Capas", "royal cloak", 91.4, 120.4, typeof( CottonCloth ), 1044286, 14, 1044287 );
			AddCraft( typeof( Robe ), "Robes/Camisas/Capas", 1027939, 53.9, 78.9, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( ArchmageRobe ), "Robes/Camisas/Capas", "archmage robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( AssassinRobe ), "Robes/Camisas/Capas", "assassin robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( AssassinShroud ), "Robes/Camisas/Capas", "assassin shroud", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( ChaosRobe ), "Robes/Camisas/Capas", "chaos robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( CultistRobe ), "Robes/Camisas/Capas", "cultist robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( DragonRobe ), "Robes/Camisas/Capas", "dragon robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( ElegantRobe ), "Robes/Camisas/Capas", "elegant robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( ExquisiteRobe ), "Robes/Camisas/Capas", "exquisite robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( FancyRobe ), "Robes/Camisas/Capas", "fancy robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( FoolsCoat ), "Robes/Camisas/Capas", "fool's coat", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( FormalRobe ), "Robes/Camisas/Capas", "formal robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( GildedRobe ), "Robes/Camisas/Capas", "gilded robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( GildedDarkRobe ), "Robes/Camisas/Capas", "gilded dark robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( GildedLightRobe ), "Robes/Camisas/Capas", "gilded light robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( JesterGarb ), "Robes/Camisas/Capas", "jester garb", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( JesterSuit ), "Robes/Camisas/Capas", 1028095, 8.2, 33.2, typeof( CottonCloth ), 1044286, 24, 1044287 );
			AddCraft( typeof( JokerRobe ), "Robes/Camisas/Capas", "jester coat", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( MagistrateRobe ), "Robes/Camisas/Capas", "magistrate robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( NecromancerRobe ), "Robes/Camisas/Capas", "necromancer robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( OrnateRobe ), "Robes/Camisas/Capas", "ornate robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( PirateCoat ), "Robes/Camisas/Capas", "pirate coat", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( PriestRobe ), "Robes/Camisas/Capas", "priest robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( ProphetRobe ), "Robes/Camisas/Capas", "prophet robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( RoyalRobe ), "Robes/Camisas/Capas", "royal robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( SageRobe ), "Robes/Camisas/Capas", "sage robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( SorcererRobe ), "Robes/Camisas/Capas", "sorcerer robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( SpiderRobe ), "Robes/Camisas/Capas", "spider robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( VagabondRobe ), "Robes/Camisas/Capas", "vagabond robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( VampireRobe ), "Robes/Camisas/Capas", "vampire robe", 70.0, 95.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( FancyShirt ), "Robes/Camisas/Capas", 1027933, 24.8, 49.8, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( FormalShirt ), "Robes/Camisas/Capas", 1028975, 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( FormalCoat ), "Robes/Camisas/Capas", "formal coat", 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( RoyalCoat ), "Robes/Camisas/Capas", "royal coat", 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( RoyalShirt ), "Robes/Camisas/Capas", "royal shirt", 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( RusticShirt ), "Robes/Camisas/Capas", "rustic shirt", 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( SquireShirt ), "Robes/Camisas/Capas", "squire shirt", 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( WizardShirt ), "Robes/Camisas/Capas", "wizard shirt", 26.0, 51.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( ClothNinjaJacket ), "Robes/Camisas/Capas", 1030207, 75.0, 100.0, typeof( CottonCloth ), 1044286, 12, 1044287 );
			AddCraft( typeof( Kamishimo ), "Robes/Camisas/Capas", 1030212, 75.0, 100.0, typeof( CottonCloth ), 1044286, 15, 1044287 );
			AddCraft( typeof( HakamaShita ), "Robes/Camisas/Capas", 1030215, 40.0, 65.0, typeof( CottonCloth ), 1044286, 14, 1044287 );
			AddCraft( typeof( MaleKimono ), "Robes/Camisas/Capas", 1030189, 50.0, 75.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( FemaleKimono ), "Robes/Camisas/Capas", 1030190, 50.0, 75.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( JinBaori ), "Robes/Camisas/Capas", 1030220, 30.0, 55.0, typeof( CottonCloth ), 1044286, 12, 1044287 );


            AddCraft(typeof(WhiteFurTunic), "Robes/Camisas/Capas", "white fur tunic", 70.5, 95.5, typeof(FursWhite), "White Fur", 12, 1042081);
            AddCraft(typeof(WhiteFurCape), "Robes/Camisas/Capas", "white fur cape", 35.0, 60.0, typeof(FursWhite), "White Fur", 13, 1042081);
            AddCraft(typeof(WhiteFurRobe), "Robes/Camisas/Capas", "white fur robe", 55.0, 80.0, typeof(FursWhite), "White Fur", 16, 1042081);
            AddCraft(typeof(FurTunic), "Robes/Camisas/Capas", "fur tunic", 70.5, 95.5, typeof(Furs), "Fur", 12, 1042081);
            AddCraft(typeof(FurCape), "Robes/Camisas/Capas", "fur cape", 35.0, 60.0, typeof(Furs), "Fur", 13, 1042081);
            AddCraft(typeof(FurRobe), "Robes/Camisas/Capas", "fur robe", 55.0, 80.0, typeof(Furs), "Fur", 16, 1042081);
            #endregion

            #region Pants
            AddCraft( typeof( ShortPants ), "Calças/Shorts", 1025422, 24.8, 49.8, typeof( CottonCloth ), 1044286, 6, 1044287 );
			AddCraft( typeof( LongPants ), "Calças/Shorts", 1025433, 24.8, 49.8, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( SailorPants ), "Calças/Shorts", "sailor pants", 24.8, 49.8, typeof( CottonCloth ), 1044286, 6, 1044287 );
			AddCraft( typeof( PiratePants ), "Calças/Shorts", "pirate pants", 24.8, 49.8, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( Kilt ), "Calças/Shorts", 1025431, 20.7, 45.7, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( Skirt ), "Calças/Shorts", 1025398, 29.0, 54.0, typeof( CottonCloth ), 1044286, 10, 1044287 );
			AddCraft( typeof( RoyalSkirt ), "Calças/Shorts", "royal skirt", 20.7, 45.7, typeof( CottonCloth ), 1044286, 8, 1044287 );
			AddCraft( typeof( RoyalLongSkirt ), "Calças/Shorts", "royal long skirt", 29.0, 54.0, typeof( CottonCloth ), 1044286, 10, 1044287 );
			
			
			AddCraft( typeof( Hakama ), "Calças/Shorts", 1030213, 50.0, 75.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
			AddCraft( typeof( TattsukeHakama ), "Calças/Shorts", 1030214, 50.0, 75.0, typeof( CottonCloth ), 1044286, 16, 1044287 );
            #endregion

            #region Footwear

            AddCraft(typeof(FurBoots), "Calçados", 1028967, 50.0, 75.0, typeof(Furs), "Fur", 12, 1042081);
            AddCraft(typeof(WhiteFurBoots), "Calçados", "white fur boots", 50.0, 75.0, typeof(FursWhite), "White Fur", 12, 1042081);
            AddCraft(typeof(NinjaTabi), "Calçados", 1030210, 70.0, 95.0, typeof(CottonCloth), 1044286, 10, 1044287);
            AddCraft(typeof(SamuraiTabi), "Calçados", 1030209, 20.0, 45.0, typeof(CottonCloth), 1044286, 6, 1044287);
            AddCraft(typeof(Sandals), "Calçados", 1025901, 12.4, 37.4, typeof(Leather), 1044462, 4, 1044463);
            AddCraft(typeof(Shoes), "Calçados", 1025904, 16.5, 41.5, typeof(Leather), 1044462, 6, 1044463);
            AddCraft(typeof(Boots), "Calçados", 1025899, 33.1, 58.1, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(ThighBoots), "Calçados", 1025906, 41.4, 66.4, typeof(Leather), 1044462, 10, 1044463);
            AddCraft(typeof(BarbarianBoots), "Calçados", "barbarian boots", 50.0, 75.0, typeof(Furs), "Fur", 12, 1042081);
            AddCraft(typeof(LeatherSandals), "Calçados", "leather sandals", 42.4, 67.4, typeof(Leather), 1044462, 4, 1044463);
            AddCraft(typeof(LeatherShoes), "Calçados", "leather shoes", 56.5, 71.5, typeof(Leather), 1044462, 6, 1044463);
            AddCraft(typeof(LeatherBoots), "Calçados", "leather boots", 63.1, 88.1, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(LeatherThighBoots), "Calçados", "leather thigh boots", 71.4, 96.4, typeof(Leather), 1044462, 10, 1044463);
            AddCraft(typeof(LeatherSoftBoots), "Calçados", "soft leather boots", 81.4, 106.4, typeof(Leather), 1044462, 8, 1044463);
            #endregion

            #region Misc
            AddCraft( typeof( BodySash ), "Variados", 1025441, 4.1, 29.1, typeof( CottonCloth ), 1044286, 4, 1044287 );
			AddCraft( typeof( LoinCloth ), "Variados", "loin cloth", 20.7, 45.7, typeof( CottonCloth ), 1044286, 6, 1044287 );
			AddCraft( typeof( HalfApron ), "Variados", 1025435, 20.7, 45.7, typeof( CottonCloth ), 1044286, 6, 1044287 );
			AddCraft( typeof( FullApron ), "Variados", 1025437, 29.0, 54.0, typeof( CottonCloth ), 1044286, 10, 1044287 );
			AddCraft( typeof( Obi ), "Variados", 1030219, 20.0, 45.0, typeof( CottonCloth ), 1044286, 6, 1044287 );
			AddCraft( typeof( HarpoonRope ), "Variados", "harpoon rope", 0.0, 40.0, typeof( CottonCloth ), 1044286, 1, 1044287 );
			AddCraft( typeof( OilCloth ), "Variados", 1041498, 74.6, 99.6, typeof( CottonCloth ), 1044286, 1, 1044287 );
			AddCraft( typeof( GozaMatEastDeed ), "Variados", 1030404, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( GozaMatSouthDeed ), "Variados", 1030405, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( SquareGozaMatEastDeed ), "Variados", 1030407, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( SquareGozaMatSouthDeed ), "Variados", 1030406, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( BrocadeGozaMatEastDeed ), "Variados", 1030408, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( BrocadeGozaMatSouthDeed ), "Variados", 1030409, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( BrocadeSquareGozaMatEastDeed ), "Variados", 1030411, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( BrocadeSquareGozaMatSouthDeed ), "Variados", 1030410, 55.0, 80.0, typeof( CottonCloth ), 1044286, 25, 1044287 );

            AddCraft(typeof(FurArms), "Variados", "fur arms", 53.9, 78.9, typeof(Furs), "Fur", 4, 1042081);
            AddCraft(typeof(FurLegs), "Variados", "fur leggings", 66.3, 91.3, typeof(Furs), "Fur", 10, 1042081);
            AddCraft(typeof(FurSarong), "Variados", "fur sarong", 35.0, 60.0, typeof(Furs), "Fur", 13, 1042081);

            AddCraft(typeof(WhiteFurArms), "Variados", "white fur arms", 53.9, 78.9, typeof(FursWhite), "White Fur", 4, 1042081);
            AddCraft(typeof(WhiteFurLegs), "Variados", "white fur leggings", 66.3, 91.3, typeof(FursWhite), "White Fur", 10, 1042081);
            AddCraft(typeof(WhiteFurSarong), "Variados", "white fur sarong", 35.0, 60.0, typeof(FursWhite), "White Fur", 13, 1042081);
            #endregion

            #region Leather Armor
            AddCraft( typeof( LeatherGorget ), "Armadura de Couro", 1025063, 53.9, 78.9, typeof(Leather), 1044462, 4, 1044463 );
			AddCraft( typeof( LeatherCap ), "Armadura de Couro", 1027609, 6.2, 31.2, typeof( Leather ), 1044462, 2, 1044463 );
			AddCraft( typeof( LeatherGloves ), "Armadura de Couro", 1025062, 51.8, 76.8, typeof( Leather ), 1044462, 3, 1044463 );
			AddCraft( typeof( LeatherArms ), "Armadura de Couro", 1025061, 53.9, 78.9, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( LeatherLegs ), "Armadura de Couro", 1025067, 66.3, 91.3, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( LeatherChest ), "Armadura de Couro", 1025068, 70.5, 95.5, typeof(Leather), 1044462, 12, 1044463 );
			AddCraft( typeof( LeatherCloak ), "Armadura de Couro", "leather cloak", 66.3, 91.3, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( typeof( LeatherRobe ), "Armadura de Couro", "leather robe", 76.3, 101.3, typeof( Leather ), 1044462, 18, 1044463 );
			AddCraft( typeof( LeatherShorts ), "Armadura de Couro", 1027168, 62.2, 87.2, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( LeatherSkirt ), "Armadura de Couro", 1027176, 58.0, 83.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( LeatherBustierArms ), "Armadura de Couro", 1027178, 58.0, 83.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( FemaleLeatherChest ), "Armadura de Couro", 1027174, 62.2, 87.2, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( LeatherJingasa ), "Armadura de Couro", 1030177, 45.0, 70.0, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( LeatherMempo ), "Armadura de Couro", 1030181, 80.0, 105.0, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( typeof( LeatherDo ), "Armadura de Couro", 1030182, 75.0, 100.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( LeatherHiroSode ), "Armadura de Couro", 1030185, 55.0, 80.0, typeof( Leather ), 1044462, 5, 1044463 );
			AddCraft( typeof( LeatherSuneate ), "Armadura de Couro", 1030193, 68.0, 93.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( LeatherHaidate ), "Armadura de Couro", 1030197, 68.0, 93.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( LeatherNinjaPants ), "Armadura de Couro", 1030204, 80.0, 105.0, typeof( Leather ), 1044462, 13, 1044463 );
			AddCraft( typeof( LeatherNinjaJacket ), "Armadura de Couro", 1030206, 85.0, 110.0, typeof( Leather ), 1044462, 13, 1044463 );
			AddCraft( typeof( LeatherNinjaBelt ), "Armadura de Couro", 1030203, 50.0, 75.0, typeof( Leather ), 1044462, 5, 1044463 );
			AddCraft( typeof( LeatherNinjaMitts ), "Armadura de Couro", 1030205, 65.0, 90.0, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( typeof( LeatherNinjaHood ), "Armadura de Couro", 1030201, 90.0, 115.0, typeof( Leather ), 1044462, 14, 1044463 );

            AddCraft(typeof(PugilistMits), "Armadura de Couro", "pugilist gloves", 32.9, 57.9, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(ThrowingGloves), "Armadura de Couro", "throwing gloves", 32.9, 57.9, typeof(Leather), 1044462, 8, 1044463);

            AddCraft(typeof(StuddedGorget), "Armadura de Couro", 1025078, 78.8, 103.8, typeof(Leather), 1044462, 6, 1044463);
            AddCraft(typeof(StuddedGloves), "Armadura de Couro", 1025077, 82.9, 107.9, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(StuddedArms), "Armadura de Couro", 1025076, 87.1, 112.1, typeof(Leather), 1044462, 10, 1044463);
            AddCraft(typeof(StuddedLegs), "Armadura de Couro", 1025082, 91.2, 116.2, typeof(Leather), 1044462, 12, 1044463);
            AddCraft(typeof(StuddedSkirt), "Armadura de Couro", "studded skirt", 91.2, 116.2, typeof(Leather), 1044462, 12, 1044463);
            AddCraft(typeof(StuddedChest), "Armadura de Couro", 1025083, 94.0, 119.0, typeof(Leather), 1044462, 14, 1044463);
            AddCraft(typeof(StuddedBustierArms), "Armadura de Couro", 1027180, 82.9, 107.9, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(FemaleStuddedChest), "Armadura de Couro", 1027170, 87.1, 112.1, typeof(Leather), 1044462, 10, 1044463);
            AddCraft(typeof(StuddedMempo), "Armadura de Couro", 1030216, 80.0, 105.0, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(StuddedDo), "Armadura de Couro", 1030183, 95.0, 120.0, typeof(Leather), 1044462, 14, 1044463);
            AddCraft(typeof(StuddedHiroSode), "Armadura de Couro", 1030186, 85.0, 110.0, typeof(Leather), 1044462, 8, 1044463);
            AddCraft(typeof(StuddedSuneate), "Armadura de Couro", 1030194, 92.0, 117.0, typeof(Leather), 1044462, 14, 1044463);
            AddCraft(typeof(StuddedHaidate), "Armadura de Couro", 1030198, 92.0, 117.0, typeof(Leather), 1044462, 14, 1044463);
            #endregion

            #region Bone Armor
            index = AddCraft( typeof( BoneHelm ), "Armadura de Ossos", 1025206, 85.0, 110.0, typeof( Leather ), 1044462, 4, 1044463 );
				AddRes( index, typeof( PolishedSkull ), "Polished Skull", 1, 1049063 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 1, 1049063 );
			index = AddCraft( typeof( BoneGloves ), "Armadura de Ossos", 1025205, 89.0, 114.0, typeof( Leather ), 1044462, 6, 1044463 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 2, 1049063 );
			index = AddCraft( typeof( BoneArms ), "Armadura de Ossos", 1025203, 92.0, 117.0, typeof( Leather ), 1044462, 8, 1044463 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 4, 1049063 );
			index = AddCraft( typeof( BoneLegs ), "Armadura de Ossos", 1025202, 95.0, 120.0, typeof( Leather ), 1044462, 10, 1044463 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 6, 1049063 );
			index = AddCraft( typeof( BoneSkirt ), "Armadura de Ossos", "bone skirt", 95.0, 120.0, typeof( Leather ), 1044462, 10, 1044463 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 6, 1049063 );
			index = AddCraft( typeof( BoneChest ), "Armadura de Ossos", 1025199, 96.0, 121.0, typeof( Leather ), 1044462, 12, 1044463 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 10, 1049063 );
			index = AddCraft(typeof(OrcHelm), "Armadura de Ossos", "horned helm", 90.0, 115.0, typeof(Leather), 1044462, 6, 1044463);
				AddRes( index, typeof( PolishedSkull ), "Polished Skull", 1, 1049063 );
				AddRes( index, typeof( PolishedBone ), "Polished Bone", 3, 1049063 );
			#endregion

			#region "Sacos/Sacolas/Mochilas"
			AddCraft( typeof( Backpack ), "Sacos/Sacolas/Mochilas", "backpack", 8.2, 33.2, typeof( Leather ), 1044462, 3, 1044463 );
			AddCraft( typeof( RuggedBackpack ), "Sacos/Sacolas/Mochilas", "backpack, rugged", 10.7, 40.7, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( Pouch ), "Sacos/Sacolas/Mochilas", "pouch", 0.0, 25.0, typeof( Leather ), 1044462, 2, 1044463 );
			AddCraft( typeof( Bag ), "Sacos/Sacolas/Mochilas", "bag", 0.0, 25.0, typeof( Leather ), 1044462, 3, 1044463 );
			AddCraft( typeof( LargeBag ), "Sacos/Sacolas/Mochilas", "bag, large", 16.5, 41.5, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( typeof( GiantBag ), "Sacos/Sacolas/Mochilas", "bag, giant", 26.0, 51.0, typeof( Leather ), 1044462, 9, 1044463 );
			AddCraft( typeof( LargeSack ), "Sacos/Sacolas/Mochilas", "rucksack", 20.7, 45.7, typeof( Leather ), 1044462, 7, 1044463 );
			AddCraft(typeof(AlchemyPouch), "Sacos/Sacolas/Mochilas", "Alchemy Rucksack", 90.0, 115.0, typeof(GoliathLeather), 1044462, 25, 1049311);
			AddCraft(typeof(MinersPouch), "Sacos/Sacolas/Mochilas", "Miners Rucksack", 90.0, 115.0, typeof(GoliathLeather), 1044462, 50, 1049311);
			AddCraft(typeof(LumberjackPouch), "Sacos/Sacolas/Mochilas", "Lumberjacks Rucksack", 90.0, 115.0, typeof(GoliathLeather), 1044462, 50, 1049311);
			AddCraft(typeof(CoinPouch), "Sacos/Sacolas/Mochilas", "Coin Pouch", 90.0, 115.0, typeof(GoliathLeather), 1044462, 25, 1049311);

            #endregion

            // Set the overridable material
            SetSubRes(typeof(CottonCloth), 1067440);

            AddSubRes(typeof(CottonCloth), 1067440, 0.0, 1044458, 1054019);
            AddSubRes(typeof(WoolCloth), 1067443, 60.0, 1044458, 1054019);
            AddSubRes(typeof(FlaxCloth), 1067441, 70.0, 1044458, 1054019);
            AddSubRes(typeof(SilkCloth), 1067442, 80.0, 1044458, 1054019);
            AddSubRes(typeof(PoliesterCloth), 1067444, 80.0, 1044458, 1054019);

            // Set the overridable material
            SetSubRes2( typeof( Leather ), 1049150 );

			// Add every material you want the player to be able to choose from
			// This will override the overridable material
			AddSubRes2( typeof( Leather ),			1049150, 00.0, 1044462, 1049311 );
            AddSubRes2( typeof( SpinedLeather),		1049151, 70.0, 1044462, 1049311);
            AddSubRes2( typeof( HornedLeather ),	1049152, 85.0, 1044462, 1049311 );
            AddSubRes2(typeof( BarbedLeather),		1049153, 80.0, 1044462, 1049311);
            AddSubRes2(typeof(AlienLeather), 1034444, 110.0, 1044462, 1049311);
            /*			AddSubRes2( typeof( NecroticLeather ),	1034403, 90.0, 1044462, 1049311 );
                        AddSubRes2( typeof( VolcanicLeather ),	1034414, 90.0, 1044462, 1049311 );
                        AddSubRes2( typeof( FrozenLeather ),	1034425, 95.0, 1044462, 1049311 );
                        AddSubRes2( typeof( GoliathLeather ),	1034370, 95.0, 1044462, 1049311 );
                        AddSubRes2( typeof( DraconicLeather ),	1034381, 100.0, 1044462, 1049311 );
                        AddSubRes2( typeof( HellishLeather ),	1034392, 100.0, 1044462, 1049311 );
                        AddSubRes2( typeof( DinosaurLeather ),	1036104, 105.0, 1044462, 1049311 );
             */



            MarkOption = true;
			Repair = Core.AOS;
			CanEnhance = Core.AOS;
		}
	}
}