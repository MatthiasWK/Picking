﻿using UnityEngine;
using MMK.Inp;

public class Hybrid_VolumeSelector : VolumeSelector_Click
{
    public GameObject container;

    public override void OnEnable()
    {
        base.OnEnable();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        // If touching multiple objects clone them and display infront of camera, then switch to ray 
        if ((/*MMKClusterInputManager.GetButtonDown("Btn_Select") || MMKClusterInputManager.GetButtonDown("Btn_AltSelect")*/Input.GetKeyDown(KeyCode.Mouse1)|| Input.GetKeyDown(KeyCode.Mouse2)) && touching.Count > 1)
        {
            Clone();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();

            transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    // Creates a clone of each object in touching and arranges the clones in a Grid
    public virtual void Clone()
    {
        float offsInterval = 0.3f;
        float offset = Mathf.Min(touching.Count, 10) * -0.5f * offsInterval;
        Vector3 clonePos = new Vector3(0, 0, 0);
        Quaternion cloneRot = new Quaternion();

        int i = 0;
        foreach (Collider orig in touching)
        {
            Transform cloneParent = Instantiate(orig.transform.parent, container.transform);
            cloneParent.localPosition = clonePos + new Vector3(offset, 0, 0);
            cloneParent.rotation = cloneRot;

            foreach (Transform child in cloneParent)
            {
                if (child.tag == "Interactive")
                {
                    child.tag = "Clone";
                    child.GetComponent<InteractiveBehaviour>().original = orig.gameObject;
                    child.GetComponent<InteractiveBehaviour>().Contact(false);
                    child.GetComponent<InteractiveBehaviour>().ResetPos();
                }
            }

            if(i < 9)
            {
                offset += offsInterval;
                i++;
            }
            else
            {
                offset = Mathf.Min(touching.Count, 10) * -0.5f * offsInterval;
                clonePos += new Vector3(0, -0.3f, 0);
                i = 0;
            }
           
        }
    }
}