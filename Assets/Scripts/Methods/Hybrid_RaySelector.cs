using UnityEngine;
using MMK.Inp;

public class Hybrid_RaySelector : RaySelector_Click
{
    public GameObject container;

    private void OnEnable()
    {
        transform.Find("RayVisual").gameObject.SetActive(true);
        type = "Clone";
        dir = Vector3.forward;
        sticky = false;
    }

    public override void Update()
    {
        base.Update();

        // Go back to Volume Selector by pressing "Btn_Return" or selecting nothing
        if (MMKClusterInputManager.GetButtonDown("Btn_Return") || (MMKClusterInputManager.GetButtonDown("Btn_Select") || MMKClusterInputManager.GetButtonDown("Btn_AltSelect")) && target == null)
        {
            transform.GetComponentInParent<MeshRenderer>().enabled = true;
            transform.GetComponentInParent<Collider>().enabled = true;
            DestroyClones();
            gameObject.SetActive(false);
        }
    }

    // Destroy all clones
    private void DestroyClones()
    {
        foreach (Transform clone in container.transform)
        {
            if (clone != null)
            {
                Destroy(clone.gameObject);
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        DestroyClones();
    }
}