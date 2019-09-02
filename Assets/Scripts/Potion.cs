using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    SpriteRenderer sprite;
    ParticleGenerator particleGenerator;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        particleGenerator = GetComponent<ParticleGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        sprite.flipY = !sprite.flipY;
    }

    void OnMouseOver()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
        particleGenerator.emit = true;
    }

    void OnMouseExit()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        particleGenerator.emit = false;
    }
}
