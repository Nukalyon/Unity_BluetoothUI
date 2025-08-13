using TMPro;
using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
    [SerializeField] TMP_Text _messageText;
   
    void Start()
    {
        if(_messageText == null) Debug.LogError("TMP_Text is null, ref it in MessageReceiver.cs");
    }
    
    /// <summary>
    /// This method is called when a message is received and device is connected
    /// In here it just concatenate to the others messages
    /// </summary>
    /// <param name="message"> Message received from another device </param>
    public void OnMessageReceive(string message)
    {
        Debug.Log("Message received : " + message);
        if (_messageText != null)
        {
            _messageText.text += message + '\n';
        }
    }
}
