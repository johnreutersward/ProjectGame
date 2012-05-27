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
        Player player;
        Char myChar;
        //Texture2D enemyTextures;
        List<Enemy> enemies;

        // menu & char screen
        private SpriteFont text;
        private Menu menu;
        private Title title;
        private Settings settings;
        private ChooseChar choosechar;
        Texture2D world_map;
        Texture2D menubg;
        Texture2D choosebg;

        // xTile map, display device reference and rendering viewport (this is pretty awesome!)
        public static Map map;
        public static Map forestMap;
        public static Map inhousMapa;
        public static Map inhousMapb;
        public static Map inhous2Mapa;
        public static Map inhous2Mapb;
        public static Map second;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;
        Layer collisionLayer;

        public Song intro;
        public Song gameost;
        public int play;

        int windowWidth;
        int windowHeight;
        #endregion
        Stopwatch stopWatch;
        public static bool done;

        public enum GameStates
        {
            TitleScreen,
            MainMenu,
            Game,
            Settings,
            ChooseCharacter,
            House1,
            End
        }

        public Game1()
        {
            #region initclasses etc
            graphics = new GraphicsDeviceManager(this);
           Content.RootDirectory = "Content";
            IsMouseVisible = true;
            // Why these values? Well 32 * 25 = 800 & 32 * 15 = 480 so it all works out! The map can be much bigger if we want it to! this is just the size of the window!
            // You can still go full screen, try it in-game by pressing "f" (it takes awhile)
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;
            player = new Player();

            myChar = new Char();
            enemies = new List<Enemy>();



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
            title = new Title();
            settings = new Settings();
            choosechar = new ChooseChar();

            // Default game state
            gamestate = GameStates.TitleScreen;

            //xTile
            mapDisplayDevice = new XnaDisplayDevice(this.Content, this.GraphicsDevice);
            map.LoadTileSheets(mapDisplayDevice);
            forestMap.LoadTileSheets(mapDisplayDevice);
            inhousMapa.LoadTileSheets(mapDisplayDevice);
            inhousMapb.LoadTileSheets(mapDisplayDevice);
            inhous2Mapa.LoadTileSheets(mapDisplayDevice);
            inhous2Mapb.LoadTileSheets(mapDisplayDevice);
            second.LoadTileSheets(mapDisplayDevice);
            // Make sure that viewport size = window size
            viewport = new xTile.Dimensions.Rectangle(new Size(windowWidth, windowHeight));

            #endregion
            stopWatch = new System.Diagnostics.Stopwatch();
            done = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            #region LoadContent
            spriteBatch = new SpriteBatch(GraphicsDevice);
            text = Content.Load<SpriteFont>("Fonts\\Arial");
            myChar.ClothIco = Content.Load<Texture2D>(@"Textures\clotharmorico");
            myChar.DarkIco = Content.Load<Texture2D>(@"Textures\deathknightico");
            myChar.GoldenIco = Content.Load<Texture2D>(@"Textures\goldenarmorico");
            myChar.LeatherIco = Content.Load<Texture2D>(@"Textures\leatherarmorico");
           
            world_map = Content.Load<Texture2D>(@"Textures\gameTitle_v3");
            menubg = Content.Load<Texture2D>(@"Textures\bg1");
            choosebg = Content.Load<Texture2D>(@"Textures\bg2");

            intro = Content.Load<Song>(@"Audio\intro");
          //  intro = Content.Load<Song>(@"Audio\StoryBegins");
            gameost = Content.Load<Song>(@"Audio\SneakySnitch");

            
           
              
            
                //player.Initalize(Content.Load<Texture2D>(@"Textures\platearmor"), startPos);
           
            //myChar.myChar = Content.Load<Texture2D>(@"Textures\blackbox");
            //myChar.myCharVector = new Vector2(32, 32);


            // All maps must have a invisible layer called "obs" that uses tile 23 to mark obstacles
            // forest map
            //map = Content.Load<Map>("Maps\\Forest");
            // test for portals in forest
            //map = Content.Load<Map>("Maps\\Foresttest");
            // test for the room
            forestMap = Content.Load<Map>("Maps\\Foresttest");
            inhousMapa = Content.Load<Map>("Maps\\standard2a");
            inhousMapb = Content.Load<Map>("Maps\\standard2b");
            inhous2Mapa = Content.Load<Map>("Maps\\standard3a");
            inhous2Mapb = Content.Load<Map>("Maps\\standard3b");
            second = Content.Load<Map>("Maps\\standard");
            map = forestMap;
            
            //collisionLayer = map.GetLayer("obs");
            #endregion
            MediaPlayer.Play(intro);
         

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
            else if (gamestate == GameStates.TitleScreen)
            {
                if (input.Enter)
                {
                    gamestate = GameStates.MainMenu;
                }
                
            }
            else if (gamestate == GameStates.Settings)
            {

                if (input.Down)
                {
                    settings.IterSettings++;
                }
                else if (input.Up)
                {
                    settings.IterSettings--;
                }

                if (input.Left)
                {
                    if (settings.IterSettings == 0)
                    {
                        settings.onoffmusic = 0;
                        MediaPlayer.Stop();
                        play = 0;
                    }
                 
                }
                else if (input.Right)
                {
                    if (settings.IterSettings == 0)
                    {
                        settings.onoffmusic = 1;
                        MediaPlayer.Play(intro);
                        play = 1;
                    }
                }
                else if (input.Enter)
                {
                    if (settings.IterSettings == 1)
                    {
                        graphics.ToggleFullScreen();
                    }
                
                
                    else if (settings.IterSettings == 2)
                    {
                        gamestate = GameStates.MainMenu;
                    }
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
                        // Set character to the commoner
                        myChar.chosenChar = 0;
                        gamestate = GameStates.Game;
                        if (play == 1)
                        {
                            MediaPlayer.Play(gameost);
                            MediaPlayer.IsRepeating = true;
                        }
                    }
                    else if (choosechar.IterChar == 1)
                    {
                        //set character to the druid
                        myChar.chosenChar = 1;
                        gamestate = GameStates.Game;
                        if (play == 1)
                        {
                            MediaPlayer.Play(gameost);
                            MediaPlayer.IsRepeating = true;
                        }
                       
                    }
                    else if (choosechar.IterChar == 2)
                    {
                        //set character to the knight
                        myChar.chosenChar = 2;
                        gamestate = GameStates.Game;
                        if (play == 1)
                        {
                            MediaPlayer.Play(gameost);
                            MediaPlayer.IsRepeating = true;
                        }
                        
                    }
                    else if (choosechar.IterChar == 3)
                    {
                        //set character to the dark knight
                        myChar.chosenChar = 3;
                        gamestate = GameStates.Game;
                        if (play == 1)
                        {
                            MediaPlayer.Play(gameost);
                            MediaPlayer.IsRepeating = true;
                        }
                        
                    }

                    else if (choosechar.IterChar == 4)
                    {
                        gamestate = GameStates.MainMenu;

                    }

                }


            }

            #endregion

            #region Game
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
                    if (play == 1)
                    {
                        MediaPlayer.Play(intro);
                        MediaPlayer.IsRepeating = true;
                    }
                }

                player.Update(gameTime, map.GetLayer("obs"));
                viewport.X = (int)player.Position.X - (int)viewport.Width / 2;
                viewport.Y = (int)player.Position.Y - (int)viewport.Height / 2;
            }
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

            map.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);

           // InitializeEnemy();

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
                menu.DrawMenu(spriteBatch, 800, text, menubg);
            }
            if (gamestate == GameStates.House1)
            {
               player.Draw(spriteBatch, new Vector2(map.DisplayWidth, map.DisplayHeight), new Vector2(windowWidth, windowHeight), new Vector2(viewport.X, viewport.Y));
            }
            else if (gamestate == GameStates.TitleScreen)
            {
                title.DrawEnd(spriteBatch, 800, world_map);
            }
            else if (gamestate == GameStates.Settings)
            {
                settings.DrawMenu(spriteBatch, 800, text, menubg);
            }
            else if (gamestate == GameStates.End)
            {
                menu.DrawEnd(spriteBatch, 800, text, world_map);
            }
            else if (gamestate == GameStates.ChooseCharacter)
            {
                choosechar.DrawMenu(spriteBatch, 800, text, choosebg);
               
                Vector2 startPos = new Vector2(32, 128);
                if (choosechar.IterChar == 0)
                {
                    spriteBatch.Draw(myChar.ClothIco, new Vector2(580, 100), Color.White);
                    player.Initalize(Content.Load<Texture2D>(@"Textures\clotharmor"), startPos);
                }
                else if (choosechar.IterChar == 1)
                {
                    spriteBatch.Draw(myChar.LeatherIco, new Vector2(580, 145), Color.White);
                    player.Initalize(Content.Load<Texture2D>(@"Textures\leatherarmor"), startPos);
                }
                else if (choosechar.IterChar == 2)
                {
                    spriteBatch.Draw(myChar.GoldenIco, new Vector2(580, 190), Color.White);
                    player.Initalize(Content.Load<Texture2D>(@"Textures\goldenarmor"), startPos);
                }
                else if (choosechar.IterChar == 3)
                {
                    spriteBatch.Draw(myChar.DarkIco, new Vector2(580, 235), Color.White);
                    player.Initalize(Content.Load<Texture2D>(@"Textures\deathknight"), startPos);
                }

            }
            #endregion

            #region draw Windows
          /*  else if (gamestate == GameStates.Something)
            {
                TimeSpan drawTime = stopWatch.Elapsed;
                // settings.DrawMenu(spriteBatch, 800, text);
                map.Draw(mapDisplayDevice, viewport);
               // spriteBatch.Draw(myChar.myChar, myChar.myCharVector, Color.White);
                //InitializeEnemy(gameTime);
                // Conversation.Draw(spriteBatch);
                spriteBatch.DrawString(text, "ms: " + drawTime.TotalMilliseconds, Vector2.UnitX * 600.0f + Vector2.UnitY * 20.0f, Color.DarkMagenta);

                stopWatch.Start();
                CreateEnemy(gameTime);


            }
            */
            #endregion

            #region draw enemy and char test
            /*else if (gamestate == GameStates.Something)
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
                player.Draw(spriteBatch, new Vector2(map.DisplayWidth, map.DisplayHeight), new Vector2(windowWidth, windowHeight), new Vector2(viewport.X, viewport.Y));

                // DEBUG PRINT, magic numbers are bad, but this will do
                Debug.OnScreenPrint(spriteBatch, text, "PlayerPos: " + player.Position.ToString(), new Vector2(viewport.Width - 400, 35 * 0));
                Debug.OnScreenPrint(spriteBatch, text, "PlayerDir: " + player.playerDirection.ToString(), new Vector2(viewport.Width - 360, 35 * 1));
                Debug.OnScreenPrint(spriteBatch, text, "MapDim: " + map.DisplaySize.ToString(), new Vector2(viewport.Width - 360, 35 * 2));
                
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
                        Tile tile = collision.Tiles[x, y];

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
                gamestate = GameStates.Settings;
                /*stopWatch.Stop();

                if (done)
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                }
                else
                {
                    Conversation.Draw(spriteBatch);
                }
                */

            }


        }

        public void MusicControl()
        {
            if (play == 1)
            { MediaPlayer.Play(intro); }
            else if (play == 1)
            { MediaPlayer.Stop(); }
        }

        private void InitializeEnemy()
        {
            #region init enemy
            //to do .. 
           // CreateEnemy();
            #endregion
        }

       
    }
}