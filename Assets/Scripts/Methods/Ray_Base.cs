using UnityEngine;
using MMK.Inp;

public class Ray_Base : MonoBehaviour
{
    private OutlineReset resetScript;
    public GameObject target = null;
    public string type;
    public Vector3 dir;

    private void Start()
    {
        resetScript = transform.parent.GetComponent<OutlineReset>();
    }

    // Reset on disable
    public virtual void OnDisable()
    {
        if (resetScript != null)
        {
            resetScript.Reset();
        }

        target = null;
    }
}