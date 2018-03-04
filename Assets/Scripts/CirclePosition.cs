using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePosition : MonoBehaviour {

    public GameObject hand;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Circle moves with the hand and always faces the camera
    void Update () {
        transform.position = hand.transform.position;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, hand.transform.eulerAngles.z);
    }
}
