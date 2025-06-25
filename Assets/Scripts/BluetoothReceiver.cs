using UnityEngine;

public class BluetoothReceiver : MonoBehaviour
{
    private void Awake()
    {
        gameObject.name = "BluetoothReceiver"; // Important : le nom doit correspondre a celui dans UnitySendMessage
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Cette methode sera appelee depuis le plugin Kotlin.
    /// </summary>
    /// <param name="msg">Le message envoye depuis le plugin</param>
    public void OnMessageArrived(string msg)
    {
        Debug.Log("Message recu du plugin Kotlin : " + msg);

        // Exemple de traitement du message
        if (msg.Contains("connected"))
        {
            Debug.Log("L'appareil est connecte !");
        }
    }
}
