using UnityEngine;
using UnityEditor;

public class RemoveCol : MonoBehaviour
{
    [MenuItem("Tools/Remove All Colliders")]
    public static void RemoveAllMeshColliders()
    {
        // ���̷�Ű�� ��� ������Ʈ Ž��
        Collider[] meshColliders = FindObjectsOfType<Collider>();

        // ã�� ��� MeshCollider�� ����
        foreach (Collider meshCollider in meshColliders)
        {
            DestroyImmediate(meshCollider);
        }

        Debug.Log(meshColliders.Length + " ���� Collider�� ���ŵǾ����ϴ�.");
    }
}
