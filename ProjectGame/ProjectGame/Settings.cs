using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class Settings
    {
        private List<string> SettingsItems;
        private int iterator;
        public string Options { get; set; }
        public string InfoText { get; set; }
        public int onoffmusic;
        public int IterSettings
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > SettingsItems.Count - 1) iterator = SettingsItems.Count - 1;
                if (iterator < 0) iterator = 0;
            }
        }
        public Settings()
        {
            Options = "Options";
            SettingsItems = new List<string>();
            SettingsItems.Add("Music");
            SettingsItems.Add("Toggle Fullscreen");
            SettingsItems.Add("Back");
            iterator = 0;
            onoffmusic = 1;

        }
        public int OptionCount()
        {
            return SettingsItems.Count;
        }

        public string GetItem(int index)
        {
            return SettingsItems[index];
        }
        
        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter, Texture2D bg)
        {
            batch.Draw(bg, new Vector2(0, 0), Color.White);
            batch.DrawString(Neverwinter, Options, new Vector2(screenWidth / 2 - Neverwinter.MeasureString(Options).X / 2, 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < OptionCount(); i++)
            {
                Color DefColor = Color.White;
                if (i == iterator)
                {
                    DefColor = Color.SaddleBrown;
                }


                if (IterSettings == 0)
                {

                    batch.DrawString(Neverwinter, "Left key - Off, Right key - On", new Vector2(200, 350), Color.White);

                    if (onoffmusic == 1)
                    {

                        batch.DrawString(Neverwinter, "On", new Vector2(450, 100), Color.SaddleBrown);

                    }
                    else if (onoffmusic == 0 && IterSettings == 0)
                    {


                        batch.DrawString(Neverwinter, "Off", new Vector2(450, 100), Color.SaddleBrown);

                    }
                }
                if (IterSettings == 1)
                {
                    batch.DrawString(Neverwinter, "Press Enter", new Vector2(320, 350), Color.White);
                }
                if (IterSettings == 2)
                {
                    batch.DrawString(Neverwinter, "Press Enter", new Vector2(320, 350), Color.White);
                }


                batch.DrawString(Neverwinter, GetItem(i), new Vector2(screenWidth / 2 - Neverwinter.MeasureString(GetItem(i)).X / 2, yPos), DefColor);
                yPos += 50;
            }
           // batch.DrawString(Neverwinter, "Use left / right key to change the value", new Vector2(150, 300), Color.White);
        }
       

        

    }
}

