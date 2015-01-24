using UnityEngine;
using System.Collections;

public class CameraSizer : MonoBehaviour
{
	Camera myCamera;
	int screenWidth;
	int screenHeight;

	void Awake()
	{
		myCamera = GetComponent<Camera>();
		if (null == myCamera)
		{
			Debug.LogError("CameraSizer needs to be on a game object with a camera component");
		}
	}

	void Update()
	{
		if (Screen.width != screenWidth || Screen.height != screenHeight)
		{
			RecalculateCameraSize();
		}
	}

	void RecalculateCameraSize()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		// Orthographic size uses screen height, not width, but if our aspect ratio gets way off,
		// we need to pick a new size based on the effective height to display our full content width
		if (screenWidth < screenHeight * 4 / 3)
		{
			var effectiveHeight = (screenWidth * 3 + 3) / 4;
			myCamera.orthographicSize = 25 * screenHeight / effectiveHeight;
		}
		else
		{
			myCamera.orthographicSize = 25;
		}
	}
}
