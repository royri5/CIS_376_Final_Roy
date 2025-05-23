// Author: Richard Roy
// Date: April 21, 2025
// Source: CIS 376 PerlinTerrain.cs
// Description: Generates terrain using Perlin noise.
//              Also ensures the terrain is navigable
//              and has a texture, normal map and material.
using Unity.AI.Navigation;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 100;
    public int depth = 100;
    public float scale = 10f;
    public float heightMultiplier = 2.5f;
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private MeshCollider meshCollider;
    // navmesh for AI
    private NavMeshSurface navMesh;
    // texture, normal map and material
    public Texture2D texture;
    public Texture2D normalMap;
    public Material mat;

    void Start()
    {
        // create mesh filter, renderer, collider and navmesh
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        navMesh = gameObject.AddComponent<NavMeshSurface>();
        // create mesh and assign material
        meshFilter.mesh = new Mesh();
        mesh = meshFilter.mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.material = mat;
        meshRenderer.material.mainTexture = texture;
        GenerateTerrain();
    }

    // Generate terrain using Perlin noise
    void GenerateTerrain()
    {
        Vector3[] vertices = new Vector3[(width + 1) * (depth + 1)];
        int[] triangles = new int[width * depth * 6];
        Vector2[] uvs = new Vector2[vertices.Length];
        Color[] colors = new Color[vertices.Length];

        // not utilized
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.blue, 0.2f),   // Water
                new GradientColorKey(Color.black, 0.3f),  // Grass
                new GradientColorKey(Color.green, 0.7f), // Sand
                new GradientColorKey(Color.gray, 0.8f),   // Rock
                new GradientColorKey(Color.white, 1.0f)   // Snow
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        // Generate vertices
        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = Mathf.PerlinNoise(x / scale, z / scale) * heightMultiplier;
                vertices[z * (width + 1) + x] = new Vector3(x, y, z);
                uvs[z * (width + 1) + x] = new Vector2((float)x / width, (float)z / depth);
                
                if (y < minHeight) minHeight = y;
                if (y > maxHeight) maxHeight = y;
            }
        }

        // Assign colors based on height
        for (int i = 0; i < vertices.Length; i++)
        {
            float heightNormalized = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
            colors[i] = gradient.Evaluate(heightNormalized);
        }

        // Generate triangles
        int tris = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int vertexIndex = z * (width + 1) + x;
                triangles[tris++] = vertexIndex;
                triangles[tris++] = vertexIndex + width + 1;
                triangles[tris++] = vertexIndex + 1;

                triangles[tris++] = vertexIndex + 1;
                triangles[tris++] = vertexIndex + width + 1;
                triangles[tris++] = vertexIndex + width + 2;
            }
        }
        // Apply to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        // Ensure the collider is updated
        GetComponent<MeshCollider>().sharedMesh = mesh;
        // bake nav mesh
        navMesh.BuildNavMesh();
        // Apply to mesh
        mesh.RecalculateNormals();
    }
}
