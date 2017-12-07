using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingToggle : MonoBehaviour {
	private bool viewBuildings = true;
	BuildingRenderer [] bUnits = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleBuilding(){
		if (bUnits == null || bUnits.Length == 0) {
			bUnits = GameObject.FindObjectsOfType<BuildingRenderer>();
		}
		viewBuildings = !viewBuildings;
		foreach (BuildingRenderer b in bUnits) {
			b.gameObject.SetActive (viewBuildings);
		}
	}
}
