using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Items
{
	public class LearnMetalBook : Item
	{
		[Constructable]
		public LearnMetalBook( ) : base( 0x4C5B )
		{
			Weight = 1.0;
			Name = "Pergaminho do Conhecimento";
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Guia sobre Mineração, Minérios & Metais" );
		}

		public class LearnMetalGump : Gump
		{
			public LearnMetalGump( Mobile from ): base( 25, 25 )
			{
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 430, 155);
				AddImage(300, 430, 155);
				AddImage(600, 430, 155);
				AddImage(0, 0, 155);
				AddImage(300, 0, 155);
				AddImage(0, 300, 155);
				AddImage(300, 300, 155);
				AddImage(600, 0, 155);
				AddImage(600, 300, 155);

				AddImage(2, 2, 129);
				AddImage(300, 2, 129);
				AddImage(598, 2, 129);
				AddImage(2, 298, 129);
				AddImage(302, 298, 129);
				AddImage(598, 298, 129);

				AddImage(6, 7, 133);
				AddImage(230, 46, 132);
				AddImage(530, 46, 132);
				AddImage(680, 7, 134);
				AddImage(598, 428, 129);
				AddImage(298, 428, 129);
				AddImage(2, 428, 129);
				AddImage(5, 484, 142);
				AddImage(329, 693, 140);
				AddImage(573, 693, 140);
				AddImage(856, 695, 143);

				AddItem(839, 111, 4017);
				AddItem(351, 112, 3898);
				AddItem(572, 126, 5092);
				AddItem(655, 123, 7865);
				AddItem(308, 114, 6585);
				AddItem(730, 117, 4015);
				AddItem(502, 126, 3718);

				AddHtml( 170, 70, 600, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>GUIA SOBRE MINERAÇÃO E OS TIPOS DE MINÉRIOS & METAIS</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

				int i = 115;
				int o = 32;

				AddItem(100, i, 7153, 0);
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Ferro</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "dull copper", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Cobre Rústico</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;

                AddItem(100, i, 7153, MaterialInfo.GetMaterialColor("copper", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Cobre</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;

                AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "shadow iron", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Ferro Negro</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "bronze", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Bronze</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;

                AddItem(100, i, 7153, MaterialInfo.GetMaterialColor("platinum", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Platinum</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;

                AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "gold", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Dourado</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;

				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "agapite", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Agapite</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;

				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "verite", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Verite</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;

				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "valorite", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Valorite</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;

                AddItem(100, i, 7153, MaterialInfo.GetMaterialColor("rosenium", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Rosenium</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;

                AddItem(100, i, 7153, MaterialInfo.GetMaterialColor("titanium", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Titanium</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;

                /*AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "nepturite", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Nepturite</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "obsidian", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Obsidian</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "steel", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Steel</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "brass", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Brass</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "mithril", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Mithril</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "xormite", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Xormite</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7153, MaterialInfo.GetMaterialColor( "dwarven", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Dwarven</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;*/

				AddHtml( 303, 198, 573, 466, @"<BODY><BASEFONT Color=#FBFBFB><BIG>A mineração é a habilidade necessária para encontrar minério dentro de cavernas e montanhas. Com o minério extraído, é possível fundi-lo e criar os lingotes e, em seguida, com a habilidade de metalurgia e funilaria pode-se criar armas, armaduras e ferramentas.<br/><br/>Você simplesmente precisa uma picareta, marreta ou uma pá, clicar duas vezes nela e, em seguida, mirar em um lado da montanha ou no chão de cavernas. Embora normalmente se obtenha minério regular (ferro), você acabará ficando habilidoso o suficiente para desenterrar outros tipos de minério mais raros.<br/>Os muitos tipos de metal estão listados aqui, começando do mais simples para o de maior qualidade. Um escudo de valorite será um escudo muito melhor do que um feito de cobre, por exemplo.<br/><br/>Entretanto, cada metal possui propriedades distintas e que podem, se bem utilizados, dar vantagens e certos aspectos como ajudar a criar e ativar atributos mágicos em armas e itens especiais.<br/>Seja qual for a cor do metal que você usar, a arma, ferramenta ou armadura manterá a cor do metal.<br/><br/>Para fazer coisas a partir do minério, você precisa transformar o minério em lingotes. Para fazer isso, clique duas vezes nos lingotes e direcione uma forja. Estas forjas são comumente encontradas em ferrarias. Então você pode começar a criar com um martelo de ferreiro, ou mexer com ferramentas de funilaria.<br/>Tenha em mente que, para fabricar itens, você precisará estar perto de uma forja e bigorna.</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) ) 
			{
				e.SendMessage("Para ler, isso precisa estar na sua mochila.");
			}
			else
			{
				e.CloseGump( typeof( LearnMetalGump ) );
				e.SendGump( new LearnMetalGump( e ) );
				e.PlaySound( 0x249 );
			}
		}

		public LearnMetalBook(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Name = "Pergaminho do Conhecimento";
			ItemID = 0x4C5B;
		}
	}
}