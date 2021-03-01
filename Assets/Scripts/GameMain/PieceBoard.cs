/**
 * @file PieceBoard.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/05 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

/**
 * @class PieceBoard
 * @brief 
 */
public class PieceBoard : MonoBehaviour {
    [SerializeField] SpriteRenderer[] m_evilSprites = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] m_goodSprites = new SpriteRenderer[4];

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start() {

    }

    /**
     * @brief 開始時処理
     * @brief  駒が取得された際にどの駒だったのかを表示する
     * @return なし
     */
    public void setPieceView(Pieces piece) {
        if(Pieces.OPPONENT_EVIL == piece || Pieces.PLAYER_EVIL == piece) {         // 悪いやつ
            foreach (SpriteRenderer sprite in m_evilSprites) {
                if(!sprite.enabled) {
                    sprite.enabled = true;
                    sprite.DOFade(1.0f, 0.5f);
                    break;
                }
            }
        } else if (Pieces.OPPONENT_GOOD == piece || Pieces.PLAYER_GOOD == piece) { // いいやつ
            foreach (SpriteRenderer sprite in m_goodSprites) {
                if (!sprite.enabled) {
                    sprite.enabled = true;
                    sprite.DOFade(1.0f, 0.5f);
                    break;
                }
            }
        }
    }
}
