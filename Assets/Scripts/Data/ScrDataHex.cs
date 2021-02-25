using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataHex")]
public class ScrDataHex {

	[XmlAttribute("name")]
	public string name;
	public string prefab;
	public int id;
	public bool scriptActive;

	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;

	public bool isReveal;
	public int[] hexNearbyId;
	public int[] unifiedId;
	public int objBuildId;

	public List<ScrDataBuild> childs;
}
