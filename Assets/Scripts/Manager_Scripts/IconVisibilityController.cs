using UnityEngine;

public class IconVisibilityController : MonoBehaviour
{
    private readonly string[] iconsToDisable = {
        "Alert Icon", "Good Signal Icon", "Low Signal Icon", "No Signal Icon"
    };
    private const string iconToEnable = "Connected Icon";

    void Start()
    {
        Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();

        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.gameObject.scene.name == null || !canvas.gameObject.activeInHierarchy)
                continue;

            Transform parent = canvas.transform;

            foreach (string iconName in iconsToDisable)
            {
                Transform icon = parent.Find(iconName);
                if (icon != null)
                    icon.gameObject.SetActive(false);
            }

            Transform connectedIcon = parent.Find(iconToEnable);
            if (connectedIcon != null)
                connectedIcon.gameObject.SetActive(true);

            // Assume icons only exist under one Canvas
            break;
        }
    }
}