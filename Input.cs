using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        // Load list of available keyboard buttons.
        private static Hashtable keyTable = new Hashtable();

        // Check if certain button is pressed.
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }

            return (bool)keyTable[key];
        }

        // Check if keyboard button is pressed.
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }

    
}
