﻿using UnityEngine;
using MMK.Inp;

public class VolumeSelector_Click_new : Volume_Base
{
    public virtual void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public override void Update()
    {
        // If touching only one object, execute its Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && touching.Count.Equals(1))
        {
            touching[0].gameObject.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute object's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && touching.Count.Equals(1))
        {
            touching[0].gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

        base.Update();
    }
}