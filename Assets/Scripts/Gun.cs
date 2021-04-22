using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();
    public abstract void Reload();

    public int maxAmmunation;
    public int ammunation;
    public GameObject bulletImpactPrefab;

    public AudioSource shootSound;
}
