using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class VolumeSelector_OnRelease_old : MonoBehaviour {

    private int numTouching;

    private List<Collider> touching;
    private OutlineReset resetScript;
    //private Vector3 offset;

    public bool resizable;
    //public GameObject hand;
    private void Start()
    {
        resetScript = transform.parent.GetComponent<OutlineReset>();
        numTouching = 0;
        touching = new List<Collider>();
        //offset = transform.position - hand.transform.position;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        numTouching = 0;
    }

    // Increase number of touching objects on enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Add(other);
            numTouching++;
        }
    }

    // Update touching objects
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            other.gameObject.GetComponent<InteractiveBehaviour>().Contact(true, numTouching.Equals(1));
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
        }
    }

    private void Update()
    {
        // Enable Volume while either button pressed
        if (MMKClusterInputManager.GetButton("Btn_Select") || MMKClusterInputManager.GetButton("Btn_AltSelect"))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }

        // If touching only one object, execute its Select function
        if (MMKClusterInputManager.GetButtonUp("Btn_Select"))
        {
            if (numTouching.Equals(1))
            {
                touching[0].gameObject.GetComponent<InteractiveBehaviour>().Select();
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();
        }

        // Execute object's alternate Select function
        if (MMKClusterInputManager.GetButtonUp("Btn_AltSelect"))
        {
            if (numTouching.Equals(1))
            {
                touching[0].gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();
        }

        // Control size with mouse wheel if Volume is resizable
        if (resizable && MMKClusterInputManager.GetButtonDown("Btn_ScaleUp") && transform.localScale.x < 2f)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localPosition += new Vector3(0, 0, 0.05f);
        }

        if (resizable && MMKClusterInputManager.GetButtonDown("Btn_ScaleDown") && transform.localScale.x > 0.1f)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            transform.localPosition -= new Vector3(0, 0, 0.05f);
        }
    }

    // Reset on disable
    private void OnDisable()
    {
        if (resetScript != null)
        {
            resetScript.Reset();
        }

        if (touching != null)
        {
            touching.Clear();
        }

        numTouching = 0;

        if (resizable)
        {
            transform.localScale = Vector3.one;
            transform.localPosition = new Vector3(0, 0, 0.5f);
        }
    }
}
