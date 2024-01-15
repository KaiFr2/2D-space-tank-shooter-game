using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed as needed
    private Transform player;
    private Camera mainCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Flag").transform;
        mainCamera = Camera.main;

        // Ensure the enemy spawns outside the camera view
        SpawnOutsideCameraView();
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
        Vector2 spawnPosition = new Vector2();

        do
        {
            // Set random spawn position outside the camera view
            spawnPosition.x = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
                                            mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
            spawnPosition.y = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y,
                                            mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
        } while (IsInsideCameraView(spawnPosition));

        transform.position = spawnPosition;
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