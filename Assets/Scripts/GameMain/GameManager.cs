/**
 * @file GameManager.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum GameStatus {
    PREPARATION = 0,
    GAME,
    GAME_END,
}


/**
 * @class GameManager
 * @brief 
 */
public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] private GameInitializeUi m_gameInitializeUi = default;
    [SerializeField] private TextMeshProUGUI  m_playername = default;
    [SerializeField] private ParticleSystem   m_GameEndEffect = default;    

    private GameStatus    m_gameStatus;
    private GameUi        m_gameUi;
    private ResultUi      m_resultUi;
    private BoardManager  m_boardManager;
    private BattleManager m_battleManager;
    private BGMManager    m_bgmManager;
    private SEManager     m_seManager;
    private AnnouncementText announcementText;
    public GameStatus IsGame {
        get { return m_gameStatus; }
        private set { m_gameStatus = value; }
    }    

    /**
     * @brief 起動処理
     * @return なし
     */
    void Awake() {
        m_battleManager = BattleManager.Instance;
        m_boardManager = BoardManager.Instance;
        m_resultUi = ResultUi.Instance;
        m_gameUi = GameUi.Instance;
        m_bgmManager = BGMManager.Instance;
        m_seManager  = SEManager.Instance;
        announcementText = AnnouncementText.Instance;

    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_gameStatus = GameStatus.PREPARATION;
        m_playername.text = PlayerPrefs.GetString("Name");
        m_bgmManager.bgms[2].Play();
        FadeManager.Instance.fadeIn().OnComplete(() => FadeManager.Instance.EndFade = false);
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    public void Initialized(bool isTurn) {
        m_gameInitializeUi.confirmationUnView();
        m_gameStatus = GameStatus.GAME;
        m_battleManager.SetTurnInfo(isTurn);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            player.GetComponent<TurnPieceView>().SetTurnPiece(isTurn);            
        }
        foreach (GameObject opponent in GameObject.FindGameObjectsWithTag("Opponent")) {
            opponent.GetComponent<TurnPieceView>().SetTurnPiece(!isTurn);
        }
        m_gameUi.GameStart();
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update() {
        
    }
    
    /**
     * @brief  ゲーム終了処理
     * @return なし
     */
    public void GameEnd() {
        m_gameUi.GameEnd();
        m_bgmManager.bgms[2].DOFade(0.0f, 1.0f)
                            .OnComplete(() => m_bgmManager.bgms[3].Play());
        m_gameStatus = GameStatus.GAME_END;
        if(m_battleManager.IsWinner == Participant.NONE) {
            m_seManager.notes[1].Play();
            if (m_battleManager.TurnPhase == Participant.OPPONENT) {
                m_battleManager.IsWinner = Participant.PLAYER;
                announcementText.GameEnd(Participant.PLAYER);
                m_resultUi.SetResultView(Participant.PLAYER);
                m_GameEndEffect.transform.position = new Vector2(-2.0f, -2.0f);
            } else if(m_battleManager.TurnPhase == Participant.PLAYER) {
                m_battleManager.IsWinner = Participant.OPPONENT;
                announcementText.GameEnd(Participant.OPPONENT);
                m_resultUi.SetResultView(Participant.OPPONENT);
                m_GameEndEffect.transform.position = new Vector2(10.0f, -2.0f);
            }
        } else {
            announcementText.GameEnd(m_battleManager.IsWinner);
            m_resultUi.SetResultView(m_battleManager.IsWinner);
            if(m_battleManager.IsWinner == Participant.PLAYER) {
                m_GameEndEffect.transform.position = new Vector2(-2.0f, -2.0f);
            } else if(m_battleManager.IsWinner == Participant.OPPONENT) {
                m_GameEndEffect.transform.position = new Vector2(10.0f, -2.0f);
            }
        }
        m_GameEndEffect.Play();
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Player")) {
            piece.GetComponent<TurnPieceView>().SetActivePiece();
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Opponent")) {
            piece.GetComponent<TurnPieceView>().SetActivePiece();
        }
    }

    public void NextTitle() {
        m_seManager.notes[1].Play();
        Tweener tweener = FadeManager.Instance.fadeOut();
        tweener.OnComplete(() => SceneLoader.Instance.sceneChange("Title"));
    }
}
 