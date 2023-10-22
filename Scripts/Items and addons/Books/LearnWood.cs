using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Items
{
	public class LearnWoodBook : Item
	{
		[Constructable]
		public LearnWoodBook( ) : base( 0x4C5E )
		{
			Weight = 1.0;
			Name = "Pergaminho do Conhecimento";
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Lenharia e Tipos de madeiras" );
		}

		public class LearnWoodBookGump : Gump
		{
			public LearnWoodBookGump( Mobile from ): base( 25, 25 )
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

				AddItem(717, 121, 7980);
				AddItem(409, 129, 4148);
				AddItem(506, 124, 4142);
				AddItem(602, 126, 4138);
				AddItem(308, 114, 3670);
				AddItem(808, 98, 1928);
				AddItem(831, 121, 1928);
				AddItem(807, 83, 4528);

				AddHtml( 170, 70, 600, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>GUIA SOBRE LENHARIA E OS TIPOS DE MADEIRA</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

				int i = 135;
				int o = 32;

				AddItem(100, i, 7137, 0);
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Comum</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "ash", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Carvalho Cinza</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
                AddItem(100, i, 7137, MaterialInfo.GetMaterialColor("ebony", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Ébano</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;
                AddItem(100, i, 7137, MaterialInfo.GetMaterialColor("golden oak", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Ipê-Amarelo</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;
                AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "cherry", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Cerejeira</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
                AddItem(100, i, 7137, MaterialInfo.GetMaterialColor("rosewood", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Pau-Brasil</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;
                AddItem(100, i, 7137, MaterialInfo.GetMaterialColor("elven", "", 0));
                AddHtml(150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Élfica</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i = i + o;
                AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "hickory", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Nogueira Branca</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				/*AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "mahogany", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Mahogany</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "oak", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Oak</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "pine", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Pine</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "ghostwood", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Ghostwood</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;*/
				/*AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "walnut", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Walnut</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "petrified", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Petrified</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;
				AddItem(100, i, 7137, MaterialInfo.GetMaterialColor( "driftwood", "", 0 ));
				AddHtml( 150, i, 137, 21, @"<BODY><BASEFONT Color=#FCFF00><BIG>Driftwood</BIG></BASEFONT></BODY>", (bool)false, (bool)false); i=i+o;*/

				AddHtml( 303, 198, 573, 466, @"<BODY><BASEFONT Color=#FAFAFA><BIG><br>A lenharia é uma tarefa mais antiga que a mineração. Você só precisa de um machado para transformar uma árvore em pedaços ou toras de madeiras. Embora normalmente você obtenha madeira comum, eventualmente, adquirirá a habilidade suficiente para cortar outros tipos de madeira. Existem variados tipos de madeira neste mundo e com elas você pode ser capaz de trabalhar de várias formas, seja com carpintaria ou construção de arcos e flechas.<br><br>Os vários tipos de madeira estão listados aqui por ordem de qualidade e raridade. Entretanto, cada tipo de madeira possui uma qualidade ou propriedade para seus diversos usos, além de possuir uma cor distinta que será mantida na construção do item. <br><br>Para fazer coisas com madeira, você precisa transformar as toras em tábuas. Para fazer isso, clique duas vezes nas toras e selecione uma serraria (são comumente encontrados em carpintarias espalhadas pelas cidades ou você pode ter uma na sua casa). Após obter as tábuas de madeira, você pode começar a trabalhar com uma ferramenta de carpintaria(serra ou martelo) ou de construção de arco/flecha.<br><br>É possível reduzir o peso das toras de madeiras ao se utilizar uma bolsa mágica de madeiras e na habilidade de alquimia existe o processo de transmutação de algumas madeiras e também é possível encontrar alguns raríssimos óleos que causam mutação em itens de madeira.</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) ) 
			{
				e.SendMessage( "Para ler, isso precisa estar na sua mochila." );
			}
			else
			{
				e.CloseGump( typeof( LearnWoodBookGump ) );
				e.SendGump( new LearnWoodBookGump( e ) );
				e.PlaySound( 0x249 );
			}
		}

		public LearnWoodBook(Serial serial) : base(serial)
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
            ItemID = 0x4C5E;
		}
	}
}