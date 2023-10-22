using System;
using Server.Items;

namespace Server.Engines.Craft
{
	public class DefShelves : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Carpentry;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044004; } // <CENTER>CARPENTRY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefShelves();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefShelves() : base( 1, 1, 1.25 )// base( 1, 1, 3.0 )
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
			// no animation
			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 9, 5, 1, true, false, 0 );

			from.PlaySound( 0x23D );
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

            

            /*AddCraft(typeof(BallotBoxDeed), 1044290, 1044327, 47.3, 72.3, typeof(Board), 1015101, 5, 1044351);
            index = AddCraft(typeof(PentagramDeed), 1044290, 1044328, 100.0, 125.0, typeof(Board), 1015101, 100, 1044351);
            AddSkill(index, SkillName.Magery, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);
            index = AddCraft(typeof(AbbatoirDeed), 1044290, 1044329, 100.0, 125.0, typeof(Board), 1015101, 100, 1044351);
            AddSkill(index, SkillName.Magery, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);

            index = AddCraft(typeof(AbbatoirDeed), 1044290, 1044329, 100.0, 125.0, typeof(Board), 1015101, 100, 1044351);
            AddSkill(index, SkillName.Magery, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 40, 1044037);

            AddCraft(typeof(PlayerBBEast), 1044290, 1062420, 85.0, 110.0, typeof(Board), 1015101, 50, 1044351);
            AddCraft(typeof(PlayerBBSouth), 1044290, 1062421, 85.0, 110.0, typeof(Board), 1015101, 50, 1044351);*/

            //beds 
            /*index = AddCraft( typeof( SmallBedSouthDeed ), 1044290, 1044321, 94.7, 119.8, typeof( Board ), 1015101, 100, 1044351 );
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
			index = AddCraft(typeof(SmallBedEastDeed), 1044290, 1044322, 94.7, 119.8, typeof(Log), 1015101, 100, 1044351);
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
			index = AddCraft(typeof(LargeBedSouthDeed), 1044290, 1044323, 94.7, 119.8, typeof(Log), 1015101, 150, 1044351);
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
			index = AddCraft(typeof(LargeBedEastDeed), 1044290, 1044324, 60.7, 90.8, typeof(Log), 1015101, 150, 1044351);
			AddSkill( index, SkillName.Tailoring, 75.0, 80.0 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );*/

            /*index = AddCraft(typeof(SleeperSmallSouthAddonDeed), 1044290, "sleeper small east", 30.0, 60.0, typeof(Board), 1015101, 100, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 40, "pano");
            index = AddCraft(typeof(SleeperSmallEWAddonDeed), 1044290, "sleeper small south", 30.0, 60.0, typeof(Board), 1015101, 100, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 40, "pano");
            index = AddCraft(typeof(SleeperFutonEWAddonDeed), 1044290, "sleeper futon east", 40.0, 70.0, typeof(Board), 1015101, 10, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 200, "pano");
            index = AddCraft(typeof(SleeperFutonNSAddonDeed), 1044290, "sleeper futon south", 40.0, 70.0, typeof(Board), 1015101, 10, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 200, "pano");
            index = AddCraft(typeof(SleeperEWAddonDeed), 1044290, "sleeper bed east", 60.0, 90.0, typeof(Board), 1015101, 200, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 80, "pano");
            index = AddCraft(typeof(SleeperNSAddonDeed), 1044290, "sleeper bed south", 60.0, 90.0, typeof(Board), 1015101, 200, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 80, "pano");
            index = AddCraft(typeof(SleeperElvenEWAddonDeed), 1044290, "sleeper elven east", 70.0, 100.0, typeof(Board), 1015101, 500, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 150, "pano");
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 5, 1042081);
            AddRes(index, typeof(IronIngot), 1044036, 20, 1044037);
            index = AddCraft(typeof(SleeperElvenSouthAddonDeed), 1044290, "sleeper elven south", 70.0, 100.0, typeof(Board), 1015101, 500, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 150, "pano");
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 5, 1042081);
            AddRes(index, typeof(IronIngot), 1044036, 20, 1044037);
            index = AddCraft(typeof(SleeperTallElvenEastAddonDeed), 1044290, "sleeper tall elven east", 90.0, 119.0, typeof(Board), 1015101, 1000, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 300, "pano");
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 10, 1042081);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);
            index = AddCraft(typeof(SleeperTallElvenSouthAddonDeed), 1044290, "sleeper tall elven south", 90.0, 119.0, typeof(Board), 1015101, 1000, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 300, "pano");
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 10, 1042081);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);*/
            /*			// 5
                        AddCraft( typeof( NewDrawersA ), "Cômodas", "dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersB ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersC ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersD ), "Cômodas", "open ", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersE ), "Cômodas", "nightstand", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersF ), "Cômodas", "dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersG ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersH ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersI ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersJ ), "Cômodas", "nightstand", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( Drawer ),		 "Cômodas", "dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersL ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersM ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersN ), "Cômodas", "open dresser", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( NewDrawersK ), "Cômodas", "nightstand", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( ColoredDresserA ), "Cômodas", "dresser*", 31.5, 56.5, typeof( Board ), 1015101, 35, 1044351 );
                        AddCraft( typeof( ColoredDresserB ), "Cômodas", "fancy dresser*", 31.5, 56.5, typeof( Board ), 1015101, 35, 1044351 );
                        AddCraft( typeof( ColoredDresserC ), "Cômodas", "medium dresser*", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( ColoredDresserD ), "Cômodas", "medium dresser*", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( ColoredDresserE ), "Cômodas", "short elegant dresser*", 31.5, 56.5, typeof( Board ), 1015101, 25, 1044351 );
                        AddCraft( typeof( ColoredDresserF ), "Cômodas", "short narrow dresser*", 31.5, 56.5, typeof( Board ), 1015101, 25, 1044351 );
                        AddCraft( typeof( ColoredDresserG ), "Cômodas", "short wide dresser*", 31.5, 56.5, typeof( Board ), 1015101, 27, 1044351 );
                        AddCraft( typeof( ColoredDresserH ), "Cômodas", "standing dresser*", 31.5, 56.5, typeof( Board ), 1015101, 27, 1044351 );
                        AddCraft( typeof( ColoredDresserI ), "Cômodas", "trinket dresser*", 31.5, 56.5, typeof( Board ), 1015101, 30, 1044351 );
                        AddCraft( typeof( ColoredDresserJ ), "Cômodas", "wide medium dresser*", 31.5, 56.5, typeof( Board ), 1015101, 35, 1044351 );*/

            // 6

            /*
			AddCraft( typeof( NewWizardShelfC ), "Estantes de bambu", "alchemy shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmorShelfB ), "Estantes de bambu", "armor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfA ), "Estantes de bambu", "baker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfB ), "Estantes de bambu", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfC ), "Estantes de bambu", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfD ), "Estantes de bambu", "book shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBowyerShelfA ), "Estantes de bambu", "bowyer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTannerShelfB ), "Estantes de bambu", "carpenter shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfA ), "Estantes de bambu", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfB ), "Estantes de bambu", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShoeShelfA ), "Estantes de bambu", "cobbler shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDrinkShelfA ), "Estantes de bambu", "drink shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewHunterShelf ), "Estantes de bambu", "hunter shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewKitchenShelfB ), "Estantes de bambu", "kitchen shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSorcererShelfA ), "Estantes de bambu", "sorcerer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTailorShelfA ), "Estantes de bambu", "tailor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTannerShelfA ), "Estantes de bambu", "supply shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTavernShelfC ), "Estantes de bambu", "tavern shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBlacksmithShelfB ), "Estantes de bambu", "tinker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );*/

            // 7
            //AddCraft(typeof(NewShelfC), "Estantes", "Estante Maciça", 41.5, 66.5, typeof(Board), 1015101, 35, 1044351);
            /*AddCraft( typeof( NewShelfC ), "Estantes Maciças", "shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShelfD ), "Estantes Maciças", "shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewWizardShelfD ), "Estantes Maciças", "alchemy shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmorShelfC ), "Estantes Maciças", "armor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfC ), "Estantes Maciças", "baker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfE ), "Estantes Maciças", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfF ), "Estantes Maciças", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfG ), "Estantes Maciças", "book shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBowyerShelfB ), "Estantes Maciças", "bowyer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewCarpenterShelfA ), "Estantes Maciças", "carpenter shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfC ), "Estantes Maciças", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfD ), "Estantes Maciças", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShoeShelfB ), "Estantes Maciças", "cobbler shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDrinkShelfB ), "Estantes Maciças", "drink shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfB ), "Estantes Maciças", "kitchen shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBlacksmithShelfC ), "Estantes Maciças", "smith shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSorcererShelfB ), "Estantes Maciças", "sorcerer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSupplyShelfA ), "Estantes Maciças", "supply shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTailorShelfB ), "Estantes Maciças", "tailor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTavernShelfD ), "Estantes Maciças", "tavern shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTinkerShelfA ), "Estantes Maciças", "tinker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );*/

            // 8

            /*
			AddCraft( typeof( NewWizardShelfE ), "Estantes Rústicas", "alchemy shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmorShelfD ), "Estantes Rústicas", "armor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfE ), "Estantes Rústicas", "baker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfH ), "Estantes Rústicas", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfI ), "Estantes Rústicas", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfJ ), "Estantes Rústicas", "book shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBowyerShelfC ), "Estantes Rústicas", "bowyer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewCarpenterShelfB ), "Estantes Rústicas", "carpenter shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfE ), "Estantes Rústicas", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfF ), "Estantes Rústicas", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShoeShelfC ), "Estantes Rústicas", "cobbler shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDrinkShelfC ), "Estantes Rústicas", "drink shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfD ), "Estantes Rústicas", "kitchen shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBlacksmithShelfD ), "Estantes Rústicas", "smith shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSorcererShelfC ), "Estantes Rústicas", "sorcerer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSupplyShelfB ), "Estantes Rústicas", "supply shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTailorShelfC ), "Estantes Rústicas", "tailor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTavernShelfE ), "Estantes Rústicas", "tavern shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTinkerShelfB ), "Estantes Rústicas", "tinker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );*/

            // 9
            /*AddCraft( typeof( ColoredShelf1 ), "Estantes MDF", "alchemy shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf2 ), "Estantes MDF", "armor shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf3 ), "Estantes MDF", "baker shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf4 ), "Estantes MDF", "barkeep shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf5 ), "Estantes MDF", "book shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf6 ), "Estantes MDF", "bowyer shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf7 ), "Estantes MDF", "carpenter shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelf8 ), "Estantes MDF", "cloth shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfA ), "Estantes MDF", "cobbler shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfN ), "Estantes MDF", "cobbler shelf*", 41.5, 66.5, typeof( Board ), 1015101, 25, 1044351 );
			AddCraft( typeof( ColoredShelfB ), "Estantes MDF", "drink shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfC ), "Estantes MDF", "empty shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfD ), "Estantes MDF", "food shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfE ), "Estantes MDF", "kitchen shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfF ), "Estantes MDF", "library shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfG ), "Estantes MDF", "liquor shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfH ), "Estantes MDF", "necromancer shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfI ), "Estantes MDF", "plain cloth shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfJ ), "Estantes MDF", "provisions shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( SailorShelf ), "Estantes MDF", "sailor shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfK ), "Estantes MDF", "short book shelf*", 41.5, 66.5, typeof( Board ), 1015101, 25, 1044351 );
			AddCraft( typeof( ColoredShelfL ), "Estantes MDF", "short empty shelf*", 41.5, 66.5, typeof( Board ), 1015101, 25, 1044351 );
			AddCraft( typeof( ColoredShelfM ), "Estantes MDF", "short kitchen shelf*", 41.5, 66.5, typeof( Board ), 1015101, 25, 1044351 );
			AddCraft( typeof( ColoredShelfO ), "Estantes MDF", "smith shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfP ), "Estantes MDF", "storage shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfQ ), "Estantes MDF", "supply shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfR ), "Estantes MDF", "tailor shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfS ), "Estantes MDF", "tall supply shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfT ), "Estantes MDF", "tall wizard shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfU ), "Estantes MDF", "tamer shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfV ), "Estantes MDF", "tavern shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfW ), "Estantes MDF", "tinker shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfX ), "Estantes MDF", "tome shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfY ), "Estantes MDF", "weaver shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredShelfZ ), "Estantes MDF", "wizard shelf*", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );*/

            // 10

            /*AddCraft( typeof( FullBookcase ), "Estantes MDP", 1022711,	41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShelfG ), "Estantes MDP", "shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShelfH ), "Estantes MDP", "shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewWizardShelfF ), "Estantes MDP", "alchemy shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmorShelfA ), "Estantes MDP", "armor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmorShelfE ), "Estantes MDP", "armor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfG ), "Estantes MDP", "baker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfK ), "Estantes MDP", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfL ), "Estantes MDP", "book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfM ), "Estantes MDP", "book shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBookShelfA ), "Estantes MDP", "book shelf, tall", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBowyerShelfD ), "Estantes MDP", "bowyer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewCarpenterShelfC ), "Estantes MDP", "carpenter shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfG ), "Estantes MDP", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewClothShelfH ), "Estantes MDP", "cloth shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewShoeShelfD ), "Estantes MDP", "cobbler shelf, small", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDarkBookShelfA ), "Estantes MDP", "dark book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDarkBookShelfB ), "Estantes MDP", "dark book shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDarkShelf ), "Estantes MDP", "dark shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDrinkShelfD ), "Estantes MDP", "drink shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewHelmShelf ), "Estantes MDP", "helm shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBakerShelfF ), "Estantes MDP", "kitchen shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewKitchenShelfA ), "Estantes MDP", "kitchen shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewDrinkShelfE ), "Estantes MDP", "liquor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewOldBookShelf ), "Estantes MDP", "old book shelf",	41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewPotionShelf ), "Estantes MDP", "potion shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewRuinedBookShelf ), "Estantes MDP", "ruined book shelf",	41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewBlacksmithShelfE ), "Estantes MDP", "smith shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSorcererShelfD ), "Estantes MDP", "sorcerer shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewSupplyShelfC ), "Estantes MDP", "supply shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTailorShelfD ), "Estantes MDP", "tailor shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTavernShelfF ), "Estantes MDP", "tavern shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTinkerShelfC ), "Estantes MDP", "tinker shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewTortureShelf ), "Estantes MDP", "torture shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewWizardShelfB ), "Estantes MDP", "wizard shelf", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewWizardShelfA ), "Estantes MDP", "wizard shelf, tall", 41.5, 66.5, typeof( Board ), 1015101, 35, 1044351 );*/


            Repair = true;
			MarkOption = true;
			CanEnhance = Core.AOS;

			SetSubRes( typeof( Board ), 1072643 );

			// Add every material you want the player to be able to choose from
			// This will override the overridable material	TODO: Verify the required skill amount
			AddSubRes( typeof( Board ), 1072643, 00.0, 1015101, 1072652 );
			AddSubRes( typeof( AshBoard ), 1095379, 60.0, 1015101, 1072652 );
            AddSubRes( typeof( EbonyBoard ), 1095381, 70.0, 1015101, 1072652 );
            AddSubRes(typeof(ElvenBoard), 1095535, 80.0, 1015101, 1072652);
            AddSubRes(typeof(GoldenOakBoard), 1095382, 85.0, 1015101, 1072652);
            AddSubRes(typeof(CherryBoard), 1095380, 90.0, 1015101, 1072652);
            AddSubRes(typeof(RosewoodBoard), 1095387, 95.0, 1015101, 1072652);
            AddSubRes( typeof( HickoryBoard ), 1095383, 100.0, 1015101, 1072652 );
            /*AddSubRes( typeof( MahoganyBoard ), 1095384, 90.0, 1015101, 1072652 );
			AddSubRes( typeof( DriftwoodBoard ), 1095409, 90.0, 1015101, 1072652 );
			AddSubRes( typeof( OakBoard ), 1095385, 95.0, 1015101, 1072652 );
			AddSubRes( typeof( PineBoard ), 1095386, 100.0, 1015101, 1072652 );
			AddSubRes( typeof( GhostBoard ), 1095511, 100.0, 1015101, 1072652 );*/
            
			/*AddSubRes( typeof( WalnutBoard ), 1095388, 100.0, 1015101, 1072652 );
			AddSubRes( typeof( PetrifiedBoard ), 1095532, 100.0, 1015101, 1072652 );*/
			
		}
	}
}