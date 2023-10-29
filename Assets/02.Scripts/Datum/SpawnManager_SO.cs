using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnManager", menuName = "Platformer/ScriptableObjects/SpawnManager")]
public class SpawnManager_SO : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    //public Vector3[] spawnPoints;
    //public Vector3 spawnAreaBoxCenter;
    //public float spawnAreaBoxWidth;
    //public float spawnAreaBoxHeight;
}