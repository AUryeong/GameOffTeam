using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackKeyManager : MonoSingleton<BackKeyManager>
{
    bool m_isEnable = true;

    public enum BackKeyAction
    {
        // 위에 있을 수록 우선순위가 높다.        
        ClosePopup,
        BackToLobby,
        ShowPopup_ExitGame,
    }

    SortedDictionary<BackKeyAction, List<System.Action>> BackKeyActionList = new SortedDictionary<BackKeyAction, List<System.Action>>();


    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        //Subscribe(BackKeyAction.ShowPopup_ExitGame, ShowExitPopup);
    }

    void OnDestory()
    {
        BackKeyActionList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_IOS
        return;
#endif

        if (m_isEnable == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                System.Action func = GetFirstAction();
                if (func != null)
                {
                    LMJ.Log("Back key : " + func.Method.ToString());
                    func();
                }
            }
        }
    }

    static public void Subscribe(BackKeyAction action, System.Action func)
    {
#if UNITY_IOS
        return;
#endif
        if (instance == null)
            return;

        SortedDictionary<BackKeyAction, List<System.Action>> BackKeyActionList = instance.BackKeyActionList;

        if (BackKeyActionList.ContainsKey(action) == false)
        {
            BackKeyActionList.Add(action, new List<System.Action>());
        }

        if (BackKeyActionList[action].Contains(func) == false)
        {
            BackKeyActionList[action].Add(func);
        }
        else
            LMJ.Log("[BackKeyManager] 어? 이미 등록되어 있는데 또 등록해??");
    }

    static public void Unsubscribe(BackKeyAction action, System.Action func)
    {
#if UNITY_IOS
        return;
#endif
        if (instance == null)
            return;

        SortedDictionary<BackKeyAction, List<System.Action>> BackKeyActionList = instance.BackKeyActionList;

        if (BackKeyActionList.ContainsKey(action) == false)
            return;

        if (BackKeyActionList[action].Contains(func))
            BackKeyActionList[action].Remove(func);

    }


    //씬 로딩시 다날리고 초기화
    static public void Reset()
    {
#if UNITY_IOS
        return;
#endif
        instance.BackKeyActionList.Clear();
        //Subscribe(BackKeyAction.ShowPopup_ExitGame, instance.ShowExitPopup);
    }

    System.Action GetFirstAction()
    {
        foreach (KeyValuePair<BackKeyAction, List<System.Action>> pair in BackKeyActionList)
        {
            List<System.Action> lst = pair.Value;

            if (lst.Count <= 0)
                continue;

            return lst[lst.Count - 1];
        }

        return null;
    }

    public static bool isStop()
    {
        return !instance.m_isEnable;
    }

    public static void Stop()
    {
        instance.m_isEnable = false;
    }

    public static void Resume()
    {
        instance.m_isEnable = true;
    }
}