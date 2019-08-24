using UnityEngine;
using System.Collections;

[AddComponentMenu("Utilities/HUDFPS")]
public class FPSGUI : MonoBehaviour
{
    // Attach this to any object to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // corstartRect overall FPS even if the interval renders something like
    // 5.5 frames.

    public Rect startRect = new Rect(10, 10, 75, 50); // The rect the window is initially displayed at.
    public bool updateColor = true; // Do you want the color to change if the FPS gets low
    public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
    public float frequency = 0.5F; // The update frequency of the fps
    public int nbDecimal = 1; // How many decimal do you want to display

    private float accum = 0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
    private string sFPS = ""; // The fps formatted into a string.
    private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.
    public GameObject disablePixelPlane;
    public GameObject disableColorPlane;
    public GameObject disableEmiters;
    public GameObject disableAccelerometer;

    void Start()
    {
        Application.targetFrameRate = 300;
        StartCoroutine(FPS());
    }

    void Update()
    {
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
    }

    IEnumerator FPS()
    {
        // Infinite loop executed every "frenquency" secondes.
        while (true)
        {
            // Update the FPS
            float fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));

            //Update the color
            color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);

            accum = 0.0F;
            frames = 0;

            yield return new WaitForSeconds(frequency);
        }
    }

    void OnGUI()
    {
        float scalex = (float)(Screen.width) / 320f;
        float scaley = (float)(Screen.height) / 480f;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(scalex, scaley, 1));
        // Copy the default label skin, change the color and the alignement
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = updateColor ? color : Color.white;
        startRect = GUI.Window(0, startRect, DoMyWindow, "");

        if (GUI.Button(new Rect(105, 50, 50, 30), "Pixel"))
            disablePixelPlane.SetActive(!disablePixelPlane.activeSelf);
        if (GUI.Button(new Rect(160, 50, 50, 30), "Color"))
            disableColorPlane.SetActive(!disableColorPlane.activeSelf);

        if (GUI.Button(new Rect(105, 15, 50, 30), "Gravity"))
            disableAccelerometer.SetActive(!disableAccelerometer.activeSelf);
        if (GUI.Button(new Rect(160, 15, 50, 30), "Emit"))
            disableEmiters.SetActive(!disableEmiters.activeSelf);
    }

    void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(0, 0, startRect.width, startRect.height - 10), sFPS + " FPS", style);
        GUI.Label(new Rect(0, 0, startRect.width, startRect.height+20), GameObject.FindGameObjectsWithTag("DynamicParticle").Length + " Objs", style);
        if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
}