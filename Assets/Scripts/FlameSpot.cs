using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpot : MonoBehaviour
{

    public GameObject prefab;
    public GameObject[] gameObjects;
    public int objNum = 250;
    public float time;
    private int pivot = 0;

    void Start()
    {
        gameObjects = new GameObject[objNum];
        for (int i = 0; i < objNum; i++)
        {
            GameObject gameObject = Instantiate(prefab, transform);
            gameObjects[i] = gameObject;
            gameObject.SetActive(false);
        }

        StartCoroutine("EnableCube");
    }

    IEnumerator EnableCube()
    {
        while (true)
        {
            gameObjects[pivot++].SetActive(true);
            if (pivot == objNum) pivot = 0;
            yield return new WaitForSeconds(time);
        }
    }
}
