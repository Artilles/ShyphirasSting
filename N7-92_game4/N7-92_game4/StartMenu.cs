using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N7_92_game4
{
    public class StartMenu : Menu
    {
        public StartMenu()
        {
            
        }
        public void Load()
        {
            menuFont = ContentClass.Fonts["myFont"];
            menuItems = new List<string> { "New Game", "Level Select", "Highscores", "Options", "Quit" };
            MeasureMenu();
        }
        public override void Select()
        {
            switch (selectedIndex)
            {
                default:
                    break;
                case 0:
                    GameBase.NewGame();
                    break;
                case 1:
                    GameBase.LevelMenu();
                    break;
                case 2:
                    GameBase.Highscores();
                    break;
                case 3:
                    GameBase.Option();
                    break;
                case 4:
                    GameBase.QuitGame();
                    break;
            }
        }
    }
}
