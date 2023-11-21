using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
namespace Server.Items
{
	public abstract class BaseClothMaterial : Item/*, IDyable*/
	{
        private bool m_colored;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ArtificialColored
        {
            get { return m_colored; }
            set { m_colored = value; InvalidateProperties(); }
        }

        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }
        }

        public BaseClothMaterial( int itemID ) : this( itemID, 1 )
		{
            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

		public BaseClothMaterial( int itemID, int amount ) : base( itemID )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;

            if (m_Resource == CraftResource.None)
                m_Resource = CraftResource.Cotton;
        }

		public BaseClothMaterial( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

            writer.Write((int)1); // version

            writer.Write(m_colored);

            writer.Write((int)m_Resource);
        }

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_colored = reader.ReadBool();
						m_Resource = (CraftResource)reader.ReadInt();
						break;
					}
			}

/*			if (version == 0)
				m_Resource = CraftResource.Cotton;*/
		}

/*		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;
            ArtificialColored = true;

			return ArtificialColored;
		}*/

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
                from.SendMessage(55, "Você precisa de um tear para transformar isso.");
                //from.SendLocalizedMessage( 500366 ); // Select a loom to use that on.
				from.Target = new PickLoomTarget( this );
			}
			else
			{
                from.SendMessage(55, "Isso precisa estar na sua mochila.");
                //from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

			if (m_Resource != CraftResource.None) 
			{
                string resourceName = CraftResources.GetName(m_Resource);
                if (string.IsNullOrEmpty(resourceName) || resourceName.ToLower() == "none" || resourceName.ToLower() == "normal" )
                {
                    resourceName = "";
                }

                if (resourceName != "")
                {
                    list.Add(1053099, ItemNameHue.UnifiedItemProps.SetColor(resourceName, "#8be4fc"));
                }
            }

            if (m_colored)
                list.Add(1070722, ItemNameHue.UnifiedItemProps.SetColor("Colorido Artificalmente", "#8be4fc"));

            list.Add(1049644, ItemNameHue.UnifiedItemProps.SetColor("Utilize um tear para transformar.", "#ffe066")); // PARENTHESIS
        }

        private class PickLoomTarget : Target
		{
			private BaseClothMaterial m_Material;

			public PickLoomTarget( BaseClothMaterial material ) : base( 3, false, TargetFlags.None )
			{
				m_Material = material;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Material.Deleted )
					return;

				ILoom loom = targeted as ILoom;

				if ( loom == null && targeted is AddonComponent )
					loom = ((AddonComponent)targeted).Addon as ILoom;

				if ( loom != null )
				{
					if ( !m_Material.IsChildOf( from.Backpack ) )
					{
                        from.SendMessage(55, "Isso precisa estar na sua mochila.");  
						//from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
					}
					else
					{
						int materialHue = m_Material.Hue;

                        int cycle = m_Material.Amount;
						int looms = loom.Phase;
						int amount = 0;

						bool sendMessage = false;

                        //from.SendMessage(55, "cycle: " + cycle + " - looms: " + looms);

                        while ( cycle > 0 )
						{
							if ( looms >= 4 )
							{
								looms = 0;
								amount++;
								sendMessage = true;
							}
							else
							{
								looms++;
							}
                            cycle--;
                        }


						if ( sendMessage )
						{
							cycle = 0;

                            m_Material.Delete();
                            loom.Phase = looms;

                            BoltOfCloth create = new BoltOfCloth(amount);
                            if (typeof(Wool) == m_Material.GetType()) 
                            {
                                create = new WoolBoltOfCloth(amount);
                            }
                            create.Hue = m_Material.Hue;
                            create.ArtificialColored = m_Material.ArtificialColored;
                            create.Resource = m_Material.Resource;

                            from.AddToBackpack(create);

                            from.SendMessage(55, "Você transforma material e coloca na mochila.");
                            //from.SendLocalizedMessage( 500368 ); // You create some cloth and put it in your backpack.

                            /*							if (typeof(LightYarnUnraveled) == m_Material.GetType()) // DEPRECATED - NOT GONNA HAPPEN
                                                        {
                                                            *//*SpoolOfThread create = new SpoolOfThread(amount * 2);
                                                            create.Hue = m_Material.Hue;
                                                            create.ArtificialColored = m_Material.ArtificialColored;
                                                            create.Resource = CraftResource.Silk;

                                                            from.AddToBackpack(create);

                                                            from.SendMessage(55, "Você transforma material em carretel(s) de linha e coloca na mochila.");*//*
                                                        }*/

                            if ( loom.Phase > 0 )
							{
                                if (from.Skills.Tailoring.Value >= 90.0)
								{
                                    if (typeof(DarkYarn) == m_Material.GetType())
                                    {
                                        DarkYarn rest = new DarkYarn(loom.Phase);
                                        rest.Hue = materialHue;
                                        rest.ArtificialColored = m_Material.ArtificialColored;
                                        rest.Resource = m_Material.Resource;

                                        from.AddToBackpack(rest);
                                    }
                                    else if (typeof(LightYarn) == m_Material.GetType())
                                    {
                                        LightYarn rest = new LightYarn(loom.Phase);
                                        rest.Hue = materialHue;
                                        rest.ArtificialColored = m_Material.ArtificialColored;
                                        rest.Resource = m_Material.Resource;

                                        from.AddToBackpack(rest);
                                    }
                                    /*else if (typeof(LightYarnUnraveled) == m_Material.GetType())  // DEPRECATED - NOT GONNA HAPPEN
									{
                                        LightYarnUnraveled rest = new LightYarnUnraveled(loom.Phase);
                                        rest.Hue = materialHue;
                                        rest.ArtificialColored = m_Material.ArtificialColored;
                                        rest.Resource = m_Material.Resource;

                                        from.AddToBackpack(rest);
                                    }*/
                                    else if (typeof(CottonSpoolOfThread) == m_Material.GetType()) 
                                    {
                                        CottonSpoolOfThread rest = new CottonSpoolOfThread(loom.Phase);
                                        rest.Hue = materialHue;
                                        rest.ArtificialColored = m_Material.ArtificialColored;

                                        from.AddToBackpack(rest);
                                    }
                                    else if (typeof(FlaxSpoolOfThread) == m_Material.GetType()) 
                                    {
                                        FlaxSpoolOfThread rest = new FlaxSpoolOfThread(loom.Phase);
                                        rest.Hue = materialHue;
                                        rest.ArtificialColored = m_Material.ArtificialColored;

                                        from.AddToBackpack(rest);
                                    }
                                    else if (typeof(SilkSpoolOfThread) == m_Material.GetType()) 
                                    {
                                        SilkSpoolOfThread rest = new SilkSpoolOfThread(loom.Phase);
                                        rest.Hue = materialHue;
                                        rest.ArtificialColored = m_Material.ArtificialColored;

                                        from.AddToBackpack(rest);
                                    }

                                    from.SendMessage(55, "Por ser altamente habilidoso, você é capaz de recuperar o resto do material utilizado.");
                                }
								else 
								{
                                    from.SendMessage(55, "Você não possui habilidade suficiente para recuperar o resto do material utilizado e acaba perdendo-o.");
                                }

                                loom.Phase = 0; // reset the loom
                            }
						}
						else
						{
							from.SendMessage(55, "Você não tem o suficiente para criar um rolo de pano.");
						}
					}
				}
				else
				{
					from.SendLocalizedMessage( 500367 ); // Try using that on a loom.
				}
			}
		}
	}

    // Yarn
	public class DarkYarn : BaseClothMaterial
	{
		[Constructable]
		public DarkYarn() : this( 1 )
		{
            Name = "bola(s) de lã grossa";
        }

		[Constructable]
		public DarkYarn( int amount ) : base( 0xE1D, amount )
		{
            Name = "bola(s) de lã grossa";
        }

		public DarkYarn( Serial serial ) : base( serial )
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

            Name = "bola(s) de lã grossa";
        }
	}

	public class LightYarn : BaseClothMaterial
	{
		[Constructable]
		public LightYarn() : this( 1 )
		{
            Name = "bola(s) de lã fina";
        }

		[Constructable]
		public LightYarn( int amount ) : base( 0xE1E, amount )
		{
            Name = "bola(s) de lã fina";
        }

		public LightYarn( Serial serial ) : base( serial )
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

            Name = "bola(s) de lã fina";
        }
	}

    public class LightYarnUnraveled : BaseClothMaterial
    {
        [Constructable]
        public LightYarnUnraveled() : this(1)
        {
            Name = "bola(s) de lã desenrolada";
        }

        [Constructable]
        public LightYarnUnraveled(int amount) : base(0xE1E, amount)
        {
            Name = "bola(s) de lã desenrolada";
        }

        public LightYarnUnraveled(Serial serial) : base(serial)
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

            Name = "bola(s) de lã desenrolada";
        }
    }

    // Threads
    public class SpoolOfThread : BaseClothMaterial
	{
		[Constructable]
		public SpoolOfThread() : this( 1 )
		{
            if (Resource == CraftResource.None)
                Resource = CraftResource.Cotton;
        }

		[Constructable]
		public SpoolOfThread( int amount, CraftResource resource ) : base( 0x543A, amount )
		{
            Resource = resource;

            if (Resource == CraftResource.None)
                Resource = CraftResource.Cotton;
            
        }

		public SpoolOfThread( Serial serial ) : base( serial )
		{
            if (Resource == CraftResource.None)
                Resource = CraftResource.Cotton;
        }

        public override int LabelNumber
        {
            get
            {
                if (Resource == CraftResource.Cotton)
                    return 1101196;
                else if (Resource == CraftResource.Flax)
                    return 1101197;
                else if (Resource == CraftResource.Silk)
                    return 1101198;

                return 1101199;
            }
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

            //ItemID = 0x543A;
		}
	}
    public class CottonSpoolOfThread : SpoolOfThread
    {
        [Constructable]
        public CottonSpoolOfThread() : this(1)
        {

        }

        [Constructable]
        public CottonSpoolOfThread(int amount) : base(amount, CraftResource.Cotton)
        {
        }

        public CottonSpoolOfThread(Serial serial) : base(serial)
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
        }
    }
    public class FlaxSpoolOfThread : SpoolOfThread
    {
        [Constructable]
        public FlaxSpoolOfThread() : this(1)
        {

        }

        [Constructable]
        public FlaxSpoolOfThread(int amount) : base(amount, CraftResource.Flax)
        {
        }

        public FlaxSpoolOfThread(Serial serial) : base(serial)
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
        }
    }
    public class SilkSpoolOfThread : SpoolOfThread
    {
        [Constructable]
        public SilkSpoolOfThread() : this(1)
        {

        }

        [Constructable]
        public SilkSpoolOfThread(int amount) : base( amount, CraftResource.Silk)
        {
        }

        public SilkSpoolOfThread(Serial serial) : base(serial)
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
        }
    }
}