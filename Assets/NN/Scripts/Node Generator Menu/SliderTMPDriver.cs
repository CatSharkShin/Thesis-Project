using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;


public class SliderTMPDriver : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Slider slider;
    private void OnEnable()
    {
        if(tmp != null)
        slider.onValueChanged.AddListener(delegate { Set(); });
    }
    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(delegate { Set(); });
    }
    private void Set()
    {
        tmp.text = slider.value.ToString("F2");
    }
}
