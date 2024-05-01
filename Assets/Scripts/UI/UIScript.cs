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

    [SerializeField] TimerScript timerScript;
    [SerializeField] TMP_Text timerText;

    [SerializeField] UIController UIcontroller;
    [SerializeField] RestartController RestartController;

    [SerializeField] GameObject critMarker;
    [SerializeField] GameObject hitMarker;

    private void Awake()
    {
        timerScript = FindFirstObjectByType<TimerScript>();
    }
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

        int minute = Mathf.FloorToInt(timerScript.CurrentTime / 60);
        int second = Mathf.FloorToInt(timerScript.CurrentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}",minute,second);
       
    }

    public void TriggerCritMarker()
    {
        StartCoroutine(critHit(true));
        print("crit"); // for debug
    }

    public void TriggerMarker()
    {
        StartCoroutine(critHit(false));
        print("hit"); // for debug
    }

    IEnumerator critHit(bool isCrit)
    {
        if(isCrit)
        {
            critMarker.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            critMarker.SetActive(false);
        }else
        {
            hitMarker.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            hitMarker.SetActive(false);
        }
        
    }
}
