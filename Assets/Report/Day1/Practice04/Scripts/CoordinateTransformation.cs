using System;
using UnityEngine;

/// <summary>
/// 演習4. 正規化ビューボリューム
/// </summary>
public class CoordinateTransformation : MonoBehaviour
{
    [SerializeField] private Camera _camera; // カメラ
    [SerializeField, Range(0f, 1f)] private float _weight = 0f;　// 可視化のウェイト
    [SerializeField] private VisualizeType _type = VisualizeType.DivideW; // 可視化タイプ
    private Mesh _mesh;
    private Vector3[] _vertices;
    
    /// <summary>
    /// 可視化タイプ
    /// </summary>
    private enum VisualizeType
    {
        VP, // VP変換の可視化
        DivideW, // W除算の可視化
    }

    private void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _vertices = new Vector3[8];

        // メッシュを初期化
        var triangles = new int[]
        {
            0, 2, 1,
            1, 2, 3,
            1, 3, 5,
            7, 5, 3,
            3, 2, 7,
            6, 7, 2,
            2, 0, 6,
            4, 6, 0,
            0, 1, 4,
            5, 4, 1,
            4, 7, 6,
            5, 7, 4
        };
        
        var colors = new Color[]
        {
            new Color(0.0f, 0.0f, 0.0f),
            new Color(1.0f, 0.0f, 0.0f),
            new Color(0.0f, 1.0f, 0.0f),
            new Color(1.0f, 1.0f, 0.0f),
            new Color(0.0f, 0.0f, 1.0f),
            new Color(1.0f, 0.0f, 1.0f),
            new Color(0.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
        };
        _mesh.vertices = _vertices;
        _mesh.triangles = triangles;
        _mesh.colors = colors;
        UpdateVertices();
    }

    private void Update()
    {
        UpdateVertices();
    }

    /// <summary>
    /// 頂点をカメラの視錐台に合わせたものに更新する
    /// </summary>
    private void UpdateVertices()
    {
        var near = _camera.nearClipPlane;
        var far = _camera.farClipPlane;

        // 視錐台の大きさの求め方は下記を参考
        // https://docs.unity3d.com/jp/current/Manual/FrustumSizeAtDistance.html
        var nearFrustumHeight = 2.0f * near * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var nearFrustumWidth = nearFrustumHeight * _camera.aspect;
        var farFrustumHeight = 2.0f * far * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var farFrustomWidth = farFrustumHeight * _camera.aspect;

        var forward = _camera.transform.forward;
        var right = _camera.transform.right;
        var up = Vector3.Cross(right, forward).normalized;
        Vector3 getVertex(float x, float y, float z) => right * x - up * y + z * forward + _camera.transform.position;

        // 視錘台の頂点
        _vertices[0] = getVertex(nearFrustumWidth * -0.5f, nearFrustumHeight * -0.5f, near);
        _vertices[1] = getVertex(nearFrustumWidth * 0.5f, nearFrustumHeight * -0.5f, near);

        _vertices[2] = getVertex(nearFrustumWidth * -0.5f, nearFrustumHeight * 0.5f, near);
        _vertices[3] = getVertex(nearFrustumWidth * 0.5f, nearFrustumHeight * 0.5f, near);

        _vertices[4] = getVertex(farFrustomWidth * -0.5f, farFrustumHeight * -0.5f, far);
        _vertices[5] = getVertex(farFrustomWidth * 0.5f, farFrustumHeight * -0.5f, far);

        _vertices[6] = getVertex(farFrustomWidth * -0.5f, farFrustumHeight * 0.5f, far);
        _vertices[7] = getVertex(farFrustomWidth * 0.5f, farFrustumHeight * 0.5f, far);

        // VP行列を適用する
        for (int i = 0; i < _vertices.Length; i++)
        {
            // 検証のため頂点情報を4次元に
            var vertex = new Vector4(_vertices[i].x, _vertices[i].y, _vertices[i].z, 1);
            // VP行列を作成
            var vpMatrix = _camera.projectionMatrix * _camera.worldToCameraMatrix;
            // VP行列を適用
            var vertex2 = vpMatrix * vertex;
            // W除算
            var vertex3 = vertex2 / vertex2.w;

            // 頂点変換の可視化
            switch (_type)
            {
                case VisualizeType.VP:
                    _vertices[i] = Vector3.Lerp(vertex, vertex2, _weight);
                    break;
                case VisualizeType.DivideW:
                    _vertices[i] = Vector3.Lerp(vertex2, vertex3, _weight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        _mesh.vertices = _vertices;
        _mesh.RecalculateBounds();
    }

}