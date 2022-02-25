using UnityEngine;

/// <summary>
/// 演習3. カメラ座標系の確認
/// </summary>
public class Practice03 : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    void Start()
    {
        // ローカル座標
        var vertex = new Vector4(2, 0, 1, 1);
        Debug.Log("v\n" + vertex);

        // モデル空間 -> ワールド空間　の変換を行う
        var mMatrix = transform.localToWorldMatrix;
        Debug.Log("M * v\n" + mMatrix * vertex);
        
        // ワールド空間 -> ビュー空間
        var vMatrix = _camera.worldToCameraMatrix;

        // モデル空間 -> ワールド空間 -> ビュー空間　の変換を行う
        var vp = vMatrix * mMatrix;
        var targetPos = vp * vertex;
        targetPos.z *= -1;
        Debug.Log("VP * v\n" + targetPos);
    }
}
