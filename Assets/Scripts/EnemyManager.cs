using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
class EnemyType
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public int squadCount;
    [SerializeField] public float rarity;
    [SerializeField] public int xp;
    [SerializeField] public float size;
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [SerializeField] public float speed;
    [SerializeField] public Color color;
    public float spawnRate;


}


[RequireComponent(typeof(UnitPool))]
public class EnemyManager : MonoBehaviour
{
    #region Fields

    [Header("Debug Settings")]
    [SerializeField] private bool debug = true;

    [Header("Spawn Settings")]
    [SerializeField] private int maxEnemiesSpawned = 100;
    [SerializeField] private int spawnsPerSecond = 1;
    [SerializeField] public bool usePooling = false;
    [SerializeField] private List<EnemyType> enemyPrefabList;

    [Header("Enemy Settings")]
    [SerializeField] private List<Color> predefinedColors; // List of predefined colors
    [SerializeField][Range(0, 20f)] private float enemySpeed = 1f; // Speed of the enemies
    [SerializeField][Range(0, .25f)] private float enemySpeedFactor = 0.05f; // Speed multiplier for the enemies

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] int maxTotalRarity = 0;
    private UnitPool pool;
    private float timer = 0;
    private int curSpawned = 0; // Should increment up to maxEnemiesSpawned

    #endregion

    #region Initialization Methods

    void Start()
    {
        pool = GetComponent<UnitPool>();
        player = FindObjectOfType<Player>().transform;
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();

        enemyPrefabList.Sort((a, b) => a.rarity.CompareTo(b.rarity));

        maxTotalRarity = (int)enemyPrefabList.Sum(e => e.rarity);

        foreach (var enemyType in enemyPrefabList)
        {
            enemyType.spawnRate = enemyType.rarity / maxTotalRarity;
        }
    }

    #endregion

    #region Update Methods

    void Update()
    {
        timer += Time.deltaTime * Time.timeScale;
        while (curSpawned < maxEnemiesSpawned && timer > 1f / spawnsPerSecond)
        {
            SpawnEnemy();
            timer -= 1f / spawnsPerSecond;
        }
    }

    #endregion

    #region Enemy Management Methods

    public void UpdateEnemySpawnRate(float newSpawnRate)
    {
        spawnsPerSecond = (int)(spawnsPerSecond + newSpawnRate);
        // Update your enemy spawning logic here
        Debug.Log("Updated Enemy Spawn Rate: " + spawnsPerSecond);
    }

    public void UpdateEnemySpeed(float modifySpeed)
    {
        enemySpeed = enemySpeed * modifySpeed;
        // Update your enemy speed logic here
        Debug.Log("Updated Enemy Speed: " + enemySpeed);
    }

    public void EnemyKilled(GameObject enemy)
    {
        if (enemy.activeSelf)
        {
            audioSource.Play();

            curSpawned--;

            if (debug)
            {
                Debug.Log("Enemy Killed");
            }
            if (usePooling)
            {
                pool.pool.Release(enemy);
            }
            else
            {
                Destroy(enemy);
            }

            // Notify the GameManager
            if (gameManager != null)
            {
                gameManager.OnEnemyKilled();
            }
        }
    }

    private GameObject SpawnEnemy(bool pooling = false)
    {
        if (enemyPrefabList == null || enemyPrefabList.Count == 0)
        {
            Debug.LogError("Enemy prefab list is empty!");
            return null;
        }

        EnemyType selectedEnemyType = SelectEnemy();

        if (selectedEnemyType == null)
        {
            Debug.LogError("Failed to select an enemy type based on rarity.");
            return null;
        }

        for (int i = 0; i < selectedEnemyType.squadCount; i++)
        {
            GameObject squadEnemy;
            if (usePooling)
            {
                squadEnemy = pool.pool.Get();
            }
            else
            {
                squadEnemy = Instantiate(selectedEnemyType.enemyPrefab, transform);
            }

            if (squadEnemy == null)
            {
                Debug.LogError("Failed to instantiate or get squad enemy from pool.");
                continue;
            }

            Enemy squadEnemyComponent = squadEnemy.GetComponent<Enemy>();
            if (squadEnemyComponent == null)
            {
                Debug.LogError("Squad enemy prefab does not have an Enemy component.");
                continue;
            }

            squadEnemyComponent.Ready();
            squadEnemyComponent.SetMoveSpeed(selectedEnemyType.speed);
            squadEnemyComponent.SetMoveSpeedFactor(enemySpeedFactor);
            squadEnemyComponent.gameObject.transform.localScale = new Vector3(selectedEnemyType.size, selectedEnemyType.size, 1);

            squadEnemy.transform.position = RandomPositionOffScreen();

            curSpawned++;
        }

        return null; // Return null as the method signature expects a GameObject return type
    }

    private EnemyType SelectEnemy()
    {
        if (enemyPrefabList == null || enemyPrefabList.Count == 0)
        {
            Debug.LogError("Enemy prefab list is empty!");
            return null;
        }

        // Calculate the total rarity sum
        float totalRarity = enemyPrefabList.Sum(e => e.rarity);

        // Generate a random number between 0 and the total rarity sum
        float randomValue = Random.Range(0f, totalRarity);

        // Select an enemy type based on the random value
        float cumulativeRarity = 0f;
        foreach (var enemyType in enemyPrefabList)
        {
            cumulativeRarity += enemyType.rarity;
            if (randomValue <= cumulativeRarity)
            {
                return enemyType;
            }
        }

        // Fallback in case of rounding errors
        return enemyPrefabList.Last();
    }

    public int GetEnemyCount()
    {
        return curSpawned;
    }

    #endregion

    #region Utility Methods

    private static Vector3 RandomPositionOffScreen()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Calculate the screen bounds in world coordinates
        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Randomly choose a side (0: top, 1: bottom, 2: right, 3: left)
        int side = Random.Range(0, 4);

        float spawnX = 0;
        float spawnY = 0;

        switch (side)
        {
            case 0: // Top
                spawnX = Random.Range(-screenWidth / 2, screenWidth / 2);
                spawnY = screenHeight / 2 + 1; // Slightly offscreen
                break;
            case 1: // Bottom
                spawnX = Random.Range(-screenWidth / 2, screenWidth / 2);
                spawnY = -screenHeight / 2 - 1; // Slightly offscreen
                break;
            case 2: // Right
                spawnX = screenWidth / 2 + 1; // Slightly offscreen
                spawnY = Random.Range(-screenHeight / 2, screenHeight / 2);
                break;
            case 3: // Left
                spawnX = -screenWidth / 2 - 1; // Slightly offscreen
                spawnY = Random.Range(-screenHeight / 2, screenHeight / 2);
                break;
        }

        // Adjust the spawn position based on the camera's current position
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        spawnPosition += mainCamera.transform.position;
        spawnPosition.z = 0; // Ensure the z-coordinate is 0

        return spawnPosition;
    }

    #endregion
}