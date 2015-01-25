using UnityEngine;
using System.Collections;

public class CameraSizer : MonoBehaviour
{
	public int baseOrthographicSize = 25;
	public int menuHeightPixels = 32;
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
			myCamera.orthographicSize = baseOrthographicSize * screenHeight / effectiveHeight;
		}
		else
		{
			myCamera.orthographicSize = baseOrthographicSize;
		}

		var fractionOfScreenForMainDisplay = 0.95f;
		myCamera.rect = new Rect(0, 0, 1, fractionOfScreenForMainDisplay);
	}
}
