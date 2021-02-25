using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasWar : MonoBehaviour {

	public float speedFactor = 1f;
	public bool isWar = false;
	public bool isFinished = false;
	public bool turn = true;
	public float timer = 0f;

	public float playerArmy;
	public float enemyArmy;

	public float playerTarget;
	public float enemyTarget;

	public string descStart;
	public string descFinished;

	public Text textTitle;
	public Text textDescription;

	public Text textPlayer;
	public Text textPlayerArmy;
	public Text textEnemy;
	public Text textEnemyArmy;

	public GameObject uiYesButton;
	Button buttonYesButton;
	Text textYesButton;

	public Slider sliderInfluence;

	// Use this for initialization
	void Start () {
		buttonYesButton = uiYesButton.GetComponent <Button> ();
		textYesButton = uiYesButton.GetComponentInChildren <Text> ();
		timer = Random.Range (0.5f, 3f);
	}
	
	// Update is called once per frame
	public void LateUpdate () {
		if (playerArmy <= playerTarget) {
			turn = true;
		} else if (enemyArmy <= enemyTarget) {
			turn = false;
		} else {
			timer -= Time.deltaTime;
			if (timer <= 0f) {
				timer = Random.Range (0.5f, 3f);
				turn = !turn;
			}
		}

		if (!isFinished) {
			if (!isWar) {
				textDescription.text = descStart;
				buttonYesButton.interactable = true;
				textYesButton.text = ScrLanguage.Translate ("ui_attack");

			} else {
				float player = playerArmy;
				float enemy = enemyArmy;

				if (turn) {
					playerArmy -= (player + enemy) * ((player - playerTarget) / (enemy - enemyTarget)) * Random.value * Time.deltaTime * speedFactor;
					enemyArmy -= (player + enemy) * ((enemy - enemyTarget) / (player - playerTarget)) * Random.value * Time.deltaTime * (speedFactor/4);
				} else {
					playerArmy -= (player + enemy) * ((player - playerTarget) / (enemy - enemyTarget)) * Random.value * Time.deltaTime * (speedFactor/4);
					enemyArmy -= (player + enemy) * ((enemy - enemyTarget) / (player - playerTarget)) * Random.value * Time.deltaTime * speedFactor;
				}

				if (playerArmy <= playerTarget && enemyArmy <= enemyTarget) {
					isFinished = true;
				}

				if (playerArmy <= playerTarget) {
					playerArmy = playerTarget;
				}
				if (enemyArmy <= enemyTarget) {
					enemyArmy = enemyTarget;
				}

				textDescription.text = "";

				buttonYesButton.interactable = false;
				textYesButton.text = ScrLanguage.Translate ("ui_wait");
			}
		}
		else {
			textDescription.text = descFinished;

			buttonYesButton.interactable = true;
			textYesButton.text = "Ok";
		}

		textPlayerArmy.text = playerArmy.ToString ("F0");
		textEnemyArmy.text = enemyArmy.ToString ("F0");
		sliderInfluence.value = 0.05f + ((playerArmy / (playerArmy + enemyArmy)) * 0.9f);
	}

	public void yesClick() {
		if (isFinished) {
			isFinished = false;
			isWar = false;
			gameObject.SetActive (false);
		} else if (!isWar) {
			isWar = true;
		}
	}
}
