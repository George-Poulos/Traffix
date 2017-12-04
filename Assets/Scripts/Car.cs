using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public Vector3[] route;
    public float MoveSpeed = 8;
    public Vector3 offset;
    public Vector3 lookOffset;

    Coroutine MoveIE;
    private Transform ObjectToMove;

    void Start()
    {
        ObjectToMove = transform;
        StartCoroutine(moveObject());
        for (int i = 0; i < route.Length; i++) {
            route[i] += offset;
        }
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
            ObjectToMove.transform.LookAt(route[currentPosition]);
            ObjectToMove.transform.rotation *= Quaternion.Euler(lookOffset);
            ObjectToMove.transform.position = Vector3.MoveTowards(ObjectToMove.transform.position, route[currentPosition], MoveSpeed * Time.deltaTime);
            yield return null;
        }

    }
}
