using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : Weapon {
    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode = FireMode.Single;

    public Transform[] projectileSpawns;
    public Projectile projectile;

    public float muzzleVelocity = 35;
    public int burstCount;
    public int projectilesPerMag;
    public float reloadTime = 0.3f;
    public float spreadAmount = 1f;

    public float msBetweenShots = 100f;
    public float minDamagePerShot = 1f;
    public float maxDamagePerShot = 2f;

    float nextShotTime;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(0.05f, 0.2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float kickSettleTime = 0.1f;
    public float recoilSettleTime = 0.1f;

    [Header("Effects")]
    //Todo: Add effects

    public float noiseDistance = 3f;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    public int projectilesRemainingInMag = 0;
    public int extraAmmo = 0;

    bool isReloading = false;


    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    FreeLookCam cam;
    MuzzleFlash muzzleflash;

    public AudioClip audioShoot;
    public AudioClip audioReload;
    AudioSource audioSource;

    void Start()
    {
        shotsRemainingInBurst = burstCount;
        cam = GameObject.FindObjectOfType<FreeLookCam>();
        muzzleflash = GetComponent<MuzzleFlash>();
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        // Animate Recoil

        //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, kickSettleTime);
        //recoilAngle = Mathf.SmoothDamp(recoilAngle, 0f, ref recoilRotSmoothDampVelocity, recoilSettleTime);
        //transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (cam != null)
        {
            //transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, kickSettleTime);
            recoilAngle = Mathf.SmoothDamp(recoilAngle, 0f, ref recoilRotSmoothDampVelocity, recoilSettleTime);
            cam.Recoil(recoilAngle);
        }

    }

    void OnDestroy()
    {
        cam.Recoil(0f);
    }

    void Update()
    {
        if (!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            } else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawns.Length; i++)
            {
                if (projectilesRemainingInMag == 0)
                {
                    break;
                }

                nextShotTime = Time.time + msBetweenShots / 1000f;

                float randomX = weaponController.player.isZoomedIn ? Random.Range(-spreadAmount, spreadAmount)/2 : Random.Range(-spreadAmount, spreadAmount);
                float randomY = weaponController.player.isZoomedIn ? Random.Range(-spreadAmount, spreadAmount) / 2 : Random.Range(-spreadAmount, spreadAmount);
                float randomZ = weaponController.player.isZoomedIn ? Random.Range(-spreadAmount, spreadAmount) / 2 : Random.Range(-spreadAmount, spreadAmount);
                Quaternion spawnRot = projectileSpawns[i].rotation * Quaternion.Euler(randomX, randomY, randomZ);

                Projectile newProjectile = Instantiate(projectile, projectileSpawns[i].position, spawnRot) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
                float bulletDamage = Random.Range(minDamagePerShot, maxDamagePerShot);
                float critValue = Random.Range(0, 100);
                bool isCrit = false;
                if (critValue < critChance)
                {
                    isCrit = true;
                    bulletDamage *= critModifier;
                }
                bulletDamage = Mathf.Round(bulletDamage);
                newProjectile.SetDamage(bulletDamage, isCrit);
            }
            projectilesRemainingInMag--;
            MakeNoise();
            anim.SetTrigger("Attack");
            muzzleflash.Activate();
            TriggerOnUpdateInfo();
            audioSource.PlayOneShot(audioShoot);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
        }
    }

    void MakeNoise()
    {
        EnemyStateManager[] enemies = GameObject.FindObjectsOfType<EnemyStateManager>();
        foreach(EnemyStateManager enemy in enemies)
        {
            enemy.HearSound(transform.position, noiseDistance);
        }
    }

    public override void Reload()
    {
        base.Reload();
        if (!isReloading && projectilesRemainingInMag != projectilesPerMag && extraAmmo > 0)
        {
            isReloading = true;
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        if (audioReload != null) audioSource.PlayOneShot(audioReload);
        if (anim != null) anim.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);

        //Insert ammo in the gun and remove it from extraammo
        int remaining = projectilesRemainingInMag;
        projectilesRemainingInMag = Mathf.Min(remaining + extraAmmo, projectilesPerMag);
        extraAmmo -= projectilesRemainingInMag - remaining;

        yield return null;
        isReloading = false;
        TriggerOnUpdateInfo();
    }

    public override void Aim(Vector3 aimPoint)
    {
        base.Aim(aimPoint);
        if (!isReloading)
        {
            transform.LookAt(aimPoint);
        }
    }

    public override void OnTriggerHold()
    {
        base.OnTriggerHold();
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public override void OnTriggerRelease()
    {
        base.OnTriggerRelease();
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }


    public override string[] GetAmmoInfo()
    {
        return new string[] { projectilesRemainingInMag.ToString(), projectilesPerMag.ToString(), extraAmmo.ToString()};
    }
}
