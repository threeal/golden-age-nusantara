using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrPrologue : MonoBehaviour {
	
	public Text text;

	public GameObject objIntro;
	public bool isStart = false;

	public float alpha = 0f;
	public float speed = 1f;

	// Use this for initialization
	void Start () {
		SetAlpha (0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!objIntro.activeInHierarchy) {
			if (!isStart) {
				StartCoroutine (Sequences ());
				isStart = true;
			}
		}
	}

	IEnumerator Sequences () {
		alpha = 0f;
		speed = 1f;
		SetAlpha (alpha);

		for (int i = 1; i <= 14; i++) {
			text.text = ScrLanguage.Translate ("prologue_" + i.ToString ("F0"));
			alpha = 0f;
			speed = Mathf.Abs (speed);
			while (alpha >= 0) {
				alpha += speed * Time.deltaTime;
				if (alpha >= 1f + (text.text.Length * 0.02f)) {
					speed = -(Mathf.Abs (speed));
				}
				if (speed > 0f && Input.GetKeyDown (KeyCode.Mouse0)) {
					speed = -(Mathf.Abs (speed));
					if (alpha > 1f) {
						alpha = 1f;
					}
				}
				SetAlpha (alpha);
				yield return null;
			}
		}
			
		PlayerPrefs.SetInt ("continue", 0);
		objIntro.GetComponent <ScrIntro> ().nextScene = "Game";
		objIntro.SetActive (true);
	}

	void SetAlpha (float alpha) {
		float a = alpha;
		if (a > 1f) {
			a = 1f;
		} else if (a < 0f) {
			a = 0f;
		}

		Color col = text.color;
		col.a = a;
		text.color = col;
	}
}
