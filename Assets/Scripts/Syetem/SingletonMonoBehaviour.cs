/**
 * @file SingletonMonoBehaviour.cs
 * @brief 
 * @author T.Shibata
 * @date 2020/10/02 作成
 */
using UnityEngine;
using System;

/**
 * @class SingletonMonoBehaviour
 * @brief 
 */
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null) {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }    

    /**
     * @brief 
     * @return 
     */
    protected bool CheckInstance() {
        if (instance == null) {
            instance = this as T;
            return true;
        } else if (Instance == this) {
            return true;
        }
        Destroy(this);
        return false;
    }
}