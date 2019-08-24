using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task RunTaskAsync()
    {
        Debug.Log("Started task...");
        await Task.Delay(600);
    }
}
