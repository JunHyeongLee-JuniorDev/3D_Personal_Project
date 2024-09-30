using UnityEngine;
using UnityEditor;

public class SaveMeshAsset : MonoBehaviour
{
    [MenuItem("Tools/Save Selected Mesh")]
    public static void SaveSelectedMesh()
    {
        // 선택된 GameObject 가져오기
        GameObject selectedObj = Selection.activeGameObject;

        if (selectedObj != null)
        {
            // MeshFilter가 있는지 확인
            MeshFilter meshFilter = selectedObj.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.sharedMesh;

                // 저장 경로 설정
                string path = EditorUtility.SaveFilePanelInProject("Save Mesh", mesh.name, "asset", "Please enter a file name to save the mesh to");
                if (path != "")
                {
                    // 메쉬를 에셋으로 저장
                    AssetDatabase.CreateAsset(mesh, path);
                    AssetDatabase.SaveAssets();
                    Debug.Log("Mesh saved to: " + path);
                }
            }
            else
            {
                Debug.LogWarning("선택된 객체에 MeshFilter가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject를 선택하세요.");
        }
    }
}
