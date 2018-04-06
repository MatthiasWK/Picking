using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class RaySelector_Click_old : MonoBehaviour
{

    //private GameObject[] pickUps;
    private OutlineReset resetScript;
    private GameObject target = null;

    //public GameObject hand;

    private void Start()
    {
        //pickUps = GameObject.FindGameObjectsWithTag("Interactive");
        //resetScript = GameObject.Find("Buttons").GetComponent<ButtonReset>();
        resetScript = transform.parent.GetComponent<OutlineReset>();
    }

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        //transform.rotation = Quaternion.Lerp(transform.rotation, hand.transform.rotation, Time.deltaTime * 100);
        //transform.position = Vector3.Lerp(transform.position, hand.transform.position, Time.deltaTime * 100);

        // Debug Raycast
        Vector3 forward = transform.TransformDirection(Vector3.forward) *50;
        //Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit = new RaycastHit(); ;
        Ray myRay = new Ray(transform.position, forward);
        if (Physics.Raycast(myRay, out hit))
        {
            //print(hit);
            if (hit.collider.gameObject.CompareTag("Interactive")) // Check if object is Interactable
            {
                if (target != hit.collider.gameObject) // Check if already touching hit object
                {
                    if (target != null) // Check if touched any object previously
                    {
                        target.GetComponent<InteractiveBehaviour>().Contact(false); // Reset previous target
                    }

                    target = hit.collider.gameObject; // Make hit object my target
                    target.GetComponent<InteractiveBehaviour>().Contact(true); // Activate outline
                }
            }
            else if (target != null) // If touching something else, reset previous target
            {
                target.GetComponent<InteractiveBehaviour>().Contact(false);
                target = null;
            }
        }
        else if (target != null) // If not touching anything, reset previous target
        {
            target.GetComponent<InteractiveBehaviour>().Contact(false);
            target = null;
        }

        // If target is selected, execute its Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute target's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

    }

    // Reset on disable
    private void OnDisable()
    {

        if (resetScript != null)
        {
            resetScript.Reset();
        }

        //if (pickUps != null)
        //{
        //    foreach (GameObject obj in pickUps)
        //    {
        //        if (obj != null)
        //        {
        //            obj.GetComponent<InteractiveBehaviour>().Contact(false);
        //        }
        //    }
        //}


        target = null;
    }
}
