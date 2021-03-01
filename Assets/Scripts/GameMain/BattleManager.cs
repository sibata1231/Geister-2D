/**
 * @file BattleManager.cs
 * @brief 
 * @author 
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public enum Participant{
    NONE = 0,
    PLAYER,
    OPPONENT,
}

/**
 * @class BattleManager
 * @brief 
 */
public class BattleManager : SingletonMonoBehaviour<BattleManager> {
    [SerializeField] private Light2D m_playerLight;   //!< プレイヤーライト
    [SerializeField] private Light2D m_opponentLight; //!< 相手側ライト
    private List<Pieces> m_opponentpieces;     //!< 相手側の獲得駒
    private List<Pieces> m_playerpieces;       //!< 自分側の獲得駒

    private Participant m_winner;              //!< 勝利者
    private Participant m_turnParticipant;     //!< ターンの所持者
    private GameManager m_gameManager;         //!< GameManager
    private TurnChangeImage m_turnChangeImage; //!<ターン更新時のアニメーション
    private GameUi m_gameUi;                   //!<GameUi

    public Participant IsWinner {
        get{
            return m_winner;
        }
        set{
            m_winner = value;
            setLight(value);
            if (Participant.PLAYER == m_winner){
                PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win", 0) + 1);
            }
            if (Participant.OPPONENT == m_winner){
                PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose", 0) + 1);
            }
            Debug.Log(PlayerPrefs.GetInt("Win") +":"+ PlayerPrefs.GetInt("Lose"));
        }
    }

    public Participant TurnPhase {
        get {
            return m_turnParticipant;
        }
        set {
            m_turnParticipant = value;
            m_turnChangeImage.TurnChangeAnimation();
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<TurnPieceView>().TurnChangePiece();    
            }
            foreach (GameObject opponent in GameObject.FindGameObjectsWithTag("Opponent"))
            {
                opponent.GetComponent<TurnPieceView>().TurnChangePiece();
            }
            if (m_turnParticipant == Participant.PLAYER)
            {
                m_gameUi.TargetNameAnimation(true);
            }
            else if(m_turnParticipant==Participant.OPPONENT)
            {
                m_gameUi.TargetNameAnimation(false);
            }
        }
    }

    public Participant Goal {
        get {
            return m_winner;
        }
        set {
            IsWinner = value;
            m_gameManager.GameEnd();
        }
    }

    /**
     * @brief インスタンス生成時初期化処理
     * @return なし
     */
    private void Awake() {
        m_gameManager = GameManager.Instance;
        m_turnChangeImage = TurnChangeImage.Instance;
        m_gameUi = GameUi.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_opponentpieces = new List<Pieces>();
        m_playerpieces = new List<Pieces>();
        m_winner = Participant.NONE;
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        
    }
    public void SetTurnInfo(bool isTurn) {
        if (isTurn) {
            m_turnParticipant = Participant.PLAYER;
            m_gameUi.TargetNameAnimation(true);
        } else {
            m_turnParticipant = Participant.OPPONENT;
            m_gameUi.TargetNameAnimation(false);
        }
    }
    /**
     * @brief AddPiece
     * @param[in] piece 
     * @return なし
     */
    public void AddPiece(Pieces piece){
        List<Pieces> result;
        switch(piece){
            case Pieces.PLAYER_EVIL:
                m_playerpieces.Add(piece);
                result = m_playerpieces.Where(value => value == Pieces.PLAYER_EVIL).ToList();
                if (result.Count >= 4) {
                    Debug.Log("playerの勝ち");
                    IsWinner = Participant.PLAYER;
                    m_gameManager.GameEnd();
                }
                break;
            case Pieces.PLAYER_GOOD:
                m_playerpieces.Add(piece);
                result = m_playerpieces.Where(value => value == Pieces.PLAYER_GOOD).ToList();
                if (result.Count >= 4)
                {
                    Debug.Log("playerの負け");
                    IsWinner = Participant.OPPONENT;
                    m_gameManager.GameEnd();
                }
                break;
            case Pieces.OPPONENT_EVIL:
                m_opponentpieces.Add(piece);
                result = m_opponentpieces.Where(value => value == Pieces.OPPONENT_EVIL).ToList();
                if (result.Count >= 4)
                {
                    Debug.Log("playerの負け");
                    IsWinner = Participant.OPPONENT;
                    m_gameManager.GameEnd();
                }

                break;
            case Pieces.OPPONENT_GOOD:
                m_opponentpieces.Add(piece);
                result = m_opponentpieces.Where(value => value == Pieces.OPPONENT_GOOD).ToList();
                if (result.Count >= 4)
                {
                    Debug.Log("playerの勝ち");
                    IsWinner = Participant.PLAYER;
                    m_gameManager.GameEnd();
                }
                break;
        }
    }

    void setLight(Participant participant) {
        if(participant == Participant.PLAYER) {
            float intensity = m_opponentLight.intensity;
            DOTween.To(() => intensity, x => m_opponentLight.intensity = x, 0.0f, 1.0f);
        } else if(participant == Participant.OPPONENT) {
            float intensity = m_playerLight.intensity;
            DOTween.To(() => intensity, x => m_playerLight.intensity = x, 0.0f, 1.0f);
        }
    }
}
