using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class WarningScript : MonoBehaviour {

	public EdgeDetection _edge;
	bool re = false;
	void Start () {
		_edge = GetComponent<EdgeDetection> ();
	}

	void Update () {
		if (_edge.edgesOnly <= 0.4 && re == false) {
			_edge.edgesOnly += Time.deltaTime*0.5f;
			return;
		}
		re = true;
		_edge.edgesOnly -= Time.deltaTime*0.5f;
		if (_edge.edgesOnly <= 0.1f) {
			re = false;
		}
	}

}
