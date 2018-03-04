using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : InteractiveBehaviour {

	
	// Update is called once per frame
	void Update () {
        // Rotate
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
	}

}
