using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile.Layers;
using xTile.Tiles;
using Microsoft.Xna.Framework.Input;


namespace ProjectGame
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public int collisionOffset = 0;
        public int speed = 10;
        public Rectangle playerBounds
        {
            get
            {
                return new Rectangle((int)Position.X + collisionOffset, (int)Position.Y + collisionOffset, PlayerTexture.Width - (collisionOffset * 2), PlayerTexture.Height - (collisionOffset * 2));
            }
        }

        public void Initalize(Texture2D PlayerTexture, Vector2 Position)
        {
            this.PlayerTexture = PlayerTexture;
            this.Position = Position;
        }

        public void Update(Layer collisionLayer)
        {
             if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position.X -= speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.X += speed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position.X += speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.X -= speed;
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Position.Y -= speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.Y += speed;
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Position.Y += speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.Y -= speed;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 mapDimension, Vector2 windowDimension, Vector2 viewport)
        {

            Vector2 realPosition = Vector2.Zero;
            if (mapDimension.X <= windowDimension.X && mapDimension.Y <= windowDimension.Y)
            {
                realPosition.X = Position.X;
                realPosition.Y = Position.Y;
            }
            else if (mapDimension.X > windowDimension.X && mapDimension.Y <= windowDimension.Y)
            {
                realPosition.X = Position.X - viewport.X;
                realPosition.Y = Position.Y;
            }
            else if (mapDimension.X <= windowDimension.X && mapDimension.Y > windowDimension.Y)
            {
                realPosition.X = Position.X;
                realPosition.Y = Position.Y - viewport.Y;
            }
            else if (mapDimension.X > windowDimension.X && mapDimension.Y > windowDimension.Y)
            {
                realPosition.X = Position.X - viewport.X;
                realPosition.Y = Position.Y - viewport.Y;
            }
            spriteBatch.Draw(PlayerTexture, realPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        private bool Collision(Vector2 pos, Layer collisionLayer)
        {
            

            int leftTile = (int)Math.Floor((float)playerBounds.Left / 32);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / 32)) - 1;
            int topTile = (int)Math.Floor((float)playerBounds.Top / 32);
            int bottomTile = (int)Math.Ceiling(((float)playerBounds.Bottom / 32)) - 1;

            //Debug.Print("left: " + leftTile + " right: " + rightTile + " top: " + topTile + " bottom: " + bottomTile);

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if ((x >= 0 && x < collisionLayer.LayerWidth) && (y >= 0 && y < collisionLayer.LayerHeight))
                    {
                        Tile tile = collisionLayer.Tiles[x, y];

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
