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
        sprite.flipY = true;
        particleGenerator.emit = true;
    }

    void OnMouseExit()
    {
        sprite.flipY = false;
        particleGenerator.emit = false;
    }
}
