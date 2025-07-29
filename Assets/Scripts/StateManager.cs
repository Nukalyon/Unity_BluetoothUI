using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.name = "StateManager";
        DontDestroyOnLoad(this);
    }

    private void OnAppStateChange(string state)
    {
        // AppState : com.example.plugin.model.AppState$Scanning@866848f
        string temp = state.PartAfter('$').PartBefore('@');
        Debug.LogError("AppState : " + temp);
    }
}
