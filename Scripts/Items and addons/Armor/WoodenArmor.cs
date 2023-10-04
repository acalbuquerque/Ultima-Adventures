// edited by The Mighty MIB HUNTER 
using System;
using Server;
using Server.Misc;
using Server.Items;
namespace Server.Items
{
	public class WoodenPlateLegs : BaseArmor ///////////////////////////////////////////////////////
    {
		public override int BasePhysicalResistance { get { return 8; } }
		public override int BaseFireResistance { get { return 1; } }
		public override int BaseColdResistance { get { return 6; } }
		public override int BasePoisonResistance { get { return 3; } }
		public override int BaseEnergyResistance { get { return 8; } }

		public override int InitMinHits { get { return 40; } }
		public override int InitMaxHits { get { return 55; } }

		public override int AosStrReq { get { return 70; } }

		public override int OldStrReq { get { return 60; } }
		public override int OldDexBonus { get { return -6; } }

		public override int ArmorBase { get { return 30; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularWood; } }

		[Constructable]
		public WoodenPlateLegs() : base(0x1411)
		{
			Name = "calça de madeira";
			Hue = 0x840;
			Weight = 5.0;
			//Layer = Layer.Pants;
		}

		public WoodenPlateLegs( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1411;
		}
	}
	public class WoodenPlateGloves : BaseArmor ///////////////////////////////////////////////////
    {
		public override int BasePhysicalResistance{ get{ return 6; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 3; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 6; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 55; } }

		public override int AosStrReq{ get{ return 50; } }
		public override int OldStrReq{ get{ return 30; } }

		public override int OldDexBonus{ get{ return -2; } }

		public override int ArmorBase{ get{ return 30; } }
		
		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularWood; } }

		[Constructable]
		public WoodenPlateGloves() : base(0x1414)
		{
			Name = "manoplas de madeira";
			Hue = 0x840;
			Weight = 1.0;
			Layer = Layer.Gloves;
		}

		public WoodenPlateGloves( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1414;
		}
	}
	public class WoodenPlateGorget : BaseArmor ///////////////////////////////////////////////////
    {
		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 4; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 55; } }

		public override int AosStrReq{ get{ return 25; } }
		public override int OldStrReq{ get{ return 30; } }

		public override int OldDexBonus{ get{ return -1; } }

		public override int ArmorBase{ get{ return 30; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularWood; } }
        
		[Constructable]
		public WoodenPlateGorget() : base(0x1413)
		{
			Name = "gorgel de madeira";
			Hue = 0x840;
			Weight = 1.0;
			Layer = Layer.Neck;
		}

		public WoodenPlateGorget( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1413;
		}
	}
	public class WoodenPlateArms : BaseArmor ///////////////////////////////////////////////////////
	{
		public override int BasePhysicalResistance{ get{ return 9; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 6; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 6; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 55; } }

		public override int AosStrReq{ get{ return 60; } }
		public override int OldStrReq{ get{ return 40; } }

		public override int OldDexBonus{ get{ return -2; } }

		public override int ArmorBase{ get{ return 30; } }
		
		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularWood; } }

		[Constructable]
		public WoodenPlateArms() : base(0x1410)
		{
			Name = "ombreiras de madeira";
			Hue = 0x840;
			Weight = 8.0;
			Layer = Layer.Arms;
		}

		public WoodenPlateArms( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1410;
		}
	}
	public class WoodenPlateChest : BaseArmor /////////////////////////////////////////////////////
	{
		public override int BasePhysicalResistance{ get{ return 11; } }
		public override int BaseFireResistance{ get{ return 2; } }
		public override int BaseColdResistance{ get{ return 6; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 7; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 55; } }

		public override int AosStrReq{ get{ return 75; } }
		public override int OldStrReq{ get{ return 60; } }

		public override int OldDexBonus{ get{ return -8; } }

		public override int ArmorBase{ get{ return 30; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularWood; } }

		[Constructable]
		public WoodenPlateChest() : base(0x1415)
		{
			Name = "peitoral de madeira";
			Hue = 0x840;
			Weight = 8.0;
			Layer = Layer.InnerTorso;
		}

		public WoodenPlateChest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1415;
		}
	}
	public class WoodenPlateHelm : BaseArmor ///////////////////////////////////////////////////////
    {
		public override int BasePhysicalResistance{ get{ return 9; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 5; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 6; } }
		
		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 55; } }

		public override int AosStrReq{ get{ return 60; } }
		public override int OldStrReq{ get{ return 40; } }

		public override int OldDexBonus{ get{ return -1; } }

		public override int ArmorBase{ get{ return 30; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularWood; } }

		[Constructable]
		public WoodenPlateHelm() : base(0x1412)
		{
			Name = "elmo de madeira";
			Hue = 0x840;
			Weight = 1.0;
			Layer = Layer.Helm;
		}

		public WoodenPlateHelm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			ItemID = 0x1412;
		}
	}
}
