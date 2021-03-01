using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum AnnouncementState {
    NONE,
    START_PLAYER,   // 初期
    START_OPPONENT, // 初期
    START_GAME,     // 初期

    GAME_PLAYER,    // ゲーム中
    GAME_OPPONENT,  // ゲーム中
    GAME_TURN,      // ゲーム中

    RESULT,         // ゲーム終了
}

public class AnnouncementText : SingletonMonoBehaviour<AnnouncementText> {
    [SerializeField] private TextMeshProUGUI m_announcementText = default;
    private AnnouncementState m_announcementState = AnnouncementState.NONE;
    const float MOVE_X = 800.0F;

    void Start() {
        m_announcementState = AnnouncementState.NONE;
        StartTurnPlayer();
    }

    public void StartTurnPlayer() {
        NextAnnounce().OnComplete(() => {
            m_announcementText.transform.localPosition = new Vector2(MOVE_X,0.0f);
            m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
            m_announcementText.text = PlayerPrefs.GetString("Name") + "さん駒の配置をしてください";
        });

    }

    public void StartTurnOpponent() {
        NextAnnounce().OnComplete(() => {
            m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
            m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
            m_announcementText.text = PlayerPrefs.GetString("Opponent","相手") + "さん駒の配置をしてください";
        });
    }

    public void StartGame() {
        NextAnnounce().OnComplete(() => {
            m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
            m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
            m_announcementText.text = "ガイスターを開始します";
        });
    }

    public void GameTurnPlayer() {
        NextAnnounce().OnComplete(() => {
            m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
            m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
            m_announcementText.text = PlayerPrefs.GetString("Name") + "さん駒を移動してください";
        });

    }

    public void GameTurnOpponent() {
        NextAnnounce().OnComplete(() => {
            m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
            m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
            m_announcementText.text = PlayerPrefs.GetString("Opponent", "相手") + "さん駒を移動してください";
        });
    }

    public void GameTurn() {
        NextAnnounce().OnComplete(() => {
            m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
            m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
            m_announcementText.text = "交代してください";
        });
    }

    public void GameEnd(Participant participant) {
        if (Participant.PLAYER == participant) {
            NextAnnounce().OnComplete(() => {
                m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
                m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
                m_announcementText.text = PlayerPrefs.GetString("Name") + "さんの勝利です";
            });
        } else if (Participant.OPPONENT == participant) {
            NextAnnounce().OnComplete(() => {
                m_announcementText.transform.localPosition = new Vector2(MOVE_X, 0.0f);
                m_announcementText.transform.DOLocalMoveX(0.0f, 0.5f);
                m_announcementText.text = PlayerPrefs.GetString("Opponent", "相手") + "さんの勝利です";
            });
        }

    }

    private Tweener NextAnnounce() {
        return m_announcementText.transform.DOLocalMoveX(-MOVE_X, 0.5f);                                           
    }
}
