using UnityEngine;


public static class ExtendPlayerPrefs
{
    //[MenuItem("ADN/Player Prefs/Delete All Keys", priority = 520)]
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    //[MenuItem("ADN/Project Prefs/Player Prefs/Save Keys", priority = 530)]
    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    // float
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    // int
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void IncInt(string key)
    {
        PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0)+1 );
    }
    public static void DecInt(string key)
    {
        PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0) + 1);
    }

    // bool
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetString(key, System.Convert.ToString(value));
    }
    public static bool GetBool(string key, bool defaultValue)
    {
        string boolean = PlayerPrefs.GetString(key, System.Convert.ToString(defaultValue));

        return System.Convert.ToBoolean(boolean);
    }

    // String
    public static void SetString(string key, string str)
    {
        PlayerPrefs.SetString(key, str);
    }
    public static string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    // String UTF-8
    public static void SetStringUTF8(string key, string str)
    {
        //base64로 인코딩
        str = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
        PlayerPrefs.SetString(key, str);
    }
    public static string GetStringUTF8(string key)
    {
        //base64로 디코딩
        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(PlayerPrefs.GetString(key)));
    }
    public static string GetStringUTF8(string key, string defaultValue)
    {
        // 주의: 이미 저장된값이 일반문자열이라면 Format익셉션 발생!!!
        defaultValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(defaultValue));
        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(PlayerPrefs.GetString(key, defaultValue)));
    }

    // Rect
    public static void SetRect(string key, Rect rect)
    {
        PlayerPrefs.SetFloat(key + "_Rect.x", rect.x);
        PlayerPrefs.SetFloat(key + "_Rect.y", rect.y);
        PlayerPrefs.SetFloat(key + "_Rect.width", rect.width);
        PlayerPrefs.SetFloat(key + "_Rect.height", rect.height);
    }
    public static Rect GetRect(string key, Rect defaultValue)
    {
        Rect rect = new Rect();

        rect.x = PlayerPrefs.GetFloat(key + "_Rect.x", defaultValue.x);
        rect.y = PlayerPrefs.GetFloat(key + "_Rect.y", defaultValue.y);
        rect.width = PlayerPrefs.GetFloat(key + "_Rect.width", defaultValue.width);
        rect.height = PlayerPrefs.GetFloat(key + "_Rect.height", defaultValue.height);

        return rect;
    }

    // Vector 2
    public static void SetVector2(string key, Vector2 vect)
    {
        PlayerPrefs.SetFloat(key + "_Vector2.x", vect.x);
        PlayerPrefs.SetFloat(key + "_Vector2.y", vect.y);
    }
    public static Vector2 GetVector2(string key, Vector2 defaultValue)
    {
        Vector2 vect = new Vector2();

        vect.x = PlayerPrefs.GetFloat(key + "_Vector2.x", defaultValue.x);
        vect.y = PlayerPrefs.GetFloat(key + "_Vector2.y", defaultValue.y);

        return vect;
    }

    // Vector 3
    public static void SetVector3(string key, Vector3 vect)
    {
        PlayerPrefs.SetFloat(key + "_Vector3.x", vect.x);
        PlayerPrefs.SetFloat(key + "_Vector3.y", vect.y);
        PlayerPrefs.SetFloat(key + "_Vector3.z", vect.z);
    }
    public static Vector3 GetVector3(string key, Vector3 defaultValue)
    {
        Vector3 vect = new Vector3();

        vect.x = PlayerPrefs.GetFloat(key + "_Vector3.x", defaultValue.x);
        vect.y = PlayerPrefs.GetFloat(key + "_Vector3.y", defaultValue.y);
        vect.z = PlayerPrefs.GetFloat(key + "_Vector3.z", defaultValue.z);

        return vect;
    }

    // Color
    public static void SetColor(string key, Color color)
    {
        PlayerPrefs.SetFloat(key + "_Color.a", color.a);
        PlayerPrefs.SetFloat(key + "_Color.r", color.r);
        PlayerPrefs.SetFloat(key + "_Color.g", color.g);
        PlayerPrefs.SetFloat(key + "_Color.b", color.b);
    }
    public static Color GetColor(string key, Color defaultValue)
    {
        var color = new Color();

        color.a = PlayerPrefs.GetFloat(key + "_Color.a", defaultValue.a);
        color.r = PlayerPrefs.GetFloat(key + "_Color.r", defaultValue.r);
        color.g = PlayerPrefs.GetFloat(key + "_Color.g", defaultValue.g);
        color.b = PlayerPrefs.GetFloat(key + "_Color.b", defaultValue.b);

        return color;
    }
}