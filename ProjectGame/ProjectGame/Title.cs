using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class Title
    {
      public void DrawEnd(SpriteBatch batch, int screenWidth, Texture2D bg)
        {
            batch.Draw(bg, new Vector2(0, 0), Color.White);
        }
    }
}
