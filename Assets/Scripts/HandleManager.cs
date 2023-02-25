using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleManager : MonoBehaviour
{
    private LineScript line;
    private float startTime;
    private float penaltyTime;
    private float penaltyTracker;
    private float whenToPunish;
    private bool destroyed = false;
    private bool success;
    private bool failure;
    private bool followMouse;
    private bool countPenalty = false;
    private Vector3 mousePosition;
    private Camera fightCam;
    private Rigidbody2D rb;

    private float minParryTime;
    private float maxPenaltyParryTime;

    private float destroyTime;
    private int sortOrder = 20;

    private SpriteRenderer spRender;
    private Vector3 correction = Vector3.zero;


    void Awake(){
        line = GetComponentInParent<LineScript>();
        rb = GetComponent<Rigidbody2D>();
        spRender = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        startTime = Time.time;
        penaltyTracker = 0f;
        whenToPunish = .05f;
        destroyTime = 2.5f;

        minParryTime = 3f;
        maxPenaltyParryTime = .2f;
    }

    public void CountPenalty(bool count){
        Debug.Log("CountingPenalty");
        if(count){
            penaltyTime = Time.time;
            countPenalty = true;
        }
        else{
            countPenalty = false;
        }
    }

    void Update(){

        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + correction;

        if(countPenalty){
            penaltyTracker = penaltyTracker + Time.deltaTime;
        }

        if(followMouse){
            this.transform.position = new Vector3(mousePosition.x, mousePosition.y, .6f);
            spRender.sortingOrder = sortOrder;
        }
        if(success && !destroyed){
            destroyed = true;
            if(Time.time - startTime <= minParryTime && penaltyTime <= maxPenaltyParryTime){
                line.DestroyThisLine(true, 2);
            }
            else{
                line.DestroyThisLine(true, 1);
            }
        }
        else if(penaltyTracker > whenToPunish){
            destroyed = true;
            line.DestroyThisLine(false, 2);
        }
        else if(failure && !destroyed){
            destroyed = true;
            line.DestroyThisLine(false, 2);
        }
        else if(Time.time - startTime >= destroyTime && !destroyed){
            destroyed = true;
            line.DestroyThisLine(false, 1);
        }
    }

    public void SetSuccess(){
        success = true;
    }
    public void SetFailure(){
        failure = true;
    }

    public void SetToMouse(bool follow){
        Debug.Log("should be following");
        correction = transform.position - Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        followMouse = follow;
    }

}
