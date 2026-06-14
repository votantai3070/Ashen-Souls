using UnityEngine;

public class ItemObject_Base : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 initialPosition;

    protected virtual void Start()
    {
        initialPosition = transform.position;
    }

    protected virtual void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = initialPosition + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatRange);
    }
}
