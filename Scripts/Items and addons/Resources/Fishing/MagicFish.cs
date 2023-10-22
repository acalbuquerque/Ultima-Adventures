using System;
using System.Collections;

using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public enum MagicFishStatType
    {
        None = 0,
        Dex = 1,
        Str,
        Int
    }

    public enum MagicFishRegenerateType
    {
        None = 0,
        Hits = 1,
        Mana,
        Stam
    }

    public abstract class BaseMagicFish : Item
	{
        private bool m_revealProps;
        private Poison m_poison;
        private int m_regenerate;
        private int m_bonus;
        private bool m_invisible;
        private MagicFishStatType m_statType;
        private MagicFishRegenerateType m_regenerateType;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RevealProps
        {
            get { return m_revealProps; }
            set { m_revealProps = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison PoisonValue
        {
            get { return m_poison; }
            set { m_poison = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BonusValue
        {
            get { return m_bonus; }
            set { m_bonus = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MagicFishStatType StatType
        {
            get{return m_statType; }
            set{m_statType = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MagicFishRegenerateType RegenerateType
        {
            get { return m_regenerateType; }
            set { m_regenerateType = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoInvisible
        {
            get { return m_invisible; }
            set { m_invisible = value; InvalidateProperties(); }
        }

        public override double DefaultWeight
		{
			get { return 1.0; }
		}

		public BaseMagicFish( int hue) : base( 0xDD6 )
		{
			Hue = hue;
        }

		public BaseMagicFish( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (m_revealProps)
            {
                if (m_poison != null)
                {
                    string tipo = "";
                    switch (m_poison.Level)
                    {
                        case 0: tipo = "Fraco"; break;
                        case 1: tipo = "Regular"; break;
                        case 2: tipo = "Forte"; break;
                        case 3: tipo = "Mortal"; break;
                        case 4: tipo = "Letal"; break;
                    }
                    list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Veneno " + tipo, "#8be4fc"));
                }

                if (m_bonus > 0 && m_statType != MagicFishStatType.None)
                {
                    switch (m_statType)
                    {
                        case MagicFishStatType.Dex: list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("DEX +"+ m_bonus, "#8be4fc")); break;
                        case MagicFishStatType.Str: list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("STR +" + m_bonus, "#8be4fc")); break;
                        case MagicFishStatType.Int: list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("INT +" + m_bonus, "#8be4fc")); break;
                    }
                }

                if (m_regenerateType != MagicFishRegenerateType.None)
                {
                    int regen = 0;
                    switch (m_regenerateType)
                    {
                        case MagicFishRegenerateType.Mana:
                            list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Regenera Mana Parcialmente", "#8be4fc"));
                            break;
                        case MagicFishRegenerateType.Hits:
                            list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Regenera Vida Parcialmente", "#8be4fc"));
                            break;
                        case MagicFishRegenerateType.Stam:
                            list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Regenera Stamina Parcialmente", "#8be4fc"));
                            break;
                    }
                }

                if(m_invisible)
                    list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Invisibilidade Momentânea", "#8be4fc"));
            }
            else 
            {
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Um peixe com propriedades mágicas", "#8be4fc"));
                list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Para descobrir suas propriedades mágicas utilize o scanner de peixes.", "#ffe066"));
            }
        }

        public virtual bool Apply( Mobile from )
		{
            /*bool applied = Spells.SpellHelper.AddStatOffset( from, Type, Bonus, TimeSpan.FromMinutes( 1.0 ) );*/
            if (!from.CanBeginAction(typeof(BaseMagicFish)))
            {
                from.PrivateOverheadMessage(MessageType.Regular, 95, false, "* Tenho que aguardar para comer outro peixe mágico. *", from.NetState);
                return false;
            }
            else
            {
                new InternalTimer(this, from, TimeSpan.FromMinutes(1)).Start();
                from.BeginAction(typeof(BaseMagicFish));

                if (m_poison != null) 
                {
                    from.ApplyPoison(from, m_poison);
                }


                if (m_bonus > 0 && m_statType != MagicFishStatType.None) 
                {
                    switch (m_statType) 
                    {
                        case MagicFishStatType.Dex: from.RawDex += m_bonus; break;
                        case MagicFishStatType.Str: from.RawStr += m_bonus; break;
                        case MagicFishStatType.Int: from.RawInt += m_bonus; break;
                    }
                }

                if (m_regenerateType != MagicFishRegenerateType.None)
                {
                    int regen = 0;
                    switch (m_regenerateType)
                    {
                        case MagicFishRegenerateType.Mana:
                            regen= (int)(from.ManaMax - from.Mana); 
                            regen = (regen / 2 < 10) ? 10 : regen / 2; 
                            from.Mana += Utility.RandomMinMax(10, regen); 
                            break;
                        case MagicFishRegenerateType.Hits:
                            regen = (int)(from.HitsMax - from.Hits);
                            regen = (regen / 2 < 10) ? 10 : regen / 2;
                            from.Hits += Utility.RandomMinMax(10, regen);
                            break;
                        case MagicFishRegenerateType.Stam:
                            regen = (int)(from.StamMax - from.Stam);
                            regen = (regen / 2 < 10) ? 10 : regen / 2;
                            from.Stam += Utility.RandomMinMax(10, regen);
                            break;
                    }
                }

                from.Hidden = m_invisible;

                this.Amount--;
                if (this.Amount <= 0)
                    this.Delete();

                return true;
            }
		}

        public static void RemoveEffect(Mobile m, BaseMagicFish fish)
        {
            m.EndAction(typeof(BaseMagicFish));

            if (fish.BonusValue > 0 && fish.StatType != MagicFishStatType.None)
            {
                switch (fish.StatType)
                {

                    case MagicFishStatType.Dex: m.RawDex -= fish.BonusValue; break;
                    case MagicFishStatType.Str: m.RawStr -= fish.BonusValue; break;
                    case MagicFishStatType.Int: m.RawInt -= fish.BonusValue; break;
                }
            }

            if (fish.DoInvisible) 
            {
                m.Hidden = false;
            }
        }

        public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( Apply( from ) )
			{
				if ( from.Hunger < 20 )
				{
					from.Hunger += 3;
					// Send message to character about their current Hunger value

					if ( from.Body.IsHuman && !from.Mounted )
						from.Animate( 34, 5, 1, true, false, 0 );
				}

				from.FixedEffect( 0x375A, 10, 15 );
				from.PlaySound( 0x1E7 );
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501774 ); // You swallow the fish whole!
				//Delete();
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

            writer.WriteEncodedInt((int)m_bonus);
            writer.WriteEncodedInt((int)m_statType);
            writer.Write((bool)m_revealProps); // version
        }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            m_bonus = reader.ReadEncodedInt();
            m_statType = (MagicFishStatType)reader.ReadEncodedInt();
            m_revealProps =  reader.ReadBool();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_m;
            private DateTime m_Expire;
            private BaseMagicFish m_fish; 

            public InternalTimer(BaseMagicFish f, Mobile m, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
            {
                m_fish = f;
                m_m = m;
                m_Expire = DateTime.UtcNow + duration;
            }

            protected override void OnTick()
            {
                if (DateTime.UtcNow >= m_Expire)
                {
                    BaseMagicFish.RemoveEffect(m_m, m_fish);
                    Stop();
                }
            }
        }
    }

	public class PrizedFish : BaseMagicFish // RANDOM STAT MOD
	{
        private static int l_bonus;
        private static MagicFishStatType l_statType;

        [Constructable]
		public PrizedFish() : base(96)
		{
            Name = "peixe mágico";

            Array values = Enum.GetValues(typeof(MagicFishStatType));
            Random random = new Random();
            l_statType = (MagicFishStatType)values.GetValue(random.Next(1, values.Length));
            l_bonus = Utility.RandomMinMax(1, 1);

            StatType = l_statType;
            BonusValue = l_bonus;
        }

		public PrizedFish( Serial serial ) : base( serial )
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

	public class WondrousFish : BaseMagicFish
	{
        private static int l_bonus;
        private static MagicFishStatType l_statType;
        [Constructable]
		public WondrousFish() : base( 86 )
		{
            Name = "peixe mágico";
            Array values = Enum.GetValues(typeof(MagicFishStatType));
            Random random = new Random();
            l_statType = (MagicFishStatType)values.GetValue(random.Next(1, values.Length));
            l_bonus = Utility.RandomMinMax(2, 2);

            StatType = l_statType;
            BonusValue = l_bonus;
        }

		public WondrousFish( Serial serial ) : base( serial )
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

			if ( Hue == 286 )
				Hue = 86;
		} 
    }

	public class TrulyRareFish : BaseMagicFish
	{
        private static int l_bonus;
        private static MagicFishStatType l_statType;
        [Constructable]
		public TrulyRareFish() : base(1166)
		{
            Name = "peixe mágico";
            Array values = Enum.GetValues(typeof(MagicFishStatType));
            Random random = new Random();
            l_statType = (MagicFishStatType)values.GetValue(random.Next(1, values.Length));
            l_bonus = Utility.RandomMinMax(3, 5);

            StatType = l_statType;
            BonusValue = l_bonus;
        }

		public TrulyRareFish( Serial serial ) : base( serial )
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

    public class PeculiarFish : BaseMagicFish // SpiritSpeaak
    {
        //public override int LabelNumber { get { return 1041076; } } // highly peculiar fish

        [Constructable]
        public PeculiarFish() : base(210)
        {
            Name = "peixe mágico";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (this.RevealProps) 
            {
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Este peixe entende a lingua dos mortos", "#8be4fc"));
            }
            else
            {
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Um peixe exótico", "#8be4fc"));
                list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Para descobrir suas propriedades mágicas utilize o scanner de peixes.", "#ffe066"));
            }
        }

        public PeculiarFish(Serial serial) : base(serial)
        {
        }

        public override bool Apply(Mobile from)
        {
            if (!from.CanBeginAction(typeof(PeculiarFish)))
            {
                from.PrivateOverheadMessage(MessageType.Regular, 95, false, "* Tenho que aguardar para comer outro peixe mágico. *", from.NetState);
                return false;
            }
            else
            {
                from.CanHearGhosts = true;
                
                new InternalTimer(from, TimeSpan.FromMinutes(1)).Start();

                from.BeginAction(typeof(PeculiarFish));
                from.PrivateOverheadMessage(MessageType.Regular, 95, false, "* Cruzes! Estou ouvindo algo estranho!. *", from.NetState);
                from.PlaySound(from.Female ? 787 : 1058);
                from.PlaySound(1073);

                from.SendMessage(55, "Você começa a ouvir uma lingua estranha.");
                this.Amount--;
                if (this.Amount <= 0)
                    this.Delete();

                return from.CanHearGhosts;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((bool)RevealProps); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hue = 210;

            RevealProps = reader.ReadBool();
        }

        private static Hashtable m_Table = new Hashtable();

        public static bool HasEffect(Mobile m)
        {
            return (m_Table[m] != null);
        }

        public static void RemoveEffect(Mobile m)
        {
            m.SendMessage(55, "Você deixa de entender a lingua dos mortos.");
            m.CanHearGhosts = false;
            m.EndAction(typeof(PeculiarFish));
        }

        private class InternalTimer : Timer
        {
            private Mobile m_m;
            private DateTime m_Expire;

            public InternalTimer(Mobile m, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1))
            {
                m_m = m;
                m_Expire = DateTime.UtcNow + duration;

            }

            protected override void OnTick()
            {
                if (DateTime.UtcNow >= m_Expire)
                {
                    PeculiarFish.RemoveEffect(m_m);
                    Stop();
                }
            }
        }
    }

    public class InvisibleFish : BaseMagicFish
    {
		[Constructable]
		public InvisibleFish() : base(990)
		{
			Name = "peixe mágico";
            DoInvisible = true;

        }

		public InvisibleFish(Serial serial) : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            DoInvisible = true;
            Hue = 990;
        }
    }

    public class PoisonFish : BaseMagicFish
    {
        //public override int LabelNumber { get { return 1041076; } } // highly peculiar fish
        private int m_poison;
        [Constructable]
        public PoisonFish() : base(66)
        {
            Name = "peixe mágico";
            m_poison = Utility.RandomMinMax(1, 5);
            PoisonValue = HitPoison(m_poison);
        }

        public PoisonFish(Serial serial) : base(serial)
        {
        }

        public Poison HitPoison(int level)
        {
            if (level == 1) { return Poison.Lesser; }
            else if (level == 2) { return Poison.Regular; }
            else if (level == 3) { return Poison.Greater; }
            else if (level == 4) { return Poison.Deadly; }
            else if (level == 5) { return Poison.Lethal; }

            return null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.WriteEncodedInt((int)m_poison);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hue = 66;
            m_poison = reader.ReadEncodedInt();
            PoisonValue = HitPoison(m_poison);
        }
    }

    // REGENERATE FISH
    public class StaminaFish : BaseMagicFish
    {
        [Constructable]
        public StaminaFish() : base(133)
        {
            Name = "peixe mágico";

            RegenerateType = MagicFishRegenerateType.Stam; 
        }

        public StaminaFish(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hue = 133;
            RegenerateType = MagicFishRegenerateType.Stam;
        }
    }

    public class HealFish : BaseMagicFish
    {
        //public override int LabelNumber { get { return 1041076; } } // highly peculiar fish

        [Constructable]
        public HealFish() : base(1161)
        {
            Name = "peixe mágico";
            RegenerateType = MagicFishRegenerateType.Hits;
        }

        public HealFish(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            RegenerateType = MagicFishRegenerateType.Hits;
            Hue = 1161;
        }
    }

    public class ManaFish : BaseMagicFish
    {
        [Constructable]
        public ManaFish() : base(91)
        {
            Name = "peixe mágico";
            RegenerateType = MagicFishRegenerateType.Mana;
        }

        public ManaFish(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hue = 91;
            RegenerateType = MagicFishRegenerateType.Mana;
        }
    }

}