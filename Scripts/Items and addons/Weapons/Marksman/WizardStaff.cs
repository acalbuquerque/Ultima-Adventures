using Server;
using System;
using System.Collections;
using Server.Targeting;
using Server.Prompts;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
	public class WizardStaff : BaseWizardStaff
	{
		public override int EffectID
		{
			get
			{
				if ( damageType == 1 ){ return 0x4D17; } 		// Fire
				else if ( damageType == 2 ){ return 0x4D18; } 	// Cold
				else if ( damageType == 3 ){ return 0x3818; } 	// Energy
				else if ( damageType == 4 ){ return 0x4F49; } 	// Poison
				return 0x4F48;
			}
		}

		public override int DefHitSound
		{
			get
			{
				if ( damageType == 1 ){ return 0x15E; } 		// Fire
				else if ( damageType == 2 ){ return 0x650; } 	// Cold
				else if ( damageType == 3 ){ return 0x211; } 	// Energy
				else if ( damageType == 4 ){ return 0x658; } 	// Poison
				return 0x1E5;
			}
		}

		public override Type AmmoType{ get{ return typeof( MageEye ); } }

/*		public override WeaponAbility PrimaryAbility
		{
			get
			{
				if ( damageType == 1 ){ return WeaponAbility.ZapStamStrike; } 		// Fire
				else if ( damageType == 2 ){ return WeaponAbility.ZapDexStrike; } 	// Cold
				else if ( damageType == 3 ){ return WeaponAbility.ZapIntStrike; } 	// Energy
				else if ( damageType == 4 ){ return WeaponAbility.ZapStrStrike;} 	// Poison
				return WeaponAbility.ZapManaStrike;
			}
		}*/
/*		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MagicProtection; } }
		public override WeaponAbility ThirdAbility{ get{ return WeaponAbility.ElementalStrike; } }
		public override WeaponAbility FourthAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility FifthAbility{ get{ return WeaponAbility.MagicProtection2; } }*/

		public override int AosStrengthReq{ get{ return 20; } }
		public override int AosMinDamage{ get{ return Core.ML ? 15 : 16; } }
		public override int AosMaxDamage{ get{ return Core.ML ? 25 : 24; } }
		public override int AosSpeed{ get{ return 25; } }
		public override float MlSpeed{ get{ return 3.00f; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 41; } }
		public override int OldSpeed{ get{ return 20; } }

		public override int DefMaxRange{ get{ return 10; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 90; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce2H; } }

		[Constructable]
		public WizardStaff() : base( 0x0908 )
		{
			Name = "bastão de mago";
			Weight = 6.0;
			Layer = Layer.TwoHanded;
			Hue = 0xB3A;// Utility.RandomDyedHue();

        }

		public WizardStaff( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( damageType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			damageType = reader.ReadInt();
		}
	}

	public class WizardStick : BaseWizardStaff
	{
        public override int EffectID
		{
			get
			{
				if ( damageType == 1 ){ return 0x4D17; } 		// Fire
				else if ( damageType == 2 ){ return 0x4D18; } 	// Cold
				else if ( damageType == 3 ){ return 0x3818; } 	// Energy
				else if ( damageType == 4 ){ return 0x4F49; } 	// Poison
				return 0x4F48;
			}
		}

		public override int DefHitSound
		{
			get
			{
				if ( damageType == 1 ){ return 0x15E; } 		// Fire
				else if ( damageType == 2 ){ return 0x650; } 	// Cold
				else if ( damageType == 3 ){ return 0x211; } 	// Energy
				else if ( damageType == 4 ){ return 0x658; } 	// Poison
				return 0x1E5;
			}
		}

		public override Type AmmoType{ get{ return typeof( MageEye ); } }

		public override WeaponAbility PrimaryAbility
		{
			get
			{
				if (damageType == 1) { return WeaponAbility.ZapStamStrike; }        // Fire
				else if (damageType == 2) { return WeaponAbility.ZapDexStrike; }    // Cold
				else if (damageType == 3) { return WeaponAbility.ZapIntStrike; }    // Energy
				else if (damageType == 4) { return WeaponAbility.ZapStrStrike; }    // Poison
				return null;//WeaponAbility.ZapManaStrike;
			}
		}
		public override WeaponAbility SecondaryAbility { get { return null;/*return WeaponAbility.MagicProtection;*/ } }
		public override WeaponAbility ThirdAbility { get { return null;  /*return WeaponAbility.ElementalStrike;*/ } }
		public override WeaponAbility FourthAbility { get { return null;  /*return WeaponAbility.ArmorIgnore;*/ } }
		public override WeaponAbility FifthAbility { get { return null;  /*return WeaponAbility.MagicProtection2;*/ } }

		public override int AosStrengthReq{ get{ return 15; } }
		public override int AosMinDamage{ get{ return Core.ML ? 11 : 16; } }
		public override int AosMaxDamage{ get{ return Core.ML ? 19 : 24; } }
		public override int AosSpeed{ get{ return 25; } }
		public override float MlSpeed{ get{ return 4.00f; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 41; } }
		public override int OldSpeed{ get{ return 20; } }

		public override int DefMaxRange{ get{ return 8; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 90; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Bash1H; } }

		[Constructable]
		public WizardStick() : base( 0xDF2 )
		{
			Name = "cetro mágico";
			Weight = 3.0;
			Layer = Layer.OneHanded;
            Attributes.LowerManaCost = 3;
            Attributes.LowerRegCost = 5;
        }

		public WizardStick( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            string subtitle = "Este é um item de curto alcance";
            list.Add(1070722, subtitle);
        }

        public override bool OnEquip(Mobile from)
		{
            //from.SendMessage(55, "Este é um item de curto alcance!");
            return base.OnEquip(from);
        }


        public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( damageType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			damageType = reader.ReadInt();
		}
	}

	public abstract class BaseWizardStaff : BaseMeleeWeapon
	{
		public int damageType;
		[CommandProperty(AccessLevel.Owner)]
		public int damage_Type { get { return damageType; } set { damageType = value; InvalidateProperties(); } }

		//public abstract int PhysicalHitSound { get; }
		public abstract int EffectID{ get; }
		public abstract Type AmmoType{ get; }

		public override int DefHitSound{ get{ return 0x54A; } }
		public override int DefMissSound{ get{ return 0x4BB; } }

		public override SkillName DefSkill{ get{ return SkillName.Magery; } }
		public override WeaponType DefType{ get{ return WeaponType.Ranged; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce2H; } }

		public override SkillName AccuracySkill{ get{ return SkillName.Magery; } }

		public BaseWizardStaff( int itemID ) : base( itemID )
		{
			damageType = 0;
			Attributes.SpellChanneling = 1;
        }

		public override void OnLocationChange( Point3D oldLocation )
		{
			EnergyType();
			Server.Items.Pitchfork.IronColor( this );
		}

		public override void OnDoubleClick( Mobile from )
		{
			Target t;

			if ( !IsChildOf( from.Backpack ) && !(Parent == from) )
			{
				from.SendMessage(55, "Este item precisa estar sob sua posse para usa-lo.");
			}
			else
			{
				from.SendMessage( 55, "Quais gemas você deseja utilizar?" );
				t = new GemTarget();
				from.Target = t;
			}
		}

		public override bool OnEquip( Mobile from )
		{
			//int necro = (int)(from.Skills[SkillName.Necromancy].Base);
			//int mages = (int)(from.Skills[SkillName.Magery].Base);

			/*string job = "mage";
				if ( necro > mages ){ job = "necromancer"; }

			if ( necro < 30 && mages < 30 )
			{
				from.SendMessage ("You are not a powerful enough " + job + " to use this!");
				return false;
			}*/

			from.SendMessage( 55, "Você precisa de cristais de olhos mágicos para alimentar este item e você pode transformar gemas nisso." );
			return base.OnEquip( from );
		}

/*        public override double GetBaseDamage(Mobile attacker)
        {
            double damage = base.GetBaseDamage(attacker);
            attacker.SendMessage("BaseDamage - " + damage);
            if (!Core.AOS && (attacker.Player || attacker.Body.IsHuman) && Layer == Layer.TwoHanded && (attacker.Skills[SkillName.Anatomy].Value / 400.0) >= Utility.RandomDouble())
            {
                damage *= 1.5;

                attacker.SendMessage("You deliver a crushing blow!"); // Is this not localized?
                attacker.PlaySound(0x11C);
            }
            attacker.SendMessage("BaseDamage - " + damage);
            return damage;
        }*/

        private class GemTarget : Target
		{
			public GemTarget() : base( 1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				Item iGem = targeted as Item;

				if ( iGem is StarSapphire || iGem is Emerald || iGem is Sapphire || iGem is Ruby || iGem is Citrine || iGem is Amethyst || iGem is Tourmaline || iGem is Amber || iGem is Diamond )
				{
					if ( !iGem.IsChildOf( from.Backpack ) )
					{
						from.SendMessage( 55, "Você só pode transformar gemas em sua mochila." );
					}
					else
					{
						int amount = 4;

						if (iGem is Diamond) { amount = iGem.Amount * 65; }
						else if (iGem is StarSapphire) { amount = iGem.Amount * 50; }
						else if (iGem is Emerald || iGem is Sapphire || iGem is Ruby) { amount = iGem.Amount * 40; }
						else if (iGem is Citrine || iGem is Amethyst || iGem is Tourmaline) { amount = iGem.Amount * 30; }
						else if (iGem is Amber) { amount = iGem.Amount * 20; }
						else { amount = iGem.Amount * 10;  }
						
						amount = (int)(amount/4);

						from.RevealingAction();
						from.PlaySound( 0x243 );
						from.AddToBackpack( new MageEye(amount) );
						from.SendMessage( 55, "Você transforma as gemas em olhos de mago." );
						iGem.Delete();
					}
				}
				else
				{
					from.SendMessage( 55, "Este objeto só pode transformar certas gemas." );
				}
			}
		}

		public void EnergyType()
		{
			int physical = 100 - AosElementDamages.Fire - AosElementDamages.Cold - AosElementDamages.Energy - AosElementDamages.Poison;
            //from.SendMessage(35, AosElementDamages.Fire + "-" + AosElementDamages.Cold + "-" + AosElementDamages.Energy + "-" + AosElementDamages.Poison);
            damageType = 0;
			if ( AosElementDamages.Fire > AosElementDamages.Cold && AosElementDamages.Fire > AosElementDamages.Poison && AosElementDamages.Fire > AosElementDamages.Energy && AosElementDamages.Fire > physical ){ damageType = 1; }
			else if ( AosElementDamages.Cold > AosElementDamages.Fire && AosElementDamages.Cold > AosElementDamages.Poison && AosElementDamages.Cold > AosElementDamages.Energy && AosElementDamages.Cold > physical ){ damageType = 2; }
			else if ( AosElementDamages.Energy > AosElementDamages.Cold && AosElementDamages.Energy > AosElementDamages.Fire && AosElementDamages.Energy > AosElementDamages.Poison && AosElementDamages.Energy > physical ){ damageType = 3; }
			else if ( AosElementDamages.Poison > AosElementDamages.Fire && AosElementDamages.Poison > AosElementDamages.Cold && AosElementDamages.Poison > AosElementDamages.Energy && AosElementDamages.Poison > physical ){ damageType = 4; }
		}

		public BaseWizardStaff( Serial serial ) : base( serial )
		{
		}

		public override TimeSpan OnSwing( Mobile attacker, Mobile defender )
		{
			WeaponAbility a = WeaponAbility.GetCurrentAbility( attacker );

			// Make sure we've been standing still for .25/.5/1 second depending on Era
			if (Core.TickCount > (attacker.LastMoveTime + (Core.SE ? 250 : (Core.AOS ? 500 : 1000) )) || (Core.AOS && WeaponAbility.GetCurrentAbility( attacker ) is MovingShot) )
			{
				bool canSwing = true;

				if ( Core.AOS )
				{
					canSwing = ( !attacker.Paralyzed && !attacker.Frozen );

					if ( canSwing )
					{
						Spell sp = attacker.Spell as Spell;

						canSwing = ( sp == null || !sp.IsCasting || !sp.BlocksMovement );
					}
				}

				if ( canSwing && attacker.HarmfulCheck( defender ) )
				{
					attacker.DisruptiveAction();
					attacker.Send( new Swing( 0, attacker, defender ) );
                    
                    if (OnFired(attacker, defender))
					{
						if (CheckHit(attacker, defender))
							OnHit( attacker, defender );
						else
							OnMiss( attacker, defender );
					}
					else {
                        attacker.SendMessage(55, "Este objeto não está carregado para causar de dano físico!");
                        attacker.PlaySound(73); // TODO achar um som melhor ?
						//InitMaxHits -= 2;
                    }
				}

				attacker.RevealingAction();

				return GetDelay( attacker );
			}
			else
			{
				attacker.RevealingAction();

				return TimeSpan.FromSeconds( 0.25 );
			}
		}

		public static bool HasStaff( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.OneHanded ) != null )
			{
				Item oneHand = from.FindItemOnLayer( Layer.OneHanded );
				if ( oneHand is BaseWizardStaff || oneHand is BaseLevelStave || oneHand is BaseGiftStave ){ return true; }
			}
			if ( from.FindItemOnLayer( Layer.TwoHanded ) != null )
			{
				Item twoHand = from.FindItemOnLayer( Layer.TwoHanded );
				if ( twoHand is BaseWizardStaff || twoHand is BaseLevelStave || twoHand is BaseGiftStave ){ return true; }
			}
			if ( from.Backpack.FindItemByType( typeof ( WizardStaff ) ) != null )
			{
				return true;
			}
			else if ( from.Backpack.FindItemByType( typeof ( WizardStick ) ) != null )
			{
				return true;
			}
			else if ( from.Backpack.FindItemByType( typeof ( LevelStave ) ) != null )
			{
				return true;
			}
			else if ( from.Backpack.FindItemByType( typeof ( LevelSceptre ) ) != null )
			{
				return true;
			}
			else if ( from.Backpack.FindItemByType( typeof ( GiftStave ) ) != null )
			{
				return true;
			}
			else if ( from.Backpack.FindItemByType( typeof ( GiftSceptre ) ) != null )
			{
				return true;
			}

			return false;
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
            //attacker.SendMessage(55, "OnHit trigged - " + damageBonus);
            base.OnHit( attacker, defender, damageBonus );
		}

		public override void OnMiss( Mobile attacker, Mobile defender )
		{
            base.OnMiss( attacker, defender );
		}

		public virtual bool OnFired( Mobile attacker, Mobile defender )
		{
            BaseQuiver quiver = attacker.FindItemOnLayer( Layer.Cloak ) as BaseQuiver;
			Container pack = attacker.Backpack;

            if ( attacker.Player )
			{
				if ( quiver == null || quiver.LowerAmmoCost == 0 || quiver.LowerAmmoCost > Utility.Random( 100 ) )
				{
					if (quiver != null && quiver.ConsumeTotal(AmmoType, 1))
					{
						quiver.InvalidateWeight();
					}
					else if (pack == null || !pack.ConsumeTotal(AmmoType, 1)) // do not cause physical damage
					{
						return false;
					}
				}
			}

			attacker.MovingEffect( defender, EffectID, 18, 1, false, false );

			return true;
		}

		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize( GenericReader reader ) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}

	public class MageEye : Item
	{
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public MageEye() : this( 1 )
		{
		}

		[Constructable]
		public MageEye( int amount ) : base( 0xF19 )
		{
			Hue = 0xB78;
			Name = "olhos de mago";
			Stackable = true;
			Amount = amount;
			Light = LightType.Circle150;
		}

		public MageEye( Serial serial ) : base( serial )
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
}