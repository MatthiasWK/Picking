using UnityEngine;
using MMK.Inp;

public class Ray_Base : MonoBehaviour
{
    private OutlineReset resetScript;
    public GameObject target = null;
    public string type;
    public Vector3 dir;
    public bool sticky;

    private void Start()
    {
        resetScript = transform.parent.GetComponent<OutlineReset>();
    }

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
    }

    // Reset on disable
    public virtual void OnDisable()
    {
        if (resetScript != null)
        {
            resetScript.Reset();
        }

        target = null;
    }
}