using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private PlayerController playerControllerScript;
    private GameManager gameManager; 
    private float speed = 30; // enemy's base movement speed
    private float lowerBound = -15f; // destroy off-screen below

    void Start()
    {
        // get player and script
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerControllerScript = player.GetComponent<PlayerController>();
        }

        // find the game manager
        gameManager = FindObjectOfType<GameManager>();

        // log error if missing manager
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Update()
    {
        // move if game isn't over
        if (playerControllerScript != null && !playerControllerScript.gameOver)
        {
            // adjust speed using multiplier
            if (gameManager != null)
            {
                float adjustedSpeed = speed * gameManager.speedMultiplier; 
                transform.Translate(Vector3.forward * Time.deltaTime * adjustedSpeed);
            }
            else
            {
                // move at default speed
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        // destroy enemy below boundary
        if (transform.position.z < lowerBound)
        {
            Destroy(gameObject);
        }
    }
}
