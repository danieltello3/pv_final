using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
   public float MaxRotation = 90f;
   public float MinRotation = -90f;

   public void RotateUpDown(float angle)
   {
      Vector3 newRotation = new Vector3(
         transform.rotation.eulerAngles.x + angle,
         transform.rotation.eulerAngles.y,
         transform.rotation.eulerAngles.z);

      newRotation.x = newRotation.x > 180f ? newRotation.x - 360f : newRotation.x;
      newRotation.x = Mathf.Clamp(newRotation.x, MinRotation, MaxRotation);

      transform.rotation = Quaternion.Euler(newRotation);

   }

    private void Start()
    {
        
        
    }

    private void Update()
    {
        if (GameManager.GameIsPaused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
