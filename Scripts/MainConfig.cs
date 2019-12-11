using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainConfig : MonoBehaviour
{
    public PredatorEatList predatorEat;

    // Start is called before the first frame update
    void Start()
    {
        predatorEat = new PredatorEatList();
        predatorEat.LoadEatingList();
    }

    public bool PredatorMatch(string predator, string pray)
    {
        return (predatorEat.eatingList.ContainsKey(predator) && predatorEat.eatingList[predator].Contains(pray));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class PredatorEatList
{
    public Dictionary<string, HashSet<string>> eatingList;
    public bool loaded = false;
    public void LoadEatingList()
    {
        if (!loaded)
        {
            eatingList = new Dictionary<string, HashSet<string>>();
            string[] lines = System.IO.File.ReadAllLines("Assets/Resources/Configs/PredatorEatingList.txt");
            string scanPredator = "";
            foreach (string line in lines)
            {
                if (line[0] != '\t')
                {
                    eatingList.Add(line.Substring(0, line.Length - 1), new HashSet<string>());
                    scanPredator = line.Substring(0, line.Length - 1);
                }
                else
                {
                    string temp = line.Trim();
                    eatingList[scanPredator].Add(temp);
                }
            }
            loaded = true;
        }
    }
}