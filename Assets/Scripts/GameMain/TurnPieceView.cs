/**
 * @file TurnPieceView.cs
 * @brief 
 * @author 
 * @date 2020/10/06 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * @class TurnPieceView
 * @brief 
 */
public class TurnPieceView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sprite = default;
    [SerializeField] private Sprite m_turntrue  = default;
    [SerializeField] private Sprite m_turnfalse = default;

    private bool m_turn;

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start()
    {

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
    public void TurnChangePiece(){
        if (m_turn) {
            m_sprite.sprite = m_turnfalse;
        } else {
            m_sprite.sprite = m_turntrue;
        }
        m_turn = !m_turn;
    }

    /**
     * @brief 
     * @param[in] isturn 
     * @return なし
     */
    public void SetTurnPiece(bool isturn){
        if (isturn){
            m_sprite.sprite = m_turntrue;
        } else{
            m_sprite.sprite = m_turnfalse;
        }
        m_turn = isturn;
    }


    /**
     * @brief 
     * @param[in] isturn 
     * @return なし
     */
    public void SetActivePiece() {
        m_sprite.sprite = m_turntrue;
    }


    /**
     * @brief 
     * @param[in] isturn 
     * @return なし
     */
    public void SetInActivePiece() {
        m_sprite.sprite = m_turnfalse;
    }
}