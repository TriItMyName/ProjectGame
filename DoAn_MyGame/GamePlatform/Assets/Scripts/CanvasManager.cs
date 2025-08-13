using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private RectTransform rt;
    [SerializeField] private float minY = -1.5f;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Start()
    {

    }

    void Update()
    {
        Vector2 pos = rt.anchoredPosition;
        if (pos.y < minY)
        {
            pos.y = minY;
        }
        rt.anchoredPosition = pos;
    }

}
