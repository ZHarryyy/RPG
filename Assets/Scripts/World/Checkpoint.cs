using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string Id;
    public bool activationStatus;
    private bool canActivate;

    [SerializeField] private GameObject lightSmall;
    [SerializeField] private GameObject lightBig;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canActivate && Input.GetKeyDown(KeyCode.E)) ActivateCheckpoint();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        Id = Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) canActivate = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) canActivate = false;
    }

    public void ActivateCheckpoint()
    {
        activationStatus = true;
        lightSmall.SetActive(false);
        anim.SetBool("active", true);
        lightBig.SetActive(true);
    }
}
