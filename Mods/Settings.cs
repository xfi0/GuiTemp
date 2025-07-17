using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiTemp.Mods
{
    internal class Settings
    {
        public static void RGB(bool on)
        {
            if (on)
            {
                GuiTemp.Menu.Main.GuiRGB = true;
            }
            else
            {
                GuiTemp.Menu.Main.GuiRGB = false;
            }
        }
    }
}
