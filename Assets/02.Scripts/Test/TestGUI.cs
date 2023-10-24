using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.Test
{
    // ��ó���� if + ����Ƽ �����͸� �ɾ������ ����Ƽ������ �׽�Ʈ�ϴ� �뵵�� �����ϴ� ��
    // ���带 �ؼ� ������ ����ų� ���� ���� �� ������ ������ ���� �ʴ´�.
#if UNITY_EDITOR
    public class TestGUI : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;

        private void Awake()
        {
            // ������ Player��� �̸��� ������Ʈ�� ã�Ƽ� �� �߿��� ��Ʈ�ѷ� ������Ʈ�� ã�ƶ�.
            // Find�� �� ã�� �Ŷ� ������ �� ����. �׷��� �̷��� �׽�Ʈ �� ���� ����Ѵ�.
            GameObject.Find("Player")?.TryGetComponent(out _controller);
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10.0f, 10.0f, 200.0f, 140.0f), "Test");

            if (GUI.Button(new Rect(20.0f, 40.0f, 140.0f, 80.0f), "Hurt"))
            {
                _controller?.DepleteHp(null, 10.0f);
            }
        }
#endif
    }
} 

