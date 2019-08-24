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
    public GameObject gravityAccelerometer;
    public Phase phase;
    public enum Phase { Start, Tutorial, Pour, Mix, Deliver};

    // Start is called before the first frame update
    async void Start()
    {
        phase = Phase.Start;
        await Task.Delay(500);
        await StartPouring();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task Test()
    {
        await StartPouring();
        await Task.Delay(5000);
        await StartMixing();
    }

    async Task StartPouring()
    {
        Debug.Log("Start Pouring...");
        await MoveElementTo(pociones.transform, 0, 0);
        await MoveElementTo(tapa.transform, 0, 5);
        phase = Phase.Pour;
        gravityAccelerometer.SetActive(false);
    }

    async Task StartMixing()
    {
        Debug.Log("Start Mixing...");
        await MoveElementTo(pociones.transform, 0, 5);
        await MoveElementTo(tapa.transform, 0, 0);
        gravityAccelerometer.SetActive(true);
        phase = Phase.Mix;
    }

    async Task MoveElementTo(Transform transform, float x, float y)
    {
        Debug.Log("MoveElementTo..." + x+","+y);
        Vector3 target = new Vector3(x, y, transform.position.z);
        while(transform.position.x != x || transform.position.y != y)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 100 * Time.deltaTime);
            await Task.Delay(10);
        }
       
    }

    public async Task ChangePhase()
    {
        switch(phase)
        {
            case Phase.Pour:
                await StartMixing();
                break;
            case Phase.Mix:
                await StartPouring();
                break;
        }
    }

    public void ChangePhase2()
    {
        ChangePhase();
    }
}
