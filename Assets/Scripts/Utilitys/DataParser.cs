using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;


public class StatsDataConverter
{
    public string ID;
    public string Icon;

    public float Hp;
    public int Type;
    public float Damage;

    public int Item1;
    public float Item1_P;

    public int Item2;
    public float Item2_P;

    public int Item3;
    public float Item3_P;

    public int Item4;
    public float Item4_P;

    public int Item5;
    public float Item5_P;

    public StatsDataConverter()
    {

    }


}

public class Passive
{
    public string ID;
    public string Name;
    public string Icon;
    public int Lv_Max;
    public int Base;
    public int Upgrade;
    public string Desc;

    public Passive()
    {

    }


}

public class Active
{
    public string ID;
    public string Name;
    public string Icon;
    public int Lv_Max;
    public string Desc;

    public Active()
    {

    }
}

public class SkillStats
{
    public string ID;
    public float Damage;
    public int Second_Active;
    public float Second_Damage;
    public float Attackspeed;
    public float Cooltime;
    public float Radius;
    public int StatusEffect;
    public int Projectile;
    public float Projectile_Speed;
    public float Projectile_Size;
    public float Knockback;
    public float Spread;
    public int Pierce;
    public int Jump;
    public float DOT;
}

public class WaveData
{
    public int ID;
    public int Stage;
    public float TimeSec;
    public string Monster_1;
    public int Count_1;
    public string Monster_2;
    public int Count_2;
    public string Monster_3;
    public int Count_3;
    public string Monster_4;
    public int Count_4;

    public WaveData()
    {

    }

}

public class StageData
{
    public int ID;
    public int Stage;
    public string Stage_Name;

    public StageData()
    {

    }


}

public class StatusEffectData
{
    public int Id;
    public string Name;
    public float Status;
    public int Duration;
}


public static class DataParser
{
    #region Constants
    private static readonly string WAVE_FILE_PATH = "CSV/Stages/";

    private static readonly string ID = "ID";
    private static readonly string Monster_ID = "MonsterID";
    private static readonly string TimeSec_ID = "TimeSec";

    #endregion


    public static void GetLoadDatas<T>(string name, System.Action<bool,List<T>> onComplete)
    {
        string json = Global.Load(name);
        List<T> data;
        if (json != null)
        {
            data = JsonConvert.DeserializeObject<List<T>>(json);
            onComplete(true, data);
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog(name, "게임 데이터가 없네요.~ 'Tools/Data/DownLoad All' Click " + name, "Got It!");
#endif
        }
    }
}

