using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Transform lookAt;
    public GameObject vaso;
    public GameObject player;
    public Camera camera;
    public Vector3 velocity;
    public Vector3 offset;
    public float boundX=0.3f, boundY = 0.15f;
    public float cameraSmoothing = 0.05f;
    public CameraShake cameraShake;
    Vector3 targetPosition;
    // private Vector3 velocity = Vector3.zero;
    //public float dampTime  = 15;
    void Start()
    {
        cameraShake = camera.GetComponent<CameraShake>();
        offset = Vector3.zero;
        //cameraShake.ShakeIt(3, .07f);
    }
    private void FixedUpdate() {
        Vector3 delta = Vector3.zero;

        //this is to check if we are inside the bounds on the x axis
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX) {
            if (transform.position.x < lookAt.position.x) {
                //moving right
                delta.x = deltaX - boundX;
            }
            else {
                //moving left
                delta.x = deltaX + boundX;
            }
        }
        //this is to check if we are inside the bounds on the y axis
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY) {
            if (transform.position.y < lookAt.position.y) {
                delta.y = deltaY - boundY;
            }
            else {
                delta.y = deltaY + boundY;
            }
        }
        targetPosition = lookAt.position + offset;
        Vector3 position = new Vector3(transform.position.x, transform.position.y, -1f);
        Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, -1f);
        //Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * moveSpeed);
        //transform.position += newPosition;
        //targetPosition = transform.position + newPosition;
        //transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothing * Time.deltaTime);
        transform.position = PixelPerfectClamp(Vector3.SmoothDamp(position, newPosition, ref velocity, cameraSmoothing * Time.deltaTime));

    }


    public float pixelPerUnit = 40f;

    public Vector3 PixelPerfectClamp(Vector3 moveVector) {
        Vector3 vectorInPixels = new Vector3(
           Mathf.Round(moveVector.x * pixelPerUnit) / pixelPerUnit,
           Mathf.Round(moveVector.y * pixelPerUnit) / pixelPerUnit, moveVector.z);
        return vectorInPixels;
    }



    /** Zoom */

    public async Task ZoomSize(float size)
    {
        Debug.Log("ZoomSize..." + size);

        while (Mathf.Round(camera.orthographicSize) != Mathf.Round(size))
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, size, 20 * Time.deltaTime);
            await Task.Delay(10);
        }
        camera.orthographicSize = size;
    }
}
