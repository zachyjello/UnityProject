using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public Transform orientation;
    public PlayerDataSO playerData;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        
        yRotation += mouseX;
        xRotation -= mouseY;
        //Clamp the camera to keep it from going upside down, and if it goes to 90, messes with vector3.forward
        xRotation = Mathf.Clamp(xRotation, -89, 89);

        // Rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerData.rotation = transform.rotation;

        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
