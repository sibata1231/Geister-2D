using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
    
public class TitleManager : SingletonMonoBehaviour<TitleManager> {
    [SerializeField] private NameInputField m_nameInputField = default;
    [SerializeField] private TMP_InputField m_pass = default;
    [SerializeField] private TitleLogoAnimation m_titleLogoAnimation = default;
    private BGMManager m_bgmManager;
    private SEManager  m_seManager;


    void Awake()
    {
        m_seManager = SEManager.Instance;
        m_bgmManager = BGMManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_bgmManager.bgms[1].Play();
        FadeManager.Instance.fadeIn().OnComplete(()=> {
            m_titleLogoAnimation.startAnimation();
            FadeManager.Instance.EndFade = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegister(){
        m_seManager.notes[1].Play();
        if (m_nameInputField.Name == "")
        {
            PlayerPrefs.SetString("Name", "ななしさん");
        }
        else { 
            PlayerPrefs.SetString("Name", m_nameInputField.Name);
        }
        PlayerPrefs.SetString("Pass", m_pass.text);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("Name"));
        Debug.Log(PlayerPrefs.GetString("Pass"));

        Tweener tweener = FadeManager.Instance.fadeOut();
        tweener.OnComplete(() => SceneLoader.Instance.sceneChange("Game"));
  
    }
}
