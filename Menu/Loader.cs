using easyInputs;
using MelonLoader;
using System;
using UnityEngine;

namespace GuiTemp.Menu
{
    public class Loader : MelonMod // the main gui loader
    {
        private bool GuiInitialized = false;

        public override void OnApplicationStart() // add anythig you need to happen at start up
        {
            if (!GuiInitialized && GameObject.Find("Main Camera") != null)
            {
                Notifications.Library.Initialize();
                Main.LoadOnce();
                GuiInitialized = true;
            }
        }

        public override void OnUpdate() // executes everyframe
        {
            try
            {
                if (GameObject.Find("Main Camera") != null)
                {
                    Main.UpdateGUI();
                    Main.ShowGui();
                    MelonLogger.Msg("Gui Initialized!");
                }
                Notifications.Library.Update();
            }
            catch (Exception ex) // melonloader/logs for the error and maybe send it to me or sum if gui aint working
            {
                MelonLogger.Error(ex);
            }
        }
    }
}