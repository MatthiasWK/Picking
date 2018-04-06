using UnityEngine;
using UnityEditor;
using MMK.Inp;

public class Circle_VolumeSelector : VolumeSelector_Click
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

        // If touching multiple objects clone them and display around hand then switch to ray
        if ((MMKClusterInputManager.GetButtonDown("Btn_Select") || MMKClusterInputManager.GetButtonDown("Btn_AltSelect")) && touching.Count > 1)
        {
            CloneCircle();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            OnDisable();

            transform.GetChild(0).gameObject.SetActive(true);
        }        
    }

    private void CloneCircle()
    {
        float radius = 0.5f;
        int i = 0;
        foreach (Collider orig in touching)
        {
            int a = 360 / touching.Count * i;
            i++;
            Vector3 pos = RandomCircle(radius, a);
            Transform cloneParent = Instantiate(orig.transform.parent, container.transform);
            cloneParent.localPosition = pos;
            cloneParent.rotation = cloneParent.parent.rotation;

            foreach (Transform child in cloneParent)
            {
                if (child.tag == "Interactive")
                {
                    child.tag = "Clone";
                    child.GetComponent<InteractiveBehaviour>().original = orig.gameObject;
                    child.GetComponent<InteractiveBehaviour>().Contact(false);
                }
            }

        }
    }

    Vector3 RandomCircle(float radius, int a)
    {
        float ang = a;
        Vector3 pos;
        pos.x = radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = 0;
        return pos;
    }
}