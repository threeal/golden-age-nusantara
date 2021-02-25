using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScrController : MonoBehaviour {

	public GameObject objSelected = null;
	public GameObject objSelectedCity = null;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objChallenge;
	ScrChallenge scrChallenge;

	public GameObject objMap;
	public GameObject objWorld;

	public GameObject objCursor;
	public GameObject uiIcons;
	public GameObject uiIcon;
	public GameObject uiIconWorld;

	public GameObject objBuilt;
	public GameObject objCloud;
	public GameObject objWater;

	public GameObject[] listHex;
	public GameObject[] listBuild;
	public GameObject[] listTech;

	public Dictionary<string, int> dictionaryBuild;

	public bool isTouchMoved = false;
	public bool isTouchPressed = true;

	// Use this for initialization
	void Start () {
		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objChallenge = GameObject.Find ("Challenge");
		scrChallenge = objChallenge.GetComponent <ScrChallenge> ();

		listHex = GameObject.FindGameObjectsWithTag ("Hex");
		listTech = GameObject.FindGameObjectsWithTag ("Tech");

		//add icon to all hex
		foreach (GameObject hex in listHex) {
			GameObject obj = (GameObject) Instantiate ((Object)uiIcon);
			obj.transform.SetParent (uiIcons.transform);
			obj.GetComponent <ScrCanvasIcon> ().objHex = hex;
		}

		//add icon to city
		GameObject[] cities = GameObject.FindGameObjectsWithTag ("City");
		foreach (GameObject city in cities) {
			GameObject obj = (GameObject)Instantiate ((Object)uiIconWorld);
			obj.transform.SetParent (uiIcons.transform);
			obj.GetComponent <ScrCanvasIconWorld> ().objCity = city;
		}

		dictionaryBuild = new Dictionary<string, int> ();

		Calculate ();
	}
	
	// Update is called once per frame
	void Update () {
		
		//istouch moved check
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				isTouchMoved = false;
			}
			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				isTouchMoved = true;
			}
		}

		//pressed check
		if (IsPressed ()) {
			isTouchPressed = !IsClickedOverUI ();
		}

		//click hex
		if (IsClicked () && IsOverEmpty() && isTouchPressed) {
			Ray ray;
			RaycastHit hit;

			//in map
			if (objMap.activeInHierarchy) {
				if (!objSelected) {
					ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Hex"))) {
						objSelected = hit.collider.gameObject;

						objCursor.SetActive (true);
						Vector3 pos = objSelected.transform.position;
						pos.y = hit.point.y;
						objCursor.transform.position = pos;
					}
				} else {
					objSelected = null;
					objCursor.SetActive (false);
				}
			}
			//in world
			if (objWorld.activeInHierarchy) {
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("City"))) {
					if (hit.collider.gameObject.GetComponent <ScrCity> ()) {
						hit.collider.gameObject.GetComponent <ScrCity> ().Selected ();
					}
				}
			}
		}
	}

	public bool IsClicked () {
		bool isTrue = false;

		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			isTrue = true;
		}
		#endif
		
		#if UNITY_ANDROID
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Ended && !isTouchMoved) {
				isTrue = true;
			}
		}
		#endif

		return isTrue;
	}
	
	public bool IsPressed () {
		bool isTrue = false;

		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			isTrue = true;
		}
		#endif

		#if UNITY_ANDROID
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began && !isTouchMoved) {
				isTrue = true;
			}
		}
		#endif

		return isTrue;
	}

	public bool IsOverEmpty () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << 5)) {
			if (hit.collider.gameObject) {
				return false;
			}
		}

		return true;
	}

	public bool IsClickedOverUI() {
		PointerEventData eventData= new PointerEventData(EventSystem.current);
		eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, results);

		return (results.Count > 0);
	}

	public void Calculate () {
		if (objPlayer) {
			scrPlayer.populationIdle = scrPlayer.populationVal;
			scrPlayer.populationMax = 0;
			if (scrPlayer.populationVal <= 19) {
				scrPlayer.populationCost = (5 + (5 * scrPlayer.populationVal)) * scrPlayer.modPopulationCost;
			} else {
				scrPlayer.populationCost = (100 + (10 * scrPlayer.populationVal)) * scrPlayer.modPopulationCost;
			}

			scrPlayer.fleetIdle = scrPlayer.fleetMax;
			scrPlayer.fleetMax = 0;
			scrPlayer.fleetCost = (60 + (30 * scrPlayer.fleetVal));
			scrPlayer.fleetProductionCost = 0;

			scrPlayer.buildUpkeep = 0;
			scrPlayer.builtCost = 0;
			scrPlayer.foodSurplus = scrPlayer.perPopFood * scrPlayer.populationVal;
			scrPlayer.productionSurplus = scrPlayer.perPopProduction * scrPlayer.populationVal;
			scrPlayer.wealthSurplus = scrPlayer.perPopWealth * scrPlayer.populationVal;
			scrPlayer.cultureSurplus = scrPlayer.perPopCulture * scrPlayer.populationVal;
			scrPlayer.militarySurplus = scrPlayer.perPopMilitary * scrPlayer.populationVal;

			//for build dictionary
			dictionaryBuild.Clear ();

			foreach (GameObject objHex in listHex) {
				ScrHex scrHex = objHex.GetComponent <ScrHex> ();
				GameObject objBuild = scrHex.objBuild;
				if (objBuild) {
					ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();

					//for build dictionary
					if (scrHex.isReveal) {
						int n;
						if (dictionaryBuild.TryGetValue (scrBuild.buildTag, out n)) {
							dictionaryBuild.Remove (scrBuild.buildTag);
							n += 1;
							dictionaryBuild.Add (scrBuild.buildTag, n);
						} else {
							dictionaryBuild.Add (scrBuild.buildTag, 1);
						}
					}

					if (scrBuild.isWorked) {
						if (scrPlayer.populationIdle > 0) {
							scrPlayer.populationIdle -= 1;
						} else {
							scrBuild.isWorked = false;
						}
					}

					scrPlayer.buildUpkeep += scrBuild.buildUpkeep;
					scrPlayer.populationMax += scrBuild.populationMax;
					scrPlayer.fleetMax += scrBuild.fleetMax;

					if (scrBuild.isWorked || (!scrBuild.isWorkable && scrBuild.isBuilding)) {
						//Building is in Work
						scrPlayer.foodSurplus += scrBuild.generateFood;
						scrPlayer.productionSurplus += scrBuild.generateProduction;
						scrPlayer.wealthSurplus += scrBuild.generateWealth;
						scrPlayer.cultureSurplus += scrBuild.generateCulture;
						scrPlayer.militarySurplus += scrBuild.generateMilitary;

						//foreach bonus per building
						if (scrBuild.perBuildingRange > 0) {
							GameObject[] hexInRange = scrHex.NearbyRange (scrBuild.perBuildingRange);
							foreach (GameObject hex in hexInRange) {
								GameObject build = hex.GetComponent <ScrHex> ().objBuild;
								bool isTrue = false;
								if (hex != objHex) {
									if (scrBuild.perBuildingTag == "water") {
										ScrHex scr = hex.GetComponent <ScrHex> ();
										if (scr.isWater && !scr.objBuild) {
											isTrue = true;
										}
									} else if (build) {
										ScrBuild scr = build.GetComponent <ScrBuild> ();
										if (scr.buildTag == scrBuild.perBuildingTag) {
											if (scr.isWorked || !scr.isWorkable) {
												isTrue = true;
											}
										}
									}
								}

								if (isTrue) {
									scrPlayer.foodSurplus += scrBuild.perBuildingFood;
									scrPlayer.productionSurplus += scrBuild.perBuildingProduction;
									scrPlayer.wealthSurplus += scrBuild.perBuildingWealth;
									scrPlayer.cultureSurplus += scrBuild.perBuildingCulture;
									scrPlayer.militarySurplus += scrBuild.perBuildingMilitary;

									// harbor produce fleet
									if (scrBuild.buildTag == "harbor") {
										scrPlayer.fleetProductionCost += 15f;
									}
								}
							}
						}

						if (scrBuild.isBuilt) {
							scrPlayer.builtCost += scrBuild.builtCost;
						}

					}
				}
			}
				
			//for cities if on command
			if (scrPlayer.enableWorld) {
				ScrWorld scrWorld = objWorld.GetComponent <ScrWorld> ();
				foreach (GameObject objCity in objWorld.GetComponent <ScrWorld> ().objCities) {
					ScrCity scrCity = objCity.GetComponent <ScrCity> ();

					if (scrCity.commandTag != "") {
						scrPlayer.fleetIdle -= 1;
					}

					//if vassal
					if (scrCity.isVassal) {
						if (scrCity.tradeFood > 0f) {
							scrPlayer.foodSurplus += scrCity.tradeFood * scrWorld.modVassalTribute;
						} else if (scrCity.tradeProduction > 0f) {
							scrPlayer.productionSurplus += scrCity.tradeProduction * scrWorld.modVassalTribute;
						} else if (scrCity.tradeWealth > 0f) {
							scrPlayer.wealthSurplus += scrCity.tradeWealth * scrWorld.modVassalTribute;
						} else if (scrCity.tradeCulture > 0f) {
							scrPlayer.cultureSurplus += scrCity.tradeCulture * scrWorld.modVassalTribute;
						} else if (scrCity.tradeMilitary > 0f) {
							scrPlayer.militarySurplus += scrCity.tradeMilitary * scrWorld.modVassalTribute;
						}
					}
				}
				if (scrPlayer.fleetIdle < 0) {
					scrPlayer.fleetIdle = 0;
				}
			}
		}
	}

	public void EndTurn () {
		//activate world;
		bool isActiveMap = objMap.activeInHierarchy;
		bool isActiveWorld = objWorld.activeInHierarchy;

		objMap.SetActive (true);
		objWorld.SetActive (true);

		// add turn count
		scrPlayer.turnCount++;

		// add resource value
		scrPlayer.wealthVal -= scrPlayer.buildUpkeep * scrPlayer.modBuildUpkeep;
		scrPlayer.foodVal += scrPlayer.foodSurplus * scrPlayer.modFoodSurplus;
		scrPlayer.wealthVal += scrPlayer.wealthSurplus *scrPlayer.modWealthSurplus;
		scrPlayer.cultureVal += scrPlayer.cultureSurplus;
		scrPlayer.militaryVal += scrPlayer.militarySurplus;
		scrPlayer.productionVal += scrPlayer.productionSurplus;

		// for build, clear, explore, etc.
		foreach (GameObject objHex in listHex) {
			ScrHex scrHex = objHex.GetComponent <ScrHex> ();
			GameObject objBuild = scrHex.objBuild;
			if (objBuild) {
				ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();

				if (scrBuild.isBuilt && scrBuild.isWorked) {
					if (scrPlayer.productionVal >= scrBuild.builtCost * scrPlayer.modBuiltCost) {
						scrPlayer.productionVal -= scrBuild.builtCost * scrPlayer.modBuiltCost;
						scrBuild.builtTime -= 1;
						if (scrBuild.builtTime <= 0) {
							if (scrBuild.buildTag == "cloud" || scrBuild.buildTag == "cloudclear") {
								scrHex.CloudReveal (scrHex.NearbyRange (scrBuild.perBuildingRange));
							}
							scrBuild.Destroy ();
						}
					}
				}
			}
		}

		while (scrPlayer.fleetProductionCost > 0) {
			if (scrPlayer.productionVal >= 15f) {
				scrPlayer.fleetProduction += 15f;
				scrPlayer.fleetProductionCost -= 15f;
				scrPlayer.productionVal -= 15f;
			} else {
				scrPlayer.fleetProductionCost = 0f;
			}
		}

		if (scrPlayer.foodVal >= scrPlayer.populationCost && scrPlayer.populationVal < scrPlayer.populationMax) {
			scrPlayer.foodVal -= scrPlayer.populationCost;
			scrPlayer.populationVal += 1;
			scrPlayer.populationIdle += 1;
		}

		if (scrPlayer.fleetProduction >= scrPlayer.fleetCost && scrPlayer.fleetVal < scrPlayer.fleetMax) {
			scrPlayer.fleetProduction -= scrPlayer.fleetCost;
			scrPlayer.fleetVal += 1;
			scrPlayer.fleetIdle += 1;
		}

		if (scrPlayer.foodVal < 0) {
			scrPlayer.foodVal = 0;
			scrPlayer.populationVal -= 1;
		}

		//for cities if command finished
		foreach (GameObject objCity in objWorld.GetComponent <ScrWorld> ().objCities) {
			ScrCity scrCity = objCity.GetComponent <ScrCity> ();

			if (scrCity.commandTag != "") {
				scrCity.commandTime -= 1;
				if (scrCity.commandTime <= 0) {
					scrChallenge.listCityFinished.Add (objCity);
				}
			}
		}

		Calculate ();

		scrChallenge.RandomEvent ();
		scrChallenge.TimedEvent ();
		scrChallenge.NonTimedEvent ();

		objMap.SetActive (isActiveMap);
		objWorld.SetActive (isActiveWorld);

		Calculate ();
	}
}
