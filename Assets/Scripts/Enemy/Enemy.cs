using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("In Editor: Base Stats\nIn Game: Final Stats")]
    //TODO Remove this field, this is just for the header attribute to work
    public string comment;
    [field: SerializeField] public float hp { get; private set; }
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float damage { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Possibly modify the hp/speed/damage here by some global modifier such as time/stage
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage) 
    {
        hp -= damage;
        if (hp <= 0) 
        {
            // Do something when dying
            OnDeath();
        }
    }

    void OnDeath() 
    {
        Destroy(gameObject);
    }
}
