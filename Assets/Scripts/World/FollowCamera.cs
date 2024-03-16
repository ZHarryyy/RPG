using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private float maxXPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (cameraTransform.position.x >= maxXPosition) return;

        Vector3 currentPosition = this.transform.position;

        currentPosition.x = cameraTransform.position.x;

        this.transform.position = currentPosition;
    }
}
