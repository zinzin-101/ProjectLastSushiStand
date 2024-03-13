using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus; //might combine GunRayCast into PlayerStatus class
    [SerializeField] TMP_Text healthText;

    [SerializeField] GunRayCast gunScript;
    [SerializeField] TMP_Text ammoText;

    private void Update()
    {
        switch (gunScript.IsReloading)
        {
            case true:
                ammoText.text = "Ammo: Reloading /" + gunScript.MaxAmmo;
                break;
            case false:
                ammoText.text = "Ammo: " + gunScript.AmmoCount + "/" + gunScript.MaxAmmo;
                break;
        }

        healthText.text = "Health: " + playerStatus.playerHealth;
    }
}
