using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoSingleton<Global>
{
    //constData
    public readonly int ConData = 4;


    //staticData
    private List<Active> activeSkillDatas;



    protected override void Awake()
    {
        base.Awake();

        //json파일로 읽어오는형식
        DataParser.GetLoadDatas<Active>("Json/Active", (isSucc, _data) =>
        {
            activeSkillDatas = _data;
        });

    }

  
    static public string Load(string name)
    {
        var path = Resources.Load<TextAsset>("Data/" + name);
        return path.ToString();
    }

}
