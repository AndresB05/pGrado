using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

public static class LevelHUDBuilder
{
    public static void CreateHUD(int levelIndex)
    {
        var old = GameObject.Find("Canvas");
        if (old != null) Object.DestroyImmediate(old);

        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        var panel = new GameObject("TopPanel");
        panel.transform.SetParent(canvasGO.transform, false);
        var rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0,1); rt.anchorMax = new Vector2(1,1);
        rt.pivot = new Vector2(0.5f,1); rt.sizeDelta = new Vector2(0,60);
        rt.anchoredPosition = Vector2.zero;
        panel.AddComponent<Image>().color = new Color(0,0,0,0.55f);

        MakeLabel(panel, "LevelLabel", "Nivel "+levelIndex, new Vector2(0,0), new Vector2(0.33f,1), font);
        MakeButton(panel, "BtnCenital", "Vista Cenital", new Vector2(0.33f,0), new Vector2(0.66f,1), font);
        MakeButton(panel, "BtnMenu", "Menu", new Vector2(0.66f,0), new Vector2(1f,1), font);

        var hud = canvasGO.AddComponent<LevelHUDRuntime>();
        hud.levelIndex = levelIndex;
    }

    static void MakeLabel(GameObject p, string n, string txt, Vector2 aMin, Vector2 aMax, Font font)
    {
        var go = new GameObject(n); go.transform.SetParent(p.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = new Vector2(8,0); rt.offsetMax = Vector2.zero;
        var t = go.AddComponent<Text>();
        t.text = txt; t.font = font; t.fontSize = 22;
        t.color = Color.white; t.alignment = TextAnchor.MiddleLeft;
    }

    static void MakeButton(GameObject p, string n, string lbl, Vector2 aMin, Vector2 aMax, Font font)
    {
        var go = new GameObject(n); go.transform.SetParent(p.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = new Vector2(4,4); rt.offsetMax = new Vector2(-4,-4);
        var img = go.AddComponent<Image>(); img.color = new Color(0.15f,0.15f,0.15f,0.85f);
        go.AddComponent<Button>().targetGraphic = img;
        var txtGO = new GameObject("Text"); txtGO.transform.SetParent(go.transform, false);
        var trt = txtGO.AddComponent<RectTransform>();
        trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
        trt.offsetMin = Vector2.zero; trt.offsetMax = Vector2.zero;
        var t = txtGO.AddComponent<Text>();
        t.text = lbl; t.font = font; t.fontSize = 18;
        t.color = Color.white; t.alignment = TextAnchor.MiddleCenter;
    }
}
#endif