using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class DebugButton : MonoBehaviour
{
    static public DebugButton current = null;
	static private bool ShowStateTxt = true;

	[SerializeField]
    private bool ShowStart = false;
	[SerializeField]
    private GameObject goPrefab;
    static GameObject instance;

	[SerializeField]
	private GameObject IngameHierachyPrefab;
	[SerializeField]
	static GameObject IngameHierachy;

	[SerializeField]
	private GameObject IngameInspectorPrefab;
	[SerializeField]
	static GameObject IngameInspector;


	[SerializeField]
	Transform stateTextRoot;
	[SerializeField]
	Text stateTextPrefab;
	Dictionary<string, Text> stateText = new Dictionary<string, Text>();

	[SerializeField]
	GameObject statisticalBoardObj;
	void Awake()
    {
        current = this;
        if (EditedBuildOption.IsDeveloper)
        {
            if (ShowStart || PlayerPrefs.HasKey("sb_dbg"))
                Create( true );
            else
                Create(false);
        }
        else
        {
			PlayerPrefs.DeleteKey("DbgTimeScale");
			gameObject.SetActive(false);
            //GameObject.Destroy(gameObject);
        }


		ShowStateTxt = PlayerPrefs.GetInt("state_text", 0) == 1;
		HideStateText(!ShowStateTxt);
		stateTextPrefab.gameObject.SetActive(false);
	}


    void Start()
    {
        
    }

    public void Create(bool show)
    {
        if (instance == null)
        {
			SetDbgFunc();  

            gameObject.SetActive(true);
            instance = Instantiate(goPrefab) as GameObject;

            if (show)
            {
                LMJ.SetLog(true);
                instance.SetActive(true);
            }
            else
                instance.SetActive(false);
        }
        
    }

	static public void AddOrUpdateStateText(string key, string text)
    {
		if (DebugButton.current == null || DebugButton.ShowStateTxt == false )
			return;

		DebugButton.current.InnnerAddOrUpdateStateText(key, text);
	}

	void HideStateText(bool hide )
	{
		ShowStateTxt = !hide;

		if(ShowStateTxt)
			PlayerPrefs.SetInt("state_text", 1);
		else
			PlayerPrefs.SetInt("state_text", 0);

		if (ShowStateTxt == false)
			stateTextRoot.gameObject.SetActive(false);
		else
			stateTextRoot.gameObject.SetActive(true);
	}

	void InnnerAddOrUpdateStateText(string key, string text)
	{
		if (ShowStateTxt == false)
			return;

		if (gameObject.activeSelf == false)
			return;

		if (stateText.ContainsKey(key))
		{
			stateText[key].text = key + " : " + text;
		}
		else
		{
			Text tt = GameObject.Instantiate(stateTextPrefab).GetComponent<Text>();
			tt.gameObject.SetActive(true);
			tt.transform.SetParent(stateTextRoot);
			tt.transform.localScale = Vector3.one;
			tt.text = key + " : " + text;
			stateText.Add(key, tt);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	void ShowHierachy()
	{
		IngameHierachy = Instantiate(IngameHierachyPrefab);
		IngameInspector = Instantiate(IngameInspectorPrefab);


		RuntimeInspectorNamespace.RuntimeHierarchy runtimeHierarchy = IngameHierachy.GetComponentInChildren<RuntimeInspectorNamespace.RuntimeHierarchy>();
		RuntimeInspectorNamespace.RuntimeInspector runtimeInspector = IngameInspector.GetComponentInChildren<RuntimeInspectorNamespace.RuntimeInspector>();
		runtimeHierarchy.ConnectedInspector = runtimeInspector;
		runtimeInspector.ConnectedHierarchy = runtimeHierarchy;
	}
	void ToggleInpector()
	{
		
	}

	public void OnClickDBG()
    {
        //if (instance == null)
        //{
        //    instance = Instantiate(goPrefab) as GameObject;
        //    return;
        //}

        if (instance.activeSelf)
            instance.SetActive(false);
        else
            instance.SetActive(true);
    }

	bool isSetDbgFunc = false;
	public void SetDbgFunc()
	{
		if (isSetDbgFunc)
			return;
		isSetDbgFunc = true;

		
		IngameDebugConsole.DebugLogConsole.AddCommand("igh", "ShowHierachy ", () =>
		{
			DebugButton.current.ShowHierachy();
			IngameDebugConsole.DebugLogManager.Instance.HideLogWindow();
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("sb", "show debug btn on start", () =>
		{
			if (PlayerPrefs.HasKey("sb_dbg"))
				PlayerPrefs.DeleteKey("sb_dbg");
			else
				PlayerPrefs.SetInt("sb_dbg", 1);
		});

		

		IngameDebugConsole.DebugLogConsole.AddCommand("lvup", "lvUp", () =>
		{
			//if (GameLogic.Inst != null)
			//{
			//	GameLogic.Inst.player.TestLvUp();
			//}
		});


		IngameDebugConsole.DebugLogConsole.AddCommand("lgeng", "English", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "English(United States)";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgkr", "Korean", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Korean";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgchT", "Chinese (Traditional)", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Chinese (Traditional)";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgchS", "Chinese (Simplified)", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Chinese (Simplified)";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgjp", "Japanese", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Japanese";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgger", "German (Germany)", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "German (Germany)";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgsp", "Spanish", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Spanish";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgfr", "French", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "French";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgru", "Russian", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Russian";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgin", "Indonesian", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Indonesian";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgar", "Arabic", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Arabic";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgma", "Malay", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Malay";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgth", "Thai", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Thai";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgpor", "Portuguese", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Portuguese";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgtur", "Turkish", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Turkish";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("lgItal", "Italian", () =>
		{
			I2.Loc.LocalizationManager.CurrentLanguage = "Italian";
			PlayerPrefs.SetString("language", I2.Loc.LocalizationManager.CurrentLanguage);
			LMJ.Log(I2.Loc.LocalizationManager.CurrentLanguage);
		});

		IngameDebugConsole.DebugLogConsole.AddCommand("ts", "TimeScale ", (int value) =>
		{
			Time.timeScale = value;
		});

		
	}

	public void ToggleDebugConsole()
    {
		if(instance.gameObject.activeSelf)
        {
			instance.gameObject.SetActive(false);

		}
		else
        {
			instance.SetActive(true);
		}

	}


}
