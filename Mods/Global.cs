using System;
using GuiTemp.Menu;

namespace GuiTemp.Mods
{
    internal class Global
    {
        public static void MainMenu()
        {
            Main.GuiState = "Main";
            Main.SelectedOptionIndex = 0;
            Main.CurrentPage = 0;
            Main.NavigateToMenu("Main");
        }
        public static void MenuSettings()
        {
            Main.GuiState = "Settings";
            Main.SelectedOptionIndex = 0;
            Main.CurrentPage = 0;
            Main.NavigateToMenu("Settings");
        }

        public static void MovementMods()
        {
            Main.GuiState = "Movement";
            Main.SelectedOptionIndex = 0;
            Main.CurrentPage = 0;
            Main.NavigateToMenu("Movement");
        }
    }
}
