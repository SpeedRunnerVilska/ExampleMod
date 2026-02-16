using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[BepInPlugin("com.yourname.randomcolorbutton", "Random Color Button", "1.0.1")]
public class RandomColorButton : BaseUnityPlugin
{
    private static ManualLogSource Log;
    private Image buttonImage;

    private void Start()
    {
        Log = Logger;
        Log.LogInfo("Random Color Button loaded.");

        CreateUI();
    }

    private void CreateUI()
    {
        // Create EventSystem if it doesn't exist
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Create Canvas
        GameObject canvasObj = new GameObject("ModCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // force on top

        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Button
        GameObject buttonObj = new GameObject("ModButton");
        buttonObj.transform.SetParent(canvasObj.transform, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;

        buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = Color.red;

        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        button.onClick.AddListener(OnButtonClick);

        // Text
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = "Click Me";
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private void OnButtonClick()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        buttonImage.color = randomColor;

        Log.LogInfo("Button clicked, new color: " + randomColor);
    }
}
