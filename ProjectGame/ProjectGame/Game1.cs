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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private Input input;
        public static GameStates gamestate;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
    
        //tile map (from XNAresources.com)
        TileMap myMap;
        Char myChar;
        int squaresAcross;
        int squaresDown;
        
        // menu & char screen
        private SpriteFont text;
        private Menu menu;
        private Settings settings;
        private ChooseChar choosechar;

        // xTile map, display device reference and rendering viewport (this is pretty awesome!)
        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;
        

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

            myMap = new TileMap();
            myChar = new Char();
            squaresAcross = 5;
            squaresDown = 5;
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
            viewport = new xTile.Dimensions.Rectangle(new Size(800, 600));


            
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
            myChar.WizardIco = Content.Load<Texture2D>(@"Textures\Misc\octo");
            myChar.KnightIco = Content.Load<Texture2D>(@"Textures\Misc\octo2");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myChar.myChar = Content.Load<Texture2D>(@"Textures\Misc\octo");
            myChar.myCharVector = new Vector2(25, 25);

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

                if (kb.IsKeyDown(Keys.Escape))
                {
                    gamestate = GameStates.MainMenu;
                }


                if (kb.IsKeyDown(Keys.Left))
                {
                    myChar.myCharVector.X -= myChar.speed;
                    viewport.X -= 1;
                }

                if (kb.IsKeyDown(Keys.Right))
                {
                    myChar.myCharVector.X += myChar.speed;
                    viewport.X += 1;
                }

                if (kb.IsKeyDown(Keys.Up))
                {
                    myChar.myCharVector.Y -= myChar.speed;
                    viewport.Y -= 1;
                }

                if (kb.IsKeyDown(Keys.Down))
                {
                    myChar.myCharVector.Y += myChar.speed;
                    viewport.Y += 1;
                }
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
            else if (gamestate == GameStates.Game)
            {
                map.Draw(mapDisplayDevice, viewport);
                if (myChar.chosenChar == 0)
                {
                    spriteBatch.Draw(myChar.WizardIco, myChar.myCharVector, Color.White);
                }
                else if (myChar.chosenChar == 1)
                {
                    spriteBatch.Draw(myChar.KnightIco, myChar.myCharVector, Color.White);
                }

                //DEBUG PRINT
                spriteBatch.DrawString(text, "charpos: " + myChar.myCharVector.ToString(), new Vector2(viewport.Width - 300, 0), Color.White);
                spriteBatch.DrawString(text, "layersize: " + map.GetLayer("stones").DisplaySize.ToString(), new Vector2(viewport.Width - 300, 35), Color.White);
                spriteBatch.DrawString(text, "layerID: " + map.GetLayer("stones").Id, new Vector2(viewport.Width - 300, 35*2), Color.White);
                spriteBatch.DrawString(text, "validTile: " + map.GetLayer("grass").IsValidTileLocation((int)myChar.myCharVector.X,(int)myChar.myCharVector.Y), new Vector2(viewport.Width - 300, 35*3), Color.White);

                Location currentLoc = new Location();
                currentLoc = map.GetLayer("stones").ConvertLayerToMapLocation(new Location((int)myChar.myCharVector.X, (int)myChar.myCharVector.Y), viewport.Size);
                spriteBatch.DrawString(text, "layer2map: " + currentLoc.ToString(), new Vector2(viewport.Width - 300, 35 * 4), Color.White);

            }

            else if (gamestate == GameStates.End)
            {
                menu.DrawEnd(spriteBatch, 800, text);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
