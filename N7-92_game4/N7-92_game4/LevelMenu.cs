using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N7_92_game4
{
    public class LevelMenu : Menu
    {
         public LevelMenu()
        {

        }
        public void Load()
        {
            menuItems = new List<string> { "One", "Two", "Three", "Four", "Five", "Six" };
            MeasureMenu();
        }
        public override void Select()
        {
            switch (selectedIndex)
            {
                default:
                    break;
            }
            GameBase.SelectLevel(selectedIndex+1);
        }
    }
}
