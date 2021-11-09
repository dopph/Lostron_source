using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager_MapManage : MonoBehaviour
{
    [SerializeField] int MaxHierarchy;
    public int currentLocale;
    public int currentHierarchy;
    [SerializeField] GameManager_Event GM_E;

    private GameObject BackGround;
    [SerializeField] List<GameObject> BackGroundList;
    [SerializeField] List<GameObject> BossBackGroundList;
    public List<Room> Rooms = new List<Room>();

    void Awake() {

        BG_Set();

        if(StartItem.isContinue == false){
            currentLocale = 2;
            currentHierarchy = 0;
            Rooms.Add(new Room(0, 2, false, 0, 0));

            for (int i=0; i<MaxHierarchy; i++){
                for (int c=0; c<5; c++){
                    if(Rooms.Count(room => room.hierarchy == i && room.column == c) == 1)generateRoom(i, c);
                }
            }
            for (int i=0; i<MaxHierarchy; i++){
                for (int c=0; c<5; c++){
                    if(Rooms.Count(room => room.hierarchy == i && room.column == c) == 1)checkStuck(i, c);
                }
            }

            for (int i=0; i<=MaxHierarchy; i++){
                for (int c=0; c<5; c++){
                    if(Rooms.Count(room => room.hierarchy == i && room.column == c) == 1)generateRisk(i, c);
                }
            }

            for (int i=0; i<=MaxHierarchy; i++){
                for (int c=0; c<5; c++){
                    if(Rooms.Count(room => room.hierarchy == i && room.column == c) == 1)checkRisk(i, c);
                }
            }
        }else{
            currentLocale = ES3.Load<int>("currentLocale");
            currentHierarchy = ES3.Load<int>("currentHierarchy");
            Rooms = ES3.Load<List<Room>>("rooms");
        }
        
    }

    private void generateRoom(int hierarchy, int column){

        bool needKey;
        int roomEvent = 0;
        int risk = 0;
        
        int leftChance = 50;
        int rightChance = 50;
        int straightChance = 50;

        int leftCurve = Random.Range(0, 100);
        int straight = Random.Range(0, 100);
        int rightCurve = Random.Range(0, 100);

        if(column == 2){leftChance += 10; rightChance += 10; straightChance -= 25;}
        if(column == 1 || column == 3){leftChance += 15; rightChance += 15; straightChance += 10;}
        if(column == 0){rightChance += 15; straightChance -= 10;}
        if(column == 4){leftChance += 15; straightChance -= 10;}

        if(Rooms.Count(room => room.hierarchy == hierarchy) <= 1){
            leftChance += 30;
            rightChance += 30;
            straightChance += 10;
        }

        if(Rooms.Count(room => room.hierarchy == hierarchy) <= 2){
            leftChance += 15;
            rightChance += 15;
        }

        if(Rooms.Count(room => room.hierarchy == hierarchy) >= 4){
            leftChance -= 20;
            rightChance -= 20;
            straightChance -= 20;
        }

        if(column == 0)leftChance = 0;
        if(column == 4)rightChance = 0;
        if(leftCurve >= leftChance && rightCurve >= rightChance && straight >= straightChance){
            if(column == 0){
                rightChance = 100;
            }else if(column == 4){
                leftChance = 100;
            }else if((hierarchy + column) % 2 == 0){
                leftChance = 100;
            }else{
                rightChance = 100;
            }
        }

        if(hierarchy != MaxHierarchy-1){
            if(straight < straightChance){

                needKey = false;
                if(Random.Range(0, 2) == 0 && hierarchy >= 4)needKey = true;

                if(Rooms.Count(room => room.hierarchy == hierarchy+1 && room.column == column) == 0)Rooms.Add(new Room(hierarchy+1, column, needKey, risk, roomEvent));
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy+1 && room.column == column).fromTop = true;
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column).straightBranch = true;

            }

            if(leftCurve < leftChance){

                needKey = false;
                if(Random.Range(0, 2) == 0 && hierarchy >= 4)needKey = true;

                if(Rooms.Count(room => room.hierarchy == hierarchy+1 && room.column == column-1) == 0)Rooms.Add(new Room(hierarchy+1, column-1, needKey, risk, roomEvent));
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy+1 && room.column == column-1).fromRight = true;
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column).leftBranch = true;
            }

            if(rightCurve < rightChance){

                needKey = false;
                if(Random.Range(0, 2) == 0 && hierarchy >= 4)needKey = true;

                if(Rooms.Count(room => room.hierarchy == hierarchy+1 && room.column == column+1) == 0)Rooms.Add(new Room(hierarchy+1, column+1, needKey, risk, roomEvent));
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy+1 && room.column == column+1).fromLeft = true;
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column).rightBranch = true;
            }
        }

        if(Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column).fromLeft == true && Rooms.Count(room => room.hierarchy == hierarchy-1 && room.column == column && room.leftBranch == true) == 1){
            
            Rooms.FirstOrDefault(room => room.hierarchy == hierarchy-1 && room.column == column).straightBranch = true;
            Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column).fromTop = true;
            
            if((hierarchy + column) % 2 == 0){
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy-1 && room.column == column-1).rightBranch = false;
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column).fromLeft = false;
            }else{
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy-1 && room.column == column).leftBranch = false;
                Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column-1).fromRight = false;
            }

            Rooms.FirstOrDefault(room => room.hierarchy == hierarchy-1 && room.column == column-1).straightBranch = true;
            Rooms.FirstOrDefault(room => room.hierarchy == hierarchy && room.column == column-1).fromTop = true;

        }

    }

    private void checkStuck(int hierarchy, int column){

        Room thisRoom = Rooms.First(room => room.hierarchy == hierarchy && room.column == column);
        bool straightLock = false;
        bool leftLock = false;
        bool rightLock = false;
        bool cantStraight = false;
        bool cantLeft = false;
        bool cantRight = false;

        if(thisRoom.straightBranch == true){
             if(Rooms.FirstOrDefault(room => room.hierarchy == hierarchy+1 && room.column == column).needKey == true){
                 straightLock = true;
                 cantStraight = true;
             }
        }else{
            cantStraight = true;
        }

        if(thisRoom.leftBranch == true ){
            if(Rooms.FirstOrDefault(room => room.hierarchy == hierarchy+1 && room.column == column-1).needKey == true){
                leftLock = true;
                cantLeft = true;
            }
        }else{
            cantLeft = true;
        }

        if(thisRoom.rightBranch == true ){
            if(Rooms.FirstOrDefault(room => room.hierarchy == hierarchy+1 && room.column == column+1).needKey == true ){
                rightLock = true;
                cantRight = true;
            }
        }else{
            cantRight = true;
        }

        if(cantLeft == true && cantStraight == true && cantRight == true){
            if( leftLock && rightLock ){
                if( column < 2 || (column == 2 && Random.Range(0, 2) == 0)){ 
                    Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column-1).needKey = false;
                }else{
                    Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column+1).needKey = false;
                }
            }else if(leftLock && straightLock){
                if( column < 2 || (column == 2 && Random.Range(0, 2) == 0)){ 
                    Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column-1).needKey = false;
                }else{
                    Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column).needKey = false;
                }
            }else if(rightLock && straightLock){
                if( column < 2 || (column == 2 && Random.Range(0, 2) == 0)){ 
                    Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column).needKey = false;
                }else{
                    Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column+1).needKey = false;
                }
            }else if(leftLock){
                Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column-1).needKey = false;
            }else if(straightLock){
                Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column).needKey = false;
            }else if(rightLock){
                Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column+1).needKey = false;
            }
        }
        if(hierarchy == 10 || hierarchy == 20)thisRoom.needKey = false;
    }

    private void generateRisk(int hierarchy, int column){

        Room thisRoom = Rooms.First(room => room.hierarchy == hierarchy && room.column == column);
        bool fromRisky = false;
        bool fromRiskyAll = true;
        List<EventDB> ed;

        if(hierarchy >= 2){

            if(thisRoom.fromTop == true){
                if(Rooms.First(room => room.hierarchy == hierarchy-1 && room.column == column).risk == 2){
                    fromRisky = true;
                }else{
                    fromRiskyAll = false;
                }
            }

            if(thisRoom.fromLeft == true){
                if(Rooms.First(room => room.hierarchy == hierarchy-1 && room.column == column-1).risk == 2){
                    fromRisky = true;
                }else{
                    fromRiskyAll = false;
                }
            }

            if(thisRoom.fromRight == true){
                if(Rooms.First(room => room.hierarchy == hierarchy-1 && room.column == column+1).risk == 2){
                    fromRisky = true;
                }else{
                    fromRiskyAll = false;
                }
            }

            if(fromRiskyAll == true){
                if(Random.Range(0,10) < 4)thisRoom.risk = 2;
            }else{
                if(fromRisky == true && Random.Range(0,10) < 3)thisRoom.risk = 2;
                if(fromRisky == false && Random.Range(0,10) < 2)thisRoom.risk = 2;
            }
        }

        if(hierarchy == 10 || hierarchy == 20){
            thisRoom.risk = 3;
        }
        
        if(hierarchy >= 1){
            if(thisRoom.risk != 3){
                if(thisRoom.risk == 0 || thisRoom.risk == 1){
                    if( thisRoom.needKey == false && Random.Range(0, 10) != 0){
                        ed = GM_E.getEventList().Where(e => e.Risk == 0 && e.ID < 1000 && e.Text != "").ToList();
                        if(hierarchy > 10)ed = GM_E.getEventList().Where(e => e.Risk == 0 && e.ID > 1000 && e.Text != "").ToList();
                        thisRoom.roomEvent = ed[Random.Range(0, ed.Count)].ID;
                    }else if(thisRoom.needKey == true){
                        ed = GM_E.getEventList().Where(e => e.Risk == 3 && e.ID < 1000 && e.Text != "").ToList();
                        //if(hierarchy > 10)ed = GM_E.getEventList().Where(e => e.Risk == 3 && e.ID > 1000 && e.Text != "").ToList();
                        thisRoom.roomEvent = ed[Random.Range(0, ed.Count)].ID;
                    }
                    
                }else if(thisRoom.risk == 2){
                    if( thisRoom.needKey == false && Random.Range(0, 10) != 0){
                        ed = GM_E.getEventList().Where(e => e.Risk == 1 && e.ID < 1000 && e.Text != "").ToList();
                        if(hierarchy > 10)ed = GM_E.getEventList().Where(e => e.Risk == 1 && e.ID > 1000 && e.Text != "").ToList();
                        thisRoom.roomEvent = ed[Random.Range(0, ed.Count)].ID;
                    }else if(thisRoom.needKey == true){
                        ed = GM_E.getEventList().Where(e => e.Risk == 4 && e.ID < 1000 && e.Text != "").ToList();
                        //if(hierarchy > 10)ed = GM_E.getEventList().Where(e => e.Risk == 4 && e.ID > 1000 && e.Text != "").ToList();
                        thisRoom.roomEvent = ed[Random.Range(0, ed.Count)].ID;
                    }
                    
                }
            }else if(thisRoom.risk == 3){
                ed = GM_E.getEventList().Where(e => e.Risk == 2 && e.Text != "").ToList();
                if(hierarchy == 7 && Random.Range(0,3) == 0)thisRoom.roomEvent = 54;
                if(hierarchy == 10)thisRoom.roomEvent = ed[0].ID;
                if(hierarchy == 20)thisRoom.roomEvent = ed[1].ID;
            }
        }
    }

    private void checkRisk(int hierarchy, int column){

        Room thisRoom = Rooms.First(room => room.hierarchy == hierarchy && room.column == column);
        bool allRisky = true;

        if(thisRoom.risk <= 1){
            if(thisRoom.straightBranch == true){
                if(Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column).risk <= 1 && Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column).needKey == false){
                    allRisky = false;
                }
            }
            if(thisRoom.leftBranch == true){
                if(Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column-1).risk <= 1 && Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column-1).needKey == false){
                    allRisky = false;
                }
            }
            if(thisRoom.rightBranch == true){
                if(Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column+1).risk <= 1 && Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column+1).needKey == false){
                    allRisky = false;
                }
            }
            
            if(allRisky == true && thisRoom.risk != 3)thisRoom.risk = 1;
        }
    }

    public void BG_Set(){

        if(BackGround != null)DestroyImmediate(BackGround);
        if(currentHierarchy == 10){
            BackGround = Instantiate(BossBackGroundList[0]);
        }else if(currentHierarchy == 20){
            BackGround = Instantiate(BossBackGroundList[1]);
        }else if(currentHierarchy < 10){
            BackGround = Instantiate(BackGroundList[Random.Range(0,5)]);
        }else{
            BackGround = Instantiate(BackGroundList[Random.Range(5,10)]);
        }

    }

    public List<Room> GetRoomDataList() {
        return Rooms;
    }
}