using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Silent_Island_PC
{
    public class Main : Game
    {
        #region Variabeln

        #region Technique

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public SpriteFont font;
        public bool debug;
        public int screenWidth;
        public int screenHeight;

        #region Speicher/Generation

        public string fileSave;
        public List<string> SaveDirectory;
        public bool load;
        public bool save;
        public bool generate = true;
        public Random random = new Random();

        #endregion

        #region Steuerung/Zeit

        public KeyboardState keyboardState;
        public MouseState mouseState;
        public Vector2 MousePos;
        public MouseState previousMouseState;
        public MouseState currentMouseState;

        private double elapsedTime;
        private double UpdateInterval;

        #endregion

        #region Kamera

        Vector2 cameraPosition;
        float zoom = 3.0f;
        float cameraSpeed = 1.0f;

        #endregion

        #endregion

        #region Textures
        //Block
        Texture2D TGrassBlock;
        Texture2D TWaterBlock;
        Texture2D TGravelBlock;
        Texture2D TStoneBlock;
        Texture2D TGrassRoot;

        Texture2D TGrassBlockHigh1;
        Texture2D TGrassBlockHigh2;

        //Deko
        Texture2D TGrassBlockEdge1;
        Texture2D TGrasBlockEdge2L;
        Texture2D TGrasBlockEdge2H;
        Texture2D TGrasBlockEdge3;
        Texture2D TGrasBlockEdge4;

        Texture2D TDeko;
        Texture2D TDekoMoss;
        Texture2D TDekoMossStone;
        Texture2D TDekoStone;

        Texture2D TOak_Log;
        Texture2D TOak_Leave;

        //Item
        Texture2D TFishing_Rod;
        Texture2D TFishing_Rod_Out;
        Texture2D TFishing_Line;

        Texture2D TChair;
        Texture2D TBarrel;
        Texture2D TPistol;

        //Entitys
        Texture2D TPlayerUp;
        Texture2D TPlayerDown;
        Texture2D TPlayerLeft;
        Texture2D TPlayerRight;

        Texture2D TFish;
        Texture2D TShark;
        //UI
        Texture2D TInventory;
        Texture2D THotbar;
        Texture2D THotbarSlot;

        //TEST
        Texture2D TTestBlock;
        Texture2D TEmpty;
        Texture2D TTestRand;


        #endregion

        #region Sound

        SoundEffect AngelAuswurf;

        #endregion

        #region World Generation

        #region Values

        public int blockUp;
        public int blockDown;
        public int blockRight;
        public int blockLeft;
        public int blockUpperLeft;
        public int blockUpperRight;
        public int blockLowerLeft;
        public int blockLowerRight;
        public int blockInsgesamt;

        #endregion

        #region Blocks

        Block[,] BlockLayer;
        public int[,] BlockID;

        public int PWater;
        public int PGrass;
        public int PGravel;

        public int smoothness = 4;

        #endregion

        #region Deko
        Block[,] Deko;
        public float[,] DekoID;

        public int rotation;
        public int xShift;
        public int yShift;
        public int[,] XShift;
        public int[,] YShift;

        public int PStein;
        public int PMoos;
        public int PSteinMoos;
        #endregion

        #region Structures
        Block[,] StructurLayer;
        public float[,] StructerID;
        #endregion

        #region Items
        //Items
        int[,] ItemID;
        Block[,] ItemLayer;
        #endregion



        #endregion

        #region Steuerung/Aktionen/Objekte
        //Allgemein
        public bool action;
        public bool moving;
        public bool update;
        public bool inventoryOpen;
        public bool chestOpen;



        //UI
        UI Hotbar;
        UI HotbarSlotFrame;
        UI Inventar;
        Entity HandObjekt;
        Item[] SlotObjekt;
        int objektHandVerschiebung;
        bool right;
        public int HotbarSlot;

        //Player/entity
        Entity Player;


        //Angeln
        bool angeln;
        bool ausgeworfen;
        Item Angel;
        Item AngelSchnur;
        Block Chair;
        Item Fish;
        Item Shark;


        #endregion

        public bool change;
        public bool layerPlaced;
        public string sdebug;

        //Hinzufügen
        public int worldSizeX = 64;
        public int worldSizeY = 64;

        public int angelSchnurRotation = 20;

        Item Pistol;
        int mostCommonNeighborID;
        bool allDifferent = true;

        int InventoryPlatz;
        double count;

        Texture2D TAngelLeiste;
        Texture2D TAngelLeisteZeiger;
        UI AngelLeiste;
        UI AngelLeisteZeiger;



        //TODO: Füge eine PositionHand hinzu wo alle Tools einfädeln

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        protected override void LoadContent()
        {
            #region Texturen

            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            fileSave = "C:\\Users\\simon\\source\\repos\\Silent Island PC\\Silent Island PC\\Content\\Speicher.txt";
            font = Content.Load<SpriteFont>("font");

            TGrassBlock = Content.Load<Texture2D>("Texturen/GrasBlock");
            TWaterBlock = Content.Load<Texture2D>("Texturen/WasserBlock");
            TStoneBlock = Content.Load<Texture2D>("Texturen/SteinBlock");
            TGravelBlock = Content.Load<Texture2D>("Texturen/KiesBlock");
            TGrassRoot = Content.Load<Texture2D>("Texturen/GrasWurzel");

            TGrassBlockHigh1 = Content.Load<Texture2D>("Texturen/GrasBlockHoch1");
            TGrassBlockHigh2 = Content.Load<Texture2D>("Texturen/GrasBlockHoch2");

            TGrassBlockEdge1 = Content.Load<Texture2D>("Texturen/GrasBlockRand1");
            TGrasBlockEdge2L = Content.Load<Texture2D>("Texturen/GrasBlockRand2L");
            TGrasBlockEdge2H = Content.Load<Texture2D>("Texturen/GrasBlockRand2H");
            TGrasBlockEdge3 = Content.Load<Texture2D>("Texturen/GrasBlockRand3");
            TGrasBlockEdge4 = Content.Load<Texture2D>("Texturen/GrasBlockRand4");


            TDekoMoss = Content.Load<Texture2D>("Texturen/DekoMoos");
            TDekoMossStone = Content.Load<Texture2D>("Texturen/DekoSteinMoos");
            TDekoStone = Content.Load<Texture2D>("Texturen/DekoStein");

            TPlayerUp = Content.Load<Texture2D>("Texturen/PlayerUp");
            TPlayerDown = Content.Load<Texture2D>("Texturen/PlayerDown");
            TPlayerLeft = Content.Load<Texture2D>("Texturen/PlayerLeft");
            TPlayerRight = Content.Load<Texture2D>("Texturen/PlayerRight");

            TFish = Content.Load<Texture2D>("Texturen/Fisch");
            TShark = Content.Load<Texture2D>("Texturen/Hai");
            TAngelLeisteZeiger = Content.Load<Texture2D>("Texturen/AngelLeisteZeiger");
            TAngelLeiste = Content.Load<Texture2D>("Texturen/AngelLeiste");


            TOak_Log = Content.Load<Texture2D>("Texturen/BaumStamm");
            TOak_Leave = Content.Load<Texture2D>("Texturen/BaumBlätter");
            TFishing_Rod = Content.Load<Texture2D>("Texturen/Angel");
            TFishing_Rod_Out = Content.Load<Texture2D>("Texturen/AngelAus");
            TFishing_Line = Content.Load<Texture2D>("Texturen/AngelSchnur");
            TChair = Content.Load<Texture2D>("Texturen/AngelStuhl");
            TBarrel = Content.Load<Texture2D>("Texturen/Fass");


            THotbar = Content.Load<Texture2D>("Texturen/Hotbar");
            THotbarSlot = Content.Load<Texture2D>("Texturen/HotbarSlot");
            TInventory = Content.Load<Texture2D>("Texturen/Inventar");

            TEmpty = Content.Load<Texture2D>("Texturen/EmptyDeko");
            TTestRand = Content.Load<Texture2D>("Texturen/TestRand");
            TTestBlock = Content.Load<Texture2D>("Texturen/TestBlock");

            TPistol = Content.Load<Texture2D>("Texturen/Pistole");

            #endregion

            #region Sound

            AngelAuswurf = Content.Load<SoundEffect>("Sounds/AngelAuswurf");

            #endregion

            #region Objekte

            //Layer
            BlockID = new int[64, 64];
            BlockLayer = new Block[64, 64];
            DekoID = new float[64, 64];
            Deko = new Block[64, 64];
            StructerID = new float[64, 64];
            StructurLayer = new Block[64, 64];
            XShift = new int[64, 64];
            YShift = new int[64, 64];
            ItemID = new int[64, 64];
            ItemLayer = new Block[64, 64];

            //Rest
            Player = new Entity(new Vector2(2, 2), TPlayerUp);
            Player.speed = 16;
            Hotbar = new UI(new Vector2(800, 1016), THotbar);
            Angel = new Item(new Vector2(0, 0), TFishing_Rod);
            Chair = new Block(new Vector2(0, 0), TChair);
            HotbarSlotFrame = new UI(new Vector2(0, 0), THotbarSlot);
            SlotObjekt = new Item[7];
            Inventar = new UI(new Vector2(0, 0), TInventory);
            Pistol = new Item(new Vector2(0, 0), TPistol);
            Fish = new Item(new Vector2(0, 0), TFish);
            Fish.ID = 3;
            Shark = new Item(new Vector2(0, 0), TShark);
            Shark.ID = 4;
            AngelLeiste = new UI(new Vector2(0, 0), TAngelLeiste);
            AngelLeisteZeiger = new UI(new Vector2(0, 0), TAngelLeisteZeiger);


            for (int i = 0; i < 7; ++i)
            {
                SlotObjekt[i] = new Item(new Vector2(0, 0), TEmpty);
            }
            HandObjekt = new Entity(new Vector2(0, 0), TEmpty);

            #endregion

            // TEST
            SlotObjekt[0].ID = 1;
            SlotObjekt[0].amount = 1;
            SlotObjekt[1].ID = 2;
            SlotObjekt[1].amount = 1;
            SlotObjekt[2].ID = 2;
            SlotObjekt[2].amount = 1;
            change = true;


            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    BlockID[i, j] = random.Next(1, 3);
                    ItemID[i, j] = 0;
                    ItemLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TEmpty);
                }
            }

            if (generate)
            {
                #region Debug
                //for (int i = 0; i < 64; ++i)
                {
                    //for (int j = 0; j < 64; ++j)
                    {
                        //Block[i, j] = new Objekt(new Vector2(i * 64, j * 64), TGrasBlock);
                        //Deko[i, j] = new Objekt(new Vector2(i * 64, j * 64), TDekoMoos);
                    }
                }
                #endregion
                GenerateBlocks();
                GenerateDeko();
                GenerateStructures();
            }

        }

        protected override void Update(GameTime gameTime)
        {
            #region Technik
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Keys[] keys = keyboardState.GetPressedKeys();
            MousePos = new Vector2(mouseState.X + cameraPosition.X, mouseState.Y + cameraPosition.Y);
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            count += gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateInterval = 10;
            #endregion

            if (elapsedTime >= UpdateInterval)
            {
                #region Steuerung
                if (!Taste(Keys.W) && !Taste(Keys.S) && !Taste(Keys.A) && !Taste(Keys.D))
                {
                    moving = false;
                }
                else
                {
                    if (Taste(Keys.W) && Player.Koordinaten.Y > 0)
                    {
                        moving = true;
                        Player.Textur = TPlayerUp;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X, Player.Koordinaten.Y - Player.speed);
                    }

                    if (Taste(Keys.S) && Player.Koordinaten.Y < 64 * 64 - 96)
                    {
                        moving = true;
                        Player.Textur = TPlayerDown;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X, Player.Koordinaten.Y + Player.speed);
                    }

                    if (Taste(Keys.A) && Player.Koordinaten.X > 0)
                    {
                        moving = true;
                        Player.Textur = TPlayerLeft;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X - Player.speed, Player.Koordinaten.Y);

                        right = false;

                    }

                    if (Taste(Keys.D) && Player.Koordinaten.X < 64 * 64 - 64)
                    {
                        moving = true;
                        Player.Textur = TPlayerRight;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X + Player.speed, Player.Koordinaten.Y);

                        right = true;

                    }
                }

                if (moving)
                {
                    Angel.Textur = TFishing_Rod;
                    ausgeworfen = false;
                }
                #endregion

                if (Taste(Keys.O))
                {
                    save = true;
                    List<string> speicherInhalt = new List<string>();
                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            speicherInhalt.Add(BlockID[i, j].ToString());

                        }
                    }
                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            speicherInhalt.Add(DekoID[i, j].ToString());
                        }
                    }

                    File.WriteAllLines(fileSave, speicherInhalt);


                }
                if (Taste(Keys.P))
                {
                    load = true;

                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            BlockID[i, j] = int.Parse(File.ReadLines(fileSave).Skip(j + i).First());
                            switch (BlockID[i, j])
                            {
                                case 1:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrassBlock);
                                    break;
                                case 2:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWaterBlock);
                                    break;
                                case 3:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGravelBlock);
                                    break;
                            }
                        }
                    }



                }

                update = false;

                if (change)
                {
                    for (int i = 0; i < 7; ++i)
                    {
                        switch (SlotObjekt[i].ID)
                        {
                            case 1:
                                SlotObjekt[i].Textur = TChair;
                                break;
                            case 2:
                                SlotObjekt[i].Textur = TFishing_Rod;
                                break;
                        }

                    }
                    change = false;
                }
                switch (MausRad())
                {
                    case 1:
                        HotbarSlot = (HotbarSlot + 1) % 7;
                        break;
                    case 2:
                        HotbarSlot = (HotbarSlot - 1 + 7) % 7;
                        break;
                    case 0:
                        break;


                }


                if (MausTaste(2))
                {

                    if (Chair.hit(MousePos, Chair) && Taste(Keys.LeftShift) && InReach(1, 1))
                    {
                        Player.Koordinaten = new Vector2(Chair.Koordinaten.X, Chair.Koordinaten.Y - 64);
                        angeln = true;
                    }
                    switch (SlotObjekt[HotbarSlot].ID)
                    {
                        case 1:

                            if (hitArray(BlockID) != 2 && InReach(2, 1))
                            {
                                ItemLayer[(int)(MousePos.X / 64), (int)(MousePos.Y / 64)].Textur = TChair;
                                ItemLayer[(int)(MousePos.X / 64), (int)(MousePos.Y / 64)].placed = true;
                                ItemLayer[(int)(MousePos.X / 64), (int)(MousePos.Y / 64)].Place(Chair, ItemLayer, MousePos);
                                layerPlaced = true;
                                clearInv(HotbarSlot);
                                SlotObjekt[HotbarSlot].amount = 0;

                                break;
                            }

                            break;
                        case 2:
                            if (hitArray(BlockID) == 2 && InReach(1, 1))
                            {
                                if (!ausgeworfen)
                                {
                                    AngelAuswurf.Play();
                                    angeln = true;
                                    ausgeworfen = true;
                                    AngelSchnur = new Item(new Vector2(HandObjekt.Koordinaten.X + 64 - 5, HandObjekt.Koordinaten.Y + 5), TFishing_Line);
                                    SlotObjekt[1].Textur = TFishing_Rod_Out;
                                    count = 0;
                                    AngelLeiste.Koordinaten = new Vector2(cameraPosition.X + (screenWidth / 2) - (AngelLeiste.Textur.Width / 2), cameraPosition.Y + (screenHeight - 100));
                                    
                                }
                                else
                                {
                                    if(count < 100) { break; }
                                    if (AngelLeisteZeiger.Koordinaten.X < AngelLeiste.Koordinaten.X + 52)
                                    {
                                        angeln = false;
                                        ausgeworfen = false;
                                        SlotObjekt[1].Textur = TFishing_Rod;
                                        break;
                                    }
                                    else if (AngelLeisteZeiger.Koordinaten.X + 8 < AngelLeiste.Koordinaten.X + 100 && AngelLeisteZeiger.Koordinaten.X > AngelLeiste.Koordinaten.X + 52 ||
                                        AngelLeisteZeiger.Koordinaten.X + 8 < AngelLeiste.Koordinaten.X + 256 && AngelLeisteZeiger.Koordinaten.X > AngelLeiste.Koordinaten.X + 180)
                                    {
                                        if (random.Next(1, 4) == 1)
                                        {
                                            Fish.Aufnehmen(Fish, SlotObjekt);
                                        }
                                        else if(random.Next(1, 4) == 1)
                                        {
                                            Shark.Aufnehmen(Shark, SlotObjekt);
                                        }
                                        angeln = false;
                                        ausgeworfen = false;
                                        SlotObjekt[1].Textur = TFishing_Rod;
                                        break;
                                    }
                                    else if (AngelLeisteZeiger.Koordinaten.X + 8 < AngelLeiste.Koordinaten.X + 124 && AngelLeisteZeiger.Koordinaten.X > AngelLeiste.Koordinaten.X + 100 ||
                                        AngelLeisteZeiger.Koordinaten.X + 8 < AngelLeiste.Koordinaten.X + 180 && AngelLeisteZeiger.Koordinaten.X > AngelLeiste.Koordinaten.X + 132)
                                    {
                                        if (random.Next(1, 2) == 1)
                                        {
                                            Fish.Aufnehmen(Fish, SlotObjekt);
                                        }
                                        else 
                                        {
                                            Shark.Aufnehmen(Shark, SlotObjekt);
                                        }
                                        angeln = false;
                                        ausgeworfen = false;
                                        SlotObjekt[1].Textur = TFishing_Rod;
                                        break;
                                    }
                                    else if (AngelLeisteZeiger.Koordinaten.X + 8 < AngelLeiste.Koordinaten.X + 132 && AngelLeisteZeiger.Koordinaten.X > AngelLeiste.Koordinaten.X + 124)
                                    {
                                        for(int i = 0; i < 2; i++)
                                        {
                                            if (random.Next(1, 2) == 1)
                                            {
                                                Fish.Aufnehmen(Fish, SlotObjekt);
                                            }
                                            else
                                            {
                                                Shark.Aufnehmen(Shark, SlotObjekt);
                                            }
                                        }
                                        
                                        angeln = false;
                                        ausgeworfen = false;
                                        SlotObjekt[1].Textur = TFishing_Rod;
                                        break;
                                    }

                                    break;
                                }

                            }

                            break;
                        default:
                            break;
                    }


                }
                if (MausTaste(1))
                {


                }



                #region Aktualisierung
                if (right)
                {
                    HandObjekt.Effekt = SpriteEffects.None;
                    objektHandVerschiebung = 0;
                    angelSchnurRotation = 20;
                }
                else
                {
                    HandObjekt.Effekt = SpriteEffects.FlipHorizontally;
                    objektHandVerschiebung = -64;
                    angelSchnurRotation = -20;
                    if (ausgeworfen)
                        AngelSchnur = new Item(new Vector2(HandObjekt.Koordinaten.X - 24 + 5, HandObjekt.Koordinaten.Y + 60), TFishing_Line);
                }

                HandObjekt.Textur = SlotObjekt[HotbarSlot].Textur;
                HandObjekt.Koordinaten = new Vector2(Player.Koordinaten.X + 30 + objektHandVerschiebung, Player.Koordinaten.Y + 30);

                //Fertig
                cameraPosition = new Vector2(Player.Koordinaten.X - (screenWidth / 2), Player.Koordinaten.Y - (screenHeight / 2));
                Hotbar.Koordinaten = new Vector2(cameraPosition.X + (screenWidth / 2) - (Hotbar.Textur.Width / 2), cameraPosition.Y + (screenHeight - 64));
                HotbarSlotFrame.Koordinaten = new Vector2(Hotbar.Koordinaten.X + 64 * HotbarSlot, Hotbar.Koordinaten.Y);
                for (int i = 0; i < 7; ++i)
                {
                    SlotObjekt[i].Koordinaten = new Vector2(Hotbar.Koordinaten.X + 64 * i, Hotbar.Koordinaten.Y);
                }
                if (ausgeworfen)
                {
                    if((count / 1000) % 2 < 1)
                    {
                        AngelLeisteZeiger.Koordinaten = new Vector2(cameraPosition.X + (screenWidth / 2) - (AngelLeiste.Textur.Width / 2) + ((int)count / 8 % 100) * 2, cameraPosition.Y + (screenHeight - 116));
                    }
                    else
                    {
                        AngelLeisteZeiger.Koordinaten = new Vector2(cameraPosition.X + (screenWidth / 2) - (AngelLeiste.Textur.Width / 2) - ((int)count / 10 % 100) * 2 + 256, cameraPosition.Y + (screenHeight - 116));
                    }
                }

                update = true;
                #endregion
                elapsedTime = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(-cameraPosition, 0)));

            //Blocks
            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    BlockLayer[i, j].Zeichne(spriteBatch, BlockLayer[i, j]);
                }
            }
            //Deko
            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    Deko[i, j].Zeichne(spriteBatch, Deko[i, j]);
                }
            }
            //Objekte
            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    StructurLayer[i, j].Zeichne(spriteBatch, StructurLayer[i, j]);
                }
            }
            //Item
            for (int i = 0; i < 64; ++i)
            {
                for (int j = 0; j < 64; ++j)
                {
                    ItemLayer[i, j].Zeichne(spriteBatch, ItemLayer[i, j]);
                }
            }



            if (layerPlaced)
                if (ItemLayer[(int)(MousePos.X / 64), (int)(MousePos.Y / 64)].placed == true)
                {
                    ItemLayer[(int)(MousePos.X / 64), (int)(MousePos.Y / 64)].Zeichne(spriteBatch, ItemLayer[(int)(MousePos.X / 64), (int)(MousePos.Y / 64)]);
                    layerPlaced = false;
                }

            if (ausgeworfen)
            {
                Angel.Textur = TFishing_Rod_Out;
                AngelSchnur.LZeichne(spriteBatch, AngelSchnur, angelSchnurRotation, new Vector2(30, 1));
                AngelLeiste.Zeichne(spriteBatch, AngelLeiste);
                AngelLeisteZeiger.Zeichne(spriteBatch, AngelLeisteZeiger);
            }


            Hotbar.Zeichne(spriteBatch, Hotbar);
            if (update)
                for (int i = 0; i < 7; ++i)
                {
                    SlotObjekt[i].Zeichne(spriteBatch, SlotObjekt[i]);
                    spriteBatch.DrawString(font, "" + SlotObjekt[i].amount, new Vector2(SlotObjekt[i].Koordinaten.X, SlotObjekt[i].Koordinaten.Y + 50), new Color(255, 255, 255));
                }
            HotbarSlotFrame.Zeichne(spriteBatch, HotbarSlotFrame);




            Player.Zeichne(spriteBatch, Player);
            HandObjekt.Zeichne(spriteBatch, HandObjekt);

            if (debug)
            {
                BlockLayer[1, 1].Zeichne(spriteBatch, new Block(new Vector2(0, 0), TTestBlock));
            }
            if (sdebug != null)
            {
                spriteBatch.DrawString(font, sdebug, new Vector2(100, 100), new Color(243, 178, 92));
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        #region Methoden
        public int[] GetSurroundingBlockIDs(int[,] blockID, int i, int j)
        {
            int[] surroundingBlockIDs = new int[8];

            surroundingBlockIDs[0] = GetNeighborBlockID(blockID, i, j, 0, -1);         // oben
            surroundingBlockIDs[1] = GetNeighborBlockID(blockID, i, j, 0, 1);          // unten
            surroundingBlockIDs[2] = GetNeighborBlockID(blockID, i, j, -1, 0);         // links
            surroundingBlockIDs[3] = GetNeighborBlockID(blockID, i, j, 1, 0);          // rechts
            surroundingBlockIDs[4] = GetNeighborBlockID(blockID, i, j, -1, -1);        // oben links
            surroundingBlockIDs[5] = GetNeighborBlockID(blockID, i, j, 1, -1);         // oben rechts
            surroundingBlockIDs[6] = GetNeighborBlockID(blockID, i, j, -1, 1);         // unten links
            surroundingBlockIDs[7] = GetNeighborBlockID(blockID, i, j, 1, 1);          // unten rechts

            return surroundingBlockIDs;
        }
        static int GetMostCommonNumber(int[] array)
        {
            // Zähle die Häufigkeit jedes Elements im Array
            var counts = array.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            // Finde das Element mit der höchsten Häufigkeit
            int mostCommonNumber = counts.OrderByDescending(kv => kv.Value).First().Key;

            return mostCommonNumber;
        }
        public int[] GetTouchingBlockIDs(int[,] blockID, int i, int j)
        {
            int[] surroundingBlockIDs = new int[8];

            surroundingBlockIDs[0] = GetNeighborBlockID(blockID, i, j, 0, -1);         // oben
            surroundingBlockIDs[1] = GetNeighborBlockID(blockID, i, j, 0, 1);          // unten
            surroundingBlockIDs[2] = GetNeighborBlockID(blockID, i, j, -1, 0);         // links
            surroundingBlockIDs[3] = GetNeighborBlockID(blockID, i, j, 1, 0);          // rechts

            return surroundingBlockIDs;
        }
        public int GetNeighborBlockID(int[,] blockID, int i, int j, int xOffset, int yOffset)
        {
            int newRow = i + xOffset;
            int newCol = j + yOffset;

            if (newRow >= 0 && newRow < blockID.GetLength(0) && newCol >= 0 && newCol < blockID.GetLength(1))
            {
                return blockID[newRow, newCol];
            }
            else
            {
                return 0;
            }
        }

        private void GenerateBlocks()
        {
            int width = BlockID.GetLength(0);
            int height = BlockID.GetLength(1);
            //Base
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    //smoothness = 10;
                    for (int t = 0; t < smoothness; ++t)
                    {
                        //Teste die umliegenden blöcke
                        for (int k = 0; k < 8; ++k)
                        {
                            if (GetSurroundingBlockIDs(BlockID, i, j)[k] == 1)
                            {
                                ++PGrass;
                            }
                            else if (GetSurroundingBlockIDs(BlockID, i, j)[k] == 2)
                            {
                                ++PWater;
                            }
                        }
                        //Setzte
                        if (BlockID[i, j] == 2 && PGrass > 4)
                        {
                            BlockID[i, j] = 1;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrassBlock);
                        }
                        else if (BlockID[i, j] == 1 && PWater > 5)
                        {
                            BlockID[i, j] = 2;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWaterBlock);
                        }
                        else
                        {
                            switch (BlockID[i, j])
                            {
                                case 1:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrassBlock);
                                    break;
                                case 2:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWaterBlock);
                                    break;
                            }
                        }
                        PWater = 0;
                        PGrass = 0;
                    }

                }
            }
            //Kies
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    //Extra
                    for (int t = 0; t < 3; ++t)
                    {
                        if (BlockID[i, j] == 2) { break; }
                        for (int k = 0; k < 8; ++k)
                        {
                            if (GetSurroundingBlockIDs(BlockID, i, j)[k] == 3)
                            {
                                ++PGravel;
                            }

                        }
                        if (PGravel > 4)
                        {
                            if (random.Next(1, PGravel - PGravel / 2) == 1)
                            {
                                BlockID[i, j] = 3;
                                BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGravelBlock);
                            }

                        }
                        else if (random.Next(1, 30) == 1)
                        {
                            BlockID[i, j] = 3;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGravelBlock);
                        }
                        if (random.Next(1, 80) == 3)
                        {
                            BlockID[i, j] = 4;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrassRoot);
                        }
                    }
                    PGravel = 0;
                }
            }

        }
        public void GenerateDeko()
        {
            int width = BlockID.GetLength(0);
            int height = BlockID.GetLength(1);

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {

                    #region Positionen
                    blockUp = GetNeighborBlockID(BlockID, i, j, 0, -1);
                    blockDown = GetNeighborBlockID(BlockID, i, j, 0, 1);
                    blockLeft = GetNeighborBlockID(BlockID, i, j, -1, 0);
                    blockRight = GetNeighborBlockID(BlockID, i, j, 1, 0);
                    blockUpperLeft = GetNeighborBlockID(BlockID, i, j, -1, -1);
                    blockUpperRight = GetNeighborBlockID(BlockID, i, j, 1, -1);
                    blockLowerLeft = GetNeighborBlockID(BlockID, i, j, -1, 1);
                    blockLowerRight = GetNeighborBlockID(BlockID, i, j, 1, 1);

                    blockInsgesamt = blockUp + blockDown + blockLeft + blockRight + blockUpperLeft + blockUpperRight + blockLowerLeft + blockLowerRight;

                    DekoID[i, j] = 0;
                    Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TEmpty);
                    #endregion

                    //Deko
                    if (BlockID[i, j] == 1)
                    {
                        if (random.Next(1, 6) == 2)
                        {
                            DekoID[i, j] = 2;
                            Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TDekoMoss);
                        }
                        else if (random.Next(1, 10) == 3)
                        {
                            DekoID[i, j] = 3;
                            Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TDekoMossStone);
                        }
                        else if (random.Next(1, 30) == 3)
                        {
                            DekoID[i, j] = 4;
                            Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TDekoStone);
                        }
                    }
                    //Rand
                    else if (BlockID[i, j] == 2 || BlockID[i, j] == 3)
                    {
                        if (blockRight == 1 || blockRight == 4)
                        {
                            //Rechts
                            SetDeko(TGrassBlockEdge1, 0, 0, 0);
                            if (blockDown == 1 || blockDown == 4)
                            {
                                //RechtsUnten
                                SetDeko(TGrasBlockEdge2L, 0, 0, 0);
                            }
                            if (blockLeft == 1 || blockLeft == 4)
                            {
                                //RechtsLinks
                                SetDeko(TGrasBlockEdge2H, 0, 0, 0);

                                if (blockDown == 1 || blockDown == 4)
                                {
                                    //RechtsUntenLinks
                                    SetDeko(TGrasBlockEdge3, 0, 0, 0);
                                }
                            }
                            if (blockUp == 1 || blockUp == 4)
                            {
                                //RechtsOben
                                SetDeko(TGrasBlockEdge2L, -90, 0, 64);

                                if (blockLeft == 1 || blockLeft == 4)
                                {
                                    //RechtsObenLinks
                                    SetDeko(TGrasBlockEdge3, 180, 64, 64);
                                    if (blockDown == 1 || blockDown == 4)
                                    {
                                        //Ganz
                                        SetDeko(TGrasBlockEdge4, 0, 0, 0);
                                    }
                                }
                                else if (blockDown == 1 || blockDown == 4)
                                {
                                    //RechtsObenUnten
                                    SetDeko(TGrasBlockEdge3, -90, 0, 64);
                                }
                            }

                        }
                        else if (blockDown == 1 || blockDown == 4)
                        {
                            //Unten
                            SetDeko(TGrassBlockEdge1, 90, 64, 0);

                            if (blockLeft == 1 || blockLeft == 4)
                            {
                                //UntenLinks
                                SetDeko(TGrasBlockEdge2L, 90, 64, 0);
                            }
                            if (blockUp == 1 || blockUp == 4)
                            {
                                //UntenOben
                                SetDeko(TGrasBlockEdge2H, -90, 0, 64);

                                if (blockLeft == 1 || blockLeft == 4)
                                {
                                    //UntenLinksOben
                                    SetDeko(TGrasBlockEdge3, 90, 64, 0);
                                }
                            }
                        }
                        else if (blockLeft == 1 || blockLeft == 4)
                        {
                            //Links
                            SetDeko(TGrassBlockEdge1, 180, 64, 64);

                            if (blockUp == 1 || blockUp == 4)
                            {
                                //LinksOben
                                SetDeko(TGrasBlockEdge2L, 180, 64, 64);
                            }
                        }
                        else if (blockUp == 1 || blockUp == 4)
                        {
                            //Oben
                            SetDeko(TGrassBlockEdge1, -90, 0, 64);
                        }
                        else
                        {
                            //Wasser
                            SetDeko(TEmpty, 0, 0, 0);
                        }
                        AddDeko(i, j);
                        XShift[i, j] = xShift;
                        YShift[i, j] = yShift;
                    }
                }

        }
        private void GenerateStructures()
        {
            int width = BlockID.GetLength(0);
            int height = BlockID.GetLength(1);

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    StructurLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TEmpty);

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    #region Positionen
                    blockUp = GetNeighborBlockID(BlockID, i, j, 0, -1);
                    blockDown = GetNeighborBlockID(BlockID, i, j, 0, 1);
                    blockLeft = GetNeighborBlockID(BlockID, i, j, -1, 0);
                    blockRight = GetNeighborBlockID(BlockID, i, j, 1, 0);
                    blockUpperLeft = GetNeighborBlockID(BlockID, i, j, -1, -1);
                    blockUpperRight = GetNeighborBlockID(BlockID, i, j, 1, -1);
                    blockLowerLeft = GetNeighborBlockID(BlockID, i, j, -1, 1);
                    blockLowerRight = GetNeighborBlockID(BlockID, i, j, 1, 1);

                    blockInsgesamt = blockUp + blockDown + blockLeft + blockRight + blockUpperLeft + blockUpperRight + blockLowerLeft + blockLowerRight;
                    #endregion


                    if (BlockID[i, j] == 4)
                    {
                        StructurLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TOak_Log);
                        StructerID[i, j] = 1;

                        if (blockUp != 0)
                        {
                            StructurLayer[i, j - 1] = new Block(new Vector2(i * 64, (j - 1) * 64), TOak_Log);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, 0, -2) != 0)
                        {
                            StructurLayer[i, j - 2] = new Block(new Vector2(i * 64, (j - 2) * 64), TOak_Leave);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, 1, -2) != 0)
                        {
                            StructurLayer[i + 1, j - 2] = new Block(new Vector2((i + 1) * 64, (j - 2) * 64), TOak_Leave);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, -1, -2) != 0)
                        {
                            StructurLayer[i - 1, j - 2] = new Block(new Vector2((i - 1) * 64, (j - 2) * 64), TOak_Leave);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, 0, -3) != 0)
                        {
                            StructurLayer[i, j - 3] = new Block(new Vector2(i * 64, (j - 3) * 64), TOak_Leave);
                        }

                    }


                }
            }
        }

        void SetDeko(Texture2D texture, int rotation, int xShift, int yShift)
        {
            TDeko = texture;
            this.rotation = rotation;
            this.xShift = xShift;
            this.yShift = yShift;
        }
        void AddDeko(int i, int j)
        {
            if (TDeko == TGrassBlockEdge1)
            {
                DekoID[i, j] = 1f;
            }
            else if (TDeko == TGrasBlockEdge2L)
            {
                DekoID[i, j] = 2.2f;
            }
            else if (TDeko == TGrasBlockEdge2H)
            {
                DekoID[i, j] = 2.25f;
            }
            else if (TDeko == TGrasBlockEdge3)
            {
                DekoID[i, j] = 3;
            }
            else if (TDeko == TGrasBlockEdge4)
            {
                DekoID[i, j] = 4;
            }
            Deko[i, j] = new Block(new Vector2(i * 64 + xShift, j * 64 + yShift), TDeko);
            Deko[i, j].Rotation = MathHelper.ToRadians(rotation);
            Deko[i, j].Drehpunkt = new Vector2(0, 0);
        }

        public int hitArray(int[,] ID)
        {
            int x = (int)(MousePos.X / 64);
            int y = (int)(MousePos.Y / 64);


            if (MousePos.X >= 0 && MousePos.X < 64 * 64 && MousePos.Y >= 0 && MousePos.Y < 64 * 64)
                return ID[x, y];


            return 0;
        }
        public void clearInv(int slot)
        {
            SlotObjekt[slot].ID = 0;
            HandObjekt.Textur = TEmpty;
            SlotObjekt[slot].Textur = TEmpty;
        }
        public bool Taste(Keys key)
        {
            if (keyboardState.IsKeyDown(key))
            {
                return true;
            }
            else { return false; }
        }
        public bool MausTaste(int button)
        {
            if (button == 1)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            else if (button == 2)
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        public int MausRad()
        {
            MouseState currentMouseState = Mouse.GetState();

            // UP
            if (currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
            {
                previousMouseState = currentMouseState;
                return 1;
            }
            // DOWN
            else if (currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
            {
                previousMouseState = currentMouseState;
                return 2;
            }

            return 0;
        }
        public bool InReach(float playerReichweiteX, float playerReichweiteY)
        {
            float minX = Player.Koordinaten.X + (Player.Textur.Width / 2) - (playerReichweiteX * 64);
            float maxX = Player.Koordinaten.X + (Player.Textur.Width / 2) + (playerReichweiteX * 64);

            if (MousePos.X >= minX && MousePos.X <= maxX)
            {
                float minY = Player.Koordinaten.Y + (Player.Textur.Height / 2) - (playerReichweiteY * 64);
                float maxY = Player.Koordinaten.Y + (Player.Textur.Height / 2) + (playerReichweiteY * 64);

                if (MousePos.Y >= minY && MousePos.Y <= maxY)
                {
                    return true;
                }
            }
            return false;
        }
        public Item SetItem(Item item, int ID)
        {
            switch (ID)
            {
                case 1:
                    item = new Item(new Vector2(0, 0), TFishing_Rod);
                    item.Name = Item.Items[1];
                    break;
                case 2:
                    item = new Item(new Vector2(0, 0), TFishing_Rod);
                    item.Name = Item.Items[1];
                    break;

                default:
                    item = new Item(new Vector2(0, 0), TEmpty);
                    break;
            }

            return item;
        }

        #endregion

    }
}




//Draw.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(-cameraPosition * zoom, 0)) * Matrix.CreateScale(zoom))


// MIT SHIFT EINF CURSOR ÄNDERN

/*
 Objekt hitLayer(Objekt[,] layer)
        {
            int x = (int)(MausPos.X / 64);
            int y = (int)(MausPos.Y / 64);


            if (MausPos.X >= 0 && MausPos.X < 64 * 64 && MausPos.Y >= 0 && MausPos.Y < 64 * 64)
                return layer[x, y];


            return new Objekt(new Vector2(0, 0), TEmpty);
        }












// Merge isolated blocks
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    for (int k = 0; k < 8; ++k)
                    {
                        if (GetSurroundingBlockIDs(BlockID, i, j)[k] == BlockID[i, j])
                        {
                            allDifferent = false;
                            break;
                        }
                    }
                    if (allDifferent)
                    {
                        BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TTestBlock);
                        //mostCommonNeighborID = GetMostCommonNumber(GetSurroundingBlockIDs(BlockID, i, j));

                        if (mostCommonNeighborID == 0)
                        {
                            mostCommonNeighborID = 1;
                        }
                        BlockID[i, j] = mostCommonNeighborID;
                        // Merge if isolated
                        if (mostCommonNeighborID == 1)
                        {
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrassBlock);
                        }
                        else if (mostCommonNeighborID == 2)
                        {

                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWaterBlock);
                        }
                        else if (mostCommonNeighborID == 3)
                        {

                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGravelBlock);
                        }
                    }
                    allDifferent = true;
                }
            }
*/