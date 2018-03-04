using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : InteractiveBehaviour {
    private Animator anim;
    private string buttonName;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        if (transform.parent != null)
        {
            buttonName = transform.parent.name;
        }
    }

    // Play ButtonPress Animation when selected
    protected override void OnSelect()
    {
        anim.Play("ButtonPress");
    }

    protected override void OnAltSelect()
    {
        anim.Play("ButtonPress");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Switch" && tag == "Interactive")
        {
            print(buttonName + " pressed");
        }
    }

}
