using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DropDownAddOptions : MonoBehaviour {

	public Dropdown dropDown;
	Dictionary<int ,string> menuMap = new Dictionary<int ,string>();
	MapManager mapManager;

	void Start () {
		DirectoryInfo dir = new DirectoryInfo (Application.streamingAssetsPath);
		FileInfo[] info = dir.GetFiles ("*.xml");
		mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
		int id = 0;
		string currFile = mapManager.xmlFileName;
		foreach( var i in info){
			dropDown.options.Add (new Dropdown.OptionData (){text = i.Name});
			if (currFile == i.Name) {
				dropDown.value = id;
			}
			menuMap.Add (id, i.Name);
			++id;
		}


	}

	public void onChange(int val){
		GameObject nav = GameObject.Find("NavMap");
		Destroy (nav);
		if (menuMap.ContainsKey (val)) {
			mapManager.xmlFileName = menuMap [val];
			mapManager.init ();
		}
	}	
	
	// Update is called once per frame
	void Update () {
		
	}
}
