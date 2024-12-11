using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private GameManager gameManager; 
    private PlayerController playerControllerScript; 
    private float speed = -30;
    private float lowerBound = -15f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerControllerScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (playerControllerScript != null && !playerControllerScript.gameOver)
        {
            if (gameManager != null)
            {
                float adjustedSpeed = speed * gameManager.speedMultiplier; 
                transform.Translate(Vector3.forward * Time.deltaTime * adjustedSpeed); 
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        if (transform.position.z < lowerBound)
        {
            Destroy(gameObject);
        }
    }
}
