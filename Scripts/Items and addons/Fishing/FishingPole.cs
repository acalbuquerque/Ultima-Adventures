using System;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Engines.Harvest;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public class FishingPole : BaseHarvestTool
	{
		public override HarvestSystem HarvestSystem{ get{ return Fishing.System; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
		public FishingPole() : this( 50 )
		{
		}

		[Constructable]
		public FishingPole( int uses ) : base( uses, 0x0DC0 )
		{
			Layer = Layer.OneHanded;
			Weight = 5.0;
		}

		public FishingPole( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
            list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor("Diga '.iniciar Auto-Pescar' para usar o sistema de automação.", "#8be4fc"));
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