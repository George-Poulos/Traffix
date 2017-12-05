using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScale : MonoBehaviour {

	float value; 
	NavMap nav;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	public void ValueChangeCheck(float newValue)
	{
		nav = GameObject.Find("NavMap").GetComponent<NavMap>();
		value = newValue;
		nav.transform.localScale = new Vector3 (value, value, value);
	}
}
