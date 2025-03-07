using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;
using System.IO;

namespace Server.Misc
{
    class LoggingFunctions
    {
		public static bool LoggingEvents()
		{
			return true; // SET TO TRUE TO ENABLE LOG SYSTEM FOR GAME EVENTS AND TOWN CRIERS
		}

		public static void CreateFile(string sPath)
		{
			/// CREATE THE FILE IF IT DOES NOT EXIST ///
			StreamWriter w = null; 
			try
			{
				using (w = File.AppendText( sPath ) ){}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (w != null)
					w.Dispose();
			}
		}

		public static void UpdateFile(string filename, string header)
		{
			int nLine = 0;
			int nTrim = 250;
			string tempfile = Path.GetTempFileName();
			StreamWriter writer = null;
			StreamReader reader = null;
			using (writer = new StreamWriter(tempfile))
			using (reader = new StreamReader(filename))
			{
				writer.WriteLine(header);
				while (!reader.EndOfStream)
				{
					nLine = nLine + 1;
					if ( nLine < nTrim )
					{
						writer.WriteLine(reader.ReadLine());
					}
					else
					{
						reader.ReadLine();
					}
				}
			}

			if (writer != null)
				writer.Dispose();

			if (reader != null)
				reader.Dispose();

			File.Copy(tempfile, filename, true);
			File.Delete(tempfile);
		}

		public static void DeleteFile(string filename)
		{
			try
			{
				File.Delete(filename);
			}
			catch(Exception)
			{
			}
		}

		public static string LogEvent( string sEvent, string sLog )
		{
			if ( LoggingFunctions.LoggingEvents() == true )
			{
				if ( sLog != "Logging Server" )
				{
					LoggingFunctions.LogServer( "Start - " + sLog );
				}
				
				if ( !Directory.Exists( "Info" ) )
					Directory.CreateDirectory( "Info" );

				string sPath = "Info/adventures.txt";

				if ( sLog == "Logging Adventures" ){ sPath = "Info/adventures.txt"; }
				else if ( sLog == "Logging Quests" ){ sPath = "Info/quests.txt"; }
				else if ( sLog == "Logging Battles" ){ sPath = "Info/battles.txt"; }
				else if ( sLog == "Logging Deaths" ){ sPath = "Info/deaths.txt"; }
				else if ( sLog == "Logging Murderers" ){ sPath = "Info/murderers.txt"; }
				else if ( sLog == "Logging Journies" ){ sPath = "Info/journies.txt"; }
				else if ( sLog == "Logging Server" ){ sPath = "Info/server.txt"; }
				else if ( sLog == "Logging Misc" ){ sPath = "Info/misc.txt"; } // FINAL
				
				
				CreateFile( sPath );

				/// PREPEND THE FILE WITH THE EVENT ///
				try
				{
					UpdateFile(sPath, sEvent);
				}
				catch(Exception)
				{
				}
				
				if ( sLog != "Logging Server" )
				{
					LoggingFunctions.LogServer( "Done - " + sLog );
				}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogRead( string sLog, Mobile m )
		{
			if ( !Directory.Exists( "Info" ) )
				Directory.CreateDirectory( "Info" );

			string sPath = "Info/adventures.txt";

			if ( sLog == "Logging Adventures" ){ sPath = "Info/adventures.txt"; }
			else if ( sLog == "Logging Quests" ){ sPath = "Info/quests.txt"; }
			else if ( sLog == "Logging Battles" ){ sPath = "Info/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Info/deaths.txt"; }
			else if ( sLog == "Logging Murderers" ){ sPath = "Info/murderers.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Info/journies.txt"; }
			else if ( sLog == "Logging Misc" ){ sPath = "Info/misc.txt"; } // FINAL

			string sBreak = "";

			if ( sLog == "Logging Murderers"){ sBreak = "<br>"; }
			string sLogEntries = "<basefont color=#FFC000><big>";

			CreateFile( sPath );

			string eachLine = "";
			int nLine = 0;
			int nBlank = 1;
			StreamReader reader = null;

			try
			{
				using (reader = new StreamReader( sPath ))
				{
					while (!reader.EndOfStream)
					{
						eachLine = reader.ReadLine();
						string[] eachWord = eachLine.Split('#');
						nLine = 1;
						foreach (string eachWords in eachWord)
						{
							if ( nLine == 1 ){ nLine = 2; sLogEntries = sLogEntries + eachWords + ".<br>" + sBreak; nBlank = 0; }
							else { nLine = 1; sLogEntries = sLogEntries + " - " + eachWords + "<br><br>"; }
						}
					}
				}
			}
			catch(Exception)
			{
				sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I am busy at the moment.";
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			if ( nBlank == 1 )
			{
				if ( sLog == "Logging Murderers" ){ sLogEntries = sLogEntries + "I am happy to say " + m.Name + ", that no one is wanted for murder."; }
				else if ( sLog == "Logging Battles" ){ sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I have no new tales of bravery to tell."; }
				else if ( sLog == "Logging Adventures" ){ sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I have no new gossip to tell."; }
				else if ( sLog == "Logging Quests" ){ sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I have no new tales of deeds to tell."; }
				else if ( sLog == "Logging Deaths" ){ sLogEntries = sLogEntries + "I am happy to say " + m.Name + ", that all of Sosaria's citizens are alive and well."; }
				else if ( sLog == "Logging Journies" ){ sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I have no new tales of exploration to tell."; }
				else if ( sLog == "Logging Misc" ){ sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I have nothing for thee, you FodDoodle"; }
				else { sLogEntries = sLogEntries + "Sorry, " + m.Name + ". I have nothing new to tell of such things."; }
			}

			sLogEntries = sLogEntries + "</big></basefont>";
			if ( sLogEntries.Contains(" .") ){ sLogEntries = sLogEntries.Replace(" .", "."); }
			if ( sLogEntries.Contains("..") ){ sLogEntries = sLogEntries.Replace("..", "."); }

			return sLogEntries;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogArticles( int article, int section )
		{
			if ( !Directory.Exists( "Info/Articles" ) )
				Directory.CreateDirectory( "Info/Articles" );

			if ( article > 10 ){ article = 0; }
			else if ( article > 0 ){}
			else { article = 0; }

			string text = article.ToString();

			string path = "Info/Articles/" + text + ".txt";

			string part = "";

			string title = "";
			string date = "";
			string message = "";

			CreateFile( path );

			StreamReader reader = null;

			int line = 0;

			try
			{
				using (reader = new StreamReader( path ))
				{
					while (!reader.EndOfStream)
					{
						if ( line == 0 ){ title = reader.ReadLine(); }
						else if ( line == 1 ){ date = reader.ReadLine(); }
						else { message = reader.ReadLine(); }

						line++;
					}
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			if ( section == 1 ){ part = title; }
			else if ( section == 2 ){ part = date; }
			else if ( section == 3 ){ part = message; }


			if ( part.Contains(" .") ){ part = part.Replace(" .", "."); }

			return part;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static int TotalLines(string filePath)
		{
			int i = 0;
			using (StreamReader r = new StreamReader(filePath)){ while (r.ReadLine() != null) { i++; } }
			return i;
		}

		public static string LogShout()
		{
			LoggingFunctions.LogServer( "Start - Town Crier" );

			if ( !Directory.Exists( "Info" ) )
				Directory.CreateDirectory( "Info" );

			string sLog = "Logging Adventures";
			switch ( Utility.Random( 7 ))
			{
				case 0: sLog = "Logging Deaths"; break;
				case 1: sLog = "Logging Quests"; break;
				case 2: sLog = "Logging Battles"; break;
				case 3: sLog = "Logging Journies"; break;
				case 4: sLog = "Logging Murderers"; break;
				case 5: sLog = "Logging Adventures"; break;
				case 6: sLog = "Logging Misc"; break;
			};

			string sPath = "Info/adventures.txt";

			if ( sLog == "Logging Adventures" ){ sPath = "Info/adventures.txt"; }
			else if ( sLog == "Logging Quests" ){ sPath = "Info/quests.txt"; }
			else if ( sLog == "Logging Battles" ){ sPath = "Info/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Info/deaths.txt"; }
			else if ( sLog == "Logging Murderers" ){ sPath = "Info/murderers.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Info/journies.txt"; }
			else if ( sLog == "Logging Misc" ){ sPath = "Info/misc.txt"; } // FINAL

			CreateFile( sPath );

			int lineCount = 1;
			string sGreet = "Hear ye, hear ye!";
				switch ( Utility.Random( 4 ))
				{
					case 0: sGreet = "Hear ye, hear ye!"; break;
					case 1: sGreet = "Everyone listen!"; break;
					case 2: sGreet = "All hail and hear my words!"; break;
					case 3: sGreet = "Your attention please!"; break;
				};

			string myShout = "";
			if ( sLog == "Logging Murderers" ){ myShout = "Justice was served. No murders roam the lands"; }
			else
			{
				switch ( Utility.Random( 4 ))
				{
					case 0: myShout = "Nothing of interest has occurred in the land"; break;
					case 1: myShout = "Things have been quiet as of late"; break;
					case 2: myShout = "The words spoken around bear no news"; break;
					case 3: myShout = "I have yet to hear word of recent events"; break;
				};
			}

			try
			{
				lineCount = TotalLines( sPath );
			}
			catch(Exception)
			{
			}

			lineCount = Utility.RandomMinMax( 1, lineCount );
			string readLine = "";
			StreamReader reader = null;
			int nWhichLine = 0;
			int nLine = 1;
			try
			{
				using (reader = new StreamReader( sPath ))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						nWhichLine = nWhichLine + 1;
						if ( nWhichLine == lineCount )
						{
							readLine = line;
							string[] shoutOut = readLine.Split('#');
							foreach (string shoutOuts in shoutOut)
							{
								if ( nLine == 1 ){ nLine = 2; readLine = shoutOuts; }
							}
						}
					}
					if ( readLine != "" ){ myShout = readLine; }
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			string sVerb1 = "";
			string sVerb2 = "";
			switch ( Utility.Random( 4 ))
			{
				case 0: sVerb1 = "was seen in";				sVerb2 = "was seen leaving"; 			break;
				case 1: sVerb1 = "was spotted in";			sVerb2 = "was spotted leaving"; 		break;
				case 2: sVerb1 = "was known to be in";		sVerb2 = "was seen near"; 				break;
				case 3: sVerb1 = "was rumored to be in";	sVerb2 = "was spotted by"; 				break;
			};

			myShout = sGreet + " " + myShout + "!";
			if ( myShout.Contains(" !") ){ myShout = myShout.Replace(" !", "!"); }
			if ( myShout.Contains(" entered ") ){ myShout = myShout.Replace(" entered ", " " + sVerb1 + " "); }
			if ( myShout.Contains(" left ") ){ myShout = myShout.Replace(" left ", " " + sVerb2 + " "); }

			LoggingFunctions.LogServer( "Done - Town Crier" );
						
			return myShout;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogSpeak()
		{
			LoggingFunctions.LogServer( "Start - Tavern Chatter" );

			if ( !Directory.Exists( "Info" ) )
				Directory.CreateDirectory( "Info" );


			string sLog = "Logging Murderers";
			switch ( Utility.Random( 6 ))
			{
				case 0: sLog = "Logging Deaths"; break;
				case 1: sLog = "Logging Battles"; break;
				case 2: sLog = "Logging Journies"; break;
				case 3: sLog = "Logging Battles"; break;
				case 4: sLog = "Logging Journies"; break;
			};


			string sPath = "Info/murderers.txt";


			if ( sLog == "Logging Battles" ){ sPath = "Info/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Info/deaths.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Info/journies.txt"; }

			CreateFile( sPath );

			int lineCount = 1;

			string mySpeaking = "nothing of interest occuring in the land";

			try
			{
				lineCount = TotalLines( sPath );
			}
			catch(Exception)
			{
			}

			lineCount = Utility.RandomMinMax( 1, lineCount );
			string readLine = "";
			StreamReader reader = null;
			int nWhichLine = 0;
			int nLine = 1;
			try
			{
				using (reader = new StreamReader( sPath ))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						nWhichLine = nWhichLine + 1;
						if ( nWhichLine == lineCount )
						{
							readLine = line;
							string[] shoutOut = readLine.Split('#');
							foreach (string shoutOuts in shoutOut)
							{
								if ( nLine == 1 ){ nLine = 2; readLine = shoutOuts; }
							}
						}
					}
					if ( readLine != "" ){ mySpeaking = readLine; }
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			string sVerb1 = "";
			string sVerb2 = "";
			string sVerb3 = "";
			switch ( Utility.Random( 4 ))
			{
				case 0: sVerb1 = "being seen in";		sVerb2 = "being seen leaving"; 		sVerb3 = "killing";		break;
				case 1: sVerb1 = "being spotted in";	sVerb2 = "being spotted leaving"; 	sVerb3 = "slaying";		break;
				case 2: sVerb1 = "being seen in";		sVerb2 = "being seen near"; 		sVerb3 = "besting";		break;
				case 3: sVerb1 = "being spotted in";	sVerb2 = "being spotted by"; 		sVerb3 = "slaying";		break;
			};

			mySpeaking = mySpeaking;
			if ( mySpeaking.Contains(" had been ") ){ mySpeaking = mySpeaking.Replace(" had been ", " being "); }
			if ( mySpeaking.Contains(" had slain ") ){ mySpeaking = mySpeaking.Replace(" had slain ", " " + sVerb3 + " "); }
			if ( mySpeaking.Contains(" had killed ") ){ mySpeaking = mySpeaking.Replace(" had killed ", " accidentally killing "); }
			if ( mySpeaking.Contains(" made a fatal mistake ") ){ mySpeaking = mySpeaking.Replace(" made a fatal mistake ", " making a fatal mistake "); }
			if ( mySpeaking.Contains(" entered ") ){ mySpeaking = mySpeaking.Replace(" entered ", " " + sVerb1 + " "); }
			if ( mySpeaking.Contains(" left ") ){ mySpeaking = mySpeaking.Replace(" left ", " " + sVerb2 + " "); }

			LoggingFunctions.LogServer( "Done - Tavern Chatter" );
						
			return mySpeaking;
		}

		public static string LogSpeakQuest()
		{
			if ( !Directory.Exists( "Info" ) )
				Directory.CreateDirectory( "Info" );

			string sLog = "Logging Quests";
			string sPath = "Info/quests.txt";

			CreateFile( sPath );

			int lineCount = 1;

			string mySpeaking = "Adventurers seem to be all sitting around in taverns";

			try
			{
				lineCount = TotalLines( sPath );
			}
			catch(Exception)
			{
			}

			lineCount = Utility.RandomMinMax( 1, lineCount );
			string readLine = "";
			StreamReader reader = null;
			int nWhichLine = 0;
			int nLine = 1;
			try
			{
				using (reader = new StreamReader( sPath ))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						nWhichLine = nWhichLine + 1;
						if ( nWhichLine == lineCount )
						{
							readLine = line;
							string[] shoutOut = readLine.Split('#');
							foreach (string shoutOuts in shoutOut)
							{
								if ( nLine == 1 ){ nLine = 2; readLine = shoutOuts; }
							}
						}
					}
					if ( readLine != "" ){ mySpeaking = readLine; }
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}
						
			return mySpeaking;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogRegions( Mobile m, string sRegion, string sDirection )
		{
			if ( m is PlayerMobile )
			{
				int nDifficulty = MyServerSettings.GetDifficultyLevel( m.Location, m.Map );
				string sDifficulty = "";

				if ( nDifficulty == -1 ){ sDifficulty = " * (Easy)"; }
				else if ( nDifficulty == 0 ){ sDifficulty = " * (Normal)"; }
				else if ( nDifficulty == 1 ){ sDifficulty = " * (Difficult)"; }
				else if ( nDifficulty == 2 ){ sDifficulty = " * (Challenging)"; }
				else if ( nDifficulty == 3 ){ sDifficulty = " * (Hard)"; }
				else if ( nDifficulty == 4 ){ sDifficulty = " * (Deadly)"; }

				if ( sDirection == "enter" )
				{
					m.SendMessage(55, "Voc� entrou em " + sRegion + sDifficulty + "."); 
					//((PlayerMobile)m).lastdeeds = " entered " + sRegion + sDifficulty + "."; 
				}
				else { m.SendMessage(55, "Voc� saiu do(a) " + sRegion + "."); }
			}

			if ( ( m is PlayerMobile ) && ( m.AccessLevel < AccessLevel.GameMaster ) )
			{
				if ( !m.Alive && m.QuestArrow == null ){ GhostHelper.OnGhostWalking( m ); }
				string sDateString = GetPlayerInfo.GetTodaysDate();
				string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
				if ( m.Title != null ){ sTitle = m.Title; }

				PlayerMobile pm = (PlayerMobile)m;
				////if (pm.PublicMyRunUO == true)
				//{
					string sEvent;

					if ( sDirection == "enter" ){ sEvent = m.Name + " " + sTitle + " entered " + sRegion + "#" + sDateString; LoggingFunctions.LogEvent( sEvent, "Logging Journies" ); }
					// else { sEvent = m.Name + " " + sTitle + " left " + sRegion + "#" + sDateString; LoggingFunctions.LogEvent( sEvent, "Logging Journies" ); }
				//}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogBattles( Mobile m, Mobile mob )
		{
			if (m == null || mob == null || mob.Blessed )
				return null;

			string sDateString = GetPlayerInfo.GetTodaysDate();

			if ( m is PlayerMobile && mob != null )
			{
				string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
				if ( m.Title != null ){ sTitle = m.Title; }

				PlayerMobile pm = (PlayerMobile)m;

				string sKiller = mob.Name;
				string[] eachWord = sKiller.Split('[');
				int nLine = 1;
				foreach (string eachWords in eachWord)
				{
					if ( nLine == 1 ){ nLine = 2; sKiller = eachWords; }
				}
				sKiller = sKiller.TrimEnd();

				//if ( pm.PublicMyRunUO == true )
				//{
					string Killed = sKiller;
						if ( mob.Title != "" && mob.Title != null ){ Killed = Killed + " " + mob.Title; }
					string sEvent = m.Name + " " + sTitle + " had slain " + Killed + "#" + sDateString;
					((PlayerMobile)m).lastdeeds = "killed " + Killed;
					LoggingFunctions.LogEvent( sEvent, "Logging Battles" );
				//}
				/*else
				{
					string privateEnemy = "an opponent";
					switch ( Utility.Random( 6 ) )
					{
						case 0: privateEnemy = "an opponent"; break;
						case 1: privateEnemy = "an enemy"; break;
						case 2: privateEnemy = "another"; break;
						case 3: privateEnemy = "an adversary"; break;
						case 4: privateEnemy = "a foe"; break;
						case 5: privateEnemy = "a rival"; break;
					}
					string sEvent = m.Name + " " + sTitle + " had slain " + privateEnemy + "#" + sDateString;
					LoggingFunctions.LogEvent( sEvent, "Logging Battles" );
				}*/
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogTraps( Mobile m, string sTrap )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sTrip = "had triggered";
			switch( Utility.Random( 7 ) )
			{
				case 0: sTrip = "had triggered";	break;
				case 1: sTrip = "had set off";	break;
				case 2: sTrip = "had walked into";	break;
				case 3: sTrip = "had stumbled into";	break;
				case 4: sTrip = "had been struck with";	break;
				case 5: sTrip = "had been affected with";	break;
				case 6: sTrip = "had ran into";	break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " " + sTrip + " " + sTrap + "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  sTrip + " " + sTrap;
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			//}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogVoid( Mobile m, string sTrap )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " " + sTrap + ", teleporting them far away#" + sDateString;
				((PlayerMobile)m).lastdeeds =  sTrap + ", teleporting them far away";
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			//}

			return null;
		}

//final lottery speak
		public static string LogLottery( Mobile m, int purchase )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " spent " + purchase + " gold on Lottery tickets!" + "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " spent " + purchase + " gold on Lottery tickets!";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}

		public static string LogInvestments( Mobile m, int goldearned, bool gain )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = "";
				if (gain)
				{
					sEvent = m.Name + " " + sTitle + " earned " + goldearned + " gold on their investments!" + "#" + sDateString;
					((PlayerMobile)m).lastdeeds =  " earned " + goldearned + " gold on their investments!";
				}
				else
				{
					sEvent = m.Name + " " + sTitle + " lost " + goldearned + " gold on their investments!" + "#" + sDateString;
					((PlayerMobile)m).lastdeeds =  " lost " + goldearned + " gold on their investments!";
				}
					
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}

		public static string LogWin( Mobile m, int win )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " won " + win + " gold in the Lottery!"+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " won " + win + " gold in the Lottery!";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}
		
		public static string LogLose( Mobile m, int pot )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " did not win " + pot + " gold that was up for grabs at the Lottery"+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " did not win " + pot + " gold that was up for grabs at the Lottery";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}
		
		public static string LogWench( Mobile m, int price, bool stolen)
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				if (stolen)
				{
				string sEvent = m.Name + " " + sTitle + " fell in love with the Wench and gave her all their gold!" + "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " fell in love with the Wench and gave her all their gold!";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
				}
				else
				{
				string sEvent = m.Name + " " + sTitle + " paid " + price + " for the services of a wench."+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " paid " + price + " for the services of a wench.";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
				}
			//}

			return null;
		}

		public static string LogPetSale( Mobile m, Mobile pet, int price)
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			string petname = pet.Name;
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " sold " + petname + " for " + price + " gold to the animal broker."+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " sold " + petname + " for " + price + " gold to the animal broker.";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}

		public static string LogBreed( Mobile m, Mobile pet, uint level)
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " bred a " + pet + " with a maximum level of " + level + " !"+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " bred a " + pet + " with a maximum level of " + level + " !";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}

		public static string LogZombie( Mobile m )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " died and rose again as a deadly infected Zombie!! "+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " died and rose again as a deadly infected Zombie!!";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}
		
		public static string LogStudy( Mobile m, SkillName skill, double amount )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = m.Name + " " + sTitle + " studied " + skill + " assiduously and raised the skill " + String.Format("{0:0.0}", amount) + " points! "+ "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " studied " + skill + " assiduously and raised the skill " + String.Format("{0:0.0}", amount) + " points!";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}
			
		public static string LogCarve( Mobile m, string name )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			AetherGlobe.BalanceLevel += Utility.RandomMinMax(1, 5);
			
			PlayerMobile pm = (PlayerMobile)m;
			////if (pm.PublicMyRunUO == true)
			//{
				string sEvent = "In an unmitigated act of pure bloodlust, " + m.Name + " " + sTitle + " butchered " + name + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " butchered " + name + ".  It was messy.";
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			//}

			return null;
		}
		
		public static string LogGM( Mobile m, Skill skill )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " is now a GrandMaster in  " + skill.Name + "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " became a grandmaster in " + skill.Name;
				LoggingFunctions.LogEvent( sEvent, "Logging Misc" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogKillTile( Mobile m, string sTrap )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " made a fatal mistake from " + sTrap + "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " made a fatal mistake from " + sTrap;
				LoggingFunctions.LogEvent( sEvent, "Logging Journies" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////


		public static string LogLoot( Mobile m, string sBox, string sType )
		{
			if (Utility.RandomDouble() < 0.25) // FInal - too many of these were being generated
			{
				string sDateString = GetPlayerInfo.GetTodaysDate();
				string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
				if ( m.Title != null ){ sTitle = m.Title; }

				string sLoot = "had searched through a";
				switch( Utility.Random( 7 ) )
				{
					case 0: sLoot = "had searched through a";	break;
					case 1: sLoot = "had found a";	break;
					case 2: sLoot = "had discovered a";	break;
					case 3: sLoot = "had looked through a";	break;
					case 4: sLoot = "had stumbled upon a";	break;
					case 5: sLoot = "had dug through a";	break;
					case 6: sLoot = "had opened a";	break;
				}
				if ( sType == "boat" )
				{
					switch( Utility.Random( 5 ) )
					{
						case 0: sLoot = "had searched through a";	break;
						case 1: sLoot = "had found a";	break;
						case 2: sLoot = "had discovered a";	break;
						case 3: sLoot = "had looked through a";	break;
						case 4: sLoot = "had sailed upon a";	break;
					}
					if ( sBox.Contains("Abandoned") || sBox.Contains("Adrift") ){ sLoot = sLoot + "n"; }
				}
				else if ( sType == "corpse" )
				{
					switch( Utility.Random( 5 ) )
					{
						case 0: sLoot = "had searched through a";	break;
						case 1: sLoot = "had found a";	break;
						case 2: sLoot = "had discovered a";	break;
						case 3: sLoot = "had looked through a";	break;
						case 4: sLoot = "had sailed upon a";	break;
					}
					if ( sBox.Contains("Abandoned") || sBox.Contains("Adrift") ){ sLoot = sLoot + "n"; }
				}

				PlayerMobile pm = (PlayerMobile)m;
				//if (pm.PublicMyRunUO == true)
				{
					string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBox + "#" + sDateString;
					((PlayerMobile)m).lastdeeds =  " " + sLoot + " " + sBox;
					LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
				}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogSlayingLord( Mobile m, string creature )
		{
			if ( m != null )
			{
				if ( m is BaseCreature )
					m = ((BaseCreature)m).GetMaster();

				if ( m is PlayerMobile )
				{
					string sDateString = GetPlayerInfo.GetTodaysDate();
					string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
					if ( m.Title != null ){ sTitle = m.Title; }

					string verb = "has destroyed";
					switch( Utility.Random( 4 ) )
					{
						case 0: verb = "has defeated";		break;
						case 1: verb = "has slain";		break;
						case 2: verb = "has destroyed";	break;
						case 3: verb = "has vanquished";	break;
					}

					PlayerMobile pm = (PlayerMobile)m;
					//if (pm.PublicMyRunUO == true)
					{
						string sEvent = m.Name + " " + sTitle + " " + verb + " " + creature + "#" + sDateString;
						((PlayerMobile)m).lastdeeds =  " " + verb + " " + creature;
						LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
					}
				}
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogCreatedArtifact( Mobile m, string sArty )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = "The gods have created a legendary artifact called " + sArty + "#" + sDateString;
				((PlayerMobile)m).lastdeeds =  " was granted a legendary artefact called " + sArty;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogRuneOfVirtue( Mobile m, string side )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sText = "has cleansed the Runes to the Chamber of Virtue.";
				if ( side == "evil" ){ sText = "has corrupted the Runes of Virtue."; }

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sText;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogCreatedSyth( Mobile m, string sArty )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = "A Syth constructed a weapon called " + sArty + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " constructed a weapon called " + sArty;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		
			// --------------------------------------------------------------------------------------------
		public static string LogCreatedJedi( Mobile m, string sArty )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = "A Jedi constructed a weapon called " + sArty + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " constructed a weapon called " + sArty;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		
		// --------------------------------------------------------------------------------------------
		public static string LogGenericQuest( Mobile m, string sText )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sText;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogFoundItemQuest( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "has discovered the";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "has found the";		break;
				case 1: sLoot = "has recovered the";	break;
				case 2: sLoot = "has unearthed the";	break;
				case 3: sLoot = "has discovered the";	break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBox + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + " " + sBox;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestItem( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "has discovered";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "has found";		break;
				case 1: sLoot = "has recovered";	break;
				case 2: sLoot = "has unearthed";	break;
				case 3: sLoot = "has discovered";	break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBox + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + " " + sBox;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestBody( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "has found";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "has found";		break;
				case 1: sLoot = "has recovered";	break;
				case 2: sLoot = "has unearthed";	break;
				case 3: sLoot = "has dug up";		break;
			}

			string sBone = "the bones";
			switch( Utility.Random( 4 ) )
			{
				case 0: sBone = "the bones";		break;
				case 1: sBone = "the body";			break;
				case 2: sBone = "the remains";		break;
				case 3: sBone = "the corpse";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBone + " of " + sBox + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + " " + sBone + " of " + sBox;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestChest( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "has found";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "has found";		break;
				case 1: sLoot = "has recovered";	break;
				case 2: sLoot = "has unearthed";	break;
				case 3: sLoot = "has dug up";		break;
			}

			string sChest = "the hidden";
			switch( Utility.Random( 4 ) )
			{
				case 0: sChest = "the hidden";		break;
				case 1: sChest = "the lost";		break;
				case 2: sChest = "the missing";		break;
				case 3: sChest = "the secret";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sChest + " chest of " + sBox + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + " " + sChest + " chest of " + sBox;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestMap( Mobile m, int sLevel, string chest )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "has found";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "has found";		break;
				case 1: sLoot = "has recovered";	break;
				case 2: sLoot = "has unearthed";	break;
				case 3: sLoot = "has dug up";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + chest + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + " " + chest;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestSea( Mobile m, int sLevel, string sShip )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "has fished up";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "has surfaced";		break;
				case 1: sLoot = "has salvaged";		break;
				case 2: sLoot = "has brought up";	break;
				case 3: sLoot = "has fished up";	break;
			}

			string sChest = "a grand sunken chest";
			switch( sLevel )
			{
				case 0: sChest = "a meager sunken chest";		break;
				case 1: sChest = "a simple sunken chest";		break;
				case 2: sChest = "a good sunken chest";			break;
				case 3: sChest = "a great sunken chest";		break;
				case 4: sChest = "an excellent sunken chest";	break;
				case 5: sChest = "a superb sunken chest";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sChest + " from " + sShip + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + " " + sChest + " from " + sShip;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestKill( Mobile m, string sBox, Mobile t )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "";
			string sWho = "";
			
			if ( sBox == "bounty" )
			{
				sWho = "";
				switch( Utility.Random( 4 ) )
				{
					case 0: sLoot = "has fullfilled a bounty on";	break;
					case 1: sLoot = "has claimed a bounty on";		break;
					case 2: sLoot = "has served a bounty on";		break;
					case 3: sLoot = "has completed a bounty on";	break;
				}
			}
			if ( sBox == "sea" )
			{
				sWho = " on the high seas";
				switch( Utility.Random( 4 ) )
				{
					case 0: sLoot = "has fullfilled a bounty on";	break;
					case 1: sLoot = "has claimed a bounty on";		break;
					case 2: sLoot = "has served a bounty on";		break;
					case 3: sLoot = "has completed a bounty on";	break;
				}
			}
			if ( sBox == "assassin" )
			{
				sWho = " for the guild";
				switch( Utility.Random( 4 ) )
				{
					case 0: sLoot = "has assassinated";		break;
					case 1: sLoot = "has dispatched";		break;
					case 2: sLoot = "has dealt with";		break;
					case 3: sLoot = "has eliminated";		break;
				}
			}
			
			sLoot = sLoot + " " + t.Name + " " + t.Title;

			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + sWho + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sLoot + sWho;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogGeneric( Mobile m, string sText )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }


			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				((PlayerMobile)m).lastdeeds = " " + sText;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}


			return null;
		}


		/////////////////////////////////////////////////////////////////////////////////////////////////////////////


		public static string LogStandard( Mobile m, string sText )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }


			PlayerMobile pm = (PlayerMobile)m;
			//if (pm.PublicMyRunUO == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				//((PlayerMobile)m).lastdeeds = " " + sText;
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static String GetTimestamp(DateTime value)
		{
			return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

		public static string LogServer ( string sText )
		{
			//String timeStamp = GetTimestamp(DateTime.UtcNow);

			//string sEvent = sText + "#" + timeStamp;
			//LoggingFunctions.LogEvent( sEvent, "Logging Server" );

			return null;
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogAccess( Mobile m, string sAccess )
		{
			PlayerMobile pm = (PlayerMobile)m;
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

            if ( m.AccessLevel < AccessLevel.GameMaster )
            {
				string sEvent;
				if ( sAccess == "login" )
				{
					sEvent = m.Name + " " + sTitle + " had entered the realm#" + sDateString;
					World.Broadcast(0x35, true, "{0} {1} has entered the realm", m.Name, sTitle);
				}
				else
				{
					sEvent = m.Name + " " + sTitle + " had left the realm#" + sDateString;
					World.Broadcast(0x35, true, "{0} {1} has left the realm", m.Name, sTitle);
				}


				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
            }

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogDeaths( Mobile m, Mobile mob )
		{
			if ( m != null && m is PlayerMobile && mob != null )
			{
				PlayerMobile pm = (PlayerMobile)m;
				string sDateString = GetPlayerInfo.GetTodaysDate();
				string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
				if ( m.Title != null ){ sTitle = m.Title; }

				string sKiller = mob.Name;
				string[] eachWord = sKiller.Split('[');
				int nLine = 1;
				foreach (string eachWords in eachWord)
				{
					if ( nLine == 1 ){ nLine = 2; sKiller = eachWords; }
				}
				sKiller = sKiller.TrimEnd();

				///////// PLAYER DIED SO DO SINGLE FILES //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if ( m.AccessLevel < AccessLevel.GameMaster )
				{
					string sEvent = "";

					//if ( pm.PublicMyRunUO == true )
					//{
						if ( ( mob == m ) && ( mob != null ) )
						{
							sEvent = m.Name + " " + sTitle + " had killed themselves#" + sDateString;
						}
						else if ( ( mob != null ) && ( mob is PlayerMobile ) )
						{
							string kTitle = " the " + GetPlayerInfo.GetSkillTitle( mob );
							if ( mob.Title != null ){ kTitle = " " + mob.Title; }
							sEvent = m.Name + " " + sTitle + " had been killed by " + sKiller + kTitle + "#" + sDateString;
						}
						else if ( mob != null )
						{
							string kTitle = "";
							if ( mob.Title != null ){ kTitle = " " + mob.Title; }
							sEvent = m.Name + " " + sTitle + " had been killed by " + sKiller + kTitle + "#" + sDateString;
						}
						else
						{
							sEvent = m.Name + " " + sTitle + " had been killed#" + sDateString;
						}
					/*}
					else
					{
						string privateEnemy = "an opponent";
						switch ( Utility.Random( 6 ) )
						{
							case 0: privateEnemy = "an opponent"; break;
							case 1: privateEnemy = "an enemy"; break;
							case 2: privateEnemy = "another"; break;
							case 3: privateEnemy = "an adversary"; break;
							case 4: privateEnemy = "a foe"; break;
							case 5: privateEnemy = "a rival"; break;
						}

						if ( ( mob == m ) && ( mob != null ) )
						{
							sEvent = m.Name + " " + sTitle + " had killed themselves#" + sDateString;
						}
						else if ( ( mob != null ) && ( mob is PlayerMobile ) )
						{
							string kTitle = "the " + GetPlayerInfo.GetSkillTitle( mob );
							if ( mob.Title != null ){ kTitle = mob.Title; }
							sEvent = m.Name + " " + sTitle + " had been killed by " + sKiller + " " + kTitle + "#" + sDateString;
						}
						else if ( mob != null )
						{
							sEvent = m.Name + " " + sTitle + " had been killed by " + privateEnemy + "#" + sDateString;
						}
						else
						{
							sEvent = m.Name + " " + sTitle + " had been killed#" + sDateString;
						}
					}*/
					LoggingFunctions.LogEvent( sEvent, "Logging Deaths" );
				}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogKillers( Mobile m, int nKills )
		{
			string sEvent = "";
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			if ( m.Kills > 1){ sEvent = m.Name + " " + sTitle + " is wanted for the murder of " + m.Kills + " people."; }
			else if ( m.Kills > 0){ sEvent = m.Name + " " + sTitle + " is wanted for murder."; }

			LoggingFunctions.LogEvent( sEvent, "Logging Murderers" );

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogClear( string sLog )
		{
			string sPath = "Info/adventures.txt";
 
			if ( sLog == "Logging Adventures" ){ sPath = "Info/adventures.txt"; }
			else if ( sLog == "Logging Battles" ){ sPath = "Info/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Info/deaths.txt"; }
			else if ( sLog == "Logging Murderers" ){ sPath = "Info/murderers.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Info/journies.txt"; }
			else if ( sLog == "Logging Misc" ){ sPath = "Info/misc.txt"; }

			DeleteFile( sPath );

			return null;
		}
	}
}
