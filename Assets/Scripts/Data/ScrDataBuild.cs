using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataHex")]
public class ScrDataBuild {

	[XmlAttribute("name")]
	public string name;
	public string prefab;
	public int id;
	public bool objectActive;
	public bool scriptActive;

	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;

	public string buildNameKey;
	public string buildDescKey;
	public string buildNeedKey;
	public string buildEffectKey;

	public bool isWorked;
	public float builtcost;
	public int builtTime;

	public int clearObjectId;
	public int builtObjectId;
	public int builtInfoId;
	public int objHexId;

	public List<ScrDataBuild> childs;
}
