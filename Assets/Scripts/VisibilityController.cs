using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public void ToggleVisibility(GameObject go)
    {
        if (go == null) {
            Debug.LogWarning("GameObject is null");
            return;
        }
        go.SetActive(!go.activeSelf);
    }
}
