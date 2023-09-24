//Copied directly from AlchemyPouch.cs and modified for ore/ingots  Pretty sure the IsReagent check isn't gonna find ore/gem/ingots 
//because they aren't reagents, but I can't test it.

using System;
using Server;
using System.Collections.Generic;

namespace Server.Items
{
	[Flipable( 0x1C10, 0x1CC6 )]
    public class MinersPouch : LargeSack
    {
		public override int MaxWeight{ get{ return 800; } }

        [Constructable]
		public MinersPouch() : base()
		{
            Weight = 1.0;
			MaxItems = 8;
            //MaxWeight = 800;
            Name = "Bolsa Mágica de Minérios";
            Hue = Utility.RandomList(0x3bf, 1151, 1788, 1912, 1956, 2086, 2114, 2193, 2262);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {

            base.AddNameProperties(list);

            list.Add("Esta bolsa reduz o peso dos minérios e granitos pela metade.");
        }

        public override void Open(Mobile from)
        {
            double totalWeight = TotalItemWeights() * (0.5);
            if (totalWeight > (int)MaxWeight)
            {

                foreach (Item item in Items)
                {
                    from.AddToBackpack(item);
                    //item.Delete();
                    break;
                }
                from.SendMessage(55, "Você percebe que a bolsa está com o peso máximo suportado e remove algum item antes que ela rasgue.");
            }
            else
            {
                DisplayTo(from);
            }
        }

        public override bool OnDragDropInto( Mobile from, Item dropped, Point3D p )
        {
            int totalItems = TotalItems();
            int maxItems = MaxItems;
            double totalWeight = TotalItemWeights() * (0.5);
            int itemPlusBagWeight = (int)(totalWeight + ((dropped.Weight * dropped.Amount) * 0.5));
            //from.SendMessage(33, "Item+Bag : " + itemPlusBagWeight);
            if (itemPlusBagWeight > (int)MaxWeight)
            {
                from.SendMessage(55, "Adicionar este item na bolsa irá ultrapassar o peso máximo suportado.");
                return false;
            }
            else if (totalItems > maxItems)
            {
                from.SendMessage(55, "A bolsa já está cheia de itens.");
                return false;
            }
            else 
            {
/*                if (dropped is Container && !(dropped is MinersPouch))
                {
                    from.SendMessage("You can only use another miners rucksack within this sack.");
                    return false;
                }*/
                if (dropped is IronOre ||
                    dropped is DullCopperOre ||
                    dropped is CopperOre ||
                    dropped is BronzeOre ||
                    dropped is ShadowIronOre ||
                    dropped is PlatinumOre ||
                    dropped is GoldOre ||
                    dropped is AgapiteOre ||
                    dropped is VeriteOre ||
                    dropped is ValoriteOre ||
                    dropped is TitaniumOre ||
                    dropped is RoseniumOre ||
                    dropped is Granite ||
                    dropped is DullCopperGranite ||
                    dropped is CopperGranite ||
                    dropped is BronzeGranite ||
                    dropped is ShadowIronGranite ||
                    dropped is PlatinumGranite ||
                    dropped is GoldGranite ||
                    dropped is AgapiteGranite ||
                    dropped is VeriteGranite ||
                    dropped is ValoriteGranite ||
                    dropped is TitaniumGranite ||
                    dropped is RoseniumGranite)
                {
                    return base.OnDragDropInto(from, dropped, p);
                }
                else
                {
                    from.SendMessage(55, "Esta bolsa serve apenas para guardar minérios e granitos.");
                    return false;
                }
            }
        }

		public override bool OnDragDrop( Mobile from, Item dropped )
        {
            int totalItems = TotalItems();
            int maxItems = MaxItems;
            double totalWeight = TotalItemWeights() * (0.5);
            int itemPlusBagWeight = (int)(totalWeight + ((dropped.Weight * dropped.Amount) * 0.5));
            //from.SendMessage(35, "Item+Bag : " + itemPlusBagWeight);
            if (itemPlusBagWeight > (int)MaxWeight)
            {
                from.SendMessage(55, "Adicionar este item na bolsa irá ultrapassar o peso máximo suportado.");
                return false;
            }
            else if (totalItems > maxItems)
            {
                from.SendMessage(55, "A bolsa já está cheia de itens.");
                return false;
            }
            else 
            {
                /*                if (dropped is Container) //&& !(dropped is MinersPouch)
                                {
                                    from.SendMessage(55, "Esta bolsa serve apenas para guardar minérios.");
                                    return false;
                                }*/
                if (dropped is IronOre ||
                    dropped is DullCopperOre ||
                    dropped is CopperOre ||
                    dropped is BronzeOre ||
                    dropped is ShadowIronOre ||
                    dropped is PlatinumOre ||
                    dropped is GoldOre ||
                    dropped is AgapiteOre ||
                    dropped is VeriteOre ||
                    dropped is ValoriteOre ||
                    dropped is TitaniumOre ||
                    dropped is RoseniumOre ||
                    dropped is Granite ||
                    dropped is DullCopperGranite ||
                    dropped is CopperGranite ||
                    dropped is BronzeGranite ||
                    dropped is ShadowIronGranite ||
                    dropped is PlatinumGranite ||
                    dropped is GoldGranite ||
                    dropped is AgapiteGranite ||
                    dropped is VeriteGranite ||
                    dropped is ValoriteGranite ||
                    dropped is TitaniumGranite ||
                    dropped is RoseniumGranite)
                {
                    return base.OnDragDrop(from, dropped);
                }
                else
                {
                    from.SendMessage(55, "Esta bolsa serve apenas para guardar minérios e granitos.");
                    return false;
                }
            }
        }

		public MinersPouch( Serial serial ) : base( serial )
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

		public override int GetTotal(TotalType type)
        {
			if (type != TotalType.Weight)
				return base.GetTotal(type);
			else
			{
				return (int)(TotalItemWeights() * (0.5));
			}
        }

		public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            if (type != TotalType.Weight)
                base.UpdateTotal(sender, type, delta);
            else
                base.UpdateTotal(sender, type, (int)(delta * (0.5)));
        }

		private double TotalItemWeights()
        {
			double weight = 0.0;

			foreach (Item item in Items)
				weight += (item.Weight * (double)(item.Amount));

			return weight;
        }

        private int TotalItems()
        {
            int total = 1;

            foreach (Item item in Items)
                total += 1;

            return total;
        }
    }
}