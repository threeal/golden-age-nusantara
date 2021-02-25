using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrButtonBuild : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject uiConfirmation;
	ScrCanvasConfirmation scrConfirmation;

	public GameObject objBuilding;
	ScrBuild scrBuilding;
	public GameObject objReqTech;
	public int objMax = -1;
	//ScrTech scrReqTech;

	Button button;
	Text buttonText;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		uiConfirmation = scrCanvas.uiConfirmation;
		scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();

		button = GetComponent <Button> ();
		buttonText = GetComponentInChildren <Text> ();


		scrBuilding = objBuilding.GetComponent <ScrBuild> ();
		//scrReqTech = objReqTech.GetComponent <ScrTech> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (scrBuilding) {
			buttonText.text = scrBuilding.DescReplace (ScrLanguage.Translate (scrBuilding.buildNameKey));
		}

		if (scrBuilding.buildTag == "wonders") {
			if (objMax > 0) {
				int n = 0, m = 0;
				if (scrController.dictionaryBuild.TryGetValue ("wonders", out n)) {
				} else {
					n = 0;
				}
				if (scrController.dictionaryBuild.TryGetValue ("sitewonders", out m)) {
				} else {
					m = 0;
				}
					button.interactable = (n < objMax && m < objMax);
			}
		}
	}

	public bool Buildable () {
		return scrBuilding.Buildable (scrController.objSelected);
	}

	public void Click () {
		scrConfirmation.clickedMethod = Build;
		scrConfirmation.textTitle.text = scrBuilding.DescReplace (ScrLanguage.Translate (scrBuilding.builtNameKey));
		scrConfirmation.textDescription.text = scrBuilding.DescReplace (ScrLanguage.Translate ("build_desc_build"));

		if (scrPlayer.populationIdle > 0) {
			scrConfirmation.textYesButton.text = scrBuilding.DescReplace (ScrLanguage.Translate ("ui_build_time"));
		} else {
			scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_need_population");
		}

		scrConfirmation.buttonYesButton.interactable = (scrPlayer.populationIdle > 0);

		uiConfirmation.SetActive (true);
	}

	void Build () {
		ScrHex scrHex = scrController.objSelected.GetComponent <ScrHex> ();
		scrController.objSelected = null;
		scrController.objCursor.SetActive (false);
		ScrBuild scrBuilding = objBuilding.GetComponent <ScrBuild> ();
		scrHex.Build (scrBuilding.builtObject);

		GameObject objBuild = scrHex.objBuild;
		if (objBuild) {
			ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();

			scrBuild.buildNameKey = scrBuilding.builtNameKey;
			scrBuild.buildDescKey = scrBuilding.buildDescKey;
			scrBuild.buildNeedKey = scrBuilding.buildNeedKey;
			scrBuild.buildEffectKey = scrBuilding.buildEffectKey;
			scrBuild.builtInfo = scrBuilding.builtInfo;
			scrBuild.builtObject = objBuilding;
			scrBuild.builtTime = scrBuilding.builtTime;
			scrBuild.builtCost = scrBuilding.builtCost;

			scrController.Calculate ();
		}		
	}
}
