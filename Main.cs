using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silent_Island_PC;
using System;
using System.Collections.Generic;
using System.IO;
//using System.Reflection.Emit;
//using System.Numerics;
//using System.Security.Claims;
//using System.Threading.Channels;

namespace Silent_Island
{
    public class Main : Game
    {
        #region Variables

        #region Technique

        #region Values

        //Graphics
        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public int screenWidth { get; private set; }
        public int screenHeight { get; private set; }

        //Classes
        private Textures texture;
        private WorldGeneration generation;
        private Structure structure;
        //private UI ui;

        private Random random = new Random();

        //debuging
        public bool debug;
        public string sdebug;

        //files
        public string fileSave;
        public List<string> SaveDirectory;

        #endregion

        #region Input/Update

        private KeyboardState keyboardState;
        private Vector2 MousePos;
        private MouseState mouseState;
        private MouseState previousMouseState;
        private MouseState currentMouseState;

        private double elapsedTime;
        private double UpdateInterval;
        private double tickCount;
        private double timeCounter;


        #endregion

        #region Camera

        public Vector2 cameraPosition { get; private set; }
        //TODO Camera scroling?
        /*
        float zoom = 3.0f;
        float cameraSpeed = 1.0f;
        */
        #endregion

        #endregion

        #region Layers
        //TODO Layer der nur visuel ist 
        public int worldSizeX { get; private set; }
        public int worldSizeY { get; private set; }

        public Block[,] BlockLayer { get; set; }
        public int[,] BlockID { get; set; }
        public Block[,] DekoLayer { get; set; }
        public int[,] DekoID { get; set; }
        public Block[,] StructurLayer { get; set; }
        public int[,] StructerID { get; set; }
        public Block[,] ItemLayer { get; set; }
        public int[,] ItemID { get; set; }

        public Layer blockLayer { get; set; }
        public Layer structureLayer { get; set; }
        public Layer dekoLayer { get; set; }
        public Layer itemLayer { get; set; }
        #endregion

        #region Objects

        #region UI
        public UI Hotbar;
        public UI Hotbar_Marker;
        public UI Inventory;
        public UI Top_UI;
        public UI Map_Frame;
        public UI Extra_Hotbar;
        public UI Extra_Hotbar_Marker;
        public UI Fishing_Bar;
        public UI Fishing_Bar_Pointer;

        public Entity HandObjekt;
        public Item[] SlotObjekt;

        public int HotbarSlotNum;

        #endregion

        #region Entity

        Entity Player;

        #endregion

        #region Items/Blocks

        Item Fishing_Rod;
        Item Fishing_Line;
        public int angelSchnurRotation = 20;
        Item Fish;
        Item Shark;
        Item Pistol;
        Item Rock;
        Item IronBar;
        Item Wood;
        Block Chair;
        #endregion

        #endregion

        #region Actions
        public bool fishing;
        public bool moving;

        public bool action;
        public bool update;
        public bool inventoryOpen;
        public bool chestOpen;

        public bool ausgeworfen;
        public bool lookingRight;
        public bool change;
        public bool layerPlaced;

        #endregion

        #region Resources

        #region Sound

        SoundEffect FishingRodOut;

        #endregion

        #region Rest

        private SpriteFont font;

        #endregion

        #endregion

        #endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;

            // Framerate = 60 FPS 
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            // V-Sync aktiv
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;


        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            #region Technique

            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            worldSizeX = 64;
            worldSizeY = 64;

            texture = new Textures(Content);
            structure = new Structure();
            //ui = new UI(texture, Vector2.Zero, texture.Empty);

            font = Content.Load<SpriteFont>("font");
            fileSave = "C:\\Users\\simon\\source\\repos\\Silent Island PC\\Silent Island PC\\Content\\Speicher.txt";

            //Load textures
            texture.Initialize(GraphicsDevice);
            spriteBatch = texture.spriteBatch;
            texture.LoadAllTextures();


            #region Sound

            FishingRodOut = Content.Load<SoundEffect>("Sounds/Fishing_Rod_out");

            #endregion

            #endregion

            #region Objects

            #region UI

            Hotbar = new UI(new Vector2(0, 0), texture.Hotbar);
            Hotbar.color = new Color(255,255,255, 0.5f);
            Hotbar_Marker = new UI(new Vector2(0, 0), texture.HotbarMarker);
            Inventory = new UI(new Vector2(0, 0), texture.Inventory);
            Top_UI = new UI(new Vector2(0, 0), texture.TopUI);
            Map_Frame = new UI(new Vector2(0, 0), texture.Map);
            Extra_Hotbar = new UI(new Vector2(0, 0), texture.ExtraHotbar);
            Extra_Hotbar_Marker = new UI(new Vector2(0, 0), texture.HotbarMarker);
            Fishing_Bar = new UI(new Vector2(0, 0), texture.FishingBar);
            Fishing_Bar_Pointer = new UI(new Vector2(0, 0), texture.FishingBarPointer);

            HandObjekt = new Entity(new Vector2(0, 0), texture.HotbarMarker);
            SlotObjekt = new Item[7];
            for (int i = 0; i < 7; i++)
            {
                SlotObjekt[i] = new Item(new Vector2(0, 0), texture.Empty);
            }

            #endregion

            Player = new Entity(new Vector2(0, 0), texture.PlayerUp);
            Player.speed = 10;
            Fishing_Rod = new Item(Vector2.Zero, texture.FishingRod);
            Fishing_Rod.ID = 1;
            Fish = new Item(Vector2.Zero, texture.Fish);
            Fish.ID = 2;
            Shark = new Item(Vector2.Zero, texture.Shark);
            Shark.ID = 3;
            
            //Shark.Aufnehmen(SlotObjekt);
            //Fish.Aufnehmen(SlotObjekt);
            Fishing_Rod.Aufnehmen(SlotObjekt);
            Fishing_Bar = new UI(Vector2.Zero, texture.Empty);
            Fishing_Bar_Pointer = new UI(Vector2.Zero, texture.Empty);
            Fishing_Line = new Item(Vector2.Zero, texture.Empty);

            #endregion

            #region Layer

            BlockLayer = new Block[worldSizeX, worldSizeY];
            BlockID = new int[worldSizeX, worldSizeY];
            DekoLayer = new Block[worldSizeX, worldSizeY];
            DekoID = new int[worldSizeX, worldSizeY];
            StructurLayer = new Block[worldSizeX, worldSizeY];
            StructerID = new int[worldSizeX, worldSizeY];
            ItemLayer = new Block[worldSizeX, worldSizeY];
            ItemID = new int[worldSizeX, worldSizeY];

            generation = new WorldGeneration(this, texture, structure);

            blockLayer = generation.GenerateBase(BlockLayer, BlockID);
            dekoLayer = generation.GenerateDeko(DekoLayer, DekoID);
            structureLayer = generation.GenerateStructers(StructurLayer, StructerID);

            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            #region Technik

            //Input
            keyboardState = Keyboard.GetState();
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            Keys[] keys = keyboardState.GetPressedKeys();
            MousePos = new Vector2(currentMouseState.X + cameraPosition.X, currentMouseState.Y + cameraPosition.Y);
            //Time
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            timeCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
            tickCount += gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateInterval = 10;


            #endregion

            if (elapsedTime >= UpdateInterval)
            {

                #region Key
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

                if (!KeyDown(Keys.W) && !KeyDown(Keys.S) && !KeyDown(Keys.A) && !KeyDown(Keys.D))
                {
                    moving = false;
                }
                else
                {
                    if (KeyDown(Keys.W) && Player.coords.Y > 0)
                    {
                        Player.MovePlayer(moving, Player, texture.PlayerUp, 0, -Player.speed);

                    }

                    if (KeyDown(Keys.S) && Player.coords.Y < worldSizeY * 64 - 96)
                    {
                        Player.MovePlayer(moving, Player, texture.PlayerDown, 0, Player.speed);
                    }

                    if (KeyDown(Keys.A) && Player.coords.X > 0)
                    {
                        Player.MovePlayer(moving, Player, texture.PlayerLeft, -Player.speed, 0);
                        //TODO über Texturen?/ Bitmap
                        lookingRight = false;
                    }

                    if (KeyDown(Keys.D) && Player.coords.X < worldSizeX * 64 - 64)
                    {
                        Player.MovePlayer(moving, Player, texture.PlayerRight, Player.speed, 0);

                        lookingRight = true;

                    }

                    moving = true;
                }
                #endregion

                #region Scrolling
                // UP
                if (currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
                {
                    HotbarSlotNum = (HotbarSlotNum + 1) % 7;
                }
                // DOWN
                else if (currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
                {
                    HotbarSlotNum = (HotbarSlotNum - 1 + 7) % 7;
                }

                // Aktualisiere `previousMouseState` nur, wenn das Mausrad tatsächlich gescrollt wurde
                if (currentMouseState.ScrollWheelValue != previousMouseState.ScrollWheelValue)
                {
                    previousMouseState = currentMouseState;
                }
                #endregion

                #region Logic
                if (moving)
                {
                    Fishing_Line.texture = texture.Empty;
                    Fishing_Rod.texture = texture.FishingRod;
                    Fishing_Bar.texture = texture.Empty;
                    Fishing_Bar_Pointer.texture = texture.Empty;
                    fishing = false;
                }

                //TODO Speichern
                if (KeyDown(Keys.O))
                {

                    List<string> speicherInhalt = new List<string>();
                    /*
                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            speicherInhalt.Add(layer.BlockID[i, j].ToString());

                        }
                    }
                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            speicherInhalt.Add(layer.DekoID[i, j].ToString());
                        }
                    }
                    */
                    File.WriteAllLines(fileSave, speicherInhalt);


                }
                if (KeyDown(Keys.P))
                {

                    /*
                    for (int i = 0; i < 64; ++i)
                    {
                        for (int j = 0; j < 64; ++j)
                        {
                            layer.BlockID[i, j] = int.Parse(File.ReadLines(fileSave).Skip(j + i).First());
                            switch (layer.BlockID[i, j])
                            {
                                case 1:
                                    layer.BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGrassBlock);
                                    break;
                                case 2:
                                    layer.BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TWaterBlock);
                                    break;
                                case 3:
                                    layer.BlockLayer[i, j] = new Block(new Vector2(i * 64, j * 64), TGravelBlock);
                                    break;
                            }
                        }
                    }
                    */


                }
                if (KeyDown(Keys.E))
                {
                    if (Inventory.activ == false)
                    {
                        Inventory.activ = true;
                    }
                    else
                    {
                        Inventory.activ = false;
                    }


                }

                update = false;
                #endregion

                #region Mouse

                //waiting
                if (timeCounter > 500)
                {

                    //leftclick
                    if (MouseKeyDown(1))
                    {
                        timeCounter = 0;
                        switch (SlotObjekt[HotbarSlotNum].ID)
                        {
                            //Fishing_Rod
                            case 1:
                                break;
                            default:
                                break;
                        }
                    }
                    //rightclick
                    else if (MouseKeyDown(2))
                    {
                        timeCounter = 0;
                        //TODO was wenn NPC da steht?
                        switch (SlotObjekt[HotbarSlotNum].ID)
                        {
                            case 1:
                                FishingUse();
                                break;
                            default:
                                break;
                        }
                    }
                }




                #endregion


                #region Positioning

                CameraMove();

                Hotbar.UpdateUI(this, screenWidth / 2, screenHeight - 50);
                Hotbar_Marker.UpdateUI(this, screenWidth / 2 - 216 + HotbarSlotNum * 72, screenHeight - 50);

                HandObjekt.coords = new Vector2(Player.coords.X + 30, Player.coords.Y);
                HandObjekt.texture = SlotObjekt[HotbarSlotNum].texture;

                Fishing_Bar.UpdateUI(this, screenWidth / 2 - Fishing_Bar.texture.Width / 2 + 28, screenHeight - 120);

                Fishing_Bar_Pointer.UpdateUI(this, screenWidth / 2 + 20 + 128f * (float)Math.Sin(timeCounter / 1000 * 2f), screenHeight - 132);
                //                                 //start                //amplitude            //t                  //frequenz

                for (int i = 0; i < 7; i++)
                {
                    SlotObjekt[i].coords = new Vector2(Hotbar.coords.X - 216 + i * 72, Hotbar.coords.Y);
                }

                #endregion
                elapsedTime = 0;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region start

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(-cameraPosition, 0)));

            #endregion

            #region Layer

            blockLayer.Zeichne(spriteBatch);
            dekoLayer.Zeichne(spriteBatch);
            structureLayer.Zeichne(spriteBatch);

            #endregion

            Player.Zeichne(spriteBatch);
            Fishing_Line.Zeichne(spriteBatch);
            HandObjekt.Zeichne(spriteBatch);

            #region UI

            Hotbar.Zeichne(spriteBatch);
            Hotbar_Marker.Zeichne(spriteBatch);

            
            for (int i = 0; i < 7; i++)
            {
                SlotObjekt[i].Zeichne(spriteBatch);
                spriteBatch.DrawString(font, "" + SlotObjekt[i].amount, new Vector2(SlotObjekt[i].coords.X + 24, SlotObjekt[i].coords.Y + 20), new Color(255, 255, 255));
            }
            #endregion

            Fishing_Bar.Zeichne(spriteBatch);
            Fishing_Bar_Pointer.Zeichne(spriteBatch);
            

            #region end
            if (debug)
            {
                spriteBatch.Draw(texture.TestBlock, new Vector2(0, 0), Color.White);

            }
            if (sdebug != null)
            {
                spriteBatch.DrawString(font, sdebug, new Vector2(100, 100), new Color(243, 178, 92));
            }

            //spriteBatch.Draw(texture.TestBlock, new Vector2(100, 100), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
            #endregion
        }

        #region Methoden
        //TODO: clearInv

        public void CameraMove()
        {
            float halfScreenWidth = screenWidth / 2;
            float halfScreenHeight = screenHeight / 2;

            // Berechne die Zielposition der Kamera basierend auf der Spielerposition
            Vector2 targetCameraPosition = new Vector2(Player.coords.X - halfScreenWidth, Player.coords.Y - halfScreenHeight);

            // Begrenze die Zielposition der Kamera innerhalb der Weltgrenzen
            if (targetCameraPosition.X < -32)
            {
                targetCameraPosition.X = -32;
            }
            else if (targetCameraPosition.X > worldSizeX * 64 - screenWidth - 32)
            {
                targetCameraPosition.X = worldSizeX * 64 - screenWidth - 32;
            }

            if (targetCameraPosition.Y < -32)
            {
                targetCameraPosition.Y = -32;
            }
            else if (targetCameraPosition.Y > worldSizeY * 64 - screenHeight - 32)
            {
                targetCameraPosition.Y = worldSizeY * 64 - screenHeight - 32;
            }

            // Anpassbare Geschwindigkeit für die Kamera-Verzögerung
            float cameraSpeed = 0.1f;

            // Interpoliere die Kamera-Position schrittweise in Richtung der Zielposition
            cameraPosition = Vector2.Lerp(cameraPosition, targetCameraPosition, cameraSpeed);
        }

        public int hitLayerBlock()
        {
            int x = (int)(MousePos.X / 64);
            int y = (int)(MousePos.Y / 64);
            //TODO für mehrere Layer?

            if (MousePos.X >= 0 && MousePos.X < worldSizeX * 64 && MousePos.Y >= 0 && MousePos.Y < worldSizeY * 64)
                return blockLayer.IDLayer[x, y];


            return 0;
        }
        public bool KeyDown(Keys key)
        {
            if (keyboardState.IsKeyDown(key))
            {
                return true;
            }
            else { return false; }
        }
        public bool MouseKeyDown(int button)
        {
            if (button == 1)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
            }
            else if (button == 2)
            {
                return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
            }
            return false;
        }
        public bool InReach(float playerReichweiteX, float playerReichweiteY)
        {
            float minX = Player.coords.X + (Player.texture.Width / 2) - (playerReichweiteX * 64);
            float maxX = Player.coords.X + (Player.texture.Width / 2) + (playerReichweiteX * 64);

            if (MousePos.X >= minX && MousePos.X <= maxX)
            {
                float minY = Player.coords.Y + (Player.texture.Height / 2) - (playerReichweiteY * 64);
                float maxY = Player.coords.Y + (Player.texture.Height / 2) + (playerReichweiteY * 64);

                if (MousePos.Y >= minY && MousePos.Y <= maxY)
                {
                    return true;
                }
            }
            return false;
        }
        public bool InHand(Item item)
        {
            if (SlotObjekt[HotbarSlotNum].ID == item.ID)
                return true;
            return false;
        }

        #region item use
        public void FishingUse()
        {
            if (fishing)
            {
                //red
                if (Fishing_Bar_Pointer.coords.X < Fishing_Bar.coords.X + 52 || Fishing_Bar_Pointer.coords.X > Fishing_Bar.coords.X + 52 + 48 + 24 + 8 + 24 + 48)
                {
                    
                }
                //yellow
                else if (Fishing_Bar_Pointer.coords.X < Fishing_Bar.coords.X + 52 + 48 || Fishing_Bar_Pointer.coords.X > Fishing_Bar.coords.X + 52 + 48 + 24 + 8 + 24)
                {
                    //33%
                    if(random.Next(1, 3) == 1)
                    Fish.Aufnehmen(SlotObjekt);
                }
                //green
                else if (Fishing_Bar_Pointer.coords.X < Fishing_Bar.coords.X + 52 + 48 + 24 || Fishing_Bar_Pointer.coords.X > Fishing_Bar.coords.X + 52 + 48 + 24 + 8)
                {
                    // 80 %
                    if (random.Next(1, 5) < 5)
                        Fish.Aufnehmen(SlotObjekt);
                }
                //blue
                else if (Fishing_Bar_Pointer.coords.X < Fishing_Bar.coords.X + 52 + 48 + 24 + 8)
                {
                    //100%
                    Fish.Aufnehmen(SlotObjekt);
                    // 20%
                    if (random.Next(1, 5) == 1)
                        Shark.Aufnehmen(SlotObjekt);
                }
                
                Fishing_Line.texture = texture.Empty;
                Fishing_Rod.texture = texture.FishingRod;
                Fishing_Bar.texture = texture.Empty;
                Fishing_Bar_Pointer.texture = texture.Empty;
                fishing = false;
            }
            else if (InReach(3, 1) && hitLayerBlock() == 2)
            {
                //TODO line
                Fishing_Line.texture = texture.FishingLine;
                Fishing_Line.rotation = (float)Math.Atan2(Fishing_Rod.coords.Y - MousePos.Y, Fishing_Rod.coords.X - MousePos.X);
                Fishing_Line.scale = new Vector2(Vector2.Distance(Fishing_Rod.coords, MousePos), 1);
                Fishing_Rod.texture = texture.FishingRodOut;
                Fishing_Bar.texture = texture.FishingBar;
                Fishing_Bar_Pointer.texture = texture.FishingBarPointer;
                fishing = true;
            }
        }
        #endregion
        #endregion
    }
}
