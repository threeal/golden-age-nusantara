using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrCanvasResource : MonoBehaviour {

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	public string strColNormal;
	public string strColGreen;
	public string strColRed;
	public string strColDebug;

	public GameObject uiPopulationInfo;
	Text textPopulationInfo;
	public GameObject uiPopulationCount;
	Text textPopulationCount;
	public GameObject uiFleetInfo;
	Text textFleetInfo;
	public GameObject uiFleetCount;
	Text textFleetCount;
	public GameObject uiFoodInfo;
	Text textFoodInfo;
	public GameObject uiProductionInfo;
	Text textProductionInfo;
	public GameObject uiWealthInfo;
	Text textWealthInfo;
	public GameObject uiCultureInfo;
	Text textCultureInfo;
	public GameObject uiMilitaryInfo;
	Text textMilitaryInfo;

	public GameObject uiDebugInfo;
	Text textDebugInfo;

	float time;
	int counter;
	int counterr;

	// Use this for initialization
	void Start () {
		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		textPopulationInfo = uiPopulationInfo.GetComponentInChildren <Text> ();
		textPopulationCount = uiPopulationCount.GetComponentInChildren <Text> ();
		textFleetInfo = uiFleetInfo.GetComponentInChildren <Text> ();
		textFleetCount = uiFleetCount.GetComponentInChildren <Text> ();
		textFoodInfo = uiFoodInfo.GetComponentInChildren <Text> ();
		textProductionInfo = uiProductionInfo.GetComponentInChildren <Text> ();
		textWealthInfo = uiWealthInfo.GetComponentInChildren <Text> ();
		textCultureInfo = uiCultureInfo.GetComponentInChildren <Text> ();
		textMilitaryInfo = uiMilitaryInfo.GetComponentInChildren <Text> ();
		textDebugInfo = uiDebugInfo.GetComponent <Text> ();

		time = 0;
		counter = 0;
		counterr = 0;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float val;

		textPopulationInfo.text = ((scrPlayer.populationVal <= scrPlayer.populationMax) ? strColNormal : strColRed);
		textPopulationInfo.text += scrPlayer.populationVal + "/" + scrPlayer.populationMax + "\n</color>";

		if (scrPlayer.foodSurplus > 0 && scrPlayer.populationVal < scrPlayer.populationMax
			&& (scrPlayer.populationCost *scrPlayer.modPopulationCost) >= scrPlayer.foodVal) {
			val = ((scrPlayer.populationCost *scrPlayer.modPopulationCost)- scrPlayer.foodVal) / (scrPlayer.foodSurplus * scrPlayer.modFoodSurplus);
			textPopulationInfo.text += "(" + Mathf.Ceil (val).ToString ("F0") + ")";
		} else {
			textPopulationInfo.text += "(-)";
		}

		scrCanvas.Activate (uiPopulationCount, scrPlayer.populationIdle > 0);
		if (scrPlayer.populationIdle > 0) {
			textPopulationCount.text = scrPlayer.populationIdle.ToString ("F0");
		}

		scrCanvas.Activate (uiFleetInfo, scrPlayer.enableWorld);
		if (uiFleetInfo.activeInHierarchy) {
			textFleetInfo.text = ((scrPlayer.fleetIdle <= scrPlayer.fleetMax) ? strColNormal : strColRed);
			textFleetInfo.text += scrPlayer.fleetIdle + "/" + scrPlayer.fleetMax + "\n</color>";

			if (scrPlayer.fleetProductionCost > 0 && scrPlayer.fleetVal < scrPlayer.fleetMax
				&& scrPlayer.fleetCost >= scrPlayer.fleetProduction) {
				val = (scrPlayer.fleetCost - scrPlayer.fleetProduction) / (scrPlayer.fleetProductionCost);
				textFleetInfo.text += "(" + Mathf.Ceil (val).ToString ("F0") + ")";
			} else {
				textFleetInfo.text += "(-)";
			}
				
			scrCanvas.Activate (uiFleetCount, scrPlayer.fleetIdle > 0);
			if (scrPlayer.fleetIdle > 0) {
				textFleetCount.text = scrPlayer.fleetIdle.ToString ("F0");
			}
		}

		ResourcePrint (textFoodInfo, scrPlayer.foodVal, scrPlayer.foodSurplus * scrPlayer.modFoodSurplus);
		val =  (scrPlayer.productionSurplus * scrPlayer.modProductionSurplus) - (scrPlayer.builtCost * scrPlayer.modBuiltCost) - (scrPlayer.fleetProductionCost);
		ResourcePrint (textProductionInfo, scrPlayer.productionVal, val);
		val =  (scrPlayer.wealthSurplus * scrPlayer.modWealthSurplus) - (scrPlayer.buildUpkeep* scrPlayer.modBuildUpkeep);
		ResourcePrint (textWealthInfo, scrPlayer.wealthVal, val);
		ResourcePrint (textCultureInfo, scrPlayer.cultureVal, scrPlayer.cultureSurplus * scrPlayer.modCultureSurplus);
		ResourcePrint (textMilitaryInfo, scrPlayer.militaryVal, scrPlayer.militarySurplus * scrPlayer.modMilitarySurplus);

		//debug
		bool isTrue = true;
		if (isTrue) {
			textDebugInfo.text = strColDebug;
			textDebugInfo.text += "fps =" + counterr;
			textDebugInfo.text += "\n" + Screen.width + " x " + Screen.height;
			textDebugInfo.text += "\nshader model " + SystemInfo.graphicsShaderLevel;
			textDebugInfo.text += "\nsupport rbgahalf " + ((SystemInfo.SupportsTextureFormat (TextureFormat.RGBAHalf)) ? "yes" : "no");
			textDebugInfo.text += "\nQuality " + QualitySettings.names [QualitySettings.GetQualityLevel ()];
			textDebugInfo.text += "\n" + SystemInfo.graphicsDeviceName;
			textDebugInfo.text += "\n" + SystemInfo.graphicsDeviceType;
			textDebugInfo.text += "\n" + SystemInfo.processorType;
			textDebugInfo.text += "\n" + SystemInfo.operatingSystem;
			textDebugInfo.text += "\n" + SystemInfo.deviceModel;
			textDebugInfo.text += "</color>";
		}

		time += Time.deltaTime;
		counter++;
		if (time >= 1) {
			time -= 1;
			counterr = counter;
			counter = 0;
		}
	}

	void ResourcePrint (Text uiInfo, float value, float surplus) {
		uiInfo.text = ((value + surplus >= 0) ? strColNormal : strColRed) + value.ToString ("F0") + "\n</color>";
		uiInfo.text += ((surplus >= 0) ? (strColNormal + "+") : strColRed) + surplus.ToString ("F1") + "</color>";
	}
}
