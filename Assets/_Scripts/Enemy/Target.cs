using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform target;
    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        agent.destination = target.position;
    }
}
