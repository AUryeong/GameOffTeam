using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Raycaster : MonoSingle<UI_Raycaster>
{
    private GraphicRaycaster gr;

    override protected void Awake()
    {
        base.Awake();
        gr = GetComponent<GraphicRaycaster>();
    }

    public List<RaycastResult> GetRacastResult()
    {
        return GetRacastResult(Input.mousePosition);
    }

    public List<RaycastResult> GetRacastResult(Vector3 pos)
    {
        var ped = new PointerEventData(null);
        ped.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        return results;
    }
}