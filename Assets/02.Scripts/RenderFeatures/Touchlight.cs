using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Platformer.GameElements.RenderFeature
{
    [RequireComponent(typeof(Light2D))]
    public class Touchlight : MonoBehaviour
    {
        private Light2D _light;
        [SerializeField] private float _waveSpeed;
        [SerializeField] private float _fallOffMin = 0.1f;
        [SerializeField] private float _fallOffMax = 0.5f;


        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void Update()
        {
            _light.falloffIntensity
                = Mathf.Clamp(Mathf.Abs(Mathf.Sin(_waveSpeed * Time.time)), _fallOffMin, _fallOffMax);
        }
    }
}
