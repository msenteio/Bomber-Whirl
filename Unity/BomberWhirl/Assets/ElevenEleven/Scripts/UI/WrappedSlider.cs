using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class WrappedSlider : Slider {
    
    [SerializeField] bool m_wrapValues = true;
    bool wrapValues {
        get { return m_wrapValues; }
    }
    
    [SerializeField] Button m_decrementButton;
    Button decrementButton {
        get { return m_decrementButton; }
    }

    [SerializeField] Button m_incrementButton;
    Button incrementButton {
        get { return m_incrementButton; }
    }

    float stepSize { get { return wholeNumbers ? 1 : (maxValue - minValue) * 0.1f; } }

    protected override void Set(float input, bool sendCallback) {
        if (wrapValues) {
            if (input < base.minValue) {
                input = base.maxValue + (input - base.minValue);
            } else if (input >= base.maxValue) {
                input = base.minValue + (input - base.maxValue);
            }
        }

        base.Set(input, sendCallback);
    }

    protected override void Awake() {
        base.Awake();
        if (decrementButton != null) {
            decrementButton.onClick.AddListener(delegate {
                ChangeValue(false);
            });
        }
        if (incrementButton != null) {
            incrementButton.onClick.AddListener(delegate {
                ChangeValue(true);
            });
        }
        //base.onValueChanged.AddListener(ValueChanged);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        //base.onValueChanged.RemoveListener(ValueChanged);
    }

    void ChangeValue(bool increment) {
        value += increment ? stepSize : -stepSize;
    }

    //void ValueChanged(float value) {

    //}
}
