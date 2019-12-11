using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    public float generateRadius;
    public int generateNum;
    public List<FishKind> fishes;
    public bool genSwitch;

    // Start is called before the first frame update
    void Start()
    {
        genSwitch = false;
        fishes = new List<FishKind>();
    }

    public IEnumerator GenerateFish()
    {
        //TextAsset kindsData = Resources.Load("Configs/FishKinds.txt") as TextAsset;
        string[] lines = System.IO.File.ReadAllLines("Assets/Resources/Configs/FishKinds.txt");
        //Debug.Log(kindsData);
        //string[] lines = kindsData.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        //Debug.Log("fuck");
        float totalRate = 0;
        foreach (string line in lines)
        {
            FishKind fishKind = new FishKind();
            fishKind.LoadLine(line);
            Debug.Log(fishKind);
            totalRate += fishKind.rate;
            fishes.Add(fishKind);
        }
        foreach (FishKind scanKind in fishes)
        {
            int kindNum = (int)(generateNum * (scanKind.rate / totalRate));
            for (int i = 0; i < kindNum; i++)
            {
                GenerateOne(scanKind.name);
                yield return null;
            }
            Debug.Log(scanKind.name + ": " + string.Format("{0:D}", kindNum));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (genSwitch)
        {
            StartCoroutine(GenerateFish());
            genSwitch = false;
        }
    }

    void GenerateOne(string name)
    {
        string path = "FishPrefabs/" + name;
        GameObject newFish = Instantiate(
            (GameObject)Resources.Load(path), 
            Random.insideUnitSphere * generateRadius, 
            Quaternion.Euler(0, Random.value * 360, 0)
            );
        newFish.name = name;
        newFish.transform.SetParent(transform);
    }
}

public class FishKind
{
    public string name;
    public float rate;
    public void LoadLine(string data)
    {
        string[] details = data.Split(' ');
        if (details.Length < 2)
        {

        }
        else
        {
            name = details[0];
            rate = float.Parse(details[1]);
        }
    }
}
