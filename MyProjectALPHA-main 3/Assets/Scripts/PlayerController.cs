using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool gameOver = false;
    public bool isOnRoad = true;
    private bool isInvincible = false; 
    public GameObject powerupIndicator;

    private Vector3 middlePosition = new Vector3(1, 0, 0);
    private Vector3 leftPosition = new Vector3(-11.5f, 0, 0);
    private Vector3 rightPosition = new Vector3(13.5f, 0, 0);

    private Animator playerAnim;
    private Rigidbody playerRb;

    public float jumpForce;
    public float gravityModifier;
    private int positionState = 1;

    private Vector3 targetPosition;
    public float sideMoveSpeed = 5f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameOver = false;
        transform.position = middlePosition;
        targetPosition = middlePosition;
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -9.81f, 0) * gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerRb.freezeRotation = true;

        playerAnim.enabled = false; // Disable at start
    }

    void Update()
    {
        if (!gameManager.IsGameStarted() || gameOver) return;

        powerupIndicator.transform.position = transform.position + new Vector3(0, 1, 0);

        if (Input.GetKeyDown(KeyCode.Space) && isOnRoad && !gameOver)
        {
            Debug.Log("Triggering Jump Animation");
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnRoad = false;
            playerAnim.SetTrigger("Jump_trig");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (positionState == 1)
            {
                targetPosition = leftPosition;
                positionState = 0;
            }
            else if (positionState == 2)
            {
                targetPosition = middlePosition;
                positionState = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (positionState == 1)
            {
                targetPosition = rightPosition;
                positionState = 2;
            }
            else if (positionState == 0)
            {
                targetPosition = middlePosition;
                positionState = 1;
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * sideMoveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameManager.IsGameStarted()) return;

        if (collision.gameObject.CompareTag("Road"))
        {
            isOnRoad = true;
        }
        else if (!isInvincible && (collision.gameObject.CompareTag("Enemybarrier") 
                 || collision.gameObject.CompareTag("Enemyarmorcar") 
                 || collision.gameObject.CompareTag("Enemybus") 
                 || collision.gameObject.CompareTag("EnemyCar")))
        {
            Debug.Log("Triggering GameOver Animation");
            gameOver = true;
            playerAnim.SetTrigger("GameOver");
            playerRb.useGravity = true;
            playerRb.velocity = Vector3.zero;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            gameManager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            isInvincible = true; 
            powerupIndicator.gameObject.SetActive(true);
            Debug.Log("Invincibility is on!!!");
            Destroy(other.gameObject); 
            StartCoroutine(ActivateInvincibility()); 
        }
    }

    IEnumerator ActivateInvincibility()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        isInvincible = false; 
        powerupIndicator.gameObject.SetActive(false);
        Debug.Log("NO more Invincibility!!!.");

    }

    public void EnablePlayerAnimator()
    {
        playerAnim.enabled = true;
    }

    public void ResetPlayer()
    {
        gameOver = false;
        isOnRoad = true;
        positionState = 1;
        targetPosition = middlePosition;
        transform.position = middlePosition;

        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        Physics.gravity = new Vector3(0, -9.81f, 0) * gravityModifier;

        playerAnim.enabled = false;
    }
}
