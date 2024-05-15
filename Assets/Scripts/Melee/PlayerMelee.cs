using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] KeyCode meleeKey = KeyCode.F;
    [SerializeField] GameObject meleeObj;
    [SerializeField] Transform meleeTransform;
    


    [SerializeField] float meleeDelay;
    private bool canMelee;
    private float delayLeft;

    private void Start()
    {
        canMelee = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(meleeKey) && canMelee)
        {
            canMelee = false;
            Instantiate(meleeObj, meleeTransform);
            SoundManager.PlaySound(SoundManager.Sound.Melee);
            
        }

        if (!canMelee)
        {
            delayLeft -= Time.deltaTime;
            if (delayLeft <= 0)
            {
                canMelee = true;
            }
        }
        else
        {
            delayLeft = meleeDelay;
        }
    }
}