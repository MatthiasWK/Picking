using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class CirclePosition : MonoBehaviour {

    public GameObject hand;
    public GameObject user;


    // Circle moves with the hand and always faces the user
    void Update () {
        transform.position = hand.transform.position;
        transform.rotation = Quaternion.LookRotation(transform.position - user.transform.position);
    }
}
