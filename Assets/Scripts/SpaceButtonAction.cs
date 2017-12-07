using UnityEngine;
using System.Collections;

public class SpaceButtonAction : MonoBehaviour {

	private bool viewBuildings = true;
	BuildingRenderer [] bUnits = null;

	float interval = 10f;

	void Start(){
		

	}

    void Update() {
        if (Input.GetKeyDown ("a")) {
                print ("space key was pressed");
                GameObject nav = GameObject.Find ("NavMap");
                if(nav != null) Destroy (nav);
                MapManager mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
                mapManager.xmlFileName = "Berlin.xml";
                mapManager.init ();
        }
        if (Input.GetKeyDown ("b")) {
                print ("space key was pressed");
                GameObject nav = GameObject.Find ("NavMap");
                if(nav != null) Destroy (nav);
                MapManager mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
                mapManager.xmlFileName = "London.xml";
                mapManager.init ();
        }
		if (Input.GetKeyDown ("q")) {
			if (bUnits == null) {
				bUnits = GameObject.FindObjectsOfType<BuildingRenderer>();
			}
	 		Debug.Log ("Hello");
			viewBuildings = !viewBuildings;
			foreach (BuildingRenderer b in bUnits) {
				b.gameObject.SetActive (viewBuildings);
			}
		}

		if (Input.GetKeyDown ("p")) {
			var tmp = GameObject.FindObjectsOfType<Spawn> ();
			foreach (Spawn b in tmp) {
				b.interval += interval;
			}
		}
	}
}
