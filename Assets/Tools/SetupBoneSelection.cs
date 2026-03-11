using UnityEngine;
using UnityEditor;

public class SetupBoneSelection
{
    [MenuItem("Tools/Bones/Setup Selection")]
    public static void Setup()
    {
        GameObject skeleton = Selection.activeGameObject;

        if (skeleton == null)
        {
            Debug.LogError("Выбери root объект скелета");
            return;
        }

        Transform[] bones = skeleton.GetComponentsInChildren<Transform>();

        int count = 0;

        foreach (Transform bone in bones)
        {
            Renderer rend = bone.GetComponent<Renderer>();

            if (rend == null)
                continue;

            if (bone.GetComponent<Collider>() == null)
            {
                MeshCollider col = bone.gameObject.AddComponent<MeshCollider>();
                col.convex = true;
            }

            if (bone.GetComponent<BoneSelectable>() == null)
            {
                bone.gameObject.AddComponent<BoneSelectable>();
            }

            count++;
        }

        Debug.Log("Настроено костей: " + count);
    }
}