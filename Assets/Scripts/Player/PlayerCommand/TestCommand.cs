using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommand : MonoBehaviour, IPlayerCommand
{
    [SerializeField] float delay = 0.5f;
    [SerializeField] GameObject projectile;

    float lastCast = 0f;
    public void Execute(GameObject player) {
        if (Time.time - lastCast > delay)
        {
            Vector3 velocity = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            velocity -= player.transform.position;
            velocity.z = 0;
            velocity = velocity.normalized * 10f;
            GameObject proj = Instantiate(projectile, player.transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Initialize(1, velocity);
            lastCast = Time.time;
        }
    }
}
