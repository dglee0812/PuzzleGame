using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JsonFx.Json;

public class GameInfoMgr: MonoBehaviour
{
	public Text txtMaxScore;
	UserInfo userInfo;
	void Start()
	{
		userInfo = new UserInfo();
		if (PlayerPrefs.GetInt ("JsonOn") != 0) {
			string jsonUserInfo = PlayerPrefs.GetString ("JsonUserInfo");
			userInfo = JsonReader.Deserialize<UserInfo> (jsonUserInfo);
			txtMaxScore.text = "BEST SCORE\n"+userInfo.maxScore.ToString ();
		} else {
			txtMaxScore.text = "BEST SCORE\n0";
		}
	}
}