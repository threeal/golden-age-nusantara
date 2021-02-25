using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrMenu : MonoBehaviour {

	public string savedPath;

	GameObject objIntro;
	ScrIntro scrIntro;

	public GameObject uiNewGame;
	Text textNewGame;

	public GameObject uiContinue;
	Button buttonContinue;
	Text textContinue;

	public GameObject uiExit;
	Text textExit;

	// Use this for initialization
	void Start () {
		objIntro = GameObject.Find ("Intro");
		scrIntro = objIntro.GetComponent <ScrIntro> ();

		textNewGame = uiNewGame.GetComponentInChildren <Text> ();

		buttonContinue = uiContinue.GetComponent <Button> ();
		textContinue = uiContinue.GetComponentInChildren <Text> ();

		textExit = uiExit.GetComponentInChildren <Text> ();

		buttonContinue.interactable = (File.Exists (Application.persistentDataPath + savedPath));
	}
	
	// Update is called once per frame
	void LateUpdate () {
		textNewGame.text = ScrLanguage.Translate ("ui_newgame");
		textContinue.text = ScrLanguage.Translate ("ui_continue");
		textExit.text = ScrLanguage.Translate ("ui_exit");
	}

	public void Continue () {
		PlayerPrefs.SetInt ("continue", 1);
		scrIntro.nextScene = "Game";
		objIntro.SetActive (true);		
	}

	public void NewGame () {
		PlayerPrefs.SetInt ("continue", 0);
		scrIntro.nextScene = "Prologue";
		objIntro.SetActive (true);
	}

	public void Exit () {
		Application.Quit ();
	}
}
