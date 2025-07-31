using TMPro;
using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
    [SerializeField] TMP_Text _messageText;
   
    void Start()
    {
        if(_messageText == null) Debug.LogError("TMP_Text is null, ref it in MessageReceiver.cs");
    }

    public void OnMessageReceive(string message)
    {
        Debug.Log("Message received : " + message);
        if (_messageText != null)
        {
            _messageText.text += message + '\n';
        }
    }
}
