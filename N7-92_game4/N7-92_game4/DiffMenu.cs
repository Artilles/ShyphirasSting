using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N7_92_game4
{
    public class DiffMenu : Menu
    {
        public DiffMenu()
        {
        }

        public void Load()
        {
            menuFont = ContentClass.Fonts["myFont"];
            menuItems = new List<string> { "Don't Hurt Me", "Hey, Not Too Rough", "I Have No Fear", "Hurt Me Plenty" };

            MeasureMenu();
        }

        public override void Select()
        {
            GameBase.Audio.StopSong();
            GameBase.StartGame(selectedIndex);
        }
    }
}
