using UnityEngine;
using System.Collections;

public class GameSecTimer : MonoSingleton<GameSecTimer>
{
    bool isstartSecTimer = false;
    public void startSecTimer()
    {
        if (isstartSecTimer == true)
            return;
        isstartSecTimer = true;
        StartCoroutine("OneSecTimer");
    }

    IEnumerator OneSecTimer()
    {
        float unScaled = 0f;
        float scaled = 0f;
        while (true)
        {
            unScaled += Time.unscaledDeltaTime;
            scaled += Time.deltaTime;

            if (scaled > 1f)
            {
                int nSec = (int)scaled;
                LmjEventManager.dispatchEvent("SecTimer", new ZEvent(nSec));
                scaled -= nSec;
                //ADN.Log("SecTimer " + nSec);
            }

            if (unScaled > 1f)
            {
                int nSec = (int)unScaled;
                LmjEventManager.dispatchEvent("SecTimerUnScaled", new ZEvent(nSec));
                unScaled -= nSec;
                //ADN.Log("SecTimerUnScaled " + nSec);
            }
            yield return null;
            //EventManager.dispatchEvent("SecTimer", new ZEvent(1));
            //yield return new WaitForSeconds(1f);
        }
    }

    float _sleepTime = -1 ;
    void OnApplicationPause(bool isPause)
    {
        if (isPause == false)
        {
            if (_sleepTime != -1)
            {
                int delta = (int)(Time.realtimeSinceStartup - _sleepTime);
                LmjEventManager.dispatchEvent("SecTimer", new ZEvent(delta));
                LmjEventManager.dispatchEvent("SecTimerUnScaled", new ZEvent(delta));

                _sleepTime = -1;
                LMJ.Log("OnApplicationAwake : " + delta);
            }

            LmjEventManager.dispatchEvent("ApplicationActive");
        }
        else
        {
            _sleepTime = Time.realtimeSinceStartup;
            LmjEventManager.dispatchEvent("ApplicationPause");
        }
    }
}