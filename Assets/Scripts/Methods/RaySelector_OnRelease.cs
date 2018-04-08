using UnityEngine;
using MMK.Inp;

public class RaySelector_OnRelease : Ray_Base
{
    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(false);
        type = "Interactive";
        dir = Vector3.forward;
        sticky = false;
    }

    public override void Update()
    {
        // while any select button is pressed down, activate ray
        if (MMKClusterInputManager.GetButton("Btn_Select") || MMKClusterInputManager.GetButton("Btn_AltSelect"))
        {
            transform.Find("RayVisual").gameObject.SetActive(true);

            base.Update();

        }
        // Execute target's Select function on release
        if (MMKClusterInputManager.GetButtonUp("Btn_Select"))
        {
            transform.Find("RayVisual").gameObject.SetActive(false);
            if (target != null)
            {
                target.GetComponent<InteractiveBehaviour>().Contact(false);
                target.GetComponent<InteractiveBehaviour>().Select();
            }

            target = null;
        }

        // Execute target's alternate Select function on release
        if (MMKClusterInputManager.GetButtonUp("Btn_AltSelect"))
        {
            transform.Find("RayVisual").gameObject.SetActive(false);
            if (target != null)
            {
                target.GetComponent<InteractiveBehaviour>().Contact(false);
                target.GetComponent<InteractiveBehaviour>().AltSelect();
            }

            target = null;
        }


    }
}