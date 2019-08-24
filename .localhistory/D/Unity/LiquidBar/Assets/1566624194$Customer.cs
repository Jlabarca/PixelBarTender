using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public GameObject chat;
    public GameObject vaso;
    public TypeTextComponent typeTextComponent;
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
        SetWantedColor(colors[Random.Range(0, colors.Count-1)]);
        Debug.Log(wantedColor);
        StartOrdering();
    }

    public void SetWantedColor(Color color)
    {
        wantedColor = colors[Random.Range(0, colors.Count - 1)];
        contenido.color = wantedColor;
    }

    private Queue<string> scripts = new Queue<string>();

    async Task<int> StartOrdering()
    {
       // scripts.Dequeue
        await Task.Delay(2000);
        state = State.Ordering;
        vaso.SetActive(false);
        chat.SetActive(true);
        typeTextComponent.SetText("<size=50>Hola</size>, dame vaso de ", onComplete: () => Debug.Log("TypeText Complete"));
        typeTextComponent.TypeText(scripts.Dequeue(), onComplete: () => Debug.Log("TypeText Complete"));

        await Task.Delay(1000);
        vaso.SetActive(true);
        return 5;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
