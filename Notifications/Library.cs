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

        private static float lastNotificationTime;
        private static int lineCount;
        private static Text notificationDisplay;

        private static GameObject container;
        private static GameObject canvasObject;
        private static Camera MainCamera;
        private static List<Notification> activeNotifications = new List<Notification>();

        private class Notification
        {
            public string Text;
            public float TimeAdded;
        }

        public static void Initialize() // so ahh made in like 10 mins also nw ahhh
        {
            MainCamera = Camera.main;
            if (MainCamera == null) return;

            container = new GameObject("DOMSNOTIFICATIONSCONTAINER")
            {
                transform = { position = MainCamera.transform.position }
            };

            canvasObject = new GameObject("DOMSNOTIFICATIONSCANVAS");
            canvasObject.transform.SetParent(container.transform);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = MainCamera;

            var canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 2f;
            canvasScaler.referencePixelsPerUnit = 2000f;
            canvasScaler.scaleFactor = 1f;

            canvasObject.AddComponent<GraphicRaycaster>();

            var rect = canvasObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(5f, 5f);
            rect.localPosition = new Vector3(0f, 0f, 1.6f);
            rect.localScale = Vector3.one;
            rect.rotation = Quaternion.Euler(0f, -270f, 0f);

            var textObj = new GameObject("NotificationText");
            textObj.transform.SetParent(canvasObject.transform);
            notificationDisplay = textObj.AddComponent<Text>();
            notificationDisplay.fontSize = 8;
            notificationDisplay.rectTransform.sizeDelta = new Vector2(260f, 160f);
            notificationDisplay.rectTransform.localScale = new Vector3(0.015f, 0.015f, 1.5f);
            notificationDisplay.rectTransform.localPosition = new Vector3(-5f, -1.5f, 0f);
            notificationDisplay.material = new Material(Shader.Find("GUI/Text Shader"));
            notificationDisplay.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            notificationDisplay.color = Color.white;
            notificationDisplay.alignment = TextAnchor.UpperLeft;
        }

        public static void Update() // uh pretty much update pos and remove lines
        {
            if (notificationDisplay == null) return;
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
                lineCount = activeNotifications.Count;
            }
        }

        public static void SendNotificationTagged(string tagcolor, string tag, string textcolor = "white", string text = "A Empty Notification Was Sent, Menu Owner Do Better!") // send a notification like [TAG] notification
        {
            if (!Enabled || notificationDisplay == null || Time.time - lastNotificationTime < Cooldown) return;
            lastNotificationTime = Time.time;
            string FormattedText = $"<color={tagcolor}>{tag}</color> <color={textcolor}>{text}</color>";
            activeNotifications.Add(new Notification { Text = FormattedText, TimeAdded = Time.time });
            notificationDisplay.text += FormattedText + "\n";
        }

        public static void SendNotification(string textcolor = "white", string text = "A Empty Notification Was Sent, Menu Owner Do Better!") // send a notification like notification
        {
            if (!Enabled || notificationDisplay == null || Time.time - lastNotificationTime < Cooldown) return;
            lastNotificationTime = Time.time;
            string FormattedText = $"<color={textcolor}>{text}</color>";
            activeNotifications.Add(new Notification { Text = FormattedText, TimeAdded = Time.time });
            notificationDisplay.text += FormattedText + "\n";
        }

        public static void Clear()
        {
            if (!Enabled || notificationDisplay == null) return;
            notificationDisplay.text = string.Empty;
            activeNotifications.Clear();
            lineCount = 0;
        }
    }
}