﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace ProjectGame
{
    abstract class Sprite
    {
        Texture2D textureImage;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;
        protected Vector2 speed;
        protected Vector2 position;

        public abstract Map currentMap
        {
            get;
            set;
        }

        
        //This constructor calls the next one (which has the millisecondsPerFrame attribute)
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed) 
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame, null)
        {}

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, Map currentMap)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.currentMap = currentMap;
        }

        public abstract Vector2 direction
        {
            get;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, 
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), 
                Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle((int)position.X + collisionOffset, (int)position.Y + collisionOffset, frameSize.X - (collisionOffset * 2), frameSize.Y - (collisionOffset * 2));
            }
        }

    }
}
