using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// // from answers.unity3d.com/questions/1188342


public class VRTouchpadMove : MonoBehaviour
{
	public GameObject player;

	SteamVR_Controller.Device device;
	SteamVR_TrackedObject controller;

	Vector2 touchpad;

	float sensitivityX = 0.75F;
	float sensitivityForward = 1.5F;
	private Vector3 playerPos;

	void Start()
	{
		controller = gameObject.GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update()
	{
		device = SteamVR_Controller.Input((int)controller.index);
		if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);


			if (touchpad.y > 0.2f || touchpad.y < -0.2f) {
				// Move Forward
				player.transform.position += controller.transform.forward * Time.deltaTime * (touchpad.y * sensitivityForward);

			}

			if (touchpad.x > 0.3f || touchpad.x < -0.3f) {
				player.transform.Rotate (0, touchpad.x * sensitivityX, 0);
			
			}

		}
	}
}