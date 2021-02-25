using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasEvents : MonoBehaviour {

	public RectTransform rectContent;
	GameObject objChallenge;
	ScrChallenge scrChallenge;

	// Use this for initialization
	void Start () {
		objChallenge = GameObject.Find ("Challenge");
		scrChallenge = objChallenge.GetComponent <ScrChallenge> ();
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		int pos = 0;
		foreach (GameObject obj in scrChallenge.listEvent) {
			if (obj) {
				if (obj.activeInHierarchy) {
					RectTransform rect = obj.GetComponent <RectTransform> ();
					rect.anchoredPosition = new Vector2 (0f, (-96f) * pos);
					pos++;
				}
			}
		}

		pos--;
		rectContent.sizeDelta = new Vector2 (0f, (96f) * pos);
	}
}
