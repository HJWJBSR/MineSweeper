using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Manager.Physics;

namespace The_True_Minesweeper.Minesweeper
{
    class Input
    {
        static Vector2 g_mousepos = new Vector2(0, 0);
        static bool g_mouse_left = false;
        static bool g_mouse_right = false;

        public static Vector2 MousePos
        {
            get { return g_mousepos; }
            set { g_mousepos = value; }
        }

        public static bool LeftButton
        {
            get { return g_mouse_left; }
            set { g_mouse_left = value; }
        }

        public static bool RightButton
        {
            get { return g_mouse_right; }
            set { g_mouse_right = value; }
        }
    }
}
