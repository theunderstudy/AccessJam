using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : Projectile
{
    public float LaunchSpeed;
    public float MoveSpeed;
    public override void Launch(Vector3 LaunchPosition, Vector3 Destination)
    {
        gameObject.transform.position = LaunchPosition;
        Rigidbody = GetComponent<Rigidbody>();
        transform.LookAt(Destination);
        Rigidbody.AddForce(transform.forward * LaunchSpeed,ForceMode.Impulse);
    }

    private void Update()
    {
        Rigidbody.AddForce(transform.forward * MoveSpeed);
    }
}
