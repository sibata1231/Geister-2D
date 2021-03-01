/**
 * @file TargetPiece.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class TargetPiece
 * @brief 
 */
public class TargetPiece : MonoBehaviour {
    [SerializeField] private Pieces           m_pieces         = default;
    [SerializeField] private SpriteRenderer   m_spriteRenderer = default;
    [SerializeField] private CircleCollider2D m_collider       = default;
    private InitializePiece m_initializePiece;    //!< 初期駒管理
    private Transform       m_transform;          //!< Transform
    private Camera          m_mainCamera;         //!< MainCamera
    private Vector3         m_mousePosition;      //!< マウス座標の保存
    private Vector3         m_initializePosition; //!< 初期座標の保存

    /**
     * @brief 起動時処理
     * @return なし
     */
    void Awake() {
        m_initializePiece = InitializePiece.Instance;
        m_mainCamera      = Camera.main;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_transform = GetComponent<Transform>();     // Transform取得
        m_initializePosition = m_transform.position; // 初期座標の取得
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        
    }

    /**
     * @brief マウス押下処理
     * @return なし
     */
    private void OnMouseDown() {
        m_initializePiece.SetTarget = m_pieces;
    }

    /**
     * @brief マウス押下処理
     * @return なし
     */
    private void OnMouseDrag() {
        m_mousePosition = Input.mousePosition; // マウス座標の取得
        m_mousePosition.z = 10f;               // Z軸修正
        m_transform.position = m_mainCamera.ScreenToWorldPoint(m_mousePosition); // ワールド座標に変換されたマウス座標を代入
    }

    private void OnMouseUp() {
        m_transform.position = m_initializePosition;
    }

    /**
     * @brief リセット処理
     * @return なし
     */
    public void resetTarget() {
        m_collider.enabled          = false;
        m_spriteRenderer.enabled    = false;       // 画像の非表示
        m_initializePiece.SetTarget = Pieces.NONE; // ターゲットのリセット        
    }

    /**
     * @brief リセット処理(activateも含む)
     * @return なし
     */
    public void cancel() {
        if(m_initializePiece.Responsible == Participant.PLAYER && 
          (m_pieces == Pieces.PLAYER_GOOD || m_pieces == Pieces.PLAYER_EVIL)) {
            m_spriteRenderer.enabled = true;
            m_collider.enabled = true;
        } else if (m_initializePiece.Responsible == Participant.OPPONENT &&
                  (m_pieces == Pieces.OPPONENT_GOOD || m_pieces == Pieces.OPPONENT_EVIL)) {
            m_spriteRenderer.enabled = true;
            m_collider.enabled = true;
        } else {
            m_spriteRenderer.enabled = false;
            m_collider.enabled = false;
        }
    }    
}
