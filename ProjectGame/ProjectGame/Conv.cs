using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class Conv
    {
    public void Update(GameTime gameTime)
        { 
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            Player.doConversation = false;
        }
        }


        public void DrawConv(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter, Texture2D background)
        {
            batch.Draw(background, new Vector2(100, 100), Color.White);
            batch.DrawString(Neverwinter, "text", new Vector2(120, 120), Color.White);
        }




    }
}

