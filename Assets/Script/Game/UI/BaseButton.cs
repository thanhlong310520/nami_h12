using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [SerializeField] protected Button bt;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(bt == null) bt = GetComponent<Button>();
        bt.onClick.AddListener(Handle);
    }
    protected virtual void Handle()
    {
        GameManager.instance.audioManager.PlaySound(0);
    }
}
