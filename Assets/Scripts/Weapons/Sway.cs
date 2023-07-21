using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    //singleton
    public static Sway Instance { private set; get; }

    //new Sway
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    //Variables
    public float intensity;
    public float smooth;

    //
    private Vector2 mDeltaLook;
    private Vector3 inital_position;
    private Quaternion origin_rotation;

    //Callbacks
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        origin_rotation = transform.localRotation;
        inital_position = transform.localPosition;
    }

    private void Update()
    {
        UpdateSway();
    }

    //Private Methods
    private void UpdateSway()
    {
        float t_x_mouse = mDeltaLook.x;
        float t_y_mouse = mDeltaLook.y;

        //ROTATION
        Quaternion t_x_adjustment = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adjustment = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);

        Quaternion target_rotation = origin_rotation * t_x_adjustment * t_y_adjustment;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);

        //POSITION
        float moveX = Mathf.Clamp(t_x_mouse * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(t_y_mouse * amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + inital_position, Time.deltaTime * smoothAmount);
    }

    //Public Methods
    public void setDeltaLook(Vector2 deltalook)
    {
        mDeltaLook = deltalook;
    }
}
