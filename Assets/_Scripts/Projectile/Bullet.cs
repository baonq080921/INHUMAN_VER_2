using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactFX;
    private TrailRenderer trailRenderer;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private Vector3 startPosition;
    private float flyDistance;

    private bool bulletDisabled;
    private Rigidbody rb => GetComponent<Rigidbody>();



    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }


    void Update()
    {
        FadeTrailIfNeeded();

        DisableBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 5f * Time.deltaTime;
        }
    }

    private void DisableBulletIfNeeded()
    {
        // if the bullet fying out of the distance we set, we return it to the pool.
        // This is to prevent the bullet from flying forever in the scene.isFlying
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && bulletDisabled == false)
        {
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;

        }
    }

    private void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
        {
            ObjectPool.instance.ReturnToObject(gameObject);
        }
    }

    public void BulletSetUp(float fylingDistance){
        bulletDisabled = false;
        trailRenderer.time = 0.25f;
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
        startPosition = transform.position;
        this.flyDistance = fylingDistance + .5f ;
    }


    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx(collision);
        ReturnBulletToPool();
    }

    private void ReturnBulletToPool() => ObjectPool.instance.ReturnToObject(gameObject);

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactFX);
            newImpactFx.transform.position = contact.point;
            ObjectPool.instance.ReturnToObject(newImpactFx, 1);
        }
    }
}
