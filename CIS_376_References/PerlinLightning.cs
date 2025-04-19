using UnityEngine;

public class PerlinLightning : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int points = 10; // Number of segments
    public float noiseScale = 0.1f;
    public float lightningWidth = 0.1f;
    public float startX = 0f;
    public float endX = 5f;
    public float height = 5f;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = points;
        lineRenderer.startWidth = lightningWidth;
        lineRenderer.endWidth = lightningWidth / 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

    }

    void Update()
    {
        Vector3[] positions = new Vector3[points];
        float xStep = (endX - startX) / (points - 1);

        for (int i = 0; i < points; i++)
        {
            float x = startX + i * xStep;
            float y = Mathf.PerlinNoise(i * noiseScale, Time.time * 100.0f) * height; // Perlin noise for randomness
            positions[i] = new Vector3(y, 10-x, 0);
        }

        lineRenderer.SetPositions(positions);
    }
}
