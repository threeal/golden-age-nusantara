using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataLanguage")]
public class ScrDataLanguage {
	[XmlArray("Keys")]
	[XmlArrayItem("Key")]
	public List<ScrDataLanguageKey> dataKey;
}