﻿using System;
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
            SettingsItems.Add("Back");
            iterator = 0;
            InfoText = string.Empty;
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


                else if (IterSettings == 0)
                {

                    batch.DrawString(Neverwinter, "Use left / right key to change the value", new Vector2(150, 300), Color.White);

                    if (onoffmusic == 1)
                    {
                        if (i == iterator)
                        {
                            DefColor = Color.SaddleBrown;
                        }
                        batch.DrawString(Neverwinter, "On", new Vector2(450, 100), DefColor);

                    }
                    else if (onoffmusic == 0)
                    {
                        if (i == iterator)
                        {
                            DefColor = Color.SaddleBrown;
                        }

                        batch.DrawString(Neverwinter, "Off", new Vector2(450, 100), DefColor);

                    }
                }
                batch.DrawString(Neverwinter, GetItem(i), new Vector2(screenWidth / 2 - Neverwinter.MeasureString(GetItem(i)).X / 2, yPos), DefColor);
                yPos += 50;
            }
           // batch.DrawString(Neverwinter, "Use left / right key to change the value", new Vector2(150, 300), Color.White);
        }

        

    }
}

