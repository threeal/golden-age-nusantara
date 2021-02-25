using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrWorld : MonoBehaviour {

	public GameObject[] objCities;
	public Dictionary<string, GameObject> dictionaryCity;

	public int allianceTime;
	public int invadeTime;
	public int tradeTime;
	public int sendGiftTime;
	public int raidTime;

	public float modArmySmall;
	public float modArmyLarge;
	public float modArmyRaid;
	public float modVassalTribute;


	// Use this for initialization
	void Start () {
		objCities = GameObject.FindGameObjectsWithTag ("City");
		dictionaryCity = new Dictionary<string, GameObject> ();

		foreach (GameObject objCity in objCities) {
			ScrCity scrCity = objCity.GetComponent <ScrCity> ();
			dictionaryCity.Add (scrCity.cityTag, objCity);
		}

		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CityRelation (string key, int relation, bool isAlly, bool isVassal) {
		GameObject obj;
		if (dictionaryCity.TryGetValue (key, out obj)) {
			if (obj) {
				ScrCity scr = obj.GetComponent <ScrCity> ();
				scr.cityRelation = relation;
				scr.isAlly = isAlly;
				scr.isVassal = isVassal;
			}
		}
	}
}
