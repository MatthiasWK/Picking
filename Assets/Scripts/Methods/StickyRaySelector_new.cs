using UnityEngine;
using MMK.Inp;

public class StickyRaySelector_new : RaySelector_Click_new
{
    private LineRenderer stickyRay;

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
        type = "Interactive";
        dir = Vector3.forward;
        sticky = true;
        stickyRay = GetComponent<LineRenderer>();
        stickyRay.enabled = false;
    }

    public override void Update()
    {
        base.Update();

        // Reset selection
        if (MMKClusterInputManager.GetButtonDown("Btn_Return") && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Contact(false);
            target = null;
            stickyRay.enabled = false;
        }

        // Once Ray hits an object draw a seperate sticky ray that stays connected to target 
        if (target != null)
        {
            stickyRay.enabled = true;
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (stickyRay != null)
        {
            stickyRay.enabled = false;
        }

    }
}