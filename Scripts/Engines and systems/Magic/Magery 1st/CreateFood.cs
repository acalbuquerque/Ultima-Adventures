using System;
using Server.Items;

namespace Server.Spells.First
{
	public class CreateFoodSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Create Food", "In Mani Ylem",
				224,
				9011,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public CreateFoodSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		private static FoodInfo[] m_Food = new FoodInfo[]
			{
				new FoodInfo( typeof( FoodStaleBread ), "pão mágico" ),
                new FoodInfo( typeof( Grapes ), "cacho de uvas" ),
				new FoodInfo( typeof( Ham ), "presunto" ),
				new FoodInfo( typeof( CheeseWedge ), "fatia de queijo" ),
				new FoodInfo( typeof( Muffins ), "muffins" ),
				new FoodInfo( typeof( FishSteak ), "filé de peixe" ),
				new FoodInfo( typeof( Ribs ), "costelinhas" ),
				new FoodInfo( typeof( CookedBird ), "frango cozido" ),
				new FoodInfo( typeof( Sausage ), "salsicha" ),
				new FoodInfo( typeof( Apple ), "maça" ),
				new FoodInfo( typeof( Peach ), "pera" )
			};

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				FoodInfo foodInfo = m_Food[Utility.Random( m_Food.Length )];
				Item food = foodInfo.Create();

				if ( food != null )
				{
                    Caster.AddToBackpack(food);
                    Caster.FixedParticles(0, 10, 5, 2003, Server.Items.CharacterDatabase.GetMySpellHue(Caster, 0), 0, EffectLayer.RightHand);
                    Caster.PlaySound(0x1E2);
                    Caster.SendMessage(55, "Magicamente um pouco de " + foodInfo.Name + " apareceu em sua mochila.");
					//Caster.SendLocalizedMessage(1042695, true, " " + foodInfo.Name);

					if (Utility.RandomMinMax(1, 10) <= 3) // 30% chance
					{
                        Caster.AddToBackpack(new WaterFlask());
                        Caster.SendMessage(20, "Uma garrafa de água surgiu magicamente!");
                    }
				}

			}

			FinishSequence();
		}
	}

	public class FoodInfo
	{
		private Type m_Type;
		private string m_Name;

		public Type Type{ get{ return m_Type; } set{ m_Type = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }

		public FoodInfo( Type type, string name )
		{
			m_Type = type;
			m_Name = name;
		}

		public Item Create()
		{
			Item item;

			try
			{
				item = (Item)Activator.CreateInstance( m_Type );
			}
			catch
			{
				item = null;
			}

			return item;
		}
	}
}