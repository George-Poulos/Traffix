using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloydWarshall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	 * Floyd Wrahsall algo that creates uses DP matrix that can later reconstruct paths using the getPath Function
	 */ 
	int [][] floydWarshall(int [][] distanceMatrix){
		int[][] paths = initialMatrix(distanceMatrix);
		for (int k = 0; k < distanceMatrix.Length; k++) {
			for (int i = 0; i < distanceMatrix.Length; i++) {
				for (int j = 0; j < distanceMatrix.Length; j++) {
					if (distanceMatrix[i][k] == int.MaxValue || distanceMatrix[k][j] == int.MaxValue) {
						continue;                  
					}
					if (distanceMatrix[i][j] > distanceMatrix[i][k] + distanceMatrix[k][j]) {
						distanceMatrix[i][j] = distanceMatrix[i][k] + distanceMatrix[k][j];
						paths[i][j] = paths[k][j];
					}

				}
			}
		}
		return paths;
	}

	int[][] initialMatrix(int[][] distanceMatrix) {
		int[][] paths = new int[distanceMatrix.Length][];
		for (int i = 0; i < distanceMatrix.Length; i++) {
			paths [i] = new int [distanceMatrix.Length];
			for (int j = 0; j < distanceMatrix.Length; j++) {
				if (distanceMatrix[i][j] != 0 && distanceMatrix[i][j] != int.MaxValue) {
					paths[i][j] = i;
				} else {
					paths[i][j] = -1;
				}
			}
		}
		return paths;
	}

	void getPath(int [][] paths, int i, int j, List<int> endPath){
		if (i == j){
			endPath.Add (i);
		}
		else if (paths [i][j] == 0){
			endPath.Clear ();
			endPath.Add (-1);
		}
		else{
			getPath(paths, i, paths[i][j], endPath);
			endPath.Add (j);
		}
	}
}
