using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server 
{ 
	public class AutoRessurection
	{ 
		public static void Initialize()
		{
			EventSink.PlayerDeath += new PlayerDeathEventHandler(e => ResurrectNowGump.TryShowAutoResurrectGump(e.Mobile as PlayerMobile));
			EventSink.Login += new LoginEventHandler(e => ResurrectNowGump.TryShowAutoResurrectGump(e.Mobile as PlayerMobile));
        }
	}
}

namespace Server.Gumps
{
    public class ResurrectNowGump : Gump
    {
        private enum ButtonType
        {
			Close = 0,
            Accept = 1,
            Cancel = 2,
            CancelAndSuppress = 3
        }

        public ResurrectNowGump( Mobile from ): base( 50, 20 )
		{
			if ( !(from is PlayerMobile) || from.Alive) return;

			double penalty;
			
			if (from.Karma >= 0)
				penalty = ( (100 - ( ((double)AetherGlobe.BalanceLevel / 100000.0) * ( ((double)from.Karma / 15000) ) )  ) / 100 ) ;
			else 
				penalty = ( (100 - (((double)(100000-AetherGlobe.BalanceLevel) / 100000.0) * ( ((double)Math.Abs(from.Karma) / 15000) ) ) ) / 100 ) ;
				
			if (penalty >= 0.999)
				penalty = 0.999;

            int HealCost = GetPlayerInfo.GetResurrectCost( from );
			int BankGold = Banker.GetBalance( from );
			
			string sText;

            string c1 = String.Format("{0:0.0}", ((1 - penalty)* 300) );
			string c2 = "10";

			string f1 = "Você deseja implorar aos deuses por sua vida de volta? Se fizer isso, você sofrerá uma perda de " + c2 + "% em sua fama e karma.";
            string f2 = "Você tem ouro suficiente no banco para oferecer o tributo da ressurreição, então talvez você queira encontrar um santuário ou curandeiro em vez de sofrer essas penalidades.";
			string f3 = "Você não pode pagar o tributo da ressurreição devido à falta de ouro no banco, então talvez você queira fazer isso.";
			string f4 = "Você também perderá " + c1 + "% de suas estatísticas e todas as habilidades.";
            if ( ( from.RawDex + from.RawInt + from.RawStr ) > 125 )
			{
				if ( !((PlayerMobile)from).Avatar )
				{
					c2 = "30";
					if (BankGold >= HealCost)
						sText = f1 + "\n" + f2;
					else
						sText = f1 + " " + f3; 
				}
				else
				{
					if ( BankGold >= HealCost )
                        sText = f1 + " " + f4 + "\n" + f2;
					else
                        sText = f1 + " " + f4 + " " + f3;
				}
			}
			else 
			{
				if ( !(((PlayerMobile)from).Avatar) )
				{
					c2 = "15";
                    if (BankGold >= HealCost)
                        sText = f1 + " " + f2;
                    else
                        sText = f1 + " " + f3;
                }

				else 
				{
                    if (BankGold >= HealCost)
                        sText = f1 + " " + f4 + " " + f2;
                    else
                        sText = f1 + " " + f4 + " " + f3;
                }
			}

			string sGrave = "VOCÊ MORREU!";
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0:	sGrave = "VOCÊ MORREU!";break;
				case 1:	sGrave = "VOCÊ PERECEU!";break;
				case 2:	sGrave = "VOCÊ CONHECEU SEU FIM!";break;
				case 3:	sGrave = "SUA VIDA ACABOU!";break;
			}

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 154);
			AddImage(300, 201, 154);
			AddImage(0, 201, 154);
			AddImage(300, 0, 154);
			AddImage(298, 199, 129);
			AddImage(2, 199, 129);
			AddImage(298, 2, 129);
			AddImage(2, 2, 129);
			AddImage(7, 6, 145);
			AddImage(8, 257, 142);
			AddImage(253, 285, 144);
			AddImage(171, 47, 132);
			AddImage(379, 8, 134);
			AddImage(167, 7, 156);
			AddImage(209, 11, 156);
			AddImage(189, 10, 156);
			AddImage(170, 44, 159);

			AddItem(173, 64, 4455);
			AddItem(186, 85, 3810);
			AddItem(209, 102, 3808);

			int firstColumn = 100;
			int secondColumn = 307;
			int buttonLabelOffset = 30;

			double bankGoldKK = BankGold;
            bool useKNotation = false;
            bool useKKNotation = false;
            if (bankGoldKK > 1000)
            {
                if (bankGoldKK > 1000000)
                {
                    useKKNotation = true;
                    bankGoldKK /= (1000 * 1000); // kks
                }
                else
                {
                    useKNotation = true;
                    bankGoldKK /= 1000;
                }

				bankGoldKK = Math.Round(bankGoldKK, 2);
            }

            int y = 365;
            AddButton(firstColumn, y, 4023, 4024, (int)ButtonType.Accept, GumpButtonType.Reply, 0);
            AddHtml(firstColumn + buttonLabelOffset, y + 2, 477, 22, @"<BODY><BASEFONT Color=#5eff00><BIG> Ressuscite-me </BIG></BASEFONT></BODY>", false, false);

            AddButton(secondColumn, y, 4017, 4018, (int)ButtonType.Cancel, GumpButtonType.Reply, 0);
            AddHtml(secondColumn + buttonLabelOffset, y + 2, 477, 22, @"<BODY><BASEFONT Color=#fff700><BIG> Talvez mais tarde </BIG></BASEFONT></BODY>", false, false);

            y += 30;
            AddButton(secondColumn, y, 4020, 4021, (int)ButtonType.CancelAndSuppress, GumpButtonType.Reply, 0);
            AddHtml(secondColumn + buttonLabelOffset, y + 2, 477, 22, @"<BODY><BASEFONT Color=#FF0000><BIG> Não e Pare de perguntar </BIG></BASEFONT></BODY>", false, false);

            AddHtml( firstColumn, 291, 190, 22, @"<BODY><BASEFONT Color=#FCFF00><BIG>Valor do Tributo</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( secondColumn, 291, 196, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + String.Format("{0} Moeda(s) de Ouro", HealCost ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

			AddHtml( firstColumn, 315, 190, 22, @"<BODY><BASEFONT Color=#FCFF00><BIG>Dinheiro no banco</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			if (useKNotation)
                AddHtml(secondColumn, 315, 196, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + bankGoldKK.ToString() + "k Moeda(s) de Ouro</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
            else if (useKKNotation)
                AddHtml(secondColumn, 315, 196, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + bankGoldKK.ToString() + "kk Moeda(s) de Ouro</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
            else
                AddHtml( secondColumn, 315, 156, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + Banker.GetBalance( from ).ToString() + " Moeda(s) de Ouro</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

			AddHtml( 267, 95, 306, 22, @"<BODY><BASEFONT Color=#fff700><BIG><CENTER>" + sGrave + "</CENTER></BIG></BASEFONT></BODY>", (bool)false, (bool)false);

			AddHtml( firstColumn, 155, 477, 130, @"<BODY><BASEFONT Color=#ffffff><BIG>" + sText + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			PlayerMobile from = state.Mobile as PlayerMobile;
			if (from == null) return;

			from.CloseGump( typeof( ResurrectNowGump ) );

			ButtonType button = (ButtonType)info.ButtonID;

            switch (button)
            {
                case ButtonType.Accept:
					if (from.Alive) return;

                    from.PlaySound(0x214);
                    from.FixedEffect(0x376A, 10, 16);

                    from.Resurrect();

                    Server.Misc.Death.Penalty(from, true, false);

                    from.Hits = from.HitsMax;
                    from.Stam = from.StamMax;
                    from.Mana = from.ManaMax;
                    from.Hidden = true;
                    from.LastAutoRes = DateTime.UtcNow;
                    break;

                case ButtonType.Cancel:
                case ButtonType.Close:
                case ButtonType.CancelAndSuppress:
				default:
					from.SendMessage(55, "Você decide permanecer no reino espiritual.");
					if (button == ButtonType.CancelAndSuppress) return;

					TryShowAutoResurrectGump(from);
                    break;

			}
        }

        public static void TryShowAutoResurrectGump(PlayerMobile mobile)
        {
			if (mobile == null || mobile.SoulBound || mobile.Alive) return;

            Timer.DelayCall(TimeSpan.FromSeconds(60), (m) =>
            {
				if (m == null || m.SoulBound || m.Alive) return;

                m.CloseGump(typeof(ResurrectNowGump));
                m.SendGump(new ResurrectNowGump(m));

            }, mobile);
        }
    }
}