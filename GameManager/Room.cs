using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Room{

    public GameManager_MainScene GameManager;
    public int hierarchy;
    public int column;
    public bool needKey = false;
    public int risk;
    public int roomEvent;
    public bool leftBranch;
    public bool rightBranch;
    public bool straightBranch;
    public bool fromLeft;
    public bool fromTop;
    public bool fromRight;

    public Room(int hierarchy, int column, bool needKey, int risk, int roomEvent){
        this.hierarchy = hierarchy;
        this.column = column;
        this.needKey = needKey;
        this.risk = risk;
        this.roomEvent = roomEvent;
        this.leftBranch = false;
        this.rightBranch = false;
        this.straightBranch = false;
        this.fromLeft = false;
        this.fromTop = false;
        this.fromRight = false;
    }

}

