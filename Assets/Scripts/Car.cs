using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public Vector3[] route;
    public float MoveSpeed = 8;
    Coroutine MoveIE;
    private Transform ObjectToMove;

    void Start()
    {
        ObjectToMove = transform;
        StartCoroutine(moveObject());
    }

    IEnumerator moveObject()
    {
        for (int i = 0; i < route.Length; i++)
        {
            MoveIE = StartCoroutine(Moving(i));
            yield return MoveIE;
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator Moving(int currentPosition)
    {
        while (ObjectToMove.transform.position != route[currentPosition])
        {
            ObjectToMove.transform.position = Vector3.MoveTowards(ObjectToMove.transform.position, route[currentPosition] , MoveSpeed * Time.deltaTime);
            yield return null;
        }

    }
}
