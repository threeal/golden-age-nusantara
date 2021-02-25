using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScrIntro : MonoBehaviour {

	public Image panel;
	public Text text;
	public Slider slider;
	public Image sliderBack;
	public Image sliderFill;

	public float speed;
	public string nextScene;

	bool isLoaded = false;

	// Use this for initialization
	void Start () {
		//loading language
		ScrLanguage.Load ();
		if (!PlayerPrefs.HasKey ("SettingLanguage")) {
			if (Application.systemLanguage == SystemLanguage.Indonesian) {
				PlayerPrefs.SetString ("SettingLanguage", "id");
			} else {
				PlayerPrefs.SetString ("SettingLanguage", "en");
			}
		}

		if (PlayerPrefs.GetString ("SettingLanguage") == "en") {
			ScrLanguage.Language = SystemLanguage.English;
		} else if (PlayerPrefs.GetString ("SettingLanguage") == "id") {
			ScrLanguage.Language = SystemLanguage.Indonesian;
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		if (!isLoaded) {
			isLoaded = true;

			float progress;
			if (nextScene != "") {
				progress = 0f;
			} else {
				if (PlayerPrefs.HasKey ("progress")) {
					progress = PlayerPrefs.GetFloat ("progress");
				} else {
					progress = 1f;
				}
			}
			PlayerPrefs.SetFloat ("progress", progress);
			text.text = ScrLanguage.Translate ("ui_loading");
			slider.value = 0.1f;

			StartCoroutine (SceneLoading ());
		}
	}

	IEnumerator SceneLoading () {
		float progress = 0f;
		GameObject objSaveLoad = GameObject.Find ("SaveLoad");



		if (nextScene != "") {

			while (panel.color.a < 1f) {
				SetAlpha (panel.color.a + (speed * Time.deltaTime));
				yield return null;
			}

			//saving for Game scene
			if (SceneManager.GetActiveScene ().name == "Game") {
				if (objSaveLoad) {
					objSaveLoad.GetComponent <ScrSaveLoad> ().Save ();
				}
			}

			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (nextScene);

			while (!asyncLoad.isDone) {
				progress += speed * Time.deltaTime * 0.5f;
				if (progress > asyncLoad.progress) {
					progress = asyncLoad.progress;
				}
				PlayerPrefs.SetFloat ("progress", progress);
				text.text = ScrLanguage.Translate ("ui_loading");
				slider.value = 0.1f + (progress * 0.9f);
				yield return null;
			}

			nextScene = "";

		} else {
			SetAlpha (1f);

			if (PlayerPrefs.HasKey ("progress")) {
				progress = PlayerPrefs.GetFloat ("progress");
			} else {
				progress = 1f;
			}
			text.text = ScrLanguage.Translate ("ui_loading");
			slider.value = 0.1f + (progress * 0.9f);

			//loading for Game scene
			if (SceneManager.GetActiveScene ().name == "Game") {
				if (PlayerPrefs.GetInt ("continue") != 0) {
					if (objSaveLoad) {
						objSaveLoad.GetComponent <ScrSaveLoad> ().Load ();
					}
				}
			}

			while (progress < 1f) {
				progress += speed * Time.deltaTime * 0.5f;
				if (progress > 1f) {
					progress = 1f;
				}
				text.text = ScrLanguage.Translate ("ui_loading");
				slider.value = 0.1f + (progress * 0.9f);
				yield return null;
			}

			while (panel.color.a > 0f) {
				SetAlpha (panel.color.a - (speed * Time.deltaTime));
				yield return null;
			}
		}

		isLoaded = false;
		gameObject.SetActive (false);
	}

	void SetAlpha (float a) {
		if (a > 1f) {
			a = 1f;
		} else if (a < 0f) {
			a = 0f;
		}

		Color col = panel.color;
		col.a = a;
		panel.color = col;

		col = text.color;
		col.a = a;
		text.color = col;

		col = sliderBack.color;
		col.a = a;
		sliderBack.color = col;

		col = sliderFill.color;
		col.a = a;
		sliderFill.color = col;
	}
}
