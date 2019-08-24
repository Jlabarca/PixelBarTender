﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject camera;
    public GameObject vaso;
    public GameObject tapa;
    public Phase phase;
    public enum Phase { Start, Tutorial, Pour, Mix, Deliver};

    // Start is called before the first frame update
    void Start()
    {
        phase = Phase.Start;
        StartPouring();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async Task StartPouring()
    {
        Debug.Log("Started phase...");
        await Task.Delay(600);
        phase = Phase.Pour;
    }

    async Task StartMixing()
    {
        Debug.Log("Started phase...");
        await Task.Delay(600);
        phase = Phase.Mix;
    }
}
