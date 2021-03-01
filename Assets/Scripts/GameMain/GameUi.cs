/**
 * @file GameUi.cs
 * @brief 
 * @author 
 * @date 2020/10/07 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/**
 * @class GameUi
 * @brief 
 */
public class GameUi : SingletonMonoBehaviour<GameUi> {
    [SerializeField] private TextMeshProUGUI m_playernameText   = default;
    [SerializeField] private TextMeshProUGUI m_opponentnameText = default;
    [SerializeField] private GameObject      m_surrenderButton  = default;
    [SerializeField] private TextMeshProUGUI m_announceText     = default;
    
    private Tweener m_playerFadetweener;
    private Tweener m_opponentFadetweener;
    private AnnouncementText m_announcementText;
    private BattleManager m_battleManager;
    const float START_ALPHA = 0.5F;
    const float END_ALPHA   = 1.0F;

    private void Awake() {
        m_announcementText = AnnouncementText.Instance;
        m_battleManager = BattleManager.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start()
    {
        m_surrenderButton.SetActive(false);
        m_announceText.enabled = true;
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {

    }

    /**
     * @brief 
     * @param[in] isTurn 
     * @return なし
     */
    public void TargetNameAnimation(bool isTurn) {
        teweenCheck(END_ALPHA);
        if (isTurn){
            m_playerFadetweener = m_playernameText.DOFade(START_ALPHA, 1.0f)
                                                  .OnComplete(() => {
                                                      teweenCheck(START_ALPHA);
                                                      m_playerFadetweener = m_playernameText.DOFade(END_ALPHA, 1.0f)
                                                                                      .SetLoops(-1, LoopType.Yoyo);
                                                  });
        }
        else{

            m_opponentFadetweener = m_opponentnameText.DOFade(START_ALPHA, 1.0f)
                                                      .OnComplete(() => {
                                                          teweenCheck(START_ALPHA);
                                                          m_opponentFadetweener = m_opponentnameText.DOFade(END_ALPHA, 1.0f)
                                                                                                .SetLoops(-1, LoopType.Yoyo);
                                                      });
        }
    }

    /**
     * @brief 
     * @param[in] alpha 
     * @return なし
     */
    void teweenCheck(float alpha) {
        if (m_playerFadetweener != null) {
            m_playerFadetweener.Kill();
            m_playerFadetweener = null;
            m_playernameText.alpha = alpha;
        }
        if(m_opponentFadetweener != null) {
            m_opponentFadetweener.Kill();
            m_opponentFadetweener = null;
            m_opponentnameText.alpha = alpha;

        }
    }

    public void GameStart() {
        setActiveSurrenderButton(true);
        if (m_battleManager.TurnPhase == Participant.PLAYER) {
            m_announcementText.GameTurnPlayer();
        } else if (m_battleManager.TurnPhase == Participant.OPPONENT) {
            m_announcementText.GameTurnOpponent();
        }

    }
    
    public void GameEnd() {
        teweenCheck(1.0f);
    }

    public void setActiveSurrenderButton(bool isActive) {
        m_surrenderButton.SetActive(isActive);
    }
}
