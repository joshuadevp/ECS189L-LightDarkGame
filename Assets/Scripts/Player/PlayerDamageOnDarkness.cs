using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageOnDarkness : MonoBehaviour
{
    [SerializeField]
    float damagePerSecond;
    private Player player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Darkness")
        {
            player.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
