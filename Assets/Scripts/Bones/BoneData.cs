using UnityEngine;

[CreateAssetMenu(fileName = "BoneData", menuName = "Skeleton/Bone Data")]
public class BoneData : ScriptableObject
{
    public string boneName;       // название кости
    public int boneID;            // ID кости
    public GameObject boneObject; // ссылка на объект кости в сцене
}
