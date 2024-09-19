using UnityEngine;
using UnityEngine.UI;

public class MouseItemControl : MonoBehaviour
{ 
    public Button use_Btn { get; private set; }
    public Button throw_Btn { get; private set; }
    private Camera mainCam;
    private Canvas canvas;
    private RectTransform rect;

    private void Start()
    {
        use_Btn = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        throw_Btn = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        mainCam = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        rect = GetComponentInParent<RectTransform>();
    }

    private void LateUpdate()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, mainCam,out );
    }
}
