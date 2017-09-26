using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCtrl : MonoBehaviour {

	Vector3 pos;
	public Vector3 nextPos;
	private Vector3 tempPos;
	float ttime = 0;
	bool moving = false;
	public int myKind = 0;
	void Start () {
		pos = this.transform.position;
		nextPos = pos;
	}
	void OnEnable()
	{
		this.transform.rotation = Quaternion.Euler (0, 180, 0);
		StartCoroutine (Recycle ());
	}
	void Update () {
		if (pos != nextPos && moving == false) {
			moving = true;
			tempPos = nextPos;
			ttime = 0;
		} 
		if(moving == true)
		{
			ttime += Time.deltaTime*5;
			this.transform.position = Vector3.Lerp (pos, nextPos, ttime);
			if (ttime >= 1f) {
				moving = false;
				pos = this.transform.position;
				tempPos = nextPos;
			}
		}
	}
	IEnumerator Recycle()
	{
		yield return new WaitForSeconds (3);
		this.GetComponent<MeshRenderer> ().enabled = true;
	}
}
