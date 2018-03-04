using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class Hybrid_RaySelector : MonoBehaviour
{
    
    private OutlineReset resetScript;
    private GameObject target = null;
    private GameObject[] clones;

    private void Start()
    {
        resetScript = transform.parent.parent.GetComponent<OutlineReset>();       
    }

    private void OnEnable()
    {
        clones = GameObject.FindGameObjectsWithTag("Clone");
        transform.Find("RayVisual").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        // Debug Raycast
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 50;
        //Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit = new RaycastHit(); ;
        Ray myRay = new Ray(transform.position, forward);
        if (Physics.Raycast(myRay, out hit))
        {
            //print(hit);
            if (hit.collider.gameObject.CompareTag("Clone")) // Check if object is a Clone of an Interactable
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
        if (Input.GetMouseButtonDown(1) && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute target's alternate Select function
        if (Input.GetMouseButtonDown(2) && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

        // Go back to Volume Selector on Shift or clicking when not touching anything
        if (Input.GetKeyDown(KeyCode.LeftShift) || (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && target == null)
        {
            transform.GetComponentInParent<MeshRenderer>().enabled = true;
            transform.GetComponentInParent<Collider>().enabled = true;
            DestroyClones();
            gameObject.SetActive(false);
        }

    }

    // Destroy all clones
    private void DestroyClones()
    {
        foreach(GameObject clone in clones)
        {
            Destroy(clone.transform.parent.gameObject);
        }
    }

    // Reset on disable
    private void OnDisable()
    {

        if (resetScript != null)
        {
            resetScript.Reset();
        }

        DestroyClones();

        target = null;
    }
}
