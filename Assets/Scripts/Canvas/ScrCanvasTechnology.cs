using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrCanvasTechnology : MonoBehaviour {

	public string strColNormal;
	public string strColRed;

	public Sprite sprNormal;
	public SpriteState sprNormalState;
	public Sprite sprResearched;
	public SpriteState sprResearchedState;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Back () {
		gameObject.SetActive (false);
	}
}
