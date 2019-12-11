using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Debug.Log("I see you fish");
            transform.parent.gameObject.SendMessage("SeeFish", other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Debug.Log("Good bye fish");
            transform.parent.gameObject.SendMessage("LostSightFish", other.gameObject);
        }
    }
}
