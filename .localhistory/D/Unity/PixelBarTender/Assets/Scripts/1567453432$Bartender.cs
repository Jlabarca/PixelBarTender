using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Bartender : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject vaso;
    public Text text;
    public Color wantedColor;
    public SpriteRenderer contenido;
    public TypeTextComponent typeTextComponent;
    // Start is called before the first frame update
    void Start()
    {
        dialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWantedColor(Color color)
    {
        wantedColor = color;
        contenido.color = wantedColor;
    }

    public async Task Speak(string speech)
    {
        vaso.SetActive(false);
        dialogue.SetActive(true);
        text.gameObject.SetActive(true);
        typeTextComponent.SetText(speech);
        await typeTextComponent.IsSpeechDone.Task;
        await Task.Delay(500);
        dialogue.SetActive(false);
    }

    public async Task ShowVaso()
    {
        text.gameObject.SetActive(false);
        vaso.SetActive(true);
        dialogue.SetActive(true);
        await Task.Delay(500);
        await AnimationUtils.PushMeAnimation(dialogue.transform);
    }

}
