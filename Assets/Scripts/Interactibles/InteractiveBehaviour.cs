using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBehaviour : MonoBehaviour {
    public GameObject original = null;
    
    public Material[] states;
    MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = true;
        rend.material = states[0];
    }

    // Enable/disable outline
    public void Contact(bool isTouching)
    {
        if (isTouching)
        {
            SetMat(1);
        }
        else
        {
            SetMat(0);
        }
    }

    // Enable/disable outline, and change color based on wether touching selectable object
    public void Contact(bool isTouching, bool selectable)
    {

        if (isTouching)
        {
            if (selectable)
            {
                SetMat(1);
            }
            else
            {
                SetMat(2);
            }
        }
        else
        {
            SetMat(0);
        }
    }

    private void SetMat(int s)
    {
        if (rend != null)
        {
            rend.material = states[s];
        }
    }

    // Push button or increase lever position and dial value
    public virtual void Select()
    {
        OnSelect();

        // If clone, interact with original
        if (original != null)
        {
            original.GetComponent<InteractiveBehaviour>().Select();
        }
    }

    // Decrease lever position and dial value
    public virtual void AltSelect()
    {
        OnAltSelect();

        // If clone, interact with original
        if (original != null)
        {
            original.GetComponent<InteractiveBehaviour>().AltSelect();
        }
    }

    protected virtual void OnSelect()
    {

    }

    protected virtual void OnAltSelect()
    {

    }

    // Only for levers to reset their position
    public virtual void ResetPos()
    {

    }
}
