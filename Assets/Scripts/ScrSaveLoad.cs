using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class ScrSaveLoad : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objChallenge;
	ScrChallenge scrChallenge;

	public string pathFile;
	public string pathHex;
	public string pathBuild;
	public GameObject objMap;

	public GameObject[] objPrefabs;

	Dictionary<int, GameObject> ObjectIds = new Dictionary<int, GameObject> ();

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objChallenge = GameObject.Find ("Challenge");
		scrChallenge = objChallenge.GetComponent <ScrChallenge> ();
	}
	
	ScrDataGame SaveDataGame () {
		ScrDataGame data = new ScrDataGame ();

		data.reignNameKey = scrPlayer.reignNameKey;
		data.enableWorld = scrPlayer.enableWorld;

		data.turnCount = scrPlayer.turnCount;

		data.populationVal = scrPlayer.populationVal;
		data.populationIdle = scrPlayer.populationIdle;
		data.populationMax = scrPlayer.populationMax;
		data.populationCost = scrPlayer.populationCost;

		data.fleetVal = scrPlayer.fleetVal;
		data.fleetIdle = scrPlayer.fleetIdle;
		data.fleetMax = scrPlayer.fleetMax;
		data.fleetCost = scrPlayer.fleetCost;
		data.fleetProduction = scrPlayer.fleetProduction;
		data.fleetProductionCost = scrPlayer.fleetProductionCost;

		data.foodVal = scrPlayer.foodVal;
		data.productionVal = scrPlayer.productionVal;
		data.wealthVal = scrPlayer.wealthVal;
		data.cultureVal = scrPlayer.cultureVal;
		data.militaryVal = scrPlayer.militaryVal;

		data.perPopFood = scrPlayer.perPopFood;
		data.perPopProduction = scrPlayer.perPopProduction;
		data.perPopWealth = scrPlayer.perPopWealth;
		data.perPopCulture = scrPlayer.perPopCulture;
		data.perPopMilitary = scrPlayer.perPopMilitary;

		data.modBuildUpkeep = scrPlayer.modBuildUpkeep;
		data.modBuiltCost = scrPlayer.modBuiltCost;
		data.modBuiltTime = scrPlayer.modBuiltTime;
		data.modFoodSurplus = scrPlayer.modFoodSurplus;
		data.modProductionSurplus = scrPlayer.modProductionSurplus;
		data.modWealthSurplus = scrPlayer.modWealthSurplus;
		data.modCultureSurplus = scrPlayer.modCultureSurplus;
		data.modMilitarySurplus = scrPlayer.modMilitarySurplus;

		data.modExplorationTime = scrPlayer.modExplorationTime;
		data.modPopulationCost = scrPlayer.modPopulationCost;
		data.modTechnologyCost = scrPlayer.modTechnologyCost;
		data.modExpeditionTime = scrPlayer.modExpeditionTime;
		data.modTradeBonus = scrPlayer.modTradeBonus;
		data.modDiplomacyCost = scrPlayer.modDiplomacyCost;
		data.modCombatStrength = scrPlayer.modCombatStrength;

		data.dataHex = SaveDataHex ();
		data.dataTech = SaveDataTech ();
		data.dataEvent = SaveDataEvent ();
		data.dataCity = SaveDataCity ();

		return data;
	}

	List<ScrDataHex> SaveDataHex () {
		List<ScrDataHex> datas = new List<ScrDataHex> ();

		for (int i = 0; i < objMap.transform.childCount; i++) {
			if (objMap.transform.GetChild (i).tag == "Hex") {
				datas.Add (HexSave (objMap.transform.GetChild (i).gameObject));
			}
		}

		return datas;
	}

	ScrDataHex HexSave (GameObject hex) {
		ScrDataHex data = new ScrDataHex ();
		ScrHex scr = hex.GetComponent <ScrHex> ();

		data.prefab = scr.prefab;
		data.id = hex.GetInstanceID ();
		data.scriptActive = scr.isActiveAndEnabled;

		data.name = hex.name;
		data.position = hex.transform.localPosition;
		data.rotation = hex.transform.localRotation;
		data.scale = hex.transform.localScale;

		data.isReveal = scr.isReveal;

		data.hexNearbyId = new int[scr.hexNearby.Length];
		for (int i = 0; i < scr.hexNearby.Length; i++) {
			data.hexNearbyId [i] = LinkerSave (scr.hexNearby [i]);
		}

		data.unifiedId = new int[scr.unified.Length];
		for (int i = 0; i < scr.unified.Length; i++) {
			data.unifiedId[i] = LinkerSave (scr.unified[i]);
		}

		data.objBuildId = LinkerSave (scr.objBuild);

		//child
		data.childs = new List<ScrDataBuild> ();
		for (int i = 0; i < hex.transform.childCount; i++) {
			if (hex.transform.GetChild (i).tag == "Build") {
				data.childs.Add (BuildSave (hex.transform.GetChild (i).gameObject));
			}
		}

		return data;
	}

	ScrDataBuild BuildSave (GameObject build) {
		ScrDataBuild data = new ScrDataBuild ();
		ScrBuild scr = build.GetComponent <ScrBuild> ();

		data.prefab = scr.prefab;
		data.id = build.GetInstanceID ();
		data.objectActive = build.activeInHierarchy;
		data.scriptActive = scr.isActiveAndEnabled;

		data.name = build.name;
		data.position = build.transform.localPosition;
		data.rotation = build.transform.localRotation;
		data.scale = build.transform.localScale;

		data.buildNameKey = scr.buildNameKey;
		data.buildDescKey = scr.buildDescKey;
		data.buildNeedKey = scr.buildNeedKey;
		data.buildEffectKey = scr.buildEffectKey;

		data.isWorked = scr.isWorked;
		data.builtcost = scr.builtCost;
		data.builtTime = scr.builtTime;

		data.clearObjectId = LinkerSave (scr.clearObject);
		data.builtObjectId = LinkerSave (scr.builtObject);
		data.builtInfoId = LinkerSave (scr.builtInfo);
		data.objHexId = LinkerSave (scr.objHex);

		//child
		data.childs = new List<ScrDataBuild> ();
		for (int i = 0; i < build.transform.childCount; i++) {
			if (build.transform.GetChild (i).tag == "Build") {
				data.childs.Add (BuildSave(build.transform.GetChild (i).gameObject));
			}
		}

		return data;
	}

	int LinkerSave (GameObject obj) {
		if (obj) {
			return obj.GetInstanceID ();
		} else {
			return -1;
		}
	}

	List<ScrDataTech> SaveDataTech () {
		List<ScrDataTech> datas = new List<ScrDataTech> ();
		GameObject[] techs = GameObject.FindGameObjectsWithTag ("Tech");

		foreach (GameObject tech in techs) {
			ScrDataTech data = new ScrDataTech ();
			ScrTech scr = tech.GetComponent <ScrTech> ();

			data.techTag = scr.techTag;
			data.isResearched = scr.isResearched;
			datas.Add (data);
		}

		return datas;
	}
		
	List<ScrDataEvent> SaveDataEvent () {
		List<ScrDataEvent> datas = new List<ScrDataEvent> ();

		foreach (GameObject objEvent in scrChallenge.listEvent) {
			ScrDataEvent data = new ScrDataEvent();
			ScrButtonEvent scr = objEvent.GetComponent <ScrButtonEvent> ();

			data.eventTag = scr.eventTag;

			data.eventTime = scr.eventTime;

			data.enemy = scr.enemy;
			data.ally = scr.ally;

			data.eventDisplayed = scr.eventDisplayed;
			data.eventFinished = scr.eventFinished;
			data.eventFailed = scr.eventFailed;

			data.isActive = objEvent.activeSelf;

			datas.Add (data);
		}

		return datas;
	}

	List<ScrDataCity> SaveDataCity () {
		List<ScrDataCity> datas = new List<ScrDataCity> ();
		bool isTrue = scrController.objWorld.activeInHierarchy;
		scrController.objWorld.SetActive (true);
		GameObject[] cities = GameObject.FindGameObjectsWithTag ("City");

		foreach (GameObject city in cities) {
			ScrDataCity data = new ScrDataCity ();
			ScrCity scr = city.GetComponent <ScrCity> ();

			data.cityTag = scr.cityTag;
			data.cityRelation = scr.cityRelation;
			data.cityMillitary = scr.cityMillitary;
			data.playerMilitary = scr.playerMilitary;
			data.isPlayer = scr.isPlayer;
			data.isAlly = scr.isAlly;
			data.isVassal = scr.isVassal;
			data.commandTag = scr.commandTag;
			data.commandTime = scr.commandTime;

			datas.Add (data);
		}

		scrController.objWorld.SetActive (isTrue);
		return datas;
	}

	void LoadDataGame (ScrDataGame data) {

		scrPlayer.reignNameKey = data.reignNameKey;
		scrPlayer.enableWorld = data.enableWorld;

		scrPlayer.turnCount = data.turnCount;

		scrPlayer.populationVal = data.populationVal;
		scrPlayer.populationIdle = data.populationIdle;
		scrPlayer.populationMax = data.populationMax;
		scrPlayer.populationCost = data.populationCost;

		scrPlayer.fleetVal = data.fleetVal;
		scrPlayer.fleetIdle = data.fleetIdle;
		scrPlayer.fleetMax = data.fleetMax;
		scrPlayer.fleetCost = data.fleetCost;
		scrPlayer.fleetProduction = data.fleetProduction;
		scrPlayer.fleetProductionCost = data.fleetProductionCost;

		scrPlayer.foodVal = data.foodVal;
		scrPlayer.productionVal = data.productionVal;
		scrPlayer.wealthVal = data.wealthVal;
		scrPlayer.cultureVal = data.cultureVal;
		scrPlayer.militaryVal = data.militaryVal;

		scrPlayer.perPopFood = data.perPopFood;
		scrPlayer.perPopProduction = data.perPopProduction;
		scrPlayer.perPopWealth = data.perPopWealth;
		scrPlayer.perPopCulture = data.perPopCulture;
		scrPlayer.perPopMilitary = data.perPopMilitary;

		scrPlayer.modBuildUpkeep = data.modBuildUpkeep;
		scrPlayer.modBuiltCost = data.modBuiltCost;
		scrPlayer.modBuiltTime = data.modBuiltTime;
		scrPlayer.modFoodSurplus = data.modFoodSurplus;
		scrPlayer.modProductionSurplus = data.modProductionSurplus;
		scrPlayer.modWealthSurplus = data.modWealthSurplus;
		scrPlayer.modCultureSurplus = data.modCultureSurplus;
		scrPlayer.modMilitarySurplus = data.modMilitarySurplus;

		scrPlayer.modExplorationTime = data.modExplorationTime;
		scrPlayer.modPopulationCost = data.modPopulationCost;
		scrPlayer.modTechnologyCost = data.modTechnologyCost;
		scrPlayer.modExpeditionTime = data.modExpeditionTime;
		scrPlayer.modTradeBonus = data.modTradeBonus;
		scrPlayer.modDiplomacyCost = data.modDiplomacyCost;
		scrPlayer.modCombatStrength = data.modCombatStrength;

		ClearIds ();
		LoadDataHexInstantiate (data.dataHex);
		LoadDataHexRelink (); 
		LoadDataHexReset ();
		LoadDataCity (data.dataCity);
		LoadDataTech (data.dataTech);
		LoadDataEvent (data.dataEvent);

		scrController.Calculate ();
	}

	void ClearIds () {
		ObjectIds.Clear ();

		foreach (GameObject prefab in objPrefabs) {
			if (!ObjectIds.ContainsKey (prefab.GetInstanceID ())) {
				ObjectIds.Add (prefab.GetInstanceID (), prefab);
			}
		}

		foreach (GameObject build in scrController.listBuild) {
			if (!ObjectIds.ContainsKey (build.GetInstanceID ())) {
				ObjectIds.Add (build.GetInstanceID (), build);
			}
		}
	}

	void LoadDataHexInstantiate (List<ScrDataHex> datas) {
		//destroy old hex
		GameObject[] hexes = GameObject.FindGameObjectsWithTag ("Hex");
		foreach (GameObject hex in hexes) {
			DestroyImmediate (hex);
		}

		//instantiate
		foreach (ScrDataHex data in datas) {
			HexInstantiate (data, objMap);
		}
	}

	void HexInstantiate (ScrDataHex data, GameObject parent) {
		GameObject hex = Instantiate (Resources.Load (pathHex + data.prefab, typeof(GameObject))) as GameObject;
		ScrHex scr = hex.GetComponent <ScrHex> ();
		scr.isLoad = true;

		if (!ObjectIds.ContainsKey (data.id)) {
			ObjectIds.Add (data.id, hex);
		}

		hex.name = data.name;
		hex.transform.SetParent (parent.transform);
		hex.transform.localPosition = data.position;
		hex.transform.localRotation = data.rotation;
		hex.transform.localScale = data.scale;

		scr.isReveal = data.isReveal;
		scr.hexNearbyId = data.hexNearbyId;
		scr.unifiedId = data.unifiedId;
		scr.objBuildId = data.objBuildId;

		foreach (ScrDataBuild child in data.childs) {
			BuildInstantiate (child, hex);
		}

		scr.enabled = data.scriptActive;
	}

	void BuildInstantiate (ScrDataBuild data, GameObject parent) {
		GameObject build = Instantiate (Resources.Load (pathBuild + data.prefab, typeof(GameObject))) as GameObject;
		ScrBuild scr = build.GetComponent <ScrBuild> ();
		scr.isLoad = true;

		if (!ObjectIds.ContainsKey (data.id)) {
			ObjectIds.Add (data.id, build);
		}

		build.name = data.name;
		build.transform.SetParent (parent.transform);
		build.transform.localPosition = data.position;
		build.transform.localRotation = data.rotation;
		build.transform.localScale = data.scale;

		scr.buildNameKey = data.buildNameKey;
		scr.buildDescKey = data.buildDescKey;
		scr.buildNeedKey = data.buildNeedKey;
		scr.buildEffectKey = data.buildEffectKey;

		scr.isWorked = data.isWorked;
		scr.builtCost = data.builtcost;
		scr.builtTime = data.builtTime;

		scr.clearObjectId = data.clearObjectId;
		scr.builtObjectId = data.builtObjectId;
		scr.builtInfoId = data.builtInfoId;
		scr.objHexId = data.objHexId;

		foreach (ScrDataBuild child in data.childs) {
			BuildInstantiate (child, build);
		}

		scr.enabled = data.scriptActive;
		build.SetActive (data.objectActive);
	}

	void LoadDataHexRelink () {
		GameObject[] hexes = GameObject.FindGameObjectsWithTag ("Hex");
		foreach (GameObject hex in hexes) {
			ScrHex scr = hex.GetComponent <ScrHex> ();

			scr.hexNearby = new GameObject[scr.hexNearbyId.Length];
			for (int i = 0; i < scr.hexNearbyId.Length; i++) {
				scr.hexNearby [i] = LoadRelink (scr.hexNearbyId [i]);
			}

			scr.unified = new GameObject[scr.unifiedId.Length];
			for (int i = 0; i < scr.unifiedId.Length; i++) {
				scr.unified [i] = LoadRelink (scr.unifiedId [i]);
			}

			scr.objBuild = LoadRelink (scr.objBuildId);
		}

		GameObject[] builds = GameObject.FindGameObjectsWithTag ("Build");
		foreach (GameObject build in builds) {
			ScrBuild scr = build.GetComponent <ScrBuild> ();

			scr.clearObject = LoadRelink (scr.clearObjectId);
			scr.builtObject = LoadRelink (scr.builtObjectId);
			scr.builtInfo = LoadRelink (scr.builtInfoId);
			scr.objHex = LoadRelink (scr.objHexId);
		}
	}

	void LoadDataHexReset () {
		//reset hex list
		scrController.listHex = GameObject.FindGameObjectsWithTag ("Hex");

		//destroy old icon
		GameObject[] icons = GameObject.FindGameObjectsWithTag ("Icon");
		foreach (GameObject icon in icons) {
			Destroy (icon);
		}

		//add icon to all hex
		foreach (GameObject hex in scrController.listHex) {
			GameObject obj = (GameObject) Instantiate ((Object)scrController.uiIcon);
			obj.transform.SetParent (scrController.uiIcons.transform);
			obj.GetComponent <ScrCanvasIcon> ().objHex = hex;
		}
	}

	GameObject LoadRelink (int targetId) {
		if (targetId != -1 && ObjectIds.ContainsKey (targetId)) {
			GameObject obj;
			if (ObjectIds.TryGetValue (targetId, out obj)) {
				return obj;
			}
		}

		return null;
	}

	void LoadDataTech (List<ScrDataTech> datas) {
		GameObject[] techs = GameObject.FindGameObjectsWithTag ("Tech");

		foreach (GameObject tech in techs) {
			ScrTech scr = tech.GetComponent <ScrTech> ();

			foreach (ScrDataTech data in datas) {
				if (scr.techTag == data.techTag) {
					scr.isResearched = data.isResearched;
					foreach (GameObject hex in scrController.listHex) {
						scr.EffectOnBuild (hex.GetComponent <ScrHex> ().objBuild);
					}
					break;
				}
			}
		}
	}

	void LoadDataEvent (List<ScrDataEvent> datas) {
		foreach (GameObject objEvent in scrChallenge.listEvent) {
			ScrButtonEvent scr = objEvent.GetComponent <ScrButtonEvent> ();

			foreach (ScrDataEvent data in datas) {
				if (scr.eventTag == data.eventTag) {

					scr.eventTime = data.eventTime;

					scr.enemy = data.enemy;
					scr.ally = data.ally;

					scr.eventDisplayed = data.eventDisplayed;
					scr.eventFinished = data.eventFinished;
					scr.eventFailed = data.eventFailed;

					objEvent.SetActive (data.isActive);
					break;
				}
			}
		}
	}

	void LoadDataCity (List<ScrDataCity> datas) {
		bool isTrue = scrController.objWorld.activeInHierarchy;
		scrController.objWorld.SetActive (true);
		GameObject[] cities = GameObject.FindGameObjectsWithTag ("City");

		foreach (GameObject city in cities) {
			ScrCity scr = city.GetComponent <ScrCity> ();

			foreach (ScrDataCity data in datas) {
				if (scr.cityTag == data.cityTag) {
					scr.cityRelation = data.cityRelation;
					scr.cityMillitary = data.cityMillitary;
					scr.playerMilitary = data.playerMilitary;
					scr.isPlayer = data.isPlayer;
					scr.isAlly = data.isAlly;
					scr.isVassal = data.isVassal;
					scr.commandTag = data.commandTag;
					scr.commandTime = data.commandTime;

					break;
				}
			}
		}

		scrController.objWorld.SetActive (isTrue);
	}

	public void Save () {
		ScrDataGame data = SaveDataGame ();
		XmlSerializer serializer = new XmlSerializer (typeof(ScrDataGame));
		FileStream file = new FileStream (Application.persistentDataPath + pathFile, FileMode.Create);
		serializer.Serialize (file, data);
		file.Close ();

		Debug.Log ("Saved");
	}

	public void Load () {
		if (File.Exists (Application.persistentDataPath + pathFile)) {
			XmlSerializer serializer = new XmlSerializer (typeof(ScrDataGame));
			FileStream file = new FileStream (Application.persistentDataPath + pathFile, FileMode.Open);
			LoadDataGame ((ScrDataGame)serializer.Deserialize (file));
			file.Close ();
			Debug.Log ("Loaded");
		} else {
			Debug.Log ("No File");
		}
	}
}
