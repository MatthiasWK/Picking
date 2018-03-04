using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class InteractiveBehaviour : MonoBehaviour {
    public Outline myOutline;
    public GameObject original = null;

    private void Start()
    {
        myOutline = GetComponent<Outline>();
    }

    // Enable/disable outline
    public void Contact(bool isTouching)
    {
        if(myOutline != null)
        {
            myOutline.color = 1;

            myOutline.eraseRenderer = !isTouching;
        }
    }

    // Enable/disable outline, and change color based on wether touching selectable object
    public void Contact(bool isTouching, bool selectable)
    {
        if (selectable)
            myOutline.color = 1;
        else
            myOutline.color = 0;

        myOutline.eraseRenderer = !isTouching;
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
}
