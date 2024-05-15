using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunInfo : ScriptableObject
{
    public enum GunType
    {
        Handgun,
        Autogun
    }

    public GunType type;
    public int damage;
    public float critMultiplier;
    public int range;
    public int maxAmmo;
    public float reloadDelay;
    public float firerate;
    public float spreadAmount;
    public SoundManager.Sound sound;

    //public ParticleSystem shootingSystem;
    //public ParticleSystem ImpactParticleSystem;
    //public TrailRenderer bulletTrail;
}
