using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MMK.Inp;

public class Circle_VolumeSelector_old : MonoBehaviour
{

    private int numTouching;
    private List<Collider> touching;
    private OutlineReset resetScript;
    public GameObject container;

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

        transform.GetChild(0).gameObject.SetActive(false);
    }

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
        // If touching only one object, execute its Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && numTouching.Equals(1))
        {
            touching[0].gameObject.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute object's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && numTouching.Equals(1))
        {
            touching[0].gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

        // If touching multiple objects clone them and display around hand then switch to ray
        if ((MMKClusterInputManager.GetButtonDown("Btn_Select") || MMKClusterInputManager.GetButtonDown("Btn_AltSelect")) && numTouching > 1)
        {
            CloneCircle();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();

            transform.GetChild(0).gameObject.SetActive(true);
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

    private void CloneCircle()
    {
        float radius = 0.5f;
        int i = 0;
        foreach (Collider orig in touching)
        {
            int a = 360 / numTouching * i;
            i++;
            Vector3 pos = RandomCircle(radius, a);
            Transform cloneParent = Instantiate(orig.transform.parent, container.transform);
            cloneParent.localPosition = pos;
            cloneParent.rotation = cloneParent.parent.rotation;

            foreach (Transform child in cloneParent)
            {
                if (child.tag == "Interactive")
                {
                    child.tag = "Clone";
                    child.GetComponent<InteractiveBehaviour>().original = orig.gameObject;
                    child.GetComponent<InteractiveBehaviour>().Contact(false);
                }
            }

        }
    }

    Vector3 RandomCircle(float radius, int a)
    {
        float ang = a;
        Vector3 pos;
        pos.x = radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = 0;
        return pos;
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

        numTouching = 0;

        if (resizable)
        {
            transform.localScale = Vector3.one;
            transform.localPosition = new Vector3(0, 0, 0.5f);
        }

    }
}
