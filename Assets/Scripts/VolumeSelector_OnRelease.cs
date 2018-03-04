using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSelector_OnRelease : MonoBehaviour {

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
        //transform.rotation = Quaternion.Lerp(transform.rotation, hand.transform.rotation, Time.deltaTime * 100);
        //transform.position = Vector3.Lerp(transform.position, hand.transform.position, Time.deltaTime * 100) + offset;

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }

        // If touching only one object, execute its Select function
        if (Input.GetMouseButtonUp(1))
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
        if (Input.GetMouseButtonUp(2))
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
        if (resizable && Input.GetAxis("Mouse ScrollWheel") > 0f && transform.localScale.x < 2f)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localPosition += new Vector3(0, 0, 0.05f);
        }

        if (resizable && Input.GetAxis("Mouse ScrollWheel") < 0f && transform.localScale.x > 0.1f)
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
