using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class MissingReferencesFinder : MonoBehaviour
{

    [MenuItem("Tools/Find MIssing/Show Missing Object References in scene", false, 50)]
    public static void FindMissingReferencesInCurrentScene()
    {
        var objects = GetSceneObjects();
        FindMissingReferences(EditorSceneManager.GetActiveScene().name, objects);
    }

    [MenuItem("Tools/Find MIssing/Show Missing Object References in all scenes", false, 51)]
    public static void MissingSpritesInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
        {
            EditorSceneManager.OpenScene(scene.path);
            FindMissingReferences(scene.path, GetSceneObjects());
        }
    }

    [MenuItem("Tools/Find MIssing/Show Missing Object References in assets", false, 52)]
    public static void MissingSpritesInAssets()
    {

        //var allAssets = AssetDatabase.FindAssets("t:Prefab", "c:\\Work\\SB_BubbleShooter\\SB_BubbleShooter\\Assets\\Effects\\IGSoft_Resources\\");

        var allAssets = AssetDatabase.GetAllAssetPaths();
        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

        FindMissingReferences("Project", objs);
    }

    [MenuItem("Tools/Find MIssing/Remove Missing script in asset", false, 50)]
    public static void RemoveMissingScript()
    {
        var allAssets = AssetDatabase.GetAllAssetPaths();
        var allAssetsLst = allAssets.ToList();
        for (int i = 0; i < allAssetsLst.Count; i++)
        {
            //if (allAssetsLst[i].Contains("IGSoft_Resources/Projects/[EffectParticle]") == false)
            //if (allAssetsLst[i].Contains("[EffectParticle]") == false)
            if (allAssetsLst[i].Contains("Projects/Effect") == false)
            {
                allAssetsLst.RemoveAt(i);
                i--;
            }
        }
        allAssets = allAssetsLst.ToArray();

        //List<string> allAssetsLst = new List<string>();
        //allAssetsLst.Add("assets/effects/igsoft_resources/projects/[effectparticle]/legacy_virtical/cloud 10.prefab");
        //allAssetsLst.Add("assets/effects/igsoft_resources/projects/[effectparticle]/legacy_virtical/cloud 11.prefab");
        //allAssetsLst.Add("assets/effects/igsoft_resources/projects/[effectparticle]/legacy_virtical/cloud 12.prefab");
        //allAssetsLst.Add("assets/effects/igsoft_resources/projects/[effectparticle]/legacy_virtical/cloud 13.prefab");
        //allAssetsLst.Add("assets/effects/igsoft_resources/projects/[effectparticle]/legacy_virtical/cloud 14.prefab");
        //var allAssets = allAssetsLst.ToArray();

        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();
        RemoveMissingScriptReferences("Project", objs);
    }

    private static void FindMissingReferences(string context, GameObject[] objects)
    {
        foreach (var go in objects)
        {
            var components = go.GetComponents<Behaviour>();

            foreach (var c in components)
            {
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + FullPath(go), go);
                    continue;
                }

                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                        }
                    }
                }
            }
        }
    }

    private static void RemoveMissingScriptReferences(string context, GameObject[] objects)
    {
        foreach (var go in objects)
        {
            RemoveMissingScriptReferencesRecur(go);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static void RemoveMissingScriptReferencesRecur(GameObject go) 
    {
        var components = go.GetComponents<Behaviour>();

        for (int j = 0; j < components.Length; j++)
        {
            var c = components[j];
            if (!c)
            {
                Debug.LogError("Missing Component in GO: " + FullPath(go), go);
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                UnityEditor.EditorUtility.SetDirty(go);
                break;
            }
        }


        for (int n = go.transform.childCount - 1; 0 <= n; n--)
        {
            if (n < go.transform.childCount)
            {
                GameObject ch = go.transform.GetChild(n).gameObject;
                RemoveMissingScriptReferencesRecur(ch);
            }
        }
    }


    [MenuItem("Tools/Find Text/Find Text Object References in scene", false, 50)]
    public static void FindTextsInCurrentScene()
    {
        var objects = GetSceneObjects();
        //var objects = GetScenePrefabs();
        FindTexts(EditorSceneManager.GetActiveScene().name, objects);
    }

    [MenuItem("Tools/Find Text/Find Text Object References in all scenes", false, 51)]
    public static void TextsInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
        {
            EditorSceneManager.OpenScene(scene.path);
            FindTexts(scene.path, GetSceneObjects());
        }
    }

    [MenuItem("Tools/Find Text/Show Text References in assets", false, 52)]
    public static void TextsInAssets()
    {
        //FindFiles(Application.dataPath + "/UISource", ref imgList, ".prefab", false);


        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        List<string> paths = new List<string>();
        //char sep = Path.DirectorySeparatorChar;
        //string dir = "";
        //dir = "Resources";
        //paths.Add(dir);
        //dir = "BubbleShooterKit/Prefabs";
        //paths.Add(dir);
        //dir = "BubbleShooterKit/Resources";
        //paths.Add(dir);
        //var allAssets = paths.ToArray();
        for(int i = 0; i < allAssets.Length; i++)
        {
            if (allAssets[i].Contains(".prefab"))
                paths.Add(allAssets[i]);
        }
        allAssets = paths.ToArray();

        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

        FindTexts("Project", objs);
    }

    private static void FindTexts(string context, GameObject[] objects)
    {
        foreach (var go in objects)
        {
            var components = go.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var c in components)
            {
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + FullPath(c.gameObject), c.gameObject);
                    continue;
                }

                SerializedObject so = new SerializedObject(c.gameObject);
                var sp = so.GetIterator();
                if (c.GetComponent<I2.Loc.Localize>() != null)
                { 
                    //ShowError(err, context, c.gameObject, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name) + " text : " + c.text);
                    continue;
                }

                const string fmt = "Find text in: [{3}]{0}. Component: {1}, Property: {2}";
                ShowError(fmt, context, c.gameObject, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name) + " text : " + c.text);

                //while (sp.NextVisible(true))
                //{
                //    ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                //}
            }
        }
    }





    [MenuItem("Tools/Change font/Change font Object References in scene", false, 50)]
    public static void ChangeFontsInCurrentScene()
    {
        var objects = GetSceneObjects();
        //var objects = GetScenePrefabs();
        ChangeFonts(EditorSceneManager.GetActiveScene().name, objects);
    }

    [MenuItem("Tools/Change font/Change font Object References in all scenes", false, 51)]
    public static void ChangeFontsInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
        {
            EditorSceneManager.OpenScene(scene.path);
            ChangeFonts(scene.path, GetSceneObjects());
        }
    }

    [MenuItem("Tools/Change font/Show Text References in assets", false, 52)]
    public static void ChangeFontsInAssets()
    {
        //FindFiles(Application.dataPath + "/UISource", ref imgList, ".prefab", false);


        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        List<string> paths = new List<string>();
        //char sep = Path.DirectorySeparatorChar;
        //string dir = "";
        //dir = "Resources";
        //paths.Add(dir);
        //dir = "BubbleShooterKit/Prefabs";
        //paths.Add(dir);
        //dir = "BubbleShooterKit/Resources";
        //paths.Add(dir);
        //var allAssets = paths.ToArray();
        for (int i = 0; i < allAssets.Length; i++)
        {
            if (allAssets[i].Contains(".prefab"))
                paths.Add(allAssets[i]);
        }
        allAssets = paths.ToArray();

        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

        ChangeFonts("Project", objs);
    }

    private static void ChangeFonts(string context, GameObject[] objects)
    {
        foreach (var go in objects)
        {
            var components = go.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var c in components)
            {
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + FullPath(c.gameObject), c.gameObject);
                    continue;
                }

                I2.Loc.Localize local = c.GetComponent<I2.Loc.Localize>();
                if (local == null)
                    continue;
                
                SerializedObject so = new SerializedObject(c.gameObject);
                var sp = so.GetIterator();

                ShowError(context, c.gameObject, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name) + " text : " + c.text);

                //while (sp.NextVisible(true))
                //{
                //    ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                //}
            }
        }
    }







    [MenuItem("Tools/AddLocalizations/AddLocalizations Object References in scene", false, 50)]
    public static void AddLocalizationsInCurrentScene()
    {
        //var objects = GetSceneObjects();
        var objects = GetScenePrefabs();
        AddLocalizations(EditorSceneManager.GetActiveScene().name, objects);
    }

    [MenuItem("Tools/AddLocalizations/AddLocalizations Object References in all scenes", false, 51)]
    public static void AddLocalizationsInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
        {
            EditorSceneManager.OpenScene(scene.path);
            AddLocalizations(scene.path, GetSceneObjects());
        }
    }

    [MenuItem("Tools/AddLocalizations/Show Text References in assets", false, 52)]
    public static void AddLocalizationsInAssets()
    {
        //FindFiles(Application.dataPath + "/UISource", ref imgList, ".prefab", false);


        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        List<string> paths = new List<string>();
        //char sep = Path.DirectorySeparatorChar;
        //string dir = "";
        //dir = "Resources";
        //paths.Add(dir);
        //dir = "BubbleShooterKit/Prefabs";
        //paths.Add(dir);
        //dir = "BubbleShooterKit/Resources";
        //paths.Add(dir);
        //var allAssets = paths.ToArray();
        for (int i = 0; i < allAssets.Length; i++)
        {
            if (allAssets[i].Contains(".prefab"))
                paths.Add(allAssets[i]);
        }
        allAssets = paths.ToArray();

        var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

        AddLocalizations("Project", objs);
    }

    private static void AddLocalizations(string context, GameObject[] objects)
    {
        GameObject template = null;
        for(int i = 0; i < objects.Length ; i++)
        {
            if (objects[i].name.Contains("languageTemp"))
                template = objects[i];
        }

        I2.Loc.Localize templateComp = null;
        if (template != null)
        {
            templateComp = template.GetComponentInChildren<I2.Loc.Localize>();
            if (templateComp == null)
            {
                LMJ.LogError("templateComp == null");
                return;
            }
        }
        else
        {
            LMJ.LogError("template == null");
            return;
        }

        foreach (var go in objects)
        {
            if (template == go)
                continue;

            var components = go.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var c in components)
            {
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + FullPath(c.gameObject), c.gameObject);
                    continue;
                }

                I2.Loc.Localize local = c.GetComponent<I2.Loc.Localize>();
                if (local != null)
                {
                    string firstTerm = local.Term;
                    string sencondTerm = local.SecondaryTerm;
                    var target = local.mLocalizeTarget;
                    var targetName = local.mLocalizeTargetName;
                    //LMJ_Utill.CopyComponent<I2.Loc.Localize>(templateComp, local.gameObject);
                    EditorUtility.CopySerialized(templateComp, local);
                    local.Term = firstTerm;
                    //local.SecondaryTerm = firstTerm;
                    local.mLocalizeTarget = target;
                    local.mLocalizeTargetName = targetName;
                }
                else
                    continue;

                SerializedObject so = new SerializedObject(c.gameObject);
                var sp = so.GetIterator();

                ShowError(context, c.gameObject, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name) + " text : " + c.text);
            }
        }
    }


    private static GameObject[] GetSceneObjects()
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
                   && go.hideFlags == HideFlags.None).ToArray();
    }

    private static GameObject[] GetScenePrefabs()
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go)) == false && AssetDatabase.GetAssetPath(go).Contains(".prefab") ).ToArray();
    }

    private const string err = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";

    private static void ShowError(string context, GameObject go, string c, string property)
    {
        ShowError(err, context, go, c, property);
    }

    private static void ShowError(string format, string context, GameObject go, string c, string property)
    {
        Debug.LogError(string.Format(format, FullPath(go), c, property, context), go);
    }

    private static string FullPath(GameObject go)
    {
        return go.transform.parent == null
            ? go.name
                : FullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}