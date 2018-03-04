using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MethodSwitcher : MonoBehaviour {



    private GameObject[] methods;
    private int numMethods;
    private int currentMethod = 0;

    public Text methodText;

    private MonoBehaviour[] sMethods;
    private int numSMethods;
    private int currentSMethod = 0;

    public Text sMethodText;

    private void Start()
    {

        methods = GameObject.FindGameObjectsWithTag("PickingMethod");
        numMethods = methods.Length;
        print (numMethods);
        //deactivate all methods
        foreach (GameObject m in methods)
        {
            // print(m);
            m.SetActive(false);
        }
            
        //activate first method
        SwitchActive(currentMethod);
    }

    // Switching
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchActive();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && sMethods.Length > 1)
        {
            SwitchActiveSelection();
        }

    }

    // Cycle through methods
    private void SwitchActive()
    {
        methods[currentMethod].SetActive(false);
        currentMethod = (currentMethod + 1) % numMethods;
        methods[currentMethod].SetActive(true);

        ResetSelection();

        methodText.text = methods[currentMethod].name;
    }

    // Activate specific method
    private void SwitchActive(int active)
    {
        methods[currentMethod].SetActive(false);
        currentMethod = active;
        methods[currentMethod].SetActive(true);

        ResetSelection();

        methodText.text = methods[currentMethod].name;
    }

    // Cycle through selection methods
    private void SwitchActiveSelection()
    {
        
        sMethods[currentSMethod].enabled = false;
        currentSMethod = (currentSMethod + 1) % numSMethods;
        sMethods[currentSMethod].enabled = true;

        string name = sMethods[currentSMethod].ToString();
        //string expr = ".*\\_";
        sMethodText.text = name;//Regex.Replace(name, expr, "");
        //print(name);
        
    }

    // Activate specific selection method
    private void SwitchActiveSelection(int active)
    {

        sMethods[currentSMethod].enabled = false;
        currentSMethod = active;
        sMethods[currentSMethod].enabled = true;

        string name = sMethods[currentSMethod].ToString();
        //string expr = ".*\\_";
        sMethodText.text = name;//Regex.Replace(name, expr, "");
                                //print(name);

    }

    // Prime selection methods when choosing picking method
    private void ResetSelection()
    {
        currentSMethod = 0;
        sMethods = methods[currentMethod].GetComponents<MonoBehaviour>();
        numSMethods = sMethods.Length;

        if (numSMethods > 1)
        {
            // Deactivate all methods
            foreach (MonoBehaviour m in sMethods)
            {
                m.enabled = false;
            }
            // Activate first method
            SwitchActiveSelection(currentSMethod);
        }
        else
        {
            sMethodText.text = sMethods[0].ToString();
        }

    }
}

