using UnityEditor;
using UnityEngine;

namespace _Projects._Scripts.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private int levelWidth = 10;
        private int levelHeight = 10;

        private GameObject tilePrefab;

        [MenuItem("LevelEditorWindow/OpenWindow")]
        private static void ShowWindow()
        {
            LevelEditorWindow window = GetWindow<LevelEditorWindow>();
            window.titleContent = new GUIContent("LevelEditor");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Settings", EditorStyles.boldLabel);

            levelWidth = EditorGUILayout.IntField("Width", levelWidth);
            levelHeight = EditorGUILayout.IntField("Height", levelHeight);

            tilePrefab =
                EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false) as GameObject;

            if (GUILayout.Button("Generate Level"))
            {
                bool result = EditorUtility.DisplayDialog("Warning",
                    "Custom Level design algorithm needed, which is not implemented yet....", "OK", "Cancel");

                EditorUtility.ClearProgressBar();
            }
        }
    }
}