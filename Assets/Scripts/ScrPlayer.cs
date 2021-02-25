using UnityEngine;
using System.Collections;

public class ScrPlayer : MonoBehaviour {

	public string reignNameKey = "ui_reign_settlement";
	public bool enableWorld = false;

	public int turnCount = 0;

	public int populationVal = 1;
	public int populationIdle = 1;
	public int populationMax = 2;
	public float populationCost = 0f;

	public int fleetVal = 0;
	public int fleetIdle = 0;
	public int fleetMax = 0;
	public float fleetCost = 0f;
	public float fleetProduction = 0f;
	public float fleetProductionCost = 0f;

	public float buildUpkeep = 0;
	public float builtCost = 0;

	public float foodVal = 0;
	public float foodSurplus = 0;
	public float productionVal = 0;
	public float productionSurplus = 0;
	public float wealthVal = 0;
	public float wealthSurplus = 0;
	public float cultureVal = 0;
	public float cultureSurplus = 0;
	public float militaryVal = 0;
	public float militarySurplus = 0;

	public float perPopFood = 0;
	public float perPopProduction = 0;
	public float perPopWealth = 0;
	public float perPopCulture = 0;
	public float perPopMilitary = 0;

	public float modBuildUpkeep = 1f;
	public float modBuiltCost = 1f;
	public float modBuiltTime = 1f;
	public float modFoodSurplus = 1f;
	public float modProductionSurplus = 1f;
	public float modWealthSurplus = 1f;
	public float modCultureSurplus = 1f;
	public float modMilitarySurplus = 1f;

	public float modExplorationTime = 1f;
	public float modPopulationCost = 1f;
	public float modTechnologyCost = 1f;
	public float modExpeditionTime = 1f;
	public float modTradeBonus = 1f;
	public float modDiplomacyCost = 1f;
	public float modCombatStrength = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
	}
}
