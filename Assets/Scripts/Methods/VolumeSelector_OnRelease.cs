using UnityEngine;
using MMK.Inp;

public class VolumeSelector_OnRelease : Volume_Base
{
    public virtual void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public override void Update()
    {
        // Enable Volume while either button pressed
        if (MMKClusterInputManager.GetButton("Btn_Select") || MMKClusterInputManager.GetButton("Btn_AltSelect"))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = true;
        }

        // If touching only one object, execute its Select function
        if (MMKClusterInputManager.GetButtonUp("Btn_Select"))
        {
            if (touching.Count.Equals(1))
            {
                touching[0].gameObject.GetComponent<InteractiveBehaviour>().Select();
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();
        }

        // Execute object's alternate Select function
        if (MMKClusterInputManager.GetButtonUp("Btn_AltSelect"))
        {
            if (touching.Count.Equals(1))
            {
                touching[0].gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();
        }

        base.Update();
    }
}