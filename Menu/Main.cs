using easyInputs;
using GuiTemp.Menu;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using static GuiTemp.Menu.ButtonInfo;
using static GuiTemp.Menu.Buttons;

namespace GuiTemp.Menu
{
    internal class Main
    {
        /* ALL COMMENTS ARE TO HELP YOU OR ME
         * IF YOU USE THIS TEMP GIVE CREDITS OR ATLEAST MAKE IT CLEAR YOU USED IT
         * IM NOT GONNA DOX YOU IF YOU DONT BUT STILL
         */
        // gui configuration
        private static string[] SectionNames;
        private static Dictionary<string, MenuOption[]> Sections = new Dictionary<string, MenuOption[]>(); // ALL THE SECTIONS oop caps lock

        // navigation
        public static int CurrentPage = 0;
        public static int OptionsPerPage = 10; 
        public static int SelectedOptionIndex = 0;

        // input
        public static float lastScrollTime = 0f;
        public static float scrollCooldown = 0.2f;
        private static bool canActivate = true;

        // ui objs and text
        private static GameObject GUIOBJ1;
        private static GameObject GUIOBJ2;
        public static GameObject MainCamera;
        private static Text GuiDisplayText;

        // gui states
        public static string GuiState = "Main"; // selection you start on
        public static string GuiColor = "purple"; // menu color
        public static bool GuiRGB = false; 
        public static float GuiRGBTimer = 0f; 
        public static MenuOption[] CurrentViewingMenu = null; // current menu

        public static void LoadOnce()
        {

            MainCamera = GameObject.Find("Main Camera");
            GUIOBJ1 = new GameObject();
            GUIOBJ2 = new GameObject();
            GUIOBJ2.name = "DomsGuiTemp";
            GUIOBJ1.name = "DomsGuiTemp";
            GUIOBJ1.AddComponent<Canvas>();
            GUIOBJ1.AddComponent<GraphicRaycaster>();

            Canvas canvas = GUIOBJ1.GetComponent<Canvas>();
            canvas.enabled = true;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = MainCamera.GetComponent<Camera>();

            RectTransform rectTransform = GUIOBJ1.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(5f, 8f);
            rectTransform.position = MainCamera.transform.position;

            CanvasScaler canvasScaler = GUIOBJ1.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 5f;
            canvasScaler.referencePixelsPerUnit = 1000f;
            canvasScaler.scaleFactor = 1f;

            GUIOBJ2.transform.position = MainCamera.transform.position;
            GUIOBJ1.transform.parent = GUIOBJ2.transform;
            GUIOBJ1.GetComponent<RectTransform>().localPosition = new Vector3(-1.8f, -1.4f, 1.6f);

            Vector3 eulerAngles = GUIOBJ1.GetComponent<RectTransform>().rotation.eulerAngles;
            eulerAngles.y = -270f;
            GUIOBJ1.transform.localScale = Vector3.one;
            GUIOBJ1.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
            System.Collections.Generic.List<string> menuNames = new List<string> { "Main" };

            if (buttons.Length > 0 && buttons[0] != null) // add all the buttons
            {
                foreach (var btn in buttons[0])
                {
                    menuNames.Add(btn.buttonText);
                }
            }
            SectionNames = menuNames.ToArray();
            GuiDisplayText = new GameObject
            {
                transform = { parent = GUIOBJ1.transform }
            }.AddComponent<Text>();
            GuiDisplayText.text = "";
            GuiDisplayText.fontSize = 10;
            GuiDisplayText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            GuiDisplayText.rectTransform.sizeDelta = new Vector2(260f, 400f);
            GuiDisplayText.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            GuiDisplayText.rectTransform.localPosition = new Vector3(-1.3f, 1f, 2f);
            GuiDisplayText.material = new Material(Shader.Find("GUI/Text Shader"));
            GuiDisplayText.alignment = TextAnchor.UpperLeft;
            GuiDisplayText.color = Color.white;
            GUIOBJ2.transform.position = MainCamera.transform.position;
            GUIOBJ2.transform.rotation = MainCamera.transform.rotation;
            cRTEATEmENUS(); // create all the menus when the gui is loaded
            GuiState = "Main";
            CurrentViewingMenu = Sections["Main"];
            UpdateMenuDisplay();
        }


        public static void UpdateGUI()
        {
            if (GUIOBJ2 != null && GUIOBJ2.activeSelf && MainCamera != null) // update the position and rotation
            {
                GUIOBJ2.transform.position = MainCamera.transform.position;
                GUIOBJ2.transform.rotation = MainCamera.transform.rotation;
            }
            UpdateMenuDisplay(); // make sure network info is uptodate maybe make slightly throttled doesnt need to be ran each frame but prob not that big of a deal
            HandleMenuInput();
            if (CurrentViewingMenu != null)
            {
                foreach (var option in CurrentViewingMenu)
                {
                    if (option.isTogglable && option.enabled && option.method != null)
                    {
                        option.method.Invoke();
                    }
                }
            }
        }

        public static void ShowGui()
        {
            if (GUIOBJ2 != null)
            {
                GUIOBJ2.SetActive(true);
            }
            if (GUIOBJ1 != null)
            {
                GUIOBJ1.SetActive(true);
            }
        }

        public static void HideGui()
        {

            if (GUIOBJ2 != null)
            {
                GUIOBJ2.SetActive(false);
            }
            if (GUIOBJ1 != null)
            {
                GUIOBJ1.SetActive(false);
            }
        }

        private static void HandleMenuInput()
        {
            if (CurrentViewingMenu != null)
            {

                bool ThumbStick = EasyInputs.GetThumbStickButtonDown(EasyHand.LeftHand) || EasyInputs.GetThumbStickButtonDown(EasyHand.RightHand);
                bool Grip = EasyInputs.GetGripButtonDown(EasyHand.RightHand) || EasyInputs.GetGripButtonDown(EasyHand.LeftHand);

                int OPtions = CurrentViewingMenu.Length;
                int Pages = (OPtions + OptionsPerPage - 1) / OptionsPerPage;

                if (ThumbStick)
                {
                    bool CanScroll = Time.time - lastScrollTime > scrollCooldown; // make sure you dont scroll really fast and when you click a aoption it doesnt insta click off

                    if (CanScroll && EasyInputs.GetTriggerButtonFloat(EasyHand.RightHand) > 0.5f)
                    {
                        HandleTriggerPress(true, OPtions);
                    }
                    else if (CanScroll && EasyInputs.GetTriggerButtonFloat(EasyHand.LeftHand) > 0.5f)
                    {
                        HandleTriggerPress(false, OPtions);
                    }
                    else if (Grip && canActivate) //make sure you dont spam options
                    {
                        ExecuteSelectedMod();
                        canActivate = false;
                    }
                }

                if (!Grip && !canActivate)
                {
                    canActivate = true;
                }
            }
        }

        private static void HandleTriggerPress(bool isRightTrigger, int totalOptions)
        {
            int Pages = (totalOptions + OptionsPerPage - 1) / OptionsPerPage;
            int CurrtentOptions = Math.Min(OptionsPerPage, totalOptions - CurrentPage * OptionsPerPage);
            if (isRightTrigger) //right trigger
            {
                SelectedOptionIndex++;

                // if at the end wrap to the start
                if (SelectedOptionIndex >= CurrtentOptions)
                {
                    if (CurrentPage < Pages - 1)
                    {
                        CurrentPage++;
                        SelectedOptionIndex = 0;
                    }
                    else
                    {
                        SelectedOptionIndex = CurrtentOptions - 1;
                    }
                }
            }
            else // left trigger
            {
                SelectedOptionIndex--;

                // if at the end wrap to the start
                if (SelectedOptionIndex < 0)
                {
                    if (CurrentPage > 0)
                    {
                        CurrentPage--;
                        SelectedOptionIndex = OptionsPerPage - 1;
                        if (CurrentPage * OptionsPerPage + SelectedOptionIndex >= totalOptions)
                        {
                            SelectedOptionIndex = (totalOptions - 1) % OptionsPerPage;
                        }
                    }
                    else
                    {
                        SelectedOptionIndex = 0;
                    }
                }
            }

            UpdateMenuDisplay();
            lastScrollTime = Time.time;
        }

        private static void ExecuteSelectedMod() 
        {
            int index = CurrentPage * OptionsPerPage + SelectedOptionIndex;
            if (CurrentViewingMenu == null || index >= CurrentViewingMenu.Length) return;

            MenuOption option = CurrentViewingMenu[index];

            if (GuiState == "Main")
            {
                int targetIndex = index + 1;
                if (targetIndex < SectionNames.Length)
                {
                    NavigateToMenu(SectionNames[targetIndex]);
                }
                return;
            }

            if (option.isTogglable)
            {
                option.enabled = !option.enabled;

                if (option.enabled && option.enableMethod != null)
                {
                    option.enableMethod.Invoke();
                    Notifications.Library.SendNotificationTagged("green", "ENABLED", "white", option.toolTip);
                }
                else if (!option.enabled && option.disableMethod != null)
                {
                    option.disableMethod.Invoke();
                    Notifications.Library.SendNotificationTagged("green", "[DISABLED]", "white", option.toolTip);
                }
            }
            else if (option.method != null)
            {
                option.method.Invoke();
                Notifications.Library.SendNotificationTagged("green", "ACTIVATED", "white", option.toolTip);
            }

            UpdateMenuDisplay();
        }

        public static void NavigateToMenu(string menuName) 
        {
            GuiState = menuName;
            SelectedOptionIndex = 0;
            CurrentPage = 0;
            lastScrollTime = Time.time;

            if (Sections.TryGetValue(menuName, out var menu))
            {
                CurrentViewingMenu = menu;
            }
            else
            {
                CurrentViewingMenu = null;
            }

            UpdateMenuDisplay();
        }

        private static void UpdateMenuDisplay()
        {
            int Options = CurrentViewingMenu.Length;
            int Pages = (Options + OptionsPerPage - 1) / OptionsPerPage;
            int Start = CurrentPage * OptionsPerPage;
            int End = Math.Min(Start + OptionsPerPage, Options);

            string menuText = $"<color=#8B0000> Gui Temp : {GuiState} (Page {CurrentPage + 1}/{Pages})</color>\n\n";

            for (int i = Start; i < End; i++)
            {
                MenuOption option = CurrentViewingMenu[i];
                string prefix = (i - Start == SelectedOptionIndex) ? "> " : "  ";
                string status = option.isTogglable ? (option.enabled ? " <color=green>[ON]</color>" : " <color=red>[OFF]</color>") : "";

                string color = GetOptionColor(i - Start == SelectedOptionIndex, option);
                menuText += $"{prefix}<color={color}>{option.buttonText}</color>{status}\n";
            }

            menuText += GuiNetworkInfoStuff();
            GuiDisplayText.text = menuText;
        }

        private static string GetOptionColor(bool isSelected, MenuOption option)
        {
            if (isSelected)
            {
                return GuiRGB ? GetRGBColor() : GuiColor;
            }
            else
            {
                return option.enabled && option.isTogglable ? "lightgreen" : "white";
            }
        }

        private static string GetRGBColor()
        {
            GuiRGBTimer += Time.deltaTime * 2f;
            if (GuiRGBTimer > 1f)
            {
                GuiRGBTimer = 0f;
            }

            Color rgbColor = Color.HSVToRGB(GuiRGBTimer, 1f, 1f);
            return $"#{ColorUtility.ToHtmlStringRGB(rgbColor)}";
        }

        private static string GuiNetworkInfoStuff()
        {
            if (PhotonNetwork.InRoom)
            {
                return $"\n\n\n\nPing: {PhotonNetwork.GetPing()}\n" +
                       $"Room Name: {PhotonNetwork.CurrentRoom.name}\n" +
                       $"Player Count: {PhotonNetwork.CurrentRoom.PlayerCount}\n";
            }

            return $"\n\n\n\nPing: 0\nRoom Name: null\nPlayer Count 0\n";
        }

        private static void cRTEATEmENUS()
        {
            for (int i = 0; i < buttons.Length && i < SectionNames.Length; i++)
            {
                MenuOption[] menuOptions = ConvertButtonsToMenuOptions(buttons[i]);
                SetMenuByIndex(i, menuOptions);
            }

            UpdateMainMenuCounts();
        }

        private static MenuOption[] ConvertButtonsToMenuOptions(ButtonInfo[] buttonInfos)
        {
            MenuOption[] menuOptions = new MenuOption[buttonInfos.Length];

            for (int i = 0; i < buttonInfos.Length; i++)
            {
                ButtonInfo button = buttonInfos[i];
                menuOptions[i] = new MenuOption
                {
                    buttonText = button.buttonText,
                    method = button.method,
                    enableMethod = button.enableMethod,
                    disableMethod = button.disableMethod,
                    isTogglable = button.isTogglable,
                    enabled = button.enabled,
                    toolTip = button.toolTip,
                    overlapText = button.overlapText
                };
            }

            return menuOptions;
        }

        private static void SetMenuByIndex(int index, MenuOption[] menuOptions)
        {
            if (index < SectionNames.Length)
            {
                string menuName = SectionNames[index];
                Sections[menuName] = menuOptions;
            }
        }

        private static void UpdateMainMenuCounts() // put all the sections on the main menu
        {
            if (Sections.ContainsKey("Main"))
            {
                var mainMenu = Sections["Main"];

                for (int i = 1; i < mainMenu.Length && i < buttons.Length; i++)
                {
                    string baseText = GetBaseButtonText(mainMenu[i].buttonText);
                    mainMenu[i].buttonText = $"{baseText}";
                }
            }
        }

        private static string GetBaseButtonText(string buttonText)
        {
            int bracketIndex = buttonText.IndexOf('[');
            return bracketIndex > 0 ? buttonText.Substring(0, bracketIndex).Trim() : buttonText;
        }
    }

    public class MenuOption
    {
        public string buttonText;
        public Action method;
        public Action enableMethod;
        public Action disableMethod;
        public bool isTogglable;
        public bool enabled;
        public string toolTip;
        public string overlapText;
    }
}