using System;
using Server;
using Server.Misc;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Felladrin.Automations;

namespace Server.Spells.Third
{
	public class MagicLockSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Magic Lock", "An Por",
				215,
				9001,
				Reagent.Garlic,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Third; } }

		public MagicLockSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( object o )
		{
			if ( o is Item )
			{
				Item targ = (Item)o;

				if ( targ is LockableContainer )
				{
					LockableContainer box = (LockableContainer)targ;
					if ( Multis.BaseHouse.CheckLockedDownOrSecured( box ) )
					{
                        // You cannot cast this on a locked down item.
                        Caster.PlaySound(Caster.Female ? 812 : 1086);
                        Caster.Say("*oops*");
                        Caster.SendMessage(95, "Você não pode lançar um feitiço em um baú seguro!");
                        //Caster.LocalOverheadMessage( MessageType.Regular, 0x22, 501761 );
					}
					else if ( box.Locked ||  box is ParagonChest || box is TreasureMapChest || box is PirateChest)
					{
                        // Target must be an unlocked chest.
                        Caster.PlaySound(Caster.Female ? 812 : 1086);
                        Caster.Say("*oops*");
                        Caster.SendMessage(95, "Você não pode lançar um feitiço em um baú já trancado.");
                        //Caster.SendLocalizedMessage( 501762 );
					}
					else if ( CheckSequence() )
					{
						SpellHelper.Turn( Caster, box );

						Point3D loc = box.GetWorldLocation();

						Effects.SendLocationParticles( EffectItem.Create( loc, box.Map, EffectItem.DefaultDuration ), 
							0x376A, 9, 32, Server.Items.CharacterDatabase.GetMySpellHue( Caster, 0 ), 0, 5020, 0 );

						Effects.PlaySound( loc, box.Map, 0x1FA );

                        // The chest is now locked!
                        Caster.SendMessage(2253, "O baú foi trancado com magia!");
                        Caster.PlaySound(Caster.Female ? 779 : 1050);
                        Caster.Say("*ah ha!*");
                        //Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501763 );

                        box.LockLevel = ((int)(Caster.Skills[SkillName.Magery].Value) >= 75) ? 75 : (int)(Caster.Skills[SkillName.Magery].Value);
						if ( box.LockLevel <= 0 ){ box.LockLevel = 0; }
						box.RequiredSkill = box.LockLevel;
                        box.MaxLockLevel = 120;
                        box.Locked = true;
                        //box.LockLevel = -255; // signal magic lock
                    }
				}
				else if ( targ is BaseDoor )
				{
					BaseDoor door = (BaseDoor)targ;
					if (Server.Items.DoorType.IsDungeonDoor(door))
					{
						if (door.Locked == true)
						{
							Caster.PlaySound(Caster.Female ? 812 : 1086);
							Caster.Say("*oops*");
							Caster.SendMessage(95, "Essa porta já está trancada!");
						}
						else
						{
							SpellHelper.Turn(Caster, door);

							Point3D loc = door.GetWorldLocation();

							Effects.SendLocationParticles(
								EffectItem.Create(loc, door.Map, EffectItem.DefaultDuration),
								0x376A, 9, 32, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, 5020, 0);

							Effects.PlaySound(loc, door.Map, 0x1FA);

							Caster.SendMessage(95, "Essa porta agora foi trancada com magia!");

							door.Locked = true;
							Server.Items.DoorType.LockDoors(door);

							new InternalTimer(door, Caster).Start();
						}
					}
					else 
					{
                        Caster.PlaySound(Caster.Female ? 812 : 1086);
                        Caster.Say("*oops*");
                        Caster.SendMessage(95, "Este feitiço não tem efeito sobre essa porta!");
                    }
				}
				else
				{
                    Caster.PlaySound(Caster.Female ? 812 : 1086);
                    Caster.Say("*oops*");
                    Caster.SendMessage(95, "Este feitiço não tem efeito sobre isso!");
                }
				
			}
			else if ( o is PlayerMobile )
			{
                Caster.CriminalAction(true);
                FailToCaptureTheSoul(95, "Esta alma é forte demais para ficar presa no frasco!");
            }
			else if ( o is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)o;

                if (Caster.Backpack.FindItemByType(typeof(ElectrumFlask)) == null)
                {
                    Caster.SendMessage(55, "Você precisa de um frasco de electrum vazio!");
                }
                else if ( !bc.Alive )
				{
                    FailToCaptureTheSoul(95, "Você não pode capturar algo que está morto!");
                }
				else if ( o is LockedCreature )
				{
                    FailToCaptureTheSoul(95, "Este ser já foi preso uma vez e não se deixará ser subjugado novamente!");
                }
				else if ( bc.Controlled )
				{
                    FailToCaptureTheSoul(95, "Este ser já está está sob o controle de outro!");
                }
				else if ( bc.Blessed || bc is CloneCharacterOnLogout.CharacterClone || (bc is BaseVendor && ((BaseVendor)bc).IsInvulnerable) || bc.IsHitchStabled)
				{
                    FailToCaptureTheSoul(95, "Este ser é protegido por uma aura misteriosa.");
                }
/*				else if ( (double)bc.Fame / 30000 > (double)Caster.Fame / 15000 )
				{
					Caster.SendMessage(95, "Você falhou em capturar alma deste ser por ele ter uma reputação elevada.");
				}*/
				else if ( bc.EmoteHue == 505 || bc.ControlSlots >= 100 ) // SUMMON QUEST AND QUEST MONSTERS
				{
                    FailToCaptureTheSoul(95, "Você não é capaz o suficiente para captura-lo!");
				}
				else if (bc is EpicCharacter || bc is TimeLord || bc is TownGuards)
				{
                    bc.Say("Você acha que pode capturar minha alma?");
                    bc.Say("Deixe-me mostrar o que posso fazer com você, seu inseto!");
                    Caster.PlaySound(Caster.Female ? 799 : 1071);
                    Caster.Say("*huh?*");
                    FailToCaptureTheSoul(55, "Você sente uma força poderosa bater em você!");
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        Caster.Kill();
                    });                   
				}
				else
				{
					int playerMagery = (int)(Caster.Skills[SkillName.Magery].Value);
                    int playerEval = (int)(Caster.Skills[SkillName.EvalInt].Value);
					int playerInt = (int)(Caster.RawInt);

                    Caster.CriminalAction(true); // this is a criminal action. Dude, you are stealing a soul...
                    
                    // This is a very tricky spell. The player must be a very strong wizzard/sorcerer.

                    if (playerMagery >= 100 && playerEval >= 100 && playerInt >= 100)
					{
						int requiredMana = 80;
						if (Caster.Mana > requiredMana) 
						{
                            double successPercentage = 0.0;
                            int random = (Utility.RandomMinMax(1, 100));
                            // percentage change of success == min 5% or max 20%
                            if (playerMagery >= 120 && playerEval >= 120) { successPercentage = 20; }
                            else if (playerMagery >= 110 && playerEval >= 110) { successPercentage = 10; }
                            else { successPercentage = 5; }

                            // success
                            if (successPercentage >= random)
                            {
                                Misc.Titles.AwardFame(Caster, (30), true); // ganha fama pela açao

                                // Consume the empty electrum flask
                                Item flask = Caster.Backpack.FindItemByType(typeof(ElectrumFlask));
                                if (flask != null) { flask.Consume(); }

                                // trap a human soul means kills
                                if (bc.Body == 400 || bc.Body == 401 || bc.Body == 605 || bc.Body == 606)
                                {
                                    Caster.Kills = Caster.Kills + 1;
                                    Caster.SendMessage(33, "Aprisionar uma alma humana significa assassinato!");
                                    Misc.Titles.AwardKarma(Caster, (-100), true); // ganha karma negativo
                                }

                                // Create the flask with the soul
                                ElectrumFlaskFilled flaskWithASoul = CreateCapturedSoulInAFlask(bc);

                                // Increase or Decrease fame/karma
                                //IncreaseOrDecreaseFameKarma(bc);

                                // Delete the original creature
                                bc.BoltEffect(0);
                                bc.PlaySound(0x665);
                                bc.Delete();

                                Caster.BoltEffect(0);
                                Caster.PlaySound(0x665);
								Caster.Mana -= requiredMana;
                                Caster.AddToBackpack(flaskWithASoul);
                                Caster.SendMessage(55, "Você capturou a alma de " + bc.Name + " em um frasco de electrum!");
                            }
                            else
                            {
								FailToCaptureTheSoul(95, "Você falha na tentativa de capturar a alma deste ser!");
                            }
                        }
						else 
						{
							FailToCaptureTheSoul(95, "Você não possui mana o suficiente!");
                        }
                    }
					else {
						FailToCaptureTheSoul(95, "Você não possui habilidades o suficiente para capturar essa alma.");
                    }
				}
			}

			FinishSequence();
		}
		// DEPRECATED
		private void IncreaseOrDecreaseFameKarma(BaseCreature bc) 
		{
			int fameWonLost = ((int)bc.Fame / 10); // + 10% of creature fame
            int karmaWonLost = ((int)bc.Karma / 10); // + 10% of creature Karma

			Caster.Fame += fameWonLost;
			Caster.Karma += karmaWonLost * -1;

            if (fameWonLost >= 0) { Caster.SendMessage(2253, "Você ganhou " + fameWonLost + " pontos de fama!"); } else { Caster.SendMessage(33, "Você perdeu " + fameWonLost + " pontos de fama!"); }
            if (karmaWonLost >= 0) { Caster.SendMessage(33, "Você ganhou " + karmaWonLost + " pontos de karma!"); } else { Caster.SendMessage(2253, "Você perdeu " + karmaWonLost * -1 + " pontos de karma!"); }
        }

		private static ElectrumFlaskFilled CreateCapturedSoulInAFlask(BaseCreature bc) 
		{
            int level = Server.Misc.IntelligentAction.GetCreatureLevel((Mobile)bc);

            ElectrumFlaskFilled bottle = new ElectrumFlaskFilled();

            int hitpoison = 0;

            if (bc.HitPoison == Poison.Lesser) { hitpoison = 1; }
            else if (bc.HitPoison == Poison.Regular) { hitpoison = 2; }
            else if (bc.HitPoison == Poison.Greater) { hitpoison = 3; }
            else if (bc.HitPoison == Poison.Deadly) { hitpoison = 4; }
            else if (bc.HitPoison == Poison.Lethal) { hitpoison = 5; }

            int immune = 0;

            if (bc.PoisonImmune == Poison.Lesser) { immune = 1; }
            else if (bc.PoisonImmune == Poison.Regular) { immune = 2; }
            else if (bc.PoisonImmune == Poison.Greater) { immune = 3; }
            else if (bc.PoisonImmune == Poison.Deadly) { immune = 4; }
            else if (bc.PoisonImmune == Poison.Lethal) { immune = 5; }

            bottle.TrappedName = bc.Name;
            bottle.TrappedTitle = bc.Title;
            bottle.TrappedHue = 2778; // branco/preto espectral // 0x9C4;
            bottle.TrappedSkills = level; // all knowledge are keeped in after life 
            bottle.TrappedAI = 2; if (bc.AI == AIType.AI_Mage) { bottle.TrappedAI = 1; }
            bottle.TrappedPoison = hitpoison;
            bottle.TrappedImmune = immune;
            bottle.TrappedCanSwim = bc.CanSwim;
            bottle.TrappedCantWalk = bc.CantWalk;
            bottle.TrappedAngerSound = bc.GetAngerSound();
            bottle.TrappedIdleSound = bc.GetIdleSound();
            bottle.TrappedDeathSound = bc.GetDeathSound();
            bottle.TrappedAttackSound = bc.GetAttackSound();
            bottle.TrappedHurtSound = bc.GetHurtSound();
            // a traped soul cannot have 100% of the creature stats. Lets make 80%.
            bottle.TrappedStr = (int)Math.Round(bc.RawStr * 0.8);
            bottle.TrappedDex = (int)Math.Round(bc.RawDex * 0.8);
            bottle.TrappedInt = (int)Math.Round(bc.RawInt * 0.8);
            bottle.TrappedHits = (int)Math.Round(bc.HitsMax * 0.8);             
			bottle.TrappedStam = (int)Math.Round(bc.StamMax * 0.8);
            bottle.TrappedMana = (int)Math.Round(bc.ManaMax * 0.8);
            bottle.TrappedDmgMin = (int)Math.Round(bc.DamageMin * 0.8);
            bottle.TrappedDmgMax = (int)Math.Round(bc.DamageMax * 0.8);
            bottle.TrappedColdDmg = (int)Math.Round(bc.ColdDamage * 0.8);
            bottle.TrappedEnergyDmg = (int)Math.Round(bc.EnergyDamage * 0.8);
            bottle.TrappedFireDmg = (int)Math.Round(bc.FireDamage * 0.8);
            bottle.TrappedPhysicalDmg = (int)Math.Round(bc.PhysicalDamage * 0.8);
            bottle.TrappedPoisonDmg = (int)Math.Round(bc.PoisonDamage * 0.8);
            bottle.TrappedColdRst = (int)Math.Round(bc.ColdResistSeed * 0.8);
            bottle.TrappedEnergyRst = (int)Math.Round(bc.EnergyResistSeed * 0.8);
            bottle.TrappedFireRst = (int)Math.Round(bc.FireResistSeed * 0.8);
            bottle.TrappedPhysicalRst = (int)Math.Round(bc.PhysicalResistanceSeed * 0.8);
            bottle.TrappedPoisonRst = (int)Math.Round(bc.PoisonResistSeed * 0.8);
            bottle.TrappedVirtualArmor = (int)Math.Round(bc.VirtualArmor * 0.8);

			// TURN HUMANOIDS TO GHOSTS SO I DON'T NEED TO WORRY ABOUT CLOTHES AND GEAR
			if (bc.Body == 400 || bc.Body == 401 || bc.Body == 605 || bc.Body == 606)
			{
				bottle.TrappedBody = 0x3CA;
				bottle.TrappedBaseSoundID = 0x482;
			}
            else  // keep original body form
            {
                bottle.TrappedBaseSoundID = bc.BaseSoundID; 
				bottle.TrappedBody = bc.Body; 
			}

			return bottle;
        }

        private void FailToCaptureTheSoul(int msgColor, String msg) 
		{
            Item flask = Caster.Backpack.FindItemByType(typeof(ElectrumFlask));
            if (flask != null) { flask.Consume(); Caster.SendMessage(33, "Um frasco de electrum foi destruido."); ; }

            Server.Misc.IntelligentAction.FizzleSpell(Caster);
            Caster.SendMessage(msgColor, msg);
        }

        private class InternalTarget : Target
		{
			private MagicLockSpell m_Owner;

			public InternalTarget( MagicLockSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				m_Owner.Target( o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}

		private class InternalTimer : Timer
		{
			private BaseDoor m_Door;

			public InternalTimer( BaseDoor door, Mobile caster ) : base( TimeSpan.FromSeconds( 0 ) )
			{
				double val = caster.Skills[SkillName.Magery].Value / 2.0;
				if ( val < 10 )
					val = 10;
				else if ( val > 60 )
					val = 60;

				m_Door = door;
				Delay = TimeSpan.FromSeconds( val );
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( m_Door.Locked == true )
				{
					m_Door.Locked = false;
					Server.Items.DoorType.UnlockDoors( m_Door );
					Effects.PlaySound( m_Door.Location, m_Door.Map, 0x3E4 );
				}
			}
		}
	}
}

namespace Server.Items
{
	public class ElectrumFlask : Item
	{
		[Constructable]
		public ElectrumFlask() : base( 0x282E )
		{
			Name = "frasco de electrum";
			Weight = 5.0;
            Hue = 2822;//2369
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage(55, "Esse frasco está vazio." );
			}
		}

		public ElectrumFlask(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
            int version = reader.ReadInt();
		}
	}


	public class ElectrumFlaskFilled : Item
	{
		[Constructable]
		public ElectrumFlaskFilled() : base( 0x282D )
		{
			Name = "frasco de electrum";
			Weight = 5.0;
			Hue = 2778;//2369
        }

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);

			string trapped;
			string prisoner;

			if ( TrappedBody > 0 )
			{
				trapped = "Contém uma alma presa";
				list.Add( 1070722, trapped );

				prisoner = TrappedName;
					if ( TrappedTitle != "" && TrappedTitle != null ){ prisoner = TrappedName + " " + TrappedTitle; }
			
				list.Add( 1049644, prisoner );
			}
        }

		public override void OnDoubleClick( Mobile from )
		{
			int nFollowers = from.FollowersMax - from.Followers;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage(95, "Isso deve estar em sua mochila para usar.");
			}
			else if ( nFollowers < 1 )
			{
				from.SendMessage(55, "Você já está controlando muitos capangas.");
			}
			else if ( HenchmanFunctions.IsInRestRegion( from ) == false )
			{
				Map map = from.Map;

				int magery = (int)(from.Skills[SkillName.Magery].Value);

				BaseCreature prisoner = new LockedCreature( this.TrappedAI, this.TrappedSkills, magery, this.TrappedHits, this.TrappedStam, this.TrappedMana, this.TrappedStr, this.TrappedDex, this.TrappedInt, this.TrappedPoison, this.TrappedImmune, this.TrappedAngerSound, this.TrappedIdleSound, this.TrappedDeathSound, this.TrappedAttackSound, this.TrappedHurtSound );

				bool validLocation = false;
				Point3D loc = from.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				prisoner.ControlMaster = from;
				prisoner.Controlled = true;
				prisoner.ControlOrder = OrderType.Come;

				prisoner.Name = this.TrappedName;
				prisoner.Title = this.TrappedTitle;
				prisoner.Body = this.TrappedBody;
				prisoner.BaseSoundID = this.TrappedBaseSoundID;
				prisoner.Hue = this.TrappedHue;
				prisoner.AI = AIType.AI_Mage; if ( this.TrappedAI == 2 ){ prisoner.AI = AIType.AI_Melee; }
				prisoner.DamageMin = this.TrappedDmgMin;
				prisoner.DamageMax = this.TrappedDmgMax;
				prisoner.ColdDamage = this.TrappedColdDmg;
				prisoner.EnergyDamage = this.TrappedEnergyDmg;
				prisoner.FireDamage = this.TrappedFireDmg;
				prisoner.PhysicalDamage = this.TrappedPhysicalDmg;
				prisoner.PoisonDamage = this.TrappedPoisonDmg;
				prisoner.ColdResistSeed = this.TrappedColdRst;
				prisoner.EnergyResistSeed = this.TrappedEnergyRst;
				prisoner.FireResistSeed = this.TrappedFireRst;
				prisoner.PhysicalResistanceSeed = this.TrappedPhysicalRst;
				prisoner.PoisonResistSeed = this.TrappedPoisonRst;
				prisoner.VirtualArmor = this.TrappedVirtualArmor;
				prisoner.CanSwim = this.TrappedCanSwim;
				prisoner.CantWalk = this.TrappedCantWalk;

				from.BoltEffect( 0 );
				from.PlaySound(0x665);
				from.PlaySound(0x03E);
				prisoner.MoveToWorld( loc, map );
				from.SendMessage(95, "Você quebra o frasco e libera " + prisoner.Name + "!" );
				this.Delete();
			}
			else
			{
				from.SendMessage(55, "Você não acha que seria uma boa ideia fazer isso aqui.");
			}
		}

		public ElectrumFlaskFilled(Serial serial) : base(serial)
		{
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( TrappedName );
			writer.Write( TrappedTitle );
			writer.Write( TrappedBody );
			writer.Write( TrappedBaseSoundID );
			writer.Write( TrappedHue );
			writer.Write( TrappedAI );
			writer.Write( TrappedStr );
			writer.Write( TrappedDex );
			writer.Write( TrappedInt );
			writer.Write( TrappedHits );
			writer.Write( TrappedStam );
			writer.Write( TrappedMana );
			writer.Write( TrappedDmgMin );
			writer.Write( TrappedDmgMax );
			writer.Write( TrappedColdDmg );
			writer.Write( TrappedEnergyDmg );
			writer.Write( TrappedFireDmg );
			writer.Write( TrappedPhysicalDmg );
			writer.Write( TrappedPoisonDmg );
			writer.Write( TrappedColdRst );
			writer.Write( TrappedEnergyRst );
			writer.Write( TrappedFireRst );
			writer.Write( TrappedPhysicalRst );
			writer.Write( TrappedPoisonRst );
			writer.Write( TrappedVirtualArmor );
			writer.Write( TrappedCanSwim );
			writer.Write( TrappedCantWalk );
			writer.Write( TrappedSkills );
			writer.Write( TrappedPoison );
			writer.Write( TrappedImmune );
			writer.Write( TrappedAngerSound );
			writer.Write( TrappedIdleSound );
			writer.Write( TrappedDeathSound );
			writer.Write( TrappedAttackSound );
			writer.Write( TrappedHurtSound );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
            int version = reader.ReadInt();
			TrappedName = reader.ReadString();
			TrappedTitle = reader.ReadString();
			TrappedBody = reader.ReadInt();
			TrappedBaseSoundID = reader.ReadInt();
			TrappedHue = reader.ReadInt();
			TrappedAI = reader.ReadInt();
			TrappedStr = reader.ReadInt();
			TrappedDex = reader.ReadInt();
			TrappedInt = reader.ReadInt();
			TrappedHits = reader.ReadInt();
			TrappedStam = reader.ReadInt();
			TrappedMana = reader.ReadInt();
			TrappedDmgMin = reader.ReadInt();
			TrappedDmgMax = reader.ReadInt();
			TrappedColdDmg = reader.ReadInt();
			TrappedEnergyDmg = reader.ReadInt();
			TrappedFireDmg = reader.ReadInt();
			TrappedPhysicalDmg = reader.ReadInt();
			TrappedPoisonDmg = reader.ReadInt();
			TrappedColdRst = reader.ReadInt();
			TrappedEnergyRst = reader.ReadInt();
			TrappedFireRst = reader.ReadInt();
			TrappedPhysicalRst = reader.ReadInt();
			TrappedPoisonRst = reader.ReadInt();
			TrappedVirtualArmor = reader.ReadInt();
			TrappedCanSwim = reader.ReadBool();
			TrappedCantWalk = reader.ReadBool();
			TrappedSkills = reader.ReadInt();
			TrappedPoison = reader.ReadInt();
			TrappedImmune = reader.ReadInt();
			TrappedAngerSound = reader.ReadInt();
			TrappedIdleSound = reader.ReadInt();
			TrappedDeathSound = reader.ReadInt();
			TrappedAttackSound = reader.ReadInt();
			TrappedHurtSound = reader.ReadInt();
		}

		public string TrappedName;
		public string TrappedTitle;
		public int TrappedBody;
		public int TrappedBaseSoundID;
		public int TrappedHue;
		public int TrappedAI; // 1 Mage, 2 Fighter
		public int TrappedStr;
		public int TrappedDex;
		public int TrappedInt;
		public int TrappedHits;
		public int TrappedStam;
		public int TrappedMana;
		public int TrappedDmgMin;
		public int TrappedDmgMax;
		public int TrappedColdDmg;
		public int TrappedEnergyDmg;
		public int TrappedFireDmg;
		public int TrappedPhysicalDmg;
		public int TrappedPoisonDmg;
		public int TrappedColdRst;
		public int TrappedEnergyRst;
		public int TrappedFireRst;
		public int TrappedPhysicalRst;
		public int TrappedPoisonRst;
		public int TrappedVirtualArmor;
		public bool TrappedCanSwim;
		public bool TrappedCantWalk;
		public int TrappedSkills;
		public int TrappedPoison;
		public int TrappedImmune;
		public int TrappedAngerSound;
		public int TrappedIdleSound;
		public int TrappedDeathSound;
		public int TrappedAttackSound;
		public int TrappedHurtSound;

		[CommandProperty(AccessLevel.Owner)]
		public string Trapped_Name { get { return TrappedName; } set { TrappedName = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Trapped_Title { get { return TrappedTitle; } set { TrappedTitle = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Body { get { return TrappedBody; } set { TrappedBody = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_BaseSoundID { get { return TrappedBaseSoundID; } set { TrappedBaseSoundID = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Hue { get { return TrappedHue; } set { TrappedHue = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_AI { get { return TrappedAI; } set { TrappedAI = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Str { get { return TrappedStr; } set { TrappedStr = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Dex { get { return TrappedDex; } set { TrappedDex = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Int { get { return TrappedInt; } set { TrappedInt = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Hits { get { return TrappedHits; } set { TrappedHits = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Stam { get { return TrappedStam; } set { TrappedStam = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Mana { get { return TrappedMana; } set { TrappedMana = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_DmgMin { get { return TrappedDmgMin; } set { TrappedDmgMin = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_DmgMax { get { return TrappedDmgMax; } set { TrappedDmgMax = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_ColdDmg { get { return TrappedColdDmg; } set { TrappedColdDmg = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_EnergyDmg { get { return TrappedEnergyDmg; } set { TrappedEnergyDmg = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_FireDmg { get { return TrappedFireDmg; } set { TrappedFireDmg = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_PhysicalDmg { get { return TrappedPhysicalDmg; } set { TrappedPhysicalDmg = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_PoisonDmg { get { return TrappedPoisonDmg; } set { TrappedPoisonDmg = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_ColdRst { get { return TrappedColdRst; } set { TrappedColdRst = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_EnergyRst { get { return TrappedEnergyRst; } set { TrappedEnergyRst = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_FireRst { get { return TrappedFireRst; } set { TrappedFireRst = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_PhysicalRst { get { return TrappedPhysicalRst; } set { TrappedPhysicalRst = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_PoisonRst { get { return TrappedPoisonRst; } set { TrappedPoisonRst = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_VirtualArmor { get { return TrappedVirtualArmor; } set { TrappedVirtualArmor = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public bool Trapped_CanSwim { get { return TrappedCanSwim; } set { TrappedCanSwim = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public bool Trapped_CantWalk { get { return TrappedCantWalk; } set { TrappedCantWalk = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Skills { get { return TrappedSkills; } set { TrappedSkills = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Poison { get { return TrappedPoison; } set { TrappedPoison = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_Immune { get { return TrappedImmune; } set { TrappedImmune = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_AngerSound { get { return TrappedAngerSound; } set { TrappedAngerSound = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_IdleSound { get { return TrappedIdleSound; } set { TrappedIdleSound = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_DeathSound { get { return TrappedDeathSound; } set { TrappedDeathSound = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_AttackSound { get { return TrappedAttackSound; } set { TrappedAttackSound = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Trapped_HurtSound { get { return TrappedHurtSound; } set { TrappedHurtSound = value; InvalidateProperties(); } }
	}
}

namespace Server.Mobiles
{
	[CorpseName( "corpo espiritual" )]
	public class LockedCreature : BaseCreature
	{
		public int BCPoison;
		public int BCImmune;
		public int BCAngerSound;
		public int BCIdleSound;
		public int BCDeathSound;
		public int BCAttackSound;
		public int BCHurtSound;

		public override bool DeleteCorpseOnDeath { get { return true; } }

		[Constructable]
		public LockedCreature( int job, int skills, int time, int maxhits, int maxstam, int maxmana, int str, int dex, int iq, int poison, int immune, int anger, int idle, int death, int attack, int hurt ): base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6 )
		{
			BCPoison = poison+0;
			BCImmune = immune+0;
			BCAngerSound = anger+0;
			BCIdleSound = idle+0;
			BCDeathSound = death+0;
			BCAttackSound = attack+0;
			BCHurtSound = hurt+0;

			Timer.DelayCall( TimeSpan.FromSeconds( (double)(10+(3*time)) ), new TimerCallback( Delete ) );

			Name = "um prisioneiro espiritual";
			Body = 2;

			SetStr( str );
			SetDex( dex);
			SetInt( iq );

			SetHits( maxhits );
			SetStam( maxstam );
			SetMana( maxmana );

			if ( job == 1 )
			{
				SetSkill( SkillName.EvalInt, (double)skills );
				SetSkill( SkillName.Magery, (double)skills );
				SetSkill( SkillName.Meditation, (double)skills );
				SetSkill( SkillName.MagicResist, (double)skills );
				SetSkill( SkillName.Wrestling, (double)skills );
			}
			else
			{
				SetSkill( SkillName.Anatomy, (double)skills );
				SetSkill( SkillName.MagicResist, (double)skills );
				SetSkill( SkillName.Tactics, (double)skills );
				SetSkill( SkillName.Wrestling, (double)skills );
			}

			Fame = 0;
			Karma = 0;

			ControlSlots = 3;
		}

		public override bool IsScaredOfScaryThings{ get{ return false; } }
		public override bool IsScaryToPets{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }

        public override int GetIdleSound(){ return BCIdleSound; }
        public override int GetAngerSound(){ return BCAngerSound; }
        public override int GetHurtSound(){ return BCHurtSound; }
        public override int GetDeathSound(){ return BCDeathSound; }
        public override int GetAttackSound(){ return BCAttackSound; }

		public override Poison PoisonImmune
		{
			get
			{
				if ( BCImmune == 1 ){ return Poison.Lesser; }
				else if ( BCImmune == 2 ){ return Poison.Regular; }
				else if ( BCImmune == 3 ){ return Poison.Greater; }
				else if ( BCImmune == 4 ){ return Poison.Deadly; }
				else if ( BCImmune == 5 ){ return Poison.Lethal; }

				return null;
			}
		}

		public override Poison HitPoison
		{
			get
			{
				if ( BCPoison == 1 ){ return Poison.Lesser; }
				else if ( BCPoison == 2 ){ return Poison.Regular; }
				else if ( BCPoison == 3 ){ return Poison.Greater; }
				else if ( BCPoison == 4 ){ return Poison.Deadly; }
				else if ( BCPoison == 5 ){ return Poison.Lethal; }

				return null;
			}
		}

		public LockedCreature( Serial serial ): base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            //Caster.SendMessage(55, "Seu prisioneiro espiritual sumiu do mundo fisico!");
            Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( Delete ) );
		}
	}
}