using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 20);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            player.Heal(player.MaxHP.Value * 0.2f);
            Destroy(this.gameObject);
        }
    }
}
