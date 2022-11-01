using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    static public FPSCounter Inst;
    float dtime;
    int counting;
    int fps = 0;
    string strFPS;

    [SerializeField]
    private Text numLivesText = null;

    void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        Inst = null;
    }

    void Start()
    {
        dtime = Time.timeSinceLevelLoad;
        counting = 0;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - dtime <= 1)
        {
            counting++;
        }
        else
        {
            fps = counting + 1;
            strFPS = "FPS[" + fps + "]";
            dtime = Time.timeSinceLevelLoad;
            counting = 0;

            if (numLivesText)
                numLivesText.text = strFPS;
        }
    }

    void OnGUI()
    {
        
        //GUILayout.Label(strFPS, GUILayout.Width(110));
    }
}
