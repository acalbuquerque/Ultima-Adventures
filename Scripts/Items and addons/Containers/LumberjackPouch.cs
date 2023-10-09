//Copied directly from AlchemyPouch.cs and modified for ore/ingots  Pretty sure the IsReagent check isn't gonna find logs/boards 
//because they aren't reagents, but I can't test it.

using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x1C10, 0x1CC6 )]
    public class LumberjackPouch : LargeSack
    {
		public override int MaxWeight{ get{ return 800; } }
		
		[Constructable]
		public LumberjackPouch() : base()
		{
            Weight = 1.0;
            MaxItems = 8;
            Name = "Bolsa mágica de madeiras";
            Hue = Utility.RandomList(0x3bf, 1151, 1788, 1912, 1956, 2086, 2114, 2193, 2262);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {

            base.AddNameProperties(list);

            list.Add("Esta bolsa reduz o peso das toras de madeiras pela metade.");
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
            if ( addItems(from, dropped) ) 
            {
                Open(from);
                return base.OnDragDropInto(from, dropped, p);
            }

            return false;
        }

		public override bool OnDragDrop( Mobile from, Item dropped )
        {
            if (addItems(from, dropped)) 
            {
                Open(from);
                return base.OnDragDrop(from, dropped);
            }

            return false;
        }

		private bool addItems(Mobile from, Item dropped) 
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
                if (dropped is Log ||
                        dropped is AshLog ||
                        dropped is CherryLog ||
                        dropped is EbonyLog ||
                        dropped is GoldenOakLog ||
                        dropped is HickoryLog ||
                        /*dropped is MahoganyLog ||
						dropped is OakLog ||
						dropped is PineLog ||*/
                        dropped is RosewoodLog ||
                        /*dropped is WalnutLog ||
						dropped is DriftwoodLog ||
						dropped is GhostLog ||
						dropped is PetrifiedLog ||*/
                        dropped is ElvenLog)
                {
                    return true; 
                }
                else
                {
                    from.SendMessage(55, "Esta bolsa serve apenas para guardar toras de madeira.");
                    return false;
                }
            }
        }

		public LumberjackPouch( Serial serial ) : base( serial )
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
			Weight = 1.0;
			MaxItems = 8;
            Name = "Bolsa mágica de madeiras";
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