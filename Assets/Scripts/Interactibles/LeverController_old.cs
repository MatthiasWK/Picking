using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController_old : InteractiveBehaviour {
    private Animator anim;
    private string leverName;
    private string pos = null;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        if (transform.parent != null)
        {
            leverName = transform.parent.name;
        }
    }

    // Increase lever position
    protected override void OnSelect()
    {
        if(pos == "Position_0")
        {
            anim.Play("Lever_0-1");
        }
        else if (pos == "Position_2")
        {
            anim.Play("Lever_2-0");
        }
    }

    // Decrease lever position
    protected override void OnAltSelect()
    {
        if (pos == "Position_1")
        {
            anim.Play("Lever_1-0");
        }
        else if (pos == "Position_0")
        {
            anim.Play("Lever_0-2");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LeverPosition" && transform.parent == other.transform.parent)
        {
            pos = other.gameObject.name;

            if (tag == "Interactive")
            {
                print(leverName + " set to " + pos);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LeverPosition" && transform.parent == other.transform.parent)
        {
            pos = null;
        }
    }
}
