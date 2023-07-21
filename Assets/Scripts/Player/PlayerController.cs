using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float groundDistance = 0.4f;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject[] Weapons;

    
    public int weaponIndicator;

    private bool isGrounded;
    private Vector3 velocity;
    private Vector2 mDirection;
    private Vector2 mDeltaLook;

    public float gravity = -9.81f;
    private bool isJumping;
    private bool isSprinting;

    private Transform cameraMain;
    private GameObject debugImpactSphere;
    private GameObject bloodObjectParticle;
    private GameObject otherObjectParticle;
    private GameObject rifleObjectParticle;
    private Vector3 weaponPosition;

    [Header("Audio")]
    public AudioSource src;
    public AudioClip Steps, SprintSteps;

    void Start()
    {
        cameraMain = transform.Find("CameraHolder").transform.Find("Main Camera");

        debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
        bloodObjectParticle = Resources.Load<GameObject>("BloodSplat_FX Variant");
        otherObjectParticle = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");
        rifleObjectParticle = Resources.Load<GameObject>("WFX_MF FPS RIFLE2 Variant");

        
        //SwitchWeapons(0);
        //CanvasManager.Instance.UpdateHealth(health);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(isJumping && isGrounded)
        {
            Jump();
            isJumping = false;
        }




        velocity.y += gravity * Time.deltaTime;

        Vector3 move = transform.right * mDirection.x + transform.forward * mDirection.y;
        if(isGrounded && isSprinting)
        {
            Debug.Log("Estoy corriendo");
            controller.Move(speed * 2f * Time.deltaTime * move);
        }
        else
        {
            controller.Move(speed * Time.deltaTime * move);
        }

        controller.Move(velocity * Time.deltaTime);

        transform.Rotate(
         Vector3.up,
         sensitivity * Time.deltaTime * mDeltaLook.x
      );

        cameraMain.GetComponent<CameraMovement>().RotateUpDown(
           -sensitivity * Time.deltaTime * mDeltaLook.y
        );

        // weaponPosition = weaponPosition = Weapons[weaponIndicator].GetComponent<Weapon>().shootBox.transform.position;
    }

    private void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>();
        //PlaySound(Steps);
        
    }

    private void OnLook(InputValue value)
    {
        mDeltaLook = value.Get<Vector2>();

        Sway.Instance.setDeltaLook(mDeltaLook);
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if(!isJumping)
                isJumping = true;
        }
    }

    private void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    //private void OnSwitchWeapon(InputValue value)
    //{
    //    if (value.isPressed)
    //    {
    //        int weapon = weaponIndicator < Weapons.Length - 1 ? weaponIndicator + 1 : 0;
    //        SwitchWeapons(weapon);
    //    }
    //}

    private void OnFire(InputValue value)
    {
        WeaponSystem.Instance.OnShoot(value);
    }

    private void OnReload(InputValue value)
    {
        WeaponSystem.Instance.OnReload(value);
    }

    private void OnAim(InputValue value)
    {
        WeaponSystem.Instance.OnAim(value);
    }

    private void PlaySound(AudioClip clip)
    {
        //src.pitch = UnityEngine.Random.Range(0.8f, 1f);
        src.PlayOneShot(clip);
    }

    private void OnRestart(InputValue value)
    {
        if (value.isPressed) 
        {
            GameManager.Instance.Restart();
        }
    }

    //private void SwitchWeapons(int index)
    //{
    //    for (int i = 0; i < Weapons.Length; i++)
    //    {
    //        Weapons[i].SetActive(false);
    //    }
    //    Weapons[index].SetActive(true);
    //    weaponIndicator = index;
    //    weaponType = Weapons[index].GetComponent<Weapon>().weaponType;
    //}
}
