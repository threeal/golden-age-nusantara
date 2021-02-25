using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasIcon : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	public GameObject objHex;
	ScrHex scrHex;

	public Sprite imgBuild;
	public Sprite imgSleep;
	public Sprite imgDemolish;
	public Sprite imgClear;
	public Sprite imgExplore;

	public GameObject uiPanel;
	Image imagePanel;

	public GameObject uiCount;
	Text textCount;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		imagePanel = uiPanel.GetComponent <Image> ();
		textCount = uiCount.GetComponentInChildren <Text> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (objHex) {
			if (objHex.activeInHierarchy) {
				Vector3 pos = objHex.transform.position;
				pos.y += 4;
				transform.position = Camera.main.WorldToScreenPoint (pos);

				if (!scrHex) {
					scrHex = objHex.GetComponent <ScrHex> ();
				}

				if (scrHex.objBuild && transform.position.z >= 0) {
					ScrBuild scrBuild = scrHex.objBuild.GetComponent <ScrBuild> ();

					//special for cloud
					/*if (scrBuild.buildTag == "cloud" && !scrBuild.isBuilt && scrBuild.Buildable (objHex)) {
						uiPanel.SetActive (true);
						imagePanel.sprite = imgExplore;
						uiCount.SetActive (false);

					} else */if (scrBuild.isWorkable) {
						if (!scrBuild.isWorked) {
							uiPanel.SetActive (true);
							imagePanel.sprite = imgSleep;

							uiCount.SetActive (false);

						} else if (scrBuild.isBuilt) {
							if (scrBuild.buildTag == "cloudclear" || scrBuild.buildTag == "ruinclear") {
								uiPanel.SetActive (true);
								imagePanel.sprite = imgExplore;

								uiCount.SetActive (true);
								textCount.text = scrBuild.builtTime.ToString ("F0");

							} else if (scrBuild.builtObject) {
								uiPanel.SetActive (true);
								imagePanel.sprite = imgBuild;

								uiCount.SetActive (true);
								textCount.text = scrBuild.builtTime.ToString ("F0");

							} else {
								if (scrBuild.builtInfo.GetComponent <ScrBuild> ().isBuilding) {
									uiPanel.SetActive (true);
									imagePanel.sprite = imgDemolish;

									uiCount.SetActive (true);
									textCount.text = scrBuild.builtTime.ToString ("F0");

								} else {
									uiPanel.SetActive (true);
									imagePanel.sprite = imgClear;

									uiCount.SetActive (true);
									textCount.text = scrBuild.builtTime.ToString ("F0");

								}
							}
						} else {
							uiPanel.SetActive (false);
							uiCount.SetActive (false);
						}
					} else {
						uiPanel.SetActive (false);
						uiCount.SetActive (false);
					}
				} else {
					uiPanel.SetActive (false);
					uiCount.SetActive (false);
				}
			} else {
				uiPanel.SetActive (false);
				uiCount.SetActive (false);
			}
		} else {
			uiPanel.SetActive (false);
			uiCount.SetActive (false);
		}
	}

	public void IconClick () {
		scrController.objSelected = objHex;
	}
}
