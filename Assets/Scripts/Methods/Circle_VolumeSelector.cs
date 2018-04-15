using UnityEngine;
using UnityEditor;
using MMK.Inp;

public class Circle_VolumeSelector : Hybrid_VolumeSelector
{

    public override void Clone()
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
                    child.GetComponent<InteractiveBehaviour>().ResetPos();
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