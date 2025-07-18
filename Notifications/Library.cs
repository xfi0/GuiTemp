using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GuiTemp.Notifications
{
    internal class Library
    {
        public static float Cooldown { get; set; } = 0.5f;
        public static bool Enabled { get; set; } = true;
        public static int MaxLineCount { get; set; } = 110;
        public static int LineGap { get; set; } = 5;
        public static float FadeTime { get; set; } = 3f;
        public static bool ShowTime { get; set; } = false;

        public static float LastNotifTme;
        public static int LineCount;
        public static Text notificationDisplay;
        
        public static GameObject container;
        public static GameObject canvasObject;
        public static Camera MainCamera;
        public static List<Notifications> activeNotifications = new List<Notifications>();

        public class Notifications
        {
            public string Text;
            public float TimeAdded;
        }

        public static void Initialize()
        {
            MainCamera = Camera.main;
            if (MainCamera != null)
            {
                container = new GameObject("DOMSNOTIFICATIONSCONTAINER")
                {
                    transform = { position = MainCamera.transform.position }
                };

                canvasObject = new GameObject("DOMSNOTIFICATIONSCANVAS");
                canvasObject.transform.SetParent(container.transform);

                Canvas canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = MainCamera;

                CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
                canvasScaler.dynamicPixelsPerUnit = 2f;
                canvasScaler.referencePixelsPerUnit = 2000f; //increase this to have like higher quality text
                canvasScaler.scaleFactor = 1f;

                canvasObject.AddComponent<GraphicRaycaster>();

                RectTransform rect = canvasObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(5f, 5f);
                rect.localPosition = new Vector3(0f, 0f, 1.6f);
                rect.localScale = Vector3.one;
                rect.rotation = Quaternion.Euler(0f, -270f, 0f);

                GameObject Text = new GameObject("NotificationText");
                Text.transform.SetParent(canvasObject.transform);
                notificationDisplay = Text.AddComponent<Text>();
                notificationDisplay.fontSize = 8;
                notificationDisplay.rectTransform.sizeDelta = new Vector2(260f, 160f);
                notificationDisplay.rectTransform.localScale = new Vector3(0.015f, 0.015f, 1.5f);
                notificationDisplay.rectTransform.localPosition = new Vector3(-5f, -1.5f, 0f);
                notificationDisplay.material = new Material(Shader.Find("GUI/Text Shader"));
                notificationDisplay.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                notificationDisplay.color = Color.white;
                notificationDisplay.alignment = TextAnchor.UpperLeft;
            }
        }

        public static void Update()
        {
            if (notificationDisplay != null)
            {
                container.transform.position = MainCamera.transform.position;
                container.transform.rotation = MainCamera.transform.rotation;

                bool needsUpdate = false;
                for (int i = activeNotifications.Count - 1; i >= 0; i--)
                {
                    var notif = activeNotifications[i];
                    float timeAlive = Time.time - notif.TimeAdded;

                    if (timeAlive > FadeTime)
                    {
                        activeNotifications.RemoveAt(i);
                        needsUpdate = true;
                    }
                }

                if (needsUpdate || activeNotifications.Count > 0)
                {
                    notificationDisplay.text = string.Join("\n", activeNotifications.Select(n => n.Text));
                    LineCount = activeNotifications.Count;
                }
            }
        }

        public static void SendNotificationTagged(string tagcolor, string tag, string text = "A Empty Notification Was Sent, Menu Owner Do Better!")
        {
            if (Enabled && notificationDisplay != null && Time.time - LastNotifTme >= Cooldown)
            {
                LastNotifTme = Time.time;
                string FormattedText = $"<color={tagcolor}>{tag}</color> <color=white>{text}</color>";
                activeNotifications.Add(new Notifications { Text = FormattedText, TimeAdded = Time.time });
                notificationDisplay.text += FormattedText + "\n";
            }
        }

        public static void SendNotification(string text = "A Empty Notification Was Sent, Menu Owner Do Better!")
        {
            if (Enabled && notificationDisplay != null && Time.time - LastNotifTme >= Cooldown)
            {
                LastNotifTme = Time.time;
                string FormattedText = $"<color=white>{text}</color>";
                activeNotifications.Add(new Notifications { Text = FormattedText, TimeAdded = Time.time });
                notificationDisplay.text += FormattedText + "\n";
            }
        }

        public static void Clear()
        {
            if (Enabled && notificationDisplay != null)
            {
                notificationDisplay.text = string.Empty;
                activeNotifications.Clear();
                LineCount = 0;
            }
        }
    }
}