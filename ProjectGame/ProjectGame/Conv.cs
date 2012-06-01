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
        public static List<string> ConvItems;
        public static int counter;
        public static bool end = false;
        String texter = "text1";
        public Conv()
        {
            ConvItems = new List<string>();
            
        }

        public int OptionCount()
        {
           
            return ConvItems.Count;
        }

        public string GetItem(int index)
        {
            return ConvItems[index];
        }

     public void DrawBox(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter, Texture2D background)
        {
        
         // draw conv box
         batch.Draw(background, new Vector2(50, 100), Color.White);
         // draw text   
         //batch.DrawString(Neverwinter, texter, new Vector2(120, 120), Color.White);
        
        
     }

     public void DrawConv(SpriteBatch batch, int x, int y, SpriteFont Neverwinter, String textme)
     {

         // draw text   
         batch.DrawString(Neverwinter, textme, new Vector2(x, y), Color.White);
        
     }




    }
}

