using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    class Enemy
    {
        public Texture2D enemy1;
        public Vector2 enemyVector = Vector2.Zero;

        //public int EnemySpeed = 1;
        public int HP;
        public int MP;
        public int MaxHP;
        public int MaxMP;
        public int Money;
        public int level;
        public int Hit;
        // public int Damage;
        public int ExpValue;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            enemy1 = texture;
            MaxHP = 20;
            MaxMP = 5;
            Money = 8;
            ExpValue = 3;
            level = 1;
            HP = 20;
            MP = 5;
            Hit = 2;

        }
        // public void Update(GameTime gameTime) { }
    }
}
