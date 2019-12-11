using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class underWater : MonoBehaviour
{
    public GameObject cm;

    public GameObject checkObject;//角色

    private float blurspread = 0.3f;
    void Start()
    {
        RenderSettings.fog = false;//关闭 雾
        cm.GetComponent<Blur>().enabled = false;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject == checkObject)
        {
            cm.GetComponent<Blur>().enabled = true;
            cm.GetComponent<Blur>().iterations = 0;
            cm.GetComponent<Blur>().blurSpread = blurspread;


            RenderSettings.fog = true;

            RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
            RenderSettings.fogDensity = 0.04f;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == checkObject)
        {
            cm.GetComponent<Blur>().enabled = false;
            RenderSettings.fog = false;

        }
    }

}