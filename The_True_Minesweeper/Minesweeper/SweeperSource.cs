using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

using Common.Manager.Device;
using Common.Manager.Physics;
using Common.Manager.Window;

namespace The_True_Minesweeper.Minesweeper
{
    class SweeperSource : Shape
    {
        #region var
        public static double Width_Brick = 18.0, Height_Brick = 17.5;
        public static int Width_Cnt = 30, Height_Cnt = 16, Brick_Cnt = 480;
        public static int Pos_Wid = 0, Pos_Hei = 0;
        public static float Brick_Width = 18.0f, Brick_Height = 17.5f;
        public static Image Bomb_Image = null;
        #endregion

        #region TransForm

        public static Vector2 Pos_End = new Vector2();
        public static Vector2 Button_End = new Vector2();
        public static Vector2 delta = new Vector2(), Right = new Vector2(), Down = new Vector2();
        public static Vector2[,] Pos = new Vector2[4, 31];

        public static Vector2 ToVector2(int x, int y)
        {
            x -= Width_Cnt / 2; y -= Height_Cnt / 2;
            return new Vector2(x * Brick_Width, y * Brick_Height);
        }

        public static void Toint(Vector2 x, ref int y, ref int z)
        {
            /*Console.Write(x.x);Console.Write(' ');
            Console.WriteLine(x.y);*/
            if (x.x < Pos[0, 0].x || x.y < Pos[0, 0].y || x.x > Pos_End.x || x.y > Pos_End.y)
            {
                if (x.x < Button_Pos.x || x.y < Button_Pos.y || x.x > Button_End.x || x.y > Button_End.y)
                    y = -2;
                else
                    y = -1;
            }
            else
            {
                y = (int)((x.x - Pos[0, 0].x) / Brick_Width);
                z = (int)((x.y - Pos[0, 0].y) / Brick_Height);
            }
        }
        #endregion

        #region Pretreat
        public static void Reset()
        {
            Game.Pretreat();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j <= Width_Cnt; j++)
                    Pos[i, j] = new Vector2();

            delta = new Vector2(0.0f, 0.0f);
            Right = new Vector2(Brick_Width, 0.0f);
            Down = new Vector2(0.0f, Brick_Height);

            Button_Pos = new Vector2(-Brick_Width / 2, -Brick_Height * Height_Cnt / 2 - 33);
            Button_End = Button_Pos + Down + Right;

            Time_Pos = new Vector2(-140.0f, 150.0f);
            Time_Right = Right;
            Time_Down = Down;

            Pos[0, 0] = new Vector2(-Brick_Width * Width_Cnt / 2, -Brick_Height * Height_Cnt / 2);
            Pos[1, 0] = Pos[0, 0] + Right * Width_Cnt;
            for (int i = 1; i <= Height_Cnt; i++)
            {
                Pos[0, i] = Pos[0, i - 1] + Down;
                Pos[1, i] = Pos[1, i - 1] + Down;
            }
            Pos[2, 0] = Pos[0, 0];
            Pos[3, 0] = Pos[2, 0] + Down * Height_Cnt;
            for (int i = 1; i <= Width_Cnt; i++)
            {
                Pos[2, i] = Pos[2, i - 1] + Right;
                Pos[3, i] = Pos[3, i - 1] + Right;
            }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j <= Width_Cnt; j++)
                    Pos[i, j] += delta;
            Pos_End = Pos[0, 0] + Right * Width_Cnt + Down * Height_Cnt;

        }

        public static void Restart()
        {
            Game.Game_State = 0;
            Pos_Wid = Pos_Hei = 0;
            Time_Now = 0.0f;
            for (int i = 0; i < Height_Cnt; i++)
                for (int j = 0; j < Width_Cnt; j++)
                {
                    Game.View[i, j] = 1;
                    Game.State[i, j] = 0;
                }
            Game.Get_New();
        }
        #endregion

        #region Drawing

        public static float Width_Of_Line = 1.0f;
        public static float Half_Width_Of_Line = Width_Of_Line / 2;
        Vector2 Feat_Line = new Vector2(Half_Width_Of_Line, Half_Width_Of_Line);
        public static float Feat_Right = (float)Width_Brick - Width_Of_Line, Feat_Down = (float)Height_Brick - Width_Of_Line;

        void Fill_Somewhere(NativeDevice device, Vector2 Bg, float Width, float Height, Brush Col)
        {
            device.FillRectangle(Bg.x, Bg.y, Bg.x + Width, Bg.y + Height, Col);
        }

        void Draw_High_Light(NativeDevice device)
        {
            Vector2 Now = new Vector2();
            if (Pos_Wid >= 0)
                Now = ToVector2(Pos_Wid, Pos_Hei);
            else
                Now = Button_Pos;
            Brush Col = new Brush(device, 0.5f, 0.5f, 0.5f);
            Now = Now + Feat_Line;
            Fill_Somewhere(device, Now, Feat_Right, Feat_Down, Col);
        }

        void Draw_Bricks(NativeDevice device)
        {
            Brush Col = new Brush(device, 0.2f, 0.2f, 0.2f);
            Fill_Somewhere(device, new Vector2(-1000.0f, -1000.0f), 2000.0f, 2000.0f, new Brush(device, 0.8f, 0.8f, 0.8f));

            if (Bomb_Image == null) Bomb_Image = new Image(device, "Chijiang_.png");
            /*for (int i = 0; i <= Height_Cnt; i++)
                device.DrawLine(Pos[0, i], Pos[1, i], device.Brush.Black, Width_Of_Line);
            Console.WriteLine(Height_Cnt);
            for (int i = 0; i <= Width_Cnt; i++)
                device.DrawLine(Pos[2, i], Pos[3, i], device.Brush.Black, Width_Of_Line);*/

            //return;

            Vector2 Now = new Vector2();
            for (int i = 0; i < Height_Cnt; i++)
            {
                Now = Pos[0, i] + Feat_Line;
                for (int j = 0; j < Width_Cnt; j++)
                {
                    //device.DrawImage(Now.x + Half_Width_Of_Line, Now.y + Half_Width_Of_Line, Now.x + Brick_Width - Half_Width_Of_Line, Now.y + Brick_Height - Half_Width_Of_Line, at);
                    Fill_Somewhere(device, Now, Feat_Right, Feat_Down, Col);
                    Now += Right;
                }
            }

            Draw_High_Light(device);
        }

        void Draw_Somewhere(NativeDevice device, Vector2 Bg, Vector2 Right, Vector2 Down, float Width = 0.4f)
        {
            device.DrawLine(Bg, Bg + Right, device.Brush.Black, Width);
            device.DrawLine(Bg, Bg + Down, device.Brush.Black, Width);
            device.DrawLine(Bg + Right, Bg + Right + Down, device.Brush.Black, Width);
            device.DrawLine(Bg + Down, Bg + Right + Down, device.Brush.Black, Width);
        }

        public static Vector2 Button_Pos = new Vector2();
        void Draw_A_Button(NativeDevice device)
        {
            Draw_Somewhere(device, Button_Pos, Right, Down, 0.4f);
            Brush Col = new Brush(device, 0.3f, 0.3f, 0.3f);
            Fill_Somewhere(device, Button_Pos + Feat_Line, Feat_Right, Feat_Down, Col);
            device.DrawText(Button_Pos.x + 0.5f, Button_Pos.y - 2.5f, "Re", new Font(device, "", 15.0f), new Brush(device, 0.0f, 0.0f, 0.0f));
        }

        public static Vector2 Time_Pos = new Vector2(), Time_Right = new Vector2(), Time_Down = new Vector2();
        public static Vector2 Counter_Right = new Vector2(), Counter_Pos = new Vector2(), Counter_Down = new Vector2();
        void Draw_Time_And_Counter(NativeDevice device)
        {
            Draw_Somewhere(device, Time_Pos, Time_Right, Time_Down, 0.4f);
            //Draw_Somewhere(device, Counter_Pos, Counter_Right, Counter_Down, 0.4f);
        }

        void Draw_Brick(NativeDevice device, int flag_left, int flag_right, char flag)
        {
            Vector2 Now = ToVector2(flag_left, flag_right);
        }

        public static Brush[] Num_Col = new Brush[20];
        public static bool Is_Pretreat = false;
        public void Pretreat_For_Colors(NativeDevice device)
        {
            Num_Col[1] = new Brush(device, 0 / 256f, 73 / 256f, 255 / 256f);
            Num_Col[2] = new Brush(device, 0 / 256f, 180 / 256f, 23 / 256f);
            Num_Col[3] = new Brush(device, 185 / 256f, 0 / 256f, 0 / 256f);
            Num_Col[4] = new Brush(device, 18 / 256f, 0 / 256f, 185 / 256f);
            Num_Col[5] = new Brush(device, 128 / 256f, 55 / 256f, 65 / 256f);
            Num_Col[6] = new Brush(device, 54 / 256f, 163 / 256f, 157 / 256f);
            Num_Col[7] = new Brush(device, 237 / 256f, 255 / 256f, 0 / 256f);
            Num_Col[8] = new Brush(device, 160 / 256f, 0 / 256f, 0 / 256f);
        }

        void Draw_Num(NativeDevice device, int flag_left, int flag_right, int num)
        {
            //Console.WriteLine(flag_left);
            //Console.WriteLine(flag_right);
            Vector2 Now = ToVector2(flag_right, flag_left);
            device.DrawText(Now.x + 4.8f, Now.y - 2.0f, Convert.ToString(num), new Font(device, "", 15.0f), Num_Col[num]);
        }

        public void Draw_Numbers(NativeDevice device)
        {
            Game.View[1, 1] = 0;
            for (int i = 0; i < Height_Cnt; i++)
                for (int j = 0; j < Width_Cnt; j++)
                    if (Game.View[i, j] == 2) Draw_Brick(device, i, j, 'F');
                    else
                        if (Game.View[i, j] == 3) Draw_Brick(device, i, j, '?');
                    else
                        if (Game.View[i, j] == 0)
                    {
                        if (Game.State[i, j] == -1) Draw_Brick(device, i, j, '*');
                        else
                            Draw_Num(device, i, j, Game.State[i, j]);
                    }
            Game.View[1, 1] = 1;
        }

        public override void OnDraw(object device)
        {
            NativeDevice new_device = device as NativeDevice;
            new_device.SetTransform(Transform);

            Draw_Bricks(new_device);
            Draw_A_Button(new_device);
            Draw_Time_And_Counter(new_device);

            if (!Is_Pretreat)
                Pretreat_For_Colors(new_device);
            Draw_Numbers(new_device);

            new_device.ClearTransform();
            base.OnDraw(device);
        }

        #endregion

        #region React

        Vector2 Last_Pos = new Vector2();
        public void Judge_Pos(bool flag)
        {
            int x = 0, y = 0;
            if (Last_Pos == Input.MousePos && !flag) return;
            Toint(Input.MousePos, ref x, ref y);
            Pos_Wid = x; Pos_Hei = y;
            return;
        }

        public static float Time_Now = 0.0f;
        public static bool Should_Time_Stop = true;

        static public void Press_Down(BoardClickEventArgs e)
        {
            if (e.key == KeyCode.Space && e.IsDown)
                Game.Do_it();
            else
            if (e.key == KeyCode.Up && e.IsDown && Pos_Hei > 0)
                Pos_Hei -= 1;
            else
            if (e.key == KeyCode.Down && e.IsDown && Pos_Hei < Height_Cnt - 1)
                Pos_Hei += 1;
            else
            if (e.key == KeyCode.Left && e.IsDown && Pos_Wid > 0)
                Pos_Wid -= 1;
            else
            if (e.key == KeyCode.Right && e.IsDown && Pos_Wid < Width_Cnt - 1)
                Pos_Wid += 1;
        }

        public override void Update(TimeSpan PassTime)
        {
            if (!Should_Time_Stop)
                Time_Now += (float)PassTime.TotalSeconds;
        }

        #endregion

    }
}
