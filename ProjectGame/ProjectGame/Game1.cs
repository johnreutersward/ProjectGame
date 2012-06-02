using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.Tiles;


namespace ProjectGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        private Input input;
        public static GameStates gamestate;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int windowWidth;
        int windowHeight;

        // Misc content
        Player player;
        Char myChar;
        public static SpriteFont text;
        public static SpriteFont textconv;
        private Menu menu;
        public static Conv Conversationbox;
        private Title title;
        private Settings settings;
        private ChooseChar choosechar;
        Texture2D world_map;
        Texture2D menubg;
        Texture2D choosebg;
        Texture2D gameover;
        public static Texture2D bg;
        public Texture2D knife;
        public static bool done;
        public KeyboardState OldKeyState;
        ProjectileManager projectileManager;

        // xTile maps, display device reference and rendering viewport
        public static Map map;
        public static Map forestMap;
        public static Map inhousMapa;
        public static Map inhousMapb;
        public static Map inhous2Mapa;
        public static Map inhous2Mapb;
        public static Map second;
        public static Map cave;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;       

        // Music
        public Song intro;
        public Song gameost;
        public int play;

        public enum GameStates
        {
            TitleScreen,
            MainMenu,
            Game,
            Settings,
            ChooseCharacter,
            End
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;
            projectileManager = new ProjectileManager();
            player = new Player(projectileManager);
            myChar = new Char();
        }

        protected override void Initialize()
        {
            base.Initialize();
            Conversationbox = new Conv();
            input = new Input();
            menu = new Menu();
            title = new Title();
            settings = new Settings();
            choosechar = new ChooseChar();
            play = 1;
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
            done = false;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fonts
            text = Content.Load<SpriteFont>("Fonts\\Arial");
            textconv = Content.Load<SpriteFont>("Fonts\\Segoe");

            // Character textures
            myChar.ClothIco = Content.Load<Texture2D>(@"Textures\clotharmorico");
            myChar.DarkIco = Content.Load<Texture2D>(@"Textures\deathknightico");
            myChar.GoldenIco = Content.Load<Texture2D>(@"Textures\goldenarmorico");
            myChar.LeatherIco = Content.Load<Texture2D>(@"Textures\leatherarmorico");
            myChar.Wizardico = Content.Load<Texture2D>(@"Textures\Wizardico");
           
            //  Misc textures
            world_map = Content.Load<Texture2D>(@"Textures\gameTitle_v3");
            menubg = Content.Load<Texture2D>(@"Textures\bg1");
            choosebg = Content.Load<Texture2D>(@"Textures\bg2");
            gameover = Content.Load<Texture2D>(@"Textures\gameover");
            knife = Content.Load<Texture2D>(@"Textures\knife");
            projectileManager.setTexture(knife);

            // Music
            intro = Content.Load<Song>(@"Audio\intro");          
            gameost = Content.Load<Song>(@"Audio\SneakySnitch");
            
            // Maps - All maps must have a invisible layer called "obs" that uses tile 23 to mark obstacles
            forestMap = Content.Load<Map>("Maps\\Foresttest");
            inhousMapa = Content.Load<Map>("Maps\\standard2a");
            inhousMapb = Content.Load<Map>("Maps\\standard2b");
            inhous2Mapa = Content.Load<Map>("Maps\\standard3a");
            inhous2Mapb = Content.Load<Map>("Maps\\standard3b");
            second = Content.Load<Map>("Maps\\standard");
            cave = Content.Load<Map>("Maps\\cave");
            
            // Set default map
            map = forestMap;
            
            // Start intro music
            MediaPlayer.Play(intro);
            
            bg = Content.Load<Texture2D>(@"Textures\DialogueBoxlong");
            DirectoryInfo directoryInfo = new DirectoryInfo(Content.RootDirectory + @"\Avatars\");
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            ArrayList arrayList = new ArrayList();

            foreach (FileInfo fi in fileInfo)
                arrayList.Add(fi.FullName);
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            
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
                KeyboardState kb = Keyboard.GetState();

                if (input.Enter)
                {
                    gamestate = GameStates.MainMenu;
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
                        //set character to the wizard
                        myChar.chosenChar = 4;
                        gamestate = GameStates.Game;
                        if (play == 1)
                        {
                            MediaPlayer.Play(gameost);
                            MediaPlayer.IsRepeating = true;
                        }

                    }

                    else if (choosechar.IterChar == 5)
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

                if (kb.IsKeyDown(Keys.Enter) && OldKeyState.IsKeyUp(Keys.Enter))
                {

                    if (Player.count > Player.convset)
                    {
                        Player.convset++;
                    }

                    else if (Player.convset == Player.count)
                    {
                        //Player.notreaded = 1;
                        Player.doConversation = false;
                    }
                }
                OldKeyState = kb;

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

            map.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            // background color
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            #region drawMenu

            if (gamestate == GameStates.MainMenu)
            {
               
                menu.DrawMenu(spriteBatch, 800, text, menubg);
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
                spriteBatch.Draw(gameover, new Vector2(0, 0), Color.White);
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
                else if (choosechar.IterChar == 4)
                {
                    spriteBatch.Draw(myChar.Wizardico, new Vector2(580, 280), Color.White);
                    player.Initalize(Content.Load<Texture2D>(@"Textures\Wizard_spreadsheet"), startPos);
                }


            }
            #endregion

            // ACTUALL GAME MODE
            else if (gamestate == GameStates.Game)
            {
                map.Draw(mapDisplayDevice, viewport);
                player.Draw(spriteBatch, new Vector2(map.DisplayWidth, map.DisplayHeight), new Vector2(windowWidth, windowHeight), new Vector2(viewport.X, viewport.Y));
                // DEBUG PRINT, magic numbers are bad, but this will do
                ///Debug.OnScreenPrint(spriteBatch, text, "PlayerPos: " + player.Position.ToString(), new Vector2(viewport.Width - 400, 35 * 0));
                //Debug.OnScreenPrint(spriteBatch, text, "PlayerDir: " + player.playerDirection.ToString(), new Vector2(viewport.Width - 360, 35 * 1));
                //Debug.OnScreenPrint(spriteBatch, text, "MapDim: " + map.DisplaySize.ToString(), new Vector2(viewport.Width - 360, 35 * 2));
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}