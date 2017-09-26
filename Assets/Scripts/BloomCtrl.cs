using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class BloomCtrl : MonoBehaviour {

	BloomAndFlares bloom;
	bool re = false;
	void Start () {
		bloom = GetComponent<BloomAndFlares> ();
	}

	void Update () {
		if (bloom.sepBlurSpread < 1 && re == false) {
			bloom.sepBlurSpread += Time.deltaTime*0.5f;
			return;
		}
		re = true;
		bloom.sepBlurSpread -= Time.deltaTime*0.5f;
		if (bloom.sepBlurSpread <= 0.1f) {
			re = false;
		}
	}
}
