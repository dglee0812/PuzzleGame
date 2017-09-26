using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JsonFx.Json;

public class SceneLoading : MonoBehaviour {

	public Text txtLoading;
	public GameObject loadingScreen;
	UserInfo userInfo;

	public void OnGameStart()
	{
		StartCoroutine (StartGame ());
	}

	IEnumerator StartGame()
	{
		float loading = 0.0f;
		AsyncOperation async = SceneManager.LoadSceneAsync("scPlay");
		txtLoading.gameObject.SetActive (true);
		loadingScreen.SetActive(false);
		while (async.progress < 0.9f)
		{
			loading = async.progress;
			//imgLoadingScreen.fillAmount = loading;
			txtLoading.text = (loading * 100).ToString("N0") + "%";
			//Debug.LogWarning(async.progress);
			yield return new WaitForEndOfFrame();
		}
		//imgLoadingScreen.fillAmount = 1;
		txtLoading.text = "100%";
		yield return new WaitForSeconds(1.0f);
	}
}
