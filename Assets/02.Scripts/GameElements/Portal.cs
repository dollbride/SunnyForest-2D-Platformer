using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Platformer.GameElements
{
    public class Portal : MonoBehaviour
    {
        // ����� �̵��ع����� ���� ���� �� ��Ż ������ �ı��ȴ�.
        // �׷��� ����ƽ�ϰ� ����Ʈ���� �ɷ��ִ� ���� �Ŵ����� �����ؼ� �����Ѵ�.
        

        public string currentScene;
        public string destinationScene;
        public bool isBusy;
        public Portal destination;  // �̵��� �ٸ� ��Ż ���

        public void Teleport(Transform target)
        {
            if (isBusy)
            {
                return;
            }

            // �̵��� ��Ż�� ���� �� ���� �ִ� ���������� ��ϵ� ������ �ű�� ����
            if (destination) 
            {
                target.position = destination.transform.position;

                Busy(1.0f);
                destination.Busy(1.0f);
            }
            // ���� ���� �ƴ� ���(������X) �̸� ������ ��ǥ ���� �ε�ȴ�. 
            else if(string.IsNullOrEmpty(destinationScene) == false)
            {
                // �ε� ���� �ϸ� ���� �� ������ �����ǰ� ���� �� ������ �� �ҷ��´�.
                // �׷��� ������ ����ƽ �̱��� ���� �Ŵ����� ���� �� ���� ����
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
            // �븮�� Invoke���� �ٸ� �Լ���. �Լ� ȣ�� �� ������ �� �� ������Ű�� �Լ�
            Invoke("Ready", duration);
        }

    }
}

