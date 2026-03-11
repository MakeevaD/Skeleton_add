using UnityEngine;

public class QuizGenerator : MonoBehaviour
{
    [SerializeField] private BoneData[] bones;

    public BoneData[] GenerateQuiz(int length)
    {
        if (bones == null || bones.Length == 0)
            return System.Array.Empty<BoneData>();

        int count = Mathf.Min(length, bones.Length);

        BoneData[] result = (BoneData[])bones.Clone();
        for (int i = result.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (result[i], result[j]) = (result[j], result[i]);
        }

        if (count == result.Length)
            return result;

        BoneData[] trimmed = new BoneData[count];
        System.Array.Copy(result, trimmed, count); 
        return trimmed;
    }
}
