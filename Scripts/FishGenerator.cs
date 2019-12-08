using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public float generateRadius;
    public int generateNum;
    public List<string> fishes;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < generateNum; i++)
        {
            GenerateOne(fishes[Random.Range(0, fishes.Count)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateOne(string name)
    {
        string path = "FishPrefabs/" + name;
        Instantiate((GameObject)Resources.Load(path), Random.insideUnitSphere * generateRadius, Quaternion.Euler(0, Random.value * 360, 0));
    }
}
