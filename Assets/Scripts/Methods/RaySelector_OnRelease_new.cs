using UnityEngine;
using MMK.Inp;

public class RaySelector_OnRelease_new : Ray_Base
{
    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(false);
        type = "Interactive";
    }

    public virtual void Update()
    {
        if (MMKClusterInputManager.GetButton("Btn_Select") || MMKClusterInputManager.GetButton("Btn_AltSelect"))
        {
            transform.Find("RayVisual").gameObject.SetActive(true);
            // Debug Raycast
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 50;
            //Debug.DrawRay(transform.position, forward, Color.green);

            RaycastHit hit = new RaycastHit(); ;
            Ray myRay = new Ray(transform.position, forward);
            if (Physics.Raycast(myRay, out hit))
            {
                //print(hit);
                if (hit.collider.gameObject.CompareTag(type)) // Check if object is Interactable
                {
                    if (target != null) // Check if touched any object previously
                    {
                        target.GetComponent<InteractiveBehaviour>().Contact(false); // Reset previous target
                    }

                    target = hit.collider.gameObject; // Make hit object my target
                    target.GetComponent<InteractiveBehaviour>().Contact(true); // Activate outline
                    
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

        }
        // Execute target's Select function on release
        if (MMKClusterInputManager.GetButtonUp("Btn_Select"))
        {
            transform.Find("RayVisual").gameObject.SetActive(false);
            if (target != null)
            {
                target.GetComponent<InteractiveBehaviour>().Contact(false);
                target.GetComponent<InteractiveBehaviour>().Select();
            }

        }

        // Execute target's alternate Select function on release
        if (MMKClusterInputManager.GetButtonUp("Btn_AltSelect"))
        {
            transform.Find("RayVisual").gameObject.SetActive(false);
            if (target != null)
            {
                target.GetComponent<InteractiveBehaviour>().Contact(false);
                target.GetComponent<InteractiveBehaviour>().AltSelect();
            }

        }


    }
}