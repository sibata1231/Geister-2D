/**
 * @file TitleLogoAnimation.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/08 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using TMPro;

/**
 * @class TitleLogoAnimation
 * @brief 
 */
public class TitleLogoAnimation : MonoBehaviour { 
    [SerializeField] private Light2D m_pointLight = default;
    [SerializeField] private Light2D m_globalLight = default;
    [SerializeField] private GameObject m_nameInputField = default;
    [SerializeField] private GameObject m_pass = default;
    [SerializeField] private GameObject m_decisionButton = default;

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_nameInputField.SetActive(false);
        m_pass.SetActive(false);
        m_decisionButton.SetActive(false);
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        
    }
    public void startAnimation() {
        m_pointLight.transform.DOMoveX(10, 2.0f)
                              .SetRelative()
                              .SetEase(Ease.InOutSine)
                              .OnComplete(() => {
                                  float intencity = m_globalLight.intensity;
                                  DOTween.To(() => intencity, x => m_globalLight.intensity = x, 1.0f, 2.0f)
                                         .OnComplete(()=> {
                                             m_nameInputField.SetActive(true);
                                             m_pass.SetActive(true);
                                             m_decisionButton.SetActive(true);
                                         });
                              });
    }
}
