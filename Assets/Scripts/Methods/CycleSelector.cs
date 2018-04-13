using UnityEngine;
using MMK.Inp;

public class CycleSelector : VolumeSelector_Click
{
    private Collider target;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Add(other);
            UpdateTarget();
        }
    }

    // Update touching objects
    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            if (other == target)
            {
                other.gameObject.GetComponent<InteractiveBehaviour>().Contact(true);
            }
            else
            {
                other.gameObject.GetComponent<InteractiveBehaviour>().Contact(true, false);
            }
        }

    }

    // Reset objects on exit
    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive") && enabled)
        {
            touching.Remove(other);
            other.gameObject.GetComponent<InteractiveBehaviour>().Contact(false);
            UpdateTarget();
        }
    }

    public override void Update()
    {
        base.Update();

        // If touching multiple objects, execute target's Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_Select") && touching.Count > 1)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().Select();
        }

        // Execute target's alternate Select function
        if (MMKClusterInputManager.GetButtonDown("Btn_AltSelect") && touching.Count > 1)
        {
            target.gameObject.GetComponent<InteractiveBehaviour>().AltSelect();
        }

        // change the current target to the next object in the list
        if (MMKClusterInputManager.GetButtonDown("Btn_Return") && touching.Count > 1)
        {
            int i = touching.IndexOf(target);
            int next = (i + 1) % touching.Count;
            target = touching[next];
        }
    }

    // If no target set or no longer touching target, set first Object in List as new target
    private void UpdateTarget()
    {
        if (touching.Count > 0)
        {
            if (target == null || (target != null && !touching.Contains(target)))
            {
                target = touching[0];
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        target = null;
    }
}