using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject powerUpPrefab; 
    public GameObject[] obstaclePrefabs; 
    private float spawnPosZ = 300f; 
    private float[] spawnPositionsX = new float[] { 12f, 1.5f, -12f }; 

    private PlayerController playerControllerScript; 
    private GameManager gameManager; 

    private int lastPowerUpScore = 0; 

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerControllerScript = player.GetComponent<PlayerController>();
        }

        gameManager = FindObjectOfType<GameManager>(); 

        InvokeRepeating("SpawnRandomObstacle", 1f, 1f);
    }

    void Update()
    {
        if (gameManager != null && gameManager.Score >= lastPowerUpScore + 500)
        {
            lastPowerUpScore += 500;
            SpawnPowerUp();
        }
    }

    void SpawnPowerUp()
    {
        float randomX = spawnPositionsX[Random.Range(0, spawnPositionsX.Length)];
        Vector3 spawnPosition = new Vector3(randomX, 2.5f, spawnPosZ);
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnRandomObstacle()
    {
        if (gameManager != null && gameManager.IsGameStarted() && playerControllerScript != null && !playerControllerScript.gameOver)
        {
            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length); 
            float randomX = spawnPositionsX[Random.Range(0, spawnPositionsX.Length)]; 
            Vector3 spawnPosition = new Vector3(randomX, 0, spawnPosZ);

            Instantiate(obstaclePrefabs[obstacleIndex], spawnPosition, obstaclePrefabs[obstacleIndex].transform.rotation);

            int currentScore = gameManager.Score;
            if (currentScore >= 1000 && currentScore < 1500)
            {
                CancelInvoke("SpawnRandomObstacle");
                InvokeRepeating("SpawnRandomObstacle", 0.5f, 0.5f); 
            }
            else if (currentScore >= 1500)
            {
                CancelInvoke("SpawnRandomObstacle");
                InvokeRepeating("SpawnRandomObstacle", 0.25f, 0.25f); 
            }
        }
    }
}
