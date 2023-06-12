using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Player main { get; private set; }

    [SerializeField] private PlayerCommand fire1;
    [SerializeField] private PlayerCommand fire2;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Player>();
        fire1 = gameObject.GetComponent<PrimaryShotCommand>();
    }

    // Update is called once per frame
    void Update()
    {
        // Using Raw values to give more precise movement
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        /*
        if (Input.GetButton("Fire1"))
        {
            this.fire1?.Execute(this.gameObject);
        }
        if (Input.GetButton("Fire2"))
        {
            this.fire2?.Execute(this.gameObject);
        }
        */

        Vector3 movement = new Vector2(x, y).normalized * Time.deltaTime * main.Speed.Value;
        transform.position += movement;

        animator.SetBool("moving", x != 0 || y != 0);
        if (x < 0)
        {
            sprite.flipX = true;
        }
        else if (x > 0) 
        {
            sprite.flipX = false;
        }
    }
}
