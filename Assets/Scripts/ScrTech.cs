using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrTech : MonoBehaviour {

	public string techTag = "";
	public string techNameKey;
	public string techDescKey;
	public string techEffectKey;
	public float techCost = 0;
	public GameObject[] needTech;
	public bool isResearched = false;

	public bool enableWorld = false;

	public float perPopFood = 0;
	public float perPopProduction = 0;
	public float perPopWealth = 0;
	public float perPopCulture = 0;
	public float perPopMilitary = 0;

	public float modBuildUpkeep = 0;
	public float modBuiltCost = 0;
	public float modBuiltTime = 0;
	public float modFoodSurplus = 0;
	public float modProductionSurplus = 0;
	public float modWealthSurplus = 0;
	public float modCultureSurplus = 0;
	public float modMilitarySurplus = 0;

	public float modExplorationTime = 0;
	public float modPopulationCost = 0;
	public float modTechnologyCost = 0;
	public float modExpeditionTime = 0;
	public float modTradeBonus = 0;
	public float modDiplomacyCost = 0;
	public float modCombatStrength = 0;

	public string buildTag = "";

	public bool isFreshWater = false;

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
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool Researchable () {
		if (isResearched) {
			return false;
		}

		int count = needTech.Length;
		foreach (GameObject tech in needTech) {
			if (tech.GetComponent <ScrTech> ().isResearched) {
				count--;
			}
		}

		return (count <= 0);
	}

	public void EffectOnActive () {
		if (isResearched) {

			if (enableWorld) {
				scrPlayer.enableWorld = true;
			}

			scrPlayer.perPopFood += perPopFood;
			scrPlayer.perPopProduction += perPopProduction;
			scrPlayer.perPopWealth += perPopWealth;
			scrPlayer.perPopCulture += perPopCulture;
			scrPlayer.perPopMilitary += perPopMilitary;

			scrPlayer.modBuildUpkeep += modBuildUpkeep;
			scrPlayer.modBuiltCost += modBuiltCost;
			scrPlayer.modBuiltTime += modBuiltTime;
			scrPlayer.modFoodSurplus += modFoodSurplus;
			scrPlayer.modProductionSurplus += modProductionSurplus;
			scrPlayer.modWealthSurplus += modWealthSurplus;
			scrPlayer.modCultureSurplus += modCultureSurplus;
			scrPlayer.modMilitarySurplus += modMilitarySurplus;

			scrPlayer.modExplorationTime += modExplorationTime;
			scrPlayer.modPopulationCost += modPopulationCost;
			scrPlayer.modTechnologyCost += modTechnologyCost;
			scrPlayer.modExpeditionTime += modExpeditionTime;
			scrPlayer.modTradeBonus += modTradeBonus;
			scrPlayer.modDiplomacyCost += modDiplomacyCost;
			scrPlayer.modCombatStrength += modCombatStrength;

			//foreach building
			foreach (GameObject objHex in scrController.listHex) {
				if (objHex) {
					EffectOnBuild (objHex.GetComponent <ScrHex> ().objBuild);
				}
			}

			//fpreach built info
			foreach (GameObject objBuild in scrController.listBuild) {
				EffectOnBuild (objBuild);
			}
		}
	}

	public void EffectOnBuild (GameObject objBuild) {
		if (objBuild && isResearched) {
			ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();

			if (scrBuild.buildTag == buildTag) {
				if (isFreshWater) {
					scrBuild.isFreshWater = true;
				}

				scrBuild.populationMax += populationMax;
				scrBuild.fleetMax += fleetMax;

				scrBuild.generateFood += generateFood;
				scrBuild.generateProduction += generateProduction;
				scrBuild.generateWealth += generateWealth;
				scrBuild.generateCulture += generateCulture;
				scrBuild.generateMilitary += generateMilitary;

				if (perBuildingTag != "") {
					scrBuild.perBuildingTag = perBuildingTag;
				}
				scrBuild.perBuildingRange += perBuildingRange;
				scrBuild.perBuildingFood += perBuildingFood;
				scrBuild.perBuildingProduction += perBuildingProduction;
				scrBuild.perBuildingWealth += perBuildingWealth;
				scrBuild.perBuildingCulture += perBuildingCulture;
				scrBuild.perBuildingMilitary += perBuildingMilitary;
			}
		}
	}
}
