/**
 * @file Target.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/02 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * @class Target
 * @brief 
 */
public class Target : MonoBehaviour {
    [SerializeField] private Direction      m_direction;      //!< 方向(上下左右)
    [SerializeField] private Piece          m_piece;          //!< 親の駒
    [SerializeField] private SpriteRenderer m_spriteRenderer; //!< SpreiteRenderer

    private Tweener m_fadeTweener;

    const float START_ALPHA = 0.5F;
    const float END_ALPHA = 1.0F;

    private void OnMouseDown() {
        if (m_spriteRenderer.enabled) {
            m_piece.SetMove(m_direction);
        }
    }
    void Start(){
        m_fadeTweener = m_spriteRenderer.DOFade(START_ALPHA, 1.0f)
                                        .OnComplete(() => {teweenCheck(START_ALPHA);m_fadeTweener = m_spriteRenderer
                                        .DOFade(END_ALPHA, 1.0f).SetLoops(-1, LoopType.Yoyo);
                                        });
    }
    void teweenCheck(float alpha){
        if (m_fadeTweener != null){
            m_fadeTweener.Kill();
            m_fadeTweener = null;
            m_spriteRenderer.DOFade(alpha, 0.0f);
        }
     
    }
}
