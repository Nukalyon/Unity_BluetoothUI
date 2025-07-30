using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private GameObject _idleDisplay;
    [SerializeField] private GameObject _scanningDisplay;
    [SerializeField] private GameObject _connectingDisplay;
    [SerializeField] private GameObject _connectedDisplay;
    [SerializeField] private GameObject _disconnectingDisplay;
    [SerializeField] private GameObject _disconnectedDisplay;
    private List<GameObject> _panels = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.name = "StateManager";
        DontDestroyOnLoad(this);
        // check if a gameobject is null
        if(_idleDisplay != null) _panels.Add(_idleDisplay);
        if(_scanningDisplay != null) _panels.Add(_scanningDisplay);
        if(_connectingDisplay != null) _panels.Add(_connectingDisplay);
        if(_connectedDisplay != null) _panels.Add(_connectedDisplay);
        if(_disconnectingDisplay != null) _panels.Add(_disconnectingDisplay);
        if(_disconnectedDisplay != null) _panels.Add(_disconnectedDisplay);
    }

    private void OnAppStateChange(string state)
    {
        // AppState : com.example.plugin.model.AppState$Scanning@866848f
        string currentState = state.PartAfter('$').PartBefore('@');
        Debug.LogWarning("AppState : " + currentState);
        GameObject caller = _idleDisplay;
        switch (currentState)
        {
            case "Idle":
                _idleDisplay.SetActive(true);
                caller = _idleDisplay;
                break;
            case "Scanning":
                _scanningDisplay.SetActive(true);
                caller = _scanningDisplay;
                break;
            case "Connecting":
                _connectingDisplay.SetActive(true);
                caller = _connectingDisplay;
                break;
            case "Connected":
                _connectedDisplay.SetActive(true);
                caller = _connectedDisplay;
                break;
            case "Disconnecting":
                _disconnectingDisplay.SetActive(true);
                caller = _disconnectingDisplay;
                break;
            case "Disconnected":
                _disconnectedDisplay.SetActive(true);
                caller = _disconnectedDisplay;
                break;
            default:
                Debug.Log("State not recognized");
                break;
        }
        ToggleOther(caller);
    }

    private void ToggleOther(GameObject displayed)
    {
        foreach (GameObject go in _panels)
        {
            if (go != displayed)
            {
                go.SetActive(false);
            }
        }
    }
}
