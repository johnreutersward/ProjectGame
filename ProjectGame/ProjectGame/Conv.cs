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
        public static int counter;
        public static bool end = false;
        String texter = "text1";


     public void DrawConv(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter, Texture2D background)
        {
        
         // draw conv box
         batch.Draw(background, new Vector2(100, 100), Color.White);
         // draw text   
         batch.DrawString(Neverwinter, texter, new Vector2(120, 120), Color.White);
        
        
     }




    }
}

