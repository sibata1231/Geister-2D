/**
 * @file TargetPiece.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * @class TargetPiece
 * @brief 
 */
public class AcceptancePiece : MonoBehaviour {
    [SerializeField] private SpriteRenderer m_sprite      = default;
    [SerializeField] private Collider2D     m_collider2D  = default;
    [SerializeField] private Participant    m_participant = default;
    private Pieces          m_pieces;
    private InitializePiece m_initializePiece;
    private BoardManager    m_boardManager;
    private SEManager       m_seManager;
    private Tweener m_fadeTweener;

    const float START_ALPHA = 0.5F;
    const float END_ALPHA = 0.7F;

    /**
     * @brief 起動時処理
     * @return なし
     */
    void Awake() {
        m_initializePiece = InitializePiece.Instance;
        m_boardManager    = BoardManager.Instance;
        m_seManager       = SEManager.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_pieces = Pieces.NONE;
        m_fadeTweener = m_sprite.DOFade(START_ALPHA, 1.5f)
                                      .OnComplete(() => {
                                          teweenCheck(START_ALPHA); m_fadeTweener = m_sprite
                          .DOFade(END_ALPHA, 1.5f).SetLoops(-1, LoopType.Yoyo);
                                      });
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update() {

    }
    
    /**
     * @brief 当たり判定処理
     * @return なし
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("InitializePiece") &&    // 相手の駒が配置駒
            m_initializePiece.SetTarget != Pieces.NONE && // ターゲットが登録されている場合
            m_sprite.enabled) {
            m_sprite.enabled = false;                                                           // 画像の非表示 
            m_collider2D.enabled = false;                                                       // コライダーの無効
            m_pieces = m_initializePiece.SetTarget;                                             // 駒の指定
            collision.GetComponent<TargetPiece>().resetTarget();                                // 相手の駒のリセット処理を呼び出す
            Vector2 pos = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);// 駒の座標を計算
            m_boardManager.registerAddPieces(m_pieces, pos);                                    // 駒の登録
            m_initializePiece.addPieces(m_pieces);                                              // 初期駒管理の登録
            m_seManager.notes[0].PlayOneShot(m_seManager.notes[2].clip);                        // 駒のSE再生
        }
    }

    /**
     * @brief キャンセル処理
     * @return なし
     */
    public void cancel() {
        if (m_initializePiece.Responsible == Participant.PLAYER &&
           (m_pieces == Pieces.PLAYER_GOOD || m_pieces == Pieces.PLAYER_EVIL)) {
            m_sprite.enabled = true;
            m_collider2D.enabled = true;
            m_pieces = Pieces.NONE;
        } else if (m_initializePiece.Responsible == Participant.OPPONENT &&
                  (m_pieces == Pieces.OPPONENT_GOOD || m_pieces == Pieces.OPPONENT_EVIL)) {
            m_sprite.enabled = true;
            m_collider2D.enabled = true;
            m_pieces = Pieces.NONE;
        }
    }

    /**
     * @brief 起動処理
     * @return なし
     */
    public void activate() {
        if (m_initializePiece.Responsible == m_participant) {
            m_sprite.enabled = true;
            m_collider2D.enabled = true;
        } else {
            m_sprite.enabled = false;
            m_collider2D.enabled = false;
        }
    }

    void teweenCheck(float alpha){
        if (m_fadeTweener != null)
        {
            m_fadeTweener.Kill();
            m_fadeTweener = null;
            m_sprite.DOFade(alpha, 0.0f);
        }
    }
}
