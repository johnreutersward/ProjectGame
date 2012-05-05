using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    class Char
    {
        public Texture2D myChar;
        public Texture2D WizardIco;
        public Texture2D KnightIco;
        public Vector2 myCharVector = Vector2.Zero;
        public int speed = 1;
        public int chosenChar;
    }
}
