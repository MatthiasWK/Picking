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

        value = 0;
    }

    // Rotate Dial right
    protected override void OnSelect()
    {
        transform.Rotate(rot);

        value = (value + 1) % 10;

        if (tag == "Interactive")
        {
            print(dialName + " changed to " + value);
        }
    }

    // Rotate Dial left
    protected override void OnAltSelect()
    {
        transform.Rotate(-rot);

        value = (value - 1) % 10;
        if (value < 0) value = 9;

        if (tag == "Interactive")
        {
            print(dialName + " changed to " + value);
        }
    }

}
