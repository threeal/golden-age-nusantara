using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrHex : MonoBehaviour {

	public bool isLoad = false;
	public string prefab;
	public int id;

	public string buildNameKey;
	public string buildDescKey;

	public string hexName = "name";
	public string infoDescription = "";
	public GameObject[] hexNearby;
	public int[] hexNearbyId;
	public GameObject[] unified;
	public int[] unifiedId;
	public bool isReveal = false;
	public bool isWater = false;
	public bool isFreshWater = false;

	public GameObject objBuild;
	public int objBuildId;

	GameObject objController;
	ScrController scrController;
	public MeshRenderer meshRenderer;

	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		if (!isLoad) {
			//positioning
			Vector3 pos = transform.position;
			float degree = Mathf.Deg2Rad * 30;
			pos.x = Mathf.Round (pos.x / (Mathf.Cos (degree) * 3)) * Mathf.Cos (degree) * 3;
			pos.y = 0;
			pos.z = Mathf.Round (pos.z / (Mathf.Sin (degree) * 3)) * Mathf.Sin (degree) * 3;
			transform.position = pos;

			//search all nearby hex
			hexNearby = new GameObject[6];
			for (int i = 0; i < 6; i++) {
				pos = transform.position;
				degree = Mathf.Deg2Rad * i * 60;
				pos.x += (Mathf.Cos (degree) - Mathf.Sin (degree)) * 3;
				pos.y += 1000f;
				pos.z += (Mathf.Sin (degree) + Mathf.Cos (degree)) * 3;

				RaycastHit hit;
				if (Physics.Raycast (pos, Vector3.down, out hit, Mathf.Infinity, 1 << 8)) {
					hexNearby [i] = hit.collider.gameObject;
				} else {
					hexNearby [i] = null;
				}
			}

			//if not explored
			if (!isReveal) {
				ScrBuild scr = GetComponentInChildren <ScrBuild> ();

				Build (scrController.objCloud);
				if (objBuild) {
					ScrBuild scrBuild = objBuild.GetComponent <ScrBuild> ();
					if (scr != null) {
						scrBuild.builtObject = scr.gameObject;
						scr.gameObject.SetActive (false);
					}
				}
			} else {
				if (unified.Length > 0) {
					CloudReveal (unified);
				}
			}
		}

		if (!isReveal) {
			meshRenderer.enabled = false;
		}

		if (isWater) {
			Vector3 pos = transform.position;
			pos.y += 2.9f;
			Instantiate (scrController.objWater, pos, scrController.objWater.transform.rotation, scrController.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Build (GameObject building) {
		GameObject temp = null;
		Quaternion rotation;

		if (objBuild) {
			temp = objBuild;
			temp.GetComponent <ScrBuild> ().enabled = false;
		}

		if (building.GetComponent <ScrBuild> ().isStatic) {
			rotation = building.transform.rotation;
		} else {
			rotation = transform.rotation;
		}

		objBuild = (GameObject) Instantiate ((Object) building, transform.position, rotation);
		objBuild.SetActive (true);
		objBuild.GetComponent <ScrBuild> ().enabled = true;
		objBuild.GetComponent <ScrBuild> ().Positioning ();
		if (objBuild) {
			if (temp && objBuild.GetComponent <ScrBuild> ().buildTag != "cloud") {
				rotation = temp.transform.rotation;
				temp.transform.parent = objBuild.transform;
				temp.transform.rotation = rotation;
			}
		}
	}

	public GameObject[] NearbyRange (int range) {
		List <GameObject> nearby = new List <GameObject> ();
		List <GameObject> list = new List <GameObject> ();
		List <GameObject> temp = new List <GameObject> ();

		nearby.Clear ();
		nearby.Add (gameObject);
		list.Clear ();
		list.Add (gameObject);

		for (int i = 0; i < range; i++) {
			temp.Clear ();

			foreach (GameObject parent in list) {
				if (parent) {
					foreach (GameObject child in parent.GetComponent <ScrHex> ().hexNearby) {
						if (child) {
							if (!nearby.Contains (child)) {
								nearby.Add (child);
								temp.Add (child);
							}
						}
					}
				}
			}

			list = new List<GameObject> (temp);
		}

		GameObject[] ret = new GameObject[nearby.Count];
		nearby.CopyTo (ret);

		return ret;
	}

	public void CloudReveal (GameObject[] hexList) {
		foreach (GameObject hex in hexList) {
			if (hex) {
				if (!hex.GetComponent <ScrHex> ().isReveal) {
					GameObject build = hex.GetComponent <ScrHex> ().objBuild;
					if (build) {
						ScrBuild scr = build.GetComponent <ScrBuild> ();
						if (scr.buildTag == "cloud" || scr.buildTag == "cloudclear") {
							scr.isBuilt = true;
							scr.Destroy ();
						} else {
							hex.GetComponent <ScrHex> ().isReveal = true;
							meshRenderer.enabled = true;
						}
					} else {
						hex.GetComponent <ScrHex> ().isReveal = true;
						meshRenderer.enabled = true;
					}
				}
			}
		}
	}
}
