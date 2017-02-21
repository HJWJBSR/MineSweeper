using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Manager.Physics;
using Common.Manager.Device;
using Common.Manager.Window;

namespace The_True_Minesweeper.Minesweeper
{
    class Main
    {
        public static SweeperSource Player = new SweeperSource();

        public static void OnRender(NativeDevice device)
        {
            Player.OnDraw(device);
        }

        public static void OnUpdate(TimeSpan PassTime)
        {
            Player.Update(PassTime);

            if (Input.LeftButton)
            {
                Console.Write(Input.MousePos.x); Console.Write(' ');
                Console.Write(Input.MousePos.y);
                Player.Judge_Pos(true); // -2 -1 0 +
                Game.Do_it();
            }

            if (Input.RightButton)
            {
                Player.Judge_Pos(true);
                Game.Flag();
            }

            PhysicsWorld.Update(PassTime);
        }
    }
}
