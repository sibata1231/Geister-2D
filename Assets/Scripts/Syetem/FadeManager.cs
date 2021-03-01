using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField] private Image m_image = default;

    public bool EndFade {
        get { return m_image.enabled; }
        set { m_image.enabled = value; }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Tweener fadeIn(){
        m_image.enabled = true;
        return m_image.DOFade(0.0f, 1.0f);
    }
    public Tweener fadeOut(){
        m_image.enabled = true;
        return m_image.DOFade(1.0f, 3.0f);
    }
}
