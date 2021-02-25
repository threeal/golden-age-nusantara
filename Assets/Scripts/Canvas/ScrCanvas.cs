using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrCanvas : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objCamera;
	ScrCamera scrCamera;

	public GameObject uiIcons;
	public GameObject uiEvents;
	public GameObject uiSelected;
	public GameObject uiResource;
	public GameObject uiControl;
	public GameObject uiDiplomacy;
	public GameObject uiTechnology;
	public GameObject uiWar;
	public GameObject uiConfirmation;
	public GameObject uiMenu;
	public GameObject uiEndTurn;
	public GameObject uiIntro;
	public GameObject uiEmpty;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objCamera = GameObject.Find ("Camera");
		scrCamera = objCamera.GetComponent <ScrCamera> ();
	}
	
	// Update is called once per frame
	void Update () {

		//uiEmpty
		if (uiEmpty.activeInHierarchy) {

			scrCamera.isStatic = true;
			Activate (uiMenu, false);
			Activate (uiWar, false);
			Activate (uiConfirmation, false);
			Activate (uiTechnology, false);
			Activate (uiDiplomacy, false);
			Activate (uiResource, false);
			Activate (uiControl, false);
			Activate (uiSelected, false);
			Activate (uiEvents, false);

		} else {
			
			//uiMenu
			if (uiMenu.activeInHierarchy) {

				scrCamera.isStatic = true;
				Activate (uiWar, false);
				Activate (uiConfirmation, false);
				Activate (uiTechnology, false);
				Activate (uiDiplomacy, false);
				Activate (uiResource, false);
				Activate (uiControl, false);
				Activate (uiSelected, false);
				Activate (uiEvents, false);

			} else {

				//uiConfirmation
				if (uiConfirmation.activeInHierarchy) {
				
					scrCamera.isStatic = true;
					Activate (uiWar, false);
					Activate (uiTechnology, false);
					Activate (uiDiplomacy, false);
					Activate (uiResource, false);
					Activate (uiControl, false);
					Activate (uiSelected, false);
					Activate (uiEvents, false);
				
				} else {

					//uiWar
					if (uiWar.activeInHierarchy) {
						
						scrCamera.isStatic = true;
						Activate (uiTechnology, false);
						Activate (uiDiplomacy, false);
						Activate (uiResource, false);
						Activate (uiControl, false);
						Activate (uiSelected, false);
						Activate (uiEvents, false);

					} else {

						//uiTechnology
						if (uiTechnology.activeInHierarchy) {

							scrCamera.isStatic = true;
							Activate (uiDiplomacy, false);
							Activate (uiResource, false);
							Activate (uiControl, false);
							Activate (uiSelected, false);
							Activate (uiEvents, false);

						} else {

							//uiDiplomacy
							if (uiDiplomacy.activeInHierarchy) {

								scrCamera.isStatic = true;
								Activate (uiResource, false);
								Activate (uiControl, false);
								Activate (uiSelected, false);
								Activate (uiEvents, false);

							} else {

								scrCamera.isStatic = false;

								//uiresource
								Activate (uiResource, true);
								Activate (uiEvents, true);

								//uiselected
								if (scrController.objSelected) {
									Activate (uiSelected, true);
								} else {
									Activate (uiSelected, false);
								}

								//uicontrol
								if (uiSelected.activeInHierarchy) {
									Activate (uiControl, false);
								} else {
									Activate (uiControl, true);
								}		
							}
						}
					}
				}
			}
		}
	}

	public void Activate(GameObject ui, bool activate) {
		if (activate) {
			if (!ui.activeInHierarchy) {
				ui.SetActive (true);
			}
		}
		else {
			if (ui.activeInHierarchy) {
				ui.SetActive (false);
			}
		}
	}
}
