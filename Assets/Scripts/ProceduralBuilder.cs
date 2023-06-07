using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBuilder : MonoBehaviour
{
    [SerializeField] private GameObject[] sections;
    [SerializeField] private GameObject endPlatform;
    [SerializeField] private string proceduralDirectory;
    [SerializeField] private string endDirectory;
    [SerializeField] private Transform startPos;


    
    void Start()
    {
        sections = Resources.LoadAll<GameObject>(proceduralDirectory);
        endPlatform = Resources.Load<GameObject>(endDirectory);
        ShuffleArray(sections);

        foreach (GameObject obj in sections) {
            GameObject newGameObject = Instantiate(obj);
            newGameObject.transform.position = startPos.position;
            //obj.transform.position = startPos.position;
            startPos = newGameObject.transform.Find("End");
        }

        GameObject endObj = Instantiate(endPlatform);
        endObj.transform.position = startPos.position;

    }

    private void ShuffleArray(GameObject[] array) {
        for (int i = array.Length - 1; i >= 1; i--) {
            int j = Random.Range(0, i + 1);
            GameObject obj = array[i];
            array[i] = array[j];
            array[j] = obj;
        }
    }
}
