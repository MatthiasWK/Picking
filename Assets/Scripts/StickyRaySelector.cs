using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class StickyRaySelector : MonoBehaviour
{

    //private GameObject[] pickUps;
    private OutlineReset resetScript;
    private GameObject target = null;
    private LineRenderer stickyRay;

    //public GameObject hand;

    private void Start()
    {
        //pickUps = GameObject.FindGameObjectsWithTag("Interactive");
        //resetScript = GameObject.Find("Buttons").GetComponent<ButtonReset>();
        resetScript = transform.parent.GetComponent<OutlineReset>();
        stickyRay = GetComponent<LineRenderer>();
        stickyRay.enabled = false;
    }

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        // Debug Raycast
        Vector3 forward = transform.TransformDirection(Vector3.forward);
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
        }

        // Once Ray hits an object draw a seperate sticky ray that stays connected to target 
        if (target != null)
        {
            stickyRay.enabled = true;
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
        }
        

        // If target is selected, execute its Select function
        if (Input.GetMouseButtonDown(1) && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute target's alternate Select function
        if (Input.GetMouseButtonDown(2) && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

        // Reset selection
        if (Input.GetKeyDown(KeyCode.LeftShift) && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Contact(false);
            target = null;
            stickyRay.enabled = false;
        }

    }

    // Reset on disable
    private void OnDisable()
    {

        if (resetScript != null)
        {
            resetScript.Reset();
        }

        target = null;
        if (stickyRay != null)
        {
            stickyRay.enabled = false;
        }
        
    }
}
