using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasControl : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject objCamera;
	ScrCamera scrCamera;

	public GameObject uiTurnButton;
	Text textTurnButton;

	public GameObject uiWorldButton;

	public GameObject uiCityButton;

	public GameObject uiMenu;

	public GameObject uiTechnology;
	public GameObject uiTechnologyCount;
	Text textTechnologyCount;

	GameObject[] objTechs;

	// Use this for initialization
	void Start () {
		uiMenu.SetActive (false);

		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		objCamera = GameObject.Find ("Camera");
		scrCamera = objCamera.GetComponent <ScrCamera> ();
		
		textTurnButton = uiTurnButton.GetComponentInChildren <Text> ();
		textTechnologyCount = uiTechnologyCount.GetComponentInChildren <Text> ();

		objTechs = GameObject.FindGameObjectsWithTag ("Tech");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		textTurnButton.text = "End Turn\n(turn " + scrPlayer.turnCount + " )";

		if (scrPlayer.enableWorld) {
			if (scrController.objWorld.activeInHierarchy) {
				scrCanvas.Activate (uiWorldButton, false);
				scrCanvas.Activate (uiCityButton, true);
			} else {
				scrCanvas.Activate (uiWorldButton, true);
				scrCanvas.Activate (uiCityButton, false);
			}
		} else {
			scrCanvas.Activate (uiWorldButton, false);
			scrCanvas.Activate (uiCityButton, false);
		}

		//Tech Count
		int count = 0;
		foreach (GameObject objTech in objTechs) {
			ScrTech scrTech = objTech.GetComponent <ScrTech> ();
			if (scrTech.Researchable () && scrPlayer.cultureVal >= (scrTech.techCost * scrPlayer.modTechnologyCost)) {
				count++;
			}
		}
		textTechnologyCount.text = count.ToString ();
		uiTechnologyCount.SetActive (count > 0);
	}

	public void EndTurn() {
		scrCanvas.uiEndTurn.SetActive (true);
		scrController.EndTurn ();
	}

	public void TechTree() {
		uiTechnology.SetActive (true);
	}

	public void WorldView() {
		scrController.objSelected = null;
		scrController.objSelectedCity = null;
		scrController.objCursor.SetActive (false);
		scrController.objWorld.SetActive (!scrController.objWorld.activeInHierarchy);
		scrController.objMap.SetActive (!scrController.objMap.activeInHierarchy);
		scrCamera.Reset ();
		scrController.Calculate ();
	}

	public void Menu() {
		uiMenu.SetActive (true);
	}
}
