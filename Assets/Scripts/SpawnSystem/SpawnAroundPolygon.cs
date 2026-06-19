using UnityEngine;

public class SpawnAroundPolygon : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private Transform parent;

    [ContextMenu("Setup Around Polygon")]
    public void SetupAroundPolygon()
    {
        if (polygonCollider == null || prefab == null)
            return;

        for (int p = 0; p < polygonCollider.pathCount; p++)
        {
            Vector2[] points = polygonCollider.GetPath(p);

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 a = polygonCollider.transform.TransformPoint(points[i]);
                Vector2 b = polygonCollider.transform.TransformPoint(points[(i + 1) % points.Length]);

                float distance = Vector2.Distance(a, b);
                int count = Mathf.Max(1, Mathf.FloorToInt(distance / spacing));

                for (int j = 0; j < count; j++)
                {
                    float t = (float)j / count;
                    Vector2 pos = Vector2.Lerp(a, b, t);

                    //Vector2 dir = (b - a).normalized;
                    //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    Instantiate(prefab, pos, Quaternion.identity, parent);
                }
            }
        }
    }
}