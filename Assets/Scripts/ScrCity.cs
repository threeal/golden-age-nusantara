using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrCity : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject objWorld;
	ScrWorld scrWorld;

	GameObject objChallenge;
	ScrChallenge scrChallenge;

	public string cityTag;

	public string cityNameKey;
	public string cityDescKey;
	public string tradeDescKey;

	public int cityRelation;
	public float cityMillitary;

	public float tradeFood;
	public float tradeProduction;
	public float tradeWealth;
	public float tradeCulture;
	public float tradeMilitary;

	public float sendGiftCost;
	public float allianceCost;

	public float playerMilitary;

	public bool isPlayer;
	public bool isAlly;
	public bool isVassal;

	public string commandTag = "";
	public int commandTime = 0;

	public Material[] materials;
	MeshRenderer[] meshes;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		objWorld = GameObject.Find ("World");
		scrWorld = objWorld.GetComponent <ScrWorld> ();

		objChallenge = GameObject.Find ("Challenge");
		scrChallenge = objChallenge.GetComponent <ScrChallenge> ();

		meshes = GetComponentsInChildren <MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayer) {
			ChangeMaterial (materials[2]);
		} else {
			if (isAlly) {
				ChangeMaterial (materials[3]);
			} else if (cityRelation >= 2) {
				ChangeMaterial (materials[2]);				
			} else if (cityRelation >= 1) {
				ChangeMaterial (materials[1]);			
			} else {
				ChangeMaterial (materials[0]);
			}
		}
	}

	void ChangeMaterial (Material material) {
		foreach (MeshRenderer mesh in meshes) {
			mesh.material = material;
		}
	}

	public void Selected () {
		scrController.objSelectedCity = gameObject;
		scrCanvas.uiDiplomacy.SetActive (true);
	}

	public void Command() {
		if (commandTag == "alliance") {
			scrChallenge.AllianceEvent (gameObject);
		} else if (commandTag == "invade") {
			scrChallenge.InvadeEvent (gameObject);
		} else if (commandTag == "trade") {
			scrChallenge.TradeEvent (gameObject);
		} else if (commandTag == "sendgift") {
			scrChallenge.SendGiftEvent (gameObject);
		} else if (commandTag == "raid") {
			scrChallenge.RaidEvent (gameObject);
		}
	}

	public string DescReplace (string str) {
		str = str.Replace ("[city_name]", ScrLanguage.Translate (cityNameKey));
		str = str.Replace ("[command_time]", commandTime.ToString ("D"));

		str = str.Replace ("[tribute_food]", (tradeFood * scrWorld.modVassalTribute).ToString ("F1"));
		str = str.Replace ("[tribute_production]", (tradeProduction * scrWorld.modVassalTribute).ToString ("F1"));
		str = str.Replace ("[tribute_wealth]", (tradeWealth * scrWorld.modVassalTribute).ToString ("F1"));
		str = str.Replace ("[tribute_culture]", (tradeCulture * scrWorld.modVassalTribute).ToString ("F1"));
		str = str.Replace ("[tribute_military]", (tradeMilitary * scrWorld.modVassalTribute).ToString ("F1"));

		return str;
	}
}
