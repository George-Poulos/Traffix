using UnityEngine;
using System.Collections;

public class SpaceButtonAction : MonoBehaviour {

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
        }
}
