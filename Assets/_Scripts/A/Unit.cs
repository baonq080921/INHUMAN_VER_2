using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    float speed = .1f;
    Vector3[] path;
    int targetIndex;

    Vector3 lastTargetPosition; // To track the last known position of the target
    bool followingPath = false; // To track if the seeker is currently following a path

    void Start()
    {
        lastTargetPosition = target.position; // Initialize the last known position
        RequestNewPath(); // Request the initial path
    }

    void Update()
    {
        // Check if the target's position has changed
        if (target.position != lastTargetPosition)
        {
            lastTargetPosition = target.position; // Update the last known position
            RequestNewPath(); // Request a new path
        }
    }

    void RequestNewPath()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0; // Reset the target index to start from the beginning of the new path

            if (!followingPath)
            {
                StartCoroutine(FollowPath());
            }
        }
    }

    IEnumerator FollowPath()
    {
        followingPath = true;

        if (path.Length == 0)
        {
            followingPath = false;
            yield break;
        }

        Vector3 currentWayPoint = path[0];

        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    followingPath = false;
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed );
            yield return null;
        }
    }
}