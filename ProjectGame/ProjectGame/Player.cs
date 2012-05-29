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
        public Point currentFrame = new Point(1,1);
        public Point sheetSize = new Point(5,9);
        public Vector2 Position;
        public int collisionOffset = 10;
        public int speed = 5;
        public int timeSinceLastFrame = 0;
        public int defaultMillisecondsPerFrame = 60;
        public static bool doConversation;
        public static int convset = 0;
        public static int count = 0;
        
         

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
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
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
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
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
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
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

             if (doConversation && convset == 4)
            {
              Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
              Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "You are to late!");
              Game1.Conversationbox.DrawConv(spriteBatch, 120, 240, Game1.textconv, "YOU: What happend here?");
            }

            if (doConversation && convset == 5)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "The town folk, they are all dead!");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 240, Game1.textconv, "YOU: What killed them?");
            }
            if (doConversation && convset == 6)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "An evil monster sent from the heavens to purge this land");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 140, Game1.textconv, "The elder call him ... the garbage collecter, gods be true!");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 240, Game1.textconv, "Let me guess, was he mumbling about mark-and-sweep?");
            }
            if (doConversation && convset == 7)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "What!? You knew of the monster");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 240, Game1.textconv, "YOU: Yes, but I heard the town was under attack from Goblins?");
            }
            if (doConversation && convset == 8)
            {
                Game1.Conversationbox.DrawBox(spriteBatch, 800, Game1.textconv, Game1.bg);
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 120, Game1.textconv, "The monster took care of them aswell");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 140, Game1.textconv, "... he said that the authors had not kept the reference to the ArrayList<Goblins> and they all had to die!");
                Game1.Conversationbox.DrawConv(spriteBatch, 120, 240, Game1.textconv, "YOU: Neat bro, cya");
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
                          else if (tile != null && tile.TileIndex == 740)
                         {
                             Game1.map = Game1.inhousMapb;
                             Position = new Vector2(380, 284);
                         
                              return true;
                         }
                        else if (tile != null && tile.TileIndex == 332)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(1768, 300);


                            return true;
                        }

                        else if (tile != null && tile.TileIndex == 741)
                        {
                            Game1.map = Game1.inhousMapa;
                            Position = new Vector2(380, 284);

                            return true;
                        }
                       
                      
                        else if (tile != null && tile.TileIndex == 331)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(2020, 300);


                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 742)
                        {
                            Game1.map = Game1.inhous2Mapa;
                            Position = new Vector2(380, 284);

                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 351)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(2276, 300);


                            return true;
                        }

                        else if (tile != null && tile.TileIndex == 743)
                        {
                            Game1.map = Game1.inhous2Mapb;
                            Position = new Vector2(380, 284);

                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 352)
                        {
                            Game1.map = Game1.forestMap;
                            Position = new Vector2(2528, 340);


                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 760)
                        {
                            Game1.map = Game1.second;
                            Position = new Vector2(100, 100);


                            return true;
                        }


                        else if (tile != null && tile.TileIndex == 942)
                        {
                           // Game1.map = Game1.second;
                            //Position = new Vector2(100, 100);
                            doConversation = true;
                         
                        
                           
                            return true;
                        }
                        else if (tile != null && tile.TileIndex == 1408)
                        {
                            
                            // how many screens count to 1 -> for 2 screens, 2  -> for three screens etc..

                            Game1.gamestate = Game1.GameStates.End; 
                     //       count = 7;
                       //     convset = 4;
                            
                         //   doConversation = true;
                            
                            
                          
                        }
                        
                        else if (tile != null && tile.TileIndex == 1901)
                        {

                            // how many screens count to 1 -> for 2 screens, 2  -> for three screens etc..
                            count = 8;
                            convset = 4;
                            doConversation = true;
                        }
                        //template for tile conv
                        else if (tile != null && tile.TileIndex == 200000)
                        {

                            // how many screens count to 1 -> for 2 screens, 2  -> for three screens etc..
                                   count = 7;
                                   convset = 4;
                                   doConversation = true;
                        }

                       

                    }
                }
            }
            return false;
        }

    }
}