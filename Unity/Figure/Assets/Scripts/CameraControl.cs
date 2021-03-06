﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	[SerializeField]
	private Camera cam;

	[SerializeField]
	private Button camModeButton;
	[SerializeField]
	private Sprite camOnSprite;
	[SerializeField]
	private Sprite camOffSprite;

	private const float minCameraAngleX = 310.0f;
	private const float maxCameraAngleX = 20.0f;
	private const float swipeSpeed = 30.0f;
	private const float autoRotateSpeed = 20.0f;

	private Vector3 baseMousePos;
	private bool isMouseDown = false;

	private bool isAutoRotation = false;
	private bool cameraMode = true;

	public void ChangeCameraMode()
	{
		if (cameraMode)
		{
			camModeButton.GetComponent<Image>().sprite = camOnSprite;
			cameraMode = false;

		}
		else {
			camModeButton.GetComponent<Image>().sprite = camOffSprite;
			cameraMode = true;

		}

	}

	public void ChangeAutoMode()
	{
		if (isAutoRotation)
		{
			isAutoRotation = false;

		}
		else {
			isAutoRotation = true;

		}

	}

	public void ReloadScene()
	{
#pragma warning disable CS0618 // Type or member is obsolete
		Application.LoadLevel(Application.loadedLevel);
#pragma warning restore CS0618 // Type or member is obsolete

	}

	void Awake()
	{
		if (camModeButton.GetComponent<Image>().sprite == null)
		{
			camModeButton.GetComponent<Image>().sprite = camOffSprite;

		}

	}

	void Update()
	{
		if (isAutoRotation)
		{
			var angleY = transform.eulerAngles.y - Time.deltaTime * autoRotateSpeed;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, 0);

		}

		if (cameraMode)
		{
			
			if ((Input.touchCount == 1 && !isMouseDown) && Input.touchCount != 2 || Input.GetMouseButtonDown(0))
			{
				baseMousePos = Input.mousePosition;
				isMouseDown = true;

			}

			if (Input.GetMouseButtonUp(0))
			{
				isMouseDown = false;

			}

			if (isMouseDown)
			{
				var mousePos = Input.mousePosition;
				var distanceMousePos = (mousePos - baseMousePos);

				var angleX = transform.eulerAngles.x - distanceMousePos.y * swipeSpeed * 0.01f;
				var angleY = transform.eulerAngles.y + distanceMousePos.x * swipeSpeed * 0.01f;

				if ((angleX >= -10.0f && angleX <= maxCameraAngleX) || (angleX >= minCameraAngleX && angleX <= 370.0f))
				{
					transform.eulerAngles = new Vector3(angleX, angleY, 0);

				}
				else
				{
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, 0);

				}
				baseMousePos = mousePos;

			}

		}

	}
}
