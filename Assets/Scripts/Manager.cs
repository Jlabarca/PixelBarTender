using Colourful;
using Colourful.Difference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public CameraController cameraController;
    public GameObject vaso;
    public GameObject tapa;
    public GameObject pociones;
    public GameObject seleccion;
    public GameObject canvas;
    public GameObject bar;
    private Customer customer;
    [SerializeField]
    private DrinkStats drinkStats;
    public Bartender bartender;
    public GravityFromAccelerometer gravityFromAccelerometer;
    public List<Customer> customerList;
    public List<Pocion> colorPotions;
    public List<Pocion> selectedColorPotions;
    /*Time*/
    private float initialTime;
    public Phase phase;
    public enum Phase { Start, Wait, Choose, Pour, Mix, Deliver, Score};

    // Start is called before the first frame update
    async void Start()
    {
        phase = Phase.Start;
        colorPotions = new List<Pocion>(FindObjectsOfType<Pocion>());
        canvas.SetActive(false);
        //await Test();
        await StartWaiting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task Test()
    {
        await StartChoosing();
        await Task.Delay(5000);
        await StartPouring();
        await Task.Delay(1000);
        await cameraController.ZoomSize(25);
        await Task.Delay(1000);
    }

    public async void CallStartChoosing(Customer customer)
    {
        initialTime = Time.time;
        customer.dialogue.SetActive(false);
        customer.state = Customer.State.Waiting;
        bartender.SetWantedColor(customer.wantedColor);
        this.customer = customer;
        await StartChoosing();
    }

    public async Task StartWaiting()
    {
        ResetVaso();
        phase = Phase.Wait;
        await MoveElementTo(vaso.transform, 0, 0);
        await MoveElementTo(pociones.transform, 0, 30);
        await MoveElementTo(tapa.transform, 0, 30);
        await Task.Delay(1000);
        cameraController.lookAt = vaso.transform;
        await cameraController.ZoomSize(20);
        await customerList[0].StartOrdering();
    }
    public async Task StartChoosing()
    {
        Debug.Log("Start Choosing...");
        phase = Phase.Choose;
        gravityFromAccelerometer.Disable();
        ResetPociones();
        await Task.Delay(200);
        cameraController.lookAt = seleccion.transform;
        await cameraController.ZoomSize(11);
        await Task.Delay(200);
        seleccion.SetActive(false);
        await Task.Delay(100);
        seleccion.SetActive(true);
        await Task.Delay(100);
        seleccion.SetActive(false);
        await Task.Delay(100);
        seleccion.SetActive(true);
        TogglePociones(true);
        await bartender.ShowVaso();
    }


    async Task StartPouring()
    {
        Debug.Log("Start Pouring...");
        bartender.dialogue.SetActive(false);
        await cameraController.ZoomSize(6);
        SetPourColor(0);
        SetPourColor(1);
        cameraController.lookAt = vaso.transform;
        await MoveElementTo(pociones.transform, 0, 0);
        await MoveElementTo(tapa.transform, 0, 30);
        phase = Phase.Pour;
        gravityFromAccelerometer.Disable();
        canvas.SetActive(true);
    }

    private void SetPourColor(int v)
    {
        Transform go = pociones.transform.GetChild(v);
        go.GetChild(0).GetComponent<SpriteRenderer>().color = selectedColorPotions[v].getColor();
        go.GetComponent<ParticleGenerator>().color = selectedColorPotions[v].getColor();
    }

    async Task StartMixing()
    {
        Debug.Log("Start Mixing...");
        var tasks = new List<Task>
        {
            FadeSpriteTo(bartender.gameObject, 0),
            FadeSpriteTo(bar, 0),
            cameraController.ZoomSize(5)
        };
        await Task.WhenAll(tasks);
        await MoveElementTo(pociones.transform, 0, 30);
        await MoveElementTo(tapa.transform, 0, 0);
        gravityFromAccelerometer.Enable();
        phase = Phase.Mix;
    }

    async Task MoveElementTo(Transform transform, float x, float y)
    {
        Debug.Log("MoveElementTo..." + x+","+y);
        Vector3 target = new Vector3(x, y, transform.position.z);
        while(transform.position.x != x || transform.position.y != y)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 100 * Time.deltaTime);
            await Task.Delay(10);
        }
    }


    async Task FadeSpriteTo(GameObject obj, float alpha)
    {
        
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Debug.Log("FadeSpriteTo..." + spriteRenderer);
        bool keepGoing = true;
        float extraAlpha = alpha;
        if (alpha == 0) extraAlpha = -1;
        if (alpha == 1) extraAlpha = 2;
        if (spriteRenderer != null)
        {
            while (keepGoing)
            {
                float lerp = Mathf.LerpUnclamped(spriteRenderer.color.a, extraAlpha, .1f);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
                    lerp);
                await Task.Delay(10);
                if (alpha == 0)
                    keepGoing = Mathf.CeilToInt(lerp) > alpha;
                else
                    keepGoing = Mathf.FloorToInt(lerp) < alpha;
            }
        }
    }

    public async Task ChangePhase()
    {
        switch(phase)
        {
            case Phase.Pour:
                await StartMixing();
                break;
            case Phase.Mix:
                await StartDeliver();
                break;
        }
    }

    private async Task StartDeliver()
    {
        canvas.SetActive(false);
        await MoveElementTo(pociones.transform, 0, 30);
        await MoveElementTo(tapa.transform, 0, 30);
        var tasks = new List<Task>
        {
            FadeSpriteTo(bartender.gameObject, 1),
            FadeSpriteTo(bar, 1),
            cameraController.ZoomSize(20)
        };
        await Task.WhenAll(tasks);
        await MoveElementTo(vaso.transform, -7.7f, -3.3f);
        await Task.Delay(1500);
        cameraController.lookAt = customer.transform;
        await Task.Delay(1000);
        await customer.StartEvaluating(await GetDrinkStats());
        await Task.Delay(1000);
        await bartender.Speak("Gracias");
        await StartWaiting();
    }

    public void ChangePhase2()
    {
        ChangePhase();
    }

    int maxPotions = 2;
    int currentPotion = 0;
    public async Task SetSelectedPotion(Pocion pocion)
    {
        Debug.Log("SetSelectedPotion");
    
        selectedColorPotions.Add(pocion);
        if (selectedColorPotions.Count > 1)
        {
            TogglePociones(false);
            await Task.Delay(500);
            await StartPouring();
        }

            
       //selectedColorPotions[0].ResetPosition();
    }

    public void ResetVaso()
    {
        DynamicParticle[] allObjects = (DynamicParticle[])FindObjectsOfType(typeof(DynamicParticle));
        foreach (DynamicParticle obj in allObjects)
        {
            Destroy(obj.gameObject);
        }
    }

    public void TogglePociones(bool active)
    {
        foreach(Pocion p in colorPotions)
        {
            p.gameObject.GetComponent<BoxCollider2D>().enabled = active;
        }
    }

    public void ResetPociones()
    {
        selectedColorPotions.Clear();
        foreach (Pocion p in colorPotions)
        {
            p.ResetPosition();
        }
    }

    private async Task<DrinkStats> GetDrinkStats()
    {
        /** http://www.colorwiki.com/wiki/Delta_E:_The_Color_Difference */
        float promR = 0, promG = 0, promB = 0;
        HashSet<Color> differentColors = new HashSet<Color>();
        DynamicParticle[] allObjects = (DynamicParticle[])FindObjectsOfType(typeof(DynamicParticle));
        int count = 0;
        int quantity = allObjects.Length;
        if (quantity < 10)
            return null;
        foreach (DynamicParticle obj in allObjects)
        {
            count++;
            Color color = obj.currentImage.gameObject.GetComponent<MeshRenderer>().materials[0].GetColor("_Color");
            promR += color.r;
            promG += color.g;
            promB += color.b;
            differentColors.Add(color);
            Destroy(obj.gameObject);
            if(count < 50)
                await Task.Delay(10); /** Simula que se toma la wea*/
        }

        promR = promR / allObjects.Length;
        promG = promG / allObjects.Length;
        promB = promB / allObjects.Length;
        Color promColor = new Color(promR, promG, promB);
        var color1 = new LabColor(promR, promG, promB);
        var color2 = new LabColor(customer.wantedColor.r, customer.wantedColor.g, customer.wantedColor.b);
        double deltaE = new CIEDE2000ColorDifference().ComputeDifference(color1, color2);



        drinkStats = new DrinkStats()
        {
            preparationTime = Time.time - initialTime,
            mixture = (differentColors.Count * 100)/ quantity,
            colorSimilarity = (float)deltaE,
            quantity = quantity,
            promColor = promColor
        };


        
 
        return drinkStats;
    }
}
[System.Serializable]
public class DrinkStats
{
    /**Cuanto tiempo en ms te demoraste en prepararlo */
    public float preparationTime;
    /**Que tan bien mezclado esta - valor de 0-1*/
    public float mixture;
    /**Que tan parecido es el color al solicitado - valor de 0-1*/
    public float colorSimilarity;
    /**Que tan parecido es el color al solicitado - valor de 0-1*/
    public float quantity;
    public Color promColor;

}

public class ColorFormulas
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public double CieL { get; set; }
    public double CieA { get; set; }
    public double CieB { get; set; }

    public ColorFormulas(float R, float G, float B)
    {
        RGBtoLAB(R, G, B);
    }

    public void RGBtoLAB(float R, float G, float B)
    {
        RGBtoXYZ(R, G, B);
        XYZtoLAB();
    }

    public void RGBtoXYZ(float RVal, float GVal, float BVal)
    {
        double R = Convert.ToDouble(RVal) / 255.0;       //R from 0 to 255
        double G = Convert.ToDouble(GVal) / 255.0;       //G from 0 to 255
        double B = Convert.ToDouble(BVal) / 255.0;       //B from 0 to 255

        if (R > 0.04045)
        {
            R = Math.Pow(((R + 0.055) / 1.055), 2.4);
        }
        else
        {
            R = R / 12.92;
        }
        if (G > 0.04045)
        {
            G = Math.Pow(((G + 0.055) / 1.055), 2.4);
        }
        else
        {
            G = G / 12.92;
        }
        if (B > 0.04045)
        {
            B = Math.Pow(((B + 0.055) / 1.055), 2.4);
        }
        else
        {
            B = B / 12.92;
        }

        R = R * 100;
        G = G * 100;
        B = B * 100;

        //Observer. = 2°, Illuminant = D65
        X = R * 0.4124 + G * 0.3576 + B * 0.1805;
        Y = R * 0.2126 + G * 0.7152 + B * 0.0722;
        Z = R * 0.0193 + G * 0.1192 + B * 0.9505;
    }

    public void XYZtoLAB()
    {
        // based upon the XYZ - CIE-L*ab formula at easyrgb.com (http://www.easyrgb.com/index.php?X=MATH&H=07#text7)
        double ref_X = 95.047;
        double ref_Y = 100.000;
        double ref_Z = 108.883;

        double var_X = X / ref_X;         // Observer= 2°, Illuminant= D65
        double var_Y = Y / ref_Y;
        double var_Z = Z / ref_Z;

        if (var_X > 0.008856)
        {
            var_X = Math.Pow(var_X, (1 / 3.0));
        }
        else
        {
            var_X = (7.787 * var_X) + (16 / 116.0);
        }
        if (var_Y > 0.008856)
        {
            var_Y = Math.Pow(var_Y, (1 / 3.0));
        }
        else
        {
            var_Y = (7.787 * var_Y) + (16 / 116.0);
        }
        if (var_Z > 0.008856)
        {
            var_Z = Math.Pow(var_Z, (1 / 3.0));
        }
        else
        {
            var_Z = (7.787 * var_Z) + (16 / 116.0);
        }

        CieL = (116 * var_Y) - 16;
        CieA = 500 * (var_X - var_Y);
        CieB = 200 * (var_Y - var_Z);
    }

    ///
    /// The smaller the number returned by this, the closer the colors are
    ///
    ///
    /// 
    public float CompareTo(ColorFormulas oComparisionColor)
    {
        // Based upon the Delta-E (1976) formula at easyrgb.com (http://www.easyrgb.com/index.php?X=DELT&H=03#text3)
        double DeltaE = Math.Sqrt(Math.Pow((CieL - oComparisionColor.CieL), 2) + Math.Pow((CieA - oComparisionColor.CieA), 2) + Math.Pow((CieB - oComparisionColor.CieB), 2));
        return Mathf.Round((float)DeltaE);
    }

    public static float DoFullCompare(Color a, Color b)
    {
        ColorFormulas oColor1 = new ColorFormulas(a.r, a.g, a.b);
        ColorFormulas oColor2 = new ColorFormulas(b.r, b.g, b.b);
        return oColor1.CompareTo(oColor2);
    }
}