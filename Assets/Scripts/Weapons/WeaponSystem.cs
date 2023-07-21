using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.HID;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance { private set; get; }
    //Weapon Stats
    public int damage, megazineSize, bulletsPerTap;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShoots;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask target;
    public ParticleSystem shootPS;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public int totalBullets;

    //Recoil
    [Header("Recoil System")]

    public Transform recoilPosition;
    public Transform rotationPoint;

    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    [Space(10)]
    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;
    [Space(10)]
    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f,0f,-0.2f);
    [Space(10)]
    public Vector3 RecoilRotationAim = new Vector3(10, 4, 6);
    public Vector3 RecoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);
    [Space(10)]
    

    Vector3 rotationalRecoil;
    Vector3 positionalRecoil;
    Vector3 Rot;
  
    //aiming
    public bool aiming;
    private Vector3 originalPosition;
    public Vector3 aimPosition;
    
    public float adsSpeed = 8f;

    private float distance;

    public AudioSource audioS;


    private void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);
    }


    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        string text = bulletsLeft + " / " + totalBullets;
        CanvasManager.Instance.UpdateBullets(text);
        AimDownSight();
    }

    private void Awake()
    {
        Instance = this;
        bulletsLeft = megazineSize;
        totalBullets = megazineSize * 4;
        readyToShoot = true;
    }

    public void OnReload(InputValue value)
    {
        if (value.isPressed && bulletsLeft < megazineSize && !reloading)
            Reload();
    }

    public void OnShoot(InputValue value)
    {
        if (value.isPressed && readyToShoot && !GameManager.GameIsPaused && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    public void OnAim(InputValue value)
    {
        if (value.isPressed)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        //Calculate Direction with spread
        Vector3 direction = fpsCam.transform.forward + (aiming ? new Vector3(0,0,0) : new Vector3(x, y, 0));

        audioS.Play();

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Target"))
            {
                var target = rayHit.collider.GetComponent<TargetController>();
                distance = Vector3.Distance(rayHit.point, transform.localPosition);
                target.TakeHit();
                target.ManagePoints(distance);
            }
        }

        //ShakeCamera
        StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        //Graphics
        var bullet = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        GameObject.Destroy(bullet, 0.5f);
        
        shootPS.Play();

        //recoil
        if (aiming)
        {
            rotationalRecoil += new Vector3(-RecoilRotationAim.x, UnityEngine.Random.Range(-RecoilRotationAim.y, RecoilRotationAim.y), UnityEngine.Random.Range(-RecoilRotationAim.z, RecoilRotationAim.z));
            positionalRecoil += new Vector3(UnityEngine.Random.Range(-RecoilKickBackAim.x, RecoilKickBackAim.x), UnityEngine.Random.Range(-RecoilKickBackAim.y, RecoilKickBackAim.y), RecoilKickBackAim.z);
        }
        else
        {
            rotationalRecoil += new Vector3(-RecoilRotation.x, UnityEngine.Random.Range(-RecoilRotation.y, RecoilRotation.y), UnityEngine.Random.Range(-RecoilRotation.z, RecoilRotation.z));
            positionalRecoil += new Vector3(UnityEngine.Random.Range(-RecoilKickBack.x, RecoilKickBack.x), UnityEngine.Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
        }

        bulletsLeft--;
        bulletsShot--;
        Invoke(nameof(ResetShoot), timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShoots);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        if (totalBullets > 0)
            Invoke(nameof(ReloadFinished), reloadTime);
        else
            reloading = false;
    }

    private void ReloadFinished()
    {
        int bulletsToRecharge = megazineSize - bulletsLeft;

        bulletsLeft = megazineSize > totalBullets ? megazineSize : bulletsLeft + bulletsToRecharge;

        if (totalBullets - bulletsToRecharge > 0)
            totalBullets -= bulletsToRecharge;
        else
            totalBullets = 0;
            
        reloading = false;
        if(totalBullets == 0 && bulletsLeft == 0)
        {
            GameManager.Instance.StopGame();
        }
    }

    private void AimDownSight()
    {
        if (aiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
        }
    }

}
