using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatRoad : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;
    public float speed = 15f;

    private PlayerController playerControllerScript;
    private GameManager gameManager;

    void Start()
    {
        startPos = transform.position;
        repeatWidth = 120;

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerControllerScript = player.GetComponent<PlayerController>();
        }

        gameManager = FindObjectOfType<GameManager>();
    }

void Update()
{
    if (gameManager != null && gameManager.IsGameStarted() && playerControllerScript != null && !playerControllerScript.gameOver)
    {
        float adjustedSpeed = speed * gameManager.speedMultiplier; // Adjust speed based on multiplier
        transform.Translate(Vector3.back * adjustedSpeed * Time.deltaTime);

        if (transform.position.z < startPos.z - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}

}
