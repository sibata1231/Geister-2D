/**
 * @file NameInputField.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/08 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * @class NameInputField
 * @brief 
 */
public class NameInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_NameInput = default;
    public string Name{
        get{
            return m_NameInput.text;
        }
        private set{
            m_NameInput.text = value;
        }

    }

    /**
     * @brief 開始時処理
     * @return なし
     */
    void Start()
    {
        Name = PlayerPrefs.GetString("Name", "ななしさん");
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        
    }


}
