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
    public GameObject seleccion;
    public Customer customer;
    public Bartender bartender;
    public GravityFromAccelerometer gravityFromAccelerometer;
    public Phase phase;
    public enum Phase { Start, Waiting, Choose, Pour, Mix, Deliver};

    // Start is called before the first frame update
    async void Start()
    {
        phase = Phase.Start;
        //await Test();
        //await StartPouring();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task Test()
    {
        await StartChoosing();
        await Task.Delay(5000);
        await StartPouring();
        await Task.Delay(1000);
        await cameraController.ZoomSize(25);
        await Task.Delay(1000);
    }

    public async void CallStartChoosing(Customer customer)
    {
        customer.chat.SetActive(false);
        customer.state = Customer.State.Waiting;
        bartender.SetWantedColor(customer.wantedColor);
        this.customer = customer;
        
    }

    public async Task StartChoosing()
    {
        Debug.Log("Start Choosing...");
        phase = Phase.Choose;
        gravityFromAccelerometer.Disable();
        cameraController.lookAt = seleccion.transform;
        await Task.Delay(500);
        await cameraController.ZoomSize(11);
        bartender.chat.SetActive(true);
        bartender.vaso.SetActive(false);
        await Task.Delay(500);
        bartender.text.text = "OK, entonces un vaso de:";
        await Task.Delay(1000);
        bartender.text.text = "";
        bartender.vaso.SetActive(true);
        await Task.Delay(1500);
        await StartChoosing();
        await Task.Delay(500);
        seleccion.SetActive(false);
        await Task.Delay(200);
        seleccion.SetActive(true);
        await Task.Delay(200);
        seleccion.SetActive(false);
        await Task.Delay(200);
        seleccion.SetActive(true);
        await cameraController.ZoomSize(11);
    }


    async Task StartPouring()
    {
        Debug.Log("Start Pouring...");
        cameraController.lookAt = vaso.transform;
        await MoveElementTo(pociones.transform, 0, 0);
        await MoveElementTo(tapa.transform, 0, 5);
        phase = Phase.Pour;
        gravityFromAccelerometer.Disable();
    }

    async Task StartMixing()
    {
        Debug.Log("Start Mixing...");
        await MoveElementTo(pociones.transform, 0, 5);
        await MoveElementTo(tapa.transform, 0, 0);
        gravityFromAccelerometer.Enable();
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
