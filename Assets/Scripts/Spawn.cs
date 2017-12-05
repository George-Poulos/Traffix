using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public float interval = 5f;
    public GameObject[] cars;

    private List<NavPath> paths;
    private Node spawnNode;
    private NavMap navMap;

    void Awake() {
        spawnNode = gameObject.GetComponent<Node>();
        navMap = GameObject.Find("NavMap").GetComponent<NavMap>();
    }

    // Use this for initialization
    void Start () {
		navMap = GameObject.Find("NavMap").GetComponent<NavMap>();
        var tmpPaths = navMap.getPaths(spawnNode.id);
        paths = new List<NavPath>();
        foreach(Node n in navMap.spawnPoints) {
            if(tmpPaths[navMap.pathMapping[n.id]].isPath && n.id != spawnNode.id)
                paths.Add(tmpPaths[navMap.pathMapping[n.id]]);
        }
		Debug.Log (paths.Count);
        if(paths.Count > 0)
            StartCoroutine(SpawnPrefab());
    }

    // Update is called once per frame
    void Update () {

    }

    private IEnumerator SpawnPrefab() {
        System.Random rand = new System.Random();
        while(true) {
            float waitTime = (float)rand.NextDouble();
            yield return new WaitForSeconds(waitTime * interval);
            var p = paths[rand.Next(paths.Count)];
            GameObject spawnedCar = Instantiate(cars[rand.Next(cars.Length)], transform.position, Quaternion.identity) as GameObject;
            spawnedCar.GetComponent<Car>().route = p.getPoints().ToArray();
        }
    }
}
