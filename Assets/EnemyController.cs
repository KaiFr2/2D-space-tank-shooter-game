using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private startgame gameManager;

    [SerializeField]
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    private float radius;

    /// <summary>
    /// maximum number of enemies
    /// </summary>
    [SerializeField]
    private int maxEnemies; // Maximum number of enemies

    /// <summary>
    /// Referenec to the player the enemies will swa
    /// </summary>
    [SerializeField]
    private Transform flag;

    /// <summary>
    /// Reference to the main camera
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// Prefab for the enemy that will target the player
    /// </summary>
    [SerializeField]
    private Enemy enemyPrefab;

    [SerializeField]
    private int spawnedEnemies = 0;

    private Coroutine huntRoutine;

    private void Awake()
    {
        mainCamera = Camera.main;
        for(int i = 0; i <= maxEnemies; i++)
        {
            SpawnEnemy();
        }

        StartTheHunt();
        
    }

    [ContextMenu("spawn enemy")]
    public void spawnShit()
    {
        Enemy inactiveEnemy = null;

        foreach (var enemy in enemies)
        {
            if (!enemy.ActivelyHunting)
            {
                inactiveEnemy = enemy;
                break; // Found an inactive enemy, no need to continue the loop
            }
        }

        SpawnOutsideCameraView(inactiveEnemy);
    }

    /// <summary>
    /// Subscribe to event when the player gets hit
    /// </summary>
    private void OnEnable()
    {
        Enemy.OnPlayerHit += HandlePlayerDied;
        Enemy.OnEnemyHit += HandleEnemyDied;
    }

    /// <summary>
    /// Unsubscribes from event
    /// </summary>
    private void OnDisable()
    {
        Enemy.OnPlayerHit -= HandlePlayerDied;
        Enemy.OnEnemyHit -= HandleEnemyDied;
    }

    private void HandleEnemyDied(Enemy enemyThatDied)
    {
        enemyThatDied.gameObject.SetActive(false);
    }

    private void HandlePlayerDied()
    {
        foreach (var enemy in enemies)
            enemy.gameObject.SetActive(false);

        if(huntRoutine != null)
         StopCoroutine(huntRoutine);

        // Logic for when the player is ded.
        gameManager.LoadScene("GameOver");
    }

    private void SpawnEnemy()
    {
        Enemy enemyClone = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
        enemyClone.gameObject.SetActive(false);
        enemies.Add(enemyClone);
    }

    private void StartTheHunt()
    {
        huntRoutine = StartCoroutine(CheckIfEnemyShouldSpawn());
    }

    private IEnumerator CheckIfEnemyShouldSpawn()
    {
       // Debug.LogError("starting the hunt");
        // turn an enemy active
        int activeEnemies = enemies.Count(x => x.ActivelyHunting);
      //  Debug.LogError($"active animes at the moment {activeEnemies}");

        // already 4 active enemies
        if (activeEnemies >= 4)
        {
        yield return new WaitForSeconds(2f);
            StopCoroutine(huntRoutine);
            huntRoutine = null;
            StartTheHunt();
        } else
        {
            Enemy notActivelyHuntingEnemy = enemies.FirstOrDefault(x => !x.ActivelyHunting);

            if (notActivelyHuntingEnemy != null)
            {
           //     Debug.LogError("1");
                // Do something with the notActivelyHuntingEnemy
                // For example, you can spawn it outside the camera view
                SpawnOutsideCameraView(notActivelyHuntingEnemy);
                yield return new WaitForSeconds(2f);
                StopCoroutine(huntRoutine);
                huntRoutine = null;
                StartTheHunt();
            }
            else
            {
                // All enemies are actively hunting or there are no enemies
              //  Debug.LogError("2");
                yield return new WaitForSeconds(2f);
                StopCoroutine(huntRoutine);
                huntRoutine = null;
                StartTheHunt();
            }
        }

      
    }


    /// <summary>
    /// Method that will spawn a new enemy outside of the screen
    /// </summary>
    private void SpawnOutsideCameraView(Enemy enemy)
    {
        if (enemies == null)
            return;

            Vector2 spawnPosition = new Vector2();
        Camera camera = Camera.main;

        // Generate a position outside of the camera. Keep trying until you get one.
            // Generate random values outside the viewport
            float randomX = Random.Range(-1.0f, 2.0f);
            float randomY = Random.Range(-1.0f, 2.0f);

            // Determine which side to spawn based on random values
            bool spawnLeft = (randomX < 0.5f);
            bool spawnBottom = (randomY < 0.5f);

            // Calculate spawn position outside the camera view with a radius
            float spawnX = spawnLeft
                ? camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - radius
                : camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + radius;

            float spawnY = spawnBottom
                ? camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - radius
                : camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + radius;

        spawnPosition = new Vector2(spawnX, spawnY);
        

        enemy.SetData(spawnPosition, flag.transform);
    }

    /// <summary>
    /// Method to calculate if a position is within the camera viewport
    /// </summary>
    /// <param name="position"></param>
    /// <returns> If the position is within the camera viewport></returns>
    bool IsInsideCameraView(Vector2 position)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
    }
}
