using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using xTile.Layers;
using xTile.Dimensions;
using xTile.Tiles;
using xTile;

namespace ProjectGame
{
    class UserControlledSprite : Sprite
    {
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        { }
        
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        { }


        public override void Update(GameTime gameTime, Microsoft.Xna.Framework.Rectangle clientBounds)
        {
           
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= speed.X;
                if (Collision(position))
                {
                    position.X += speed.X;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += speed.X;
                if (Collision(position))
                {
                    position.X -= speed.X;
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position.Y -= speed.Y;
                if (Collision(position))
                {
                    position.Y += speed.Y;
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position.Y += speed.Y;
                if (Collision(position))
                {
                    position.Y -= speed.Y;
                }
            }

            
            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds);
        }

        private bool Collision(Vector2 pos)
        {

            Layer collision = null;
            Tile tile;

            int leftTile = (int)Math.Floor((float)collisionRect.Left / frameSize.X);
            int rightTile = (int)Math.Ceiling(((float)collisionRect.Right / frameSize.X)) - 1;
            int topTile = (int)Math.Floor((float)collisionRect.Top / frameSize.Y);
            int bottomTile = (int)Math.Ceiling(((float)collisionRect.Bottom / frameSize.Y)) - 1;

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
        }
    }
}
