using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("DataTech")]
public class ScrDataEvent {

	public string eventTag;
	public int eventTime = 0;

	public float enemy = 0f;
	public float ally = 0f;

	public bool eventDisplayed = false;
	public bool eventFinished = false;
	public bool eventFailed = false;

	public bool isActive = false;

}
