/**
 * @file GameInitializeUi.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/06 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/**
 * @class GameInitializeUi
 * @brief 
 */
public class GameInitializeUi : MonoBehaviour {
    [SerializeField] private Image           m_backGroundImage    = default;
    [SerializeField] private Image           m_coinImage          = default;
    [SerializeField] private GameObject      m_decisionButton     = default;
    [SerializeField] private GameObject      m_cancelButton       = default;
    [SerializeField] private Sprite          m_firstSprite        = default;
    [SerializeField] private Sprite          m_secondSprite       = default;
    [SerializeField] private TextMeshProUGUI m_turnPlayerText     = default;
    [SerializeField] private TextMeshProUGUI m_turnOpponentText   = default;
    [SerializeField] private Material        m_firstMaterial      = default;
    [SerializeField] private Material        m_secondMaterial     = default;
    [SerializeField] private GameObject      m_turnDecisionButton = default;

    private Tweener     m_rotateTwenner;
    private GameManager m_gameManager;
    private InitializePiece m_initializePiece;
    private SEManager   m_seManager;
    private AnnouncementText m_announcementText;
    private bool        m_isTurn;
    

    private void Awake() {
        m_gameManager = GameManager.Instance;
        m_seManager = SEManager.Instance;
        m_initializePiece = InitializePiece.Instance;
        m_announcementText = AnnouncementText.Instance;
    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        confirmationUnView();
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        
    }

    /**
     * @brief 確認UIの起動
     * @return なし
     */
    public void confirmationView() {
        m_backGroundImage.enabled = true;
        m_decisionButton.SetActive(true);
        m_cancelButton.SetActive(true);
    }

    /**
     * @brief 確認UIの起動
     * @return なし
     */
    public void confirmationUnView() {
        m_backGroundImage.enabled  = false;
        m_coinImage.enabled        = false;
        m_turnPlayerText.DOFade(0.0f, 0.5f).OnComplete( () => m_turnPlayerText.enabled = false);
        m_turnOpponentText.DOFade(0.0f, 0.5f).OnComplete( () => m_turnOpponentText.enabled = false);
        m_decisionButton.SetActive(false);
        m_cancelButton.SetActive(false);
        m_turnDecisionButton.SetActive(false);
    }

    /**
     * @brief 確認UIの起動
     * @return なし
     */
    public void cointossAnimation() {
        m_initializePiece.activate();
        if (m_initializePiece.Responsible == Participant.OPPONENT) {
            m_announcementText.GameTurnOpponent();
            confirmationUnView();
            m_seManager.notes[1].Play();
            return;
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Player")) {
            piece.GetComponent<TurnPieceView>().SetInActivePiece();
        }
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Opponent")) {
            piece.GetComponent<TurnPieceView>().SetInActivePiece();
        }
        m_coinImage.enabled = true;
        m_isTurn = ((int)(Random.Range(0.0f, 10.0f)) % 2 == 0) ? true : false;
        m_decisionButton.SetActive(false);
        m_cancelButton.SetActive(false);
        m_seManager.notes[3].Play();
        m_coinImage.transform.DOLocalJump(new Vector3(0.0f, -300.0f, 0.0f), 600.0f, 1, 1.0f)
                             .SetRelative()
                             .OnComplete(()=> {
                                 m_rotateTwenner.Kill();
                                 m_rotateTwenner = null;
                                 m_turnPlayerText.enabled = true;
                                 m_turnOpponentText.enabled = true;
                                 if (m_isTurn) {
                                     m_coinImage.sprite = m_firstSprite;
                                     m_turnPlayerText.text = "先攻";
                                     m_turnPlayerText.fontMaterial = m_firstMaterial;
                                     m_turnOpponentText.text = "後攻";
                                     m_turnOpponentText.fontMaterial = m_secondMaterial;
                                 } else {
                                     m_coinImage.sprite = m_secondSprite;
                                     m_turnPlayerText.text = "後攻";
                                     m_turnPlayerText.fontMaterial = m_secondMaterial;
                                     m_turnOpponentText.text = "先攻";
                                     m_turnOpponentText.fontMaterial = m_firstMaterial;
                                 }
                                 m_turnPlayerText.DOFade(1.0f, 0.5f);
                                 m_turnOpponentText.DOFade(1.0f, 0.5f);
                                 m_turnDecisionButton.SetActive(true);
                             });
        m_rotateTwenner = m_coinImage.transform.DOLocalRotate(new Vector3(360.0f, 0.0f, 0.0f), 1.0f, RotateMode.FastBeyond360)
                                          .SetEase(Ease.Linear)
                                          .SetLoops(-1, LoopType.Restart);
    }

    public void startGame() {
        m_seManager.notes[1].Play();
        m_turnDecisionButton.SetActive(false);
        m_backGroundImage.DOFade(0.0f, 1.0f).OnComplete(() => {
            m_gameManager.Initialized(m_isTurn);
        });
    }
}
