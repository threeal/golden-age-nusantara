using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataHex")]
public class ScrDataCity {

	public string cityTag;

	public int cityRelation;
	public float cityMillitary;
	public float playerMilitary;

	public bool isPlayer;
	public bool isAlly;
	public bool isVassal;

	public string commandTag = "";
	public int commandTime = 0;

}
