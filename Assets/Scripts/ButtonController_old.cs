using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController_old : InteractiveBehaviour {

    // Enable/disable MeshRenderer when selected
    public override void Select()
    {
        MeshRenderer myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = !myRenderer.enabled;
    }
}
