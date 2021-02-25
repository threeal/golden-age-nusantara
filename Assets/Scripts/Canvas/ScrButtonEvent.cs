using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrButtonEvent : MonoBehaviour {

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject uiConfirmation;
	ScrCanvasConfirmation scrConfirmation;

	GameObject uiWar;
	ScrCanvasWar scrWar;

	public string eventTag = "";

	public bool isRandom = false;
	public bool isTimed = false;
	public bool isWar = false;
	public bool isTask = false;

	public int eventTurn = -1;
	public int eventTime = 0;

	public float chance = 0f;

	public string warNameKey;
	public string warEnemyKey;

	public float enemy = 0f;
	public float ally = 0f;

	public bool eventDisplayed = false;
	public bool eventFinished = false;
	public bool eventFailed = false;

	public string eventNameKey = "";
	public string eventNameFinishedKey = "";
	public string eventDescKey = "";
	public string eventDescFinishedKey = "";
	public string eventDescFailedKey = "";

	public GameObject uiCount;
	Text textCount;

	// Use this for initialization
	void Start () {
		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		uiConfirmation = scrCanvas.uiConfirmation;
		scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();

		uiWar = scrCanvas.uiConfirmation;
		scrWar = uiWar.GetComponent <ScrCanvasWar> ();

		textCount = uiCount.GetComponentInChildren <Text> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		scrCanvas.Activate(uiCount, (eventTime > 0 && isTask));
		if (uiCount) {
			textCount.text = eventTime.ToString ("F0");
		}
	}

	public void Click () {
		ChallengeView ();
	}

	public void ChallengeView () {
		string str;

		if (!scrConfirmation) {
			objCanvas = GameObject.Find ("Canvas");
			scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

			uiConfirmation = scrCanvas.uiConfirmation;
			scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();
		}

		scrConfirmation.textTitle.text = ScrLanguage.Translate (eventNameKey);
		if (isTask && isTimed) {
			scrConfirmation.textTitle.text += " ( " + eventTime.ToString ("F0") + " " + ScrLanguage.Translate ("misc_turns") + ")";
		}

		str = ScrLanguage.Translate (eventDescKey);
		str = str.Replace ("[army]", enemy.ToString ("F0"));
		scrConfirmation.textDescription.text = str;

		if (isWar) {
			scrConfirmation.isChoice = true;

			scrConfirmation.cancelMethod = ChallengeOk;

			scrConfirmation.textChoiceAButton.text = ScrLanguage.Translate ("ui_attack");
			scrConfirmation.choiceAMethod = WarOk;
			scrConfirmation.buttonChoiceAButton.interactable = true;

			scrConfirmation.textChoiceBButton.text = ScrLanguage.Translate ("ui_later");
			scrConfirmation.choiceBMethod = ChallengeOk;
			scrConfirmation.buttonChoiceBButton.interactable = true;
		} else {
			scrConfirmation.cancelMethod = ChallengeOk;

			scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_ok");
			scrConfirmation.clickedMethod = ChallengeOk;
			scrConfirmation.buttonYesButton.interactable = true;
		}


		scrCanvas.uiConfirmation.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	public void FinishedView () {
		if (!scrConfirmation) {
			objCanvas = GameObject.Find ("Canvas");
			scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

			uiConfirmation = scrCanvas.uiConfirmation;
			scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();
		}

		if (eventNameFinishedKey != "") {
			scrConfirmation.textTitle.text = ScrLanguage.Translate (eventNameFinishedKey);
		} else {
			scrConfirmation.textTitle.text = ScrLanguage.Translate (eventNameKey);
		}

		if (eventFailed) {
			if (isTask) {
				scrConfirmation.textTitle.text += " (" + ScrLanguage.Translate ("misc_failed") + ")";
			}
			if (eventDescFailedKey != "") { 
				scrConfirmation.textDescription.text = ScrLanguage.Translate (eventDescFailedKey);
			} else {
				scrConfirmation.textDescription.text = ScrLanguage.Translate (eventDescFinishedKey);
			}
		} else {
			if (isTask) {
				scrConfirmation.textTitle.text += " (" + ScrLanguage.Translate ("misc_finished") + ")";
			}
			scrConfirmation.textDescription.text = ScrLanguage.Translate (eventDescFinishedKey);
		}

		scrConfirmation.clickedMethod = FinishedOk;
		scrConfirmation.cancelMethod = FinishedOk;

		scrConfirmation.buttonYesButton.interactable = true;
		scrConfirmation.textYesButton.text = ScrLanguage.Translate ("ui_ok");

		scrCanvas.uiConfirmation.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	void ChallengeOk () {
		eventDisplayed = true;
		scrCanvas.uiEmpty.SetActive (true);
	}

	public void WarStart () {
		WarOk ();
	}

	void WarOk () {
		if (!scrWar) {
			objCanvas = GameObject.Find ("Canvas");
			scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

			uiWar = scrCanvas.uiWar;
			scrWar = uiWar.GetComponent <ScrCanvasWar> ();
		}
		string str;

		ally = ally + scrPlayer.militaryVal;
		scrPlayer.militaryVal = 0f;

		enemy = enemy * Random.Range (0.8f, 1.2f);

		str = ScrLanguage.Translate ("challenge_war_name");
		str = str.Replace ("[name]", ScrLanguage.Translate (warEnemyKey));
		scrWar.textTitle.text = str;

		scrWar.textPlayer.text = ScrLanguage.Translate ("city_name_player");
		scrWar.textEnemy.text = ScrLanguage.Translate (warEnemyKey);

		str = ScrLanguage.Translate ("challenge_war_start");
		str = str.Replace ("[name]", ScrLanguage.Translate (warEnemyKey));
		scrWar.descStart = str;

		scrWar.playerArmy = ally;
		scrWar.enemyArmy = enemy;

		bool turn = (Random.value < ally/((ally + enemy > 0) ? (ally + enemy) : 1));
		while (ally > 0 && enemy > 0) {
			if (turn) {
				enemy -= ally * Random.Range (0, 0.2f) * scrPlayer.modCombatStrength;
			}
			else {
				ally -= enemy * Random.Range (0, 0.2f);
			}
			turn = !turn;
		}

		if (enemy <= 0f) {
			enemy = 0f;
			scrWar.playerTarget = ally;
			scrWar.enemyTarget = enemy;

			str = ScrLanguage.Translate ("challenge_war_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (warEnemyKey));
			str = str.Replace ("[val1]", (scrWar.playerArmy - ally).ToString ("F0"));
			str = str.Replace ("[val2]", (scrWar.enemyArmy - enemy).ToString ("F0"));
			scrWar.descFinished = str;

			scrPlayer.militaryVal += ally;

			eventFailed = false;
		}
		else {
			scrWar.playerTarget = ally;
			scrWar.enemyTarget = enemy;

			str = ScrLanguage.Translate ("challenge_war_failed");
			str = str.Replace ("[name]", ScrLanguage.Translate (warEnemyKey));
			str = str.Replace ("[val1]", (scrWar.playerArmy - ally).ToString ("F0"));
			str = str.Replace ("[val2]", (scrWar.enemyArmy - enemy).ToString ("F0"));
			scrWar.descFinished = str;

			eventFailed = true;
		}
			
		GameObject.Find ("Challenge").GetComponent <ScrChallenge> ().AfterWar (eventTag, eventFailed);
		eventFinished = true;
		eventDisplayed = false;
		scrWar.LateUpdate ();
		scrCanvas.uiWar.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	void FinishedOk () {;
		eventFinished = true;
		eventDisplayed = true;

		scrCanvas.uiEmpty.SetActive (true);

		gameObject.SetActive (false);
	}
}
