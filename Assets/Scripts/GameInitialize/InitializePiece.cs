/**
 * @file InitializePiece.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class InitializePiece
 * @brief 
 */
public class InitializePiece : SingletonMonoBehaviour<InitializePiece> {
    [SerializeField] private GameInitializeUi m_gameInitializeUi = default;
    private List<Pieces> m_evilPieces;   //!< 駒の配置数
    private List<Pieces> m_goodPieces;   //!< 駒の配置数
    private Pieces       m_target;       //!< Target
    private GameManager  m_gameManager;  //!< GameManager
    private BoardManager m_boardManager; //!< BoardManager
    private Participant  m_responsible;  //!< 配置担当者
    private SEManager    m_seManager;    //!< SEManager
    private AnnouncementText m_announcementText;

    public Pieces SetTarget {
        get { return m_target; }
        set { m_target = value; }
    }

    public Participant Responsible {
        get { return m_responsible; }
        private set { m_responsible = value; }
    }

    private void Awake() {
        m_gameManager = GameManager.Instance;
        m_boardManager = BoardManager.Instance;
        m_seManager = SEManager.Instance;
        m_announcementText = AnnouncementText.Instance;
    }
    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_evilPieces = new List<Pieces>();
        m_goodPieces = new List<Pieces>();
        m_target = Pieces.NONE;
        Responsible = Participant.PLAYER;
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("InitializePiece")) {
            piece.GetComponent<TargetPiece>().cancel();
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("InitializeField")) {
            piece.GetComponent<AcceptancePiece>().activate();
        }
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update() {

    }

    /**
     * @brief 追加処理
     * @return なし
     */
    public void addPieces(Pieces pieces) {
        if(pieces == Pieces.PLAYER_EVIL || pieces == Pieces.OPPONENT_EVIL) {
            m_evilPieces.Add(pieces);
        } else if(pieces == Pieces.PLAYER_GOOD || pieces == Pieces.OPPONENT_GOOD) {
            m_goodPieces.Add(pieces);
        }
        checkedPiece();
    }

    /**
     * @brief チェック処理
     * @return なし
     */
    private void checkedPiece() {
        int current = m_evilPieces.Count + m_goodPieces.Count;
        Debug.Log("現在のカウント : " + current);
        if(current < 8) {
            return;
        }
        if (Responsible == Participant.OPPONENT) {
            m_announcementText.StartGame();
            Responsible = Participant.NONE;
            m_gameInitializeUi.confirmationView();
        } else if(Responsible == Participant.PLAYER) {
            Responsible = Participant.OPPONENT;
            m_announcementText.GameTurn();
            m_evilPieces.Clear();
            m_goodPieces.Clear();
            m_target = Pieces.NONE;
            m_gameInitializeUi.confirmationView();
        }
    }

    public void resetPieces() {
        m_seManager.notes[1].Play();
        m_evilPieces.Clear();
        m_goodPieces.Clear();
        m_boardManager.ClearField();
        m_target = Pieces.NONE;
        if (Responsible == Participant.OPPONENT) {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
                Destroy(player);
            }
            Responsible = Participant.PLAYER;
        } else if(Responsible == Participant.NONE){
            foreach (GameObject opponent in GameObject.FindGameObjectsWithTag("Opponent")) {
                Destroy(opponent);
            }
            Responsible = Participant.OPPONENT;
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("InitializePiece")) {
            piece.GetComponent<TargetPiece>().cancel();
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("InitializeField")) {
            piece.GetComponent<AcceptancePiece>().cancel();
        }
        m_gameInitializeUi.confirmationUnView();
    }

    public void activate() {
        // 起動切り替え処理
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("InitializePiece")) {
            piece.GetComponent<TargetPiece>().cancel();
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("InitializeField")) {
            piece.GetComponent<AcceptancePiece>().activate();
        }

        // ターン交代
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            player.GetComponent<TurnPieceView>().SetTurnPiece(false);
        }
        foreach (GameObject opponent in GameObject.FindGameObjectsWithTag("Opponent")) {
            opponent.GetComponent<TurnPieceView>().SetTurnPiece(true);
        }
    }
}
