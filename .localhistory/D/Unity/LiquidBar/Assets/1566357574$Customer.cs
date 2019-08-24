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
    public List<Color> colors;
    // Start is called before the first frame update
    void Start()
    {
        state = State.None;
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
