using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloydWarshall {
	public static void Main()
	{
		int[][] graph = new int[][]
		{
			new int[] {0,5,int.MaxValue, 10},
			new int[] {int.MaxValue,0, 3,int.MaxValue},
			new int[] {int.MaxValue, int.MaxValue, 0, 1},
			new int[] {int.MaxValue,int.MaxValue,int.MaxValue,0}
		};
		int [][] tmp = floydWarshall(graph);
		List<int> path = new List<int>();
		getPath(tmp, 0, 2, path);

	}

	/**
	 * Floyd Wrahsall algo that creates uses DP matrix that can later reconstruct paths using the getPath Function
	 */ 
	static int [][] floydWarshall(int [][] distanceMatrix){
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
  
	static int[][] initialMatrix(int[][] distanceMatrix) {
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

	static void getPath(int [][] paths, int i, int j, List<int> endPath){
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
