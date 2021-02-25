using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrCanvasSelected : MonoBehaviour {
	
	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject uiConfirmation;
	ScrCanvasConfirmation scrConfirmation;

	public GameObject uiNamePanel;
	Text textNamePanel;

	public GameObject uiActionButton;
	public GameObject uiActionContent;
	RectTransform rectActionContent;

	public GameObject uiBuildButton;
	public GameObject uiBuildContent;
	RectTransform rectBuildContent;

	public GameObject[] uiBuildList;

	public GameObject uiInfoButton;
	RectTransform rectInfoButton;
	Text textInfoButton;

	public GameObject uiWorkedButton;
	RectTransform rectWorkedButton;
	Image imageWorkedButton;
	Button buttonWorkedButton;
	Text textWorkedButton;

	public GameObject uiClearButton;
	RectTransform rectClearButton;
	Image imageClearButton;
	Button buttonClearButton;
	Text textClearButton;

	public GameObject uiHammerButton;
	RectTransform rectHammerButton;
	Text textHammerButton;

	public Sprite spriteDemolish;
	public Sprite spriteRemove;
	public Sprite spriteExplore;
	public Sprite spriteSlaughter;
	public Sprite spriteSleep;
	public Sprite spriteWork;

	GameObject lastSelected = null;
	bool isUpdated = false;

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

		textNamePanel = uiNamePanel.GetComponentInChildren <Text> ();

		rectActionContent = uiActionContent.GetComponent <RectTransform> ();
		rectBuildContent = uiBuildContent.GetComponent <RectTransform> ();

		rectInfoButton = uiInfoButton.GetComponent <RectTransform> ();
		textInfoButton = uiInfoButton.GetComponentInChildren <Text> ();

		rectWorkedButton = uiWorkedButton.GetComponent <RectTransform> ();
		imageWorkedButton = uiWorkedButton.GetComponent <Image> ();
		buttonWorkedButton = uiWorkedButton.GetComponent <Button> ();
		textWorkedButton = uiWorkedButton.GetComponentInChildren <Text> ();

		rectClearButton = uiClearButton.GetComponent <RectTransform> ();
		imageClearButton = uiClearButton.GetComponent <Image> ();
		buttonClearButton = uiClearButton.GetComponent <Button> ();
		textClearButton = uiClearButton.GetComponentInChildren <Text> ();

		rectHammerButton = uiHammerButton.GetComponent <RectTransform> ();
		textHammerButton = uiHammerButton.GetComponentInChildren <Text> ();
	}

	void OnEnable () {
		if (isUpdated) {
			scrCanvas.Activate (uiActionButton, true);
			scrCanvas.Activate (uiBuildButton, false);
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		isUpdated = true;
		int i;
		bool isTrue;

		//selectable exist?
		if (scrController.objSelected && scrController.objSelected == lastSelected) {
			ScrHex scrHex = scrController.objSelected.GetComponent <ScrHex> ();

			scrCanvas.Activate (uiNamePanel, true);
			textNamePanel.text = ScrLanguage.Translate (scrHex.buildNameKey);

			//if no uibuildbutton
			if (!uiBuildButton.activeInHierarchy) {
				scrCanvas.Activate (uiActionButton, true);
			}

			//info button
			textInfoButton.text = ScrLanguage.Translate ("ui_info");
			scrCanvas.Activate (uiInfoButton, true);

			//hammer button
			isTrue = false;
			foreach (GameObject obj in uiBuildList) {
				ScrButtonBuild scr = obj.GetComponent <ScrButtonBuild> ();
				if (scr.objReqTech.GetComponent <ScrTech> ().isResearched && scr.Buildable ()) {
					isTrue = true;
					break;
				}
			}
			textHammerButton.text = ScrLanguage.Translate ("ui_hammer");
			scrCanvas.Activate (uiHammerButton, isTrue);

			//selectable's building exist?
			isTrue = false;
			if (scrHex.objBuild) {
				if (!scrHex.objBuild.GetComponent <ScrBuild> ().isDeco) {
					isTrue = true;
				}
			}

			if (isTrue) {

				ScrBuild scrBuild = scrHex.objBuild.GetComponent <ScrBuild> ();

				//rename
				textNamePanel.text = scrBuild.DescReplace (ScrLanguage.Translate (scrBuild.buildNameKey));

				//work button
				if (scrBuild.isWorkable) {
					if (scrBuild.isWorked) {
						textWorkedButton.text = ScrLanguage.Translate ("ui_sleep");
						imageWorkedButton.sprite = spriteSleep;
					} else {
						textWorkedButton.text = ScrLanguage.Translate ("ui_work");
						imageWorkedButton.sprite = spriteWork;
					}
					buttonWorkedButton.interactable = (scrBuild.isWorked || scrPlayer.populationIdle > 0);
					scrCanvas.Activate (uiWorkedButton, true);
				} else {
					scrCanvas.Activate (uiWorkedButton, false);
				}

				//clear button
				if (scrBuild.isClearable) {
					if (scrBuild.isBuilding) {
						textClearButton.text = ScrLanguage.Translate ("ui_demolish");
						imageClearButton.sprite = spriteDemolish;
					} else {
						if (scrBuild.buildTag == "ruin") {
							textClearButton.text = ScrLanguage.Translate ("ui_explore");
							imageClearButton.sprite = spriteExplore;
						} else if (scrBuild.buildTag == "cattle") {
							textClearButton.text = ScrLanguage.Translate ("ui_slaughter");
							imageClearButton.sprite = spriteSlaughter;			
						} else {
							textClearButton.text = ScrLanguage.Translate ("ui_remove");
							imageClearButton.sprite = spriteRemove;
						}
					}
					buttonClearButton.interactable = true;
					scrCanvas.Activate (uiClearButton, true);
				} else {
					scrCanvas.Activate (uiClearButton, false);
				}

				//special behavior for cloud
				if (scrBuild.buildTag == "cloud") {
					if (scrBuild.isBuilt) {
						if (scrBuild.isWorked) {
							textWorkedButton.text = ScrLanguage.Translate ("ui_sleep");
							imageWorkedButton.sprite = spriteSleep;
						} else {
							textWorkedButton.text = ScrLanguage.Translate ("ui_explore");
							imageWorkedButton.sprite = spriteExplore;
						}
						
						buttonWorkedButton.interactable = (scrBuild.isWorked || scrPlayer.populationIdle > 0);
						scrCanvas.Activate (uiWorkedButton, true);
						scrCanvas.Activate (uiClearButton, false);
					} else {
						if (scrBuild.Buildable (scrController.objSelected)) {
							textClearButton.text = ScrLanguage.Translate ("ui_explore");
							imageClearButton.sprite = spriteExplore;
							buttonClearButton.interactable = true;
							scrCanvas.Activate (uiClearButton, true);
						} else {
							scrCanvas.Activate (uiClearButton, false);
						}
						scrCanvas.Activate (uiWorkedButton, false);
					}
				}


			} else {
				scrCanvas.Activate (uiClearButton, false);
				scrCanvas.Activate (uiWorkedButton, false);
			}

			if (uiActionButton.activeInHierarchy) {
				//sorting button
				i = 0;
				if (uiInfoButton.activeInHierarchy) {
					rectInfoButton.anchoredPosition = new Vector2 (i * 128f, 0f);
					i++;
				}
				if (uiWorkedButton.activeInHierarchy) {
					rectWorkedButton.anchoredPosition = new Vector2 (i * 128f, 0f);
					i++;
				}
				if (uiHammerButton.activeInHierarchy) {
					rectHammerButton.anchoredPosition = new Vector2 (i * 128f, 0f);
					i++;
				}
				if (uiClearButton.activeInHierarchy) {
					rectClearButton.anchoredPosition = new Vector2 (i * 128f, 0f);
					i++;
				}

				i--;
				rectActionContent.sizeDelta = new Vector2 ((i * 128f) + 96f, 0f);

			} else if (uiBuildButton.activeInHierarchy) {
				i = 0;
				foreach (GameObject obj in uiBuildList) {
					ScrButtonBuild scr = obj.GetComponent <ScrButtonBuild> ();
					if (scr.objReqTech.GetComponent <ScrTech> ().isResearched && scr.Buildable ()) {
						scrCanvas.Activate (obj, true);
						obj.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (i * 128f, 0f);
						i++;
					} else {
						scrCanvas.Activate (obj, false);
					}
				}

				i--;
				rectBuildContent.sizeDelta = new Vector2 ((i * 128f) + 96f, 0f);
			}

		} else {
			lastSelected = scrController.objSelected;
			scrCanvas.Activate (uiActionButton, false);
			scrCanvas.Activate (uiBuildButton, false);
		}
	}

	public void Worked () {
		ScrHex scrHex = scrController.objSelected.GetComponent <ScrHex> ();
		ScrBuild scrBuild = scrHex.objBuild.GetComponent <ScrBuild> ();

		if (scrBuild.isWorked) {
			scrBuild.isWorked = false;
		} else {
			scrBuild.isWorked = true;
		}

		scrController.Calculate ();
	}

	public void Hammer () {
		//enable build and disable active
		scrCanvas.Activate (uiActionButton, false);
		scrCanvas.Activate (uiBuildButton, true);

	}

	public void ClickInfo () {
		ScrHex scrHex = scrController.objSelected.GetComponent <ScrHex> ();

		if (scrHex.objBuild) {
			ScrBuild scrBuild = scrHex.objBuild.GetComponent <ScrBuild> ();

			if (!scrBuild.isDeco) {
				scrConfirmation.textTitle.text = scrBuild.DescReplace (ScrLanguage.Translate (scrBuild.buildNameKey));
				if (scrBuild.isBuilt) {
					if (!scrBuild.builtObject) {
						scrConfirmation.textDescription.text = scrBuild.DescReplace (ScrLanguage.Translate (scrBuild.buildDescKey));	
					} else {
						scrConfirmation.textDescription.text = scrBuild.builtObject.GetComponent <ScrBuild> ().DescReplace (ScrLanguage.Translate ("build_desc_info"));
					}
				}else if (scrBuild.isBuilding) {
					scrConfirmation.textDescription.text = scrBuild.DescReplace (ScrLanguage.Translate ("build_desc_info"));
				} else {
					scrConfirmation.textDescription.text = scrBuild.DescReplace (ScrLanguage.Translate (scrBuild.buildDescKey));				
				}
			} else {
				scrConfirmation.textTitle.text = ScrLanguage.Translate(scrHex.buildNameKey);
				scrConfirmation.textDescription.text = ScrLanguage.Translate(scrHex.buildDescKey);	
			}

		} else {
			scrConfirmation.textTitle.text = ScrLanguage.Translate(scrHex.buildNameKey);
			scrConfirmation.textDescription.text = ScrLanguage.Translate(scrHex.buildDescKey);			
		}

		scrConfirmation.clickedMethod = Info;
		scrConfirmation.buttonYesButton.interactable = true;
		scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_ok");

		uiConfirmation.SetActive (true);
	}

	void Info () {
	}

	public void ClickClear () {
		GameObject objBuild = scrController.objSelected.GetComponent <ScrHex> ().objBuild;
		ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();

		scrConfirmation.textTitle.text = scrBuild.DescReplace (ScrLanguage.Translate (scrBuild.clearNameKey));
		scrConfirmation.textDescription.text = scrBuild.DescReplace (ScrLanguage.Translate (scrBuild.clearDescKey));

		if (scrPlayer.populationIdle > 0) {
			if (scrBuild.isBuilding) {
				scrConfirmation.textYesButton.text = scrBuild.DescReplace (ScrLanguage.Translate ("ui_demolish_time"));
			} else if (scrBuild.buildTag == "cloud" || scrBuild.buildTag == "ruin") {
				scrConfirmation.textYesButton.text = scrBuild.DescReplace (ScrLanguage.Translate ("ui_explore_time"));
			} else if (scrBuild.buildTag == "cattle") {
				scrConfirmation.textYesButton.text = scrBuild.DescReplace (ScrLanguage.Translate ("ui_slaughter_time"));				
			} else {
				scrConfirmation.textYesButton.text = scrBuild.DescReplace (ScrLanguage.Translate ("ui_remove_time"));
			}
		} else {
			scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_need_population");
		}
		scrConfirmation.clickedMethod = Clear;
		scrConfirmation.buttonYesButton.interactable = (scrPlayer.populationIdle > 0);

		uiConfirmation.SetActive (true);
	}

	public void Clear () {
		ScrHex scrHex = scrController.objSelected.GetComponent <ScrHex> ();
		scrController.objSelected = null;
		scrController.objCursor.SetActive (false);
		GameObject objBuilding = scrHex.objBuild;
		ScrBuild scrBuilding = objBuilding.GetComponent <ScrBuild> ();

		scrHex.Build (scrBuilding.clearObject);
		GameObject objBuild = scrHex.objBuild;

		if (objBuild) {
			ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();

			scrBuild.buildNameKey = scrBuilding.clearNameKey;
			scrBuild.buildDescKey = scrBuilding.clearDescKey;
			scrBuild.buildEffectKey = scrBuilding.buildEffectKey;
			scrBuild.builtInfo = scrBuilding.builtInfo;
			if (scrBuild.buildTag == "cloudclear") {
				scrBuild.builtTime = (int) (Vector3.Distance (Vector3.zero, scrHex.transform.position) * 0.2f * scrPlayer.modExplorationTime);
				scrBuild.builtObject = scrBuilding.builtObject;
			} else {
				scrBuild.builtTime = scrBuilding.builtTime/2;
				scrBuild.builtObject = null;
			}
		}

		scrController.Calculate ();
	}
}
