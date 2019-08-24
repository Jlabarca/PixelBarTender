using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public GameObject chat;
    public Color wantedColor;
    public SpriteRenderer contenido;
    public State state;
    public enum State { None, Ordering, Waiting, Evaluating };
    private List<Color> colors = new List<Color>()
    {
        new Color(8f, 38f, 88f),
        new Color(25f, 10f, 91f),
        new Color(206f, 145f, 36f),
        new Color(206f, 173f, 36f)
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
