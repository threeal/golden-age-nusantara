using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasDiplomacy : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject objWorld;
	ScrWorld scrWorld;

	GameObject uiConfirmation;
	ScrCanvasConfirmation scrConfirmation;

	public Text textName;
	public Text textDescription;
	public Text textCommand;

	public GameObject uiAllianceButton;
	Button buttonAllianceButton;
	Text textAllianceButton;

	public GameObject uiInvadeButton;
	Button buttonInvadeButton;
	Text textInvadeButton;

	public GameObject uiTradeButton;
	Button buttonTradeButton;
	Text textTradeButton;

	public GameObject uiSendGiftButton;
	Button buttonSendGiftButton;
	Text textSendGiftButton;

	public GameObject uiRaidButton;
	Button buttonRaidButton;
	Text textRaidButton;

	public GameObject uiBreakButton;
	Button buttonBreakButton;
	Text textBreakButton;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		objWorld = GameObject.Find ("World");
		scrWorld = objWorld.GetComponent <ScrWorld> ();

		uiConfirmation = scrCanvas.uiConfirmation;
		scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();

		buttonAllianceButton = uiAllianceButton.GetComponent <Button> ();
		textAllianceButton = uiAllianceButton.GetComponentInChildren <Text> ();

		buttonInvadeButton = uiInvadeButton.GetComponent <Button> ();
		textInvadeButton = uiInvadeButton.GetComponentInChildren <Text> ();

		buttonTradeButton = uiTradeButton.GetComponent <Button> ();
		textTradeButton = uiTradeButton.GetComponentInChildren <Text> ();

		buttonSendGiftButton = uiSendGiftButton.GetComponent <Button> ();
		textSendGiftButton = uiSendGiftButton.GetComponentInChildren <Text> ();

		buttonRaidButton = uiRaidButton.GetComponent <Button> ();
		textRaidButton = uiRaidButton.GetComponentInChildren <Text> ();

		buttonBreakButton = uiBreakButton.GetComponent <Button> ();
		textBreakButton = uiBreakButton.GetComponentInChildren <Text> ();

		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (scrController.objSelectedCity) {

			ScrCity scr = scrController.objSelectedCity.GetComponent <ScrCity> ();
			textName.text = ScrLanguage.Translate (scr.cityNameKey);

			if (scr.isPlayer) {
				textDescription.text = scr.DescReplace (ScrLanguage.Translate (scr.cityDescKey));
			} else {
				textDescription.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_intro"));
				textDescription.text += "\n" + scr.DescReplace (ScrLanguage.Translate (scr.cityDescKey)) + "\n";

				if (scr.isAlly) {
					if (scr.isVassal) {
						textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_relation5");
					} else {
						textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_relation4");
					}
				} else {
					if (scr.cityRelation >= 2) {
						textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_relation3");
					} else if (scr.cityRelation >= 1) {
						textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_relation2");
					} else {
						textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_relation1");
					}
				}

				if (scr.cityMillitary < 75f) {
					textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_military1");
				} else if (scr.cityMillitary < 350f) {
					textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_military2");
				} else {
					textDescription.text += "\n" + ScrLanguage.Translate ("city_desc_military3");
				}

				textDescription.text += "\n" + ScrLanguage.Translate (scr.tradeDescKey);
			}

			//if ally
			if (scr.isAlly || scr.commandTag != "") {

				scrCanvas.Activate (uiAllianceButton, false);
				scrCanvas.Activate (uiInvadeButton, false);
				scrCanvas.Activate (uiTradeButton, false);
				scrCanvas.Activate (uiSendGiftButton, false);
				scrCanvas.Activate (uiRaidButton, false);
				scrCanvas.Activate (uiBreakButton, false);

				if (scr.isAlly) {
					if (scr.isVassal) {
						if (scr.tradeFood > 0f) {
							textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_tributefood"));
						}
						if (scr.tradeProduction > 0f) {
							textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_tributeproduction"));
						}
						if (scr.tradeWealth > 0f) {
							textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_tributewealth"));
						}
						if (scr.tradeCulture > 0f) {
							textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_tributeculture"));
						}
						if (scr.tradeMilitary > 0f) {
							textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_tributemilitary"));
						}
					} else {
						if (scr.isPlayer) {
							textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_capital"));
						} else {
							textCommand.text = "";
							scrCanvas.Activate (uiBreakButton, true);
							buttonBreakButton.interactable = true;
						}
					}
				} else {
					if (scr.commandTag == "alliance") {
						textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_alliance"));
					} else if (scr.commandTag == "invade") {
						textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_invade"));
					} else if (scr.commandTag == "trade") {
						textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_trade"));
					} else if (scr.commandTag == "sendgift") {
						textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_sendgift"));
					} else if (scr.commandTag == "raid") {
						textCommand.text = scr.DescReplace (ScrLanguage.Translate ("city_desc_command_raid"));
					}
				}
			} else {
				//button order
				textCommand.text = "";
				scrCanvas.Activate (uiBreakButton, false);

				if (scr.cityRelation >= 2) {
					scrCanvas.Activate (uiAllianceButton, true);
					scrCanvas.Activate (uiInvadeButton, false);
				} else {
					scrCanvas.Activate (uiAllianceButton, false);
					scrCanvas.Activate (uiInvadeButton, true);
				}

				if (scr.cityRelation < 1) {
					scrCanvas.Activate (uiSendGiftButton, true);
					scrCanvas.Activate (uiTradeButton, false);
				} else {
					scrCanvas.Activate (uiSendGiftButton, false);
					scrCanvas.Activate (uiTradeButton, true);
				}

				scrCanvas.Activate (uiRaidButton, true);
			}


			//disbale button if no fleet
			buttonAllianceButton.interactable = (scrPlayer.fleetIdle > 0);
			buttonInvadeButton.interactable = (scrPlayer.fleetIdle > 0);
			buttonTradeButton.interactable = (scrPlayer.fleetIdle > 0);
			buttonSendGiftButton.interactable = (scrPlayer.fleetIdle > 0);
			buttonRaidButton.interactable = (scrPlayer.fleetIdle > 0);

			//set button text;
			textAllianceButton.text = ScrLanguage.Translate ("ui_alliance");
			textInvadeButton.text = ScrLanguage.Translate ("ui_invade");
			textTradeButton.text = ScrLanguage.Translate ("ui_trade");
			textSendGiftButton.text = ScrLanguage.Translate ("ui_sendgift");
			textRaidButton.text = ScrLanguage.Translate ("ui_raid");
			textBreakButton.text = ScrLanguage.Translate ("ui_break");

		} else {
			gameObject.SetActive (false);
		}
	}

	public void Back () {
		scrController.objSelectedCity = null;
		gameObject.SetActive (false);
	}

	//ALLIANCE
	public void ClickAlliance () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		string str;

		str = ScrLanguage.Translate ("expedition_name_alliance_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrConfirmation.textTitle.text = str;

		str = ScrLanguage.Translate ("expedition_desc_alliance_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		str = str.Replace ("[time]", (scrWorld.allianceTime * scrPlayer.modExpeditionTime).ToString ("F0"));
		scrConfirmation.textDescription.text = str;

		str = ScrLanguage.Translate ("expedition_button_alliance_init");
		str = str.Replace ("[value]", (scrCity.allianceCost * scrPlayer.modDiplomacyCost).ToString ("F0"));
		scrConfirmation.textYesButton.text = str;

		scrConfirmation.clickedMethod = Alliance;
		scrConfirmation.buttonYesButton.interactable = (scrPlayer.wealthVal >= (scrCity.allianceCost * scrPlayer.modDiplomacyCost));

		scrConfirmation.cancelMethod = Cancel;
		uiConfirmation.SetActive (true);
	}

	void Alliance () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		scrPlayer.wealthVal -= scrCity.allianceCost * scrPlayer.modDiplomacyCost;

		scrCity.commandTag = "alliance";
		scrCity.commandTime = (int) (scrWorld.allianceTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	//INVADE
	public void ClickInvade () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		string str;

		str = ScrLanguage.Translate ("expedition_name_invade_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrConfirmation.textTitle.text = str;

		float val = scrCity.cityMillitary;
		str = ScrLanguage.Translate ("expedition_desc_invade_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		str = str.Replace ("[time]", (scrWorld.invadeTime * scrPlayer.modExpeditionTime).ToString ("F0"));
		str = str.Replace ("[value]", val.ToString ("F0"));
		scrConfirmation.textDescription.text = str;

		scrConfirmation.isChoice = true;

		val = scrCity.cityMillitary * scrWorld.modArmySmall;
		str = ScrLanguage.Translate ("expedition_buttona_invade_init");
		str = str.Replace ("[value]", val.ToString ("F0"));
		scrConfirmation.textChoiceAButton.text = str;

		scrConfirmation.choiceAMethod = InvadeSmall;
		scrConfirmation.buttonChoiceAButton.interactable = (scrPlayer.militaryVal >= val);

		val = scrCity.cityMillitary * scrWorld.modArmyLarge;
		str = ScrLanguage.Translate ("expedition_buttonb_invade_init");
		str = str.Replace ("[value]", val.ToString ("F0"));
		scrConfirmation.textChoiceBButton.text = str;

		scrConfirmation.choiceBMethod = InvadeLarge;
		scrConfirmation.buttonChoiceBButton.interactable = (scrPlayer.militaryVal >= val);

		scrConfirmation.cancelMethod = Cancel;
		uiConfirmation.SetActive (true);
	}

	void InvadeSmall () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		float val = scrCity.cityMillitary * scrWorld.modArmySmall;
		scrPlayer.militaryVal -= val;
		scrCity.playerMilitary = val;

		scrCity.cityRelation -= 1;
		if (scrCity.cityRelation < 0) {
			scrCity.cityRelation = 0;
		}

		scrCity.commandTag = "invade";
		scrCity.commandTime = (int) (scrWorld.invadeTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	void InvadeLarge() {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		float val = scrCity.cityMillitary * scrWorld.modArmyLarge;
		scrPlayer.militaryVal -= val;
		scrCity.playerMilitary = val;

		scrCity.cityRelation -= 1;
		if (scrCity.cityRelation < 0) {
			scrCity.cityRelation = 0;
		}

		scrCity.commandTag = "invade";
		scrCity.commandTime = (int) (scrWorld.invadeTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	//TRADE
	public void ClickTrade () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		string str;

		str = ScrLanguage.Translate ("expedition_name_trade_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrConfirmation.textTitle.text = str;

		str = ScrLanguage.Translate ("expedition_desc_trade_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		str = str.Replace ("[time]", (scrWorld.tradeTime * scrPlayer.modExpeditionTime).ToString ("F0"));
		if (scrCity.tradeFood > 0f) {
			str = str.Replace ("[value]", scrCity.tradeFood.ToString ("F0") + " " + ScrLanguage.Translate ("misc_food"));
		}
		if (scrCity.tradeProduction > 0f) {
			str = str.Replace ("[value]", scrCity.tradeProduction.ToString ("F0") + " " + ScrLanguage.Translate ("misc_production"));
		}
		if (scrCity.tradeWealth > 0f) {
			str = str.Replace ("[value]", scrCity.tradeWealth.ToString ("F0") + " " + ScrLanguage.Translate ("misc_wealth"));
		}
		if (scrCity.tradeCulture > 0f) {
			str = str.Replace ("[value]", scrCity.tradeCulture.ToString ("F0") + " " + ScrLanguage.Translate ("misc_culture"));
		}
		if (scrCity.tradeMilitary > 0f) {
			str = str.Replace ("[value]", scrCity.tradeMilitary.ToString ("F0") + " " + ScrLanguage.Translate ("misc_military"));
		}
		scrConfirmation.textDescription.text = str;

		str = ScrLanguage.Translate ("expedition_button_trade_init");
		if (scrCity.tradeFood < 0f) {
			str = str.Replace ("[value]", Mathf.Abs (scrCity.tradeFood).ToString ("F0") + " " + ScrLanguage.Translate ("misc_food"));
			scrConfirmation.buttonYesButton.interactable = (scrPlayer.foodVal + scrCity.tradeFood >= 0f);			
		}
		if (scrCity.tradeProduction < 0f) {
			str = str.Replace ("[value]", Mathf.Abs (scrCity.tradeProduction).ToString ("F0") + " " + ScrLanguage.Translate ("misc_production"));
			scrConfirmation.buttonYesButton.interactable = (scrPlayer.productionVal + scrCity.tradeProduction >= 0f);			
		}
		if (scrCity.tradeWealth < 0f) {
			str = str.Replace ("[value]", Mathf.Abs (scrCity.tradeWealth).ToString ("F0") + " " + ScrLanguage.Translate ("misc_wealth"));
			scrConfirmation.buttonYesButton.interactable = (scrPlayer.wealthVal + scrCity.tradeWealth>= 0f);			
		}
		if (scrCity.tradeCulture < 0f) {
			str = str.Replace ("[value]", Mathf.Abs (scrCity.tradeCulture).ToString ("F0") + " " + ScrLanguage.Translate ("misc_culture"));
			scrConfirmation.buttonYesButton.interactable = (scrPlayer.cultureVal + scrCity.tradeCulture >= 0f);			
		}
		if (scrCity.tradeMilitary < 0f) {
			str = str.Replace ("[value]", Mathf.Abs (scrCity.tradeMilitary).ToString ("F0") + " " + ScrLanguage.Translate ("misc_military"));
			scrConfirmation.buttonYesButton.interactable = (scrPlayer.militaryVal + scrCity.tradeMilitary >= 0f);			
		}
		scrConfirmation.textYesButton.text = str;

		scrConfirmation.clickedMethod = Trade;
		scrConfirmation.cancelMethod = Cancel;
		uiConfirmation.SetActive (true);
	}

	void Trade () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();

		if (scrCity.tradeFood < 0f) {
			scrPlayer.foodVal += scrCity.tradeFood;	
		}
		if (scrCity.tradeProduction < 0f) {
			scrPlayer.productionVal += scrCity.tradeProduction;	
		}
		if (scrCity.tradeWealth < 0f) {
			scrPlayer.wealthVal += scrCity.tradeWealth;
		}
		if (scrCity.tradeCulture < 0f) {
			scrPlayer.cultureVal += scrCity.tradeCulture;
		}
		if (scrCity.tradeMilitary < 0f) {
			scrPlayer.militaryVal += scrCity.tradeMilitary;
		}

		scrCity.commandTag = "trade";
		scrCity.commandTime = (int) (scrWorld.tradeTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	//SEND GIFT
	public void ClickSendGift() {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		string str;

		str = ScrLanguage.Translate ("expedition_name_sendgift_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrConfirmation.textTitle.text = str;

		str = ScrLanguage.Translate ("expedition_desc_sendgift_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		str = str.Replace ("[time]", (scrWorld.sendGiftTime * scrPlayer.modExpeditionTime).ToString ("F0"));
		scrConfirmation.textDescription.text = str;

		str = ScrLanguage.Translate ("expedition_button_sendgift_init");
		str = str.Replace ("[value]", (scrCity.sendGiftCost * scrPlayer.modDiplomacyCost).ToString ("F0"));
		scrConfirmation.textYesButton.text = str;

		scrConfirmation.clickedMethod = SendGift;
		scrConfirmation.buttonYesButton.interactable = (scrPlayer.wealthVal >= (scrCity.sendGiftCost * scrPlayer.modDiplomacyCost));

		scrConfirmation.cancelMethod = Cancel;
		uiConfirmation.SetActive (true);
	}

	void SendGift () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		scrPlayer.wealthVal -= scrCity.sendGiftCost * scrPlayer.modDiplomacyCost;

		scrCity.commandTag = "sendgift";
		scrCity.commandTime = (int) (scrWorld.sendGiftTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	//RAID
	public void ClickRaid () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		string str;

		str = ScrLanguage.Translate ("expedition_name_raid_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrConfirmation.textTitle.text = str;

		float val = scrCity.cityMillitary * scrWorld.modArmyRaid;
		str = ScrLanguage.Translate ("expedition_desc_raid_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		str = str.Replace ("[time]", (scrWorld.raidTime * scrPlayer.modExpeditionTime).ToString ("F0"));
		str = str.Replace ("[value]", val.ToString ("F0"));
		scrConfirmation.textDescription.text = str;

		scrConfirmation.isChoice = true;

		val = scrCity.cityMillitary * scrWorld.modArmyRaid * scrWorld.modArmySmall;
		str = ScrLanguage.Translate ("expedition_buttona_raid_init");
		str = str.Replace ("[value]", val.ToString ("F0"));
		scrConfirmation.textChoiceAButton.text = str;

		scrConfirmation.choiceAMethod = RaidSmall;
		scrConfirmation.buttonChoiceAButton.interactable = (scrPlayer.militaryVal >= val);

		val = scrCity.cityMillitary * scrWorld.modArmyRaid * scrWorld.modArmyLarge;
		str = ScrLanguage.Translate ("expedition_buttonb_raid_init");
		str = str.Replace ("[value]", val.ToString ("F0"));
		scrConfirmation.textChoiceBButton.text = str;

		scrConfirmation.choiceBMethod = RaidLarge;
		scrConfirmation.buttonChoiceBButton.interactable = (scrPlayer.militaryVal >= val);

		scrConfirmation.cancelMethod = Cancel;
		uiConfirmation.SetActive (true);
	}

	void RaidSmall () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		float val = scrCity.cityMillitary * scrWorld.modArmyRaid * scrWorld.modArmySmall;
		scrPlayer.militaryVal -= val;
		scrCity.playerMilitary = val;

		scrCity.cityRelation -= 1;
		if (scrCity.cityRelation < 0) {
			scrCity.cityRelation = 0;
		}

		scrCity.commandTag = "raid";
		scrCity.commandTime = (int) (scrWorld.raidTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	void RaidLarge() {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		float val = scrCity.cityMillitary * scrWorld.modArmyRaid * scrWorld.modArmyLarge;
		scrPlayer.militaryVal -= val;
		scrCity.playerMilitary = val;

		scrCity.cityRelation -= 1;
		if (scrCity.cityRelation < 0) {
			scrCity.cityRelation = 0;
		}

		scrCity.commandTag = "raid";
		scrCity.commandTime = (int) (scrWorld.raidTime * scrPlayer.modExpeditionTime);
		scrController.Calculate ();
	}

	//break
	public void ClickBreak () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();
		string str;

		scrConfirmation.textTitle.text = ScrLanguage.Translate ("expedition_name_break_init");

		str = ScrLanguage.Translate ("expedition_desc_break_init");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrConfirmation.textDescription.text = str;

		scrConfirmation.textYesButton.text = ScrLanguage.Translate ("expedition_button_break_init");
		scrConfirmation.clickedMethod = Break;
		scrConfirmation.buttonYesButton.interactable = true;

		scrConfirmation.cancelMethod = Cancel;
		uiConfirmation.SetActive (true);
	}

	void Break () {
		ScrCity scrCity = scrController.objSelectedCity.GetComponent <ScrCity> ();

		scrCity.isAlly = false;
		scrCity.isVassal = false;

		scrCity.cityRelation -= 1;
		if (scrCity.cityRelation < 0) {
			scrCity.cityRelation = 0;
		}
	}

	void Cancel () {
		gameObject.SetActive (true);
	}
}
