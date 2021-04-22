using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    [SerializeField] Camera playerCamera;


    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        maxAmmunation = ((GunInfo)itemInfo).ammunation;
        ammunation = maxAmmunation;

    }
    public override void Reload()
    {
        ammunation = maxAmmunation;
    }

    public override void Use()
    {
        Shoot();

    }
    void Shoot()
    {
        if(ammunation > 0)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = playerCamera.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                PV.RPC("RPCShot", RpcTarget.All, hit.point, hit.normal);
            }
            PV.RPC("PlayShootSound", RpcTarget.All);
            ammunation--;
        }
        else
        {
            Reload();
        }
    }

    [PunRPC]
    void RPCShot(Vector3 hitposition, Vector3 normal)
    {
        GameObject bulletImpact = Instantiate(bulletImpactPrefab, hitposition + normal * 0.001f, Quaternion.LookRotation(normal, Vector3.up)* bulletImpactPrefab.transform.rotation);
        Destroy(bulletImpact, 5f);

    }
    [PunRPC]
    void PlayShootSound()
    {
        shootSound.Play();
    }

}
