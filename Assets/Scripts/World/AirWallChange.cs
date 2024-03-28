using Cinemachine;
using UnityEngine;

public class AirWallChange : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D cd;
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;

    [SerializeField] private bool isDefault;
    [SerializeField] private bool isChange;

    private Vector2[] defaultPosition;
    [SerializeField] private Vector2[] positionToChange;

    private void Start()
    {
        defaultPosition = cd.points;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (isChange)
            {
                cd.SetPath(0, positionToChange);
                cinemachineConfiner.InvalidateCache();
            }
            else if (isDefault)
            {
                cd.SetPath(0, defaultPosition);
                cinemachineConfiner.InvalidateCache();
            }
        }
    }
}
