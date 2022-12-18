using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Gradient _gradient;
    [SerializeField]
    private Image _fill;
   
    /// <summary>
    /// Setam valoarea maxima a vietii din bara
    /// </summary>
    /// <param name="health">Viata maxima</param>
    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        _fill.color = _gradient.Evaluate(1f);
    }

    /// <summary>
    /// Schimbam dimensiunea culorii din bara de viata si gradientul
    /// </summary>
    /// <param name="health">Valoarea noua a vietii</param>
    public void SetHealth(float health)
    {
        _slider.value = health;

        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    public void DestroyHealthBar()
    {
        Destroy(gameObject);
    }
}
