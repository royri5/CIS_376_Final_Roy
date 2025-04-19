using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public int segments = 10;
    public float randomness = 0.5f;
    public float duration = 0.1f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        InvokeRepeating(nameof(GenerateLightning), 0, duration);
    }

    void GenerateLightning()
    {
        Vector3[] positions = new Vector3[segments + 1];
        positions[0] = startPoint.position;
        positions[segments] = endPoint.position;

        for (int i = 1; i < segments; i++)
        {
            float t = (float)i / segments;
            Vector3 point = Vector3.Lerp(startPoint.position, endPoint.position, t);
            point.x += Random.Range(-randomness, randomness);
            point.y += Random.Range(-randomness, randomness);
            positions[i] = point;
        }

        lineRenderer.SetPositions(positions);
    }
}
