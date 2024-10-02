using UnityEngine;
using UnityEditor;

public class RemoveMeshColliders : MonoBehaviour
{
    [MenuItem("Tools/Remove All Mesh Colliders")]
    public static void RemoveAllMeshColliders()
    {
        // ���̷�Ű�� ��� ������Ʈ Ž��
        MeshCollider[] meshColliders = FindObjectsOfType<MeshCollider>();

        // ã�� ��� MeshCollider�� ����
        foreach (MeshCollider meshCollider in meshColliders)
        {
            DestroyImmediate(meshCollider);
        }

        Debug.Log(meshColliders.Length + " ���� MeshCollider�� ���ŵǾ����ϴ�.");
    }
}
