using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileAngle = 45;
    protected Rigidbody Rigidbody;
    public Unit Owner;
    private float Damage;
    private float TimeAlive = 0;

    public MeshRenderer[] PrimaryMeshRenderers;
    public MeshRenderer[] SecondaryMeshRenderers;

    public virtual void Init(Unit InOwner, float InDamage)
    {
        Owner = InOwner;
        Damage = InDamage;
        ColorManager.Instance.SetProjectileColors(this);
    }
    public virtual void Launch(Vector3 LaunchPosition, Vector3 Destination)
    {
        Rigidbody = GetComponent<Rigidbody>();
        gameObject.transform.position = LaunchPosition;
        Rigidbody.velocity = BallisticVelocity(Destination, ProjectileAngle);
    }

    public Vector3 BallisticVelocity(Vector3 Destination, float Angle)
    {
        Vector3 dir = Destination - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = Angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }
    private void Update()
    {
        TimeAlive += Time.deltaTime;
        if (TimeAlive > 5)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Targetable TargetHit = other.gameObject.GetComponent<Targetable>();
        if (TargetHit != null)
        {
            if (TargetHit.Allegiance != Owner.Allegiance)
            {
                TargetHit.TakeDamage(Damage, 
                    (bool bKilled)=>
                    {
                        if (bKilled)
                        {
                            Debug.Log("Projectile killed");
                        }
                       
                    });
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        Projectile OtherProj = collision.gameObject.GetComponent<Projectile>();
        if (OtherProj != null)
        {
            Destroy(gameObject);
            Destroy(OtherProj.gameObject);
        }
    }
}
