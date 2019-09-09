using System.Threading.Tasks;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public float shakeAmount = 0.003f;
  
	public void ShakeIt(float shakeTime, float shakeAmount = 0.003f) {
        this.shakeAmount = shakeAmount;
        InvokeRepeating("StartCameraShaking", 0f, 0.01f);
        Invoke("StopCameraCameraShaking",shakeTime);
    }
    /*
    Task<int> MyAsyncMethod()
    {
        await Task.Delay(1000);
        return 5;
    }*/

    public async Task ShakeCamera(float shakeTime, float shakeAmount = 0.003f)
    {
        this.shakeAmount = shakeAmount;
        InvokeRepeating("StartCameraShaking", 0f, 0.01f);
        Invoke("StopCameraCameraShaking", shakeTime);
        await Task.Delay(shakeTime);
    }

    private void StartCameraShaking() {
        if(shakeAmount > 0) {
            Vector3 camPos = transform.position;
            float cameraShakingOffsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float cameraShakingOffsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += cameraShakingOffsetX;
            camPos.y += cameraShakingOffsetY;
            transform.position = camPos;
        }
    
    }

    private void StopCameraCameraShaking() {
        CancelInvoke("StartCameraShaking");
        transform.localPosition = Vector3.zero;
    }
}
