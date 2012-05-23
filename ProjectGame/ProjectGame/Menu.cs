using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGame
{
    public class Menu
    {
        private List<string> MenuItems;
        private int iterator;
        public string InfoText { get; set; }
        public string Title { get; set; }
        public int Iterator
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1) iterator = MenuItems.Count - 1;
                if (iterator < 0) iterator = 0;
            }
        }

        public Menu()
        {
            Title = "Land of Magic";
            MenuItems = new List<string>();
            MenuItems.Add("Single Player");
            MenuItems.Add("Settings");
            MenuItems.Add("Something");
            MenuItems.Add("Exit Game");
            Iterator = 0;
            InfoText = string.Empty;
        }

        public int OptionCount()
        {
            return MenuItems.Count;
        }

        public string GetItem(int index)
        {
            return MenuItems[index];
        }

        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter, Texture2D bg)
        {
            batch.Draw(bg, new Vector2(-100, -400), Color.White);
            batch.DrawString(Neverwinter, Title, new Vector2(screenWidth / 2 - Neverwinter.MeasureString(Title).X / 2, 20), Color.White);
            int yPos = 100;
            for (int i = 0; i < OptionCount(); i++)
            {
                Color DefColor = Color.White;
                if (i == Iterator)
                {
                    DefColor = Color.Gray;
                }

                batch.DrawString(Neverwinter, GetItem(i), new Vector2(screenWidth / 2 - Neverwinter.MeasureString(GetItem(i)).X / 2, yPos), DefColor);
                yPos += 50;
            }
        }



        public void DrawEnd(SpriteBatch batch, int screenWidth, SpriteFont Neverwinter, Texture2D bg)
        {
            batch.Draw(bg, new Vector2(0, 0), Color.White);
            batch.DrawString(Neverwinter, InfoText, new Vector2(screenWidth / 2 - Neverwinter.MeasureString(InfoText).X / 2, 300), Color.White);
            string prompt = "Press Enter to Continue";
            batch.DrawString(Neverwinter, prompt, new Vector2(screenWidth / 2 - Neverwinter.MeasureString(prompt).X / 2, 400), Color.White);
        }
    }
}
