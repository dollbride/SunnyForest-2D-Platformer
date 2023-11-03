using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Platformer.GameElements
{
    public class Portal : MonoBehaviour
    {
        // 장면을 이동해버리면 지금 쓰던 이 포탈 정보가 파괴된다.
        // 그래서 스태틱하게 돈디스트로이 걸려있는 포털 매니저를 참조해서 생성한다.
        

        public string currentScene;
        public string destinationScene;
        public bool isBusy;
        public Portal destination;  // 이동할 다른 포탈 등록

        public void Teleport(Transform target)
        {
            if (isBusy)
            {
                return;
            }

            // 이동할 포탈이 같은 씬 내에 있는 프리팹으로 등록돼 있으면 거기로 가고
            if (destination) 
            {
                target.position = destination.transform.position;

                Busy(1.0f);
                destination.Busy(1.0f);
            }
            // 같은 씬이 아닐 경우(프리팹X) 미리 지정된 목표 씬이 로드된다. 
            else if(string.IsNullOrEmpty(destinationScene) == false)
            {
                // 로드 씬을 하면 기존 씬 정보는 삭제되고 다음 씬 정보를 싹 불러온다.
                // 그래서 별도의 스태틱 싱글톤 게임 매니저에 현재 씬 정보 저장
                //SceneManager.LoadScene(destinationScene);
                PortalManager.instance.TeleportScene(currentScene, destinationScene);
            }
            

        }

        public void Ready()
        {
            isBusy = false;
        }

        public void Busy(float duration)
        {
            isBusy = true;
            // 대리자 Invoke랑은 다른 함수임. 함수 호출 뒤 실행을 몇 초 지연시키는 함수
            Invoke("Ready", duration);
        }

    }
}

