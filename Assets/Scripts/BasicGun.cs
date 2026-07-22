using UnityEngine;
using System.Collections;

public class BasicGun : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSound;

    [Header("Settings")]
    public float fireRate = 8f;
    public float bulletSpeed = 100f;
    public int magazineSize = 30;
    public float reloadTime = 2f;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    void Start()
    {
        currentAmmo = AmmoCounter.instance.boltCurrentValue;

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        currentAmmo = AmmoCounter.instance.boltCurrentValue;
        if (isReloading)
            return;

        if (Input.GetButton("Fire1"))
        {
            //// Automatically reload if empty
            //if (currentAmmo <= 0)
            //{
            //    StartCoroutine(Reload());
            //    return;
            //}

            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (AmmoCounter.instance.boltCurrentValue <= 0) 
        {
            return;
        }
        AmmoCounter.instance.DecreaseSilverBolts(1);

        if (muzzleFlash)
            muzzleFlash.Play();

        if (gunSound)
            gunSound.Play();

        Vector3 direction = playerCamera.transform.forward;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = direction * bulletSpeed;
    }

    IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        isReloading = true;

        // Play reload animation/sound here

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
    }
}