using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playerMovement : MonoBehaviour {

    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;
    float turning = 0.0f;
    float vertical = 0.0f;
    float curSpeed;
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);
        turning = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (vertical > 0)
        {
            if (turning > 0)
            {
                forward = forward + right;
            }
            else if (turning < 0)
            {
                forward = forward + left;
            }
            curSpeed = speed * vertical;
            controller.SimpleMove(forward * curSpeed);
        }
        else if (vertical < 0)
        {
            if (turning > 0)
            {
                forward = forward + left;
            }
            else if (turning < 0)
            {
                forward = forward + right;
            }
            curSpeed = speed * vertical;
            controller.SimpleMove(forward * curSpeed);
        }
        else {
            if (turning > 0)
            {
                forward = right;
                curSpeed = speed * turning;
                controller.SimpleMove(forward * curSpeed);
            }
            else if (turning < 0)
            {
                forward = left;
                curSpeed = speed * turning;
                controller.SimpleMove(forward * -curSpeed);
            }
            


        }
    }
}


