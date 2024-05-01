using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus; //might combine GunRayCast into PlayerStatus class
    [SerializeField] TMP_Text healthText;

    [SerializeField] GunScript gunScript;
    [SerializeField] TMP_Text ammoText;

    [SerializeField] UIController UIcontroller;
    [SerializeField] RestartController RestartController;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (GameManager.Instance.IsPaused)
            {
                case true:
                    UIcontroller.Resume();
                    break;

                case false:
                    UIcontroller.Pause();
                    break;
            }
        }

        if(playerStatus.IsPlayerAlive == false)
        {
            RestartController.Active();
        }

        

        switch (gunScript.IsReloading)
        {
            case true:
                ammoText.text = "Ammo: Reloading /" + gunScript.MaxAmmo;
                break;
            case false:
                ammoText.text = "Ammo: " + gunScript.AmmoCount + "/" + gunScript.MaxAmmo;
                break;
        }

        healthText.text = "Health: " + playerStatus.PlayerHealth;

    }

    public void TriggerCritMarker()
    {
        //code to trigger red hit marker
        print("crit"); // for debug
    }

    public void TriggerMarker()
    {
        //code to trigger normal hit marker
        print("hit"); // for debug
    }
}
