using Platformer.Controllers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUi : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;
    [SerializeField] private PlayerController _player;

    private void Start()
    {
        _hpBar.minValue = 0.0f;
        _hpBar.maxValue = _player.hpMax;
        _hpBar.value = _player.hpValue;

        _player.onHpChanged += Refresh;
    }
    private void Refresh(float Value)
    {
        _hpBar.value = Value;
    }
}
