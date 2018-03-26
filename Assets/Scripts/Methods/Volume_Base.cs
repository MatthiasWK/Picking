using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class Volume_Base : MonoBehaviour
{
    private OutlineReset resetScript;
    public List<Collider> touching;
    public bool resizable;

    private void Start()
    {
        resetScript = transform.parent.GetComponent<OutlineReset>();
        touching = new List<Collider>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Add(other);
        }
    }

    // Update touching objects
    public virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            other.gameObject.GetComponent<InteractiveBehaviour>().Contact(true, touching.Count.Equals(1));
        }

    }

    // Reset objects on exit
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Remove(other);
            other.gameObject.GetComponent<InteractiveBehaviour>().Contact(false);
        }
    }

    public virtual void Update()
    {

        // Control size if Volume is resizable
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
    public virtual void OnDisable()
    {
        if (resetScript != null)
        {
            resetScript.Reset();
        }

        if (touching != null)
        {
            touching.Clear();
        }

        if (resizable)
        {
            transform.localScale = Vector3.one;
            transform.localPosition = new Vector3(0, 0, 0.5f);
        }

    }
}
