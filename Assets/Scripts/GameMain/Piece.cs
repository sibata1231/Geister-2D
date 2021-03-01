/**
 * @file Piece.cs
 * @brief 
 * @author 
 * @date 2020/10/03 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * @class Piece
 * @brief 
 */
public class Piece : MonoBehaviour {
    [SerializeField] private GameObject[]   m_area = new GameObject[4]; //!< 移動先のマークオブジェクト
    [SerializeField] private ParticleSystem m_particleSystem = default; //!< 駒配置時のエフェクト
    [SerializeField] private SpriteRenderer m_sprite         = default; //!< SpriteRenderer
    public Pieces  m_pieces; //!< 識別(駒)
    public Vector2 m_pos;    //!< 駒上の座標

    private Transform      m_transform;      //!< Transform
    private BoardManager   m_boardmanager;   //!< BoardManager
    private BattleManager  m_battleManager;  //!< BattleManager
    private GameManager    m_gameManager;    //!< GameManager
    private SEManager      m_seManager;      //!< SEManager


    /**
     * @brief インスタンス生成時初期化処理
     * @return なし
     */
    void Awake() {
        m_boardmanager  = BoardManager.Instance;
        m_battleManager = BattleManager.Instance;
        m_gameManager   = GameManager.Instance;
        m_seManager     = SEManager.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start()
    {
        m_transform = GetComponent<Transform>();        
    }

    /**
     * @brief 
     * @param[in] direction 
     * @return なし
     */
    public void SetMove(Direction direction) {
        //m_particleSystem.Play();
        m_seManager.notes[0].PlayOneShot(m_seManager.notes[2].clip);
        m_boardmanager.registerMove(this, direction);
        switch (direction){
                case Direction.UP:
                    m_transform.DOLocalMoveY(1.0f, 0.5f)
                               .SetRelative().SetEase(Ease.Linear);
                    break;
                case Direction.DOWN:
                    m_transform.DOLocalMoveY(-1.0f, 0.5f)
                               .SetRelative().SetEase(Ease.Linear);
                break;
                case Direction.RIGHT:
                    m_transform.DOLocalMoveX(1.0f, 0.5f)
                               .SetRelative().SetEase(Ease.Linear);
                break;
                case Direction.LEFT:
                    m_transform.DOLocalMoveX(-1.0f, 0.5f)
                               .SetRelative().SetEase(Ease.Linear);
                break;
        }
        m_boardmanager.setOffLight();
        setActive(new bool[4]);
    }

    /**
     * @brief マウスターゲットが押された瞬間処理
     * @return なし
     */
    private void OnMouseDown() {
        if(m_gameManager.IsGame != GameStatus.GAME) {
            return;
        }
        if (m_battleManager.TurnPhase == Participant.PLAYER &&
            (m_pieces == Pieces.PLAYER_EVIL || m_pieces == Pieces.PLAYER_GOOD )) {
            m_boardmanager.setTarget(this);
        } else if (m_battleManager.TurnPhase == Participant.OPPONENT
            && (m_pieces == Pieces.OPPONENT_EVIL || m_pieces == Pieces.OPPONENT_GOOD )) {
            m_boardmanager.setTarget(this);
        }
        Debug.Log(m_pos);
    }

    /**
     * @brief     ターゲットを表示非表示する処理
     * @param[in] check 
     * @return 
     */
    public void setActive(bool[] check) { //上下左右
        for (int i = 0; i < m_area.Length; i++) {
            m_area[i].GetComponent<SpriteRenderer>().enabled   = check[i];
            m_area[i].GetComponent<CircleCollider2D>().enabled = check[i];
        }
    }

    public void OnTarget() {
        m_sprite.DOColor(Color.white, 0.5f);
    }
    public void OffTarget() {
        m_sprite.DOColor(Color.gray, 0.5f);
    }
}
