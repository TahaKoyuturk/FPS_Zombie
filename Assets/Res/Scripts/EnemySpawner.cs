using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables

    #region Public Variables

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int NumberOfEnemy;

    #endregion

    #region Private Variables

    private List<Transform> usedSpawnPoints = new List<Transform>();

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        SpawnMultipleEnemies(NumberOfEnemy); 
    }
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnLevelUp, SpawnMultipleEnemies);
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnLevelUp, SpawnMultipleEnemies);
    }

    #endregion

    #region Custom Methods

    public void SpawnMultipleEnemies(object value)
    {
        for (int i = 0; i < NumberOfEnemy; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        List<Transform> availableSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!usedSpawnPoints.Contains(spawnPoint))
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        if (availableSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];

            Instantiate(enemyPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation,this.transform);

            // Kullanılan spawn noktayı listeye ekle
            usedSpawnPoints.Add(selectedSpawnPoint);
        }
    }

    #endregion

    #endregion
}
