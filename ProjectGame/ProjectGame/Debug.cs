using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    static class Debug
    {
        public static void Print(String msg)
        {
            //We could use this to save debug onto a text file here aswell
            DateTime currentTime = DateTime.Now.ToUniversalTime();
            String outMsg = msg + " [" + currentTime + "]";
            Console.WriteLine(outMsg);
        }

        public static void OnScreenPrint(SpriteBatch spriteBatch, SpriteFont font, String msg, Vector2 screenPosition)
        {
            spriteBatch.DrawString(font, msg, screenPosition, Color.Silver);

            spriteBatch.DrawString(font, msg, screenPosition, Color.Silver); 

        }
    }
}
