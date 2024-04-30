using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 10;
    private int playerHealth;
    public int PlayerHealth => playerHealth;

    private bool isPlayerAlive;
    public bool IsPlayerAlive => isPlayerAlive;

    private SceneIndexManager sceneIndexManager;

    private void Awake()
    {
        sceneIndexManager = FindObjectOfType<SceneIndexManager>();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneIndexManager.SetLastSceneIndex(nextSceneIndex);
    }

    void Start()
    {
        isPlayerAlive = true;

        playerHealth = maxPlayerHealth;
    }

    private void Update()
    {
        if (!isPlayerAlive)
        {
            return;
        }

        if (playerHealth <= 0)
        {
            isPlayerAlive = false;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerHealth--;
        }
    }

    // Method to apply damage to the player
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth); // Ensure health stays within range
    }

    // Method to restore player health
    public void RestoreHealth(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth); // Ensure health stays within range
    }

    public void SetPlayerAlive(bool isAlive)
    {
        isPlayerAlive =isAlive;
    }
}
