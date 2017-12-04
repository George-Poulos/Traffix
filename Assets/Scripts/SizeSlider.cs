﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeSlider : MonoBehaviour {

	float value; 
	NavMap nav = GameObject.Find("NavMap").GetComponent<NavMap>();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ValueChangeCheck(float newValue)
	{
		value = newValue;
		nav.transform.localScale = new Vector3 (value, value, value);
	}
}
