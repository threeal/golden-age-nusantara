using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrCanvasMenu : MonoBehaviour {

	GameObject objCamera;
	ScrCamera scrCamera;

	GameObject objIntro;
	ScrIntro scrIntro;

	GameObject objCanvas;
	ScrCanvas scrCanvas;

	public Text textTitle;

	public GameObject uiOption;
	public GameObject uiHelp;
	public GameObject uiCredit;

	public Button buttonOption;
	public Button buttonHelp;
	public Button buttonCredit;

	public GameObject uiLanguageEn;
	public GameObject uiLanguageId;

	public GameObject uiHelpEn;
	public GameObject uiHelpId;

	public GameObject uiCreditEn;
	public GameObject uiCreditId;

	public Text textGraphicText;
	public GameObject uiGraphicSlider;
	Slider sliderGraphicSlider;
	Text textGraphicSlider;

	public Text textMusicText;
	public GameObject uiMusicSlider;
	Slider sliderMusicSlider;
	Text textMusicSlider;

	public Text textSoundText;
	public GameObject uiSoundSlider;
	Slider sliderSoundSlider;
	Text textSoundSlider;

	AudioSource audioMusic;
	int levelCurrent;
	int levelCount;

	// Use this for initialization
	void Start () {
		objCamera = GameObject.Find ("Camera");
		scrCamera = objCamera.GetComponentInChildren <ScrCamera> ();

		objIntro = GameObject.Find ("Intro");
		scrIntro = objIntro.GetComponent <ScrIntro> ();

		objCanvas = GameObject.Find ("Canvas");
		scrCanvas = objCanvas.GetComponent <ScrCanvas> ();

		sliderGraphicSlider = uiGraphicSlider.GetComponent <Slider> ();
		textGraphicSlider = uiGraphicSlider.GetComponentInChildren <Text> ();

		sliderMusicSlider = uiMusicSlider.GetComponent <Slider> ();
		textMusicSlider = uiMusicSlider.GetComponentInChildren <Text> ();

		sliderSoundSlider = uiSoundSlider.GetComponent <Slider> ();
		textSoundSlider = uiSoundSlider.GetComponentInChildren <Text> ();

		audioMusic = objCamera.GetComponent <AudioSource> ();
		sliderMusicSlider.normalizedValue = audioMusic.volume;

		if (!PlayerPrefs.HasKey ("SettingGraphic")) {
			if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D11
			   || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Direct3D12) {
				PlayerPrefs.SetInt ("SettingGraphic", 2);
			} else if (SystemInfo.graphicsShaderLevel >= 50) {
				PlayerPrefs.SetInt ("SettingGraphic", 1);
			} else {
				PlayerPrefs.SetInt ("SettingGraphic", 0);
			}
		}
		levelCurrent = -1;
		levelCount = QualitySettings.names.Length - 1;
		sliderGraphicSlider.normalizedValue = ((float)PlayerPrefs.GetInt ("SettingGraphic")) / ((float)levelCount);

		if (!PlayerPrefs.HasKey ("SettingMusic")) {
			PlayerPrefs.SetInt ("SettingMusic", 80);
		}
		sliderMusicSlider.normalizedValue = ((float)PlayerPrefs.GetInt ("SettingMusic")) / 100f;


		if (!PlayerPrefs.HasKey ("SettingSound")) {
			PlayerPrefs.SetInt ("SettingSound", 80);
		}
		sliderSoundSlider.normalizedValue = ((float)PlayerPrefs.GetInt ("SettingSound")) / 100f;

		LateUpdate ();
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (uiOption.activeInHierarchy) {

			textTitle.text = ScrLanguage.Translate ("ui_option");

			scrCanvas.Activate (uiHelp, false);
			scrCanvas.Activate (uiCredit, false);

			buttonOption.interactable = false;
			buttonHelp.interactable = true;
			buttonCredit.interactable = true;

			textGraphicText.text = ScrLanguage.Translate ("ui_graphic");
			textMusicText.text = ScrLanguage.Translate ("ui_music");
			textSoundText.text = ScrLanguage.Translate ("ui_sound");

			int val = Mathf.RoundToInt (sliderGraphicSlider.normalizedValue * levelCount);
			if (!Input.GetKey (KeyCode.Mouse0)) {
				sliderGraphicSlider.normalizedValue = (float)val / (float)levelCount;
			}
			if (val != levelCurrent) {
				levelCurrent = val;
				PlayerPrefs.SetInt ("SettingGraphic", levelCurrent);
				scrCamera.postFX.profile = scrCamera.qualityPostFX [levelCurrent];
				QualitySettings.SetQualityLevel (levelCurrent, true);
			}
			textGraphicSlider.text = ScrLanguage.Translate ("ui_graphic_" + levelCurrent.ToString ("F0"));


			audioMusic.volume = sliderMusicSlider.normalizedValue;
			PlayerPrefs.SetInt ("SettingMusic", (int)(sliderMusicSlider.normalizedValue * 100f));
			textMusicSlider.text = (int)(sliderMusicSlider.normalizedValue * 100) + "%";

			PlayerPrefs.SetInt ("SettingSound", (int)(sliderSoundSlider.normalizedValue * 100f));
			textSoundSlider.text = (int)(sliderSoundSlider.normalizedValue * 100) + "%";

			scrCanvas.Activate (uiLanguageEn, ScrLanguage.Language == SystemLanguage.English);
			scrCanvas.Activate (uiLanguageId, ScrLanguage.Language != SystemLanguage.English);

		} else if (uiHelp.activeInHierarchy) {

			textTitle.text = ScrLanguage.Translate ("ui_help");

			scrCanvas.Activate (uiOption, false);
			scrCanvas.Activate (uiCredit, false);

			buttonOption.interactable = true;
			buttonHelp.interactable = false;
			buttonCredit.interactable = true;

			scrCanvas.Activate (uiHelpEn, ScrLanguage.Language == SystemLanguage.English);
			scrCanvas.Activate (uiHelpId, ScrLanguage.Language != SystemLanguage.English);
			
		} else if (uiCredit.activeInHierarchy) {
			
			textTitle.text = ScrLanguage.Translate ("ui_credit");

			scrCanvas.Activate (uiOption, false);
			scrCanvas.Activate (uiHelp, false);

			buttonOption.interactable = true;
			buttonHelp.interactable = true;
			buttonCredit.interactable = false;

			scrCanvas.Activate (uiCreditEn, ScrLanguage.Language == SystemLanguage.English);
			scrCanvas.Activate (uiCreditId, ScrLanguage.Language != SystemLanguage.English);

		}
	}

	public void Language () {
		if (ScrLanguage.Language == SystemLanguage.English) {
			ScrLanguage.Language = SystemLanguage.Indonesian;
			PlayerPrefs.SetString ("SettingLanguage", "id");
		} else {
			ScrLanguage.Language = SystemLanguage.English;
			PlayerPrefs.SetString ("SettingLanguage", "en");
		}
	}

	public void Back() {
		gameObject.SetActive (false);
	}

	public void Exit() {
		scrIntro.nextScene = "Menu";
		objIntro.SetActive (true);
	}

	public void Option () {
		scrCanvas.Activate (uiOption, true);
		scrCanvas.Activate (uiHelp, false);
		scrCanvas.Activate (uiCredit, false);
	}

	public void Help() {
		scrCanvas.Activate (uiOption, false);
		scrCanvas.Activate (uiHelp, true);
		scrCanvas.Activate (uiCredit, false);
	}

	public void Credit() {
		scrCanvas.Activate (uiOption, false);
		scrCanvas.Activate (uiHelp, false);
		scrCanvas.Activate (uiCredit, true);
	}

}
