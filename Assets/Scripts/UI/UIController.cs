using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GunParticle gunParticle; // Corrected variable name to follow C# conventions
    private int currentAmmo;

    [SerializeField] private TextMeshProUGUI AmmoText;

    // Use Start() to ensure that the initialization happens before Update()
    void Start()
    {
        // Assuming GunParticle is a component on the same GameObject as UIController
        gunParticle = GetComponent<GunParticle>();
        /*
        // Check if gunParticle is null to avoid potential issues
        if (gunParticle == null)
        {
            Debug.LogError("GunParticle script not found on the same GameObject as UIController.");
        }
        */
    }
    
    void Update()
    {
        if (gunParticle != null)
        {
            currentAmmo = gunParticle.getAmmo();
            AmmoText.text = "Ammo: " + currentAmmo;
        }
        /*
        else
        {
            // Handle the case where gunParticle is not initialized or found
            Debug.LogError("GunParticle script is not initialized.");
        }
        */
    }
    
}