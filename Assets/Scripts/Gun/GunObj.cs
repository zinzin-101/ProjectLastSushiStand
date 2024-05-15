using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunObj
{
    public GunInfo GunInfo;
    public GameObject GunModel;
    public Transform GunPos;
    private int currentAmmoCount;
    public int CurrentAmmoCount => currentAmmoCount;

    private Animator animator;

    public void InitAnimator()
    {
        animator = GunModel.GetComponent<Animator>();
    }

    public void SetCurrentAmmo(int amount)
    {
        currentAmmoCount = amount;
    }

    public void TriggerFireAnim()
    {
        animator.SetTrigger("Fire");
    }

    public void TriggerReloadStart()
    {
        animator.SetTrigger("ReloadStart");
    }

    public void TriggerReloadEnd()
    {
        animator.SetTrigger("ReloadEnd");
    }
}
