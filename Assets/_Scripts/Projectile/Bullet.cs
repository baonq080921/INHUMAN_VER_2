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
        if(Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f){
            trailRenderer.time -= 5f * Time.deltaTime;
        }
        
        // if the bullet fying out of the distance we set, we return it to the pool.
        // This is to prevent the bullet from flying forever in the scene.isFlying
        if(Vector3.Distance(startPosition,transform.position) > flyDistance && bulletDisabled == false){
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;

        }
        if(trailRenderer.time < 0){
            ObjectPool.instance.ReturnBullet(gameObject);
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
        ObjectPool.instance.ReturnBullet(gameObject);
    }

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFx =
                Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newImpactFx, 1f);
        }
    }
}
