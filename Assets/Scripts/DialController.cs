using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialController : InteractiveBehaviour {
    private Vector3 rot;
    private string dialName;
    private int value;

    private void Awake()
    {
        rot = new Vector3(0,36,0);

        if(transform.parent != null)
        {
            dialName = transform.parent.name;
        }

        value = -1;
    }

    // Rotate Dial right
    protected override void OnSelect()
    {
        transform.Rotate(rot);
    }

    // Rotate Dial left
    protected override void OnAltSelect()
    {
        transform.Rotate(-rot);
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            value = (value + 1) % 10;

            if(tag == "Interactive")
            {
                print(dialName + " changed to " + value);
            }

            transform.hasChanged = false;
        }
    }

}
