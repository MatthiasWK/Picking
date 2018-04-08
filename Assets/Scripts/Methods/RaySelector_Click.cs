using UnityEngine;
using MMK.Inp;

public class RaySelector_Click : Ray_Base
{

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
        type = "Interactive";
        dir = Vector3.forward;
        sticky = false;
    }

    // Update is called once per frame
    public override void Update()
    {

        base.Update();

        // If target is selected, execute its Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && target != null)
        {
            target.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute target's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && target != null)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

    }
}