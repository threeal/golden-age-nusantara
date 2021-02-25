using UnityEngine;
using System.Collections;

public class ScrBuild : MonoBehaviour {

	public bool isLoad = false;
	public string prefab;
	public int id;

	public string buildNameKey;
	public string builtNameKey;
	public string clearNameKey;
	public string buildDescKey;
	public string clearDescKey;
	public string buildNeedKey;
	public string buildEffectKey;
	
	public string buildTag = "tag";

	public bool isBuilding = true;
	public bool isDeco = false;
	public bool isStatic = false;
	public bool isRotateable = true;
	public bool isBuilt = false;
	public bool isWorkable = false;
	public bool isClearable = true;
	public bool isWorked = false;
	public bool isFreshWater = false;

	public string needBuildInTag = "";
	public string needNearbyTag = "";
	public int needNearbyCount = 0;
	public bool needNearbySelf = false;
	public bool needWater = false;

	public GameObject clearObject;
	public int clearObjectId;
	public GameObject builtObject;
	public int builtObjectId;
	public GameObject builtInfo;
	public int builtInfoId;
	public int builtTime = 0;
	public float builtCost = 0;
	public float buildUpkeep = 0;

	public int populationMax = 0;
	public int fleetMax = 0;

	public float generateFood = 0;
	public float generateProduction = 0;
	public float generateWealth = 0;
	public float generateCulture = 0;
	public float generateMilitary = 0;

	public string perBuildingTag = "";
	public int perBuildingRange = 0;
	public float perBuildingFood = 0;
	public float perBuildingProduction = 0;
	public float perBuildingWealth = 0;
	public float perBuildingCulture = 0;
	public float perBuildingMilitary = 0;

	public GameObject objHex;
	public int objHexId;

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		//built info
		if (!builtInfo) {
			foreach (GameObject objBuild in scrController.listBuild) {
				if (objBuild.GetComponent <ScrBuild> ().prefab == prefab) {
					builtInfo = objBuild;
				}
			}
		}

		if (!isLoad) {
			if (!objHex) {
				objHex = transform.parent.gameObject;
				objHex.GetComponent <ScrHex> ().objBuild = gameObject;
				Positioning ();
			}

			//auto worked
			if (!isWorked && isWorkable && (buildTag != "cloud" || isBuilt)) {
				if (scrPlayer.populationIdle > 0) {
					isWorked = true;
				}
			}

			//tech bonus modifier
			foreach (GameObject objTech in scrController.listTech) {
				if (objTech) {
					if (objTech.activeInHierarchy) {
						objTech.GetComponent <ScrTech> ().EffectOnBuild (gameObject);
					}
				}
			}

			//built modifier
			if (buildTag != "cloud") {
				builtTime = (int)(builtTime * scrPlayer.modBuiltTime);
			}

			scrController.Calculate ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Positioning () {
		Vector3 pos = transform.position;
		pos.y = 1000f;
		RaycastHit hit;
		if (Physics.Raycast (pos, Vector3.down, out hit, Mathf.Infinity, 1 << 8)) {
			objHex = hit.collider.gameObject;
			transform.SetParent (objHex.transform);

			if (buildTag == "cloud") {
				pos.y = 0f;
			} else {
				pos.y = hit.point.y;
			}
			transform.position = pos;

			if (!isStatic) {
				if (isRotateable) {
					transform.RotateAround (transform.position, Vector3.up, Mathf.Round (Random.Range (0f, 360f) / 60f) * 60f);
				} else {
					transform.rotation = objHex.transform.rotation;
				}
			}

		} else {
			Destroy (gameObject);
		}
	}

	public bool Buildable (GameObject Hex) {
		bool isTrue;

		ScrHex scrHex = Hex.GetComponent <ScrHex> ();

		if (needNearbyCount > 0) {
			int count = needNearbyCount;
			foreach (GameObject hex in scrHex.hexNearby) {
				if (hex) {
					GameObject build = hex.GetComponent <ScrHex> ().objBuild;

					//for cloud
					if (needNearbyTag == "cloud") {
						if (hex.GetComponent <ScrHex> ().isReveal) {
							count -= 1;
						}
					} else if (hex.GetComponent <ScrHex> ().isReveal) {
						if (needNearbyTag == "freshwater") {
							if (hex.GetComponent <ScrHex> ().isFreshWater) {
								count -= 1;
							} else if (build) {
								ScrBuild scr = build.GetComponent <ScrBuild> ();
								if (scr.isFreshWater) {
									count -= 1;
								}
							}
						}
						else if (build) {
							ScrBuild scr = build.GetComponent <ScrBuild> ();
							if (needNearbySelf && scr.buildTag == buildTag) {
								return false;
							}
							else if (scr.buildTag == needNearbyTag) {
								count -= 1;
							}
						}

					}
				}
			}
			if (needNearbyTag == "cloud" && count <= 0) {
				return true;
			}
			else if (count > 0) {
				return false;
			}
		}

		if (scrHex.isWater) {
			if (needWater && !scrHex.isFreshWater) {
				isTrue = false;
				foreach (GameObject hex in scrHex.hexNearby) {
					if (!isTrue && hex) {
						if (!hex.GetComponent <ScrHex> ().isWater) {
							isTrue = true;
						}
					}
				}
				if (!isTrue) {
					return false;
				}
			} else {
				return false;
			}
		} else if (needWater) {
			return false;
		}

		if (needBuildInTag != "") {
			if (scrHex.objBuild) {
				ScrBuild scrBuild = scrHex.objBuild.GetComponent <ScrBuild> ();
				if (scrBuild.buildTag != needBuildInTag) {
					return false;
				}
			} else {
				return false;
			}
		}else if (scrHex.objBuild) {
			if (!scrHex.objBuild.GetComponent <ScrBuild> ().isDeco) {
				return false;
			}
		}

		return true;
	}

	public void Destroy () {
		ScrHex scrHex = objHex.GetComponent <ScrHex> ();
		if (isWorked) {
			scrPlayer.populationIdle += 1;
			isWorked = false;
		}

		if (buildTag == "cloud" || buildTag == "cloudclear") {
			scrHex.isReveal = true;
			scrHex.meshRenderer.enabled = true;

			if (scrHex.unified.Length > 0) {
				scrHex.CloudReveal (scrHex.unified);
			}
		}

		if (isBuilt) {
			scrHex.objBuild = null;
			if (builtObject) {
				if (buildTag == "cloud" || buildTag == "cloudclear") {
					builtObject.SetActive (true);
					scrHex.objBuild = builtObject;
				} else {
					scrHex.Build (builtObject);
				}
			}
		}
		Destroy (gameObject);
	}

	public string DescReplace (string str) {
		scrController = GameObject.Find ("Controller").GetComponent <ScrController> ();
		scrPlayer = GameObject.Find ("Player").GetComponent <ScrPlayer> ();

		if (!builtInfo) {
			foreach (GameObject objBuild in scrController.listBuild) {
				if (objBuild.GetComponent <ScrBuild> ().prefab == prefab) {
					builtInfo = objBuild;
				}
			}
		}
		ScrBuild info = builtInfo.GetComponent <ScrBuild> ();

		str = str.Replace ("[perbuilding_range]", info.perBuildingRange.ToString ("F0"));

		str = str.Replace ("[built_time]", (info.builtTime * scrPlayer.modBuiltTime).ToString ("F0"));
		str = str.Replace ("[clear_time]", ((info.builtTime * scrPlayer.modBuiltTime) / 2).ToString ("F0"));

		//for desc need effect

		if (str.Contains ("[desc]")) {
			str = str.Replace ("[desc]", ScrLanguage.Translate (buildDescKey));
		}
		if (str.Contains ("[need]")) {
			str = str.Replace ("[need]", ScrLanguage.Translate (buildNeedKey));
		}
		if (str.Contains ("[effect]")) {
			str = str.Replace ("[effect]", ScrLanguage.Translate (buildEffectKey));
		}

		if (str.Contains ("[built_cost]")) {
			if (info.builtCost > 0) {
				str = str.Replace ("[built_cost]", ScrLanguage.Translate ("build_effect_cost"));
				str = str.Replace ("[val]", (info.builtCost * scrPlayer.modBuiltCost).ToString ("F1"));
			} else {
				str = str.Replace ("[built_cost]", "");
			}
		}
		if (str.Contains ("[build_upkeep]")) {
			if (info.builtCost > 0) {
				str = str.Replace ("[build_upkeep]", ScrLanguage.Translate ("build_effect_upkeep"));
				str = str.Replace ("[val]", (info.buildUpkeep * scrPlayer.modBuildUpkeep).ToString ("F1"));
			} else {
				str = str.Replace ("[build_upkeep]", "");
			}
		}

		//pop max
		if (str.Contains ("[max_population]")) {
			if (info.populationMax > 0) {
				str = str.Replace ("[max_population]", ScrLanguage.Translate ("build_effect_max_population"));
				str = str.Replace ("[val]", info.populationMax.ToString ("F0"));
			}
			else {
				str = str.Replace ("[max_population]", "");					
			}
		}
		if (str.Contains ("[max_fleet]")) {
			if (info.fleetMax > 0) {
				str = str.Replace ("[max_fleet]", ScrLanguage.Translate ("build_effect_max_Fleet"));
				str = str.Replace ("[val]", info.fleetMax.ToString ("F0"));
			} else {
				str = str.Replace ("[max_fleet]", "");
			}
		}

		//generate
		if (str.Contains ("[generate_food]")) {
			if (info.generateFood > 0f) {
				str = str.Replace ("[generate_food]", ScrLanguage.Translate ("build_effect_generate_food"));
				str = str.Replace ("[val]", (info.generateFood * scrPlayer.modFoodSurplus).ToString ("F1"));
			} else {
				str = str.Replace ("[generate_food]", "");
			}
		}
		if (str.Contains ("[generate_production]")) {
			if (info.generateProduction > 0f) {
				str = str.Replace ("[generate_production]", ScrLanguage.Translate ("build_effect_generate_production"));
				str = str.Replace ("[val]", (info.generateProduction * scrPlayer.modProductionSurplus).ToString ("F1"));
			} else {
				str = str.Replace ("[generate_production]", "");
			}
		}
		if (str.Contains ("[generate_wealth]")) {
			if (info.generateWealth > 0f) {
				str = str.Replace ("[generate_wealth]", ScrLanguage.Translate ("build_effect_generate_wealth"));
				str = str.Replace ("[val]", (info.generateWealth * scrPlayer.modWealthSurplus).ToString ("F1"));
			} else {
				str = str.Replace ("[generate_wealth]", "");
			}
		}
		if (str.Contains ("[generate_culture]")) {
			if (info.generateCulture > 0f) {
				str = str.Replace ("[generate_culture]", ScrLanguage.Translate ("build_effect_generate_culture"));
				str = str.Replace ("[val]", (info.generateCulture * scrPlayer.modCultureSurplus).ToString ("F1"));
			} else {
				str = str.Replace ("[generate_culture]", "");
			}
		}
		if (str.Contains ("[generate_military]")) {
			if (info.generateMilitary > 0f) {
				str = str.Replace ("[generate_military]", ScrLanguage.Translate ("build_effect_generate_military"));
				str = str.Replace ("[val]", (info.generateMilitary * scrPlayer.modMilitarySurplus).ToString ("F1"));
			} else {
				str = str.Replace ("[generate_military]", "");
			}
		}

		//clear
		if (info.clearObject) {
			ScrBuild clearInfo = info.clearObject.GetComponent <ScrBuild> ();

			if (str.Contains ("[clear_food]")) {
				if (clearInfo.generateFood > 0f) {
					str = str.Replace ("[clear_food]", ScrLanguage.Translate ("build_effect_clear_food"));
					str = str.Replace ("[val]", (clearInfo.generateFood * scrPlayer.modFoodSurplus).ToString ("F1"));
				} else {
					str = str.Replace ("[clear_food]", "");
				}
			}
			if (str.Contains ("[clear_production]")) {
				if (clearInfo.generateProduction > 0f) {
					str = str.Replace ("[clear_production]", ScrLanguage.Translate ("build_effect_clear_production"));
					str = str.Replace ("[val]", (clearInfo.generateProduction * scrPlayer.modProductionSurplus).ToString ("F1"));
				} else {
					str = str.Replace ("[clear_production]", "");
				}
			}
			if (str.Contains ("[clear_wealth]")) {
				if (clearInfo.generateWealth > 0f) {
					str = str.Replace ("[clear_wealth]", ScrLanguage.Translate ("build_effect_clear_wealth"));
					str = str.Replace ("[val]", (clearInfo.generateWealth * scrPlayer.modWealthSurplus).ToString ("F1"));
				} else {
					str = str.Replace ("[clear_wealth]", "");
				}
			}
			if (str.Contains ("[clear_culture]")) {
				if (clearInfo.generateCulture > 0f) {
					str = str.Replace ("[clear_culture]", ScrLanguage.Translate ("build_effect_clear_culture"));
					str = str.Replace ("[val]", (clearInfo.generateCulture * scrPlayer.modCultureSurplus).ToString ("F1"));
				} else {
					str = str.Replace ("[clear_culture]", "");
				}
			}
			if (str.Contains ("[clear_military]")) {
				if (clearInfo.generateMilitary > 0f) {
					str = str.Replace ("[clear_military]", ScrLanguage.Translate ("build_effect_clear_military"));
					str = str.Replace ("[val]", (clearInfo.generateMilitary * scrPlayer.modMilitarySurplus).ToString ("F1"));
				} else {
					str = str.Replace ("[clear_military]", "");
				}
			}
		}

		//perbuilding
		string tempName = "";
		string tempRange = info.perBuildingRange.ToString ("F0");
		if (perBuildingTag == "sea") {
			tempName = ScrLanguage.Translate ("build_name_sea");
		} else if (perBuildingTag == "farm") {
			tempName = ScrLanguage.Translate ("build_name_farm");
		} else if (perBuildingTag == "forest") {
			tempName = ScrLanguage.Translate ("build_name_forest");
		} else if (perBuildingTag == "hill") {
			tempName = ScrLanguage.Translate ("build_name_hill");
		} else if (perBuildingTag == "granary") {
			tempName = ScrLanguage.Translate ("build_name_granary");
		} else if (perBuildingTag == "house") {
			tempName = ScrLanguage.Translate ("build_name_house");
		} else if (perBuildingTag == "pasture") {
			tempName = ScrLanguage.Translate ("build_name_pasture");
		} else if (perBuildingTag == "market") {
			tempName = ScrLanguage.Translate ("build_name_market");
		}

		if (str.Contains ("[perbuilding_food]")) {
			if (info.perBuildingFood > 0f) {
				str = str.Replace ("[perbuilding_food]", ScrLanguage.Translate ("build_effect_perbuilding_food"));
				str = str.Replace ("[val]", (info.perBuildingFood * scrPlayer.modFoodSurplus).ToString ("F1"));
				str = str.Replace ("[name]", tempName);
				str = str.Replace ("[range]", tempRange);
			} else {
				str = str.Replace ("[perbuilding_food]", "");
			}
		}
		if (str.Contains ("[perbuilding_production]")) {
			if (info.perBuildingProduction > 0f) {
				str = str.Replace ("[perbuilding_production]", ScrLanguage.Translate ("build_effect_perbuilding_production"));
				str = str.Replace ("[val]", (info.perBuildingProduction * scrPlayer.modProductionSurplus).ToString ("F1"));
				str = str.Replace ("[name]", tempName);
				str = str.Replace ("[range]", tempRange);
			} else {
				str = str.Replace ("[perbuilding_production]", "");
			}
		}
		if (str.Contains ("[perbuilding_wealth]")) {
			if (info.perBuildingWealth > 0f) {
				str = str.Replace ("[perbuilding_wealth]", ScrLanguage.Translate ("build_effect_perbuilding_wealth"));
				str = str.Replace ("[val]", (info.perBuildingWealth * scrPlayer.modWealthSurplus).ToString ("F1"));
				str = str.Replace ("[name]", tempName);
				str = str.Replace ("[range]", tempRange);
			} else {
				str = str.Replace ("[perbuilding_wealth]", "");
			}
		}
		if (str.Contains ("[perbuilding_culture]")) {
			if (info.perBuildingCulture > 0f) {
				str = str.Replace ("[perbuilding_culture]", ScrLanguage.Translate ("build_effect_perbuilding_culture"));
				str = str.Replace ("[val]", (info.perBuildingCulture * scrPlayer.modCultureSurplus).ToString ("F1"));
				str = str.Replace ("[name]", tempName);
				str = str.Replace ("[range]", tempRange);
			} else {
				str = str.Replace ("[perbuilding_culture]", "");
			}
		}
		if (str.Contains ("[perbuilding_military]")) {
			if (info.perBuildingMilitary > 0f) {
				str = str.Replace ("[perbuilding_military]", ScrLanguage.Translate ("build_effect_perbuilding_military"));
				str = str.Replace ("[val]", (info.perBuildingMilitary * scrPlayer.modMilitarySurplus).ToString ("F1"));
				str = str.Replace ("[name]", tempName);
				str = str.Replace ("[range]", tempRange);
			} else {
				str = str.Replace ("[perbuilding_military]", "");
			}
		}

		return str;
	}
}
