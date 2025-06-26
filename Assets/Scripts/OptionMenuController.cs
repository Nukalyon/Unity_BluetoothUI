using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    public Button openMenuButton;
    public Button returnButton;
    public GameObject optionMenu;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openMenuButton.onClick.AddListener(ToggleMenu);
        returnButton.onClick.AddListener(ToggleMenu);
        optionMenu.SetActive(false);
    }

    void ToggleMenu()
    {
        bool isVisible = optionMenu.activeSelf;
        openMenuButton.gameObject.SetActive(isVisible);
        optionMenu.SetActive(!isVisible);
    }
}
