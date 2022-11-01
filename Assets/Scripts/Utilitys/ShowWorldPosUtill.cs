using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWorldPosUtill : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    [ContextMenu("Update")]
    void Update()
    {
        Position = transform.position;
        Angle = transform.rotation.eulerAngles;
    }
}
