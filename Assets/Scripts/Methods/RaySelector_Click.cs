using UnityEngine;
using MMK.Inp;

public class RaySelector_Click : Ray_Base
{
    public bool sticky = false;

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
        type = "Interactive";
        dir = Vector3.forward;
    }

    // Update is called once per frame
    public virtual void Update()
    {

        // Debug Raycast
        Vector3 forward = transform.TransformDirection(dir) * 50;
        //Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit = new RaycastHit(); ;
        Ray myRay = new Ray(transform.position, forward);
        if (Physics.Raycast(myRay, out hit))
        {
            //print(hit);
            if (hit.collider.gameObject.CompareTag(type)) // Check if object is Interactable
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
            else if (!sticky && target != null) // If touching something else, reset previous target
            {
                target.GetComponent<InteractiveBehaviour>().Contact(false);
                target = null;
            }
        }
        else if (!sticky && target != null) // If not touching anything, reset previous target
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
}