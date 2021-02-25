using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasEndTurn : MonoBehaviour {

	GameObject objPlayer;
	ScrPlayer scrPlayer;

	public Image panel;
	Color panelCol;

	public Text text;
	Color textCol;

	public float speed;

	bool isReverse = false;
	float alpha = 0;

	// Use this for initialization
	void Start () {
		objPlayer = GameObject.Find ("Player");
		scrPlayer = objPlayer.GetComponent <ScrPlayer> ();

		panelCol = panel.color;
		textCol = text.color;

		panel.color = SetAlpha (panelCol, 0f);
		text.color = SetAlpha (textCol, 0f);

		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (scrPlayer.reignNameKey == "ui_reign_settlement") {
			text.text = ScrLanguage.Translate ("ui_endtun1");
		} else {
			text.text = ScrLanguage.Translate ("ui_endtun2");
		}
		text.text = text.text.Replace ("[name]", ScrLanguage.Translate (scrPlayer.reignNameKey));
		text.text = text.text.Replace ("[time]", scrPlayer.turnCount.ToString ("F0"));

		if (!isReverse) {
			alpha += speed * Time.deltaTime;
			if (alpha >= 1.5f) {
				isReverse = true;
				alpha = 1.5f;
			}
		} else {
			alpha -= speed * Time.deltaTime;
			if (alpha <= 0) {
				gameObject.SetActive (false);
				isReverse = false;
				alpha = 0f;
			}
		}

		panel.color = SetAlpha (panelCol, alpha);
		text.color = SetAlpha (textCol, alpha);
	}

	Color SetAlpha (Color colBase, float a) {
		if (a > 0.9f) {
			a = 0.9f;
		}
		Color col;
		col = colBase;
		col.a = a;

		return col;
	}
}
