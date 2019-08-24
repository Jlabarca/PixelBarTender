using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public CameraController cameraController;
    public GameObject vaso;
    public GameObject tapa;
    public GameObject pociones;
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
        Debug.Log("Start Pouring...");
        await Task.Delay(500);
        await MoveElementTo(pociones.transform, 0 , 0);
        phase = Phase.Pour;


    }

    async Task StartMixing()
    {
        Debug.Log("Start Mixing...");
        await Task.Delay(600);
        phase = Phase.Mix;
    }

    async Task MoveElementTo(Transform transform, float x, float y)
    {
        Debug.Log(transform.position.y+"MoveElementTo..." + x+","+y);
        Vector3 target = new Vector3(x, y, transform.position.z);
        while(transform.position.y != y)
        {
            Debug.Log(transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, target, 20 * Time.deltaTime);
            await Task.Delay(100);
        }
       
    }
}
