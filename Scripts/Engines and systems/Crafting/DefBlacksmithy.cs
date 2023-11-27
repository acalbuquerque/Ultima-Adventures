using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefBlacksmithy : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Blacksmith;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044002; } // <CENTER>BLACKSMITHY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefBlacksmithy();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefBlacksmithy() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
		{
			/*
			
			base( MinCraftEffect, MaxCraftEffect, Delay )
			
			MinCraftEffect	: The minimum number of time the mobile will play the craft effect
			MaxCraftEffect	: The maximum number of time the mobile will play the craft effect
			Delay			: The delay between each craft effect
			
			Example: (3, 6, 1.7) would make the mobile do the PlayCraftEffect override
			function between 3 and 6 time, with a 1.7 second delay each time.
			
			*/ 
		}

		private static Type typeofAnvil = typeof( AnvilAttribute );
		private static Type typeofForge = typeof( ForgeAttribute );

		public static bool IsForge( object obj )
		{
			if ( Core.ML && obj is Mobile && ((Mobile)obj).IsDeadBondedPet )
				return false;

			if ( obj.GetType().IsDefined( typeof( ForgeAttribute ), false ) )
				return true;

			int itemID = 0;

			if ( obj is Item )
				itemID = ((Item)obj).ItemID;
			else if ( obj is StaticTarget )
				itemID = ((StaticTarget)obj).ItemID;

			if ( itemID >= 6896 && itemID <= 6898 )
			{
				if ( obj is FireGiantForge )
				{
					FireGiantForge kettle = (FireGiantForge)obj;
					Server.Items.FireGiantForge.ConsumeCharge( kettle );
					return true;
				}
			}

			return ( itemID == 4017 || (itemID >= 0x10DE && itemID <= 0x10E0) || (itemID >= 6522 && itemID <= 6569) || (itemID >= 0x544B && itemID <= 0x544E) );
		}

		public static void CheckAnvilAndForge( Mobile from, int range, out bool anvil, out bool forge )
		{
			anvil = false;
			forge = false;

			Map map = from.Map;

			if ( map == null )
				return;

			IPooledEnumerable eable = map.GetItemsInRange( from.Location, range );

			foreach ( Item item in eable )
			{
				Type type = item.GetType();

				bool isAnvil = ( type.IsDefined( typeofAnvil, false ) || item.ItemID == 4015 || item.ItemID == 4016 || item.ItemID == 0x2DD5 || item.ItemID == 0x2DD6 || item.ItemID == 0x2B55 || item.ItemID == 0x2B57 );
				bool isForge = ( type.IsDefined( typeofForge, false ) || item.ItemID == 4017 || (item.ItemID >= 0x10DE && item.ItemID <= 0x10E0) || (item.ItemID >= 6522 && item.ItemID <= 6569) || item.ItemID == 0x2DD8 || (item.ItemID >= 0x544B && item.ItemID <= 0x544E) );

				if ( isAnvil || isForge )
				{
					if ( (from.Z + 16) < item.Z || (item.Z + 16) < from.Z || !from.InLOS( item ) )
						continue;

					anvil = anvil || isAnvil;
					forge = forge || isForge;

					if ( anvil && forge )
						break;
				}
			}

			eable.Free();

			for ( int x = -range; (!anvil || !forge) && x <= range; ++x )
			{
				for ( int y = -range; (!anvil || !forge) && y <= range; ++y )
				{
					StaticTile[] tiles = map.Tiles.GetStaticTiles( from.X+x, from.Y+y, true );

					for ( int i = 0; (!anvil || !forge) && i < tiles.Length; ++i )
					{
						int id = tiles[i].ID;

						bool isAnvil = ( id == 4015 || id == 4016 || id == 0x2DD5 || id == 0x2DD6 || id == 0x2B55 || id == 0x2B57 || id == 0xFAF);
						bool isForge = ( id == 4017 || (id >= 0x10DE && id <= 0x10E0) || (id >= 6522 && id <= 6569) || id == 0x2DD8 || (id >= 0x544B && id <= 0x544E) || (id >= 0x197A && id <= 0x1984)  );

						if ( isAnvil || isForge )
						{
							if ( (from.Z + 16) < tiles[i].Z || (tiles[i].Z + 16) < from.Z || !from.InLOS( new Point3D( from.X+x, from.Y+y, tiles[i].Z + (tiles[i].Height/2) + 1 ) ) )
								continue;

							anvil = anvil || isAnvil;
							forge = forge || isForge;
						}
					}
				}
			}

			if (from.Map == Map.Felucca && from.X >= 6896 && from.X <= 6912 && from.Y >= 145 && from.Y <= 163)
			{ //wasnt working in skara for some reason
				anvil = true;
				forge = true;
			}
			if (from.Map == Map.Felucca && from.X >= 6911 && from.X <= 6920 && from.Y >= 179 && from.Y <= 186)
			{ //wasnt working in skara for some reason
				anvil = true;
				forge = true;
			}
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			bool anvil, forge;

			CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( anvil && forge )
				return 0;

			return 1044267; // You must be near an anvil and a forge to smith items.
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no animation, instant sound
			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 9, 5, 1, true, false, 0 );
			//new InternalTimer( from ).Start();

			from.PlaySound( 0x541 );
		}

		// Delay to synchronize the sound with the hit on the anvil
		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x541 );
			}
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
            /*
			Synthax for a SIMPLE craft item
			AddCraft( ObjectType, Group, MinSkill, MaxSkill, ResourceType, Amount, Message )
			
			ObjectType		: The type of the object you want to add to the build list.
			Group			: The group in wich the object will be showed in the craft menu.
			MinSkill		: The minimum of skill value
			MaxSkill		: The maximum of skill value
			ResourceType	: The type of the resource the mobile need to create the item
			Amount			: The amount of the ResourceType it need to create the item
			Message			: String or Int for Localized.  The message that will be sent to the mobile, if the specified resource is missing.
			
			Synthax for a COMPLEXE craft item.  A complexe item is an item that need either more than
			only one skill, or more than only one resource.
		
			*/
            string chainRingTitle = "Loriga & Malhas";

            #region Ringmail
            AddCraft(typeof(RingmailGloves), chainRingTitle, "Luvas de Loriga", 32.0, 62.0, typeof(IronIngot), 1044036, 8, 1044037);
            AddCraft(typeof(RingmailLegs), chainRingTitle, "Calça de Loriga", 40.4, 69.4, typeof(IronIngot), 1044036, 12, 1044037);
            AddCraft(typeof(RingmailArms), chainRingTitle, "Ombreiras de Loriga", 36.9, 66.9, typeof(IronIngot), 1044036, 10, 1044037);
            AddCraft(typeof(RingmailChest), chainRingTitle, "Peitoral de Loriga", 44.9, 74.9, typeof(IronIngot), 1044036, 14, 1044037);
            #endregion

            #region Chainmail
            AddCraft( typeof( ChainCoif ), chainRingTitle, "Coifa de Malha", 34.5, 64.5, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( ChainLegs ), chainRingTitle, "Calça de Malha", 46.7, 86.7, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( ChainChest ), chainRingTitle, "Tunica de Malha", 49.1, 89.1, typeof( IronIngot ), 1044036, 16, 1044037 );
            #endregion

/*			AddCraft( typeof( RingmailSkirt ), chainRingTitle, "banded mail skirt", 19.4, 69.4, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( ChainSkirt ), chainRingTitle, "metal skirt", 36.7, 86.7, typeof( IronIngot ), 1044036, 18, 1044037 );*/

			int index = -1;

            #region Platemail
            string platemailTitle = "Armadura de Metal";
            AddCraft( typeof( PlateArms ), platemailTitle, "Ombreiras de Metal", 66.3, 116.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( PlateGloves ), platemailTitle, "Luvas de Metal", 58.9, 108.9, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( PlateGorget ), platemailTitle, "Gorgel de Metal", 56.4, 106.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( PlateLegs ), platemailTitle, "Calças de Metal", 70.8, 118.8, typeof( IronIngot ), 1044036, 20, 1044037 );
			//AddCraft( typeof( PlateSkirt ), chainRingTitle, "platemail skirt", 68.8, 118.8, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( PlateChest ), platemailTitle, "Peitoral de Metal", 75.0, 125.0, typeof( IronIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( FemalePlateChest ), platemailTitle, "Peitoral Feminino de Metal", 64.1, 104.1, typeof( IronIngot ), 1044036, 22, 1044037 );

            index = AddCraft(typeof(HorseArmor), platemailTitle, "Armadura para Cavalos", 90.0, 100.1, typeof(IronIngot), 1044036, 250, 1044037);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            /*index = AddCraft( typeof( DragonBardingDeed ), platemailTitle, 1053012, 72.5, 150.1, typeof( IronIngot ), 1044036, 650, 1044037 );
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);*/


            /*index = AddCraft( typeof( PlateMempo ), "Platemail", 1030180, 80.0, 130.0, typeof( IronIngot ), 1044036, 18, 1044037 );
			index = AddCraft( typeof( PlateDo ), "Platemail", 1030184, 80.0, 130.0, typeof( IronIngot ), 1044036, 28, 1044037 );
			index = AddCraft( typeof( PlateHiroSode ), "Platemail", 1030187, 80.0, 130.0, typeof( IronIngot ), 1044036, 16, 1044037 );
			index = AddCraft( typeof( PlateSuneate ), "Platemail", 1030195, 65.0, 115.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			index = AddCraft( typeof( PlateHaidate ), "Platemail", 1030200, 65.0, 115.0, typeof( IronIngot ), 1044036, 20, 1044037 );*/

            #endregion

            #region Royal
            string royalTitle = "Armadura de Real";

            index = AddCraft( typeof( RoyalGloves ), royalTitle, "Braçadeiras Real", 70.0, 140.1, typeof( IronIngot ), 1044036, 12, 1044037 );
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            index = AddCraft(typeof(RoyalGorget), royalTitle, "Gorgel Real", 72.4, 140.1, typeof(IronIngot), 1044036, 8, 1044037);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            index = AddCraft( typeof( RoyalHelm ), royalTitle, "Elmo Real", 75.0, 140.1, typeof( IronIngot ), 1044036, 15, 1044037 );
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            index = AddCraft( typeof( RoyalsLegs ), royalTitle, "Calças Real", 80.0, 140.1, typeof( IronIngot ), 1044036, 20, 1044037 );
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            index = AddCraft(typeof(RoyalBoots), royalTitle, "Botas Real", 82.9, 140.1, typeof(IronIngot), 1044036, 9, 1044037);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            index = AddCraft( typeof( RoyalArms ), royalTitle, "Ombreira Real", 85.0, 140.1, typeof( IronIngot ), 1044036, 18, 1044037 );
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            index = AddCraft( typeof( RoyalChest ), royalTitle, "Tunica Real", 90.0, 140.1, typeof( IronIngot ), 1044036, 24, 1044037 );
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 70.0, 110.0);
            #endregion

            #region Dragon Scale Armor

            string scalemailTitle = "Armadura de Escamas";

			index = AddCraft( typeof( DragonGloves ), scalemailTitle, "Luvas de Escamas", 70.0, 150.1, typeof( RedScales ), "Escamas Reptilianas", 12, 1042081 );
            AddRes(index, typeof(PlatinumIngot), "Lingotes de Platina", 6, 1042081);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 80.0, 110.0);
            SetUseSubRes2( index, true );

			index = AddCraft( typeof( DragonHelm ), scalemailTitle, "Elmo de Escamas", 75.0, 150.1, typeof( RedScales ), "Escamas Reptilianas", 16, 1042081 );
            AddRes(index, typeof(PlatinumIngot), "Lingotes de Platina", 8, 1042081);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 80.0, 110.0);
            SetUseSubRes2( index, true );

			index = AddCraft( typeof( DragonLegs ), scalemailTitle, "Calças de Escamas", 80.0, 150.1, typeof( RedScales ), "Escamas Reptilianas", 20, 1042081 );
            AddRes(index, typeof(PlatinumIngot), "Lingotes de Platina", 12, 1042081);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 80.0, 110.0);
            SetUseSubRes2( index, true );

			index = AddCraft( typeof( DragonArms ), scalemailTitle, "Ombreiras de Escamas", 85.0, 150.1, typeof( RedScales ), "Escamas Reptilianas", 18, 1042081 );
            AddRes(index, typeof(PlatinumIngot), "Lingotes de Platina", 10, 1042081);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 80.0, 110.0);
            SetUseSubRes2( index, true );

			index = AddCraft( typeof( DragonChest ), scalemailTitle, "Tunica de Escamas", 90.0, 150.1, typeof( RedScales ), "Escamas Reptilianas", 24, 1042081 );
            AddRes(index, typeof(PlatinumIngot), "Lingotes de Platina", 14, 1042081);
            AddSkill(index, SkillName.ArmsLore, 100.0, 120.0);
            AddSkill(index, SkillName.Magery, 80.0, 110.0);
            SetUseSubRes2( index, true );
            #endregion

            #region Helmets
            string helmetsTitle = "Elmos & Capacetes";

            AddCraft( typeof( Bascinet ), helmetsTitle, "Bacinete ", 28.3, 58.3, typeof( IronIngot ), 1044036, 11, 1044037 );
			AddCraft( typeof( CloseHelm ), helmetsTitle, "Elmo Fechado", 57.9, 87.9, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Helmet ), helmetsTitle, "Elmo Comum", 37.9, 87.9, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( NorseHelm ), helmetsTitle, "Elmo Nórdico", 47.9, 87.9, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( PlateHelm ), helmetsTitle, "Elmo Completo", 62.6, 112.6, typeof( IronIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( DreadHelm ), helmetsTitle, "Elmo de Chifres", 62.6, 112.6, typeof( IronIngot ), 1044036, 15, 1044037 );

            /*if( Core.SE )
			{
				index = AddCraft( typeof( ChainHatsuburi ), "Helmets", 1030175, 30.0, 80.0, typeof( IronIngot ), 1044036, 20, 1044037 );

				index = AddCraft( typeof( PlateHatsuburi ), "Helmets", 1030176, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );

				index = AddCraft( typeof( HeavyPlateJingasa ), "Helmets", 1030178, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );

				index = AddCraft( typeof( LightPlateJingasa ), "Helmets", 1030188, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );

				index = AddCraft( typeof( SmallPlateJingasa ), "Helmets", 1030191, 45.0, 95.0, typeof( IronIngot ), 1044036, 20, 1044037 );

				index = AddCraft( typeof( DecorativePlateKabuto ), "Helmets", 1030179, 90.0, 140.0, typeof( IronIngot ), 1044036, 25, 1044037 );

				index = AddCraft( typeof( PlateBattleKabuto ), "Helmets", 1030192, 90.0, 140.0, typeof( IronIngot ), 1044036, 25, 1044037 );

				index = AddCraft( typeof( StandardPlateKabuto ), "Helmets", 1030196, 90.0, 140.0, typeof( IronIngot ), 1044036, 25, 1044037 );
				 
			}*/
            #endregion

            #region Shields

            string shieldsTitle = "Escudos";

            AddCraft( typeof( Buckler ), shieldsTitle, "Buckler", 0.0, 45.0, typeof( IronIngot ), 1044036, 10, 1044037 );
            AddCraft(typeof( MetalShield ), shieldsTitle, "Escudo Redondo", 15.2, 49.8, typeof(IronIngot), 1044036, 12, 1044037);
            AddCraft(typeof( WoodenKiteShield), shieldsTitle, "Escudo Kite", 20.2, 64.8, typeof(IronIngot), 1044036, 9, 1044037);
            AddCraft( typeof( BronzeShield ), shieldsTitle, "Escudo Bizantino", 35.2, 69.8, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( MetalKiteShield ), shieldsTitle, "Escudo Heater", 54.6, 85.6, typeof( IronIngot ), 1044036, 16, 1044037 );
            AddCraft(typeof(HeaterShield), shieldsTitle, "Escudo Corporal", 65.3, 89.3, typeof(IronIngot), 1044036, 18, 1044037);

            if (Core.AOS)
            {
                AddCraft(typeof(ChaosShield), shieldsTitle, "Escudo do Caos", 85.0, 125.0, typeof(IronIngot), 1044036, 25, 1044037);
                AddCraft(typeof(OrderShield), shieldsTitle, "Escudo da Ordem", 85.0, 125.0, typeof(IronIngot), 1044036, 25, 1044037);
            }

            /*AddCraft( typeof( RoyalShield ), "Shields", "royal shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( GuardsmanShield ), "Shields", "guardsman shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( ElvenShield ), "Shields", "elven shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( DarkShield ), "Shields", "dark shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( CrestedShield ), "Shields", "crested shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( ChampionShield ), "Shields", "champion shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			index = AddCraft( typeof( JeweledShield ), "Shields", "jeweled shield", 54.3, 84.3, typeof( IronIngot ), 1044036, 18, 1044037 );
			AddRes( index, typeof( StarSapphire ), 1023855, 1, 1044037 );*/

            #endregion

            #region Bladed
            string bladesTitle = "Lâminas e Espadas";

            AddCraft(typeof(Dagger), bladesTitle, "Adaga", 0, 49.6, typeof(IronIngot), 1044036, 3, 1044037);
            AddCraft(typeof(Cutlass), bladesTitle, "Cutelo", 24.3, 64.3, typeof(IronIngot), 1044036, 8, 1044037);
            AddCraft(typeof(Scimitar), bladesTitle, "Cimitarra", 31.7, 71.7, typeof(IronIngot), 1044036, 10, 1044037);

            AddCraft(typeof(Kryss), bladesTitle, "Kopesh", 36.7, 88.7, typeof(IronIngot), 1044036, 8, 1044037);
            AddCraft(typeof(Katana), bladesTitle, "Katana", 42.1, 90.1, typeof(IronIngot), 1044036, 9, 1044037);

            AddCraft( typeof( VikingSword ), bladesTitle, "Espada Bastarda", 48.3, 84.3, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Broadsword ), bladesTitle, "Espada Larga", 55.4, 95.4, typeof( IronIngot ), 1044036, 11, 1044037 );

			AddCraft( typeof( Longsword ), bladesTitle, "Espada Longa", 59.0, 88.0, typeof( IronIngot ), 1044036, 14, 1044037 );

            /*AddCraft(typeof(AssassinSpike), bladesTitle, "assassin dagger", 10.0, 49.6, typeof(IronIngot), 1044036, 3, 1044037);
            AddCraft(typeof(ElvenSpellblade), bladesTitle, "assassin sword", 44.1, 94.1, typeof(IronIngot), 1044036, 8, 1044037);
            AddCraft(typeof(CrescentBlade), bladesTitle, 1029921, 45.0, 95.0, typeof(IronIngot), 1044036, 14, 1044037);
            AddCraft(typeof(RadiantScimitar), bladesTitle, "falchion", 35.4, 85.4, typeof(IronIngot), 1044036, 10, 1044037);

            AddCraft( typeof( ElvenMachete ), bladesTitle, "machete", 33.0, 83.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( RoyalSword ), bladesTitle, "royal sword", 54.3, 84.3, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( BoneHarvester ), bladesTitle, "sickle", 33.0, 83.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			
			AddCraft( typeof( RuneBlade ), bladesTitle, "war blades", 28.0, 78.0, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( WarCleaver ), bladesTitle, "war cleaver", 10.0, 49.6, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( Leafblade ), bladesTitle, "war dagger", 20.0, 59.6, typeof( IronIngot ), 1044036, 5, 1044037 );

			if( Core.SE )
			{
				index = AddCraft( typeof( NoDachi ), "Bladed", 1030221, 75.0, 125.0, typeof( IronIngot ), 1044036, 18, 1044037 );
				 
				index = AddCraft( typeof( Wakizashi ), "Bladed", 1030223, 50.0, 100.0, typeof( IronIngot ), 1044036, 8, 1044037 );
				 
				index = AddCraft( typeof( Lajatang ), "Bladed", 1030226, 80.0, 130.0, typeof( IronIngot ), 1044036, 25, 1044037 );
				 
				index = AddCraft( typeof( Daisho ), "Bladed", 1030228, 60.0, 110.0, typeof( IronIngot ), 1044036, 15, 1044037 );
				 
				index = AddCraft( typeof( Tekagi ), "Bladed", 1030230, 55.0, 105.0, typeof( IronIngot ), 1044036, 12, 1044037 );
				 
				index = AddCraft( typeof( Shuriken ), "Bladed", 1030231, 45.0, 95.0, typeof( IronIngot ), 1044036, 5, 1044037 );
				 
				index = AddCraft( typeof( Kama ), "Bladed", 1030232, 40.0, 90.0, typeof( IronIngot ), 1044036, 14, 1044037 );
				 
				index = AddCraft( typeof( Sai ), "Bladed", 1030234, 50.0, 100.0, typeof( IronIngot ), 1044036, 12, 1044037 );
				 
			}*/
            #endregion

            #region Axes
            string axeTitle = "Machados";
            
            AddCraft( typeof( Hatchet ), axeTitle, "Machadinha", 24.2, 64.2, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( LumberAxe ), axeTitle, "Machado de Lenhador", 24.2, 64.2, typeof( IronIngot ), 1044036, 10, 1044037 );
            
            AddCraft( typeof( BattleAxe ), axeTitle, "Machado de Batalha", 30.5, 70.5, typeof( IronIngot ), 1044036, 11, 1044037 );
            AddCraft(typeof(Axe), axeTitle, "Machado Comum", 35.2, 74.2, typeof(IronIngot), 1044036, 12, 1044037);

            AddCraft(typeof(TwoHandedAxe), axeTitle, "Machado de Duas Mãos", 63.0, 83.0, typeof(IronIngot), 1044036, 16, 1044037);

            AddCraft(typeof(WarAxe), axeTitle, "Machado de Guerra", 69.1, 89.1, typeof(IronIngot), 1044036, 13, 1044037);
            AddCraft( typeof( DoubleAxe ), axeTitle, "Machado Duplo", 39.3, 79.3, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( ExecutionersAxe ), axeTitle, "Machado de Carrasco", 44.2, 84.2, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( LargeBattleAxe ), axeTitle, "Machado Grande de Batalha", 58.0, 88.0, typeof( IronIngot ), 1044036, 14, 1044037 );

            //AddCraft( typeof( OrnateAxe ), axeTitle, "barbarian axe", 39.1, 89.1, typeof( IronIngot ), 1044036, 16, 1044037 );

            #endregion

            #region Pole Arms
            string poleArmsTitle = "Lanças & Hastes";

            AddCraft(typeof(Harpoon), poleArmsTitle, "Arpão", 30.0, 70.0, typeof(IronIngot), 1044036, 10, 1044351);

            AddCraft(typeof(ShortSpear), poleArmsTitle, "Lança Pequena", 45.3, 85.3, typeof(IronIngot), 1044036, 8, 1044037);
            AddCraft(typeof(Spear), poleArmsTitle, "Lança", 55.0, 90.0, typeof(IronIngot), 1044036, 12, 1044037);
            if (Core.AOS)
                AddCraft(typeof(Pike), poleArmsTitle, "Pique", 47.0, 87.0, typeof(IronIngot), 1044036, 10, 1044037);

            AddCraft(typeof(WarFork), poleArmsTitle, "Garfo de Guerra", 52.9, 92.9, typeof(IronIngot), 1044036, 12, 1044037);

            AddCraft( typeof( Bardiche ), poleArmsTitle, "Bardiche", 31.7, 85.7, typeof( IronIngot ), 1044036, 16, 1044037 );
            AddCraft(typeof(Halberd), poleArmsTitle, "Alabarda", 39.1, 89.1, typeof(IronIngot), 1044036, 16, 1044037);

            //AddCraft(typeof(Pitchfork), "Polearms", 1023720, 36.1, 86.1, typeof(IronIngot), 1044036, 12, 1044037); // tridente
            /*if ( Core.AOS )
				AddCraft( typeof( BladedStaff ), "Polearms", 1029917, 40.0, 90.0, typeof( IronIngot ), 1044036, 12, 1044037 );

			if ( Core.AOS )
				AddCraft( typeof( DoubleBladedStaff ), "Polearms", 1029919, 45.0, 95.0, typeof( IronIngot ), 1044036, 16, 1044037 );

			if ( Core.AOS )
				AddCraft( typeof( Lance ), "Polearms", 1029920, 48.0, 98.0, typeof( IronIngot ), 1044036, 20, 1044037 );

			if ( Core.AOS )
				AddCraft( typeof( Scythe ), "Polearms", 1029914, 39.0, 89.0, typeof( IronIngot ), 1044036, 14, 1044037 );*/

            // Not craftable (is this an AOS change ??)

            #endregion

            #region Bashing
            string bashingTitle = "Macas e Martelos";

            AddCraft( typeof( HammerPick ), bashingTitle, "Martelo Picareta", 34.2, 84.2, typeof( IronIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Mace ), bashingTitle, "Maca", 14.5, 64.5, typeof( IronIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Maul ), bashingTitle, "Maul", 19.4, 69.4, typeof( IronIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( WarMace ), bashingTitle, "Maca de Guerra", 28.0, 78.0, typeof( IronIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( WarHammer ), bashingTitle, "Martelo de Guerra", 34.2, 84.2, typeof( IronIngot ), 1044036, 13, 1044037 );

            /*AddCraft(typeof(DiamondMace), bashingTitle, "battle mace", 28.0, 78.0, typeof(IronIngot), 1044036, 14, 1044037);
            AddCraft(typeof(Scepter), "Bashing", 1029916, 21.4, 71.4, typeof(IronIngot), 1044036, 10, 1044037);
            index = AddCraft(typeof(Tessen), "Bashing", 1030222, 85.0, 135.0, typeof(IronIngot), 1044036, 16, 1044037);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);*/

            #endregion

            // Set the overridable material
            SetSubRes( typeof( IronIngot ), 1044022 );

			// Add every material you want the player to be able to choose from
			// This will override the overridable material
			// The NameNumber is in CILOC file!!!!
			AddSubRes( typeof( IronIngot ),			1044022, 00.0, 1044036, 1044267 );
			AddSubRes( typeof( DullCopperIngot ),	1044023, 65.0, 1044036, 1044268 );
			AddSubRes( typeof( CopperIngot ),		1044025, 70.0, 1044036, 1044268 );
			AddSubRes( typeof( BronzeIngot ),		1044026, 75.0, 1044036, 1044268 );
            AddSubRes( typeof(ShadowIronIngot),		1044024, 80.0, 1044036, 1044268);
            AddSubRes( typeof(PlatinumIngot),		6663000, 85.0, 1044036, 1044268);
            AddSubRes( typeof( GoldIngot ),			1044027, 85.0, 1044036, 1044268 );
			AddSubRes( typeof( AgapiteIngot ),		1044028, 90.0, 1044036, 1044268 );
			AddSubRes( typeof( VeriteIngot ),		1044029, 95.0, 1044036, 1044268 );
			AddSubRes( typeof( ValoriteIngot ),		1044030, 95.0, 1044036, 1044268 );
            AddSubRes( typeof( TitaniumIngot),		6661000, 100.0, 1044036, 1044268);
            AddSubRes( typeof(RoseniumIngot),		6662000, 100.0, 1044036, 1044268);
            /*AddSubRes( typeof( NepturiteIngot ),	1036173, 105.0, 1044036, 1044268 );
			AddSubRes( typeof( ObsidianIngot ),		1036162, 105.0, 1044036, 1044268 );
			AddSubRes( typeof( SteelIngot ),		1036144, 110.0, 1044036, 1044268 );
			AddSubRes( typeof( BrassIngot ),		1036152, 110.0, 1044036, 1044268 );
			AddSubRes( typeof( MithrilIngot ),		1036137, 115.0, 1044036, 1044268 );
			AddSubRes( typeof( XormiteIngot ),		1034437, 115.0, 1044036, 1044268 );
			AddSubRes( typeof( DwarvenIngot ),		1036181, 120.0, 1044036, 1044268 );*/

			SetSubRes2( typeof( RedScales ), 1060875 );

			AddSubRes2( typeof( RedScales ),		1060875, 0.0, 1053137, 1054018 );
			AddSubRes2( typeof(YellowScales),		1060876, 0.0, 1053137, 1054018 );
			AddSubRes2( typeof( BlackScales ),		1060877, 0.0, 1053137, 1054018 );
			AddSubRes2( typeof( GreenScales ),		1060878, 0.0, 1053137, 1054018 );
			AddSubRes2( typeof( WhiteScales ),		1060879, 0.0, 1053137, 1054018 );
			AddSubRes2( typeof( BlueScales ),		1060880, 0.0, 1053137, 1054018 );
			AddSubRes2( typeof( DinosaurScales ),	1054017, 0.0, 1053137, 1054018 );

			Resmelt = true;
			Repair = true;
			MarkOption = true;
			CanEnhance = Core.AOS;
		}
	}

	public class ForgeAttribute : Attribute
	{
		public ForgeAttribute()
		{
		}
	}

	public class AnvilAttribute : Attribute
	{
		public AnvilAttribute()
		{
		}
	}
}