using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject vaso;
    public Text text;
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
        new Color32 (57,71,54, 255),
        new Color32 (32, 50, 143, 255),
        new Color32 (143,32,50, 255),
        new Color32 (143,70,32, 255),
        new Color32 (143,126,32, 255),
        new Color32 (32,143,126, 255),
        new Color32 (126,32,143, 255),
        new Color32 (163,199,175, 255),
        new Color32 (144,225,12, 255),
        new Color32 (129,225,210, 255),
        new Color32 (129,192,225, 255),
        new Color32 (204,210,243, 255),
        new Color32 (210,243,204, 255)
    };
    // Start is called before the first frame update
    void Start()
    {
        state = State.None;
        dialogue.SetActive(false);
        SetWantedColor();
    }

    public void SetWantedColor()
    {
        wantedColor = colors[Random.Range(0, colors.Count - 1)];
        contenido.color = wantedColor;
    }

    private Queue<string> scripts = new Queue<string>();

    public async Task<int> StartOrdering()
    {
        SetWantedColor();
        await Task.Delay(1000);
        state = State.Ordering;
        //await Speak("<size=50>Hola</size>, dame vaso de ...");
        await ShowVaso();
        return 5;
    }


    public async Task<int> StartEvaluating(DrinkStats drinkStats)
    {
        state = State.Evaluating;
        if (drinkStats == null)
        {
            await Speak("ESta wea esta vaciaaaaaaaaaaaaaaaaa gil culiao estafador");
            return 0;
        }
        int score = 100;
        Debug.Log(drinkStats.quantity);
        /* tiempo */
        float seconds = Mathf.Round(drinkStats.preparationTime * 100) / 100;
        switch (seconds)
        {
            case float n when n >= 80:
                await Speak("Te demoraste mucho! ");
                score -= 50;
                break;
            case float n when n >= 30:
                await Speak("Algo lento");
                score -= 20;
                break;
            case float n when n < 5:
                await Speak("Culiao rápido");
                score += 20;
                break;
            case float n when n < 10:
                await Speak("OOH que rápido");
                score += 10;
                break;
        }

        /*Cantidad*/
        switch (drinkStats.quantity)
        {
            case float n when n >= 100:
                await Speak("Buena cantidad!");
                break;
            case float n when n >= 80:
                break;
            case float n when n >= 50:
                await Speak("Hermano que wea con la cantidad");
                score -= 30;
                break;
            case float n when n >= 30:
                await Speak("Tan poco que le echaste... cagao");
                score -= 50;
                break;
            case float n when n < 30:
                await Speak("Entrega monea chchtumare");
                score -= 70;
                break;
            case float n when n < 10:
                await Speak("ESta wea esta vaciaaaaaaaaaaaaaaaaa gil culiao estafador");
                score -= 170;
                break;
        }
        if (drinkStats.quantity < 30)
            return score;
        
        /*Mezcla*/
        switch (drinkStats.mixture)
        {
            
            case float n when n >= 92:
                break;
            case float n when n >= 90:
                await Speak("Mmm...");
                score -= 10;
                break;
            case float n when n >= 80:
                await Speak("Lo mezclaste poco eh");
                score -= 20;
                break;
            case float n when n < 70:
                await Speak("Ni lo mezclaste csmmmm!");
                score -= 30;
                break;
            
            
        }

        


        /*Color*/
        switch (drinkStats.colorSimilarity)
        {
            case float n when n >= .55f:
                await Speak("No se parece a lo que te pedí");
                score -= 50;
                break;
            case float n when n >= .46f:
                await Speak("Esta raro el color");
                score -= 30;
                break;
            case float n when n >= .4f:
                await Speak("Se ve bien");
                score += 10;
                break;
            case float n when n >= .3f:
                await Speak("Muy buen color");
                score += 20;
                break;
            case float n when n < .3f:
                //await Speak("Est");
                score += 30;
                break;
        }


        /*En resumen*/
        //await Speak("En resumen.....");
        await Task.Delay(1000);
        switch (score)
        {
            case int n when n >= 90:
                await Speak("Es perfecto!");
                break;
            case int n when n >= 80:
                await Speak("Bien Hecho!");
                break;
            case int n when n >= 70:
                await Speak("Bastante bueno");
                break;
            case int n when n >= 40:
                await Speak("Maomeno noma");
                break;
            case int n when n < 40:
                await Speak("Te quedó como las <size=40>weas</size>");
                break;
        }
        
        Debug.Log("Score: " + score);
        return score;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task Speak(string speech)
    {
        vaso.SetActive(false);
        text.gameObject.SetActive(true);
        dialogue.SetActive(true);
        typeTextComponent.SetText(speech);
        await typeTextComponent.IsSpeechDone.Task;
        await Task.Delay(500);
        dialogue.SetActive(false);
    }

    public async Task ShowVaso()
    {
        typeTextComponent.SetText("");
        vaso.SetActive(true);
        text.gameObject.SetActive(false);
        dialogue.SetActive(true);
        await Task.Delay(500);
        await AnimationUtils.PushMeAnimation(dialogue.transform);
    }
}
