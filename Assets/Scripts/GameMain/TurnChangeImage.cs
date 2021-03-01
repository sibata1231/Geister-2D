/**
 * @file TurnChangeImage.cs
 * @brief 
 * @author 
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/**
 * @class TurnChangeImage
 * @brief 
 */
public class TurnChangeImage : SingletonMonoBehaviour<TurnChangeImage> {
    [SerializeField] private Image m_image = default;
    private Tweener m_fadeTweener;
    private Tweener m_rotateTweener;

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {
        m_fadeTweener   = null;
        m_rotateTweener = null;
        m_image.enabled = false;
        m_image.DOFade(0.0f, 0.0f);
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
     * @return なし
     */
    public void TurnChangeAnimation(){
        m_image.enabled = true;
        // Fade初期化
        if (m_fadeTweener != null) {
            m_fadeTweener.Kill();
            m_fadeTweener = null;
            m_image.DOFade(0.0f, 0.0f);
        }
        // 回転の初期化
        if (m_rotateTweener != null) {
            m_rotateTweener.Kill();
            m_rotateTweener = null;
            m_image.transform.localRotation = Quaternion.identity;
        }
        m_rotateTweener = m_image.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 360.0f), 1.0f, RotateMode.FastBeyond360)
                                           .SetEase(Ease.Linear)
                                           .SetLoops(-1, LoopType.Restart);

        m_fadeTweener = m_image.DOFade(1.0f, 0.5f)
                               .OnComplete(() => { m_fadeTweener = m_image.DOFade(0.0f, 0.5f)
                                                                          .OnComplete(() => { m_image.enabled = false; }); });
        
    }

}
