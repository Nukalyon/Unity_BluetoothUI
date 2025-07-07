
using UnityEngine;
using System;

public class BluetoothPermissionManager : AndroidJavaProxy
{
    private AndroidJavaClass _unityPlayerClass;
    private AndroidJavaObject _managerInstance;
    private Action<bool> permissionCallback;

    public BluetoothPermissionManager() : base("com.example.plugin.PermissionCallback")
    {
        _unityPlayerClass = new AndroidJavaClass("com.example.plugin.MyUnityPlayer");
        AndroidJavaObject activity = _unityPlayerClass.CallStatic<AndroidJavaObject>("getActivity");
        
        _managerInstance = new AndroidJavaObject(
            "com.example.plugin.BluetoothPermissionManager", 
            activity.Call<AndroidJavaObject>("getApplicationContext")
        );
    }

    public bool CheckPermissions()
    {
        return _managerInstance.Call<bool>("checkAllPermissionsGranted");
    }

    public void RequestPermissions(Action<bool> callback)
    {
        permissionCallback = callback;
        
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            _managerInstance.Call("requestBluetoothPermissions", activity);
        }));
    }

    // Called from Kotlin
    public void OnPermissionResult(bool granted)
    {
        if (permissionCallback != null)
        {
            // Execute on main thread
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                permissionCallback(granted);
            });
        }
    }
}

// Required for thread dispatching
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private static readonly object lockObject = new();
    private static System.Collections.Generic.Queue<Action> executionQueue = new();

    public static UnityMainThreadDispatcher Instance()
    {
        lock(lockObject)
        {
            if (instance == null)
            {
                GameObject go = new GameObject("UnityMainThreadDispatcher");
                instance = go.AddComponent<UnityMainThreadDispatcher>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
    }
}
