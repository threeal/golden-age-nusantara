using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataKey")]
public class ScrDataLanguageKey {
	[XmlAttribute("Key")]
	public string key = "";
	public string en = "";
	public string id = "";
}
