using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera playerCamera;
    public override void Use()
    {
        Shoot();
    }
    void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = playerCamera.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Wee hit" + hit.collider.gameObject.name);
        }
    }
}
