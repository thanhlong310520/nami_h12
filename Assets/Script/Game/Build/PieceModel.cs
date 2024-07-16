using TMPro;
using UnityEngine;

public class PieceModel : MonoBehaviour
{
    [SerializeField] int cost = 20;
    public int Cost { get => cost; private set => cost = value; }
    public bool isActive = false;
    public ModelCtr modelCtr;
    public Transform holder;
    public Animator animator;


    public bool ismap = false;
    public Sprite icon;
    public string textTask = "test";

    public void SetStart(bool atv)
    {
        isActive = atv;
        holder.gameObject.SetActive (isActive);
    }
    public void FillColorForMesh(){
        holder.gameObject.SetActive(true);
        animator.Play("Show");
    }
    public void ActiveModel() 
    {
        isActive = true;
        modelCtr.AddPieceToData(this);
    }
    public bool CheckCanfill()
    {
        if (isActive) return false;
        if (GameManager.instance.playerData.coinNumber < cost) return false;
        return true;
    }


}
