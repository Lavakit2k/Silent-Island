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

        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public int screenWidth { get; private set; }
        public int screenHeight { get; private set; }
        public int worldSizeX { get; private set; }
        public int worldSizeY { get; private set; }

        public Random random = new Random();

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
        public Item item { get; private set; }
        public Block block { get; private set; }
        public UI ui { get; private set; }
        public Entity entity { get; private set; }
        public Button button { get; private set; }

        #endregion

        #region Input/Update Management

        private KeyboardState keyboardState;
        private Vector2 MousePos;
        private MouseState mouseState;
        private MouseState previousMouseState;
        private MouseState currentMouseState;
        Keys[] keys;

        public double elapsedTime { get; private set; }
        public double UpdateInterval { get; private set; }
        public double tickCount { get; private set; }
        public double timeCounter { get; private set; }

        private double fps;
        private double FPStotalTime;
        private int frameCount;

        #endregion

        #region Camera Management

        public Vector2 cameraPosition { get; private set; }

        // Camera Scrolling (Unimplemented)
        // float zoom = 3.0f;
        // float cameraSpeed = 1.0f;

        #endregion

        #region Resources

        // Sound
        SoundEffect FishingRodOut;

        // Font
        public SpriteFont font { get; private set; }

        #endregion

        #region Gameplay Variables
 
        public int ToolHotbarSlotNum;
        public int ExtraHotbarSlotNum;
        public int fishingPointerOffset;

        #endregion

        #region Gameplay Booleans

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

        private bool debugMenu = false;
        private bool hitboxOn = false;

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
            spriteBatch = textures.spriteBatch;
            textures.LoadAllTextures();

            #endregion

            #region Object Initialization

            structure = new Structure();

            mainObjekt = new Objekt(textures, this);

            block = new Block(textures, this);
            block.LoadAllBlocks();

            item = new Item(textures, this);
            item.LoadAllItems();

            ui = new UI(textures, this);
            ui.LoadAllUIs();

            generation = new WorldGeneration(this, textures, structure, block);

            entity = new Entity(textures, this);
            entity.LoadAllEnitys(this);

            button = new Button(textures, this);
            button.LoadAllButton();

            #endregion
        }


        protected override void Update(GameTime gameTime)
        {
            #region Technik

            //Input
            keyboardState = Keyboard.GetState();
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            keys = keyboardState.GetPressedKeys();
            MousePos = new Vector2(currentMouseState.X + cameraPosition.X, currentMouseState.Y + cameraPosition.Y);
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

                #region Key


                bool isMoving = false;

                if (KeyDown(Keys.W) && entity.Player.pos.Y > 0 && !entity.Player.ColideLayer(block.BaseLayer, new Vector2(0, -entity.Player.speed)))
                {
                    entity.Player.MovePlayer(isMoving, textures.PlayerUp, 0, -entity.Player.speed);
                }

                if (KeyDown(Keys.S) && entity.Player.pos.Y < worldSizeY * 64 - 96 && !entity.Player.ColideLayer(block.BaseLayer, new Vector2(0, entity.Player.speed)))
                {
                    entity.Player.MovePlayer(isMoving, textures.PlayerDown, 0, entity.Player.speed);
                }

                if (KeyDown(Keys.A) && entity.Player.pos.X > 0 && !entity.Player.ColideLayer(block.BaseLayer, new Vector2(-entity.Player.speed, 0)))
                {
                    entity.Player.MovePlayer(isMoving, textures.PlayerLeft, -entity.Player.speed, 0);
                    lookingRight = false;
                }

                if (KeyDown(Keys.D) && entity.Player.pos.X < worldSizeX * 64 - 64 && !entity.Player.ColideLayer(block.BaseLayer, new Vector2(entity.Player.speed, 0)))
                {
                    entity.Player.MovePlayer(isMoving, textures.PlayerRight, entity.Player.speed, 0);
                    lookingRight = true;
                }



                moving = isMoving;
                #endregion

                #region Scrolling
                // UP
                if (currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
                {
                    ToolHotbarSlotNum = (ToolHotbarSlotNum - 1 + 4) % 4;                  
                }
                // DOWN
                else if (currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
                {
                    ToolHotbarSlotNum = (ToolHotbarSlotNum + 1) % 4;
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
                    ResetFishing();
                }

                //TODO Speichern
                if (KeyDown(Keys.O))
                {

                }
                if (KeyDown(Keys.P))
                {

                }
                if (KeyDown(Keys.E))
                {
                    ui.Inventory.activ = !ui.Inventory.activ;
                }

                update = false;
                #endregion

                #region Mouse/Delayed Keys

                //waiting
                if (timeCounter > 400)
                {

                    if (KeyDown(Keys.F3) && !KeyDown(Keys.B))
                    {
                        debugMenu = !debugMenu;
                        ui.DebugMenu.activ = !ui.DebugMenu.activ;
                        button.DebugLevelDesign.activ = !button.DebugLevelDesign.activ;
                        button.Test.activ = !button.Test.activ;
                        timeCounter = 0;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    if (KeyDown(Keys.F3) && KeyDown(Keys.B))
                    {
                        hitboxOn = !hitboxOn;
                        timeCounter = 0;
                    }
                    //leftclick
                    if (MouseKeyDown(1))
                    {
                        timeCounter = 0;
                        switch (ui.HandObjekt.ID)
                        {
                            //mainItem.FishingRod
                            case 1:
                                break;
                            default:
                                break;
                        }
                        button.CheckButtonHit(MousePos);
                        for (int i = 0; i < block.takeBlock.Length; i++)
                        {
                            if (block.takeBlock[i].hit(MousePos))
                            {
                                block.tokenBlock = block.takeBlock[i].Clone();
                            }
                        }
                        if (KeyDown(Keys.LeftAlt))
                        {
                            block.EditorModeGetBlock(MousePos);
                            timeCounter = 200;
                        }
                        if (KeyDown(Keys.LeftShift))
                        {
                            block.EditorModeSetBlock(MousePos);
                            timeCounter = 300;
                        }
                    }
                    //rightclick
                    else if (MouseKeyDown(2))
                    {
                        timeCounter = 0;

                        switch (ui.HandObjekt.ID)
                        {
                            case 1:
                                fishingPointerOffset = random.Next(-400, 400);
                                FishingUse();
                                timeCounter = 200;
                                break;
                            case 5:
                                ShovelUse();
                                timeCounter = 200;
                                break;
                            default:
                                break;
                        }

                    }
                }

                #endregion

                #region Positioning

                CameraMove();

                ui.UpdateAll(cameraPosition, item);
                item.UpdateAll(cameraPosition, ui);
                button.UpdateAll();

                if (debugMenu)
                {
                    block.EditorModeUpdate(cameraPosition);
                    sdebug = item.ToolHotbarItem[3].pos.ToString();
                    
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
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(new Vector3(-cameraPosition, 0)));

            #endregion

            //Layer
            block.ZeichneAll(spriteBatch);
            //Player
            entity.Player.Zeichne(spriteBatch);
            //CreativMode
            if (hitboxOn)   
                block.HitboxAllDraw(spriteBatch, textures.Pixel);               
            if (debugMenu)
                block.EditorModeDraw(spriteBatch);

            //UI
            ui.ZeichneAll(spriteBatch);
            if (hitboxOn)
                ui.HitboxAllDraw(spriteBatch, textures.Pixel);
            //Slot
            item.DrawItems(spriteBatch, font);
            //Button
            button.ZeichneAll(spriteBatch);
            

            #region end
            if (debug)
            {
                spriteBatch.Draw(textures.TestBlock, new Vector2(0, 0), Color.White);

            }
            if (sdebug != null)
            {
                spriteBatch.DrawString(font, sdebug, new Vector2(100, 100), new Color(243, 178, 92));
            }
            if (debugMenu)
            {
                spriteBatch.DrawString(font, "FPS: " + fps.ToString("0.00"), new Vector2(cameraPosition.X + 264, cameraPosition.Y + 32), Color.White);
                spriteBatch.DrawString(font, "X: " + entity.Player.pos.X.ToString("0.00") + "Y: " + entity.Player.pos.Y.ToString("0.00"), new Vector2(cameraPosition.X + 264, cameraPosition.Y + 64), Color.White);
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
            Vector2 targetCameraPosition = new Vector2(entity.Player.pos.X - halfScreenWidth, entity.Player.pos.Y - halfScreenHeight);

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
        public int hitLayerBlock()
        {
            int x = (int)(MousePos.X / 64);
            int y = (int)(MousePos.Y / 64);
            //TODO für mehrere Layer?

            if (MousePos.X >= 0 && MousePos.X < worldSizeX * 64 && MousePos.Y >= 0 && MousePos.Y < worldSizeY * 64)
                return block.BaseLayer[x, y].ID;


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
            //TODO with rect?
            float minX = entity.Player.pos.X + (entity.Player.texture.Width / 2) - (playerReichweiteX * 64);
            float maxX = entity.Player.pos.X + (entity.Player.texture.Width / 2) + (playerReichweiteX * 64);

            if (MousePos.X >= minX && MousePos.X <= maxX)
            {
                float minY = entity.Player.pos.Y + (entity.Player.texture.Height / 2) - (playerReichweiteY * 64);
                float maxY = entity.Player.pos.Y + (entity.Player.texture.Height / 2) + (playerReichweiteY * 64);

                if (MousePos.Y >= minY && MousePos.Y <= maxY)
                {
                    return true;
                }
            }
            return false;
        }
        public bool InHand(Item item)
        {
            if (ui.HandObjekt.ID == item.ID)
                return true;
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
                        item.Aufnehmen(item.Fish);
                }
                //green
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 + 24 || ui.FishingBarPointer.pos.X + 8 > ui.FishingBar.pos.X + 52 + 48 + 24 + 8)
                {
                    if (Chance(76))
                        item.Aufnehmen(item.Fish);
                }
                //blue
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 + 24 + 8)
                {
                    item.Aufnehmen(item.Fish);

                    if (Chance(20))
                        item.Aufnehmen(item.Shark);
                }

                ResetFishing();
            }
            else if (InReach(4, 2) && hitLayerBlock() == 2)
            {
                ui.HandObjekt.texture = textures.FishingRodOut;
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
            if (InReach(3, 1) && hitLayerBlock() == 3)
            {
                //TODO schaufel nach unten bewegen
                if (Chance(13))
                    item.Aufnehmen(item.SeaShell1);
                if (Chance(8))
                    item.Aufnehmen(item.SeaShell2);
                if (Chance(7))
                    item.Aufnehmen(item.SeaShell3);
                if (Chance(3))
                    item.Aufnehmen(item.SeaShell4);
            }
        }
        #endregion


    }
}