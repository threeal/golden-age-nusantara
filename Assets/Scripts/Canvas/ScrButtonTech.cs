using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrButtonTech : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	GameObject uiConfirmation;
	GameObject uiTechnology;
	ScrCanvasTechnology scrCanvasTechnology;

	public GameObject objTech;
	ScrTech scrTech;

	Button button;
	Image buttonImage;
	Text buttonText;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();	

		objCanvas = GameObject.Find ("Canvas");
		uiConfirmation = objCanvas.GetComponent <ScrCanvas> ().uiConfirmation;
		uiTechnology = objCanvas.GetComponent <ScrCanvas> ().uiTechnology;
		scrCanvasTechnology = uiTechnology.GetComponent <ScrCanvasTechnology> ();

		scrTech = objTech.GetComponent <ScrTech> ();

		button = GetComponent <Button> ();
		buttonImage = GetComponent <Image> ();
		buttonText = GetComponentInChildren <Text> ();

		button.interactable = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!scrTech.isResearched) {
			buttonImage.sprite = scrCanvasTechnology.sprNormal;
			button.spriteState = scrCanvasTechnology.sprNormalState;
		}
		else {
			buttonImage.sprite = scrCanvasTechnology.sprResearched;
			button.spriteState = scrCanvasTechnology.sprResearchedState;
		}

		buttonText.text = ScrLanguage.Translate (scrTech.techNameKey);
	}

	public void Click () {
		ScrCanvasConfirmation scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();
		string str;

		if (scrTech.isResearched) {
			scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_researched");
			scrConfirmation.clickedMethod = null;
			scrConfirmation.buttonYesButton.interactable = false;
		} else if (!scrTech.Researchable ()) {
			scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_locked");
			scrConfirmation.clickedMethod = null;
			scrConfirmation.buttonYesButton.interactable = false;		
		} else {
			if (scrPlayer.cultureVal >= (scrTech.techCost * scrPlayer.modTechnologyCost)) {
				str = ScrLanguage.Translate ("ui_research_time");
				str = str.Replace ("[value]", (scrTech.techCost * scrPlayer.modTechnologyCost).ToString ("F0"));
				scrConfirmation.textYesButton.text = str;

				scrConfirmation.clickedMethod = ClickTech;
				scrConfirmation.buttonYesButton.interactable = true;		
			} else {
				str = ScrLanguage.Translate ("ui_need_culture");
				str = str.Replace ("[value]", (scrTech.techCost * scrPlayer.modTechnologyCost).ToString ("F0"));
				scrConfirmation.textYesButton.text = str;
				scrConfirmation.clickedMethod = null;
				scrConfirmation.buttonYesButton.interactable = false;	
			}
		}

		scrConfirmation.cancelMethod = CancelTech;
		scrConfirmation.textTitle.text = ScrLanguage.Translate (scrTech.techNameKey);
		scrConfirmation.textDescription.text = ScrLanguage.Translate (scrTech.techDescKey);
		scrConfirmation.textDescription.text += "\n" + ScrLanguage.Translate (scrTech.techEffectKey);

		uiConfirmation.SetActive (true);
	}

	void ClickTech() {
		scrTech.isResearched = true;
		scrPlayer.cultureVal -= scrTech.techCost * scrPlayer.modTechnologyCost;
		scrTech.EffectOnActive ();
		scrController.Calculate ();

		CancelTech ();
	}

	void CancelTech() {
		uiTechnology.SetActive (true);
	}
}
