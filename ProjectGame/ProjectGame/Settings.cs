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
            SettingsItems.Add("Back");
            iterator = 0;
        }
        public int OptionCount()
        {
            return SettingsItems.Count;
        }

        public string GetItem(int index)
        {
            return SettingsItems[index];
        }

        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter)
        {
            batch.DrawString(Neverwinter, Options, new Vector2(screenWidth / 2 - Neverwinter.MeasureString(Options).X / 2, 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < OptionCount(); i++)
            {
                Color DefColor = Color.White;
                if (i == iterator)
                {
                    DefColor = Color.Gray;
                }

                batch.DrawString(Neverwinter, GetItem(i), new Vector2(screenWidth / 2 - Neverwinter.MeasureString(GetItem(i)).X / 2, yPos), DefColor);
                yPos += 50;
            }
        }

    }
}

