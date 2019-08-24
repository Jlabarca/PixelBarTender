using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public GameObject chat;
    public GameObject vaso;
    public Text text;
    public Color wantedColor;
    public SpriteRenderer contenido;
    public State state;
    public enum State { None, Ordering, Waiting, Evaluating };
    private List<Color> colors = new List<Color>()
    {
        new Color32 (8, 38, 88, 255),
        new Color32 (25, 10, 91, 255),
        new Color32 (206, 145, 36, 255),
        new Color32 (206, 173, 36, 255)
    };
    // Start is called before the first frame update
    void Start()
    {
        state = State.None;
        wantedColor = colors[Random.Range(0, colors.Count-1)];
        contenido.color = wantedColor;
        Debug.Log(wantedColor);
        StartOrdering();
    }
    async Task<int> StartOrdering()
    {
        await Task.Delay(1000);
        state = State.Ordering;
        chat.SetActive(true);
        return 5;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
