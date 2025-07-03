using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSlider : MonoBehaviour, IPointerClickHandler
{
    /***    Crédit à la chaîne : Christina Creates Games (https://www.youtube.com/@ChristinaCreatesGames)
     *      ref: https://www.youtube.com/watch?v=E9AWlbPGi_4
     */

    [Header("Options Slider")] 
    [SerializeField, Range(0f, 1f)] protected float sliderValue;
    public bool CurrentValue { get; private set;}
    private bool previousValue;
    private Slider slider;
    
    [Header("Slider Animation")]
    [SerializeField, Range(0f, 1f)] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0,0,1,1);
    
    private Coroutine animateSliderCoroutine;
    protected Action transitionEffect;
    
    [Header("Slider Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;
    
    private ToggleSwitchManager toggleSwitchManager;

    protected void OnValidate()
    {
        SetupToggleComponents();
        slider.value = sliderValue;
    }

    protected virtual void Awake()
    {
        SetupToggleComponents();
    }


    private void SetupToggleComponents()
    {
        if (slider != null)
        {
            return;
        }
        SetupSliderComponents();
    }

    private void SetupSliderComponents()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogWarning("SliderComponents not found");
            return;
        }
        slider.interactable = false;
        var sliderColor = slider.colors;
        sliderColor.disabledColor = Color.white;
        slider.colors = sliderColor;
        slider.transition = Selectable.Transition.None;
    }

    public void SetupManager(ToggleSwitchManager manager)
    {
        toggleSwitchManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        if (toggleSwitchManager != null)
        {
            toggleSwitchManager.ToggleGroup(this);
        }
        else
        {
            SetStateAndStartAnimation(!CurrentValue);
        }
    }

    private void SetStateAndStartAnimation(bool state)
    {
        previousValue = CurrentValue;
        CurrentValue = state;
        if (previousValue != CurrentValue) {
            onToggleOn?.Invoke();
        }
        else {
            onToggleOff?.Invoke();
        }

        if (animateSliderCoroutine != null)
        {
            StopCoroutine(animateSliderCoroutine);
        }

        animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);
                transitionEffect?.Invoke();
                yield return null;
            }
        }
        slider.value = endValue;
    }
}
