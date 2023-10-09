using System;
using Server.Items;
using Server.Mobiles;
namespace Server.Engines.Craft
{
	public class DefCarpentry : CraftSystem
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
					m_CraftSystem = new DefCarpentry();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefCarpentry() : base( 1, 1, 1.25 )// base( 1, 1, 3.0 )
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

            //index =	AddCraft( typeof( Board ),			1044294, 1027127,	0.0,   0.0,	typeof( BaseLog ), 1044466,  1, 1044465 );
            //SetUseAllRes( index, true );

            // 1
            AddCraft(typeof(NewArmoireG), "Armários", "Armário de Ripas", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredArmoireB), "Armários", "Armário Bonito", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredArmoireA), "Armários", "Cristaleira", 51.5, 76.5, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(FancyArmoire), "Armários", "Guarda-Roupa", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(Armoire), "Armários", "Roupeiro", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredCabinetB), "Armários", "Armário de Cozinha", 51.5, 76.5, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(ColoredCabinetF), "Armários", "Armário Elegante (Pequeno)", 51.5, 76.5, typeof(Board), 1015101, 25, 1044351);
            AddCraft(typeof(ColoredCabinetM), "Armários", "Armário Elegante (Médio)", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredCabinetE), "Armários", "Armário Simples (Pequeno)", 51.5, 76.5, typeof(Board), 1015101, 25, 1044351);
            AddCraft(typeof(ColoredCabinetC), "Armários", "Armário Simples (Médio)", 51.5, 76.5, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(ColoredCabinetG), "Armários", "Armário Curto", 51.5, 76.5, typeof(Board), 1015101, 25, 1044351);
            AddCraft(typeof(ColoredCabinetD), "Armários", "Armário de Livros (Estreito)", 51.5, 76.5, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(ColoredCabinetA), "Armários", "Armário de Livros (Grande)", 51.5, 76.5, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(ColoredCabinetN), "Armários", "Armário de Armazenamento (Pequeno)", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredCabinetH), "Armários", "Armário de Armazenamento (Grande)", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredCabinetK), "Armários", "Armário Alto Simples", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ColoredCabinetL), "Armários", "Armário Alto Elegante", 51.5, 76.5, typeof(Board), 1015101, 35, 1044351);
            /*AddCraft( typeof( NewArmoireA ), 		"Armários", "bamboo armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmoireB ), 		"Armários", "bamboo armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmoireC ), 		"Armários", "bamboo armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );*/
            /*AddCraft( typeof( NewArmoireD ), 		"Armários", "armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );*/
            /*AddCraft( typeof( NewArmoireE ),		"Armários", "empty armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmoireF ), 		"Armários", "open armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );*/
            /*AddCraft( typeof( NewArmoireH ), 		"Armários", "empty armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmoireI ), 		"Armários", "open armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( NewArmoireJ ), 		"Armários", "open armoire", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );*/
            /*AddCraft( typeof( ColoredCabinetI ), "Gabinetes", "tall fancy cabinet*", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );
			AddCraft( typeof( ColoredCabinetJ ), "Gabinetes", "tall medium cabinet*", 51.5, 76.5, typeof( Board ), 1015101, 35, 1044351 );*/

            // 2
            AddCraft(typeof(SmallCrate), "Baús & Caixas", "Caixa Simples (Pequena)", 10.0, 35.0, typeof(Board), 1015101, 8, 1044351);
            AddCraft(typeof(MediumCrate), "Baús & Caixas", "Caixa Simples (Média)", 31.0, 56.0, typeof(Board), 1015101, 15, 1044351);
            AddCraft(typeof(LargeCrate), "Baús & Caixas", "Caixa Simples (Grande)", 47.3, 72.3, typeof(Board), 1015101, 18, 1044351);
            AddCraft(typeof(WoodenBox), "Baús & Caixas", "Baú de Madeira (Pequeno)", 21.0, 46.0, typeof(Board), 1015101, 10, 1044351);
            AddCraft(typeof(WoodenChest), "Baús & Caixas", "Baú de Madeira (Médio)", 73.6, 98.6, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(FinishedWoodenChest), "Baús & Caixas", "Baú de Madeira (Grande)", 90.0, 115.0, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(GildedWoodenChest), "Baús & Caixas", "Baú de Madeira Real", 90.0, 115.0, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(WoodenFootLocker), "Baús & Caixas", "Baú de Madeira (Baixo)", 90.0, 115.0, typeof(Board), 1015101, 30, 1044351);
            index = AddCraft(typeof(WoodenCoffin), "Baús & Caixas", "Caixão Simples", 85.0, 90.0, typeof(Board), 1015101, 40, 1044351);
            AddSkill(index, SkillName.Forensics, 60.0, 70.0);
            index = AddCraft(typeof(WoodenCasket), "Baús & Caixas", "Caixão Elegante", 90.0, 95.0, typeof(Board), 1015101, 40, 1044351);
            AddSkill(index, SkillName.Forensics, 70.0, 80.0);

            // 3
            /*AddCraft(typeof(AdventurerCrate), "Caixas", "Caixa de Aventureiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(AlchemyCrate), "Caixas", "Caixa de Alquimista", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(ArmsCrate), "Caixas", "Caixa de Armeiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(BakerCrate), "Caixas", "Caixa de Padeiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(BeekeeperCrate), "Caixas", "Caixa de Apicultor", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(BlacksmithCrate), "Caixas", "Caixa de Ferreiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(BowyerCrate), "Caixas", "Caixa de Arqueiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(ButcherCrate), "Caixas", "Caixa de Açougueiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(CarpenterCrate), "Caixas", "Caixa de Carpinteiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(FletcherCrate), "Caixas", "Caixa de Flecheiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(HealerCrate), "Caixas", "Caixa de Curandeiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(JewelerCrate), "Caixas", "Caixa de Joalheiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(LibrarianCrate), "Caixas", "Caixa de Bibliotecário", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(MusicianCrate), "Caixas", "Caixa de Músico", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(SailorCrate), "Caixas", "Caixa de Marinheiro", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(TailorCrate), "Caixas", "Caixa de Alfaiate", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(TinkerCrate), "Caixas", "Caixa de Inventor", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(WizardryCrate), "Caixas", "Caixa de Mago", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/

            /*AddCraft(typeof(HugeCrate), "Caixas", "huge Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/
            /*AddCraft(typeof(NecromancerCrate), "Caixas", "necromancer Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/
            /*AddCraft(typeof(ProvisionerCrate), "Caixas", "provisioner Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/
            /*AddCraft(typeof(StableCrate), "Caixas", "stable Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(SupplyCrate), "Caixas", "supply Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/
            /*AddCraft(typeof(TavernCrate), "Caixas", "tavern Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/
            /*AddCraft(typeof(TreasureCrate), "Caixas", "treasure Caixa de ", 47.3, 72.3, typeof(Board), 1015101, 20, 1044351);*/

            // 4
            AddCraft(typeof(TallCabinet), "Cômodas & Gaveteiros", "Gabinete Alto", 90.0, 115.0, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(ShortCabinet), "Cômodas & Gaveteiros", "Gabinete Baixo", 90.0, 115.0, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(MapleArmoire), "Cômodas & Gaveteiros", "Cômoda Simples", 90.0, 115.0, typeof(Board), 1015101, 40, 1044351);
            AddCraft(typeof(CherryArmoire), "Cômodas & Gaveteiros", "Cômoda Grande", 90.0, 115.0, typeof(Board), 1015101, 40, 1044351);
            AddCraft(typeof(OrnateWoodenChest), "Cômodas & Gaveteiros", "Cômoda Real", 90.0, 115.0, typeof(Board), 1015101, 30, 1044351);
            AddCraft(typeof(RedArmoire), "Cômodas & Gaveteiros", "Gaveteiro Simples", 90.0, 115.0, typeof(Board), 1015101, 40, 1044351);
            AddCraft(typeof(ElegantArmoire), "Cômodas & Gaveteiros", "Gaveteiro Elegante", 90.0, 115.0, typeof(Board), 1015101, 40, 1044351);
            AddCraft(typeof(PlainWoodenChest), "Cômodas & Gaveteiros", "Gaveteiro Real", 90.0, 115.0, typeof(Board), 1015101, 30, 1044351);

            // 5
            AddCraft(typeof(NewShelfA), "Estantes", "Estante de Bambu (Pequena)", 41.5, 66.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(NewShelfB), "Estantes", "Estante de Bambu (Grande)", 41.5, 66.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(NewShelfE), "Estantes", "Estante Rústica (Pequena)", 41.5, 66.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(NewShelfF), "Estantes", "Estante Rústica (Grande)", 41.5, 66.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(NewShelfD), "Estantes", "Estante Maciça (Pequena)", 41.5, 66.5, typeof(Board), 1015101, 35, 1044351);
            AddCraft(typeof(EmptyBookcase), "Estantes", "Estante Maciça (Grande)", 41.5, 66.5, typeof(Board), 1015101, 25, 1044351);

            // 6 - Furniture
            AddCraft(typeof(FootStool), "Mesas & Assentos", "Banquinho", 11.0, 28.0, typeof(Board), 1015101, 9, 1044351);
            AddCraft(typeof(Stool), "Mesas & Assentos", "Banco", 27.0, 36.0, typeof(Board), 1015101, 9, 1044351);
            AddCraft(typeof(WoodenBench), "Mesas & Assentos", "Banco Rústico", 35.6, 47.6, typeof(Board), 1015101, 17, 1044351);
            AddCraft(typeof(BambooChair), "Mesas & Assentos", "Cadeira de Palha", 46.0, 50.0, typeof(Board), 1015101, 13, 1044351);
            AddCraft(typeof(WoodenChair), "Mesas & Assentos", "Cadeira Simples", 49.0, 56.0, typeof(Board), 1015101, 13, 1044351);
            AddCraft(typeof(WoodenChairCushion), "Mesas & Assentos", "Cadeira Normal", 52.1, 59.1, typeof(Board), 1015101, 13, 1044351);
            AddCraft(typeof(FancyWoodenChairCushion), "Mesas & Assentos", "Cadeira Elegante", 58.1, 67.1, typeof(Board), 1015101, 15, 1044351);
            AddCraft(typeof(Throne), "Mesas & Assentos", "Trono Simples", 66.6, 71.6, typeof(Board), 1015101, 19, 1044351);
            AddCraft(typeof(WoodenThrone), "Mesas & Assentos", "Trono Elegante", 70.6, 77.6, typeof(Board), 1015101, 17, 1044351);
            index = AddCraft(typeof(OrnateElvenChair), "Mesas & Assentos", "Trono Élfico", 77.0, 82.0, typeof(Board), 1044041, 30, 1044351);

            AddCraft(typeof(Nightstand), "Mesas & Assentos", "Mesa de Cabeceira", 42.1, 57.1, typeof(Board), 1015101, 17, 1044351);
            AddCraft(typeof(PlainLargeTable), "Mesas & Assentos", "Mesa Larga", 55.1, 67.1, typeof(Board), 1015101, 23, 1044351);
            AddCraft(typeof(WritingTable), "Mesas & Assentos", "Mesa de Estudo", 63.1, 68.1, typeof(Board), 1015101, 20, 1044351);
            AddCraft(typeof(YewWoodTable), "Mesas & Assentos", "Mesa Simples", 64.1, 73.1, typeof(Board), 1015101, 23, 1044351);
            AddCraft(typeof(LargeTable), "Mesas & Assentos", "Mesa Grande", 69.2, 79.2, typeof(Board), 1015101, 27, 1044351);
            index = AddCraft(typeof(PlainLowTable), "Mesas & Assentos", "Mesa de Centro Simples", 79.0, 85.0, typeof(Board), 1044041, 35, 1044351);
            index = AddCraft(typeof(ElegantLowTable), "Mesas & Assentos", "Mesa de Centro Elegante", 80.0, 88.0, typeof(Board), 1044041, 40, 1044351);

            // Instruments
            AddCraft(typeof(ShortMusicStand), "Instrumentos Musicais", "Estante de Partitura (Curta)", 30.0, 35.0, typeof(Board), 1015101, 15, 1044351);
            AddCraft(typeof(TallMusicStand), "Instrumentos Musicais", "Estante de Partitura (Alta)", 35.0, 40.0, typeof(Board), 1015101, 22, 1044351);

            index = AddCraft(typeof(BambooFlute), "Instrumentos Musicais", "Flauta", 70.0, 75.0, typeof(Board), 1015101, 15, 1044351);
            AddSkill(index, SkillName.Musicianship, 30.0, 45.0);

            index = AddCraft(typeof(Drums), "Instrumentos Musicais", "Tambor", 75.0, 80.0, typeof(Board), 1015101, 23, 1044351);
            AddSkill(index, SkillName.Musicianship, 45.0, 50.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(Tambourine), "Instrumentos Musicais", "Pandeiro", 78.0, 83.0, typeof(Board), 1015101, 16, 1044351);
            AddSkill(index, SkillName.Musicianship, 50.0, 55.0);
            AddRes(index, typeof(Leather), 1044462, 5, 1044463);
            AddRes(index, typeof(IronIngot), 1044036, 4, 1044037);

            index = AddCraft(typeof(TambourineTassel), "Instrumentos Musicais", "Pandeiro com fita", 80.0, 85.0, typeof(Board), 1015101, 16, 1044351);
            AddSkill(index, SkillName.Musicianship, 55.0, 60.0);
            AddRes(index, typeof(Leather), 1044462, 5, 1044463);
            AddRes(index, typeof(IronIngot), 1044036, 4, 1044037);
            AddRes(index, typeof(Cloth), 1044286, 4, 1044287);

            index = AddCraft(typeof(LapHarp), "Instrumentos Musicais", "Harpa (Pequena)", 84.0, 89.1, typeof(Board), 1015101, 22, 1044351);
            AddSkill(index, SkillName.Musicianship, 60.0, 65.0);
            AddRes(index, typeof(IronIngot), 1044036, 5, 1044037);

            index = AddCraft(typeof(Harp), "Instrumentos Musicais", "Harpa (Grande)", 88.9, 93.6, typeof(Board), 1015101, 30, 1044351);
            AddSkill(index, SkillName.Musicianship, 65.0, 70.0);
            AddRes(index, typeof(IronIngot), 1044036, 10, 1044037);

            index = AddCraft(typeof(Lute), "Instrumentos Musicais", "Alaúde", 92.4, 96.3, typeof(Board), 1015101, 24, 1044351);
            AddSkill(index, SkillName.Musicianship, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 7, 1044037);

            index = AddCraft(typeof(Pipes), "Instrumentos Musicais", "Gaita", 95.8, 98.0, typeof(Board), 1015101, 28, 1044351);
            AddSkill(index, SkillName.Musicianship, 80.0, 90.0);
            AddRes(index, typeof(IronIngot), 1044036, 6, 1044037);

            index = AddCraft(typeof(Fiddle), "Instrumentos Musicais", "Violino", 97.7, 101.1, typeof(Board), 1015101, 31, 1044351);
            AddSkill(index, SkillName.Musicianship, 90.0, 100.0);
            AddRes(index, typeof(IronIngot), 1044036, 8, 1044037);
            AddRes(index, typeof(Shaft), 1027125, 2, 1044561);

            // Staves and Shields

            AddCraft(typeof(ShepherdsCrook), 1044295, "cajado de pastor", 50.0, 57.9, typeof(Board), 1015101, 7, 1044351);
            index = AddCraft(typeof(WildStaff), 1044295, "cajado druida", 56.9, 60.9, typeof(Board), 1015101, 8, 1044351);
            AddRes(index, typeof(Feather), 1044562, 4, 1044563);

            AddCraft(typeof(QuarterStaff), 1044295, "bastão", 60.6, 65.6, typeof(Board), 1015101, 8, 1044351);
            AddCraft(typeof(GnarledStaff), 1044295, "bastão retorciddo", 64.9, 70.9, typeof(Board), 1015101, 7, 1044351);
            AddCraft(typeof(WoodenShield), 1044295, "escudo de madeira", 70.6, 75.6, typeof(Board), 1015101, 12, 1044351);

            index = AddCraft(typeof(Bokuto), 1044295, 1030227, 73.0, 78.5, typeof(Board), 1015101, 9, 1044351);
            AddRes(index, typeof(Cloth), 1044286, 4, 1044287);
            index = AddCraft(typeof(Fukiya), 1044295, 1030229, 78.0, 81.0, typeof(Board), 1015101, 9, 1044351);
            AddRes(index, typeof(IronIngot), 1044036, 2, 1044037);
            index = AddCraft(typeof(Tetsubo), 1044295, 1030225, 80.5, 85.3, typeof(Board), 1015101, 13, 1044351);
            AddRes(index, typeof(IronIngot), 1044036, 5, 1044037);

            // Armor
            index = AddCraft(typeof(WoodenPlateArms), 1044295, "ombreiras de madeira", 66.3, 116.3, typeof(ReaperOil), "óleo ceifador", 2, 1042081);
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 2, 1042081);
            AddRes(index, typeof(Board), 1015101, 18, 1044351);

            index = AddCraft(typeof(WoodenPlateHelm), 1044295, "elmo de madeira", 62.6, 112.6, typeof(ReaperOil), "óleo ceifador", 1, 1042081);
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 1, 1042081);
            AddRes(index, typeof(Board), 1015101, 15, 1044351);

            index = AddCraft(typeof(WoodenPlateGloves), 1044295, "manoplas de madeira", 58.9, 108.9, typeof(ReaperOil), "óleo ceifador", 1, 1042081);
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 1, 1042081);
            AddRes(index, typeof(Board), 1015101, 12, 1044351);

            index = AddCraft(typeof(WoodenPlateGorget), 1044295, "gorgel de madeira", 56.4, 106.4, typeof(ReaperOil), "óleo ceifador", 1, 1042081);
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 1, 1042081);
            AddRes(index, typeof(Board), 1015101, 10, 1044351);

            index = AddCraft(typeof(WoodenPlateLegs), 1044295, "calça de madeira", 68.8, 118.8, typeof(ReaperOil), "óleo ceifador", 3, 1042081);
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 3, 1042081);
            AddRes(index, typeof(Board), 1015101, 20, 1044351);

            index = AddCraft(typeof(WoodenPlateChest), 1044295, "peitoral de madeira", 75.0, 125.0, typeof(ReaperOil), "óleo ceifador", 3, 1042081);
            AddRes(index, typeof(MysticalTreeSap), "seiva de árvore mística", 3, 1042081);
            AddRes(index, typeof(Board), 1015101, 25, 1044351);

            

            ////////////////////////////////////////////////////////
            // ADDONS
            ////////////////////////////////////////////////////////
            // Blacksmithy
            index = AddCraft(typeof(SmallForgeDeed), 1044290, 1044330, 73.6, 98.6, typeof(Board), 1015101, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 75, 1044037);

            index = AddCraft(typeof(LargeForgeEastDeed), 1044290, 1044331, 78.9, 103.9, typeof(Board), 1015101, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 80.0, 85.0);
            AddRes(index, typeof(IronIngot), 1044036, 100, 1044037);

            index = AddCraft(typeof(LargeForgeSouthDeed), 1044290, 1044332, 78.9, 103.9, typeof(Board), 1015101, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 80.0, 85.0);
            AddRes(index, typeof(IronIngot), 1044036, 100, 1044037);

            index = AddCraft(typeof(AnvilEastDeed), 1044290, 1044333, 73.6, 98.6, typeof(Board), 1015101, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 150, 1044037);

            index = AddCraft(typeof(AnvilSouthDeed), 1044290, 1044334, 73.6, 98.6, typeof(Board), 1015101, 5, 1044351);
            AddSkill(index, SkillName.Blacksmith, 75.0, 80.0);
            AddRes(index, typeof(IronIngot), 1044036, 150, 1044037);

            // Training
            index = AddCraft(typeof(TrainingDummyEastDeed), 1044290/*1044297*/, 1044335, 68.4, 93.4, typeof(Board), 1015101, 55, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(TrainingDummySouthDeed), 1044290, 1044336, 68.4, 93.4, typeof(Board), 1015101, 55, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(PickpocketDipEastDeed), 1044290, 1044337, 73.6, 98.6, typeof(Board), 1015101, 65, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(PickpocketDipSouthDeed), 1044290, 1044338, 73.6, 98.6, typeof(Board), 1015101, 65, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            // Tailoring
            index = AddCraft(typeof(Dressform), 1044290, 1044339, 63.1, 88.1, typeof(Board), 1015101, 25, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 10, 1044287);

            index = AddCraft(typeof(SpinningwheelEastDeed), 1044290, 1044341, 73.6, 98.6, typeof(Board), 1015101, 75, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            index = AddCraft(typeof(SpinningwheelSouthDeed), 1044290, 1044342, 73.6, 98.6, typeof(Board), 1015101, 75, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            index = AddCraft(typeof(LoomEastDeed), 1044290, 1044343, 84.2, 109.2, typeof(Board), 1015101, 85, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            index = AddCraft(typeof(LoomSouthDeed), 1044290, 1044344, 84.2, 109.2, typeof(Board), 1015101, 85, 1044351);
            AddSkill(index, SkillName.Tailoring, 65.0, 70.0);
            AddRes(index, typeof(Cloth), 1044286, 25, 1044287);

            // Cooking
            index = AddCraft(typeof(StoneOvenEastDeed), 1044290, 1044345, 68.4, 93.4, typeof(Board), 1015101, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);

            index = AddCraft(typeof(StoneOvenSouthDeed), 1044290, 1044346, 68.4, 93.4, typeof(Board), 1015101, 85, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 125, 1044037);

            index = AddCraft(typeof(FlourMillEastDeed), 1044290, 1044347, 94.7, 119.7, typeof(Board), 1015101, 100, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            index = AddCraft(typeof(FlourMillSouthDeed), 1044290, 1044348, 94.7, 119.7, typeof(Board), 1015101, 100, 1044351);
            AddSkill(index, SkillName.Tinkering, 50.0, 55.0);
            AddRes(index, typeof(IronIngot), 1044036, 50, 1044037);

            AddCraft(typeof(WaterTroughEastDeed), 1044290, 1044349, 94.7, 119.7, typeof(Board), 1015101, 150, 1044351);
            AddCraft(typeof(WaterTroughSouthDeed), 1044290, 1044350, 94.7, 119.7, typeof(Board), 1015101, 150, 1044351);

            // GENERAL MISC
            AddCraft(typeof(DartBoardSouthDeed), 1044290, 1044325, 15.7, 40.7, typeof(Board), 1015101, 5, 1044351);
            AddCraft(typeof(DartBoardEastDeed), 1044290, 1044326, 15.7, 40.7, typeof(Board), 1015101, 5, 1044351); 

            // Outros & Variados
            AddCraft(typeof(Kindling), "Outros & Variados", "gravetos", 0.0, 10.0, typeof(Log), 1015101, 1, 1044351);

            index = AddCraft(typeof(Kindling), "Outros & Variados", "lote de gravetos", 9.0, 15.0, typeof(Log), 1015101, 1, 1044351);
            SetUseAllRes(index, true);

            index = AddCraft(typeof(BarkFragment), "Outros & Variados", "casca de árvore", 14.0, 20.0, typeof(Board), 1015101, 2, 1044465);
            SetUseAllRes(index, true);

            AddCraft(typeof(BarrelStaves), "Outros & Variados", 1027857, 18.0, 25.0, typeof(Board), 1015101, 5, 1044351);
            AddCraft(typeof(BarrelLid), "Outros & Variados", 1027608, 23.0, 30.2, typeof(Board), 1015101, 4, 1044351);

            AddCraft(typeof(MixingSpoon), "Outros & Variados", "misturador de caldeirão", 30.0, 40.0, typeof(Board), 1015101, 5, 1044351);

            index = AddCraft(typeof(Keg), "Outros & Variados", 1023711, 75.0, 80.8, typeof(BarrelStaves), 1044288, 4, 1044253);
            AddRes(index, typeof(BarrelHoops), 1044289, 1, 1044253);
            AddRes(index, typeof(BarrelLid), 1044251, 2, 1044253);
            AddRes(index, typeof(BarrelTap), 1044252, 1, 1044253);

            index = AddCraft(typeof(AlchemyTub), "Outros & Variados", "caldeirão de alquimia", 87.8, 102.8, typeof(BarrelStaves), 1044288, 4, 1044253);
            AddRes(index, typeof(BarrelHoops), 1044289, 1, 1044253);
            AddRes(index, typeof(BarrelLid), 1044251, 1, 1044253);

            index = AddCraft(typeof(FishingPole), "Outros & Variados", 1023519, 50.0, 60.4, typeof(Board), 1015101, 5, 1044351);
            AddSkill(index, SkillName.Tailoring, 40.0, 45.0);
            AddRes(index, typeof(Cloth), 1044286, 5, 1044287);

            index = AddCraft(typeof(ShojiScreen), "Outros & Variados", 1029423, 60.0, 65.0, typeof(Board), 1015101, 75, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            index = AddCraft(typeof(BambooScreen), "Outros & Variados", 1029428, 65.0, 70.0, typeof(Board), 1015101, 75, 1044351);
            AddSkill(index, SkillName.Tailoring, 50.0, 55.0);
            AddRes(index, typeof(Cloth), 1044286, 60, 1044287);

            AddCraft(typeof(Easle), "Outros & Variados", 1044317, 70.0, 75.0, typeof(Board), 1015101, 20, 1044351);

            index = AddCraft(typeof(WhiteHangingLantern), "Outros & Variados", 1029416, 60.0, 65.0, typeof(Board), 1015101, 6, 1044351);
            AddRes(index, typeof(BlankScroll), 1044377, 10, 1044378);

            index = AddCraft(typeof(RedHangingLantern), "Outros & Variados", 1029412, 65.0, 70.0, typeof(Board), 1015101, 6, 1044351);
            AddRes(index, typeof(BlankScroll), 1044377, 10, 1044378);

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
            AddSubRes(typeof(HickoryBoard), 1095383, 100.0, 1015101, 1072652);

            /*AddSubRes( typeof( MahoganyBoard ), 1095384, 90.0, 1015101, 1072652 );
			AddSubRes( typeof( OakBoard ), 1095385, 95.0, 1015101, 1072652 );
			AddSubRes( typeof( PineBoard ), 1095386, 100.0, 1015101, 1072652 );*/

            /*AddSubRes( typeof( WalnutBoard ), 1095388, 100.0, 1015101, 1072652 );
			AddSubRes( typeof( DriftwoodBoard ), 1095409, 105.0, 1015101, 1072652 );
			AddSubRes( typeof( GhostBoard ), 1095511, 110.0, 1015101, 1072652 );
			AddSubRes( typeof( PetrifiedBoard ), 1095532, 115.0, 1015101, 1072652 );*/

        }
	}
}
