using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0, 0, 0.5f);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
	}
	
}
