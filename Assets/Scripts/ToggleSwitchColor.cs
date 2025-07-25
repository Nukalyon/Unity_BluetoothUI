using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchColor : ToggleSlider
{
    [Header("Nouvelles couleurs")]
    [SerializeField] private Image backgroundImage;
    [Space]
    [SerializeField] private bool recolorBackground = true;
    [Space]
    [SerializeField] private Color backgroundColorOn = new Color(37f, 99f, 235f, 255f);
    [SerializeField] private Color backgroundColorOff = new Color(229f, 231f, 235f, 255f);


    private new void OnValidate()
    {
        base.OnValidate();
        ChangeColor();
    }

    private void OnEnable()
    {
        TransitionEffect += ChangeColor;
    }

    private void OnDisable()
    {
        TransitionEffect -= ChangeColor;
    }

    protected override void Awake()
    {
        base.Awake();
        ChangeColor();
    }


    private void ChangeColor()
    {
        if (recolorBackground && backgroundImage is not null)
        {
            backgroundImage.color = Color.Lerp(backgroundColorOff, backgroundColorOn, sliderValue);
        }
    }
}
