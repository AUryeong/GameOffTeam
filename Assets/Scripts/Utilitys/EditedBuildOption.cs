using UnityEngine;

public class EditedBuildOption
{
    static double ApkBuildDate = 2210281048;

    static double IpaBuildDate = 2205290224;

    public static string AndroidVersion = "0.0.8";
    public static int AndroidVersionCode = 8;

    public static string iOSVersion = "0.0.2";
    public static int iOSBuildNumber = 2;

    public static bool showDbgBtn = false;

    static string devIP = "211.178.39.227";

    public static double BuildDate
    {
        get
        {
#if UNITY_ANDROID
            return ApkBuildDate;
#else
            return IpaBuildDate;
#endif
        }
        set
        {
#if UNITY_ANDROID
            ApkBuildDate = value;
#else
            IpaBuildDate = value;
#endif
        }
    }

    public static bool IsDeveloper 
    {
        get
        {
#if UNITY_EDITOR
            return true;
#endif
            //if (EditedBuildOption.showDbgBtn || PlayerPrefs.HasKey("sb_dbg") )
            //    return true;

            //if(LMJ_Utill._myExternalIP == devIP)
            //    return true;

            //if(UserInfoMgr.UserInfo != null && UserInfoMgr.AccountInfo.is_developer == true)
            //    return true;

            return true; ;
        }
    }
} 
