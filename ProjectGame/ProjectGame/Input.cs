using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ProjectGame
{
    public class Input
    {
        private KeyboardState keyboardState;
        private KeyboardState lastState;

        public Input()
        {
            keyboardState = Keyboard.GetState();
            lastState = keyboardState;
        }

        public void Update()
        {
            lastState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public bool Up
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.MainMenu || Game1.gamestate == Game1.GameStates.ChooseCharacter)
                {
                    return keyboardState.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Up);
                } 
            }
        }

        public bool Down
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.MainMenu || Game1.gamestate == Game1.GameStates.ChooseCharacter)
                {
                    return keyboardState.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Down);
                }
            }
        }
  
        public bool Enter
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter);
            }
        }
    }
}
