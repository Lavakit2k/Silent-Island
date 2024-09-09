using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Silent_Island
{
    public class Main : Game
    {
        #region Fields and Properties

        #region Technique Fields

        public GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch { get; set; }
        public static  int screenWidth { get; private set; }
        public static int screenHeight { get; private set; }
        public static int worldSizeX { get; private set; }
        public static int worldSizeY { get; private set; }

        public static Random random = new Random();

        // Debugging
        public bool debug;
        public string sdebug;

        // File Management
        public string fileSave;
        public List<string> SaveDirectory;

        #endregion

        #region Class Instances

        public Textures textures { get; private set; }
        public WorldGeneration generation { get; private set; }
        public Structure structure { get; private set; }
        public Objekt mainObjekt { get; private set; }
        public static Item item { get; private set; }
        public static Block block { get; private set; }
        public static UI ui { get; private set; }
        public static Entity entity { get; private set; }
        public static Button button { get; private set; }
        public static Inventory inventory { get; private set; }
        public InputManager inputManager { get; private set; }

        #endregion

        #region Input/Update Management

        
        

        private double elapsedTime;
        private double UpdateInterval;
        private double tickCount;
        public double timeCounter;

        private double fps;
        private double FPStotalTime;
        private int frameCount;

        #endregion

        #region Camera Management

        public static Vector2 cameraPosition { get; private set; }

        // Camera Scrolling (Unimplemented)
        // float zoom = 3.0f;
        // float cameraSpeed = 1.0f;

        #endregion

        #region Resources

        // Sound
        private SoundEffect FishingRodOut;

        // Font
        public static SpriteFont font { get; private set; }

        #endregion

        #region Gameplay Variables

        public static int ToolHotbarSlotNum;
        public static int ExtraHotbarSlotNum;
        public int fishingPointerOffset;
        

        #endregion

        #region Gameplay Booleans

        public bool fishing;
        public bool moving;

        public bool inventoryOpen;

        public bool debugMenu = false;
        public bool hitboxOn = false;

        #endregion

        public enum GameState
        {
            Menu,
            NewGame,
            LoadGame,
            Playing
        }

        #endregion


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;

            // Framerate = var FPS
            // TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            // V-Sync aktiv
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
        }

        protected override void Initialize()
        {
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            worldSizeX = 64;
            worldSizeY = 64;

            
            base.Initialize();
        }


        protected override void LoadContent()
        {
            #region General            

            font = Content.Load<SpriteFont>("font");
            fileSave = "C:\\Users\\simon\\source\\repos\\Silent Island PC\\Silent Island PC\\Content\\Speicher.txt";

            //Sound
            FishingRodOut = Content.Load<SoundEffect>("Sounds/Fishing_Rod_out");

            #endregion

            #region Texture

            textures = new Textures(Content);
            textures.Initialize(GraphicsDevice);
            textures.LoadAllTextures();

            #endregion

            #region Object Initialization

            structure = new Structure();

            mainObjekt = new Objekt(this);

            block = new Block(this);
            block.LoadAllBlocks();

            item = new Item(this);
            item.LoadAllItems();

            ui = new UI(this);
            ui.LoadAllUIs();

            generation = new WorldGeneration(this, structure, block);

            entity = new Entity(this);
            entity.LoadAllEnitys();

            button = new Button(this);
            button.LoadAllButton();

            inputManager = new InputManager(this);

            inventory = new Inventory(GraphicsDevice);

            inventory.AddItem(item.Fish, 100);
            inventory.AddItem(item.Shark);
            inventory.AddItem(item.FishingRod);
            inventory.AddItem(item.IronIngot);
            #endregion
        }


        protected override void Update(GameTime gameTime)
        {
            #region Technik
            inputManager.Update();

            //Time
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            timeCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
            tickCount += gameTime.ElapsedGameTime.TotalMilliseconds;
            UpdateInterval = 10;

            FPStotalTime += gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;

            if (FPStotalTime >= 1.0)
            {
                fps = frameCount / FPStotalTime;
                FPStotalTime -= 1.0; // Subtrahiere 1.0, um zu vermeiden, dass zu viel Zeit akkumuliert wird
                frameCount = 0;
            }

            #endregion

            if (elapsedTime >= UpdateInterval)
            {

                inputManager.Move();
                inputManager.LogicUpdate();

                if (timeCounter > 400)
                {
                    inputManager.Scrole();
                    inputManager.CheckKeyInputs();
                    inputManager.CheckMouseInputs(block);
                }


                #region Positioning

                CameraMove();

                ui.UpdateAll(item);
                item.UpdateAll();
                button.UpdateAll();
                inventory.UpdateInventoryInterface(InputManager.scrollDelta);

                if (debugMenu)
                {
                    block.EditorModeUpdate(cameraPosition);
                    //sdebug = inventory.Inventory[0].amount.ToString();

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
            //TODO samplerState in optionen veränderbar
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(new Vector3(-cameraPosition, 0)));

            #endregion

            //Layer
            block.ZeichneAll();
            //Player
            Entity.Player.Zeichne();
            //CreativMode
            if (hitboxOn)
                block.HitboxAllDraw(spriteBatch, Textures.Pixel);
            if (debugMenu)
                block.EditorModeDraw(spriteBatch);

            //UI
            ui.ZeichneAll();

            //Hitbox
            if (hitboxOn)
            {
                ui.HitboxAllDraw();
            }

            //Slot
            item.DrawItems();

            //Button
            button.ZeichneAll();

            //Inventory
            if (inventoryOpen)
                inventory.DrawInventoryInterface();
                #region end
            if (debug)
            {
                spriteBatch.Draw(Textures.TestBlock, new Vector2(0, 0), Color.White);

            }
            if (sdebug != null)
            {
                spriteBatch.DrawString(font, sdebug, new Vector2(100, 100), new Color(243, 178, 92));
            }
            if (debugMenu)
            {
                spriteBatch.DrawString(font, "FPS: " + fps.ToString("0.00"), new Vector2(cameraPosition.X + 264, cameraPosition.Y + 32), Color.White);
                spriteBatch.DrawString(font, "X: " + Entity.Player.pos.X.ToString("0.00") + "Y: " + Entity.Player.pos.Y.ToString("0.00"), new Vector2(cameraPosition.X + 264, cameraPosition.Y + 64), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
            #endregion
        }

        #region Methoden
        public void CameraMove()
        {
            float halfScreenWidth = screenWidth / 2;
            float halfScreenHeight = screenHeight / 2;

            // Berechne die Zielposition der Kamera basierend auf der Spielerposition
            Vector2 targetCameraPosition = new Vector2(Entity.Player.pos.X - halfScreenWidth, Entity.Player.pos.Y - halfScreenHeight);

            // Begrenze die Zielposition der Kamera innerhalb der Weltgrenzen
            if (targetCameraPosition.X < 0)
            {
                targetCameraPosition.X = 0;
            }
            else if (targetCameraPosition.X > worldSizeX * 64 - screenWidth)
            {
                targetCameraPosition.X = worldSizeX * 64 - screenWidth;
            }

            if (targetCameraPosition.Y < 0)
            {
                targetCameraPosition.Y = 0;
            }
            else if (targetCameraPosition.Y > worldSizeY * 64 - screenHeight)
            {
                targetCameraPosition.Y = worldSizeY * 64 - screenHeight;
            }

            // Anpassbare Geschwindigkeit für die Kamera-Verzögerung
            float cameraSpeed = 0.1f;

            // Interpoliere die Kamera-Position schrittweise in Richtung der Zielposition
            cameraPosition = Vector2.Lerp(cameraPosition, targetCameraPosition, cameraSpeed);
        }
        
        
        
        public bool InReach(float playerReichweiteX, float playerReichweiteY)
        {
            //TODO with rect?
            float minX = Entity.Player.pos.X + (Entity.Player.texture.Width / 2) - (playerReichweiteX * 64);
            float maxX = Entity.Player.pos.X + (Entity.Player.texture.Width / 2) + (playerReichweiteX * 64);

            if (InputManager.MousePos.X >= minX && InputManager.MousePos.X <= maxX)
            {
                float minY = Entity.Player.pos.Y + (Entity.Player.texture.Height / 2) - (playerReichweiteY * 64);
                float maxY = Entity.Player.pos.Y + (Entity.Player.texture.Height / 2) + (playerReichweiteY * 64);

                if (InputManager.MousePos.Y >= minY && InputManager.MousePos.Y <= maxY)
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool Chance(int probability)
        {
            if (random.Next(1, 101) <= probability)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region item use
        public void FishingUse()
        {
            if (fishing)
            {
                //TODO this right
                //red
                if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 || ui.FishingBarPointer.pos.X + 8 > ui.FishingBar.pos.X + 52 + 48 + 24 + 8 + 24 + 48)
                {

                }
                //yellow
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 || ui.FishingBarPointer.pos.X + 8 > ui.FishingBar.pos.X + 52 + 48 + 24 + 8 + 24)
                {
                    if (Chance(33))
                        inventory.AddItem(item.Fish);
                }
                //green
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 + 24 || ui.FishingBarPointer.pos.X + 8 > ui.FishingBar.pos.X + 52 + 48 + 24 + 8)
                {
                    if (Chance(76))
                        inventory.AddItem(item.Fish);
                }
                //blue
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 + 24 + 8)
                {
                    inventory.AddItem(item.Fish);

                    if (Chance(20))
                        inventory.AddItem(item.Shark);
                }

                ResetFishing();
            }
            else if (InReach(4, 2) && inputManager.hitLayerBlock() == 2)
            {
                UI.HandObjekt.texture = Textures.FishingRodOut;
                ui.FishingBar.activ = true;
                ui.FishingBarPointer.activ = true;
                FishingRodOut.Play();
                fishing = true;
            }
        }
        public void ResetFishing()
        {
            ui.FishingBar.activ = false;
            ui.FishingBarPointer.activ = false;
            fishing = false;
        }
        public void ShovelUse()
        {
            if (InReach(3, 1) && inputManager.hitLayerBlock() == 3)
            {
                //TODO schaufel nach unten bewegen
                if (Chance(13))
                    inventory.AddItem(item.SeaShell1);
                if (Chance(8))
                    inventory.AddItem(item.SeaShell2);
                if (Chance(7))
                    inventory.AddItem(item.SeaShell3);
                if (Chance(3))
                    inventory.AddItem(item.SeaShell4);
            }
        }
        #endregion


    }
}