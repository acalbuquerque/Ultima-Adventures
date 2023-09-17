/*
 * 
 * By Gargouille
 * Date: 07/06/2014
 * 
 * 
 */

/// <summary>
/// The MineSpirit localize the spot on the map, with a point and a range
/// This is the easiest way for GameMasters to place mines, more than creating regions
/// I make it with a mob rather than an item, just to have access to Mobile.GetDistanceToSqrt in DynamicMining.cs
/// </summary>

using System;
using Server.Items;
using Server.Engines.Harvest;
using Server.DeepMine;
namespace Server.Mobiles
{
	public class MineSpirit : StaticElemental
    {
		//change it for the base class of ore if custom
		private static Type m_BaseType = typeof(BaseOre);
		
		[Constructable]
		public MineSpirit( )
		{
			//Not a real ingame mobile
/*			Hidden = true;
			Frozen = true;
			CantWalk = true;
			Blessed = true;*/
		}

        private HarvestSystem m_HarvestSystem;
		public HarvestSystem HarvestSystem
		{
			get
			{
				if(m_HarvestSystem==null)
				{
                    HarvestResource[] res = new HarvestResource[]
					{
						new HarvestResource( 00.0, 00.0, 100.0, "Você encontrou alguns minérios de ferro.", typeof(IronOre ), typeof(Granite ) )
					};

                    DynamicMining system = new DynamicMining();
					
					system.Ore.Resources = new HarvestResource[]
					{
						new HarvestResource( m_ReqSkill, m_MinSkill, m_MaxSkill, m_HarvestMessage, m_OreType )
					};
					
					system.Ore.Veins = new HarvestVein[]
					{
						new HarvestVein( 50, 0.5, system.Ore.Resources[0], res[0]),
                        new HarvestVein( 50, 0.0, res[0], null) // iron
                    };
					
					m_HarvestSystem = system;
				}
				return m_HarvestSystem;
				
			}
		}

        private string m_HarvestMessage
		{
			get
			{
				//can use cliloc cf 1007072 if ores not custom
				//string ore = m_OreType.FullName;//use substring, my customs ore have a getname method
				string ore = m_OreType.Name.Substring(0, m_OreType.Name.Length - 3);//CraftResources.GetName(m_OreType.m_Resource);//FormatName.ToOre(m_OreType.FullName);
                return "Você encontrou alguns minérios de "+ore+ " e colocou em sua mochila.";
			}
		}
		
		#region Props
		private int m_Range=3;
		[CommandProperty(AccessLevel.Administrator)]
		public int Range {
			get { return m_Range; }
			set { m_Range = value; m_Range = Math.Max(0,m_Range);m_Range=Math.Min(3,m_Range);}
		}
		
		private Type m_OreType = m_BaseType;
		[CommandProperty(AccessLevel.Administrator)]
		public Type OreType
		{
			get { return m_OreType; }
			set { Type t = value; if(m_BaseType.IsAssignableFrom(t)){m_OreType = t; m_HarvestSystem=null;}}
		}
		
		private double m_ReqSkill = 50;
		[CommandProperty(AccessLevel.Administrator)]
		public double ReqSkill
		{
			get { return m_ReqSkill; }
			set { m_ReqSkill = value; m_ReqSkill=Math.Max(0,m_ReqSkill);m_ReqSkill=Math.Min(120,m_ReqSkill);m_HarvestSystem=null;}
		}
		
		private double m_MinSkill = 50;
		[CommandProperty(AccessLevel.Administrator)]
		public double MinSkill
		{
			get { return m_MinSkill; }
			set { m_MinSkill = value; m_MinSkill=Math.Max(0,m_MinSkill);m_MinSkill=Math.Min(120,m_MinSkill);m_HarvestSystem=null;}
		}
		
		private double m_MaxSkill = 120;
		[CommandProperty(AccessLevel.Administrator)]
		public double MaxSkill
		{
			get { return m_MaxSkill; }
			set { m_MaxSkill = value; m_MaxSkill=Math.Max(0,m_MaxSkill);m_MaxSkill=Math.Min(120,m_MaxSkill);m_HarvestSystem=null;}
		}
		#endregion
		
		#region Serial
		public MineSpirit( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version

			writer.Write( (int) m_Range );
			
			writer.Write( (string) m_OreType.FullName );
			
			writer.Write( (double) m_MinSkill );
			
			writer.Write( (double) m_MaxSkill );
			
			writer.Write( (double) m_ReqSkill );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Range = reader.ReadInt();
			
			Type t = ScriptCompiler.FindTypeByFullName(reader.ReadString());
			if(t!=null && m_BaseType.IsAssignableFrom(t))
				m_OreType = t;
			
			m_MinSkill = reader.ReadDouble();
			
			m_MaxSkill = reader.ReadDouble();
			
			m_ReqSkill = reader.ReadDouble();
		}
		
		
		#endregion
	}
}
