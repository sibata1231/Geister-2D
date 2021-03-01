/**
 * @file WaitingTime.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * @class WaitingTime
 * @brief 
 */
public class WaitingTime : SingletonMonoBehaviour<WaitingTime> {
    [SerializeField] private TextMeshProUGUI m_waitingTimeText = default; //!< 表示UIText
    [SerializeField] private float           m_seconds;                   //!< 秒数

    private float         m_preSeconds;    //!< 前のUpdateの時の秒数
    private GameManager   m_gameManager;   //!< GameManager
    private BattleManager m_battleManager; //!< BattleManager
    private bool          m_isStop;        //!< 時間フラグ
    /**
     * @brief 起動時処理
     * @return なし
     */
    void Awake() {
        m_gameManager   = GameManager.Instance;
        m_battleManager = BattleManager.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_seconds    = 60.0f;
        m_preSeconds = 0.0f;
        m_isStop     = false;
    }

    /**
     * @brief 固定間隔更新処理
     * @return なし
     */
    void FixedUpdate() {
        if (m_gameManager.IsGame != GameStatus.GAME || m_isStop) { // Game中でない場合
            return;
        }
        m_seconds -= Time.deltaTime;
        if (m_seconds <= 0f) {
            m_seconds += 60;
            if (m_battleManager.TurnPhase == Participant.PLAYER) {
                m_battleManager.TurnPhase = Participant.OPPONENT;
            } else if(m_battleManager.TurnPhase == Participant.OPPONENT) {
                m_battleManager.TurnPhase = Participant.PLAYER;
            }
        }
        //　値が変わった時だけテキストUIを更新
        if ((int)m_seconds != (int)m_preSeconds) {
            m_waitingTimeText.text = ((int)m_seconds).ToString("00") + "秒";
        }
        m_preSeconds = m_seconds;
    }

    public void TurnChangeTime() {
        m_seconds    = 60.0f;
        m_preSeconds = 0.0f;
        m_isStop     = false;
    }

    public void stopTimer() {
        m_isStop = true;
    }
}