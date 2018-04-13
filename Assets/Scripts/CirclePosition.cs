using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class CirclePosition : MonoBehaviour {

    public GameObject hand;
    public GameObject user;
    //private Camera cam;

    //private void Start()
    //{
    //    cam = Camera.main;
    //}

    // Circle moves with the hand and always faces the user
    void Update () {
        transform.position = hand.transform.position;
        //transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        transform.rotation = Quaternion.LookRotation(transform.position - user.transform.position);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, hand.transform.eulerAngles.z);
    }
}
