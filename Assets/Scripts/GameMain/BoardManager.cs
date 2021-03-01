/**
 * @file BoardManager.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/02 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public enum Pieces {
    NONE = 0,      //!< 何もなし
    PLAYER_EVIL,   //!< プレイヤー悪いやつ
    PLAYER_GOOD,   //!< プレイヤーいいやつ
    OPPONENT_EVIL, //!< 相手側悪いやつ
    OPPONENT_GOOD, //!< 相手側いいやつ

    PLAYER_GOAL,   //!< ゴール
    OPPONENT_GOAL, //!< ゴール
    WALL,          //!< 壁
    MAX,
}

public enum Direction {
    UP = 0, //!< 上
    DOWN,   //!< 下
    RIGHT,  //!< 右
    LEFT,   //!< 左
}

/**
 * @class BoardManager
 * @brief ボード上の管理
 */
public class BoardManager : SingletonMonoBehaviour<BoardManager> {
    [SerializeField] private GameObject[] m_Pieces = new GameObject[(int)Pieces.MAX]; //!< 駒の種類(5個)
    [SerializeField] private PieceBoard m_opponentPieceBoard = default;
    [SerializeField] private PieceBoard m_playerPieceBoard   = default;
    [SerializeField] private Light2D    m_boardLight         = default;
    [SerializeField] private Light2D    m_goalLight          = default;
    [SerializeField] private GameObject m_turnchangeButton   = default;
    [SerializeField] private float      m_fadeDurationTime   = 1.0f;
    const int HEIGHT = 6; //!< フィールドの高さ
    const int WIDTH  = 8; //!< フィールドの幅

    private Piece[][]      m_fieldData = new Piece[HEIGHT][]; //!< 駒クラス配列
    private Piece          m_Target;                          //!< 現在触っている駒
    private BattleManager  m_battleManager;                   //!< BattleManager
    private WaitingTime    m_waitingTime;                     //!< 待ち時間
    private SEManager      m_seManager;                       //!< SeManager
    private bool           m_isTarget;                        //!< 配置確認
    private AnnouncementText m_announcementText;
    private GameUi         m_gameUi;
    /**
     * @brief 起動時処理
     * @return なし
     */
    void Awake() {
        m_battleManager = BattleManager.Instance;
        m_waitingTime   = WaitingTime.Instance;
        m_seManager     = SEManager.Instance;
        m_gameUi        = GameUi.Instance;
        m_announcementText = AnnouncementText.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        ClearField();

        // ゴールの設定
        registerAddPieces(Pieces.PLAYER_GOAL, new Vector2(0, 0));
        registerAddPieces(Pieces.PLAYER_GOAL, new Vector2(7, 0));
        registerAddPieces(Pieces.OPPONENT_GOAL, new Vector2(0, 5));
        registerAddPieces(Pieces.OPPONENT_GOAL, new Vector2(7, 5));

        // 壁の設定
        registerAddPieces(Pieces.WALL, new Vector2(0, 1));
        registerAddPieces(Pieces.WALL, new Vector2(0, 2));
        registerAddPieces(Pieces.WALL, new Vector2(0, 3));
        registerAddPieces(Pieces.WALL, new Vector2(0, 4));
        registerAddPieces(Pieces.WALL, new Vector2(7, 1));
        registerAddPieces(Pieces.WALL, new Vector2(7, 2));
        registerAddPieces(Pieces.WALL, new Vector2(7, 3));
        registerAddPieces(Pieces.WALL, new Vector2(7, 4));

        m_Target = null;
        m_boardLight.intensity = 1.0f;
        m_goalLight.intensity = 0.0f;
        m_turnchangeButton.SetActive(false);
        m_waitingTime.TurnChangeTime();
        m_isTarget = false;
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update() {
        
    }

    public void ClearField() {
        Piece piece = new Piece();
        // 要素確保
        for (int i = 0; i < HEIGHT; i++) {
            m_fieldData[i] = new Piece[WIDTH];
            for (int j = 0; j < WIDTH; j++) {
                m_fieldData[i][j] = piece;
            }
        }
    }
    
    /**
     * @brief 削除登録処理
     * @param piece (駒)
     * @return なし
     */
    public bool registerRemovePieces(Vector2 pos) {
        Destroy(m_fieldData[(int)pos.x][(int)pos.y].gameObject);
        return true;
    }

    /**
     * @brief 駒登録処理
     * @param piece (駒)
     * @param vector2 (座標)
     * @return なし
     */
    public bool registerAddPieces(Pieces piecetag, Vector2 pos) {
        GameObject gameObject = Instantiate(m_Pieces[(int)piecetag], new Vector3(0.5f + pos.x, 0.5f + pos.y, 0.0f), Quaternion.identity);
        Piece piece    = gameObject.GetComponent<Piece>();
        piece.m_pieces = piecetag;
        piece.m_pos    = pos;
        m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x] = piece;
        return false;
    }

    /**
     * @brief 
     * @param piece (駒)
     * @param 
     * @return なし
     */
    public void setTarget(Piece piece) {
        if (m_isTarget) {
            return;
        }
        bool[] piececheck = new bool[4];
        if (m_Target != null){          // 駒が登録されている状態
            m_Target.setActive(piececheck);// リセット
            m_Target.OffTarget();
        }
        if (piece.m_pos.y < HEIGHT - 1) {
            piececheck[0] = isCheckedPiece(piece, new Vector2(1, 0)); // 上
        }
        if (piece.m_pos.y > 0) {
            piececheck[1] = isCheckedPiece(piece, new Vector2(-1, 0)); // 下
        }
        if (piece.m_pos.x < WIDTH - 1) {
            piececheck[2] = isCheckedPiece(piece, new Vector2(0, 1));　// 左
        }
        if (piece.m_pos.x > 0) {
            piececheck[3] = isCheckedPiece(piece, new Vector2(0, -1)); // 右
        }
        piece.setActive(piececheck);
        piece.OnTarget();
        m_Target = piece;
    }

    /**
     * @brief 移動登録処理
     * @param piece (駒)
     * @param direction (向き)
     * @return なし
     */
    public void registerMove(Piece piece, Direction direction) {
        Piece defPiece = new Piece();
        switch (direction) {
            case Direction.UP:
                if (removeCheck(piece, checkedPiece(piece, new Vector2(1, 0)))) {
                    if(m_battleManager.Goal != Participant.NONE) {
                        goalLight(new Vector2(piece.m_pos.x, piece.m_pos.y + 1));
                    }
                    registerRemovePieces(new Vector2(piece.m_pos.y + 1, piece.m_pos.x)); // 削除
                }
                m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x] = defPiece; // 移動前
                m_fieldData[(int)piece.m_pos.y + 1][(int)piece.m_pos.x] = piece;   // 移動後
                piece.m_pos.y += 1;
                break;
            case Direction.DOWN:
                if (removeCheck(piece, checkedPiece(piece, new Vector2(-1, 0)))) {
                    if (m_battleManager.Goal != Participant.NONE) {
                        goalLight(new Vector2(piece.m_pos.x, piece.m_pos.y - 1));
                    }
                    registerRemovePieces(new Vector2(piece.m_pos.y - 1, piece.m_pos.x));
                }
                m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x] = defPiece;         // 移動前
                m_fieldData[(int)piece.m_pos.y - 1][(int)piece.m_pos.x] = piece;  // 移動後
                piece.m_pos.y -= 1;
                break;
            case Direction.RIGHT:
                if(removeCheck(piece, checkedPiece(piece, new Vector2(0, 1)))) {
                    if (m_battleManager.Goal != Participant.NONE) {
                        goalLight(new Vector2(piece.m_pos.x + 1, piece.m_pos.y));
                    }
                    registerRemovePieces(new Vector2(piece.m_pos.y, piece.m_pos.x + 1));
                }
                m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x] = defPiece;         // 移動前
                m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x + 1] = piece;  // 移動後
                piece.m_pos.x += 1;
                break;
            case Direction.LEFT:
                if(removeCheck(piece, checkedPiece(piece, new Vector2(0, -1)))) {
                    if (m_battleManager.Goal != Participant.NONE) {
                        goalLight(new Vector2(piece.m_pos.x - 1, piece.m_pos.y));
                    }
                    registerRemovePieces(new Vector2(piece.m_pos.y,piece.m_pos.x - 1));
                }
                m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x] = defPiece;         // 移動前
                m_fieldData[(int)piece.m_pos.y][(int)piece.m_pos.x - 1] = piece;  // 移動後
                piece.m_pos.x -= 1;
                break;
        }
        m_isTarget = true;
    }

    private bool isCheckedPiece(Piece piece, Vector2 addPos) {
        Pieces nextPiece = m_fieldData[(int)(piece.m_pos.y + addPos.x)][(int)(piece.m_pos.x + addPos.y)].m_pieces;
        if(nextPiece == Pieces.WALL) {
            return false;
        }
        // 駒の情報が相手側側
        if (piece.m_pieces == Pieces.OPPONENT_EVIL || piece.m_pieces == Pieces.OPPONENT_GOOD) {
            if(piece.m_pieces == Pieces.OPPONENT_GOOD && nextPiece == Pieces.PLAYER_GOAL) {
                return true;
            }
            if (nextPiece == Pieces.NONE ||
                nextPiece == Pieces.PLAYER_EVIL ||
                nextPiece == Pieces.PLAYER_GOOD) {
                return true;
            }
        // 駒の情報が自分側
        } else if (piece.m_pieces == Pieces.PLAYER_EVIL || piece.m_pieces == Pieces.PLAYER_GOOD) {
            if (piece.m_pieces == Pieces.PLAYER_GOOD && nextPiece == Pieces.OPPONENT_GOAL) {
                return true;
            }
            if (nextPiece == Pieces.NONE ||
                nextPiece == Pieces.OPPONENT_EVIL ||
                nextPiece == Pieces.OPPONENT_GOOD) {
                return true;
            }
        }
        return false;
    }

    private Pieces checkedPiece(Piece piece, Vector2 addPos) {
        Pieces nextPiece = m_fieldData[(int)(piece.m_pos.y + addPos.x)][(int)(piece.m_pos.x + addPos.y)].m_pieces;
        if (nextPiece == Pieces.WALL) {
            return Pieces.WALL;
        }
        if (piece.m_pieces == Pieces.OPPONENT_EVIL || piece.m_pieces == Pieces.OPPONENT_GOOD) {
            if (piece.m_pieces == Pieces.OPPONENT_GOOD && nextPiece == Pieces.PLAYER_GOAL) {
                return nextPiece;
            }
            if (nextPiece == Pieces.NONE ||
                nextPiece == Pieces.PLAYER_EVIL ||
                nextPiece == Pieces.PLAYER_GOOD) {
                return nextPiece;
            }
        } else if (piece.m_pieces == Pieces.PLAYER_EVIL || piece.m_pieces == Pieces.PLAYER_GOOD) {
            if (piece.m_pieces == Pieces.PLAYER_GOOD && nextPiece == Pieces.OPPONENT_GOAL) {
                return nextPiece;
            }
            if (nextPiece == Pieces.NONE ||
                nextPiece == Pieces.OPPONENT_EVIL ||
                nextPiece == Pieces.OPPONENT_GOOD) {
                return nextPiece;
            }
        }
        return Pieces.NONE;
    }

    private bool removeCheck(Piece piece, Pieces nextPiece) {
        // プレイヤー側
        if (Pieces.PLAYER_EVIL == piece.m_pieces || Pieces.PLAYER_GOOD   == piece.m_pieces) {
            if((Pieces.OPPONENT_EVIL == nextPiece || Pieces.OPPONENT_GOOD == nextPiece)) {
                m_playerPieceBoard.setPieceView(nextPiece); // 取得した駒の表示登録
                m_battleManager.AddPiece(nextPiece);        // 駒の取得を登録
                return true;
            } else if(Pieces.OPPONENT_GOAL == nextPiece){
                m_battleManager.Goal = Participant.PLAYER;
                return true;
            }
        }
        // 敵側
        if (Pieces.OPPONENT_EVIL == piece.m_pieces || Pieces.OPPONENT_GOOD == piece.m_pieces) {
            if(Pieces.PLAYER_EVIL == nextPiece || Pieces.PLAYER_GOOD == nextPiece) {
                m_opponentPieceBoard.setPieceView(nextPiece);
                m_battleManager.AddPiece(nextPiece);
                return true;
            } else if (Pieces.PLAYER_GOAL == nextPiece) {
                m_battleManager.Goal = Participant.OPPONENT;
                return true;
            }
        }
        return false;
    }

    // ターン確認時
    public void setOnLight() {
        if (m_battleManager.TurnPhase == Participant.PLAYER) {
            m_announcementText.GameTurnPlayer();
        } else if (m_battleManager.TurnPhase == Participant.OPPONENT) {
            m_announcementText.GameTurnOpponent();
        }
        m_seManager.notes[0].PlayOneShot(m_seManager.notes[1].clip);
        float intensity = m_boardLight.intensity;
        m_turnchangeButton.SetActive(false);
        DOTween.To(() => intensity, x => m_boardLight.intensity = x, 1.0f, m_fadeDurationTime)
               .OnComplete(() => {
                   m_waitingTime.TurnChangeTime();
                   m_gameUi.setActiveSurrenderButton(true);
                   m_isTarget = false;
               });
    }

    // 駒の配置時
    public void setOffLight() {
        m_gameUi.setActiveSurrenderButton(false);
        m_waitingTime.stopTimer();
        m_announcementText.GameTurn();
        if (m_battleManager.IsWinner != Participant.NONE) {
            return;
        }
        float intensity = m_boardLight.intensity;
        DOTween.To(() => intensity, x => m_boardLight.intensity = x, 0.0f, m_fadeDurationTime)
               .OnComplete(() => {
                   m_turnchangeButton.SetActive(true);
                   m_Target.OffTarget();
                   turnChange();
               });
    }

    private void turnChange() {
        if (m_battleManager.TurnPhase == Participant.PLAYER) {
            m_battleManager.TurnPhase = Participant.OPPONENT;

        } else if (m_battleManager.TurnPhase == Participant.OPPONENT) {
            m_battleManager.TurnPhase = Participant.PLAYER;
        }
    }

    private void goalLight(Vector2 pos) {
        m_goalLight.transform.DOMove(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f), 0.0f);
        float intensity = m_goalLight.intensity;
        DOTween.To(() => intensity, x => m_goalLight.intensity = x, 2.0f, 1.0f);
    }
}
