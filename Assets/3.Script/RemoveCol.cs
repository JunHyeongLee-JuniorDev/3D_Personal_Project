using UnityEngine;
using UnityEditor;

public class RemoveCol : MonoBehaviour
{
    [MenuItem("Tools/Remove All Mesh Colliders")]
    public static void RemoveAllMeshColliders()
    {
        // 하이러키의 모든 오브젝트 탐색
        Collider[] meshColliders = FindObjectsOfType<Collider>();

        // 찾은 모든 MeshCollider를 제거
        foreach (Collider meshCollider in meshColliders)
        {
            DestroyImmediate(meshCollider);
        }

        Debug.Log(meshColliders.Length + " 개의 MeshCollider가 제거되었습니다.");
    }
}
