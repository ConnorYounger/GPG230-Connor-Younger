using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W8ShipMovement : MonoBehaviour
{
    public float cruseSpeed = 10;
    public float turnSpeed = 10;
    public Transform shipDirection;
    public Transform shipRAW;

    float xRotation = 0;
    float yRotation = 0;

    void Start()
    {
        
    }

    void Update()
    {
        ShipMovement();
    }

    void ShipMovement()
    {
        transform.position += shipDirection.forward * cruseSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;

        xRotation += mouseX;
        yRotation -= mouseY;

        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0);

        //shipDirection.Rotate(shipDirection.forward.normalized * Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed);
        //shipDirection.Rotate(shipDirection.right.normalized * Input.GetAxis("Vertical") * Time.deltaTime * turnSpeed);
    }
}
