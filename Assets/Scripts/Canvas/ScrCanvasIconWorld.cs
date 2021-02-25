using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasIconWorld : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	public GameObject objCity;
	ScrCity scrCity;

	public Sprite imgAlliance;
	public Sprite imgInvade;
	public Sprite imgTrade;
	public Sprite imgSendGift;
	public Sprite imgRaid;
	public Sprite imgCapital;
	public Sprite imgAlly;
	public Sprite imgVassal;

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
		if (objCity) {
			if (objCity.activeInHierarchy) {
				Vector3 pos = objCity.transform.position;
				pos.y += 2.5f;
				transform.position = Camera.main.WorldToScreenPoint (pos);

				if (!scrCity) {
					scrCity = objCity.GetComponent <ScrCity> ();
				}

				if (transform.position.z >= 0) {
					if (scrCity.isPlayer) {
						imagePanel.sprite = imgCapital;
						uiPanel.SetActive (true);
						uiCount.SetActive (false);
					} else {
						if (scrCity.isAlly) {
							if (scrCity.isVassal) {
								imagePanel.sprite = imgVassal;
								uiPanel.SetActive (true);
								uiCount.SetActive (false);
							} else {
								imagePanel.sprite = imgAlly;
								uiPanel.SetActive (true);
								uiCount.SetActive (false);
							}
						} else if (scrCity.commandTag != "") {
							if (scrCity.commandTag == "alliance") {
								imagePanel.sprite = imgAlliance;
								uiPanel.SetActive (true);
							}
							else if (scrCity.commandTag == "invade") {
								imagePanel.sprite = imgInvade;
								uiPanel.SetActive (true);
							}
							else if (scrCity.commandTag == "trade") {
								imagePanel.sprite = imgTrade;
								uiPanel.SetActive (true);
							}
							else if (scrCity.commandTag == "sendgift") {
								imagePanel.sprite = imgSendGift;
								uiPanel.SetActive (true);
							}
							else if (scrCity.commandTag == "raid") {
								imagePanel.sprite = imgRaid;
								uiPanel.SetActive (true);
							}

							textCount.text = scrCity.commandTime.ToString ("F0");
							uiCount.SetActive (true);
						} else {
							uiPanel.SetActive (false);
							uiCount.SetActive (false);
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
	}

	public void IconClick () {
		scrController.objSelectedCity = objCity;
	}
}
