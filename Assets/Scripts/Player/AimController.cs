using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private IAbilityCommand fire1;
    [SerializeField] private IAbilityCommand fire2;

    private float timeSinceFire1;
    private float timeSinceFire2;

    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = FindObjectOfType<MouseScreenController>().transform.position;
        rb = GetComponent<Rigidbody2D>();

        fire1 = gameObject.AddComponent<PrimaryFlickerCommand>();
        fire2 = gameObject.AddComponent<PrimaryCandleCommand>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceFire1 += Time.deltaTime;
        timeSinceFire2 += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            timeSinceFire1 = this.fire1.Execute(this.gameObject, timeSinceFire1);
        }
        if (Input.GetButton("Fire2"))
        {
            timeSinceFire2 = this.fire2.Execute(this.gameObject, timeSinceFire2);
            //this.fire2?.Execute(this.gameObject, timeSinceFire2);
        }

        mousePosition = FindObjectOfType<MouseScreenController>().transform.position;
        Vector2 aimDirection = mousePosition - transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        //Quaternion q = Quaternion.euler(aimAngle);
        rb.rotation = aimAngle;
        
    }
}
