using UnityEngine;

public class SlugSpawnerPool : MonoBehaviour
{
    public GameObject prefabToSpawn;    // ������ ���
    public string prefabName;                      // ������ �̸�
    public int numberOfPrefabsToCreate; // ������ ��ü �� 

    private Vector3[] spawnPoints;
    public Vector3 spawnAreaBoxCenter;
    public Vector3 spawnAreaBoxSize;
    [SerializeField] private LayerMask groundMask;

    int instanceNumber = 1;

    void Start()
    {
        spawnPoints = new Vector3[numberOfPrefabsToCreate];
        SpawnEntities();
    }

    void SpawnEntities()
    {
        for (int i = 0; i < numberOfPrefabsToCreate; i++)
        {
            // ���� ���� ��ġ �߰�
            spawnPoints[i] = new Vector3(Random.Range(spawnAreaBoxCenter.x - spawnAreaBoxSize.x / 2, spawnAreaBoxCenter.x + spawnAreaBoxSize.x / 2), Random.Range(spawnAreaBoxCenter.y - spawnAreaBoxSize.y / 2, spawnAreaBoxCenter.y + spawnAreaBoxSize.y / 2), 0f);

            // Raycast�� �� ��ġ ����
            Vector3 castStartPos = spawnPoints[i];
            RaycastHit2D downHit = Physics2D.Raycast(castStartPos, Vector2.down, 1.5f, groundMask);
            if (downHit)
            {
                // �������� ���� �ε��� ��ġ�� ����
                GameObject currentEntity = Instantiate(prefabToSpawn, downHit.point, Quaternion.identity);
                currentEntity.name = prefabName + instanceNumber;
            }

            instanceNumber++;

        }

    }
}