using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Setting_Game : MonoBehaviour
{

    public GameObject Win;
    public GameObject Lose;

    public Text textTime;
    private float mytime = 180;
    // Start is called before the first frame update
    void Start()
    {
        mytime = 180;
    }

    // Update is called once per frame
    void Update()
    {
        if (mytime > 0)
        {
            mytime -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(mytime);
            textTime.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
        else {
            textTime.text = "0:00";
            Lose.SetActive(true);
            mytime -= Time.deltaTime;
            if (mytime < -5) {
                Debug.Log("back");
            }
        }
    }
}
