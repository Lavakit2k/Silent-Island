using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Silent_Island
{
    //TODO static?
    public class InputManager
    {
        private Main main;
        public static int scrollDelta;

        public static Vector2 MousePos { get; private set; }
        public KeyboardState keyboardState;
        public MouseState mouseState;
        public MouseState previousMouseState;
        public MouseState currentMouseState;
        private Keys[] keys;
        public InputManager(Main main)
        {
            this.main = main;
        }
        public void Update()
        {
            keyboardState = Keyboard.GetState();
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            keys = keyboardState.GetPressedKeys();
            MousePos = new Vector2(currentMouseState.X + Main.cameraPosition.X, currentMouseState.Y + Main.cameraPosition.Y);
        }
        public int hitLayerBlock()
        {
            int x = (int)(MousePos.X / 64);
            int y = (int)(MousePos.Y / 64);
            //TODO für mehrere Layer?

            if (MousePos.X >= 0 && MousePos.X < Main.worldSizeX * 64 && MousePos.Y >= 0 && MousePos.Y < Main.worldSizeY * 64)
                return Block.BaseLayer[x, y].ID;


            return 0;
        }
        public void Move()
        {
            bool isMoving = false;

            if (KeyDown(Keys.W) && Entity.Player.pos.Y > 0 && !Entity.Player.ColideLayer(Block.BaseLayer, new Vector2(0, -Entity.Player.speed)))
            {
                Entity.Player.MovePlayer(isMoving, Textures.PlayerUp, 0, -Entity.Player.speed);
            }

            if (KeyDown(Keys.S) && Entity.Player.pos.Y < Main.worldSizeY * 64 - 96 && !Entity.Player.ColideLayer(Block.BaseLayer, new Vector2(0, Entity.Player.speed)))
            {
                Entity.Player.MovePlayer(isMoving, Textures.PlayerDown, 0, Entity.Player.speed);
            }

            if (KeyDown(Keys.A) && Entity.Player.pos.X > 0 && !Entity.Player.ColideLayer(Block.BaseLayer, new Vector2(-Entity.Player.speed, 0)))
            {
                Entity.Player.MovePlayer(isMoving, Textures.PlayerLeft, -Entity.Player.speed, 0);
            }

            if (KeyDown(Keys.D) && Entity.Player.pos.X < Main.worldSizeX * 64 - 64 && !Entity.Player.ColideLayer(Block.BaseLayer, new Vector2(Entity.Player.speed, 0)))
            {
                Entity.Player.MovePlayer(isMoving, Textures.PlayerRight, Entity.Player.speed, 0);
            }
            main.moving = isMoving;
        }
        public void Scrole()
        {
            scrollDelta = 0;
            // UP
            if (currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
            {

                if (main.inventoryOpen)
                {
                    scrollDelta = -1;  // nach oben scrollen
                }
                else
                    Main.ToolHotbarSlotNum = (Main.ToolHotbarSlotNum - 1 + 4) % 4;
            }
            // DOWN
            else if (currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue)
            {
                if (main.inventoryOpen)
                {
                    scrollDelta = 1;  // nach unten scrollen
                }
                else
                    Main.ToolHotbarSlotNum = (Main.ToolHotbarSlotNum + 1) % 4;

            }

            // Aktualisiere `previousMouseState` nur, wenn das Mausrad tatsächlich gescrollt wurde
            if (currentMouseState.ScrollWheelValue != previousMouseState.ScrollWheelValue)
            {
                previousMouseState = currentMouseState;
            }
        }
        public void CheckKeyInputs()
        {
            if (KeyDown(Keys.F3) && !KeyDown(Keys.B))
            {
                main.debugMenu = !main.debugMenu;
                UI.DebugMenu.activ = !UI.DebugMenu.activ;
                main.timeCounter = 0;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                main.Exit();
            }
            if (KeyDown(Keys.F3) && KeyDown(Keys.B))
            {
                main.hitboxOn = !main.hitboxOn;
                main.timeCounter = 0;
            }
            if (KeyDown(Keys.E))
            {
                main.inventoryOpen = !main.inventoryOpen;
                main.timeCounter = 200;
                Inventory.scrollOffset = 0;
            }
            //TODO Speichern
            if (KeyDown(Keys.O))
            {

            }
            if (KeyDown(Keys.P))
            {

            }
        }
        public void CheckMouseInputs(Block block)
        {
            //leftclick
            if (MouseKeyDown(1))
            {
                main.timeCounter = 0;
                switch (UI.HandObjekt.ID)
                {
                    //mainItem.FishingRod
                    case 1:
                        break;
                    default:
                        break;
                }

                Button.CheckButtonHit(InputManager.MousePos);

                for (int i = 0; i < block.takeBlock.Length; i++)
                {
                    if (block.takeBlock[i].hit())
                    {
                        block.tokenBlock = block.takeBlock[i].Clone();
                    }
                }
                if (KeyDown(Keys.LeftAlt))
                {
                    block.EditorModeGetBlock(InputManager.MousePos);
                    main.timeCounter = 200;
                }
                if (KeyDown(Keys.LeftShift))
                {
                    block.EditorModeSetBlock(InputManager.MousePos);
                    main.timeCounter = 300;
                }
            }
            //rightclick
            else if (MouseKeyDown(2))
            {
                main.timeCounter = 0;

                switch (UI.HandObjekt.ID)
                {
                    case 1:
                        main.fishingPointerOffset = Main.random.Next(-400, 400);
                        main.FishingUse();
                        main.timeCounter = 200;
                        break;
                    case 5:
                        main.ShovelUse();
                        main.timeCounter = 200;
                        break;
                    default:
                        break;
                }

            }
        }
        public void LogicUpdate()
        {
            if (main.moving)
            {
                main.ResetFishing();
            }
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
    }
}
