using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Düþman Prefablarý")]
    public List<EnemyTypePrefab> enemyPrefabs; // Inspector'da ekle
    [Header("Malzeme Prefablarý")]
    public List<CollectibleTypePrefab> collectiblePrefabs; // Inspector'da ekle

    private Dictionary<string, GameObject> enemyPrefabDict;
    private Dictionary<string, GameObject> collectiblePrefabDict;

    [Header("Spawn Noktalarý")]
    public Transform[] enemySpawnPoints;
    public Transform[] collectibleSpawnPoints;

    void Awake()
    {
        // Prefablarý dictionary'ye aktar
        enemyPrefabDict = new Dictionary<string, GameObject>();
        foreach (var item in enemyPrefabs)
            enemyPrefabDict[item.typeName] = item.prefab;

        collectiblePrefabDict = new Dictionary<string, GameObject>();
        foreach (var item in collectiblePrefabs)
            collectiblePrefabDict[item.typeName] = item.prefab;
    }

    // Görev tipine göre spawn iþlemi
    public void SpawnQuestTargets(string questType, string targetType, int count)
    {
        if (questType == "kill")
        {
            SpawnEnemies(targetType, count);
        }
        else if (questType == "gather")
        {
            SpawnCollectibles(targetType, count);
        }
    }

    public void SpawnEnemies(string enemyType, int count)
    {
        if (!enemyPrefabDict.ContainsKey(enemyType))
        {
            Debug.LogWarning($"SpawnManager: '{enemyType}' prefabý bulunamadý!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = enemySpawnPoints.Length > 0 ? enemySpawnPoints[i % enemySpawnPoints.Length] : transform;
            GameObject enemyObj = Instantiate(enemyPrefabDict[enemyType], spawnPoint.position, Quaternion.identity);
            EnemyVariant variant = enemyObj.GetComponent<EnemyVariant>();
            if (variant != null)
                variant.enemyType = enemyType;
        }
    }

    public void SpawnCollectibles(string collectibleType, int count)
    {
        if (!collectiblePrefabDict.ContainsKey(collectibleType))
        {
            Debug.LogWarning($"SpawnManager: '{collectibleType}' prefabý bulunamadý!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = collectibleSpawnPoints.Length > 0 ? collectibleSpawnPoints[i % collectibleSpawnPoints.Length] : transform;
            GameObject colObj = Instantiate(collectiblePrefabDict[collectibleType], spawnPoint.position, Quaternion.identity);
            // Collectibles scripti kendi türünü içeriyor
        }
    }
}

[System.Serializable]
public class EnemyTypePrefab
{
    public string typeName; // Örn: "Kurt", "Haydut"
    public GameObject prefab;
}

[System.Serializable]
public class CollectibleTypePrefab
{
    public string typeName; // Örn: "Odun", "Taþ"
    public GameObject prefab;
}