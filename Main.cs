using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silent_Island_PC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
        private int screenWidth;
        private int screenHeight;

        //Classes
        private Textures texture;
        private WorldGeneration generation;
        private Structure structure;

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

        #endregion

        #region Actions
        public bool action;
        public bool moving;
        public bool update;
        public bool inventoryOpen;
        public bool chestOpen;
        public bool angeln;
        public bool ausgeworfen;
        public bool lookingRight;
        public bool change;
        public bool layerPlaced;

        #endregion

        #region Camera

        Vector2 cameraPosition;
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
        UI Hotbar;
        UI HotbarSlot;
        UI Inventar;
        UI Top_UI;
        UI Map_Frame;
        UI ExtraSlot;
        UI AngelLeiste;
        UI AngelLeisteZeiger;

        Entity HandObjekt;
        Item[] SlotObjekt;
        
        public int HotbarSlotNum;

        #endregion

        #region Entity

        Entity Player;

        #endregion

        #region Items/Blocks

        Item Fishing_Rod;
        Item AngelSchnur;
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

            // Framerate auf 60 FPS begrenzen
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            // V-Sync aktivieren
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
            worldSizeX = 164;
            worldSizeY = 24;

            texture = new Textures(Content);
            structure = new Structure();

            font = Content.Load<SpriteFont>("font");
            fileSave = "C:\\Users\\simon\\source\\repos\\Silent Island PC\\Silent Island PC\\Content\\Speicher.txt";

            texture.Initialize(GraphicsDevice);
            spriteBatch = texture.spriteBatch;
            texture.LoadAllTextures();
            

            #region Sound

            FishingRodOut = Content.Load<SoundEffect>("Sounds/Fishing_Rod_out");

            #endregion

            #endregion

            #region Objects

            Player = new Entity(new Vector2(10, 10), texture.PlayerUp);
            Fishing_Rod = new Item(new Vector2(0, 0), texture.FishingRod);
            Player.speed = 20;

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
            mouseState = Mouse.GetState();
            Keys[] keys = keyboardState.GetPressedKeys();
            MousePos = new Vector2(mouseState.X + cameraPosition.X, mouseState.Y + cameraPosition.Y);
            //Time
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            tickCount += gameTime.ElapsedGameTime.TotalMilliseconds;
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
                    if (Taste(Keys.W) && Player.coords.Y > 0)
                    {
                        moving = true;
                        Player.texture = texture.PlayerUp;
                        Player.coords = new Vector2(Player.coords.X, Player.coords.Y - Player.speed);
                    }

                    if (Taste(Keys.S) && Player.coords.Y < worldSizeY * 64 - 96)
                    {
                        moving = true;
                        Player.texture = texture.PlayerDown;
                        Player.coords = new Vector2(Player.coords.X, Player.coords.Y + Player.speed);
                    }

                    if (Taste(Keys.A) && Player.coords.X > 0)
                    {
                        moving = true;
                        Player.texture = texture.PlayerLeft;
                        Player.coords = new Vector2(Player.coords.X - Player.speed, Player.coords.Y);

                        lookingRight = false;

                    }

                    if (Taste(Keys.D) && Player.coords.X < worldSizeX * 64 - 64)
                    {
                        moving = true;
                        Player.texture = texture.PlayerRight;
                        Player.coords = new Vector2(Player.coords.X + Player.speed, Player.coords.Y);

                        lookingRight = true;

                    }

                    moving = true;
                }

                if (moving)
                {
                    Fishing_Rod.texture = texture.FishingRod;
                    ausgeworfen = false;
                }

                //TODO Speichern
                if (Taste(Keys.O))
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
                if (Taste(Keys.P))
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
                if (Taste(Keys.E))
                {
                    if (Inventar.activ == false)
                    {
                        Inventar.activ = true;
                    }
                    else
                    {
                        Inventar.activ = false;
                    }


                }

                update = false;
                #endregion

                cameraPosition = new Vector2(Player.coords.X - (screenWidth / 2), Player.coords.Y - (screenHeight / 2));

                elapsedTime = 0;
            }
            #region Keys
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))Exit();


            #endregion

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(-cameraPosition, 0)));


            spriteBatch.Draw(texture.TestBlock, new Vector2(100, 100), Color.White);

            
            blockLayer.Zeichne(spriteBatch);
            dekoLayer.Zeichne(spriteBatch);
            structureLayer.Zeichne(spriteBatch);

            Player.Zeichne(spriteBatch);

            //debug
            if (debug)
            {
                spriteBatch.Draw(texture.TestBlock, new Vector2(0, 0), Color.White);
        
            }
            if (sdebug != null)
            {
                spriteBatch.DrawString(font, sdebug, new Vector2(100, 100), new Color(243, 178, 92));
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Methoden
        //TODO: clearInv

        public int hitArray(int[,] ID)
        {
            int x = (int)(MousePos.X / 64);
            int y = (int)(MousePos.Y / 64);


            if (MousePos.X >= 0 && MousePos.X < 64 * 64 && MousePos.Y >= 0 && MousePos.Y < 64 * 64)
                return ID[x, y];


            return 0;
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

        //InReach
        /*
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
        */

        #endregion
    }
}
