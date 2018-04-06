using UnityEngine;
using MMK.Inp;

public class Circle_RaySelector : Hybrid_RaySelector
{
    public GameObject container;
    public GameObject hand;

    private void OnEnable()
    {
        clones = GameObject.FindGameObjectsWithTag("Clone");
        transform.position = container.transform.position;
        type = "Clone";
        dir = Vector3.up;
    }

    public override void Update()
    {
        // Direction of ray depends on z axis of hand
        transform.eulerAngles = new Vector3(container.transform.eulerAngles.x, container.transform.eulerAngles.y, hand.transform.eulerAngles.z);

        base.Update();
    }
}