﻿using GuiTemp.Mods;
using System;
using static GuiTemp.Menu.ButtonInfo;
using static GuiTemp.Menu.Main;

namespace GuiTemp.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            // MAIN MENU
            new ButtonInfo[]
            {
                new ButtonInfo { buttonText = "Settings", method = () => NavigateToMenu("Settings"), isTogglable = false },
                new ButtonInfo { buttonText = "Movement", method = () => NavigateToMenu("Movement"), isTogglable = false, toolTip = "Opens Movement Mods" }
            },

            // SETTINGS MENU
            new ButtonInfo[]
            {
                new ButtonInfo { buttonText = "Return to Main Menu", method = () => NavigateToMenu("Main"), isTogglable = false },
                new ButtonInfo { buttonText = "RGB Gui", enableMethod = () => Settings.RGB(true), disableMethod = () => Settings.RGB(false), toolTip = "Makes Some Of The Gui RGB", isTogglable = true },
                new ButtonInfo { buttonText = "Toggleable Placeholder 1", isTogglable = true },
                new ButtonInfo { buttonText = "Toggleable Placeholder 2", isTogglable = true },
                new ButtonInfo { buttonText = "Toggleable Placeholder 3", isTogglable = true },
                new ButtonInfo { buttonText = "Untoggleable Placeholder 1", isTogglable = false },
                new ButtonInfo { buttonText = "Untoggleable Placeholder 2", isTogglable = false },
                new ButtonInfo { buttonText = "Untoggleable Placeholder 3", isTogglable = false }
            },

            // MOVEMENT MENU
            new ButtonInfo[]
            {
                new ButtonInfo { buttonText = "Return to Main Menu", method = () => NavigateToMenu("Main"), isTogglable = false, toolTip = "Returned To Main" },
                new ButtonInfo { buttonText = "Long Arms", enableMethod = () => Movement.LongArms(true), disableMethod = () => Movement.LongArms(false), toolTip = "Makes Your Arms Long", isTogglable = true },
                new ButtonInfo { buttonText = "Trigger Fly", method = () => Movement.TriggerFly(), toolTip = "Makes Your Arms Long", isTogglable = true }
            },
        };
    }
}
