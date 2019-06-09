using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetCollection : Powerup
{
    public MeshRenderer meshRenderer;

    protected override void Start()
    {
        base.Start();
        meshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f);
    }

    public override void PickedUp()
    {
        GameManager.instance.AddScore(1);        
        base.PickedUp();
    }

    private void FixedUpdate()
    {        
        transform.Rotate(new Vector3(100 * Time.deltaTime, 0, 100 * Time.deltaTime));
    }


    //   Vector3 originalPos;
    //   GameObject target;

    //   [SerializeField]
    //   float damping = 1.5f;

    //// Use this for initialization
    //void Start () {
    //       originalPos = transform.position;
    //       target = GameObject.FindGameObjectWithTag("ImgSweet");
    //       //targetPos = Camera.mGameObject.FindGameObjectWithTag("ImgSweet")ain.ScreenToWorldPoint(GameObject.FindGameObjectWithTag("ImgSweet").transform.position);
    //   }

    //// Update is called once per frame
    //void Update () {

    //       var targetPos = Camera.main.ScreenToWorldPoint(target.GetComponent<RectTransform>().position) + new Vector3(0, 0, 3); 
    //       targetPos.y = originalPos.y;

    //       Vector3 newPos = Vector3.Lerp(transform.position, targetPos, damping * Time.deltaTime);

    //       transform.position = newPos;

    //       if (Vector3.Distance(transform.position, targetPos) < 2)
    //       {
    //           GameManager.instance.AddScore(1);
    //           GameManager.instance.UpdateCookiesUI();
    //           AudioManager.instance.Play("sweet_collect");
    //           Destroy(gameObject);
    //       }
    //   }
}
