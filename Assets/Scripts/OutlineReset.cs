using UnityEngine;
using System.Collections;

public class OutlineReset : MonoBehaviour
{
    private GameObject[] objects;
    // Use this for initialization
    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Interactive");
    }

    // Reset all Interactibles
    public void Reset()
    {
        if(objects != null)
        {
            foreach (GameObject obj in objects)
            {
                if (obj != null)
                {
                    obj.GetComponent<InteractiveBehaviour>().Contact(false);
                }          
            }
        }
      
    }
}
