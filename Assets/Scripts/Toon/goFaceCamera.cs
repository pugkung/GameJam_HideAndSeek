using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goFaceCamera : MonoBehaviour {


	void Update () {
		var newRotation = Quaternion.Slerp(this.gameObject.transform.rotation,Quaternion.LookRotation(Camera.main.gameObject.transform.position - this.gameObject.transform.position), 10*Time.deltaTime).eulerAngles;
		newRotation.x = 0;
		newRotation.z = 0;
		this.gameObject.transform.rotation = Quaternion.Euler(newRotation);
	}
}
