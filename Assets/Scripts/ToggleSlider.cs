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

    internal bool CurrentValue { get; set;}
    private bool _previousValue;
    private Slider _slider;
    
    [Header("Slider Animation")]
    [SerializeField, Range(0f, 1f)] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0,0,1,1);
    
    private Coroutine _animateSliderCoroutine;
    protected Action TransitionEffect;
    
    [Header("Slider Events")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;
    
    private ToggleSwitchGroupManager _toggleSwitchManager;

    protected void OnValidate()
    {
        SetupToggleComponents();
        _slider.value = sliderValue;
    }

    protected virtual void Awake()
    {
        SetupToggleComponents();
    }


    private void SetupToggleComponents()
    {
        if (_slider != null)
        {
            return;
        }
        SetupSliderComponents();
    }

    private void SetupSliderComponents()
    {
        _slider = GetComponent<Slider>();
        if (_slider == null)
        {
            Debug.LogWarning("SliderComponents not found");
            return;
        }
        _slider.interactable = false;
        var sliderColor = _slider.colors;
        sliderColor.disabledColor = Color.white;
        _slider.colors = sliderColor;
        _slider.transition = Selectable.Transition.None;
    }

    public void SetupManager(ToggleSwitchGroupManager manager)
    {
        _toggleSwitchManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        if (_toggleSwitchManager != null)
        {
            _toggleSwitchManager.ToggleGroup(this);
        }
        else
        {
            SetStateAndStartAnimation(!CurrentValue);
        }
    }

    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        CurrentValue = state;
        if (_previousValue != CurrentValue) {
            onToggleOn?.Invoke();
        }
        else {
            onToggleOff?.Invoke();
        }

        if (_animateSliderCoroutine != null)
        {
            StopCoroutine(_animateSliderCoroutine);
        }

        if (gameObject.activeInHierarchy)
        {
            _animateSliderCoroutine = StartCoroutine(AnimateSlider());
        }
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);
                TransitionEffect?.Invoke();
                yield return null;
            }
        }
        _slider.value = endValue;
    }
}
