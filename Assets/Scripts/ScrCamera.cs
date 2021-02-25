using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ScrCamera : MonoBehaviour {

	GameObject objController;
	ScrController scrController;

	public bool isStatic = false;
	public bool isCinematic = false;

	public float movSpeed = 3f;
	public float movTouchSpeed = 1f;
	public float zoomSpeed = 0.5f;
	public float rotSpeed = 2f;
	public float scale = 0.7f;
	Vector3 posOriginal;
	Vector3 ScaleOriginal;
	Quaternion rotOriginal;
	Vector3 camPosOriginal;
	Vector3 camScaleOriginal;
	Quaternion camRotOriginal;
	Quaternion camRot;
	float touchRange;
	float touchAngle;
	float touchScale;

	public PostProcessingProfile[] qualityPostFX; 
	public PostProcessingBehaviour postFX;

	// Use this for initialization
	void Start () {
		objController = GameObject.Find ("Controller");
		scrController = objController.GetComponent <ScrController> ();

		posOriginal = transform.position;
		ScaleOriginal = transform.localScale;
		rotOriginal = transform.rotation;

		camPosOriginal = Camera.main.transform.position;
		camScaleOriginal = Camera.main.transform.localScale;
		camRotOriginal = Camera.main.transform.rotation;
		camRot = camRotOriginal;

		int width = (int)(Screen.currentResolution.width * (720f / (float)Screen.currentResolution.height));
		Screen.SetResolution (width, 720, true);
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 movement = Vector3.zero;
		float angle = 0f;

		if (isCinematic) {
			//cinematic
			transform.position = posOriginal;
			Rotate (Time.deltaTime * 5f);
			scale = 0.4f;
			Zoom (0f);

		} else if (isStatic) {
			touchAngle = 0f;
			Zoom (0f);
		}
		else {
			//movable


			if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.E)) {
				if (Input.GetKey (KeyCode.Q)) {
					angle = 30f;
				} else {
					angle = -30f;
				}

				Rotate (angle * rotSpeed * Time.deltaTime);
			}

			if (Input.mousePresent) {
				if (Input.GetKey (KeyCode.Mouse2)) {
					Rotate (Input.GetAxis ("Mouse X") * 100f * rotSpeed * Time.deltaTime);
					Zoom (Input.GetAxis ("Mouse Y") * 0.1f * zoomSpeed * Time.deltaTime);
				}

				if (Input.GetKey (KeyCode.Mouse1)) {
					movement -= transform.right * Input.GetAxis ("Mouse X") * movSpeed * scale * Time.deltaTime;
					movement -= transform.forward * Input.GetAxis ("Mouse Y") * movSpeed * scale * Time.deltaTime;
					Move (movement);
				}
			}

			if (scrController.IsOverEmpty () && !scrController.IsClickedOverUI ()) {
				if (Input.touchCount > 1) {
					float tempY, tempX;

					if (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (1).phase == TouchPhase.Began) {
						touchRange = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);
						tempY = Input.GetTouch (0).position.y - Input.GetTouch (1).position.y;
						tempX = Input.GetTouch (0).position.x - Input.GetTouch (1).position.x;
						touchAngle = Mathf.Atan2 (tempY, tempX) * Mathf.Rad2Deg;
						touchScale = scale;
					}

					float distance = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);
					scale = (touchScale * touchRange) / distance;

					tempY = Input.GetTouch (0).position.y - Input.GetTouch (1).position.y;
					tempX = Input.GetTouch (0).position.x - Input.GetTouch (1).position.x;
					angle = Mathf.Atan2 (tempY, tempX) * Mathf.Rad2Deg;

					Rotate (Mathf.DeltaAngle (touchAngle, angle) * rotSpeed);
					touchAngle = angle;

				} else if (Input.touchCount > 0) {
					if (Input.GetTouch (0).phase == TouchPhase.Moved) {
						Vector2 touchDeltaPos = Input.GetTouch (0).deltaPosition;
						movement -= transform.right * touchDeltaPos.x * movTouchSpeed * scale * Time.deltaTime;
						movement -= transform.forward * touchDeltaPos.y * movTouchSpeed * scale * Time.deltaTime;
					}
				}
			}

			movement += transform.right * Input.GetAxis ("Horizontal") * movSpeed * scale * Time.deltaTime;
			movement += transform.forward * Input.GetAxis ("Vertical") * movSpeed * scale * Time.deltaTime;
			Move (movement);

			Zoom (Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime);
		}
	}

	void Move (Vector3 movement) {
		transform.position += movement;
	}

	void Rotate (float value) {
		Camera.main.transform.rotation = camRot;
		transform.RotateAround (transform.position, Vector3.up, value);
		camRot = Camera.main.transform.rotation;
	}

	void Zoom (float value) {
		scale -= value;
		scale = Mathf.Clamp (scale, 0.3f, 0.9f);
		transform.localScale = ScaleOriginal * scale;

		Camera.main.transform.rotation = camRot;
		float rotation = Mathf.Pow (1 - scale, 2) * (-45f);
		Camera.main.transform.RotateAround (Camera.main.transform.position, Camera.main.transform.right, rotation);
	}

	public void Reset () {
		scale = 0.7f;
		transform.position = posOriginal;
		transform.localScale = ScaleOriginal;
		transform.rotation = rotOriginal;

		Camera.main.transform.position = camPosOriginal;
		Camera.main.transform.localScale = camScaleOriginal;
		Camera.main.transform.rotation = camRotOriginal;
		camRot = camRotOriginal;
	}
}
