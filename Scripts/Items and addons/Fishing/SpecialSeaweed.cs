using System;
using Server;

namespace Server.Items
{
	public class SpecialSeaweed : Item
	{
		public int SkillNeeded;
		
		[CommandProperty(AccessLevel.Owner)]
		public int Skill_Needed { get { return SkillNeeded; } set { SkillNeeded = value; InvalidateProperties(); } }

		[Constructable]
		public SpecialSeaweed() : this( 1 )
		{
		}

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public SpecialSeaweed( int amount ) : base( 0x0A96 )
		{
			switch( Utility.Random( 31 ) )
			{
				case 0 : this.Hue = 1109; this.Name = "Algas da Visão Noturna"; SkillNeeded = 50; break;
				case 1 : this.Hue = 45; this.Name = "Algas da Cura Menor"; SkillNeeded = 50; break;
				case 2 : this.Hue = 45; this.Name = "Algas da Cura"; SkillNeeded = 60; break;
				case 3 : this.Hue = 45; this.Name = "Algas da Cura Maior"; SkillNeeded = 80; break;
				case 4 : this.Hue = 396; this.Name = "Algas da Agilidade"; SkillNeeded = 60; break;
				case 5 : this.Hue = 396; this.Name = "Algas da Agilidade Maior"; SkillNeeded = 80; break;
				case 6 : this.Hue = 1001; this.Name = "Algas da Força"; SkillNeeded = 60; break;
				case 7 : this.Hue = 1001; this.Name = "Algas da Força Maior"; SkillNeeded = 80; break;
				case 8 : this.Hue = 73; this.Name = "Algas de Veneno Fraco"; SkillNeeded = 50; break;
				case 9 : this.Hue = 73; this.Name = "Algas Venenosas"; SkillNeeded = 60; break;
				case 10 : this.Hue = 73; this.Name = "Algas de Veneno Maior"; SkillNeeded = 70; break;
				case 11 : this.Hue = 73; this.Name = "Algas de Veneno Mortal"; SkillNeeded = 80; break;
				case 12 : this.Hue = 73; this.Name = "Algas de Veneno Letal"; SkillNeeded = 90; break;
				case 13 : this.Hue = 140; this.Name = "Algas da Revigoração"; SkillNeeded = 60; break;
				case 14 : this.Hue = 140; this.Name = "Algas da Revigoração Total"; SkillNeeded = 80; break;
				case 15 : this.Hue = 50; this.Name = "Algas da Vida Menor"; SkillNeeded = 50; break;
				case 16 : this.Hue = 50; this.Name = "Algas da Vida"; SkillNeeded = 60; break;
				case 17 : this.Hue = 50; this.Name = "Algas da Vida Maior"; SkillNeeded = 80; break;
				case 18 : this.Hue = 425; this.Name = "Algas da Explosão Menor"; SkillNeeded = 50; break;
				case 19 : this.Hue = 425; this.Name = "Algas da Explosão"; SkillNeeded = 60; break;
				case 20 : this.Hue = 425; this.Name = "Algas da Explosão Maior"; SkillNeeded = 80; break;
				case 21 : this.Hue = 0x490; this.Name = "Algas da Invisibilidade Menor"; SkillNeeded = 50; break;
				case 22 : this.Hue = 0x490; this.Name = "Algas da Invisibilidade"; SkillNeeded = 60; break;
				case 23 : this.Hue = 0x490; this.Name = "Algas da Invisibilidade Maior"; SkillNeeded = 80; break;
				case 24 : this.Hue = 0x48E; this.Name = "Algas de Rejuvenescimento Menor"; SkillNeeded = 50; break;
				case 25 : this.Hue = 0x48E; this.Name = "Algas de Rejuvenescimento"; SkillNeeded = 60; break;
				case 26 : this.Hue = 0x48E; this.Name = "Algas de Rejuvenescimento Maior"; SkillNeeded = 80; break;
				case 27 : this.Hue = 0x48D; this.Name = "Algas da Mana Menor"; SkillNeeded = 50; break;
				case 28 : this.Hue = 0x48D; this.Name = "Algas da Mana"; SkillNeeded = 60; break;
				case 29 : this.Hue = 0x48D; this.Name = "Algas da Mana Maior"; SkillNeeded = 80; break;
				case 30 : this.Hue = 0x496; this.Name = "Algas da Invulnerabilidade"; SkillNeeded = 95; break;
			}

			Stackable = true;
			Amount = amount;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.CheckSkill( SkillName.Fishing, SkillNeeded, 125 ) )
			{
				if (!from.Backpack.ConsumeTotal(typeof(Bottle), 1))
				{
					from.SendMessage(55, "Você precisa de uma garrafa vazia para drenar o líquido das algas.");
					return;
				}
				else
				{
					from.PlaySound( 0x240 );

					if ( this.Name == "Algas da Visão Noturna" ) { from.AddToBackpack( new NightSightPotion() ); }
					else if ( this.Name == "Algas da Cura Menor") { from.AddToBackpack( new LesserCurePotion() ); }
					else if ( this.Name == "Algas da Cura") { from.AddToBackpack( new CurePotion() ); }
					else if ( this.Name == "Algas da Cura Maior") { from.AddToBackpack( new GreaterCurePotion() ); }
					else if ( this.Name == "Algas da Agilidade") { from.AddToBackpack( new AgilityPotion() ); }
					else if ( this.Name == "Algas da Agilidade Maior") { from.AddToBackpack( new GreaterAgilityPotion() ); }
					else if ( this.Name == "Algas da Força") { from.AddToBackpack( new StrengthPotion() ); }
					else if ( this.Name == "Algas da Força Maior") { from.AddToBackpack( new GreaterStrengthPotion() ); }
					else if ( this.Name == "Algas de Veneno Fraco") { from.AddToBackpack( new LesserPoisonPotion() ); }
					else if ( this.Name == "Algas Venenosas") { from.AddToBackpack( new PoisonPotion() ); }
					else if ( this.Name == "Algas de Veneno Maior") { from.AddToBackpack( new GreaterPoisonPotion() ); }
					else if ( this.Name == "Algas de Veneno Mortal") { from.AddToBackpack( new DeadlyPoisonPotion() ); }
					else if ( this.Name == "Algas de Veneno Letal") { from.AddToBackpack( new LethalPoisonPotion() ); }
					else if ( this.Name == "Algas da Revigoração") { from.AddToBackpack( new RefreshPotion() ); }
					else if ( this.Name == "Algas da Revigoração Total") { from.AddToBackpack( new TotalRefreshPotion() ); }
					else if ( this.Name == "Algas da Vida Menor") { from.AddToBackpack( new LesserHealPotion() ); }
					else if ( this.Name == "Algas da Vida") { from.AddToBackpack( new HealPotion() ); }
					else if ( this.Name == "Algas da Vida Maior") { from.AddToBackpack( new GreaterHealPotion() ); }
					else if ( this.Name == "Algas da Explosão Menor") { from.AddToBackpack( new LesserExplosionPotion() ); }
					else if ( this.Name == "Algas da Explosão") { from.AddToBackpack( new ExplosionPotion() ); }
					else if ( this.Name == "Algas da Explosão Maior") { from.AddToBackpack( new GreaterExplosionPotion() ); }
					else if ( this.Name == "Algas da Invisibilidade Menor") { from.AddToBackpack( new LesserInvisibilityPotion() ); }
					else if ( this.Name == "Algas da Invisibilidade") { from.AddToBackpack( new InvisibilityPotion() ); }
					else if ( this.Name == "Algas da Invisibilidade Maior") { from.AddToBackpack( new GreaterInvisibilityPotion() ); }
					else if ( this.Name == "Algas da Rejuvenescimento Menor") { from.AddToBackpack( new LesserRejuvenatePotion() ); }
					else if ( this.Name == "Algas de Rejuvenescimento") { from.AddToBackpack( new RejuvenatePotion() ); }
					else if ( this.Name == "Algas de Rejuvenescimento Maior") { from.AddToBackpack( new GreaterRejuvenatePotion() ); }
					else if ( this.Name == "Algas da Mana Menor") { from.AddToBackpack( new LesserManaPotion() ); }
					else if ( this.Name == "Algas da Mana") { from.AddToBackpack( new ManaPotion() ); }
					else if ( this.Name == "Algas da Mana Maior") { from.AddToBackpack( new GreaterManaPotion() ); }
					else if ( this.Name == "Algas da Invulnerabilidade") { from.AddToBackpack( new InvulnerabilityPotion() ); }

					from.SendMessage(55, "Você espreme o líquido e coloca na garrafa.");
					this.Consume();

					return;
				}
			}
			else
			{
				from.SendMessage(55, "Você não consegue obter nenhum líquido das algas.");
				this.Consume();
				return;
			}
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, ItemNameHue.UnifiedItemProps.SetColor("Tente espremer para extrair fluido", "#8be4fc"));
			list.Add( 1049644, ItemNameHue.UnifiedItemProps.SetColor("Precisa de uma garrafa vazia", "#ffe066")); // PARENTHESIS
        }

		public SpecialSeaweed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
            writer.Write( SkillNeeded );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            SkillNeeded = reader.ReadInt();
			ItemID = 0x0A96;
		}
	}
}