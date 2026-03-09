using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuizManager))]
public class QuizManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        _ = DrawDefaultInspector();

        QuizManager quizManager = (QuizManager) target;

        GUILayout.Space(10);

        if (GUILayout.Button("Fill bones data"))
        {
            Undo.RegisterCompleteObjectUndo(quizManager.gameObject, "Fill bones data");

            string[] guids = AssetDatabase.FindAssets
            (
                filter: "t:BoneData",
                searchInFolders: new string[] { "Assets/BoneData" }
            );

            var bones = guids.Select(guid =>
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BoneData asset = AssetDatabase.LoadAssetAtPath<BoneData>(path);
                return asset;
            });

            quizManager.SetBones(bones);

            if (!Application.isPlaying)
                _ = UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(quizManager.gameObject.scene);
        }
    }
}
