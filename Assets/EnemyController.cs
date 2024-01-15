using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed as needed
    public int maxEnemies = 5; // Maximum number of enemies
    private Transform player;
    private Camera mainCamera;
    private int spawnedEnemies = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Flag").transform;
        mainCamera = Camera.main;

        // Start spawning enemies with random delays
        InvokeRepeating("SpawnOutsideCameraView", Random.Range(5f, 10f), Random.Range(5f, 10f));
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void SpawnOutsideCameraView()
    {
        if (spawnedEnemies < maxEnemies)
        {
            Vector2 spawnPosition = new Vector2();

            do
            {
                // Set random spawn position outside the camera view
                spawnPosition.x = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
                                                mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
                spawnPosition.y = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y,
                                                mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
            } while (IsInsideCameraView(spawnPosition));

            // Instantiate a new enemy clone
            GameObject enemyClone = Instantiate(gameObject, spawnPosition, Quaternion.identity).gameObject;

            // Set the new clone's properties if needed
            // For example, you might want to set the clone's speed or other attributes.

            spawnedEnemies++;
        }
        else
        {
            // Reached maximum number of enemies, you can handle this case as needed
            // For example, you might want to disable further spawning or take other actions.
            Debug.Log("Maximum number of enemies reached.");
        }
    }

    bool IsInsideCameraView(Vector2 position)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy OnTriggerEnter2D: " + other.gameObject.name);

        if (other.CompareTag("Flag"))
        {
            Debug.Log("Enemy collided with Flag");
            SceneManager.LoadScene("GameOver");
        }
    }
}
