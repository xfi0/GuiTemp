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
                UpdateButtons();
                if (GameObject.Find("Main Camera") != null)
                {
                    Main.UpdateGUI();
                    Main.ShowGui();
                    MelonLogger.Msg("Gui Initialized!"); // this was for development remove if you want
                }
                Notifications.Library.Update();
            }
            catch (Exception ex) // melonloader/etc/logs for the error and maybe send it to me or sum if gui aint working
            {
                MelonLogger.Error(ex);
            }
        }
        public static void UpdateButtons()
        {
            rightPrimary = EasyInputs.GetPrimaryButtonDown(EasyHand.RightHand);
            rightSecondary = EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand);
            leftPrimary = EasyInputs.GetPrimaryButtonDown(EasyHand.LeftHand);
            leftSecondary = EasyInputs.GetSecondaryButtonDown(EasyHand.LeftHand);
            leftGrab = EasyInputs.GetGripButtonDown(EasyHand.LeftHand);
            rightGrab = EasyInputs.GetGripButtonDown(EasyHand.RightHand);
            leftTrigger = EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand);
            rightTrigger = EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
        }
        public static bool rightPrimary = EasyInputs.GetPrimaryButtonDown(EasyHand.RightHand);
        public static bool rightSecondary = EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand);
        public static bool leftPrimary = EasyInputs.GetPrimaryButtonDown(EasyHand.LeftHand);
        public static bool leftSecondary = EasyInputs.GetSecondaryButtonDown(EasyHand.LeftHand);
        public static bool leftGrab = EasyInputs.GetGripButtonDown(EasyHand.LeftHand);
        public static bool rightGrab = EasyInputs.GetGripButtonDown(EasyHand.RightHand);
        public static bool leftTrigger = EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand);
        public static bool rightTrigger = EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
    }
}
