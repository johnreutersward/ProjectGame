using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Media;

using xTile;
using xTile.Dimensions;
using xTile.Display;

namespace ProjectGame
{
    using Microsoft.Xna.Framework.Input;
    using xTile.Layers;
    using xTile.Tiles;

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private Input input;
        public static GameStates gamestate;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
    
        // octo knight
        Char myChar;

        // menu & char screen
        private SpriteFont text;
        private Menu menu;
        private Settings settings;
        private ChooseChar choosechar;

        // xTile map, display device reference and rendering viewport (this is pretty awesome!)
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        int windowWidth;
        int windowHeight;


        Microsoft.Xna.Framework.Rectangle Collisionbox;

        public enum GameStates
        {
            MainMenu,
            Game,
            Settings,
            ChooseCharacter,
            End
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            myChar = new Char();
            

            // Why these values? Well 32 * 25 = 800 & 32 * 15 = 480 so it all works out! The map can be much bigger if we want it to! this is just the size of the window!
            // You can still go full screen, try it in-game by pressing "f" (it takes awhile)
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;

            Collisionbox = new Microsoft.Xna.Framework.Rectangle();
            Collisionbox.Height = 32;
            Collisionbox.Width = 32;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            
            input = new Input();
            menu = new Menu();
            settings = new Settings();
            choosechar = new ChooseChar();
            gamestate = GameStates.MainMenu;


            //xTile
            mapDisplayDevice = new XnaDisplayDevice(this.Content, this.GraphicsDevice);
            map.LoadTileSheets(mapDisplayDevice);
            // Make sure that viewport size = window size
            viewport = new xTile.Dimensions.Rectangle(new Size(windowWidth, windowHeight));
            //viewport.X = viewport.Width / 2;
            //viewport.Y = viewport.Height / 2;
            


            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            /// full editable, if you have a font installed in the system, just change the name of the font you want to use in the file Neverwinter // arial
            /// If you installed some new fonts, restart visual express in order to make fonts visible 
            /// you can find some good fonts i.e. http://www.dafont.com/
            /// t.ex dl this one: http://www.dafont.com/neverwinter.font, "install" on your station, change ("Fonts\\Arial") to ("Fonts\\Neverwinter") and it should work since 
            /// it is already changed to that font.
            text = Content.Load<SpriteFont>("Fonts\\Arial");
            myChar.WizardIco = Content.Load<Texture2D>(@"Textures\Misc\blackbox");
            myChar.KnightIco = Content.Load<Texture2D>(@"Textures\Misc\octo2");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myChar.myChar = Content.Load<Texture2D>(@"Textures\Misc\blackbox");
            myChar.myCharVector = new Vector2(128, 0);

            // Keeping the other maps in here for now
            //map = Content.Load<Map>("Maps\\Map01");
            //map = Content.Load<Map>("Maps\\theRoad");
            map = Content.Load<Map>("Maps\\320x320_test1");
            
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update();

            // Manages menu 
            if (gamestate == GameStates.MainMenu)
            {
                if (input.Down)
                {
                    menu.Iterator++;
                }
                else if (input.Up)
                {
                    menu.Iterator--;
                }

                if (input.Enter)
                {
                    if (menu.Iterator == 0)
                    {
                        gamestate = GameStates.ChooseCharacter;
                    }
                    else if (menu.Iterator == 1)
                    {
                        gamestate = GameStates.Settings;
                    }
                    else if (menu.Iterator == 2)
                    {
                        //gamestate = GameStates.Something;
                    }
                    else if (menu.Iterator == 3)
                    {
                        this.Exit();
                    }
                    menu.Iterator = 0;
                }
            }

            else if (gamestate == GameStates.End)
            {
                if (input.Enter)
                {
                    gamestate = GameStates.MainMenu;
                }
            }
            else if (gamestate == GameStates.Settings)
            {
                if (input.Enter)
                {
                    gamestate = GameStates.MainMenu;
                }
            }

           else  if (gamestate == GameStates.ChooseCharacter)
            {
                if (input.Down)
                {
                    choosechar.IterChar++;
                }
                else if (input.Up)
                {
                    choosechar.IterChar--;
                }
                
                if (input.Enter)
                {
                    if (choosechar.IterChar == 0)
                    {
                        // Set character to wizard 
                        myChar.chosenChar = 0;
                        gamestate = GameStates.Game;
                    }
                    else if (choosechar.IterChar == 1)
                    {
                        //set character to Knight
                        myChar.chosenChar = 1;
                        gamestate = GameStates.Game;
                    }
                   
                    else if (choosechar.IterChar == 2)
                    {
                        gamestate = GameStates.MainMenu;
                    }
                    
                }


            }


            else if (gamestate == GameStates.Game)
            {
                KeyboardState kb = Keyboard.GetState();


                // DEBUG feature only
                if (kb.IsKeyDown(Keys.Delete))
                {
                    this.Exit();
                }

                // DEBUG feature only (maybe)
                if (kb.IsKeyDown(Keys.F))
                {
                    graphics.ToggleFullScreen();
                }

                if (kb.IsKeyDown(Keys.Escape))
                {
                    gamestate = GameStates.MainMenu;
                }


                if (kb.IsKeyDown(Keys.Left))
                {
                    myChar.myCharVector.X -= myChar.speed;
                    //viewport.X = (int)myChar.myCharVector.X - (int)viewport.Width / 2;
                }

                if (kb.IsKeyDown(Keys.Right))
                {
                    myChar.myCharVector.X += myChar.speed;
                    //viewport.X = (int)myChar.myCharVector.X - (int)viewport.Width / 2;
                }

                if (kb.IsKeyDown(Keys.Up))
                {
                    myChar.myCharVector.Y -= myChar.speed;
                    //viewport.Y = (int)myChar.myCharVector.Y - (int)viewport.Height / 2;
                }

                if (kb.IsKeyDown(Keys.Down))
                {
                    myChar.myCharVector.Y += myChar.speed;
                    //viewport.Y = (int)myChar.myCharVector.Y - (int)viewport.Height / 2;
                }
                viewport.X = (int)myChar.myCharVector.X - (int)viewport.Width / 2;
                viewport.Y = (int)myChar.myCharVector.Y - (int)viewport.Height / 2;
            }
            map.Update(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // background color
            GraphicsDevice.Clear(Color.Black);

            
            // Draws the main map screen aswell as text and other UI stuff
            // Break into smaller modules in ze future
            spriteBatch.Begin();


            if (gamestate == GameStates.MainMenu)
            {
                menu.DrawMenu(spriteBatch, 800, text);
            }
            else if (gamestate == GameStates.Settings)
            {
                settings.DrawMenu(spriteBatch, 800, text);
            }
            else if (gamestate == GameStates.ChooseCharacter)
            {
                choosechar.DrawMenu(spriteBatch, 800, text);
                if (choosechar.IterChar == 0)
                {
                    spriteBatch.Draw(myChar.WizardIco, new Vector2(500, 100), Color.White);
                }
                else if (choosechar.IterChar == 1)
                {
                    spriteBatch.Draw(myChar.KnightIco, new Vector2(500, 150), Color.White);
                }
            }

            // ACTUALL GAME MODE
            else if (gamestate == GameStates.Game)
            {
                map.Draw(mapDisplayDevice, viewport);

                // Real ABS screen pos
                Vector2 realPos = Vector2.Zero;
                if (map.DisplayWidth <= windowWidth && map.DisplayHeight <= windowHeight)
                {
                    realPos.X = myChar.myCharVector.X;
                    realPos.Y = myChar.myCharVector.Y;
                }
                else if (map.DisplayWidth > windowWidth && map.DisplayHeight <= windowHeight)
                {
                    realPos.X = myChar.myCharVector.X - viewport.X;
                    realPos.Y = myChar.myCharVector.Y;
                }
                else if (map.DisplayWidth <= windowWidth && map.DisplayHeight > windowHeight)
                {
                    realPos.X = myChar.myCharVector.X;
                    realPos.Y = myChar.myCharVector.Y - viewport.Y;
                }
                else if (map.DisplayWidth > windowWidth && map.DisplayHeight > windowHeight)
                {
                    realPos.X = myChar.myCharVector.X - viewport.X;
                    realPos.Y = myChar.myCharVector.Y - viewport.Y;
                }

                if (myChar.chosenChar == 0)
                {
                    spriteBatch.Draw(myChar.WizardIco, realPos, Color.White);
                }
                else if (myChar.chosenChar == 1)
                {
                    spriteBatch.Draw(myChar.KnightIco, realPos, Color.White);
                }

                //DEBUG PRINT
                spriteBatch.DrawString(text, "charpos: " + myChar.myCharVector.ToString(), new Vector2(viewport.Width - 400, 0), Color.White);
                //spriteBatch.DrawString(text, "layersize: " + map.GetLayer("stones").DisplaySize.ToString(), new Vector2(viewport.Width - 300, 35), Color.White);
                spriteBatch.DrawString(text, "mapsize: " + map.DisplaySize.ToString(), new Vector2(viewport.Width - 400, 35*4), Color.White);
                //spriteBatch.DrawString(text, "layerID: " + map.GetLayer("stones").Id, new Vector2(viewport.Width - 300, 35*2), Color.White);
                //spriteBatch.DrawString(text, "validTile: " + map.GetLayer("grass").IsValidTileLocation((int)myChar.myCharVector.X,(int)myChar.myCharVector.Y), new Vector2(viewport.Width - 300, 35*3), Color.White);
                //spriteBatch.DrawString(text, "layer2map: " + map.GetLayer("stones").ConvertLayerToMapLocation(new Location((int)myChar.myCharVector.X, (int)myChar.myCharVector.Y), viewport.Size).ToString(), new Vector2(viewport.Width - 300, 35 * 5), Color.White);
                spriteBatch.DrawString(text, "viewportX: " + viewport.X, new Vector2(viewport.Width - 400, 35*6), Color.White);
                spriteBatch.DrawString(text, "viewportY: " + viewport.Y, new Vector2(viewport.Width - 400, 35 * 7), Color.White);
                spriteBatch.DrawString(text, "COLLISION " + Collision(myChar.myCharVector).ToString(), new Vector2(viewport.Width - 500, 35 * 10), Color.White);
                spriteBatch.DrawString(text, "boxCords " + Collisionbox.Center + " " + Collisionbox.Location, new Vector2(viewport.Width - 600, 35 * 9), Color.White);
            }

            else if (gamestate == GameStates.End)
            {
                menu.DrawEnd(spriteBatch, 800, text);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        // Semi-Horrible test code for collision. This will be moved later, it can be more modular.
        private bool Collision(Vector2 pos)
        {

            Layer collision = map.GetLayer("stones");
            Location tileLocation;
            Tile tile;

            tileLocation = new Location(((int)myChar.myCharVector.X - Collisionbox.Width) / 32, ((int)myChar.myCharVector.Y - Collisionbox.Height) / 32);
            if (tileLocation.X >= 0 && tileLocation.Y >= 0)
            {
                tile = collision.Tiles[tileLocation];
                if (tile != null && tile.TileIndex == 97)
                {
                    Debug.Print("Collision with tile at --" + tileLocation.ToString());
                    return true;
                }
            }

            tileLocation = new Location(((int)myChar.myCharVector.X + Collisionbox.Width) / 32, ((int)myChar.myCharVector.Y - Collisionbox.Height) / 32);
            if (tileLocation.X >= 0 && tileLocation.Y >= 0)
            {
                tile = collision.Tiles[tileLocation];
                if (tile != null && tile.TileIndex == 97)
                {
                    Debug.Print("Collision with tile at +-" + tileLocation.ToString());
                    return true;
                }
            }
            
            tileLocation = new Location(((int)myChar.myCharVector.X + Collisionbox.Width) / 32, ((int)myChar.myCharVector.Y + Collisionbox.Height) / 32);
            if (tileLocation.X >= 0 && tileLocation.Y >= 0)
            {
                tile = collision.Tiles[tileLocation];
                if (tile != null && tile.TileIndex == 97)
                {
                    Debug.Print("Collision with tile at ++" + tileLocation.ToString());
                    return true;
                }
            }

            tileLocation = new Location(((int)myChar.myCharVector.X - Collisionbox.Width) / 32, ((int)myChar.myCharVector.Y + Collisionbox.Height) / 32);
            if (tileLocation.X >= 0 && tileLocation.Y >= 0)
            {
                tile = collision.Tiles[tileLocation];
                if (tile != null && tile.TileIndex == 97)
                {
                    Debug.Print("Collision with tile at -+" + tileLocation.ToString());
                    return true;
                }
            }

            return false;

        }
    }
}
