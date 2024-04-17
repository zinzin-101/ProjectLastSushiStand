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

    public void SetCurrentAmmo(int amount)
    {
        currentAmmoCount = amount;
    }
}
