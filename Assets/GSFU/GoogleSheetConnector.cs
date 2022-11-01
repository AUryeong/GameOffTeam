using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif


//Google script 계정이 google sheet 접근 권한이 있거나, 전체 공유이어야 한다.
//https://script.google.com/d/1h5td4o0uQbdBcQCDrUZpFVUXhs2FJr4r6L65R-F6XABU7u_QAcN9-iME/edit

public class GoogleSheetConnector : MonoSingleton<GoogleSheetConnector>
{
#if UNITY_EDITOR

	[MenuItem("Tools/GetGoogleSheetData/DownLoad All", false, 1)]
	public static void DownLoadAllInfo()
	{
		GoogleSheetConnectorCore.DownLoadAllInfo(null);
	}

	static int updateing = 0;


	[MenuItem("Tools/GetGoogleSheetData/Get ActiveSkillDatas ", false, 1)]
	public static void GetActiveSkillDatas()
	{
		updateing++;
		GoogleSheetConnectorCore.GetActiveSkills((issUCC) =>
		{
			updateing--;
			if (updateing == 0)
			{
				Debug.Log("Download complete!");
				if (Application.isPlaying)
					LmjEventManager.dispatchEvent("LoadTableComplete");
#if UNITY_EDITOR
				AssetDatabase.Refresh();
#endif
			}
		});
	}


#endif


}