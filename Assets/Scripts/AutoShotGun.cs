using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AutoShotGun : Gun
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Animator anim;
    bool reloading = false;
    bool isFire;
    public float shotCounter;
    public float fireRate = 0.1f;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        ammunation = maxAmmunation;
    }
    public override void Reload()
    {
        if (ammunation < maxAmmunation)
        {
            StartCoroutine(TimerReload());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            isFire = true;
        else if (Input.GetMouseButtonUp(0))
            isFire = false;
        if (isFire)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = fireRate;
                Shoot();
            }
        }
        else
            shotCounter -= Time.deltaTime;
    }
    public override void Use()
    {
        
        
    }

    void Shoot()
    {    
        if(ammunation > 0 && !reloading)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = playerCamera.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage);
                PV.RPC("RPCShot", RpcTarget.All, hit.point, hit.normal);
            }
            PV.RPC("PlayEffects", RpcTarget.All);
            ammunation--;
        }
    }
    IEnumerator TimerReload()
    {
        reloading = true;
        anim.SetBool("Reloading", reloading);

        yield return new WaitForSeconds(1.0f);

        ammunation = maxAmmunation;
        reloading = false;
        anim.SetBool("Reloading", reloading);
    }

    [PunRPC]
    void RPCShot(Vector3 hitposition, Vector3 normal)
    {
        GameObject bulletImpact = Instantiate(bulletImpactPrefab, hitposition + normal * 0.001f, Quaternion.LookRotation(normal, Vector3.up)* bulletImpactPrefab.transform.rotation);
        Destroy(bulletImpact, 5f);

    }
    [PunRPC]
    void PlayEffects()
    {
        shootSound.Play();
        muzzleFlash.Play();
    }
}
