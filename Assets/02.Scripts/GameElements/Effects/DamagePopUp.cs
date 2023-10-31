using UnityEngine;
using TMPro;

namespace Platformer.Effetcs
{
    public class DamagePopUp : MonoBehaviour
    {
        private TMP_Text _amount;
        private float _fadeSpeed = 0.8f;
        private Vector3 _move = new Vector3(0.0f, 0.3f, 0.0f);
        private Color _color;

        public void Show(float amount)
        {
            _amount.text = ((int)amount).ToString();
            _color.a = 1.0f;
            _amount.color = _color; // ������� ���� ���� 0���� ����ó�� �ߴ� �� �ٽ� ����
        }

        private void Awake()
        {
            _amount = GetComponent<TMP_Text>();
            _color = _amount.color;
        }
        private void Update()
        {
            transform.position += _move * Time.deltaTime;
            _color.a -= _fadeSpeed * Time.deltaTime; // _color�� Color ����4 ����ü�� ���� ����
            _amount.color = _color;                 // TMP�� ����� �ִ� color�� ����ü���� ���� �Ұ� 

            if (_color.a < 0.0f)
                //Destroy(gameObject);
                gameObject.SetActive(false);


        }
    }
}
