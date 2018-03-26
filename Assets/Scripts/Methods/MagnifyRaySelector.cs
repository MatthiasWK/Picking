using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;


public class MagnifyRaySelector : MonoBehaviour
{
    
    private OutlineReset resetScript;
    private GameObject target = null;
    private GameObject clone = null;

    public GameObject container;

    private void Start()
    {
        resetScript = transform.parent.GetComponent<OutlineReset>();
    }

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        // Debug Raycast
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 50;

        RaycastHit hit = new RaycastHit(); ;
        Ray myRay = new Ray(transform.position, forward);
        if (Physics.Raycast(myRay, out hit))
        {
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
                    //if (clone == null)
                    //{
                        CloneTarget();
                    //}
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
            //print("blub");
            //Destroy(clone);
        }

        if(target == null && clone != null)
        {
            Destroy(clone);
        }

        // If target is selected, execute its Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Select();
            SelectClone(false);
        }

        // Execute target's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
            SelectClone(true);
        }

    }

    private void CloneTarget()
    {
        Vector3 clonePos = new Vector3(0, 0, 0);
        Quaternion cloneRot = new Quaternion();

        clone = Instantiate(target.transform.parent.gameObject, container.transform);
        clone.transform.localPosition = clonePos;
        clone.transform.rotation = cloneRot;

        foreach (Transform child in clone.transform)
        {
            if (child.tag == "Interactive")
            {
                child.tag = "Clone";
                child.GetComponent<InteractiveBehaviour>().original = target;
                child.GetComponent<InteractiveBehaviour>().Contact(false);
            }
        }
    }

    private void SelectClone(bool alt)
    {
        foreach (Transform child in clone.transform)
        {
            if (child.tag == "Clone")
            {
                if (alt)
                {
                    child.GetComponent<InteractiveBehaviour>().AltSelect();
                }
                else
                {
                    child.GetComponent<InteractiveBehaviour>().Select();
                }
            }
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
        Destroy(clone);
    }
}
