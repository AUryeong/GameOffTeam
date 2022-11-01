using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoSingle<CameraController>
{

    public enum UpdateType
    {
        FixedUpdate,
        LateUpdate,
        ManualUpdate,
    }


    [Space(10.0f)]
    public Transform target;

    public float offsetY = 3.0f;
    public float CameraDistance = -5.0f;


    [Header("Screen Shake Variables")]
    public float shakeAmount = 0.2f;
    float shakeTime;
    Vector3 initpos;


    [SerializeField] private UpdateType m_UpdateType;

    [SerializeField] private float m_MoveSpeed = 3;
    [SerializeField] private bool m_FollowTilt = true;
    [SerializeField] private float m_SpinTurnLimit = 90;
    private float m_LastFlatAngle;
    private float m_CurrentTurnAmount;
    private float m_TurnSpeedVelocityChange;
    private Vector3 m_RollUp = Vector3.up;

    [Space(10.0f)]
    [SerializeField] private UnityEngine.UI.Image flashPanel = null;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (target == null)
            Debug.LogError("ERROR  : CameraTarget Null !! ");
    }


    private void LateUpdate()
    {
        if (m_UpdateType == UpdateType.LateUpdate)
        {
            ShakeCheckTime();
            FollowTarget(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (m_UpdateType == UpdateType.FixedUpdate)
        {
            ShakeCheckTime();
            FollowTarget(Time.deltaTime);
        }
    }
    public void ManualUpdate()
    {
        if (m_UpdateType == UpdateType.ManualUpdate)
        {
            ShakeCheckTime();
            FollowTarget(Time.deltaTime);
        }
    }

    void FollowTarget(float deltaTime)
    {
        if (!(deltaTime > 0) || target == null)
        {
            return;
        }

        var targetForward = target.forward;
        var targetUp = target.up;

        {
            var currentFlatAngle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;
            if (m_SpinTurnLimit > 0)
            {
                var targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(m_LastFlatAngle, currentFlatAngle)) / deltaTime;
                var desiredTurnAmount = Mathf.InverseLerp(m_SpinTurnLimit, m_SpinTurnLimit * 0.75f, targetSpinSpeed);
                var turnReactSpeed = (m_CurrentTurnAmount > desiredTurnAmount ? .1f : 1f);
                if (Application.isPlaying)
                {
                    m_CurrentTurnAmount = Mathf.SmoothDamp(m_CurrentTurnAmount, desiredTurnAmount,
                                                         ref m_TurnSpeedVelocityChange, turnReactSpeed);
                }
                else
                {
                    m_CurrentTurnAmount = desiredTurnAmount;
                }
            }
            else
            {
                m_CurrentTurnAmount = 1;
            }
            m_LastFlatAngle = currentFlatAngle;
        }


        Vector3 _target;
        if (Screen.width > Screen.height) //가로화면
        {
            _target = new Vector3(target.position.x, target.transform.position.y + offsetY/1.5f, target.transform.position.z + CameraDistance/1.5f);
        }
        else //세로화면
        {
            _target = new Vector3(target.position.x, target.transform.position.y + offsetY, target.transform.position.z + CameraDistance);
        }

        transform.position = Vector3.Lerp(transform.position, _target, deltaTime * m_MoveSpeed);


        if (!m_FollowTilt)
        {
            targetForward.y = 0;
            if (targetForward.sqrMagnitude < float.Epsilon)
            {
                targetForward = transform.forward;
            }
        }
    }

    public void Shake(float time)
    {
        if (shakeTime > 0)
            return;
        initpos = transform.position;
        shakeTime = time;
    }

    void ShakeCheckTime()
    {
        return;
        if (shakeTime > 0)
        {
            transform.localPosition = Random.insideUnitSphere * shakeAmount + initpos;
            shakeTime -= Time.unscaledDeltaTime;
        }
    }

    public void SetSightRange(float _value) 
    {
        offsetY = offsetY + offsetY * (_value / 100.0f);
        CameraDistance = CameraDistance + CameraDistance * (_value / 100.0f);

    }

    public void StartFlash()
    {
        StartCoroutine(goBliend());
    }

    private IEnumerator goBliend()
    {
        flashPanel.gameObject.SetActive(true);
        Color c = new Color(1, 1, 1, 1);
        int duration = 255;
        flashPanel.color = c;
        while (c.a > 0.15f  )
        {
            duration--;
            c.a = duration / 255.0f;
            flashPanel.color = c;
            yield return new WaitForSecondsRealtime(0.01f);

        }
        flashPanel.gameObject.SetActive(false);
    }
}
