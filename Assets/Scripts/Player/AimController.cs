using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private PlayerCommand fire1;
    [SerializeField] private PlayerCommand fire2;

    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = FindObjectOfType<MouseScreenController>().transform.position;
        rb = GetComponent<Rigidbody2D>();

        fire1 = gameObject.GetComponent<PrimaryShotCommand>();
        fire2 = gameObject.GetComponent<UpgradeTestCommand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            this.fire1?.Execute(this.gameObject);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            this.fire2?.Execute(this.gameObject);
        }

        mousePosition = FindObjectOfType<MouseScreenController>().transform.position;
        Vector2 aimDirection = mousePosition - transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        //Quaternion q = Quaternion.euler(aimAngle);
        rb.rotation = aimAngle;
        
    }
}
