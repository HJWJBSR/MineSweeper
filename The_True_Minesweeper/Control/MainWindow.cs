using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Manager.GameWindow;
using System.Windows;
using System.Web;
using System.Windows.Forms;
using System.Drawing;
using The_True_Minesweeper.Minesweeper;

namespace The_True_Minesweeper.Minesweeper
{
    class MainWindow : GameWindow
    {
        MainPage main_page = new MainPage();
        
        public static int DefWidth = 600;
        public static int DefHeight = 369;
        
        protected override void InitalizeResource()
        {
            //Get_Last_Data();
            SweeperSource.Reset();
            SweeperSource.Restart();
            base.InitalizeResource();
        }

        public MainWindow()
        {
            Initalize("Minesweeper", "", DefWidth, DefHeight);
            CurrentPage = main_page;
        }

    }
}
