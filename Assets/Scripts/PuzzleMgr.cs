using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using JsonFx.Json;

public class PuzzleMgr : MonoBehaviour {

	public GameObject theEnd;
	public Text txtScore;
	public Image timeBar;
	float timeLeft = 60;
	public Color[] _color = new Color[3];
	WarningScript camEdge;
	int score = 0;
	int[] arrayPuzzle;
	GameObject[] objPuzzle;
	public int width = 5;
	public int height = 5;
	string puzzleName;
	int max = 63;
	int beforeNum=0;
	int afterNum = 0;
	float delayTime = 0.5f;
	bool change = false;
	bool selectPuzzle = false;

	UserInfo userInfo;

	void Start () {
		theEnd.SetActive (false);
		camEdge = GameObject.Find ("Main Camera").GetComponent<WarningScript> ();
		camEdge.enabled = false;

		userInfo = new UserInfo();
		if (PlayerPrefs.GetInt ("JsonOn") != 0) {
			string jsonUserInfo = PlayerPrefs.GetString ("JsonUserInfo");
			userInfo = JsonReader.Deserialize<UserInfo> (jsonUserInfo);
			//score = userInfo.currentScore;
		}
		PlayerPrefs.SetInt ("JsonOn", 1);

		max = width * 14;
		arrayPuzzle = new int[width * height];
		objPuzzle = new GameObject[width * height];
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				int ran = Random.Range (0, 100)%6;
				switch (ran) {
				case 0:
					puzzleName = "p_red";
					break;
				case 1:
					puzzleName = "p_green";
					break;
				case 2:
					puzzleName = "p_blue";
					break;
				case 3:
					puzzleName = "p_yellow";
					break;
				case 4:
					puzzleName = "p_pupple";
					break;
				case 5:
					puzzleName = "p_pink";
					break;
				}
				int ran2 = Random.Range (0, 100);
				if (ran2 == 0) {
					ran = 6;
					puzzleName = "p_item1";
				} else if(ran2 == 1) {
					ran = 7;
					puzzleName = "p_item2";
				}
				else if(ran2 == 2) {
					ran = 8;
					puzzleName = "p_block";
				}
				objPuzzle [i*width+j] = Instantiate (Resources.Load ("puzzle/" + puzzleName)) as GameObject;
				objPuzzle [i*width+j].transform.position = new Vector3 (j - (width - 1) * 0.5f, i - 3, 0);
				//objPuzzle [i * width + j].transform.rotation = Quaternion.Euler (0, 180, Random.Range (-180f, 180f));
				arrayPuzzle [i*width+j] = ran;
				objPuzzle [i * width + j].name = (i * width + j).ToString();
				objPuzzle [i * width + j].GetComponent<PuzzleCtrl> ().myKind = ran;
				objPuzzle [i * width + j].SetActive (true);
			}

		}
//		string deg = "";
//		for (int i = height; i >0; i--) {
//			for (int j = width; j >0; j--) {
//				deg += arrayPuzzle [i*width-j].ToString()+" ";
//			}
//			deg += '\n';
//		}
//		Debug.Log (deg);
	}

	void FixedUpdate () {

		timeLeft -= Time.deltaTime;
		timeBar.fillAmount = timeLeft / 60.0f;
		if (timeBar.fillAmount >= 0.5f) {
			timeBar.color = _color [0];
		} else if (timeBar.fillAmount >= 0.2f) {
			timeBar.color = _color [1];
		} else if(timeBar.fillAmount>0) {
			timeBar.color = _color [2];
			camEdge.enabled = true;
		}

		if (timeBar.fillAmount <= 0) {
			theEnd.SetActive (true);
			camEdge._edge.enabled = false;
			camEdge.enabled = false;
			//SceneManager.LoadSceneAsync ("scLobby");
			//Debug.Log ("Game Over!!");
		}

		userInfo.currentScore = score;
		if (userInfo.maxScore < score) {
			userInfo.maxScore = score;
		}
		string jsonUserInfo = JsonWriter.Serialize(userInfo);
		PlayerPrefs.SetString("JsonUserInfo", jsonUserInfo);

		txtScore.text = score.ToString();

		if (delayTime < 0.25f) {
			delayTime += Time.deltaTime;
			return;
		} else {

			bool isPuzzle = GetPuzzle ();
			if (change == true && !isPuzzle) {
				change = false;
				int temp = arrayPuzzle [beforeNum];
				arrayPuzzle [beforeNum] = arrayPuzzle [afterNum];
				arrayPuzzle [afterNum] = temp;

				GameObject tempObj = objPuzzle [beforeNum];
				Vector3 before = objPuzzle [beforeNum].transform.position;
				Vector3 after = objPuzzle [afterNum].transform.position;
				objPuzzle [beforeNum] = objPuzzle [afterNum];
				objPuzzle [afterNum] = tempObj;
				objPuzzle [beforeNum].GetComponent<PuzzleCtrl> ().nextPos = before;
				objPuzzle [afterNum].GetComponent<PuzzleCtrl> ().nextPos = after;
				delayTime = 0;
				return;
			} else if (isPuzzle) {
				change = false;
				delayTime = 0;
				return;
			} else {
				change = false;
			}
		}

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.CompareTag ("Puzzle")) {
					selectPuzzle = true;
					int y = (int)hit.collider.transform.position.y + 3;
					int x = (int)hit.collider.transform.position.x + (int)((width - 1) * 0.5f);
					beforeNum = y * width + x;
				}
				if (hit.collider.CompareTag ("Item")) {
					selectPuzzle = false;
					int y = (int)hit.collider.transform.position.y + 3;
					int x = (int)hit.collider.transform.position.x + (int)((width - 1) * 0.5f);
					for (int i = 0; i < width; i++) {
						if (arrayPuzzle [y * width + i] == 7) {
							for (int j = 0; j < 14; j++) {
								if (arrayPuzzle [j * width + i] == 6) {
									for (int k = 0; k < width; k++) {
										objPuzzle [j * width + k].GetComponent<RotationItems> ().enabled = true;
										score += 1;
									}
								}
								objPuzzle [j * width + i].GetComponent<RotationItems> ().enabled = true;
								score += 1;
							}
						}
						objPuzzle [y * width + i].GetComponent<RotationItems> ().enabled = true;
						score += 1;
					}
					beforeNum = y * width + x;
				}
				if (hit.collider.CompareTag ("Item2")) {
					selectPuzzle = false;
					int y = (int)hit.collider.transform.position.y + 3;
					int x = (int)hit.collider.transform.position.x + (int)((width - 1) * 0.5f);
					for (int i = 0; i < 14; i++) {
						if (arrayPuzzle [i * width + x] == 6) {
							for (int j = 0; j < width; j++) {
								if (arrayPuzzle [j * width + i] == 7) {
									for (int k = 0; k < 14; k++) {
										objPuzzle [k * width + j].GetComponent<RotationItems> ().enabled = true;
										score += 1;
									}
								}
								objPuzzle [i * width + j].GetComponent<RotationItems> ().enabled = true;
								score += 1;
							}
						}
						objPuzzle [i * width + x].GetComponent<RotationItems> ().enabled = true;
						score += 1;
					}
					beforeNum = y * width + x;
				}
			}
		}
		if (Input.GetMouseButton (0)) {
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.CompareTag ("Puzzle")||hit.collider.CompareTag ("Item")||hit.collider.CompareTag ("Item2")) {
						int y = (int)hit.collider.transform.position.y + 3;
						int x = (int)hit.collider.transform.position.x + (int)((width - 1) * 0.5f);
						afterNum = y * width + x;
					if (beforeNum != afterNum && change == false && selectPuzzle == true &&
					    (beforeNum == afterNum + 1 || beforeNum == afterNum - 1 || beforeNum == afterNum + width || beforeNum == afterNum - width)) {

						change = true;
						selectPuzzle = false;
						int temp = arrayPuzzle [beforeNum];
						arrayPuzzle [beforeNum] = arrayPuzzle [afterNum];
						arrayPuzzle [afterNum] = temp;

						GameObject tempObj = objPuzzle [beforeNum];
						Vector3 before = objPuzzle [beforeNum].transform.position;
						Vector3 after = objPuzzle [afterNum].transform.position;
						objPuzzle [beforeNum] = objPuzzle [afterNum];
						objPuzzle [afterNum] = tempObj;
						objPuzzle [beforeNum].GetComponent<PuzzleCtrl> ().nextPos = before;
						objPuzzle [afterNum].GetComponent<PuzzleCtrl> ().nextPos = after;
						delayTime = 0;
					}
				}
			}
		}
			
	}

	private bool GetPuzzle()
	{
		bool judgment = false;
		for (int i = 0; i < max; i++) {
			if (i - 1 < 0 || i + 1 >= max || i % width == 0 || i % width == width - 1 || arrayPuzzle [i] == 9)
				continue;
			if (arrayPuzzle [i] == arrayPuzzle [i - 1] && arrayPuzzle [i] == arrayPuzzle [i + 1]) {
//				objPuzzle [i].SetActive (false);
//				objPuzzle [i - 1].SetActive (false);
//				objPuzzle [i + 1].SetActive (false);
				objPuzzle [i].GetComponent<RotationItems>().enabled = true;
				objPuzzle [i-1].GetComponent<RotationItems>().enabled = true;
				objPuzzle [i+1].GetComponent<RotationItems>().enabled = true;
				judgment = true;
				score += 1;
			}
		}
		for (int i = 0; i < max; i++) {
			if (i - width < 0 || i + width >= max || arrayPuzzle [i] == 9)
				continue;

			if (arrayPuzzle [i] == arrayPuzzle [i - width] && arrayPuzzle [i] == arrayPuzzle [i + width]) {
//				objPuzzle [i].SetActive (false);
//				objPuzzle [i - width].SetActive (false);
//				objPuzzle [i + width].SetActive (false);
				objPuzzle [i].GetComponent<RotationItems>().enabled = true;
				objPuzzle [i-width].GetComponent<RotationItems>().enabled = true;
				objPuzzle [i+width].GetComponent<RotationItems>().enabled = true;
				judgment = true;
				score += 1;
			}
		}
		for (int i = 0; i < arrayPuzzle.Length; i++) {
			if (objPuzzle [i].activeSelf == false) {
				arrayPuzzle [i] = 9;
			}
		}
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (arrayPuzzle [i * width + j] != 9) {
					int count = 0;
					for (int k = i * width + j - width; k >= 0; k -= width) {
						if (arrayPuzzle [k] == 9) {
							count++;
						} else {
							break;
						}
					}
					if (count != 0) {
						int temp = arrayPuzzle [i * width + j];
						arrayPuzzle [i * width + j] = arrayPuzzle [i * width + j - width * count];
						arrayPuzzle [i * width + j - width * count] = temp;

						GameObject tempObj = objPuzzle[i * width + j - width * count];
						objPuzzle [i * width + j - width * count] = objPuzzle [i * width + j];
						objPuzzle [i * width + j] = tempObj;
						objPuzzle [i * width + j].transform.position = new Vector3 (j - (width - 1) * 0.5f, i - 3, 0);
						objPuzzle [i * width + j - width * count].GetComponent<PuzzleCtrl> ().nextPos = new Vector3 (j - (width - 1) * 0.5f, (i - count) - 3, 0);
					}
				}
			}
		}
//		string deg = "";
//		for (int i = height; i > 0; i--) {
//			for (int j = width; j > 0; j--) {
//				deg += arrayPuzzle [i * width - j].ToString () + " ";
//			}
//			deg += '\n';
//		}
//		Debug.Log (deg);
		RecyclePuzzle();
		return judgment;
	}
	private void RecyclePuzzle()
	{
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (arrayPuzzle [i * width + j] == 9) {
					arrayPuzzle [i * width + j] = objPuzzle [i * width + j].GetComponent<PuzzleCtrl>().myKind;
					objPuzzle [i * width + j].GetComponent<RotationItems> ().enabled = false;
					objPuzzle [i * width + j].GetComponent<MeshRenderer> ().enabled = false;
					objPuzzle [i * width + j].SetActive (true);
				}
			}
		}
	}
	public void OnLobby()
	{
		StartCoroutine (StartGame ());
	}

	IEnumerator StartGame()
	{
		float loading = 0.0f;
		AsyncOperation async = SceneManager.LoadSceneAsync("scLobby");

		while (async.progress < 0.9f)
		{
			loading = async.progress;
			//imgLoadingScreen.fillAmount = loading;
			//txtLoading.text = (loading * 100).ToString("N0") + "%";
			//Debug.LogWarning(async.progress);
			yield return new WaitForEndOfFrame();
		}
		//imgLoadingScreen.fillAmount = 1;
		//txtLoading.text = "100%";
		yield return new WaitForSeconds(1.0f);
	}
}

