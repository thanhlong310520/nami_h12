using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointWin : MonoBehaviour
{
    [SerializeField] SkeletonAnimation humanSkeleton;
    [SerializeField] SpriteRenderer spriteLose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLevel()
    {
        humanSkeleton.gameObject.SetActive(true);
        //spriteLose.gameObject.SetActive(false);
        humanSkeleton.AnimationName = "Hi";
    }
    public void Lose()
    {
        humanSkeleton.gameObject.SetActive(true);
        humanSkeleton.AnimationName = "Angry";
        //spriteLose.gameObject.SetActive(true);
        //spriteLose.GetComponent<Animator>().Play("Lose");
    }
    public void Win()
    {
        humanSkeleton.gameObject.SetActive(false);
        spriteLose.gameObject.SetActive(false);
    }

}
