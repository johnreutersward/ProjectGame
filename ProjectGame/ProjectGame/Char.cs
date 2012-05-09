using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    class Char
    {   // character textures
        public Texture2D myChar;
        public Texture2D WizardIco;
        public Texture2D KnightIco;

        //char position
        public Vector2 myCharVector = Vector2.Zero;

        // characters movement speed
        public int speed = 10;

        // chosen character value
        public int chosenChar;

        // string name;

        // Character abilities
        public int HP;
        public int MP;
        public int MaxHP;
        public int MaxMP;
        public int Money;
        public int Exp;
        public int level;
        //public int Hit;
        //public int Damage;
        //Weapons weapon;
        //Items Item;


        public void Initialize(Texture2D texture, Vector2 position)
        {
            myChar = texture;
            MaxHP = 100;
            MaxMP = 100;
            Money = 10;
            Exp = 0;
            level = 1;
            HP = 100;
            MP = 100;

        }

        public void Update()
        {
        }
    }
}
