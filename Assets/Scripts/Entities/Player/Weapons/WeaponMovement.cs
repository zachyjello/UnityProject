using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float staffToPlayerDistance = 2.0f;

    void Update()
    {
        this.transform.rotation = cameraTransform.rotation;
        //Vector3 position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
        //this.transform.position = Vector3.Normalize(position) * staffToPlayerDistance;
    }
}
