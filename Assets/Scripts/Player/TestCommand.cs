using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommand : PlayerCommand
{
    [SerializeField] float delay = 0.5f;
    [SerializeField] GameObject projectile;

    float lastCast = 0f;
    public override void Execute(GameObject player) {
        if (Time.time - lastCast > delay)
        {
            Vector3 velocity = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            velocity -= player.transform.position;
            velocity.z = 0;
            velocity = velocity.normalized * 10f;
            GameObject proj = Instantiate(projectile, player.transform.position, Quaternion.identity);
            lastCast = Time.time;
        }
    }
}
