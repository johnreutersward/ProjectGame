using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectGame
{
    public class ChooseChar
    {
        private List<string> CharacterList;
        private int iteratorchar;
        public Vector2 Position = new Vector2(0, 0);
        public string ChooseCharacter { get; set; }
        
        public int IterChar
        {
            get
            {
                return iteratorchar;
            }
            set
            {
                iteratorchar = value;
                if (iteratorchar > CharacterList.Count - 1) iteratorchar = CharacterList.Count - 1;
                if (iteratorchar < 0) iteratorchar = 0;
            }
        }
        public ChooseChar()
        {
            ChooseCharacter = "Choose your hero";
            CharacterList = new List<string>();
            CharacterList.Add("Kjell-Erik - the Commoner");
            CharacterList.Add("Hans-Krister - the Druid");
            CharacterList.Add("Sir Excellence - the Noble Knight");
            CharacterList.Add("Nighty Knight - the Keeper of grief");
            CharacterList.Add("Back");
            iteratorchar = 0;

        }

        public int OptionCount()
        {
            return CharacterList.Count;
        }

        public string GetItem(int index)
        {
            return CharacterList[index];
        }

        public void DrawMenu(SpriteBatch batch, float screenWidth, SpriteFont Neverwinter, Texture2D bg)
        {
            batch.Draw(bg, new Vector2(0, 0), Color.White);
            batch.DrawString(Neverwinter, ChooseCharacter, new Vector2(screenWidth / 2 - Neverwinter.MeasureString(ChooseCharacter).X / 2, 20), Color.White);
            int yPos = 100;
            
            for (int i = 0; i < OptionCount(); i++)
            {
                Color DefColor = Color.White;
                if (i == iteratorchar)
                {
                    DefColor = Color.SaddleBrown;
                }

                

                batch.DrawString(Neverwinter, GetItem(i), new Vector2(30, yPos), DefColor);
                yPos += 50;
            }
        }

    }
}

