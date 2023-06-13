using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectileCollisionDamage : MonoBehaviour
{
    Player player;
    FogOfDarknessManager darknessManager;
    private float hitCounter = 0;
    // Start is called before the first frame update
    void Awake()
    {
        darknessManager = GameObject.FindObjectOfType<FogOfDarknessManager>();
        player = GameObject.FindAnyObjectByType<Player>();
        hitCounter = player.ProjectilePierce.Value;
        transform.localScale = player.ProjectileSize.Value * new Vector3(1,1,1);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Darkness")
        {
            darknessManager.DamageDarkness(other.transform.position, (int) player.Damage.Value * 10);
            hitCounter -= 0.2f;
        }
        else if (hit.tag == "Enemy")
        {
            hitCounter--;
            hit.GetComponent<Enemy>().HitBy(this.gameObject, player.Damage.Value);
        }
        if (hitCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
