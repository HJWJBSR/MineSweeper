using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Common.Manager.Window;
using Common.Manager.Device;
using Common.Manager.Physics;

using The_True_Minesweeper.Minesweeper;

namespace The_True_Minesweeper.Minesweeper
{
    class MainPage : NativePage
    {
        DateTime Begin_Time = DateTime.Now;

        protected override void OnKeyDown(object sender, BoardClickEventArgs e)
        {
            SweeperSource.Press_Down(e);
            base.OnKeyDown(sender, e);
        }

        protected override void OnMouseDown(object sender, MouseClickEventArgs e)
        {
            if (e.button == MouseButton.LeftButton)
                Input.LeftButton = true;
            else
            if (e.button == MouseButton.RightButton)
                Input.RightButton = true;
            base.OnMouseDown(sender, e);
        }

        protected override void OnMouseUp(object sender, MouseClickEventArgs e)
        {
            Input.LeftButton = Input.RightButton = false;
            base.OnMouseUp(sender, e);
        }
        
        protected override void OnDraw(object sender, DrawEventArgs e)
        {
            Minesweeper.Main.OnRender(e.device as NativeDevice);

            base.OnDraw(sender, e);
        }

        protected override void OnUpdate(object sender, EventArgs e)
        {
            TimeSpan PassTime = DateTime.Now - Begin_Time;

            Minesweeper.Main.OnUpdate(PassTime);

            base.OnUpdate(sender, e);
        }

        protected override void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            Input.MousePos = (sender as MainWindow).
                AsCenter(new Vector2(e.x, e.y));
            base.OnMouseMove(sender, e);
        }
    }
}
