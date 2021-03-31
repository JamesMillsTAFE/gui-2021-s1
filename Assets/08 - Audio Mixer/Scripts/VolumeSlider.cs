using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private string volumeParameter;

    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = PlayerPrefs.GetFloat(volumeParameter, 1);
        slider.onValueChanged.AddListener(_value => mixer.SetFloat(volumeParameter, Remap(_value, 0, 1, -80, 0)));
    }

    // This function is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Takes a float and changes its minimum and maximum range to the new one.
    /// </summary>
    /// <param name="_value">The value being remapped.</param>
    /// <param name="_oldMin">The old minimum value of the float.</param>
    /// <param name="_oldMax">The old maximum value of the float.</param>
    /// <param name="_newMin">The new minimum value of the float.</param>
    /// <param name="_newMax">The new maximum value of the float.</param>
    private float Remap(float _value, float _oldMin, float _oldMax, float _newMin, float _newMax)
    {
        return (_value - _oldMin) / (_oldMax - _oldMin) * (_newMax - _newMin) + _newMin;
    }
}
