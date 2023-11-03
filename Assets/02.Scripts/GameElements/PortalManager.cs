using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.GameElements
{
    public class PortalManager : MonoBehaviour
    {
        public static PortalManager instance
        {
            get
            {
                if (_instance == null)
                {
                    // 포탈매니저는 MonoBehaviour이기 때문에 생성자로 못 만듦.
                    // 그래서 현재 씬에서 다음 씬으로 넘어가려고 포탈 매니저를 호출했을 때
                    // 빈 깡통의 게임오브젝트를 만들고 거기에 컴포넌트로 클래스를 붙인다.
                    _instance = new GameObject("PortalManager").AddComponent<PortalManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;    
            }
        }
        private static PortalManager _instance;

        public void TeleportScene(string from, string to)
        {
            StartCoroutine(C_TeleportScene(from, to));
        }

        private IEnumerator C_TeleportScene(string from, string to)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(to);

            while (asyncOperation.isDone == false)
                yield return null;

            FindObjectsOfType<Portal>()
                .FirstOrDefault(portal => portal.currentScene == to &&
                                          portal.destinationScene == from);
        }


    }
}
