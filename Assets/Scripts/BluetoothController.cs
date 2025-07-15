using UnityEngine;

public class BluetoothController : MonoBehaviour
{
    [Header("Bluetooth Settings")]
    public GameObject pluginInitializer;
    public TestPlugin testPlugin;
    
    private BluetoothPermissionManager _permissionManager;
    private bool _allPermissionsGranted = true;
    
    private void Awake()
    {
        gameObject.name = "BluetoothController";
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (pluginInitializer != null)
        {
            testPlugin = pluginInitializer.GetComponent<TestPlugin>();
        }
        
        
        _permissionManager = new BluetoothPermissionManager();
        if (!_permissionManager.CheckPermissions())
        {
            _permissionManager.RequestPermissions((granted) => {
                if (granted)
                {
                    // Proceed with Bluetooth operations
                    Debug.Log("Bluetooth permissions granted");
                }
                else
                {
                    // Show message or fallback
                    Debug.LogWarning("Bluetooth permissions denied");
                    _allPermissionsGranted = false;
                }
            });
        }
        else
        {
            // Permissions already granted
            Debug.Log("Bluetooth permissions already granted");
        }
    }
}