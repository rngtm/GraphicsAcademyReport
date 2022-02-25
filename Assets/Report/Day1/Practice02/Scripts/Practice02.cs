using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 演習2. 球体のメッシュを自作
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Practice02 : MonoBehaviour
{
    [SerializeField] private int divsU = 24; // XZ平面に平行な円の分割数
    [SerializeField] private int divsV = 24; // Y軸方向の円の分割数
    [SerializeField] private Material material = null;

    private void Start()
    {
        var vertices = new Vector3[divsU * divsV];
        var triangles = new List<int>();
        int index = 0;
        for (int vi = 0; vi < divsV; vi++)
        {
            float radianV = vi * Mathf.PI / (divsV - 1);

            float y = -Mathf.Cos(radianV);
            float r = Mathf.Sin(radianV); // 点をXZ平面におろした垂線の足

            for (int ui = 0; ui < divsU; ui++)
            {
                float radianU = ui * Mathf.PI * 2f / (divsU);
                float x = r * Mathf.Cos(radianU);
                float z = r * Mathf.Sin(radianU);

                vertices[index++] = new Vector3(x, y, z);
            }
        }

        for (int vi = 0; vi < divsV - 1; vi++)
        {
            for (int ui = 0; ui < divsU; ui++)
            {
                int t0 = vi * divsU + ui;
                int t1 = vi * divsU + (ui + 1) % divsU;
                int t2 = (vi + 1) * divsU + ui;
                int t3 = (vi + 1) * divsU + (ui + 1) % divsU;
                
                triangles.Add(t0);
                triangles.Add(t2);
                triangles.Add(t1);
                
                triangles.Add(t2);
                triangles.Add(t3);
                triangles.Add(t1);
            }
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