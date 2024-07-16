using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;

public class ObjectCtr : MonoBehaviour
{
    public Rigidbody2D rig;
    public Transform check;
    public Transform checkGround;
    public Collider2D coli;
    public ParticleSystem particleSmoke;
    public ParticleSystem expolPS;
    public ParticleSystem firePS;
    public SpriteRenderer sprite;
    public List<Sprite> spriteCar;
    public Vector3 startPos;
    public float speed = 5;
    public float radius = 0.5f;
    public float radiusCG = 0.1f;
    public bool contactBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        SetDefault();
    }

    // Update is called once per frame
    void Update()
    {
        if (!contactBlock) return; 
        rig.velocity = new Vector2(speed, 0);
        CheckContactBlock();
        CheckContactPointWin();
        CheckGround();
    }

    void CheckContactBlock()
    {
        Collider2D[] listContactBlock = Physics2D.OverlapCircleAll(check.position, radius, LayerMask.GetMask("Block"));
        if (listContactBlock.Length > 0)
        {
            StartCoroutine(Contact());
        }
    }
    void CheckContactPointWin()
    {
        Collider2D[] listContactBlock = Physics2D.OverlapCircleAll(check.position, radius, LayerMask.GetMask("PointWin"));
        if (listContactBlock.Length > 0)
        {
            Complete();
        }
    }
    void CheckGround()
    {
        Collider2D[] listContactBlock = Physics2D.OverlapCircleAll(check.position, radiusCG);
        if (listContactBlock.Length <= 0)
        {
            coli.isTrigger = false;
            rig.gravityScale = 2;
        }
    }

    public void SetDefault()
    {
        if (coli == null) coli = gameObject.GetComponent<Collider2D>();
        if(rig == null) rig = gameObject.GetComponent<Rigidbody2D>();
        coli.isTrigger = true;
        firePS?.Stop();
        sprite.sprite = spriteCar[0];
        rig.velocity = Vector2.zero;
        rig.angularVelocity = 0;
        if(rig.sharedMaterial != null)
        {
            rig.sharedMaterial.friction = 0;
        }
        rig.gravityScale = 0;
        transform.rotation =  Quaternion.Euler(0, 0, 0);
        transform.position = startPos;
        particleSmoke?.Stop();
        contactBlock = false;
    }
    public void Move()
    {
        contactBlock = true;
        rig.gravityScale = 0;
        coli.isTrigger = true; 
    }

    IEnumerator Contact()
    {
        GameManager.instance.Vibrate();
        coli.isTrigger = false;
        rig.gravityScale = 2;
        rig.sharedMaterial.friction = 1;
        particleSmoke.Stop();
        StartCoroutine(SetAnimContact());
        GameManager.instance.audioManager.PlaySound(3);
        GameCtr.instance.GameOver();
        Debug.Log("contact");
        contactBlock = false;
        //StartCoroutine(SetAnimContact());
        yield return new WaitForSeconds(0.1f);
        rig.velocity = Vector2.zero;
        //rig.gravityScale = 0;
    }
    IEnumerator SetAnimContact()
    {
        expolPS.Play();
        //fireSkeleton.gameObject.SetActive(true);
        //fireSkeleton.AnimationName = "Begin";
        yield return new WaitForSeconds(0.25f);
        firePS.Play();
        //fireSkeleton.AnimationName = "Idle";
    }
    void Complete()
    {
        rig.sharedMaterial.friction = 1;
        rig.velocity = Vector2.zero;
        //canvas1.SetActive(true);
        particleSmoke.Stop();
        Debug.Log("Complete");
        StartCoroutine(GameCtr.instance.Win());
        contactBlock = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(check.position, radius);
        Gizmos.DrawWireSphere(checkGround.position, radiusCG);
    }
}
