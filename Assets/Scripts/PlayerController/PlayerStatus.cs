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
    private UIScript ui;

    private void Awake()
    {
        sceneIndexManager = FindObjectOfType<SceneIndexManager>();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneIndexManager.SetLastSceneIndex(nextSceneIndex);

        ui = FindFirstObjectByType<UIScript>();
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
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    playerHealth--;
        //}
    }

    // Method to apply damage to the player
    public void TakeDamage(int damage, Vector3 sourcePos)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth); // Ensure health stays within range
        
        Vector3 direction = (sourcePos - transform.position).normalized;
        direction.y = 0f;
        float angle = Vector3.Angle(direction, transform.forward);
        Vector3 cross = Vector3.Cross(direction, transform.forward);

        if (cross.y > 0f)
        {
            angle = 360f - angle;
        }

        ui.TriggerDamageDirection(angle);
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
