using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

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
        public Textures texture { get; private set; }
        public WorldGeneration generation { get; private set; }
        public Structure structure { get; private set; }
        public Objekt mainObjekt { get; private set; }
        public Item item { get; private set; }
        public Block block { get; private set; }
        public UI ui { get; private set; }
        public Entity entity { get; private set; }


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

 
        public int worldSizeX { get; private set; }
        public int worldSizeY { get; private set; }

        public Layer layer;

        public Entity HandObjekt;
        public Item[] SlotObjekt;

        public int HotbarSlotNum;


        Entity Player;
        public int angelSchnurRotation = 20;
        int fishingPointerOffset;

        Block Chair;

        #region Action bools
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

        SoundEffect FishingRodOut;

        public SpriteFont font { get; private set; }

        #endregion

        #endregion

        Button button;

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

            font = Content.Load<SpriteFont>("font");
            fileSave = "C:\\Users\\simon\\source\\repos\\Silent Island PC\\Silent Island PC\\Content\\Speicher.txt";

            //textures
            texture = new Textures(Content);
            texture.Initialize(GraphicsDevice);
            spriteBatch = texture.spriteBatch;
            texture.LoadAllTextures();

            layer = new Layer(texture, this);
            layer.LoadAllLayers();
            structure = new Structure();
            generation = new WorldGeneration(this, texture, layer, structure);

            //objekts
            mainObjekt = new Objekt(texture, this);
            
            item = new Item(texture, this);
            item.LoadAllItems();
            
            entity = new Entity(Vector2.Zero, texture.Empty);
            
            ui = new UI(texture, this);
            ui.LoadAllUIs();
            
            block = new Block(Vector2.Zero, texture.Empty);
            

            #region Sound

            FishingRodOut = Content.Load<SoundEffect>("Sounds/Fishing_Rod_out");

            #endregion

            #endregion

            InitUI();
            InitEntitys();

            button = new Button(texture, this);
            button.LoadAllButton();

            //Shark.Aufnehmen(SlotObjekt);
            //mainItem.Fish.Aufnehmen(SlotObjekt);
            item.FishingRod.Aufnehmen(SlotObjekt);
            
            item.Shovel.Aufnehmen(SlotObjekt);
            //mainItem.SeaShell1.Aufnehmen(SlotObjekt);
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
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                bool isMoving = false;

                if (KeyDown(Keys.W) && Player.pos.Y > 0 && !Player.ColideLayer(layer.blockLayer, new Vector2(0, -Player.speed)))
                {
                    Player.MovePlayer(isMoving, texture.PlayerUp, 0, -Player.speed);
                }

                if (KeyDown(Keys.S) && Player.pos.Y < worldSizeY * 64 - 96 && !Player.ColideLayer(layer.blockLayer, new Vector2(0, Player.speed)))
                {
                    Player.MovePlayer(isMoving, texture.PlayerDown, 0, Player.speed);
                }

                if (KeyDown(Keys.A) && Player.pos.X > 0 && !Player.ColideLayer(layer.blockLayer, new Vector2(-Player.speed, 0)))
                {
                    Player.MovePlayer(isMoving, texture.PlayerLeft, -Player.speed, 0);
                    lookingRight = false;
                }

                if (KeyDown(Keys.D) && Player.pos.X < worldSizeX * 64 - 64 && !Player.ColideLayer(layer.blockLayer, new Vector2(Player.speed, 0)))
                {
                    Player.MovePlayer(isMoving, texture.PlayerRight, Player.speed, 0);
                    lookingRight = true;
                }


                moving = isMoving;
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
                            //mainItem.FishingRod
                            case 1:
                                break;
                            default:
                                break;
                        }
                        if(button.myButton.hit(MousePos))
                        {
                            debug = true;
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
                                fishingPointerOffset = random.Next(-400, 400);
                                FishingUse();
                                break;
                            case 5:
                                ShovelUse();
                                break;
                            default:
                                break;
                        }
                    }
                }

                #endregion

                #region Positioning

                CameraMove();

                ui.Hotbar.UpdateUI(this, screenWidth / 2, screenHeight - 50);
                ui.HotbarMarker.UpdateUI(this, screenWidth / 2 - 216 + HotbarSlotNum * 72, screenHeight - 50);

                HandObjekt.pos = new Vector2(Player.pos.X + 30, Player.pos.Y);
                HandObjekt.texture = SlotObjekt[HotbarSlotNum].texture;

                //TODO wieder richtig
                ui.FishingBar.UpdateUI(this, screenWidth / 2 - texture.FishingBar.Width / 2 + 146, screenHeight - 120);
                if(ui.FishingBarPointer.activ)
                ui.FishingBarPointer.UpdateUI(this, screenWidth / 2 + 20 + 128f * (float)Math.Sin((timeCounter + fishingPointerOffset) / 1000 * 2f), screenHeight - 132);
                //                                 start                   amplitude               t             offset                         frequenz

                for (int i = 0; i < 7; i++)
                {
                    SlotObjekt[i].pos = new Vector2(ui.Hotbar.pos.X - 216 + i * 72, ui.Hotbar.pos.Y);
                }




                button.myButton.SetPosition(new Vector2(cameraPosition.X, cameraPosition.Y));
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

            layer.blockLayer.Zeichne();
            layer.dekoLayer.Zeichne();
            layer.structureLayer.Zeichne();

            #endregion

            Player.Zeichne(spriteBatch);
            item.FishingLine.Zeichne(spriteBatch);
            HandObjekt.Zeichne(spriteBatch);

            #region UI

            ui.Hotbar.Zeichne(spriteBatch);
            ui.HotbarMarker.Zeichne(spriteBatch);

            button.myButton.Zeichne(spriteBatch);


            for (int i = 0; i < 7; i++)
            {
                SlotObjekt[i].Zeichne(spriteBatch);
                spriteBatch.DrawString(font, "" + SlotObjekt[i].amount, new Vector2(SlotObjekt[i].pos.X + 24, SlotObjekt[i].pos.Y + 20), new Color(0, 0, 0));
            }

            ui.FishingBar.Zeichne(spriteBatch);
            ui.FishingBarPointer.Zeichne(spriteBatch);

            #endregion

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

        #region Init
        private void InitEntitys()
        {
            for (int i = 0; i < layer.blockLayer.IDLayer.GetLength(0); i++)
            {
                for (int j = 0; j < layer.blockLayer.IDLayer.GetLength(1); j++)
                {
                    if (layer.blockLayer.IDLayer[i, j] == 1 &&
                        i > 0 && layer.blockLayer.IDLayer[i - 1, j] == 1 && // links
                        i < layer.blockLayer.IDLayer.GetLength(0) - 1 && layer.blockLayer.IDLayer[i + 1, j] == 1 && // rechts
                        j > 0 && layer.blockLayer.IDLayer[i, j - 1] == 1 && // oben
                        j < layer.blockLayer.IDLayer.GetLength(1) - 1 && layer.blockLayer.IDLayer[i, j + 1] == 1) // unten
                    {
                        Player = new Entity(new Vector2(i * 64, j * 64), texture.PlayerUp);
                        Player.speed = 8;
                        return;
                    }
                }
            }


        }
        
        private void InitUI()
        {
            HandObjekt = new Entity(new Vector2(0, 0), texture.HotbarMarker);
            SlotObjekt = new Item[7];
            for (int i = 0; i < 7; i++)
            {
                SlotObjekt[i] = new Item(new Vector2(0, 0), texture.Empty, 0);
            }
        }

        #endregion

        public void CameraMove()
        {
            float halfScreenWidth = screenWidth / 2;
            float halfScreenHeight = screenHeight / 2;

            // Berechne die Zielposition der Kamera basierend auf der Spielerposition
            Vector2 targetCameraPosition = new Vector2(Player.pos.X - halfScreenWidth, Player.pos.Y - halfScreenHeight);

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
                return layer.blockLayer.IDLayer[x, y];


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
            float minX = Player.pos.X + (Player.texture.Width / 2) - (playerReichweiteX * 64);
            float maxX = Player.pos.X + (Player.texture.Width / 2) + (playerReichweiteX * 64);

            if (MousePos.X >= minX && MousePos.X <= maxX)
            {
                float minY = Player.pos.Y + (Player.texture.Height / 2) - (playerReichweiteY * 64);
                float maxY = Player.pos.Y + (Player.texture.Height / 2) + (playerReichweiteY * 64);

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
        public bool Chance(int probability)
        {
            if (random.Next(1, 101) <= probability)
            {
                return true;
            }
            return false;
        }
        //TODO: clearInv

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
                        item.Fish.Aufnehmen(SlotObjekt);
                }
                //green
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 + 24 || ui.FishingBarPointer.pos.X + 8 > ui.FishingBar.pos.X + 52 + 48 + 24 + 8)
                {
                    if (Chance(76))
                        item.Fish.Aufnehmen(SlotObjekt);
                }
                //blue
                else if (ui.FishingBarPointer.pos.X + 8 < ui.FishingBar.pos.X + 52 + 48 + 24 + 8)
                {
                    item.Fish.Aufnehmen(SlotObjekt);

                    if (Chance(20))
                        item.Shark.Aufnehmen(SlotObjekt);
                }

                ResetFishing();
            }
            else if (InReach(4, 2) && hitLayerBlock() == 2)
            {
                //TODO line
                item.FishingLine.texture = texture.FishingLine;
                item.FishingLine.rotation = (float)Math.Atan2(item.FishingRod.pos.Y - MousePos.Y, item.FishingRod.pos.X - MousePos.X);
                item.FishingLine.scale = new Vector2(Vector2.Distance(item.FishingRod.pos, MousePos), 1);
                HandObjekt.texture = texture.FishingRodOut;
                ui.FishingBar.activ = true;
                ui.FishingBarPointer.activ = true;

                fishing = true;
            }
        }
        public void ResetFishing()
        {
            item.FishingLine.activ = false;
            item.FishingRod.texture = texture.FishingRod;
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
                    item.SeaShell1.Aufnehmen(SlotObjekt);
                if (Chance(8))
                    item.SeaShell2.Aufnehmen(SlotObjekt);
                if (Chance(7))
                    item.SeaShell3.Aufnehmen(SlotObjekt);
                if (Chance(3))
                    item.SeaShell4.Aufnehmen(SlotObjekt);
            }
        }
        #endregion

        #endregion
    }
}