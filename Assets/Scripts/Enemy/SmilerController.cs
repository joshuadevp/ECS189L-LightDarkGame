using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmilerController : MeleeEnemy
{
    [SerializeField] float AwakeRange;
    protected bool awake;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        awake = false;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (!awake) 
        {
            if ((player.transform.position - transform.position).magnitude < AwakeRange)
            {
                GameManager.Instance.AudioManager.PlayOneShot("Howling3");
                awake = true;
                GetComponentInChildren<Animator>().SetBool("Awaken", true);
            }
            else {
                return;
            }
        }
        base.Update();
    }
    public override void HitBy(GameObject projectile)
    {
        awake = true;
        GetComponentInChildren<Animator>().SetBool("Awaken", true);
        base.HitBy(projectile);
    }
}
