using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShipProjectile : MonoBehaviour
{
    public int projectileDamage;
    public float projectileSpeed;
    public GameObject destroyEffect;
    public GameObject destroyEffectMulti;
    public bool spawnMultiDestroyEffect;

    public bool isHoming;
    public GameObject target;

    public float lifeTime = 1;
    public PhotonView photonView;
    private bool hitSomething;
    public SphereCollider hitBox;
    public ShipWeaponManager shipWeaponManager;

    void Start()
    {
        //photonView = gameObject.GetComponent<PhotonView>();

        if(photonView)
        {
            //hitBox = gameObject.GetComponent<SphereCollider>();
            hitBox.enabled = false;

            if (photonView.IsMine)
            {
                hitBox.enabled = true;
            }
        }
    }

    void Update()
    {
        ProjectileMovement();
    }

    void ProjectileMovement()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        if (isHoming)
        {
            if (target)
            {
                transform.LookAt(target.transform.position);
            }
            else
            {
                FindTarget();
            }
        }
    }

    void FindTarget()
    {
        if (EnemyShipSpawnManager.spawnedEnemies.Count > 0)
        {
            float dis = 10000;
            foreach (GameObject enemy in EnemyShipSpawnManager.spawnedEnemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < dis)
                {
                    dis = Vector3.Distance(transform.position, enemy.transform.position);
                    target = enemy;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hit enemy or player, deal damage

        if (photonView == null)
        {
            DestroyProjectile();

            if (other.GetComponent<EnemyShipHealth>())
            {
                other.GetComponent<EnemyShipHealth>().TakeDamage(projectileDamage);
            }
            else if (other.GetComponent<PlayerShipHealth>())
            {
                other.GetComponent<PlayerShipHealth>().TakeDamage(Mathf.RoundToInt(projectileDamage / 2));
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                hitSomething = true;

                if (other.GetComponent<PhotonView>() != null && other.GetComponent<PhotonView>() == photonView)
                    return;
                else
                {
                    if (other.GetComponent<PlayerShipHealth>())
                    {
                        //other.GetComponent<PlayerShipHealth>().TakeDamage(Mathf.RoundToInt(projectileDamage / 2));
                        other.GetComponent<PlayerShipHealth>().photonView.RPC("TakeDamage", RpcTarget.AllBuffered, Mathf.RoundToInt(projectileDamage / 2));
                        Debug.Log("Hit Enemy Player");
                    }

                    //photonView.RPC("DestroyProjectile", RpcTarget.All);
                    //PhotonNetwork.Destroy(gameObject);
                    shipWeaponManager.DestroySpecificProjectile(this);
                }
            }
        }
    }

    public IEnumerator ProjectileLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        if(photonView == null)
            DestroyProjectile();
        else
        {
            if (photonView.IsMine)
                shipWeaponManager.DestroySpecificProjectile(this);
        }
    }

    public void DestroyProjectile()
    {
        if (destroyEffect) 
        {
            if (photonView == null)
            {
                GameObject fx = Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(fx, 3);
            }
            else
            {
                if (photonView.IsMine && spawnMultiDestroyEffect) 
                {
                    PhotonNetwork.Instantiate(destroyEffectMulti.name, transform.position, transform.rotation);
                }
                else
                {
                    GameObject fx = Instantiate(destroyEffect, transform.position, transform.rotation);
                    Destroy(fx, 3);
                }
            }
        }

        StopCoroutine("ProjectileLifeTime");
        Destroy(gameObject);
        //if (photonView == null)
        //    Destroy(gameObject);
        //else
        //    PhotonNetwork.Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (photonView && destroyEffect && hitSomething)
        {
            //if (photonView.IsMine && spawnMultiDestroyEffect)
            //{
            //    PhotonNetwork.Instantiate(destroyEffectMulti.name, transform.position, transform.rotation);
            //}
            //else
            //{
            //    GameObject fx = Instantiate(destroyEffect, transform.position, transform.rotation);
            //    Destroy(fx, 3);
            //}

            GameObject fx = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(fx, 3);
        }
    }
}
