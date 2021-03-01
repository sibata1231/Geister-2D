/**
 * @file LighthingAnimation.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/08 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

/**
 * @class LighthingAnimation
 * @brief 
 */
public class LighthingAnimation : MonoBehaviour {
    [SerializeField] private Light2D m_light;

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        // ライトの強さ
        float intensity = m_light.intensity;
        DOTween.To(() => intensity, x => m_light.intensity = x, 5.0f, 3.0f)
               .OnComplete(() => DOTween.To(() => intensity, x => m_light.intensity = x, 2.0f, 3.0f))
                                        .SetLoops(-1, LoopType.Yoyo);
        // ライトの内半径
        float inner = m_light.pointLightInnerRadius;
        DOTween.To(() => inner, x => m_light.pointLightInnerRadius = x, 2.0f, 3.0f)
               .OnComplete(() => DOTween.To(() => inner, x => m_light.pointLightInnerRadius = x, 1.0f, 3.0f))
                                        .SetLoops(-1, LoopType.Yoyo);
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update() {
        
    }
}
