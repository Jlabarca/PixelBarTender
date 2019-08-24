using UnityEngine;

public class GravityFromAccelerometer : MonoBehaviour
{

    // gravity constant
    public float g = 9.8f;
    public Vector2 actualGravity;
    void Update()
    {
        // normalize axis
        

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Physics2D.gravity = new Vector2(moveHorizontal, moveVertical) * g;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Physics2D.gravity = Input.acceleration * 5 * g;
        }
        actualGravity = Physics2D.gravity;
    }

}