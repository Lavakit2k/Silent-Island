using Microsoft.Xna.Framework;
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

        #region Technik
        //Technisch
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public SpriteFont font;
        public string fileSpeicher;
        List<string> speicherInhalt;

        KeyboardState keyboardState;
        MouseState mouseState;

        Vector2 MausPos;

        MouseState previousMouseState;
        MouseState currentMouseState;

        private double elapsedTime;
        private double UpdateInterval;

        int screenWidth;
        int screenHeight;
        public bool laden;
        public bool speichern;
        public bool generieren = true;
        Random random = new Random();

        bool debug;
        #endregion

        #region Kamera
        Vector2 cameraPosition;
        float zoom = 3.0f;
        float cameraSpeed = 1.0f;
        #endregion

        #region Texturen
        //Block
        Texture2D TGrasBlock;
        Texture2D TWasserBlock;
        Texture2D TKiesBlock;
        Texture2D TStoneBlock;
        Texture2D TGrasWurzel;
        //Deko
        Texture2D TGrasBlockRand1;
        Texture2D TGrasBlockRand2L;
        Texture2D TGrasBlockRand2H;
        Texture2D TGrasBlockRand3;
        Texture2D TGrasBlockRand4;

        Texture2D TDeko;
        Texture2D TDekoMoos;
        Texture2D TDekoMoosStein;
        Texture2D TDekoStein;
        //Objekte
        Texture2D TBaumStamm;
        Texture2D TBaumBlätter;

        Texture2D TAngel;
        Texture2D TAngelAus;
        Texture2D TAngelSchnur;

        Texture2D TAngelStuhl;
        Texture2D TFass;


        //Player
        Texture2D TPlayerUp;
        Texture2D TPlayerDown;
        Texture2D TPlayerLeft;
        Texture2D TPlayerRight;
        //UI
        Texture2D THotbar;
        Texture2D THotbarSlot;
        Texture2D TInventar;

        //TEST
        Texture2D TTestBlock;
        Texture2D TEmpty;
        Texture2D TTestRand;


        #endregion

        #region WeltGenerierung

        #region Allgemein
        
        int blockUp;
        int blockDown;
        int blockRight;
        int blockLeft;
        int blockUpperLeft;
        int blockUpperRight;
        int blockLowerLeft;
        int blockLowerRight;
        int blockInsgesamt;
        #endregion

        #region Block
        Block[,] BlockLayer;
        public int[,] BlockID;

        int PWasser;
        int PGras;
        int PKies = 34;

        int smoothing = 4;
        #endregion

        #region Deko
        Block[,] Deko;
        public float[,] DekoID;
        
        int rotation;
        int xShift;
        int yShift;
        int[,] XShift;
        int[,] YShift;

        int PStein;
        int PMoos;
        int PSteinMoos;
        #endregion

        #region Objekte
        Block[,] ObjekteLayer;
        public float[,] ObjekteID;
        #endregion

        #endregion

        #region Steuerung/Aktionen
        //Allgemein
        public bool aktion;
        public bool bewegen;
        public bool aktualisieren;
        public bool inventarOpen;
        public bool chestOpen;

        //Items
        //int[] ItemListID;
        int[,] ItemID;
        Block[,] ItemLayer;

        //UI
        GameObjekt Hotbar;
        GameObjekt Inventar;
        public int HotbarSlot;
        GameObjekt HandObjekt;
        int objektHandVerschiebung;
        bool rechts;
        GameObjekt HotbarSlotRahmen;
        Item[] SlotObjekt;
        

        //Player
        GameObjekt Player;
        int playerSpeed = 20;

        //Angeln
        bool angeln;
        bool ausgeworfen;
        Item Angel;
        Item AngelSchnur;
        Block AngelStuhl;
        #endregion

        public bool change;
        public bool layerPlaced;
        public string sdebug;

        //Hinzufügen
        public int worldSizeX;
        public int worldSizeY;

        public int playerReichweite = 64 * 4;




        //TODO: Mach ein Objekt ObjektInHand; welches dann ausgetauscht werdden kann

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
            fileSpeicher = "C:\\Users\\simon\\source\\repos\\Silent Island PC\\Silent Island PC\\Content\\Speicher.txt";
            font = Content.Load<SpriteFont>("font");

            TGrasBlock = Content.Load<Texture2D>("Texturen/GrasBlock");
            TWasserBlock = Content.Load<Texture2D>("Texturen/WasserBlock");
            TStoneBlock = Content.Load<Texture2D>("Texturen/SteinBlock");
            TKiesBlock = Content.Load<Texture2D>("Texturen/KiesBlock");
            TGrasWurzel = Content.Load<Texture2D>("Texturen/GrasWurzel");

            TGrasBlockRand1 = Content.Load<Texture2D>("Texturen/GrasBlockRand1");
            TGrasBlockRand2L = Content.Load<Texture2D>("Texturen/GrasBlockRand2L");
            TGrasBlockRand2H = Content.Load<Texture2D>("Texturen/GrasBlockRand2H");
            TGrasBlockRand3 = Content.Load<Texture2D>("Texturen/GrasBlockRand3");
            TGrasBlockRand4 = Content.Load<Texture2D>("Texturen/GrasBlockRand4");


            TDekoMoos = Content.Load<Texture2D>("Texturen/DekoMoos");
            TDekoMoosStein = Content.Load<Texture2D>("Texturen/DekoSteinMoos");
            TDekoStein = Content.Load<Texture2D>("Texturen/DekoStein");

            TPlayerUp = Content.Load<Texture2D>("Texturen/PlayerUp");
            TPlayerDown = Content.Load<Texture2D>("Texturen/PlayerDown");
            TPlayerLeft = Content.Load<Texture2D>("Texturen/PlayerLeft");
            TPlayerRight = Content.Load<Texture2D>("Texturen/PlayerRight");

            TBaumStamm = Content.Load<Texture2D>("Texturen/BaumStamm");
            TBaumBlätter = Content.Load<Texture2D>("Texturen/BaumBlätter");
            TAngel = Content.Load<Texture2D>("Texturen/Angel");
            TAngelAus = Content.Load<Texture2D>("Texturen/AngelAus");
            TAngelSchnur = Content.Load<Texture2D>("Texturen/AngelSchnur");
            TAngelStuhl = Content.Load<Texture2D>("Texturen/AngelStuhl");
            TFass = Content.Load<Texture2D>("Texturen/Fass");


            THotbar = Content.Load<Texture2D>("Texturen/Hotbar");
            THotbarSlot = Content.Load<Texture2D>("Texturen/HotbarSlot");
            TInventar = Content.Load<Texture2D>("Texturen/Inventar");

            TEmpty = Content.Load<Texture2D>("Texturen/EmptyDeko");
            TTestRand = Content.Load<Texture2D>("Texturen/TestRand");
            TTestBlock = Content.Load<Texture2D>("Texturen/TestBlock");

            #endregion
            #region Objekte
            //Layer
            BlockID = new int[64, 64];
            BlockLayer = new Block[64, 64];
            DekoID = new float[64, 64];
            Deko = new Block[64, 64];
            ObjekteID = new float[64, 64];
            ObjekteLayer = new Block[64, 64];
            XShift = new int[64, 64];
            YShift = new int[64, 64];
            ItemID = new int[64, 64];
            ItemLayer = new Block[64, 64];

            //UI
            Player = new GameObjekt(new Vector2(2, 2), TPlayerUp);
            Hotbar = new GameObjekt(new Vector2(800, 1016), THotbar);
            Angel = new Item(new Vector2(0, 0), TAngel);
            AngelStuhl = new Block(new Vector2(0, 0), TAngelStuhl);
            HotbarSlotRahmen = new GameObjekt(new Vector2(0, 0), THotbarSlot);
            SlotObjekt = new Item[7];
            Inventar = new GameObjekt(new Vector2(0, 0), TInventar);


            for (int i = 0; i < 7; ++i)
            {
                SlotObjekt[i] = new Item(new Vector2(0, 0), TEmpty);
            }
            HandObjekt = new GameObjekt(new Vector2(0, 0), TEmpty);

            #endregion







            // TEST
            SlotObjekt[0].ID = 1;
            SlotObjekt[0].amount = 1;
            SlotObjekt[1].ID = 2;
            SlotObjekt[1].amount = 1;
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

            if (generieren)
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
                GenerateObjekte();
            }

        }

        protected override void Update(GameTime gameTime)
        {
            #region Technik
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Keys[] keys = keyboardState.GetPressedKeys();
            MausPos = new Vector2(mouseState.X + cameraPosition.X, mouseState.Y + cameraPosition.Y);
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            UpdateInterval = 10;
            #endregion



            if (elapsedTime >= UpdateInterval)
            {
                #region Steuerung
                if (!Taste(Keys.W) && !Taste(Keys.S) && !Taste(Keys.A) && !Taste(Keys.D))
                {
                    bewegen = false;
                }
                else
                {
                    if (Taste(Keys.W) && Player.Koordinaten.Y > 0)
                    {
                        bewegen = true;
                        Player.Textur = TPlayerUp;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X, Player.Koordinaten.Y - playerSpeed);
                    }

                    if (Taste(Keys.S) && Player.Koordinaten.Y < 64 * 64 - 96)
                    {
                        bewegen = true;
                        Player.Textur = TPlayerDown;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X, Player.Koordinaten.Y + playerSpeed);
                    }

                    if (Taste(Keys.A) && Player.Koordinaten.X > 0)
                    {
                        bewegen = true;
                        Player.Textur = TPlayerLeft;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X - playerSpeed, Player.Koordinaten.Y);

                        rechts = false;

                    }

                    if (Taste(Keys.D) && Player.Koordinaten.X < 64 * 64 - 64)
                    {
                        bewegen = true;
                        Player.Textur = TPlayerRight;
                        Player.Koordinaten = new Vector2(Player.Koordinaten.X + playerSpeed, Player.Koordinaten.Y);

                        rechts = true;

                    }
                }

                if (bewegen)
                {
                    Angel.Textur = TAngel;
                    ausgeworfen = false;
                }
                #endregion

                if (Taste(Keys.O))
                {
                    speichern = true;
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

                    File.WriteAllLines(fileSpeicher, speicherInhalt);


                }
                if (Taste(Keys.P))
                {
                    laden = true;

                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            BlockID[i, j] = int.Parse(File.ReadLines(fileSpeicher).Skip(j + i).First());
                            switch (BlockID[i, j])
                            {
                                case 1:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrasBlock);
                                    break;
                                case 2:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWasserBlock);
                                    break;
                                case 3:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TKiesBlock);
                                    break;
                            }
                        }
                    }



                }

                aktualisieren = false;

                if (change)
                {
                    for (int i = 0; i < 7; ++i)
                    {
                        switch (SlotObjekt[i].ID)
                        {
                            case 1:
                                SlotObjekt[i].Textur = TAngelStuhl;
                                break;
                            case 2:
                                SlotObjekt[i].Textur = TAngel;
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

                    if (AngelStuhl.hit(MausPos, AngelStuhl) && Taste(Keys.LeftShift))
                    {
                        Player.Koordinaten = new Vector2(AngelStuhl.Koordinaten.X, AngelStuhl.Koordinaten.Y - 64);
                        angeln = true;
                    }
                    switch (SlotObjekt[HotbarSlot].ID)
                    {
                        case 1:

                            if (hitArray(BlockID) != 2)
                            {
                                ItemLayer[(int)(MausPos.X / 64), (int)(MausPos.Y / 64)].Textur = TAngelStuhl;
                                ItemLayer[(int)(MausPos.X / 64), (int)(MausPos.Y / 64)].placed = true;
                                ItemLayer[(int)(MausPos.X / 64), (int)(MausPos.Y / 64)].Place(AngelStuhl, ItemLayer, MausPos);
                                layerPlaced = true;
                                clearInv(HotbarSlot);
                                SlotObjekt[HotbarSlot].amount = 0;

                                break;
                            }

                            break;
                        case 2:
                            if (hitArray(BlockID) == 2)
                            {
                                debug = true;
                                angeln = true;
                                ausgeworfen = true;
                                AngelSchnur = new Item(new Vector2(HandObjekt.Koordinaten.X + 64 - 5, HandObjekt.Koordinaten.Y + 5), TAngelSchnur);
                                SlotObjekt[1].Textur = TAngelAus;
                            }

                            break;
                        default:
                            break;
                    }


                }
                if (MausTaste(1))
                {
                    

                }

                if (rechts)
                {
                    HandObjekt.Effekt = SpriteEffects.None;
                    objektHandVerschiebung = 0;

                }
                else
                {
                    HandObjekt.Effekt = SpriteEffects.FlipHorizontally;
                    objektHandVerschiebung = -64;
                }

                HandObjekt.Textur = SlotObjekt[HotbarSlot].Textur;
                HandObjekt.Koordinaten = new Vector2(Player.Koordinaten.X + 30 + objektHandVerschiebung, Player.Koordinaten.Y + 30);

                //Fertig
                cameraPosition = new Vector2(Player.Koordinaten.X - (screenWidth / 2), Player.Koordinaten.Y - (screenHeight / 2));
                Hotbar.Koordinaten = new Vector2(cameraPosition.X + (screenWidth / 2) - (Hotbar.Textur.Width / 2), cameraPosition.Y + (screenHeight - 64));
                HotbarSlotRahmen.Koordinaten = new Vector2(Hotbar.Koordinaten.X + 64 * HotbarSlot, Hotbar.Koordinaten.Y);
                for (int i = 0; i < 7; ++i)
                {
                    SlotObjekt[i].Koordinaten = new Vector2(Hotbar.Koordinaten.X + 64 * i, Hotbar.Koordinaten.Y);
                }


                aktualisieren = true;

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
                    ObjekteLayer[i, j].Zeichne(spriteBatch, ObjekteLayer[i, j]);
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
            


            
            if(layerPlaced)
            if (ItemLayer[(int)(MausPos.X / 64), (int)(MausPos.Y / 64)].placed == true)
            {
                ItemLayer[(int)(MausPos.X / 64), (int)(MausPos.Y / 64)].Zeichne(spriteBatch, ItemLayer[(int)(MausPos.X / 64), (int)(MausPos.Y / 64)]);
                layerPlaced = false;
            }

            if (ausgeworfen)
            {
                Angel.Textur = TAngelAus;
                AngelSchnur.LZeichne(spriteBatch, AngelSchnur, 20, new Vector2(30, 1));
            }


            Hotbar.Zeichne(spriteBatch, Hotbar);
            if (aktualisieren)
                for (int i = 0; i < 7; ++i)
                {
                    SlotObjekt[i].Zeichne(spriteBatch, SlotObjekt[i]);
                }
            HotbarSlotRahmen.Zeichne(spriteBatch, HotbarSlotRahmen);




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

                    for (int t = 0; t < smoothing; ++t)
                    {
                        if (BlockID[i, j] == 2 && blockInsgesamt < 12)
                        {
                            BlockID[i, j] = 1;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrasBlock);
                        }
                        else if (BlockID[i, j] == 1 && blockInsgesamt > 12)
                        {
                            BlockID[i, j] = 2;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWasserBlock);
                        }
                        else
                        {
                            switch (BlockID[i, j])
                            {
                                case 1:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrasBlock);
                                    break;
                                case 2:
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWasserBlock);
                                    break;
                            }
                        }
                    }

                }
            }
            //Extra
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    for (int t = 0; t < 1; ++t)
                    {
                        //WICHTIG!!!
                        if (BlockID[i, j] == 2) { break; }
                        for (int k = 0; k < 8; ++k)
                        {
                            if (GetTouchingBlockIDs(BlockID, i, j)[k] == 3)
                            {
                                if (random.Next(1, PKies / 10) == 1)
                                {
                                    BlockID[i, j] = 3;
                                    BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TKiesBlock);
                                }
                            }
                        }
                        if (random.Next(1, PKies) == 3)
                        {
                            BlockID[i, j] = 3;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TKiesBlock);
                        }
                        if (random.Next(1, 80) == 3)
                        {
                            BlockID[i, j] = 4;
                            BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrasWurzel);
                        }
                    }
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
                            Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TDekoMoos);
                        }
                        else if (random.Next(1, 10) == 3)
                        {
                            DekoID[i, j] = 3;
                            Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TDekoMoosStein);
                        }
                        else if (random.Next(1, 30) == 3)
                        {
                            DekoID[i, j] = 4;
                            Deko[i, j] = new Block(new Vector2(i * 64, j * 64), TDekoStein);
                        }
                    }
                    //Rand
                    else if (BlockID[i, j] == 2 || BlockID[i, j] == 3)
                    {
                        if (blockRight == 1 || blockRight == 4)
                        {
                            //Rechts
                            SetDeko(TGrasBlockRand1, 0, 0, 0);
                            if (blockDown == 1 || blockDown == 4)
                            {
                                //RechtsUnten
                                SetDeko(TGrasBlockRand2L, 0, 0, 0);
                            }
                            if (blockLeft == 1 || blockLeft == 4)
                            {
                                //RechtsLinks
                                SetDeko(TGrasBlockRand2H, 0, 0, 0);

                                if (blockDown == 1 || blockDown == 4)
                                {
                                    //RechtsUntenLinks
                                    SetDeko(TGrasBlockRand3, 0, 0, 0);
                                }
                            }
                            if (blockUp == 1 || blockUp == 4)
                            {
                                //RechtsOben
                                SetDeko(TGrasBlockRand2L, -90, 0, 64);

                                if (blockLeft == 1 || blockLeft == 4)
                                {
                                    //RechtsObenLinks
                                    SetDeko(TGrasBlockRand3, 180, 64, 64);
                                    if (blockDown == 1 || blockDown == 4)
                                    {
                                        //Ganz
                                        SetDeko(TGrasBlockRand4, 0, 0, 0);
                                    }
                                }
                                else if (blockDown == 1 || blockDown == 4)
                                {
                                    //RechtsObenUnten
                                    SetDeko(TGrasBlockRand3, -90, 0, 64);
                                }
                            }

                        }
                        else if (blockDown == 1 || blockDown == 4)
                        {
                            //Unten
                            SetDeko(TGrasBlockRand1, 90, 64, 0);

                            if (blockLeft == 1 || blockLeft == 4)
                            {
                                //UntenLinks
                                SetDeko(TGrasBlockRand2L, 90, 64, 0);
                            }
                            if (blockUp == 1 || blockUp == 4)
                            {
                                //UntenOben
                                SetDeko(TGrasBlockRand2H, -90, 0, 64);

                                if (blockLeft == 1 || blockLeft == 4)
                                {
                                    //UntenLinksOben
                                    SetDeko(TGrasBlockRand3, 90, 64, 0);
                                }
                            }
                        }
                        else if (blockLeft == 1 || blockLeft == 4)
                        {
                            //Links
                            SetDeko(TGrasBlockRand1, 180, 64, 64);

                            if (blockUp == 1 || blockUp == 4)
                            {
                                //LinksOben
                                SetDeko(TGrasBlockRand2L, 180, 64, 64);
                            }
                        }
                        else if (blockUp == 1 || blockUp == 4)
                        {
                            //Oben
                            SetDeko(TGrasBlockRand1, -90, 0, 64);
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
        private void GenerateObjekte()
        {
            int width = BlockID.GetLength(0);
            int height = BlockID.GetLength(1);

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    ObjekteLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TEmpty);

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
                        ObjekteLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TBaumStamm);
                        ObjekteID[i, j] = 1;

                        if (blockUp != 0)
                        {
                            ObjekteLayer[i, j - 1] = new Block(new Vector2(i * 64, (j - 1) * 64), TBaumStamm);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, 0, -2) != 0)
                        {
                            ObjekteLayer[i, j - 2] = new Block(new Vector2(i * 64, (j - 2) * 64), TBaumBlätter);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, 1, -2) != 0)
                        {
                            ObjekteLayer[i + 1, j - 2] = new Block(new Vector2((i + 1) * 64, (j - 2) * 64), TBaumBlätter);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, -1, -2) != 0)
                        {
                            ObjekteLayer[i - 1, j - 2] = new Block(new Vector2((i - 1) * 64, (j - 2) * 64), TBaumBlätter);
                        }
                        if (GetNeighborBlockID(BlockID, i, j, 0, -3) != 0)
                        {
                            ObjekteLayer[i, j - 3] = new Block(new Vector2(i * 64, (j - 3) * 64), TBaumBlätter);
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
            if (TDeko == TGrasBlockRand1)
            {
                DekoID[i, j] = 1f;
            }
            else if (TDeko == TGrasBlockRand2L)
            {
                DekoID[i, j] = 2.2f;
            }
            else if (TDeko == TGrasBlockRand2H)
            {
                DekoID[i, j] = 2.25f;
            }
            else if (TDeko == TGrasBlockRand3)
            {
                DekoID[i, j] = 3;
            }
            else if (TDeko == TGrasBlockRand4)
            {
                DekoID[i, j] = 4;
            }
            Deko[i, j] = new Block(new Vector2(i * 64 + xShift, j * 64 + yShift), TDeko);
            Deko[i, j].Rotation = MathHelper.ToRadians(rotation);
            Deko[i, j].Drehpunkt = new Vector2(0, 0);
        }

        public int hitArray(int[,] ID)
        {
            int x = (int)(MausPos.X / 64);
            int y = (int)(MausPos.Y / 64);


            if (MausPos.X >= 0 && MausPos.X < 64 * 64 && MausPos.Y >= 0 && MausPos.Y < 64 * 64)
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

        #endregion

        public bool InReach()
        {
            float minX = Player.Koordinaten.X - playerReichweite;
            float maxX = Player.Koordinaten.X + playerReichweite;

            if (MausPos.X >= minX && MausPos.X <= maxX)
            {
                float minY = Player.Koordinaten.Y - playerReichweite;
                float maxY = Player.Koordinaten.Y + playerReichweite;

                if (MausPos.Y >= minY && MausPos.Y <= maxY)
                {
                    return true;
                }
            }
            return false;
        }

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
*/