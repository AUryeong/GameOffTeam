using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DGSafeAreaUI : MonoBehaviour
{
    static public float cutoutLocalSize = 0;

    static public void Init()
    {
        _cutoutWorldSize = 0;
    }

    static float _cutoutWorldSize = 0;
    static public float cutoutWorldSize
    {
        get
        {
            if (cutoutLocalSize == 0)
                return 0;

            if (_cutoutWorldSize == 0)
            {
                var pos1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
                var pos2 = Camera.main.ScreenToWorldPoint(new Vector3(0, cutoutLocalSize, 0));
                _cutoutWorldSize = Mathf.Abs(pos1.y - pos2.y) ;
            }
            return _cutoutWorldSize;
        }
    }
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        //Rect safeArea = Screen.safeArea;
        //if (rt != null && safeArea.y != 0F)
        //{
        //    var anchorMax = safeArea.position + safeArea.size;
        //    anchorMax.x /= Screen.width;
        //    anchorMax.y /= Screen.height;
        //    rt.anchorMax = anchorMax;

        //    var anchorMin = safeArea.position;
        //    anchorMin.x /= Screen.width;
        //    anchorMin.y /= Screen.height;
        //    rt.anchorMin = anchorMin;
        //}

        LMJ.Log("safeArea " + Screen.safeArea.position.ToString());
        LMJ.Log("safeArea " + Screen.safeArea.size.ToString());


        RectTransform canvasTR = (RectTransform)GetComponentInParent<Canvas>().transform;
        int screenHeight = Screen.height;
        float safeYMax = Screen.safeArea.yMax;

#if UNITY_EDITOR
        if(screenHeight == 2340 )
            safeYMax = 2264;//redmi 8 pro
#endif

        //LG v50
        //screenHeight = 2340;
        //safeYMax = 2273;

        LMJ.LogFormat("safeYMax{0}  screenHeight{1}", safeYMax, screenHeight);
        if (safeYMax < screenHeight - 1) // 1: a small threshold
        {
            cutoutLocalSize = (screenHeight - safeYMax) ;
            rt.anchoredPosition = new Vector2(0f, -cutoutLocalSize/2f);
            rt.sizeDelta = new Vector2(0f, -cutoutLocalSize);
        }
        else
        {
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
        }
    }
}
