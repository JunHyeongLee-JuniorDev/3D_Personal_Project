using UnityEngine;
using UnityEditor;

public class SaveMeshAsset : MonoBehaviour
{
    [MenuItem("Tools/Save Selected Mesh")]
    public static void SaveSelectedMesh()
    {
        // ���õ� GameObject ��������
        GameObject selectedObj = Selection.activeGameObject;

        if (selectedObj != null)
        {
            // MeshFilter�� �ִ��� Ȯ��
            MeshFilter meshFilter = selectedObj.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.sharedMesh;

                // ���� ��� ����
                string path = EditorUtility.SaveFilePanelInProject("Save Mesh", mesh.name, "asset", "Please enter a file name to save the mesh to");
                if (path != "")
                {
                    // �޽��� �������� ����
                    AssetDatabase.CreateAsset(mesh, path);
                    AssetDatabase.SaveAssets();
                    Debug.Log("Mesh saved to: " + path);
                }
            }
            else
            {
                Debug.LogWarning("���õ� ��ü�� MeshFilter�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject�� �����ϼ���.");
        }
    }
}
