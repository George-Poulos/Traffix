﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScale : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	public void ValueChangeCheck(float newValue)
	{
		var tmp = GameObject.FindObjectsOfType<Spawn> ();
		foreach (Spawn b in tmp) {
			b.interval = newValue;
		}
	}
}
