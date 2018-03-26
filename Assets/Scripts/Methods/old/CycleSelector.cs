using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class CycleSelector : MonoBehaviour
{

    private int numTouching;
    private List<Collider> touching;
    private Collider target;
    private OutlineReset resetScript;

    public bool resizable;

    private void Start()
    {
        resetScript = transform.parent.GetComponent<OutlineReset>();
        numTouching = 0;
        touching = new List<Collider>();
    }

    private void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;

        numTouching = 0;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Add(other);
            numTouching++;
            UpdateTarget();
        }
    }

    // Update touching objects
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            if (other == target)
            {
                other.gameObject.GetComponent<InteractiveBehaviour>().Contact(true);
            }
            else
            {
                other.gameObject.GetComponent<InteractiveBehaviour>().Contact(true, numTouching.Equals(1));
            }           
        }

    }

    // Reset objects on exit
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Remove(other);
            numTouching--;
            other.gameObject.GetComponent<InteractiveBehaviour>().Contact(false);
            UpdateTarget();
        }
    }

    private void Update()
    {
        // If touching only one object, execute its Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute object's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

        if (MMKClusterInputManager.GetButtonDown("Btn_Return") && numTouching > 1)
        {
            int i = touching.IndexOf(target);
            int next = (i + 1) % numTouching;
            target = touching[next];
        }

            // Control size with mouse wheel if Volume is resizable
            if (resizable && MMKClusterInputManager.GetButtonDown("Btn_ScaleUp") && transform.localScale.x < 2f && GetComponent<MeshRenderer>().enabled)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localPosition += new Vector3(0, 0, 0.05f);
        }

        if (resizable && MMKClusterInputManager.GetButtonDown("Btn_ScaleDown") && transform.localScale.x > 0.1f && GetComponent<MeshRenderer>().enabled)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            transform.localPosition -= new Vector3(0, 0, 0.05f);
        }

    }

    private void UpdateTarget()
    {
        if(numTouching > 0)
        {
            if(target == null || (target != null && !touching.Contains(target)))
            {
                target = touching[0];
            }
        }
    }

    // Reset on disable
    public void OnDisable()
    {
        if (resetScript != null)
        {
            resetScript.Reset();
        }

        if (touching != null)
        {
            touching.Clear();
        }

        target = null;
        numTouching = 0;

        if (resizable)
        {
            transform.localScale = Vector3.one;
            transform.localPosition = new Vector3(0, 0, 0.5f);
        }

    }
}
