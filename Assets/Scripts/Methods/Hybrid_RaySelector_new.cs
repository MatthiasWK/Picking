using UnityEngine;
using MMK.Inp;

public class Hybrid_RaySelector_new : RaySelector_Click_new
{
    public GameObject[] clones;

    private void OnEnable()
    {
        clones = GameObject.FindGameObjectsWithTag("Clone");
        transform.Find("RayVisual").gameObject.SetActive(true);
        type = "Clone";
        dir = Vector3.forward;
    }

    public override void Update()
    {
        base.Update();

        // Go back to Volume Selector on Shift or clicking when not touching anything
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
        foreach (GameObject clone in clones)
        {
            if (clone != null)
            {
                Destroy(clone.transform.parent.gameObject);
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        DestroyClones();
    }
}