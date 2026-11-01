using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _spawnTimer = 1f;
    
    //적을 _spawnTimer마다 1개 생성하세요.
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.GetComponent<Enemy01>().SetUp(_wayPoints);
            
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}
