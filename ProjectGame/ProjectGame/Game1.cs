using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.Tiles;

namespace ProjectGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region instances

        private Input input;
        public static GameStates gamestate;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //char & enemies
        Char myChar;
        Texture2D enemyTextures;
        List<Enemy> enemies;

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
        #endregion
        Stopwatch stopWatch;
        public static bool done;


        #region gamestates
        public enum GameStates
        {
            MainMenu,
            Game,
            Settings,
            Something,
            ChooseCharacter,
            End
        }
        #endregion

        public Game1()
        {
            #region initclasses etc
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            myChar = new Char();
            enemies = new List<Enemy>();
            

            // Why these values? Well 32 * 25 = 800 & 32 * 15 = 480 so it all works out! The map can be much bigger if we want it to! this is just the size of the window!
            // You can still go full screen, try it in-game by pressing "f" (it takes awhile)
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;
            #endregion

           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region Initialize

            base.Initialize();

            input = new Input();
            menu = new Menu();
            settings = new Settings();
            choosechar = new ChooseChar();

            // Default game state
            gamestate = GameStates.MainMenu;

            //xTile
            mapDisplayDevice = new XnaDisplayDevice(this.Content, this.GraphicsDevice);
            map.LoadTileSheets(mapDisplayDevice);

            // Make sure that viewport size = window size
            viewport = new xTile.Dimensions.Rectangle(new Size(windowWidth, windowHeight));
            //viewport.X = viewport.Width / 2;
            //viewport.Y = viewport.Height / 2;
            #endregion

            stopWatch = new System.Diagnostics.Stopwatch();
            done = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        ///  full editable fonts, if you have a font installed in the system, just change the name of the font you want to use in the file Neverwinter // arial
        /// If you installed some new fonts, restart visual express in order to make fonts visible 
        /// you can find some good fonts i.e. http://www.dafont.com/
        /// t.ex dl this one: http://www.dafont.com/neverwinter.font, "install" on your station, change ("Fonts\\Arial") to ("Fonts\\Neverwinter") and it should work since 
        /// it is already changed to that font.
        /// </summary>
        protected override void LoadContent()
        {
            #region LoadContent
            text = Content.Load<SpriteFont>("Fonts\\Arial");
            myChar.WizardIco = Content.Load<Texture2D>(@"Textures\Misc\blackbox");
            myChar.KnightIco = Content.Load<Texture2D>(@"Textures\Misc\octo2");

            enemyTextures = Content.Load<Texture2D>(@"Textures\Misc\octo2");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myChar.myChar = Content.Load<Texture2D>(@"Textures\Misc\blackbox");
            myChar.myCharVector = new Vector2(32, 32);

            // Keeping the other maps in here for now
            //map = Content.Load<Map>("Maps\\Map01");
            //map = Content.Load<Map>("Maps\\theRoad");
            //map = Content.Load<Map>("Maps\\320x320_test1");
            map = Content.Load<Map>("Maps\\standard");
            #endregion

            #region windowsMode
            // windows mode content loader
            Conversation.Initialize(Content.Load<SpriteFont>(@"Fonts\Segoe"),
                Content.Load<Texture2D>(@"Textures\bg\DialogueBox"),
                new Microsoft.Xna.Framework.Rectangle(50, 50, 400, 100),
                Content.Load<Texture2D>(@"Textures\Misc\BorderImage"),
                5,
                Color.Black,
                Content.Load<Texture2D>(@"Textures\Misc\Continue"),
                Content);

            // Load Avatars
            DirectoryInfo directoryInfo = new DirectoryInfo(Content.RootDirectory + @"\Avatars\");
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            ArrayList arrayList = new ArrayList();

            foreach (FileInfo fi in fileInfo)
                arrayList.Add(fi.FullName);

            for (int i = 0; i < arrayList.Count; i++)
            {
                Conversation.Avatars.Add(Content.Load<Texture2D>(@"Avatars\" + i));
            }

            //load conversation by its id
            Conversation.StartConversation(1);
            

            #endregion
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
            #region update

            input.Update();

            #region MainMenu
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
                        gamestate = GameStates.Something;
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
                
           else if (gamestate == GameStates.ChooseCharacter)
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

            #endregion

            #region Game
            else if (gamestate == GameStates.Game || gamestate == GameStates.Something)
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
                    if (Collision(myChar.myCharVector))
                    {
                        myChar.myCharVector.X += myChar.speed;
                    }
                }

                if (kb.IsKeyDown(Keys.Right))
                {
                    myChar.myCharVector.X += myChar.speed;
                    if (Collision(myChar.myCharVector))
                    {
                        myChar.myCharVector.X -= myChar.speed;
                    }
                }


                if (kb.IsKeyDown(Keys.Up))
                {
                    myChar.myCharVector.Y -= myChar.speed;
                    if (Collision(myChar.myCharVector))
                    {
                        myChar.myCharVector.Y += myChar.speed;
                    }
                }


                if (kb.IsKeyDown(Keys.Down))
                {
                    myChar.myCharVector.Y += myChar.speed;
                    if (Collision(myChar.myCharVector))
                    {
                        myChar.myCharVector.Y -= myChar.speed;
                    }
                }


                viewport.X = (int)myChar.myCharVector.X - (int)viewport.Width / 2;
                viewport.Y = (int)myChar.myCharVector.Y - (int)viewport.Height / 2;
            }
            #endregion

            map.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);

           #endregion

            #region windows mode 
            Conversation.Update(gameTime);
/*
            if (Conversation.Expired)
            {
                if (visible == true)
                {

                    Conversation.StartConversation(1);

                }
                else
                {
                    Conversation.RemoveBox();
                }
            }*/
            #endregion
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            #region draw
            // background color
            GraphicsDevice.Clear(Color.Black);

            // Draws the main map screen aswell as text and other UI stuff. Break into smaller modules in ze future
            spriteBatch.Begin();

            #region drawMenu

            if (gamestate == GameStates.MainMenu)
            {
                menu.DrawMenu(spriteBatch, 800, text);
            }
            else if (gamestate == GameStates.Settings)
            {
                settings.DrawMenu(spriteBatch, 800, text);
            }
            else if (gamestate == GameStates.End)
            {
                menu.DrawEnd(spriteBatch, 800, text);
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
            #endregion

            #region draw Windows
            else if (gamestate == GameStates.Something)
            {
                TimeSpan drawTime = stopWatch.Elapsed; 
               // settings.DrawMenu(spriteBatch, 800, text);
                map.Draw(mapDisplayDevice, viewport);
                spriteBatch.Draw(myChar.myChar, myChar.myCharVector, Color.White);
                //InitializeEnemy(gameTime);
                   // Conversation.Draw(spriteBatch);
                spriteBatch.DrawString(text, "ms: " + drawTime.TotalMilliseconds, Vector2.UnitX * 600.0f + Vector2.UnitY * 20.0f, Color.DarkMagenta);

                stopWatch.Start();
               CreateEnemy(gameTime);
               
               
            }
            
            #endregion

            #region draw enemy and char test
          /*  else if (gamestate == GameStates.Something)
            {
                spriteBatch.Draw(myChar.myChar, myChar.myCharVector, Color.White);
                spriteBatch.Draw(enemies[1].enemy1, enemies[0].enemyVector, Color.White);
            }*/
            #endregion

            #region drawGame
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
                spriteBatch.DrawString(text, "mapsize: " + map.DisplaySize.ToString(), new Vector2(viewport.Width - 400, 35 * 4), Color.White);
                //spriteBatch.DrawString(text, "layerID: " + map.GetLayer("stones").Id, new Vector2(viewport.Width - 300, 35*2), Color.White);
                //spriteBatch.DrawString(text, "validTile: " + map.GetLayer("grass").IsValidTileLocation((int)myChar.myCharVector.X,(int)myChar.myCharVector.Y), new Vector2(viewport.Width - 300, 35*3), Color.White);
                //spriteBatch.DrawString(text, "layer2map: " + map.GetLayer("stones").ConvertLayerToMapLocation(new Location((int)myChar.myCharVector.X, (int)myChar.myCharVector.Y), viewport.Size).ToString(), new Vector2(viewport.Width - 300, 35 * 5), Color.White);
                spriteBatch.DrawString(text, "viewportX: " + viewport.X, new Vector2(viewport.Width - 400, 35 * 6), Color.White);
                spriteBatch.DrawString(text, "viewportY: " + viewport.Y, new Vector2(viewport.Width - 400, 35 * 7), Color.White);
                spriteBatch.DrawString(text, "COLLISION " + Collision(myChar.myCharVector).ToString(), new Vector2(viewport.Width - 500, 35 * 10), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
            #endregion

            #endregion
        }

        private bool Collision(Vector2 pos)
        {
            #region collision
            //Horrible test code for collision

            Layer collision = map.GetLayer("obs");
            Location tileLocation;
            Tile tile;

            Microsoft.Xna.Framework.Rectangle characterBounds = new Microsoft.Xna.Framework.Rectangle((int)myChar.myCharVector.X, (int)myChar.myCharVector.Y, 32, 32);

            int leftTile = (int)Math.Floor((float)characterBounds.Left / 32);
            int rightTile = (int)Math.Ceiling(((float)characterBounds.Right / 32)) - 1;
            int topTile = (int)Math.Floor((float)characterBounds.Top / 32);
            int bottomTile = (int)Math.Ceiling(((float)characterBounds.Bottom / 32)) - 1;

            //Debug.Print("left: " + leftTile + " right: " + rightTile + " top: " + topTile + " bottom: " + bottomTile);

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if ((x >= 0 && x < collision.LayerWidth) && (y >= 0 && y < collision.LayerHeight))
                    {
                        tile = collision.Tiles[x, y];
                        
                        if (tile != null && tile.TileIndex == 23)
                        {
                            //Debug.Print("Collision with tile at {" + x + "," + y + "}");
                            return true;
                        }
                    }
                }
            }
            return false;

          
            #endregion
        }

        private void CreateEnemy(GameTime gameTime)
        {
            if (stopWatch.Elapsed.Seconds > 2)
            {
                stopWatch.Stop();

               if (done)
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                }
                else
                {
                    Conversation.Draw(spriteBatch);
                }


            }
                
           
        }

       
          
           
           
        

        
    }
}
