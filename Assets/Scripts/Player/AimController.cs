using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private PlayerCommand fire1;
    [SerializeField] private PlayerCommand fire2;

    [SerializeField] private Vector3 mousePosition;
    private MouseScreenController mouseScreenController;
    // Start is called before the first frame update
    void Start()
    {
        mouseScreenController = FindObjectOfType<MouseScreenController>();

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
        /*
        if (Input.GetButtonDown("Fire2"))
        {
            this.fire2?.Execute(this.gameObject);
        }
        */

        mousePosition = mouseScreenController.transform.position;
        Vector3 aimDirection = mousePosition - transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        // Removed rigidbody 2D since it was getting stuck when colliding with smiler
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }
}
