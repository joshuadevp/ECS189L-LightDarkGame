using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryModifierPickup : MonoBehaviour
{
    [SerializeField] StatModifier modifier;
    [SerializeField] float duration;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.AddTemporaryModifier(player.Speed, modifier, duration);
            Destroy(gameObject);
        }
    }
}
