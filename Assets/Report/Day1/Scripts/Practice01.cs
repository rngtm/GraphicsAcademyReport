using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 演習1. Cubeの作成
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Practice01 : MonoBehaviour
{
    [SerializeField] private Material material = null;

    void Start()
    {
        // +X方向の面 / -X方向の面　の頂点座標
        var verticesX = new List<Vector3>
        {
            // +X面
            new Vector3(1, -1, 1), // 0
            new Vector3(1, 1, 1), // 1
            new Vector3(1, 1, -1), // 2
            new Vector3(1, -1, -1), // 3

            // -X面
            new Vector3(-1, -1, -1), // 0
            new Vector3(-1, 1, -1), // 1
            new Vector3(-1, 1, 1), // 2
            new Vector3(-1, -1, 1), // 3
        };
        
        var vertices = new List<Vector3>();
        // +X面 / -X面の追加
        vertices.AddRange(verticesX);
        // +Y面 / -Y面の追加
        vertices.AddRange(verticesX.Select(v => new Vector3(v.y, v.z, v.x)));
        // +Z面 / -Z面の追加
        vertices.AddRange(verticesX.Select(v => new Vector3(v.z, v.x, v.y)));

        // 1面の頂点を結ぶ順番
        var faceTriangle = new[] {0, 3, 1, 1, 3, 2};

        // 6面ぶんの頂点インデックス追加
        var triangles = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            triangles.AddRange(faceTriangle.Select(t => t + i * 4));
        }
        
        var mesh = new Mesh();
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }
}