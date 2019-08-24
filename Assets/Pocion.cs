using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Pocion : MonoBehaviour
{
    private SpriteRenderer content;
    private Vector3 initialPosition;
    private bool move = false;
    private Vector3 movePosition;
    private Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        content = transform.GetChild(0).GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        manager = FindObjectsOfType<Manager>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
            transform.position = Vector3.Lerp(transform.position, movePosition, 3 * Time.deltaTime);
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
        //manager.SetSelectedPotion(this);
        move = true;
        await Task.Delay(2000);
        gameObject.SetActive(false);
    }

    public void ResetPosition()
    {
        move = false;
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
