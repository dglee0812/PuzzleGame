using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationItems : MonoBehaviour {

    public float rotSpeed = 10;
	void OnEnable()
	{
		StartCoroutine ("end");
	}
	void Update () {
		this.transform.Rotate(0, Time.deltaTime * -rotSpeed , 0);
	}
	IEnumerator end()
	{
		yield return new WaitForSeconds (0.5f);
		gameObject.SetActive (false);
	}
}
