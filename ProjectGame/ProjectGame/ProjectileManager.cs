using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using xTile.Layers;
using xTile.Tiles;

namespace ProjectGame
{
    class ProjectileManager
    {

        public ProjectileManager()
        {
            knives = new List<Knife>();
        }

        public enum ProjectileDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public List<Knife> knives;
        public Texture2D Texture;

        public class Knife
        {
            public Vector2 Position;
            public Texture2D Texture;
            public int Speed = 10;
            public KnifeDirection dir;
            public float kniferotaion = 0.08727f;

            public enum KnifeDirection
            {
                Left,
                Right,
                Up,
                Down
            }
            
            public Rectangle Bounds
            {
                get
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, 1, 1);
                }
            }

            public Knife(Vector2 Position, Texture2D Texture, Knife.KnifeDirection Direction)
            {
                this.Position = Position;
                this.Texture = Texture;
                this.dir = Direction;
            }

        }


        public void setTexture(Texture2D Texture)
        {
            this.Texture = Texture;
            //AddKnife(new Vector2(0, 0), Knife.KnifeDirection.Right);
            

        }

        public void AddKnife(Vector2 Position, Knife.KnifeDirection Dir)
        {
            //Debug.Print("Knife " + Position);
            knives.Add(new Knife(Position, this.Texture, Dir));

        }


    }
}
