using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : MonoBehaviour {

	public float rotSpeed = 10;

	void Update () {
		this.transform.Rotate(0, Time.deltaTime * -rotSpeed , 0);
	}
}
