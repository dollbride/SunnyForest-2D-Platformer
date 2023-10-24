using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.Test
{
    // 전처리기 if + 유니티 에디터를 걸어놓으면 유니티에서만 테스트하는 용도로 한정하는 것
    // 빌드를 해서 앱으로 만들거나 했을 때는 이 내용이 컴파일 되지 않는다.
#if UNITY_EDITOR
    public class TestGUI : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;

        private void Awake()
        {
            // 씬에서 Player라는 이름의 오브젝트를 찾아서 그 중에서 컨트롤러 컴포넌트를 찾아라.
            // Find는 다 찾는 거라 성능이 안 좋음. 그래서 이렇게 테스트 할 때만 사용한다.
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

