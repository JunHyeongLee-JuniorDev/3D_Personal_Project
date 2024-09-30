using UnityEngine;
using UnityEditor;

public class RemoveCol : MonoBehaviour
{
    [MenuItem("Tools/Remove All Mesh Colliders")]
    public static void RemoveAllMeshColliders()
    {
        // ���̷�Ű�� ��� ������Ʈ Ž��
        Collider[] meshColliders = FindObjectsOfType<Collider>();

        // ã�� ��� MeshCollider�� ����
        foreach (Collider meshCollider in meshColliders)
        {
            DestroyImmediate(meshCollider);
        }

        Debug.Log(meshColliders.Length + " ���� MeshCollider�� ���ŵǾ����ϴ�.");
    }
}
