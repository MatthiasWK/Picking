using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour {
    public float rotSpeed = 100f;
    public float movSpeed = 10f;
	
	// Translation
	void Update () {
        if (transform.gameObject.tag == "Hand_1")
        {
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.S))
                transform.Translate(-Vector3.forward * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
                transform.Translate(-Vector3.right * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Space))
                transform.Translate(Vector3.up * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.X))
                transform.Translate(-Vector3.up * movSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8))
                transform.Translate(Vector3.forward * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Keypad5))
                transform.Translate(-Vector3.forward * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Keypad6))
                transform.Translate(Vector3.right * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Keypad4))
                transform.Translate(-Vector3.right * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.KeypadDivide))
                transform.Translate(Vector3.up * movSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Keypad2))
                transform.Translate(-Vector3.up * movSpeed * Time.deltaTime);
        }

    }

    // Rotation
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX);
        transform.Rotate(Vector3.right, rotY);

    }
}
