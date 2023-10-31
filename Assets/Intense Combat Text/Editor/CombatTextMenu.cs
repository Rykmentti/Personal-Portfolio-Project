using System.IO;
using UnityEditor;
using UnityEngine;
namespace DevPlz.CombatText
{
#if UNITY_EDITOR
    public class CombatTextMenu : MonoBehaviour
    {

    [MenuItem("Tools/Intense Combat Text/Add CombatText Manager to scene")]
        public static void AddManagerToScene()
        {
            if (FindObjectOfType<CombatText>())
            {
                Debug.Log("Scene already contains a CombatText Manager");
                return;
            }
            GameObject o = Instantiate((GameObject)Resources.Load("Prefabs/CombatText Manager"));
            o.transform.position = Vector3.zero;
            o.name = "CombatText Manager";
            o.GetComponent<CombatText>().Reset();
            Undo.RegisterCreatedObjectUndo(o, "Create Combat Text Manager");
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        }

        [MenuItem("Tools/Intense Combat Text/Refresh TextStyle enum")]
        public static void CreateEnumFile()
        {
            string path = "Assets/Intense Combat Text/Runtime/Generated/TextStyleEnum.cs";
            CombatTextStyle[] styles = Resources.LoadAll<CombatTextStyle>("CombatTextStyles/");
            using (StreamWriter outfile =
                     new StreamWriter(path))
            {
                outfile.WriteLine("namespace DevPlz.CombatText");
                outfile.WriteLine("{");
                outfile.WriteLine("//This is an automatically generated file.");
                outfile.WriteLine("//To rebuild it and add new enum values, use Tools/Combat Text/Refresh TextStyle enum");
                outfile.WriteLine("public enum TextStyle");
                outfile.WriteLine("{");
                for (int i = 0; i < styles.Length; i++)
                {
                    outfile.Write(styles[i].name);
                    if (i < styles.Length - 1) outfile.Write(",");
                }
                outfile.WriteLine("}");
                outfile.WriteLine("}");

            }
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/Intense Combat Text/Create New CombatTextStyle")]
        public static void CreateNewCombatTextStyle() 
        {
            CombatTextStyle newStyle = new CombatTextStyle();
            AssetDatabase.CreateAsset(newStyle, "Assets/Intense Combat Text/Resources/CombatTextStyles/NewCombatTextStyle.asset");
            EditorGUIUtility.PingObject(newStyle);
        }
    }
#endif
}

