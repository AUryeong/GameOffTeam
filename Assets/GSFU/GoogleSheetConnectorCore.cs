using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.IO;


//Google script 계정이 google sheet 접근 권한이 있거나, 전체 공유이어야 한다.
//https://script.google.com/home/projects/1qOQ2PfECSHXoU_ss30aUIyD2fxdrDzsb-bD3U56vVM7AHsGXccr1rqZb/edit
public class GoogleSheetConnectorCore : MonoSingleton<GoogleSheetConnectorCore>
{
	const string dataSheet = "11yekdgvLTY7ofLZf15Dbk-iY8WqAW744LAfUzo3MIJE"; //문서 ID 
	const string webServiceUrl = "https://script.google.com/macros/s/AKfycby9Kqx7UkLF1K_wM2lTkgwgD6mrK9PJ43RIHv7ETyKSl5hVoLIj6RYPCxShsDxPzU4f/exec";
	public string password = "UnityDataConnectorQuant";
	public float maxWaitTime = 10f;

	public bool debugMode;

	public new static GoogleSheetConnectorCore Get()
	{
		if (Application.isPlaying == false) //GoogleSheetConnectorCore 는 픞레이 중이 안인 경우에도 실행가능해야함
		{
			if (!obj)
			{
				obj = (GoogleSheetConnectorCore)FindObjectOfType(typeof(GoogleSheetConnectorCore));
				if (!obj)
				{
					obj = new GameObject(typeof(GoogleSheetConnectorCore).Name).AddComponent<GoogleSheetConnectorCore>();
					obj.Initialize();
				}
				else
					obj.Initialize();
			}
			return obj;
		}

		return MonoSingleton<GoogleSheetConnectorCore>.Get();
	}


	public static void DownLoadAllInfo(System.Action onComplete)
	{
		LmjEventManager.addEventListener("LoadTableComplete", (e) =>
		{
			LmjEventManager.removeEventListener(e);
			if(onComplete != null)
				onComplete();
		});

		GetActiveSkills(null);
	}

	 
	public static string BaseSavePath()
	{
#if UNITY_EDITOR
		return "/Resources/Data";
#else
		return Application.streamingAssetsPath ;
#endif
	}

	static int updateing = 0;

	public static void GetActiveSkills(System.Action<bool> onComplete)
	{
		updateing++;
		GoogleSheetConnectorCore.Get().GetRowsKeyValue(dataSheet, "Active", 3, -1, (isSucc, strData) => //@@@@@@@@@@@@@@@@@@@@@@@ 3 변수명 라인  -1count 변수 의미 
		{
			updateing--;
			if (updateing == 0)
			{
				Debug.Log("Download complete!");
				if (Application.isPlaying)
					LmjEventManager.dispatchEvent("LoadTableComplete");
			}
			if (isSucc)
			{

				List<Active> datas = JsonConvert.DeserializeObject<List<Active>>(strData);
				if (datas == null)
					return;
				string str = JsonConvert.SerializeObject(datas, Formatting.Indented);

				LMJ_Utill.SaveText(string.Format(BaseSavePath() + "/Json/{0}.json", "Active"), str);

				if (onComplete != null)
					onComplete(true);
			}
			else
			{
				Debug.LogError("Fishs load error!");
				if (onComplete != null)
					onComplete(false);
			}
		});
	}



	void Logger(string str)
	{
		LMJ.Log(str);
	}

	public void GetRowsKeyValue(string sheetID, string sheetName, int start, int count, System.Action<bool, string> onRecv)
	{
		string action = "GetRows";
		string connectionString = webServiceUrl +
									"?ssid=" + sheetID +
									"&sheet=" + sheetName +
									"&pass=" + password +
									"&action=" + action +
									"&start=" + start +
									"&count=" + count;

		DownLoadText(connectionString, null, (isSucc, jsnStr) =>
		{
			if (onRecv != null)
				onRecv(isSucc, jsnStr);
			Logger(jsnStr);

		});
	}

	public void GetKeyValue(string sheetID, string sheetName, int start, int count, System.Action<bool, string> onRecv)
	{
		string action = "GetKeyValue";
		string connectionString = webServiceUrl +
									"?ssid=" + sheetID+
									"&sheet=" + sheetName +
									"&pass=" + password +
									"&action=" + action +
									"&start=" + start +
									"&count=" + count;

		DownLoadText(connectionString, null, (isSucc, jsnStr) =>
		{
			if (onRecv != null)
				onRecv(isSucc, jsnStr);
			Logger(jsnStr);

		});
	}


	public void GetRow(string sheetID, string sheetName, int row, int col_cnt, System.Action<bool, string> onRecv)
	{
		string action = "GetRow";
		string connectionString = webServiceUrl +
									"?ssid=" + sheetID +
									"&sheet=" + sheetName +
									"&pass=" + password +
									"&action=" + action +
									"&col_cnt=" + col_cnt +
									"&row=" + row;

		DownLoadText(connectionString, null, (isSucc, jsnStr) =>
		{
			if (onRecv != null)
				onRecv(isSucc, jsnStr);
			Logger(jsnStr);

		});
	}


	public void SetRow(string sheetID, string sheetName, string value, int row, System.Action<bool, string> onRecv)
	{
		string action = "SetRow";
		string connectionString = webServiceUrl +
									"?ssid=" + sheetID +
									"&sheet=" + sheetName +
									"&pass=" + password +
									"&action=" + action +
									"&val1=" + value +
									"&row=" + row;

		DownLoadText(connectionString, null, (isSucc, jsnStr) =>
		{
			if (onRecv != null)
				onRecv(isSucc, jsnStr);
			Logger(jsnStr);

		});
	}

	public void SetRowText(string sheetID, string sheetName, string value, int row, System.Action<bool, string> onRecv)
	{
		string action = "SetRowLaw";
		string connectionString = webServiceUrl +
									"?ssid=" + sheetID +
									"&sheet=" + sheetName +
									"&pass=" + password +
									"&action=" + action +
									"&val1=" + value +
									"&row=" + row;

		DownLoadText(connectionString, null, (isSucc, jsnStr) =>
		{
			if (onRecv != null)
				onRecv(isSucc, jsnStr);
			Logger(jsnStr);

		});
	}

	public void SetData(string sheetID, string sheetName, int row, int col, object value, System.Action<bool, string> onRecv)
	{
		string action = "SetData";
		string connectionString = webServiceUrl +
									"?ssid=" + sheetID +
									"&sheet=" + sheetName +
									"&pass=" + password +
									"&action=" + action +
									"&row=" + row +
									"&col=" + col +
									"&value=" + value;

		DownLoadText(connectionString, null, (isSucc, jsnStr) =>
		{
			if (onRecv != null)
				onRecv(isSucc, jsnStr);
			Logger(jsnStr);

		});
	}



	
	
	public void DownLoadText(string url, WWWForm wform, System.Action<bool, string> onLoad)
	{
		StartCoroutine(LoadWebText(url, wform, onLoad));
	}
	public void DownLoadText(string url, System.Action<bool, string> onLoad)
	{
		StartCoroutine(LoadWebText( url, null, onLoad));
	}
	IEnumerator LoadWebText(string url, WWWForm wform, System.Action<bool, string> onLoad)
	{
		Debug.Log(url);
		WWW www;
		if (wform != null)
			www = new WWW(url, wform);
		else
			www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			onLoad(true, System.Text.Encoding.UTF8.GetString(www.bytes).Trim());
		}
		else
		{
			onLoad(false, www.error);
		}
		www.Dispose();
	}
	
}