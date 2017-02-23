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

            PhysicsWorld.Update(PassTime);
        }
    }
}
