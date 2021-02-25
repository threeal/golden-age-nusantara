using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class ScrLanguage {

	public static SystemLanguage Language = SystemLanguage.English;
	public static Dictionary<string, string> LibEn = new Dictionary<string, string> ();
	public static Dictionary<string, string> LibId = new Dictionary<string, string> ();


	public static void Load () {
		LibEn.Clear ();
		LibId.Clear ();

		LoadXml ("Language/LanguageMisc");
		LoadXml ("Language/LanguageUI");
		LoadXml ("Language/LanguageBuild");
		LoadXml ("Language/LanguageTech");
		LoadXml ("Language/LanguageCity");
		LoadXml ("Language/LanguageChallenge");

		Debug.Log ("Language Loaded");
	}

	static void LoadXml (string xmlPath) {
		TextAsset xml = (TextAsset) Resources.Load (xmlPath, typeof(TextAsset));
		TextReader reader = new System.IO.StringReader (xml.text);
		XmlSerializer serializer = new XmlSerializer (typeof(ScrDataLanguage));
		ScrDataLanguage datas = ((ScrDataLanguage)serializer.Deserialize (reader));

		foreach (ScrDataLanguageKey data in datas.dataKey) {
			if (!LibEn.ContainsKey (data.key)) {
				LibEn.Add (data.key, data.en);
			}
			if (!LibId.ContainsKey (data.key)) {
				LibId.Add (data.key, data.id);
			}
		}
	}

	public static string Translate (string key) {
		string str = "???";
		string tmp;
		if (Language == SystemLanguage.English) {
			if (LibEn.TryGetValue (key, out tmp)) {
				str = tmp;
			}
		} else if (Language == SystemLanguage.Indonesian) {
			if (LibId.TryGetValue (key, out tmp)) {
				str = tmp;
			}
		}

		//replace rich text
		str = str.Replace ("[]", "\n");
		str = str.Replace ("[b]", "<b>");
		str = str.Replace ("[/b]", "</b>");

		return str;
	}

	public static string Color (string str, string key) {
		if (key == "col_white") {
			str = "<color=#FFFFFFFF>" + str + "</Color>";
		} else if (key == "col_red") {
			str = "<color=#FF9B9BFF>" + str + "</Color>";
		} else if (key == "col_green") {
			str = "<color=#BCFF9BFF>" + str + "</Color>";
		}

		return str;
	}
}
