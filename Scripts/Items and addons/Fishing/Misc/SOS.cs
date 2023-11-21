using System;
using Server.Network;
using Server.Gumps;
using Server.Misc;

namespace Server.Items
{
	public class SOS : Item
	{
		public override int LabelNumber
		{
			get
			{
				if ( IsAncient )
					return 1063450; // an ancient SOS

				return 1041081; // a waterstained SOS
			}
		}

		private int m_Level;
		private Map m_TargetMap;
		private Point3D m_TargetLocation;
		public string MapWorld;
		public string ShipStory;
		public string ShipName;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsAncient { get{ return ( m_Level >= 4 ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Level
		{
			get{ return m_Level; }
			set
			{
				m_Level = Math.Max( 1, Math.Min( value, 4 ) );
				UpdateHue();
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map TargetMap { get{ return m_TargetMap; } set{ m_TargetMap = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D TargetLocation { get{ return m_TargetLocation; } set{ m_TargetLocation = value; } }

		[CommandProperty(AccessLevel.Owner)]
		public string Map_World { get { return MapWorld; } set { MapWorld = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Ship_Story { get { return ShipStory; } set { ShipStory = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Ship_Name { get { return ShipName; } set { ShipName = value; InvalidateProperties(); } }

		public void UpdateHue()
		{
			if ( IsAncient )
				Hue = Utility.RandomList( 149, 150, 249 );
			else
				Hue = 0;
		}

		[Constructable]
		public SOS( string world, int level ) : base( 0x14ED )
		{
			if ( level < 1 ){ level = MessageInABottle.GetRandomLevel(); }

			if ( world == "the Town of Skara Brae" ){ world = "the Land of Sosaria"; } // NO SOSs IN SKARA BRAE
			else if ( world == "the Moon of Luna" ){ world = "the Land of Sosaria"; } // NO SOSs ON THE MOON
			else if ( world == "the Underworld" ){ world = "the Land of Sosaria"; } // NO SOSs IN THE UNDERWORLD

			Name = "Um Pedido de Socorro (SOS)";
			Weight = 1.0;

			Point3D loc = Worlds.GetRandomLocation( world, "sea" );
			Map map = Worlds.GetMyDefaultMap( world );

			MapWorld = world;
			m_Level = level;
			m_TargetMap = map;

			m_TargetLocation = loc;

			UpdateHue();


			ShipName = RandomThings.GetRandomShipName( "", 0 );


			string Beast = "uma besta marinha";
			switch ( Utility.Random( 6 ) )
			{
				case 0: Beast = "um monstro gigante"; break;
				case 1: Beast = "um monstro marinho"; break;
				case 2: Beast = "um leviatã"; break;
				case 3: Beast = "uma coisa enorme"; break;
				case 4: Beast = "uma besta marinha"; break;
				case 5: Beast = "uma criatura enorme"; break;
			}

			if ( IsAncient ){ ShipStory = "Este pergaminho é muito antigo e quase se desfaz na sua mão. Você sabe que quem escreveu isso está morto possivelmente há séculos... "; }

			switch ( Utility.Random( 5 ) )
			{
				case 0: ShipStory = ShipStory + "Estavamos navegando em " + MapWorld + " quando " + Beast + " surgiu das profundezas do oceano e atacou nosso navio! O casco sofreu muitos danos e " + ShipName + " está afundando lentamente nas profundezas do mar! Quem encontrar isso, mande um navio para as coordenadas abaixo! Rápido! Não tenho certeza de quanto tempo vamos durar aqui!"; break;
				case 1: ShipStory = ShipStory + "Se você nunca viu " + Beast + " antes, considere-se com sorte. Houve poucos avisos antes de atingirem nosso navio, " + ShipName + ", enquanto navegavam em " + MapWorld + ". Achavamos que tinhamos atingido um recife, mas erramos. Somente eu e " + QuestCharacters.ParchmentWriter() + " conseguimos sobreviver ao ataque da fera. Agora estamos sentados aqui, em alguma ilha. As últimas coordenadas de que me lembro são onde a sua nave afundou. Envie um resgate e haverá ouro para pagamento se você fizer isso."; break;
				case 2: ShipStory = ShipStory + "Estou escrevendo isso com minhas últimas forças a bordo '" + ShipName + "'. " + QuestCharacters.ParchmentWriter() + " o Pirata veio até nós durante a noite, longe da terra em " + MapWorld + ". Não tivemos a menor chance. Tentamos fugir, mas o vento estava contra nós, com certeza. Ele incendiou nosso navio e fugiu para longe. Agora afundamos lentamente no oceano. Se você encontrar isso, escrevi nossas coordenadas abaixo. Você ainda pode chegar aqui a tempo de salvar os outros. Se puder, conte " + QuestCharacters.ParchmentWriter() + " minha história para que eles nunca vivam se perguntando sobre meu destino. Eles moram em algum lugar em " + RandomThings.GetRandomCity() + "."; break;
				case 3: ShipStory = ShipStory + "'" + ShipName + "' estar afundando longe da terra. O que pensávamos ser um navio mercante era na verdade um navio de guerra disfarçado. Eles estão caçando piratas em alto mar em " + MapWorld + "...e hoje nossa sorte acabou. Seus canhões rasgaram nossas velas e abriram buracos em nosso casco. Eles mataram a maior parte da tripulação, onde apenas " + Utility.RandomMinMax(3, 16) + " de nós sobreviveu. Eles já se foram, mas os tubarões começaram a cercar os destroços. Acabei de ver " + QuestCharacters.ParchmentWriter() + " sendo puxado para baixo das ondas, o sangue jorrando de baixo. Estou no maior pedaço de destroços e só posso esperar sobreviver até você chegar aqui."; break;
				case 4: ShipStory = ShipStory + "Eu sabia que " + QuestCharacters.ParchmentWriter() + " não era bom em ser capitão de '" + ShipName + "'. Agora este provavelmente será o nosso fim aqui em " + MapWorld + ". Estamos sob ataque de " + Beast + " e não temos chance de chegar a " + RandomThings.GetRandomCity() + " agora. Temo nunca mais ver minha esposa. Se você encontrar esta nota, por favor, encontre-nos antes que afundemos. Tenho um artefato antigo que poderia trocar pela sua ajuda."; break;
			}
		}

		public SOS( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (IsAncient)
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Pergaminho Ancestral", "#8be4fc"));

            list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Para encontrar o naufrágio basta navegar para a localização e usar a vara de pescar.", "#ffe066"));
        }

        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 4 ); // version
			writer.Write( m_Level );
			writer.Write( m_TargetMap );
			writer.Write( m_TargetLocation );
            writer.Write( MapWorld );
            writer.Write( ShipName );
            writer.Write( ShipStory );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Level = reader.ReadInt();
			m_TargetMap = reader.ReadMap();
			m_TargetLocation = reader.ReadPoint3D();
            MapWorld = reader.ReadString();
            ShipName = reader.ReadString();
            ShipStory = reader.ReadString();
			//ItemID = 0x12AD;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.CloseGump( typeof( MessageGump ) );
				from.SendGump( new MessageGump( m_TargetMap, m_TargetLocation, MapWorld, ShipStory ) );
				from.PlaySound( 0x249 );
			}
			else
			{
                from.SendMessage(55, "O item precisa estar em sua mochila para que você possa ler");
                //from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
            }
		}

		private class MessageGump : Gump
		{
			public MessageGump( Map map, Point3D loc, string world, string story ) : base( 150, 50 )
			{
				int xLong = 0, yLat = 0;
				int xMins = 0, yMins = 0;
				bool xEast = false, ySouth = false;
				string fmt;

				if ( Sextant.Format( loc, map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
					fmt = String.Format( "{0}°{1}'{2}, {3}°{4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
				else
					fmt = "?????";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(46, 26, 1247);
				AddHtml( 102, 58, 284, 202, @"<BODY><BASEFONT Color=#111111><BIG>" + story + "</BIG></BASEFONT></BODY>", (bool)false, (bool)true);
				AddHtml( 102, 264, 280, 22, @"<BODY><BASEFONT Color=#000000><BIG><b>" + fmt + "</b></BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				from.PlaySound( 0x249 );
			}
		}
	}
}