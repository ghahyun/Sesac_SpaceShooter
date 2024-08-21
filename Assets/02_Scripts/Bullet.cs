using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot()
    {
        rb.rotation = Quaternion.LookRotation(transform.forward);
        rb.AddRelativeForce(Vector3.forward * 1200.0f);
    }

    void OnCollisionEnter(Collision coll)
    {
        InitItem();
        PoolManager.Instance.bulletPool.Release(this);
    }

    void InitItem()
    {
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        rb.linearVelocity = rb.angularVelocity = Vector3.zero;
    }
}
