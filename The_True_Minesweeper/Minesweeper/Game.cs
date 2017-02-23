using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Manager.Physics;
using Common.Manager.Device;

namespace The_True_Minesweeper.Minesweeper
{
    class Game
    {
        #region var
        public static int[,] View = new int[16, 30];
        // 0 : Able  1 : Disable  2 : Flaged  3 : ?
        public static int[,] State = new int[16, 30];
        // 0~8 : Numbers  -1 : Sweeper
        public static int Game_Cnt = 0;
        public static int Width_Cnt = 30, Height_Cnt = 16, Brick_Cnt = 480, Sweeper_Cnt = 99;
        public static int Remained_Sweeper = 99, Remained_Brick = Brick_Cnt - Remained_Sweeper;
        public static bool[] For_Random = new bool[Brick_Cnt];
        public static int State_of_Game = 0;
        // 0 : All of Disabled 1 : In Game 2 : Lose the game
        public static int[,] Directions = new int[9, 2];
        #endregion

        #region Tools

        static void Swap(ref bool x, ref bool y)
        {
            bool z = x;
            x = y;
            y = z;
        }

        static Random random = new Random();

        public static int GetRand(int x)
        {
            return random.Next() % x;
        }

        public static void Trans_To_Pos(int z, ref int x, ref int y)
        {
            x = z / Width_Cnt; y = z % Width_Cnt;
        }

        public static void Trans_To_Num(int x, int y, ref int z)
        {
            z = x * Width_Cnt + y;
        }

        static bool Judge_Point(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Height_Cnt && y < Width_Cnt;
        }

        #endregion

        #region Game

        public static void Pretreat()
        {
            for (int i = 0; i < 9; i++)
                Directions[i, 0] = Directions[i, 1] = 0;
            Directions[0, 0] = Directions[0, 1] = Directions[1, 1] = Directions[2, 1] = Directions[3, 0] = Directions[6, 0] = -1;
            Directions[2, 0] = Directions[5, 0] = Directions[6, 1] = Directions[7, 1] = Directions[8, 1] = Directions[8, 0] = 1;
        }

        public static bool Judge()
        {
            int x, y;
/*            for (int i = 0; i < Height_Cnt; i++)
                for (int j = 0; j < Width_Cnt; j++)
                    if (State[i, j] == -1) Console.WriteLine(new Vector2(i, j));*/
            for (int i = 0; i < Height_Cnt; i ++)
                for (int j = 0; j < Width_Cnt; j ++)
                {
                    if (State[i, j] == -1) continue;
                    for (int k = 0; k < 9; k ++)
                    {
                        x = i + Directions[k, 0];
                        y = j + Directions[k, 1];
                        if (Judge_Point(x, y) && State[x, y] == -1)
                            State[i, j] ++;
                    }
                    if (State[i, j] == 8) return false;
                }
            return true;
        }

        public static void Get_New()
        {
            Remained_Brick = Brick_Cnt - Remained_Sweeper;

            int x, y;
            int Random_Cnt = Brick_Cnt;
            Remained_Sweeper = 99;

            for (int i = 0; i < 9; i++)
            {
                x = SweeperSource.Pos_Wid + Directions[i, 0];
                y = SweeperSource.Pos_Hei + Directions[i, 1];
                if (Judge_Point(x, y))
                    Random_Cnt--;
            }

            for (int i = 0; i < Sweeper_Cnt; i ++)
                For_Random[i] = true;
            for (int i = Sweeper_Cnt; i < Random_Cnt; i++)
                For_Random[i] = false;

            do
            {
                for (int i = Random_Cnt - 1; i != -1; i --)
                    Swap(ref For_Random[i], ref For_Random[GetRand(i + 1)]);
                
                for (int i = 0, Now = 0; i < Height_Cnt; i ++)
                    for (int j = 0; j < Width_Cnt; j ++)
                    {
                        if (Math.Abs(i - SweeperSource.Pos_Hei) <= 1 &&
                            Math.Abs(j - SweeperSource.Pos_Wid) <= 1)
                        {
                            State[i, j] = 0;
                            continue;
                        }
                        if (For_Random[Now ++])
                            State[i, j] = -1;
                        else
                            State[i, j] = 0;
                    }
            } while (!Judge());
            
        }

        public static void Flag()
        {
            int x = SweeperSource.Pos_Hei, y = SweeperSource.Pos_Wid;
            if (x < 0 || View[x, y] == 0) return;
            if (View[x, y] == 2) Remained_Sweeper ++;
            View[x, y]++;
            if (View[x, y] > 3) View[x, y] = 1;
            if (View[x, y] == 2) Remained_Sweeper --;
        }

        public static void EnAble(int x, int y)
        {
            if (View[x, y] == 0 || View[x, y] == 2) return;
            View[x, y] = 0;
            if (-- Remained_Brick == 0) Win();
            if (State[x, y] != 0) return;
            for (int i = 0; i < 9; i ++)
            {
                int Nowx = Directions[i, 0] + x, Nowy = Directions[i, 1] + y;
                if (Judge_Point(Nowx, Nowy))
                    EnAble(Nowx, Nowy);
            }
        }

        public static void Win()
        {
            State_of_Game = 2;
            SweeperSource.Should_Time_Stop = true;
        }

        public static void Lose()
        {
            State_of_Game = 2;
            SweeperSource.Should_Time_Stop = true;
            for (int i = 0; i < Height_Cnt; i++)
                for (int j = 0; j < Width_Cnt; j++)
                    View[i, j] = 0;
        }

        public static void Do_it()
        {
            int x = SweeperSource.Pos_Wid, y = SweeperSource.Pos_Hei;
            if (y == -1)
            {
                SweeperSource.Clean_Old();
                return;
            }
            if (y == -2) return;
            if (State_of_Game == 0)
            {
                SweeperSource.Restart();
                State_of_Game = 1;
                Game_Cnt++;
            }
            SweeperSource.Should_Time_Stop = false;
            if (View[y, x] == 2) return;
            if (State[y, x] == -1)
                Lose();
            else
                EnAble(y, x);
            Console.WriteLine(new Vector2(y, x));
            Console.WriteLine(View[y, x]);
            Console.WriteLine(State[y, x]);
            Console.WriteLine(State_of_Game);
            Console.WriteLine();
        }
        #endregion
    }
}
