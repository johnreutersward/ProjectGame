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
        public Direction playerDirection = Direction.Right;
        public Texture2D PlayerTexture;
        public Point frameSize = new Point(64,64);
        public Point currentFrame = new Point(0,0);
        public Point sheetSize = new Point(5,9);
        public Vector2 Position;
        public int collisionOffset = 10;
        public int speed = 2;
        public int timeSinceLastFrame = 0;
        public int defaultMillisecondsPerFrame = 60;

        public Rectangle playerBounds
        {
            get
            {
                return new Rectangle((int)Position.X + collisionOffset, (int)Position.Y + collisionOffset, frameSize.X - (collisionOffset * 2), frameSize.Y - (collisionOffset * 2));
            }
        }

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public void Initalize(Texture2D PlayerTexture, Vector2 Position)
        {
            this.PlayerTexture = PlayerTexture;
            this.Position = Position;
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                playerDirection = Direction.Left;
                Position.X -= speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.X += speed;
                }
                
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 1;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                playerDirection = Direction.Right;
                Position.X += speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.X -= speed;
                }
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 1;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                playerDirection = Direction.Up;
                Position.Y -= speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.Y += speed;
                }
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 4;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                playerDirection = Direction.Down;
                Position.Y += speed;
                if (Collision(Position, collisionLayer))
                {
                    Position.Y -= speed;
                }
                if (timeSinceLastFrame > defaultMillisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.Y = 7;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X-1)
                    {
                        currentFrame.X = 0;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 mapDimension, Vector2 windowDimension, Vector2 viewport)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (playerDirection == Direction.Left)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(PlayerTexture, CalculateScreenPosition(mapDimension, windowDimension, viewport), new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
        }

        private Vector2 CalculateScreenPosition(Vector2 mapDimension, Vector2 windowDimension, Vector2 viewport)
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
            return realPosition;
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
                            Debug.Print("Collision with tile at {" + x + "," + y + "}");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
