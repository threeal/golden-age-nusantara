using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrChallenge : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	GameObject objWorld;
	ScrWorld scrWorld;

	GameObject uiConfirmation;
	ScrCanvasConfirmation scrConfirmation;

	GameObject uiWar;
	ScrCanvasWar scrWar;

	GameObject uiEndTurn;
	GameObject uiIntro;

	public GameObject objEvent;

	public List<GameObject> listEvent;
	public List<GameObject> listCityFinished;
	public Dictionary<string, GameObject> dictionaryEvent;

	bool eventFirst = false;
	bool eventEmpty = true;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		objWorld = GameObject.Find ("World");
		scrWorld = objWorld.GetComponent <ScrWorld> ();

		uiConfirmation = scrCanvas.uiConfirmation;
		scrConfirmation = uiConfirmation.GetComponent <ScrCanvasConfirmation> ();

		uiWar = scrCanvas.uiWar;
		scrWar = uiWar.GetComponent <ScrCanvasWar> ();

		uiEndTurn = scrCanvas.uiEndTurn;
		uiIntro = scrCanvas.uiIntro;

		listCityFinished = new List<GameObject> ();

		//foreach Event
		dictionaryEvent = new Dictionary<string, GameObject> ();
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Event");
		foreach (GameObject obj in objs) {
			ScrButtonEvent scr = obj.GetComponent <ScrButtonEvent> ();
			obj.SetActive (false);
			dictionaryEvent.Add (scr.eventTag, obj);
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		ScrButtonEvent scr;

		if (!uiIntro.activeInHierarchy) {
			if (!eventFirst) {
				if (scr = EventFind ("rise")) {
					if (!scr.eventFinished) {
						EventEnable ("rise", true);
						EventFinish ("rise", false);
						EventEnable ("radenwijaya", true);
						EventEnable ("newsettlement1", true);
					}
				}
				eventFirst = true;
			}

			if (!uiEndTurn.activeInHierarchy) {
				//foreach already city
				foreach (GameObject objCity in listCityFinished) {
					if (!uiConfirmation.activeInHierarchy && !uiWar.activeInHierarchy && !scrCanvas.uiMenu.activeInHierarchy) {
						eventEmpty = false;
						scrCanvas.uiEmpty.SetActive (true);
						ScrCity scrCity = objCity.GetComponent <ScrCity> ();
						scrCity.Command ();
						scrCity.commandTag = "";
						listCityFinished.Remove (objCity);
						scrController.Calculate ();
						break;
					}
				}

				foreach (GameObject obj in listEvent) {
					if (!uiConfirmation.activeInHierarchy && !uiWar.activeInHierarchy && !scrCanvas.uiMenu.activeInHierarchy) {
						if (obj.activeSelf) {
							scr = obj.GetComponent <ScrButtonEvent> ();

							if (!scr.eventDisplayed) {
								eventEmpty = false;
								if (scr.eventFinished) {
									scr.FinishedView ();
								} else {
									scr.ChallengeView ();
								}
							}
						}
					}
				}

				if (!uiConfirmation.activeInHierarchy && !uiWar.activeInHierarchy && !scrCanvas.uiMenu.activeInHierarchy) {
					if (IsEventFinished ("fall")) {
						Debug.Log ("pisan");
						EndGame ();
						EventFinish ("fall", true);
					}
				}

			}
		}

		if (!uiEndTurn.activeInHierarchy && ! uiIntro.activeInHierarchy) {
			if (!uiConfirmation.activeInHierarchy && !uiWar.activeInHierarchy && !scrCanvas.uiMenu.activeInHierarchy) {
				if (eventEmpty) {
					scrCanvas.uiEmpty.SetActive (false);
				}
				eventEmpty = true;
			}
		}

	}

	public void RandomEvent () {
		foreach (KeyValuePair<string, GameObject> entry in dictionaryEvent) {
			ScrButtonEvent scr = entry.Value.GetComponent <ScrButtonEvent> ();
			if (scr.isRandom) {
				if (!entry.Value.activeInHierarchy) {
					if (Random.value <= scr.chance) {
						scr.eventFinished = false;
						scr.eventDisplayed = false;
						scr.eventTime = (int)Random.Range (3, 6);

						if (entry.Key == "drought") {
							scrPlayer.modFoodSurplus -= 0.5f;
						} else if (entry.Key == "flood") {
							scrPlayer.modProductionSurplus -= 0.5f;
						} else if (entry.Key == "blessed") {
							scrPlayer.modProductionSurplus += 0.5f;
						} else if (entry.Key == "enlightned") {
							scrPlayer.modCultureSurplus += 0.5f;
						} else if (entry.Key == "rain") {
							scrPlayer.modFoodSurplus += 0.5f;
						}

						entry.Value.SetActive (true);
					}
				} else {
					scr.eventTime -= 1;
					if (scr.eventTime <= 0) {
						scr.eventFinished = true;
						scr.eventDisplayed = false;

						if (entry.Key == "drought") {
							scrPlayer.modFoodSurplus += 0.5f;
						} else if (entry.Key == "flood") {
							scrPlayer.modProductionSurplus += 0.5f;
						} else if (entry.Key == "blessed") {
							scrPlayer.modProductionSurplus -= 0.5f;
						} else if (entry.Key == "enlightned") {
							scrPlayer.modCultureSurplus -= 0.5f;
						} else if (entry.Key == "rain") {
							scrPlayer.modFoodSurplus -= 0.5f;
						}
					}
				}
			}
		}
	}

	public void TimedEvent () {
		ScrButtonEvent scr, tempScr;
		int n;
		bool isTrue;

		foreach (KeyValuePair<string, GameObject> entry in dictionaryEvent) {
			scr = entry.Value.GetComponent <ScrButtonEvent> ();
			if (scr.isTimed) {
				if (!entry.Value.activeInHierarchy) {
					if (scrPlayer.turnCount == scr.eventTurn) {

						if (entry.Key == "radenwijaya") {
							scrPlayer.modPopulationCost -= 0.3333f;
						} else if (entry.Key == "jayanegara") {
							scrPlayer.modCombatStrength += 0.25f;
							scrPlayer.reignNameKey = "ui_reign_jayanegara";
						} else if (entry.Key == "dyahgitarja") {
							scrPlayer.modTechnologyCost -= 0.25f;
							scrPlayer.reignNameKey = "ui_reign_dyahgitarja";
						} else if (entry.Key == "hayamwuruk") {
							scrPlayer.modExpeditionTime -= 0.3333f;
							scrPlayer.reignNameKey = "ui_reign_hayamwuruk";
						} else if (entry.Key == "wikramawardhana") {
							scrPlayer.modFoodSurplus -= 0.25f;
							scrPlayer.modProductionSurplus -= 0.25f;
							scrPlayer.modWealthSurplus -= 0.25f;
							scrPlayer.modCultureSurplus -= 0.25f;
							scrPlayer.modMilitarySurplus -= 0.25f;
							scrPlayer.reignNameKey = "ui_reign_wikramawardhana";
						}

						if (entry.Key == "attackkediri") {
						} else if (entry.Key == "attackmongol") {
							scrPlayer.modFoodSurplus -= 0.25f;
						} else if (entry.Key == "buildharbor") {
						} else if (entry.Key == "attackkuti") {
							scrPlayer.modBuildUpkeep += 1f;
						} else if (entry.Key == "buildtemple") {
						} else if (entry.Key == "palapaoath") {
						} else if (entry.Key == "sundamarriage") {
						} else if (entry.Key == "bubattragedy") {
							EventFinish ("sundamarriage", true);
						} else if (entry.Key == "buildwonders") {
						} else if (entry.Key == "paregregwar") {
						}

						isTrue = false;
						if (entry.Key == "mongolhelp") {
							if (tempScr = EventFind ("attackkediri")) {
								if (tempScr.eventFinished) {
									isTrue = true;
								}
							}
						}

						if (!isTrue) {
							scr.eventFinished = false;
							scr.eventDisplayed = false;
							entry.Value.SetActive (true);
						}
					}
				} else {
					scr.eventTime -= 1;
					if (scr.eventTime > 0) {
						if (entry.Key == "mongolhelp") {
							if (scrPlayer.cultureVal > 50f) {
								scrPlayer.cultureVal -= 50f;
								scrPlayer.militaryVal += 100f;

								scr.eventFinished = true;
								scr.eventFailed = false;
								scr.eventDisplayed = false;
							}
						} else if (entry.Key == "buildharbor") {
							if (scrController.dictionaryBuild.TryGetValue ("harbor", out n)) {
								if (n >= 1) {
									scrPlayer.fleetVal += 1;
									if (scrPlayer.fleetVal > scrPlayer.fleetMax) {
										scrPlayer.fleetVal = scrPlayer.fleetMax;
									}

									scr.eventFinished = true;
									scr.eventFailed = false;
									scr.eventDisplayed = false;
								}
							}
						} else if (entry.Key == "buildtemple") {
							if (scrController.dictionaryBuild.TryGetValue ("temple", out n)) {
								if (n >= 3) {
									scrPlayer.modWealthSurplus += 0.25f;

									scr.eventFinished = true;
									scr.eventFailed = false;
									scr.eventDisplayed = false;
								}
							}
						} else if (entry.Key == "palapaoath") {
							isTrue = true;
							foreach (GameObject objCity in scrWorld.objCities) {
								ScrCity scrCity = objCity.GetComponent <ScrCity> ();
								if (!scrCity.isAlly && !scrCity.isPlayer) {
									isTrue = false;
									break;
								}
							}

							if (isTrue) {
								scrPlayer.modTradeBonus += 0.25f;

								scr.eventFinished = true;
								scr.eventFailed = false;
								scr.eventDisplayed = false;
							}

						} else if (entry.Key == "buildwonders") {
							if (scrController.dictionaryBuild.TryGetValue ("wonders", out n)) {
								if (n >= 1) {
									scr.eventFinished = true;
									scr.eventFailed = false;
									scr.eventDisplayed = false;
								}
							}
						}
					}
					else {
						scr.eventFinished = true;
						scr.eventDisplayed = false;

						if (entry.Key == "radenwijaya") {
							scrPlayer.modPopulationCost += 0.3333f;
							EventEnable ("jayanegara", true);
						} else if (entry.Key == "jayanegara") {
							scrPlayer.modCombatStrength -= 0.25f;
							EventEnable ("dyahgitarja", true);
						} else if (entry.Key == "dyahgitarja") {
							scrPlayer.modTechnologyCost += 0.25f;
							EventEnable ("hayamwuruk", true);
						} else if (entry.Key == "hayamwuruk") {
							scrPlayer.modExpeditionTime += 0.3333f;
							EventEnable ("wikramawardhana", true);
						} else if (entry.Key == "wikramawardhana") {
							scrPlayer.modFoodSurplus += 0.25f;
							scrPlayer.modProductionSurplus += 0.25f;
							scrPlayer.modWealthSurplus += 0.25f;
							scrPlayer.modCultureSurplus += 0.25f;
							scrPlayer.modMilitarySurplus += 0.25f;
							EventEnable ("fall", true);
							EventFinish ("fall", false);
						}

						if (entry.Key == "attackkediri") {
							scr.WarStart ();
						} else if (entry.Key == "mongolhelp") {
							scrPlayer.modFoodSurplus += 0.25f;
							scr.eventFailed = true;
						} else if (entry.Key == "attackmongol") {
							scr.WarStart ();
						} else if (entry.Key == "buildharbor") {
							scr.eventFailed = true;
						} else if (entry.Key == "attackkuti") {
							scr.WarStart ();
						} else if (entry.Key == "buildtemple") {
							scr.eventFailed = true;
						} else if (entry.Key == "palapaoath") {
							scr.eventFailed = true;
						} else if (entry.Key == "sundamarriage") {
							scr.eventFailed = true;
						} else if (entry.Key == "bubattragedy") {
							scr.WarStart ();
						} else if (entry.Key == "buildwonders") {
							scr.eventFailed = true;
						} else if (entry.Key == "paregregwar") {
							scr.WarStart ();
						}
					}
				}
			}
		}
	}

	public void NonTimedEvent () {
		int n;
		float f;

		foreach (KeyValuePair<string, GameObject> entry in dictionaryEvent) {
			ScrButtonEvent scr = entry.Value.GetComponent <ScrButtonEvent> ();
			if (!scr.isTimed && !scr.isRandom) {
				if (entry.Value.activeInHierarchy) {


					if (entry.Key == "newsettlement1") {
						if (scrController.dictionaryBuild.TryGetValue ("farm", out n)) {
							if (n >= 1) {
								scrPlayer.populationVal += 1;
								scr.eventFinished = true;
								scr.eventDisplayed = false;
								EventEnable ("newsettlement2", true);
							}
						}
					} else if (entry.Key == "newsettlement2") {
						if (scrController.dictionaryBuild.TryGetValue ("woodcut", out n)) {
							if (n >= 1) {
								scrPlayer.wealthVal += 10f;
								scr.eventFinished = true;
								scr.eventDisplayed = false;
								EventEnable ("newsettlement3", true);
							}
						}
					} else if (entry.Key == "newsettlement3") {
						if (scrController.dictionaryBuild.TryGetValue ("ruin", out n)) {
							if (n >= 1) {
								scrPlayer.productionVal += 20f;
								scr.eventFinished = true;
								scr.eventDisplayed = false;
								EventEnable ("newsettlement4", true);
							}
						}
					} else if (entry.Key == "newsettlement4") {
						if (scrPlayer.populationVal >= 5) {
							scrPlayer.cultureVal += 20f;
							scr.eventFinished = true;
							scr.eventDisplayed = false;
							EventEnable ("newsettlement5", true);
						}
					} else if (entry.Key == "newsettlement5") {
						if (scrController.dictionaryBuild.TryGetValue ("market", out n)) {
							if (n >= 1) {
								scrPlayer.militaryVal += 30f;
								scr.eventFinished = true;
								scr.eventDisplayed = false;
							}
						}
					} 

				}

				if (entry.Key == "noproduction") {
					
					if (scrPlayer.builtCost * scrPlayer.modBuiltCost > 0) {
						f = scrPlayer.productionSurplus * scrPlayer.modProductionSurplus;
						f -= scrPlayer.builtCost * scrPlayer.modBuiltCost;
						f -= scrPlayer.fleetProductionCost;

						if (scrPlayer.productionVal + f < 0f) {
							if (!entry.Value.activeInHierarchy) {
								scr.eventDisplayed = false;
								scr.eventFinished = false;
								entry.Value.SetActive (true);
							}
						}
						else {
							if (entry.Value.activeInHierarchy) {
								entry.Value.SetActive (false);
							}							
						}

					}
				}
			}
		}
	}

	public void AfterWar (string key, bool eventFailed) {
		if (key == "attackkediri") {
			scrPlayer.reignNameKey = "ui_reign_radenwijaya";
		} else if (key == "attackmongol") {
			scrPlayer.modFoodSurplus += 0.25f;
		} else if (key == "attackkuti") {
			scrPlayer.modBuildUpkeep -= 1f;
		}

		if (!eventFailed) {
			if (key == "attackkediri") {
				scrWorld.CityRelation ("java2", 2, true, true);
				scrWorld.CityRelation ("java3", 2, true, false);
			} else if (key == "attackmongol") {
				scrPlayer.wealthVal += 100f;
			} else if (key == "attackkuti") {
				scrPlayer.modCombatStrength += 0.25f;
			} else if (key == "paregregwar") {
				scrPlayer.modFoodSurplus += 0.25f;
				scrPlayer.modProductionSurplus += 0.25f;
				scrPlayer.modWealthSurplus += 0.25f;
				scrPlayer.modCultureSurplus += 0.25f;
				scrPlayer.modMilitarySurplus += 0.25f;
			}
		}
	}

	public void EventEnable (string key, bool enable) {
		GameObject obj;
		ScrButtonEvent scr;
		if (dictionaryEvent.TryGetValue (key, out obj)) {
			scr = obj.GetComponent <ScrButtonEvent> ();

			if (enable) {
				scr.eventFailed = false;
				scr.eventFinished = false;
				scr.eventDisplayed = false;
			} else {
				scr.eventFinished = true;
			}

			obj.SetActive (enable);
		}
	}

	public void EventFinish (string key, bool failed) {
		GameObject obj;
		ScrButtonEvent scr;
		if (dictionaryEvent.TryGetValue (key, out obj)) {
			scr = obj.GetComponent <ScrButtonEvent> ();

			scr.eventFinished = true;
			scr.eventDisplayed = false;
			scr.eventFailed = failed;
		}
	}

	public ScrButtonEvent EventFind (string key) {
		GameObject obj = null;
		ScrButtonEvent scr = null;
		if (dictionaryEvent.TryGetValue (key, out obj)) {
			if (obj) {
				scr = obj.GetComponent <ScrButtonEvent> ();
			}
		}
		return scr;		
	}

	public bool IsEventFinished (string key) {
		GameObject obj = null;
		if (dictionaryEvent.TryGetValue (key, out obj)) {
			if (obj) {
				ScrButtonEvent scr = obj.GetComponent <ScrButtonEvent> ();
				if (scr) {
					if (scr.eventFinished && !scr.eventFailed) {
						return true;
					}
				}
			}
		}

		return false;
	}

	void ChallengeDefault (GameObject objEvent) {
		ScrButtonEvent scrEvent = objEvent.GetComponent <ScrButtonEvent> ();
		if (!scrEvent.eventDisplayed) {
			scrEvent.ChallengeView ();
		}		
	}

	//End Game
	public void EndGame () {
		string str;
		bool isTrue;
		int score = 0;

		scrConfirmation.textTitle.text = ScrLanguage.Translate ("ui_gameover");


		str = ScrLanguage.Translate ("ui_gameover_desc");

		isTrue = IsEventFinished ("attackkediri");
		str = str.Replace ("[attackkediri]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 200 : 50);

		isTrue = IsEventFinished ("attackmongol");
		str = str.Replace ("[attackmongol]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 200 : 50);

		isTrue = IsEventFinished ("buildharbor");
		str = str.Replace ("[buildharbor]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 200 : 50);

		isTrue = IsEventFinished ("attackkuti");
		str = str.Replace ("[attackkuti]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 300 : 50);

		isTrue = IsEventFinished ("buildtemple");
		str = str.Replace ("[buildtemple]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 300 : 50);

		isTrue = IsEventFinished ("palapaoath");
		str = str.Replace ("[palapaoath]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 600 : 50);

		isTrue = IsEventFinished ("bubattragedy");
		str = str.Replace ("[bubattragedy]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 300 : 50);

		isTrue = IsEventFinished ("buildwonders");
		str = str.Replace ("[buildwonders]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 600 : 50);

		isTrue = IsEventFinished ("paregregwar");
		str = str.Replace ("[paregregwar]", (isTrue ? ScrLanguage.Translate ("misc_finished") : ScrLanguage.Translate ("misc_failed")));
		score += (isTrue ? 300 : 50);

		str = str.Replace ("[val]", score.ToString ("F0"));

		scrConfirmation.textDescription.text = str;

		scrConfirmation.isChoice = true;

		scrConfirmation.cancelMethod = Ok;

		scrConfirmation.textChoiceAButton.text = ScrLanguage.Translate ("ui_gameover_continue");
		scrConfirmation.buttonChoiceAButton.interactable = true;
		scrConfirmation.choiceAMethod = Ok;

		scrConfirmation.textChoiceBButton.text = ScrLanguage.Translate ("ui_gameover_endgame");
		scrConfirmation.buttonChoiceBButton.interactable = true;
		scrConfirmation.choiceBMethod = EndGameClick;

		scrCanvas.uiConfirmation.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	void EndGameClick () {
		ScrIntro scrIntro = scrCanvas.uiIntro.GetComponent <ScrIntro> ();

		scrIntro.nextScene = "Menu";
		scrCanvas.uiIntro.SetActive (true);		
	}

	//CITY EXPEDITION

	public void AllianceEvent (GameObject objCity) {
		ScrCity scrCity = objCity.GetComponent <ScrCity> ();
		string str;

		scrConfirmation.textTitle.text = ScrLanguage.Translate ("expedition_name_alliance_finish");
		if (Random.value < 0.4f * scrCity.cityRelation) {
			str = ScrLanguage.Translate ("expedition_desc_alliance_finish_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			scrConfirmation.textDescription.text = str;

			scrCity.isAlly = true;
			scrCity.isVassal = false;

		} else {
			str = ScrLanguage.Translate ("expedition_desc_alliance_finish_failed");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			scrConfirmation.textDescription.text = str;

			scrCity.cityRelation -= 1;
			if (scrCity.cityRelation < 0) {
				scrCity.cityRelation = 0;
			}
		}

		DefaultEvent ();
	}

	public void InvadeEvent (GameObject objCity) {
		ScrCity scrCity = objCity.GetComponent <ScrCity> ();
		string str;

		scrCity.cityMillitary *= Random.Range (0.8f, 1.2f);
		float ally = scrCity.playerMilitary;
		float enemy = scrCity.cityMillitary;

		str = ScrLanguage.Translate ("expedition_name_invade_finish");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrWar.textTitle.text = str;

		scrWar.textPlayer.text = ScrLanguage.Translate ("city_name_player");
		scrWar.textEnemy.text = ScrLanguage.Translate (scrCity.cityNameKey);

		str = ScrLanguage.Translate ("expedition_desc_invade_finish_start");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrWar.descStart = str;

		scrWar.playerArmy = ally;
		scrWar.enemyArmy = enemy;

		bool turn = (Random.value < ally/(ally + enemy));
		while (scrCity.cityMillitary > 0 && scrCity.playerMilitary > 0) {
			if (turn) {
				scrCity.cityMillitary -= scrCity.playerMilitary * Random.Range (0, 0.2f) * scrPlayer.modCombatStrength;
			}
			else {
				scrCity.playerMilitary -= scrCity.cityMillitary * Random.Range (0, 0.2f);
			}
			turn = !turn;
		}

		if (scrCity.cityMillitary <= 0f) {
			scrCity.cityMillitary = 0f;
			scrWar.playerTarget = scrCity.playerMilitary;
			scrWar.enemyTarget = scrCity.cityMillitary;

			str = ScrLanguage.Translate ("expedition_desc_invade_finish_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			str = str.Replace ("[val1]", (ally - scrCity.playerMilitary).ToString ("F0"));
			str = str.Replace ("[val2]", (enemy - scrCity.cityMillitary).ToString ("F0"));
			str = str.Replace ("[val3]", scrCity.playerMilitary.ToString ("F0"));
			scrWar.descFinished = str;

			scrCity.isAlly = true;
			scrCity.isVassal = true;
			scrPlayer.militaryVal += scrCity.playerMilitary;
			scrCity.playerMilitary = 0f;
		}
		else {
			scrCity.playerMilitary = 0f;
			scrWar.playerTarget = scrCity.playerMilitary;
			scrWar.enemyTarget = scrCity.cityMillitary;

			str = ScrLanguage.Translate ("expedition_desc_invade_finish_failed");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			str = str.Replace ("[val1]", (ally - scrCity.playerMilitary).ToString ("F0"));
			str = str.Replace ("[val2]", (enemy - scrCity.cityMillitary).ToString ("F0"));
			scrWar.descFinished = str;
		}

		scrWar.LateUpdate ();
		scrCanvas.uiWar.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	public void TradeEvent (GameObject objCity) {
		ScrCity scrCity = objCity.GetComponent <ScrCity> ();
		string str;

		scrConfirmation.textTitle.text = ScrLanguage.Translate ("expedition_name_trade_finish");
		if (Random.value < 0.8f) {
			str = ScrLanguage.Translate ("expedition_desc_trade_finish_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));

			float val;
			if (scrCity.tradeFood > 0f) {
				val = scrCity.tradeFood * Random.Range (0.75f, 1.25f) * scrPlayer.modTradeBonus;
				str = str.Replace ("[value]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_food"));
				scrPlayer.foodVal += val;
			}
			if (scrCity.tradeProduction > 0f) {
				val = scrCity.tradeProduction * Random.Range (0.75f, 1.25f) * scrPlayer.modTradeBonus;
				str = str.Replace ("[value]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_production"));
				scrPlayer.productionVal += val;
			}
			if (scrCity.tradeWealth > 0f) {
				val = scrCity.tradeWealth * Random.Range (0.75f, 1.25f) * scrPlayer.modTradeBonus;
				str = str.Replace ("[value]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_wealth"));
				scrPlayer.wealthVal += val;
			}
			if (scrCity.tradeCulture > 0f) {
				val = scrCity.tradeCulture * Random.Range (0.75f, 1.25f) * scrPlayer.modTradeBonus;
				str = str.Replace ("[value]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_culture"));
				scrPlayer.cultureVal += val;
			}
			if (scrCity.tradeMilitary > 0f) {
				val = scrCity.tradeFood * Random.Range (0.75f, 1.25f) * scrPlayer.modTradeBonus;
				str = str.Replace ("[value]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_military"));
				scrPlayer.militaryVal += val;
			}
			scrConfirmation.textDescription.text = str;

			scrCity.cityRelation += 1;
			if (scrCity.cityRelation > 2) {
				scrCity.cityRelation = 2;
			}

		} else {
			str = ScrLanguage.Translate ("expedition_desc_trade_finish_failed");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			scrConfirmation.textDescription.text = str; 
		}

		DefaultEvent ();
	}

	public void SendGiftEvent (GameObject objCity) {
		ScrCity scrCity = objCity.GetComponent <ScrCity> ();
		string str;

		scrConfirmation.textTitle.text = ScrLanguage.Translate ("expedition_name_sendgift_finish");
		if (Random.value < 0.8f) {
			str = ScrLanguage.Translate ("expedition_desc_sendgift_finish_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			scrConfirmation.textDescription.text = str;

			scrCity.cityRelation += 1;
			if (scrCity.cityRelation > 2) {
				scrCity.cityRelation = 2;
			}

		} else {
			str = ScrLanguage.Translate ("expedition_desc_sendgift_finish_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			scrConfirmation.textDescription.text = str;

			scrCity.cityRelation -= 1;
			if (scrCity.cityRelation < 0) {
				scrCity.cityRelation = 0;
			}
		}

		DefaultEvent ();
	}

	public void RaidEvent (GameObject objCity) {
		ScrCity scrCity = objCity.GetComponent <ScrCity> ();
		string str;

		scrCity.cityMillitary *= Random.Range (0.8f, 1.2f);
		float ally = scrCity.playerMilitary;
		float enemy = scrCity.cityMillitary * scrWorld.modArmyRaid;
		float enemyIdle = scrCity.cityMillitary * (1f - scrWorld.modArmyRaid);
		scrCity.cityMillitary = enemy;

		str = ScrLanguage.Translate ("expedition_name_raid_finish");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrWar.textTitle.text = str;

		scrWar.textPlayer.text = ScrLanguage.Translate ("city_name_player");
		scrWar.textEnemy.text = ScrLanguage.Translate (scrCity.cityNameKey);

		str = ScrLanguage.Translate ("expedition_desc_raid_finish_start");
		str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
		scrWar.descStart = str;

		scrWar.playerArmy = ally;
		scrWar.enemyArmy = enemy;

		bool turn = (Random.value < ally/(ally + enemy));
		while (scrCity.cityMillitary > 0f  && scrCity.playerMilitary > 0f) {
			if (turn) {
				scrCity.cityMillitary -= scrCity.playerMilitary * Random.Range (0f, 0.2f) * scrPlayer.modCombatStrength;
			}
			else {
				scrCity.playerMilitary -= scrCity.cityMillitary * Random.Range (0f, 0.2f);
			}
			turn = !turn;
		}


		if (scrCity.cityMillitary <= 0) {
			scrCity.cityMillitary = 0f;
			scrWar.playerTarget = scrCity.playerMilitary;
			scrWar.enemyTarget = scrCity.cityMillitary;

			str = ScrLanguage.Translate ("expedition_desc_raid_finish_success");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			str = str.Replace ("[val1]", (ally - scrCity.playerMilitary).ToString ("F0"));
			str = str.Replace ("[val2]", (enemy - scrCity.cityMillitary).ToString ("F0"));
			str = str.Replace ("[val3]", scrCity.playerMilitary.ToString ("F0"));

			float val;
			if (scrCity.tradeFood > 0f) {
				val = scrCity.tradeFood * Random.Range (0.5f, 1.0f);
				str = str.Replace ("[val4]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_food"));
				scrPlayer.foodVal += val;
			}
			if (scrCity.tradeProduction > 0f) {
				val = scrCity.tradeProduction * Random.Range (0.5f, 1.0f);
				str = str.Replace ("[val4]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_production"));
				scrPlayer.productionVal += val;
			}
			if (scrCity.tradeWealth > 0f) {
				val = scrCity.tradeWealth * Random.Range (0.5f, 1.0f);
				str = str.Replace ("[val4]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_wealth"));
				scrPlayer.wealthVal += val;
			}
			if (scrCity.tradeCulture > 0f) {
				val = scrCity.tradeCulture * Random.Range (0.5f, 1.0f);
				str = str.Replace ("[val4]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_culture"));
				scrPlayer.cultureVal += val;
			}
			if (scrCity.tradeMilitary > 0f) {
				val = scrCity.tradeMilitary * Random.Range (0.5f, 1.0f);
				str = str.Replace ("[val4]", val.ToString ("F0") + " " + ScrLanguage.Translate ("misc_military"));
				scrPlayer.militaryVal += val;
			}
			scrWar.descFinished = str;

			scrCity.isAlly = true;
			scrCity.isVassal = true;
			scrPlayer.militaryVal += scrCity.playerMilitary;
			scrCity.cityMillitary += enemyIdle;
		}
		else {
			scrCity.playerMilitary = 0f;
			scrWar.playerTarget = scrCity.playerMilitary;
			scrWar.enemyTarget = scrCity.cityMillitary;

			str = ScrLanguage.Translate ("expedition_desc_raid_finish_failed");
			str = str.Replace ("[name]", ScrLanguage.Translate (scrCity.cityNameKey));
			str = str.Replace ("[val1]", (ally - scrCity.playerMilitary).ToString ("F0"));
			str = str.Replace ("[val2]", (enemy - scrCity.cityMillitary).ToString ("F0"));
			scrWar.descFinished = str;

			scrCity.cityMillitary += enemyIdle;
		}

		scrWar.LateUpdate ();
		scrCanvas.uiWar.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	void DefaultEvent () {
		scrConfirmation.cancelMethod = Ok;
		scrConfirmation.clickedMethod = Ok;
		scrConfirmation.textYesButton.text = "Ok";
		scrConfirmation.buttonYesButton.interactable = true;

		scrCanvas.uiConfirmation.SetActive (true);
		scrCanvas.uiEmpty.SetActive (false);
	}

	public void Ok () {
		scrCanvas.uiEmpty.SetActive (true);
	}
}
