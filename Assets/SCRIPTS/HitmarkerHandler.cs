using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitmarkerHandler : MonoBehaviour
{
    public float lifetime;

    [HideInInspector] public Transform target;
    public TextMeshProUGUI textObj;

    private void Awake()
    {
        target = GameObject.Find("MainCamera").transform;
    }

    private void Start()
    {
        Invoke("DestroyHitmarker", lifetime);
    }

    private void Update()
    {
        transform.LookAt(transform.position - (target.position - transform.position));
    }

    public void SetNumber(float n)
    {
        textObj.text = "-" + n.ToString();
    }

    public void DestroyHitmarker()
    {
        Destroy(gameObject);
    }
}
