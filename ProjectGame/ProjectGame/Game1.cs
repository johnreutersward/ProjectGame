using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using xTile;
using xTile.Dimensions;
using xTile.Display;

namespace ProjectGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TileMap myMap;
        Char myChar;
        int squaresAcross;
        int squaresDown;

        Map map;
        IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;


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
            // TODO: Add your initialization logic here
            base.Initialize();
            mapDisplayDevice = new XnaDisplayDevice(this.Content, this.GraphicsDevice);

            map.LoadTileSheets(mapDisplayDevice);

            viewport = new xTile.Dimensions.Rectangle(new Size(800,600));

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part1_tileset");
            myChar.myChar = Content.Load<Texture2D>(@"Textures\Misc\octo");
            myChar.myCharVector = new Vector2(25, 25);


            map = Content.Load<Map>("Maps\\Map01");
            // TODO: use this.Content to load your game content here
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
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (kb.IsKeyDown(Keys.Left))
            {
                myChar.myCharVector.X -= myChar.speed;
                //Camera.Location.X -= myChar.speed;
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 2, 0, (myMap.MapWidth - squaresAcross) * 32);
                viewport.X -= 1;
            }

            if (kb.IsKeyDown(Keys.Right))
            {
                myChar.myCharVector.X += myChar.speed;
                //Camera.Location.X += myChar.speed;
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 2, 0, (myMap.MapWidth - squaresAcross) * 32);
                viewport.X += 1;
            }

            if (kb.IsKeyDown(Keys.Up))
            {
                myChar.myCharVector.Y -= myChar.speed;
                //Camera.Location.Y -= myChar.speed;
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 2, 0, (myMap.MapHeight - squaresDown) * 32);
                viewport.Y -= 1;
            }

            if (kb.IsKeyDown(Keys.Down))
            {
                myChar.myCharVector.Y += myChar.speed;
                //Camera.Location.Y += myChar.speed;
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 2, 0, (myMap.MapHeight - squaresDown) * 32);
                viewport.Y += 1;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            map.Draw(mapDisplayDevice, viewport);


            //Vector2 firstSquare = new Vector2(Camera.Location.X / 32, Camera.Location.Y / 32);
            //int firstX = (int)firstSquare.X;
            //int firstY = (int)firstSquare.Y;

            //Vector2 squareOffset = new Vector2(Camera.Location.X % 32, Camera.Location.Y % 32);
            //int offsetX = (int)squareOffset.X;
            //int offsetY = (int)squareOffset.Y;

            //for (int y = 0; y < squaresDown; y++)
            //{   
            //    for (int x = 0; x < squaresAcross; x++)
            //    {
            //        spriteBatch.Draw(
            //            Tile.TileSetTexture,
            //            new Rectangle((x * 32) - offsetX, (y * 32) - offsetY, 32, 32),
            //            Tile.GetSourceRectangle(myMap.Rows[y + firstY].Columns[x + firstX].TileID),
            //            Color.White);
            //    }
            //}
            spriteBatch.Draw(myChar.myChar, myChar.myCharVector, Color.White);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
