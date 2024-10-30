using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Transform[] _spawnPoints;
    private float _spawnTimer = 1f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] Canvas _canvas;
    // Start is called before the first frame update
    void Start()
    {
        _spawnPoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnTimer < 0)
        {
            _canvas.enabled = false;
            Instantiate(_enemyPrefab, _spawnPoints[Random.Range(1, 3)].transform.position, Quaternion.identity);
            _spawnTimer = Random.Range(1f, 4f);
        }
        _spawnTimer -= Time.deltaTime;

    }
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}
