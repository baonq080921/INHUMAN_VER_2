using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance; // Singleton instance of the CameraManager
    private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera
    private CinemachineFramingTransposer transposer; // Reference to the Cinemachine framing transposer
    private float targetCameraDistance; // target camera distance
    public float zoomRate; // Rate of zooming in/out
    [SerializeField] bool canChangeCameraDistance;
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject); // Destroy duplicate instance  if it exists
        }

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>(); // Get the virtual camera component
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>(); // Get the framing transposer component
    }
    void Update()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistance()
    {
        if(canChangeCameraDistance == false){
            float currentCameraDistance = transposer.m_CameraDistance; // Get the current camera distance   

            if (Mathf.Abs(targetCameraDistance - currentCameraDistance) <.01f)  return;

            transposer.m_CameraDistance =
                Mathf.Lerp(currentCameraDistance, targetCameraDistance, zoomRate); // Smoothly interpolate the camera distance towards the target distance
        }
    }

    public void ChangeCameraDistance(float distance)
    {
        targetCameraDistance = distance; // Change the camera distance
    }



}
