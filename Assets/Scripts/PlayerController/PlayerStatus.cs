using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] int maxPlayerHealth = 10;
    public int playerHealth;

    void Start()
    {
        playerHealth = maxPlayerHealth;
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
}
