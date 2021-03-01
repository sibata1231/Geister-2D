/**
 * @file ResultUi.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/07 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/**
 * @class ResultUi
 * @brief 
 */
public class ResultUi : SingletonMonoBehaviour<ResultUi>
{
    [SerializeField] private Image m_backgroundimage = default;
    [SerializeField] private Image m_playerimage = default;
    [SerializeField] private Image m_opponentimage = default;
    [SerializeField] private Sprite m_winsprite = default;
    [SerializeField] private Sprite m_losesprite = default;
    [SerializeField] private TextMeshProUGUI m_resulttext = default;
    [SerializeField] private GameObject m_confirmationButton = default;

    private SEManager m_seManager;

    private void Awake() {
        m_seManager = SEManager.Instance;
    }
    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start()
    {
        m_backgroundimage.enabled = false;
        m_playerimage.enabled = false;
        m_opponentimage.enabled = false;
        m_resulttext.enabled = false;
        m_confirmationButton.SetActive(false);
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
     * @param[in] winner 
     * @return なし
     */
    public void SetResultView(Participant winner) {
        m_backgroundimage.enabled = true;
        m_playerimage.enabled = true;
        m_opponentimage.enabled = true;
        m_resulttext.enabled = true;
        m_playerimage.DOFade(1.0f, 1.0f);
        m_opponentimage.DOFade(1.0f, 1.0f);
        m_playerimage.transform.DOLocalMoveY(500.0f, 1.0f)
                               .SetEase(Ease.Linear)
                               .SetRelative()
                               .OnComplete(() => {
                                   m_seManager.notes[4].Play();
                                   m_confirmationButton.SetActive(true);
                               });
        m_opponentimage.transform.DOLocalMoveY(-500.0f, 1.0f)
                             .SetEase(Ease.Linear)
                             .SetRelative();
        if (Participant.PLAYER == winner){
            m_playerimage.sprite = m_winsprite;
            m_opponentimage.sprite = m_losesprite;
        }
        else if (Participant.OPPONENT == winner){
            m_playerimage.sprite = m_losesprite;
            m_opponentimage.sprite = m_winsprite;
        }
        m_resulttext.text = ("勝ち" + PlayerPrefs.GetInt("Win") + ":" + "負け" + PlayerPrefs.GetInt("Lose"));
    }
}
