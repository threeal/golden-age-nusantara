using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataGame")]
public class ScrDataGame {

	public string reignNameKey = "ui_reign_radenwijaya";
	public bool enableWorld;

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

	public float foodVal = 0;
	public float productionVal = 0;
	public float wealthVal = 0;
	public float cultureVal = 0;
	public float militaryVal = 0;

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

	public int challengeId;
	public int challengeCount;
	public bool challengeDisplayed;

	[XmlArray("Hexes")]
	[XmlArrayItem("Hex")]
	public List<ScrDataHex> dataHex;

	[XmlArray("Techs")]
	[XmlArrayItem("Tech")]
	public List<ScrDataTech> dataTech;

	[XmlArray("Events")]
	[XmlArrayItem("Event")]
	public List<ScrDataEvent> dataEvent;

	[XmlArray("Cities")]
	[XmlArrayItem("City")]
	public List<ScrDataCity> dataCity;
}
