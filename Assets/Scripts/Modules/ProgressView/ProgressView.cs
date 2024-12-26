using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressView : MonoBehaviour
{
    public Slider slider;
    public Slider sliderFront;
    public float delayTime = 1;
    public float maxValue = 1;
    public float Value
    {
        get
        {
            return m_value;
        }
        set
        {
            value = Mathf.Clamp(value, 0, maxValue);
            if (m_targetValue != value)
            {
                m_targetValue = value;

                m_intervalValue = (m_targetValue - m_value) / (delayTime / Time.deltaTime);
            }
        }
    }

    private float m_value;
    private float m_targetValue; // 目标值
    private float m_intervalValue; // 距离目标值

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxValue;
        sliderFront.maxValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = maxValue;
        sliderFront.maxValue = maxValue;
        if (Mathf.Abs(m_value - m_targetValue) > 0.1f)
        {
            m_value += m_intervalValue;
        }
        slider.value = m_value;
        sliderFront.value = m_targetValue;
    }

    public void SetValueIm(float value)
    {
        m_value = value;
        m_targetValue = value;
    }
}
