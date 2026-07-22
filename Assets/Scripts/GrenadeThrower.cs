using UnityEngine;
using System.Collections;

public class GrenadeThrower : MonoBehaviour
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
        currentAmmo = AmmoCounter.instance.grenadeCurrentValue;

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        currentAmmo = AmmoCounter.instance.grenadeCurrentValue;
        if (isReloading)
            return;

        if (Input.GetButton("Fire2"))
        {
            Debug.Log("Throwing Grenade");

            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (AmmoCounter.instance.grenadeCurrentValue <= 0)
        {
            return;
        }
        AmmoCounter.instance.DecreaseGrenades(1);

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
            rb.AddForce(direction * bulletSpeed);
    }
}