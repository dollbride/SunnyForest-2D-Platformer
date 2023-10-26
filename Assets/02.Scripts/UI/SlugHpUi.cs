using Platformer.Controllers;
using UnityEngine;
using UnityEngine.UI;

public class SlugHpUi : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;
    [SerializeField] private SlugController _slug;

    private void Start()
    {
        _hpBar.minValue = 0.0f;
        _hpBar.maxValue = _slug.hpMax;
        _hpBar.value = _slug.hpValue;

        _slug.onHpChanged += Refresh;
    }
    private void Refresh(float Value)
    {
        _hpBar.value = Value;
    }
}
