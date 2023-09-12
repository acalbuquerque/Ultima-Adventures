using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
	public class ResurrectCostGump : Gump
	{
		private int m_Price;
		private int m_Healer;
		private int m_Bank;
		private int m_ResurrectType;

		public ResurrectCostGump( Mobile owner, int healer ) : base( 150, 50 )
		{

			if ( !(owner is PlayerMobile) )
				return;

			if ( ((PlayerMobile)owner).SoulBound )
			{
				((PlayerMobile)owner).ResetPlayer( owner );
				owner.CloseGump( typeof( ResurrectCostGump ) );
				return;
			}

			m_Healer = healer;
			m_Price = GetPlayerInfo.GetResurrectCost( owner );
			m_Bank = Banker.GetBalance( owner );
			m_ResurrectType = 0;

			string sText = "";

			double penalty;
			
			int karma = Math.Abs(owner.Karma);
			if ( karma < 5000)
				karma = 5000;
			
			if (owner.Karma >= 0)
				penalty = (((double)AetherGlobe.BalanceLevel / 100000.0) * ( ((double)karma / 15000))) /1.5 ; // range of 0.0 to 0.66
			else
				penalty =  (( (100000-(double)AetherGlobe.BalanceLevel) / 100000.0) * ( ((double)karma / 15000))) /1.5;

			if (penalty < 0.1)
				penalty = 0.1;

			string c1 = String.Format("{0:0.0}", penalty);
			string c2 = "5";
			if ( !((PlayerMobile)owner).Avatar)
				c2  = "10";

			string loss = "";

            string f0 = "Atualmente você tem ouro suficiente no banco para fazer uma oferenda ao curandeiro. Você deseja prestar homenagem ao curandeiro pela sua vida de volta?";
            string f1 = "<br/>Se fizer isso, você sofrerá uma perda de " + c2 + "% de perda de sua fama e karma.";
			string f2 = "Você também perderá " + c1 + "% de algumas habilidades aleatórias.";
            string f3 = "Atualmente você não tem ouro suficiente no banco para oferecer uma oferenda ao curandeiro. Você deseja implorar ao curador pela sua vida de volta agora, sem prestar homenagem?";
			string f4 = "Felizmente para você, o santuário não precisa de ouro para trazê-lo de volta à vida. Você deseja implorar aos deuses por sua vida de volta agora?";
            string f5 = "Você também perderá " + c1 + "% de suas estatísticas e habilidades aleatórias.";

            if (((PlayerMobile)owner).Avatar)
				loss = " " + f1 + " " + f2;
			else
				loss = " " + f1;

            if ( m_Price > 0 )
			{
				if ( m_Price > m_Bank )
				{
					c1 = String.Format("{0:0.0}", (penalty * 3));
					
					if ( !((PlayerMobile)owner).Avatar)
					{
						if ((owner.RawStr + owner.RawDex + owner.RawInt) < 125)
							c2 = "20";
						else
							c2 = "40";
					}
					else 
						c2 = "10";		
					
					/*if ( (owner.RawStr + owner.RawDex + owner.RawInt) < 125 )
					{
						if ( !((PlayerMobile)owner).Avatar )
							sText = "You currently do not have enough gold in the bank to provide an offering to the healer. Do you wish to plead to the healer for your life back now, without providing tribute? If you do, you will suffer a " + c2 + "% loss to your fame and karma.";
						else	
							sText = "You currently do not have enough gold in the bank to provide an offering to the healer. Do you wish to plead to the healer for your life back now, without providing tribute? If you do, you will suffer a " + c2 + "% loss to your fame and karma. You will not loose any stats or skills because of your weak status.";
						m_ResurrectType = 1;
					}*/

					/*else */
					if ( m_Healer < 2 )
					{
						if (!((PlayerMobile)owner).Avatar)
							sText = f3 + " " + f1;
						else
							sText = f3 + " " + f1 + " " + f5;
						m_ResurrectType = 1;
					}
					else
					{
						m_ResurrectType = 1;
						
						if (m_Healer == 2)
						{
							if ( !((PlayerMobile)owner).Avatar)
							{
								if ((owner.RawStr + owner.RawDex + owner.RawInt) < 125)
									c2 = "20";
								else
									c2 = "40";
							}
							else
								c2 = "20";
							c1 = String.Format("{0:0.0}", (penalty));
							
							if ( !((PlayerMobile)owner).Avatar )
								sText = f4 + " " + f1;
							else
								sText = f4 + " " + f1 + " " + f5;

							m_ResurrectType = 3;
						}
												
						else if ( m_Healer == 3 )
                        {// Azrael
                            if ( !((PlayerMobile)owner).Avatar)
								sText = f3 + " " + f1; 
							else
								sText = f3 + " " + f1 + " " + f5;
                            m_ResurrectType = 1;
						}
						else if ( m_Healer == 4 )
                        { //Reaper
                            if ( !((PlayerMobile)owner).Avatar)
								sText = sText = f3 + " " + f1;
							else
								sText = f3 + " " + f1 + " " + f5;
							m_ResurrectType = 1;
						}
						else if ( m_Healer == 5 )
                        { //goddess of the sea
                            if (!((PlayerMobile)owner).Avatar)
                                sText = sText = f3 + " " + f1;
                            else
                                sText = f3 + " " + f1 + " " + f5;
                            m_ResurrectType = 1;
						}
					}
				}
				
				else //they have enough gold
				{
					
					if ( !((PlayerMobile)owner).Avatar)
					{
						if ((owner.RawStr + owner.RawDex + owner.RawInt) < 125)
							c2 = "10";
						else
							c2 = "20";
					}
					
					/*if ( (owner.RawStr + owner.RawDex + owner.RawInt) < 125 )
					{
						if ( !((PlayerMobile)owner).Avatar)
						sText = "You currently have enough gold in the bank to provide an offering to the healer. Do you wish to offer the tribute to the healer for your life back? If you do, you will suffer a " + c2 + "% loss to your fame and karma. You will not loose any stats or skills because of your weak status.";
					}
					
					else */if ( m_Healer < 2 )
					{
						sText = f0 + loss;
						m_ResurrectType = 2;
					}
					else
					{
						if (m_Healer == 2)
						{
							if ( !((PlayerMobile)owner).Avatar)
							{
								if ((owner.RawStr + owner.RawDex + owner.RawInt) < 125)
									c2 = "20";
								else
									c2 = "40";
							}
							else
								c2 = "10";
							
							c1 = String.Format("{0:0.0}", (penalty));
							sText = f4 + " " + f1 + " " + f5;
                            m_ResurrectType = 3;
						}
						else if ( m_Healer == 3 )
						{
							sText = "Azrael ainda não está pronto para receber sua alma e atualmente você tem ouro suficiente no banco para fazer uma oferenda a ele. Você deseja prestar homenagem a Azrael pela sua vida de volta?" + loss;
						m_ResurrectType = 2;
						}
						else if ( m_Healer == 4 )
						{
							sText = "Embora o Ceifador ficasse feliz em levar sua alma, ele acha que seu tempo chegou ao fim muito cedo. Atualmente você tem ouro suficiente no banco para fazer uma oferenda ao Reaper. Você deseja prestar-lhe uma homenagem pela sua vida de volta?" + loss;
						m_ResurrectType = 2;
						}
						else if ( m_Healer == 5 )
						{
							sText = "Atualmente você tem ouro suficiente no banco para fazer uma oferenda à deusa do mar. Você deseja homenagear Anfitrite pela sua vida de volta?" + loss;
						m_ResurrectType = 2;
						}
					}
					
				}
			}
			else
			{
				if ( m_Healer < 2 )
				{
					sText = "Você deseja que o curandeiro o traga de volta à vida?";
				}
				else
				{
					sText = "Você deseja que os deuses o devolvam à vida?";

					if ( m_Healer == 3 )
					{
						sText = "Você deseja que Azrael o traga de volta à vida?";
					}
					else if ( m_Healer == 4 )
					{
						sText = "Você deseja que o Reaper o traga de volta à vida?";
					}
					else if ( m_Healer == 5 )
					{
						sText = "Você deseja que Anfitrite o traga de volta à vida?";
					}
				}
			}

			string sGrave = "VOLTAR À VIDA";
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0:	sGrave = "SUA VIDA DE VOLTA";break;
				case 1:	sGrave = "SUA RESSURREIÇÃO";break;
				case 2:	sGrave = "VOLTAR À VIDA";break;
				case 3:	sGrave = "RETORNO DOS MORTOS";break;
			}

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

            double bankGoldKK = m_Bank;
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

			AddButton(101, 365, 4023, 4023, 1, GumpButtonType.Reply, 0);
            AddHtml(101 + 30, 365 + 2, 477, 22, @"<BODY><BASEFONT Color=#5eff00><BIG> Ressuscite-me </BIG></BASEFONT></BODY>", false, false);
            AddButton(307, 365, 4020, 4020, 2, GumpButtonType.Reply, 0);
            AddHtml(307 + 30, 365 + 2, 477, 22, @"<BODY><BASEFONT Color=#FF0000><BIG> Não </BIG></BASEFONT></BODY>", false, false);

            if ( m_Price > 0 && m_Healer != 2 )
			{
				AddHtml( 101, 271, 190, 22, @"<BODY><BASEFONT Color=#FCFF00><BIG>Tributo</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 307, 271, 196, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + String.Format("{0} Moeda(s) de Ouro", m_Price ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

				AddHtml( 101, 305, 190, 22, @"<BODY><BASEFONT Color=#FCFF00><BIG>Dinheiro no banco</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
                if (useKNotation)
                    AddHtml(307, 305, 196, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + bankGoldKK.ToString() + "k Moeda(s) de Ouro</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
                else if (useKKNotation)
                    AddHtml(307, 305, 196, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + bankGoldKK.ToString() + "kk Moeda(s) de Ouro</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
                else
                    AddHtml(307, 305, 156, 22, @"<BODY><BASEFONT Color=#ffffff><BIG>" + String.Format("{0} Gold", m_Bank) + " Moeda(s) de Ouro</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
                //AddHtml( 307, 305, 116, 22, @"<BODY><BASEFONT Color=#FCFF00><BIG>" + String.Format("{0} Gold", m_Bank ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			}

			AddHtml( 177, 90, 400, 22, @"<BODY><BASEFONT Color=#fff700><BIG><CENTER>" + sGrave + "</CENTER></BIG></BASEFONT></BODY>", (bool)false, (bool)false);

			AddHtml( 100, 155, 477, 123, @"<BODY><BASEFONT Color=#ffffff><BIG>" + sText + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			from.CloseGump( typeof( ResurrectCostGump ) );

			if( info.ButtonID == 1 )
			{
				if( from.Map == null || !from.Map.CanFit( from.Location, 16, false, false ) )
				{
                    from.SendMessage(55, "Você não pode ser ressuscitado aqui!");
					return;
				}

				if ( m_ResurrectType == 2  )
				{
					if (from is PlayerMobile)
					{
						if ( AetherGlobe.EvilChamp != from && AetherGlobe.GoodChamp != from )
						{
							Banker.Withdraw( from, m_Price );
							from.SendLocalizedMessage( 1060398, m_Price.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
							from.SendLocalizedMessage( 1060022, Banker.GetBalance( from ).ToString() ); // You have ~1_AMOUNT~ gold in cash remaining in your bank box.
						}
						Server.Misc.Death.Penalty( from, false );
					}
				}
				else if ( m_ResurrectType == 1 )
				{
					Server.Misc.Death.Penalty( from, true );
				}
				else if ( m_ResurrectType == 3 )
				{
					Server.Misc.Death.Penalty( from, false, true);
				}

				from.PlaySound( 0x214 );
				from.FixedEffect( 0x376A, 10, 16 );

				from.Resurrect();

				from.Hits = from.HitsMax;
				from.Stam = from.StamMax;
				from.Mana = from.ManaMax;
				if (from.Criminal)
					from.Criminal = false;
				from.Hidden = true;
			}
			else
			{
                from.SendMessage(55, "Você decide permanecer no reino espiritual.");
            }
		}
	}
}
