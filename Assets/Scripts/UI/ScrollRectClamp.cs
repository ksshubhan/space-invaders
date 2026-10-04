using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectClamp : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 100f;  

    private void Update()
    {
        ClampScrollPosition();
    }

    // Clamps the scroll position within specified range
    private void ClampScrollPosition()
    {
        if (scrollRect != null && scrollRect.content != null)
        {
            float clampedY = Mathf.Clamp(scrollRect.content.anchoredPosition.y, minY, maxY);
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, clampedY);
        }
        else
        {
            Debug.LogWarning("ScrollRect or content is not assigned in the inspector. Please assign them to ensure proper functionality.");
        }
    }
}
