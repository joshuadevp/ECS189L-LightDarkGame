using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    Player player;

    [SerializeField] private IPlayerCommand fire1;
    [SerializeField] private IPlayerCommand fire2;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        fire1 = gameObject.AddComponent<TestCommand>();
    }

    // Update is called once per frame
    void Update()
    {
        // Using Raw values to give more precise movement
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Fire1"))
        {
            this.fire1?.Execute(this.gameObject);
        }
        if (Input.GetButton("Fire2"))
        {
            this.fire2?.Execute(this.gameObject);
        }

        transform.Translate(new Vector2(x, y).normalized * Time.deltaTime * player.Speed);
    }
}
