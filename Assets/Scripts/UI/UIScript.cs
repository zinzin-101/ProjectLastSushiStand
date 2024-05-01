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
        playerStatus = FindFirstObjectByType<PlayerStatus>();
        gunScript = FindFirstObjectByType<GunScript>();
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

        if (gunScript != null)
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
        }        

        healthText.text = "Health: " + playerStatus.PlayerHealth;

        int minute = Mathf.FloorToInt(timerScript.CurrentTime / 60);
        int second = Mathf.FloorToInt(timerScript.CurrentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}",minute,second);
       
    }

    public void TriggerCritMarker()
    {
        StartCoroutine(CritHit(true));
        //print("crit");
    }

    public void TriggerMarker()
    {
        StartCoroutine(CritHit(false));
        //print("hit");
    }

    IEnumerator CritHit(bool isCrit)
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

    public void TriggerDamageDirection(float angle)
    {
        print("angle = " + angle);
        StartCoroutine(DamageIndicator(angle));
    }

    IEnumerator DamageIndicator(float angle)
    {
        //CODE -- show indicator that rotate with angle
        yield return new WaitForSeconds(0.75f); // can modify to any
        //CODE -- hide indicator
    }
}
