﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Pocion : MonoBehaviour
{
    private SpriteRenderer content;
    private Vector3 initialPosition;
    private bool move = false;
    private Vector3 movePosition;
    // Start is called before the first frame update
    void Start()
    {
        content = transform.GetChild(0).GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnMouseUp()
    {
        CallMoveAndDisapear(new Vector3(0,-5,0));
    }

    void CallMoveAndDisapear(Vector3 target)
    {
        movePosition = target;
        MoveAndDisapear();
    }

    async Task MoveAndDisapear()
    {
        move = true;
        await Task.Delay(2000);
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        move = false;
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
