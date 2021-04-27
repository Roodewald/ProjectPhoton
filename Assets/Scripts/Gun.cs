using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();
    public abstract void Reload();

    
    public int damage;
    public int maxAmmunation;
    public float reloadTime;

    [HideInInspector] public int ammunation;
    public GameObject bulletImpactPrefab;
    public ParticleSystem muzzleFlash;

    public AudioSource shootSound;
}
