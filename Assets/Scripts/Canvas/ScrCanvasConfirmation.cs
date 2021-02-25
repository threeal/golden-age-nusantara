using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasConfirmation : MonoBehaviour {

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	public GameObject objTitle;
	public Text textTitle;

	public GameObject objDescription;
	public Text textDescription;

	public GameObject objYesButton;
	public Button buttonYesButton;
	public Text textYesButton;

	public GameObject objChoiceAButton;
	public Button buttonChoiceAButton;
	public Text textChoiceAButton;

	public GameObject objChoiceBButton;
	public Button buttonChoiceBButton;
	public Text textChoiceBButton;

	public GameObject objCancelButton;

	public delegate void ClickedMethod();
	public ClickedMethod clickedMethod = null;
	public ClickedMethod choiceAMethod = null;
	public ClickedMethod choiceBMethod = null;
	public ClickedMethod cancelMethod = null;

	public bool isChoice = false;

	// Use this for initialization
	void Start () {
		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		textTitle = objTitle.GetComponent <Text> ();
		textDescription = objDescription.GetComponent <Text> ();

		buttonYesButton = objYesButton.GetComponent <Button> ();
		textYesButton = objYesButton.GetComponentInChildren <Text> ();

		buttonChoiceAButton = objChoiceAButton.GetComponent <Button> ();
		textChoiceAButton = objChoiceAButton.GetComponentInChildren <Text> ();

		buttonChoiceBButton = objChoiceBButton.GetComponent <Button> ();
		textChoiceBButton = objChoiceBButton.GetComponentInChildren <Text> ();

		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		scrCanvas.Activate (objYesButton, !isChoice);
		scrCanvas.Activate (objChoiceAButton, isChoice);
		scrCanvas.Activate (objChoiceBButton, isChoice);

		if (clickedMethod == null) {
			buttonYesButton.interactable = false;
		}
		if (choiceAMethod == null) {
			buttonChoiceAButton.interactable = false;
		}
		if (choiceBMethod == null) {
			buttonChoiceBButton.interactable = false;
		}
	}

	public void Yes () {
		clickedMethod ();
		cancelMethod = null;
		Cancel ();
	}

	public void ChoiceA () {
		choiceAMethod ();
		cancelMethod = null;
		Cancel ();
	}

	public void ChoiceB () {
		choiceBMethod ();
		cancelMethod = null;
		Cancel ();
	}

	public void Cancel () {
		if (cancelMethod != null) {
			cancelMethod ();
		}
		clickedMethod = null;
		choiceAMethod = null;
		choiceBMethod = null;
		cancelMethod = null;
		isChoice = false;
		gameObject.SetActive (false);
	}
}
