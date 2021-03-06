using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    [Header("Player Stats")]
    public ShipWeaponStats weapon;

    public Transform shootPoint;

    public bool canFire = true;

    public GameObject shootFx;

    [Header("Enemy Stats")]
    public bool isEnemy;
    public float turnSpeed = 10;
    public EnemyShipAI shipAI;

    private Transform player;
    private GameObject target;

    public AudioSource audioSource;
    public AudioClip[] shootSounds;

    // Start is called before the first frame update
    void Start()
    {
        if (isEnemy)
        {
            player = GameObject.Find("Player").transform;
        }

        if(isEnemy && shipAI)
        {
            shipAI.weapons.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemy)
            WeaponAiming();
        else
        {
            EnemyAiming();

            if (shipAI.isTriggered)
                EnemyShooting();
        }
    }

    void WeaponAiming()
    {
        Vector3 lookPoint = new Vector3();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, ~((1 << 8) | (1 << 12) | (1 << 14))))
        {
            if (hit.collider != null)
            {
                lookPoint = hit.point;
                target = hit.collider.gameObject;
            }
            else
            {
                //Plane playerPlane = new Plane(Vector3.up, transform.position);
                //Ray ray2 = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                //float hitDist = 1000;

                //if (playerPlane.Raycast(ray2, out hitDist))
                //{
                //    lookPoint = ray2.GetPoint(hitDist);
                //}

                RaycastHit hit2;
                Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hit, 1000, ~(1 << 10));
                if (hit.collider != null)
                {
                    //lookPoint = hit.point;
                }

                lookPoint = hit.point;
                //lookPoint = ray.origin * ray.direction.y * 1000;
                //lookPoint = transform.parent.forward;
            }
        }

        transform.LookAt(lookPoint);
    }

    void EnemyShooting()
    {
        if (canFire)
        {
            Shoot();

            canFire = false;
            StartCoroutine("ShootCoolDown");
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(weapon.projectilePrefab, shootPoint.position, shootPoint.rotation);
        ShipProjectile projectile = bullet.GetComponent<ShipProjectile>();

        if (projectile != null)
        {
            projectile.projectileSpeed = weapon.projectileSpeed;
            projectile.projectileDamage = weapon.damage;

            if (target)
            {
                projectile.target = target;
            }
        }

        if(audioSource && shootSounds.Length > 0)
        {
            PlayRandSound.PlayRandomSound(audioSource, shootSounds);
        }

        bullet.layer = 12;

        Destroy(bullet, weapon.projectileLifeTime);
    }

    public IEnumerator FireParticleFx()
    {
        if (shootFx)
        {
            int rand = Random.Range(0, 360);
            shootFx.transform.Rotate(shootFx.transform.forward * rand);
        }

        yield return new WaitForSeconds(0.2f);

        if (shootFx)
            shootFx.SetActive(false);
    }

    void EnemyAiming()
    {
        Vector3 targetDir = player.position - shootPoint.position;
        targetDir = targetDir.normalized;

        Vector3 currentDir = shootPoint.forward;

        currentDir = Vector3.RotateTowards(currentDir, targetDir, turnSpeed * Time.deltaTime, 1.0f);

        Quaternion qDir = new Quaternion();
        qDir.SetLookRotation(currentDir, Vector3.up);
        shootPoint.rotation = qDir;
    }

    IEnumerator ShootCoolDown()
    {
        yield return new WaitForSeconds(weapon.fireRate);

        canFire = true;
    }
}
