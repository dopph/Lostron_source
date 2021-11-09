using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class GameManager_Event{
    
    public bool SelectRequire(int Condition){

        if(Condition == 1){//仲間2人以下
            if(GM.Players[0].GetComponent<Character>().isDown == true || GM.Players[1].GetComponent<Character>().isDown == true || GM.Players[2].GetComponent<Character>().isDown == true) return true;
        }if(Condition == 2){//警備が手薄フラグなし
            if(EventFlag.IndexOf("diversion") == -1) return true;
        }if(Condition == 3){//警備が手薄フラグあり
            if(EventFlag.IndexOf("diversion") != -1) return true;
        }else if(Condition == 302001){//回復キット所持
            if(GM_I.GetItemDataList().Count(item=> item.getID() == Condition) > 0){
                return true;
            }
        }else if(Condition == 302002){//回復キット所持
            if(GM_I.GetItemDataList().Count(item=> item.getID() == Condition) > 0){
                return true;
            }
        }else if(Condition == 104001){//グレネード所持
            if(GM_I.GetItemDataList().Count(item=> item.getID() == Condition) > 0){
                return true;
            }
        }
        return false;
    }

    public void AddMember(GameObject Chara){

        GameObject obj;
        if(GM.Players[0].GetComponent<Character>().isDown == true){
            GM.AppearPlayer(Chara, 0);
        }else if(GM.Players[1].GetComponent<Character>().isDown == true){
            GM.AppearPlayer(Chara, 1);
        }else if(GM.Players[2].GetComponent<Character>().isDown == true){
            GM.AppearPlayer(Chara, 2);
        }

    }

    public void EventAllHeal(int Value){
        foreach(GameObject Chara in GM.Players){
            if(Chara.GetComponent<Character>().isDown == false){
                Chara.GetComponent<Character>().Heal(Value);
            }
        }
    }
    public Character EventRandomOneHeal(int Value){
        List<Character> arrive = new List<Character>();
        Character PickChara;
        foreach(GameObject Chara in GM.Players){
            if(Chara.GetComponent<Character>().isDown == false){
                arrive.Add(Chara.GetComponent<Character>());
            }
        }
        PickChara = arrive[Random.Range(0, arrive.Count)];
        PickChara.Heal(Value);
        return PickChara;
    }

    public void EventAllDamage(int Value){
        foreach(GameObject Chara in GM.Players){
            if(Chara.GetComponent<Character>().isDown == false){
                Chara.GetComponent<Character>().TakeDamage(Value, false);
                if(Chara.GetComponent<Character>().getHP() < 1)Chara.GetComponent<Character>().setHP(1, Chara.GetComponent<Character>().getMaxHP());
            }
        }
    }

    public Character EventRandomOneDamage(int Value){
        List<Character> arrive = new List<Character>();
        Character PickChara;
        foreach(GameObject Chara in GM.Players){
            if(Chara.GetComponent<Character>().isDown == false){
                arrive.Add(Chara.GetComponent<Character>());
            }
        }
        PickChara = arrive[Random.Range(0, arrive.Count)];
        PickChara.TakeDamage(Value, false);
        return PickChara;
    }

    public void OverrideEvent(int hierarchy, int column, int ID){
        if(GM_M.Rooms.Count(room => room.hierarchy == hierarchy && room.column == column) != 0){
            Room thisRoom = GM_M.Rooms.First(room => room.hierarchy == hierarchy && room.column == column);
            thisRoom.roomEvent = ID;
        }
    }

    public void OverrideNextFloorEvent(int hierarchy, int column, int ID, int floor){
        Room thisRoom = GM_M.Rooms.First(room => room.hierarchy == hierarchy && room.column == column);
        if(thisRoom.straightBranch == true){
            if( GM_M.Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column).needKey == false ){
                OverrideEvent(hierarchy, column, ID);
                if(floor > 0)OverrideNextFloorEvent(hierarchy+1, column, ID, floor-1);
            }
        }
        if(thisRoom.leftBranch == true){
            if( GM_M.Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column-1).needKey == false ){
                OverrideEvent(hierarchy, column, ID);
                if(floor > 0)OverrideNextFloorEvent(hierarchy+1, column-1, ID, floor-1);
            }
        }
        if(thisRoom.rightBranch == true){
            if( GM_M.Rooms.First(room => room.hierarchy == hierarchy+1 && room.column == column+1).needKey == false ){
                OverrideEvent(hierarchy, column, ID);
                if(floor > 0)OverrideNextFloorEvent(hierarchy+1, column+1, ID, floor-1);
            }
        }
    }

    public bool loadEvents(int ID){
        
        if(ID == 2){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[0], EnemyData[0], null});

        }else if(ID == -2){
            EventAllHeal(1);

        }else if(ID == 5){
            GM_I.addItem(102002, 100);
            GM_I.addItem(106004, 101);
        }else if(ID == 6){
            EventRandomOneDamage(1);

        }else if(ID == 7){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[0], EnemyData[0]});

        }else if(ID == -7){
            GM_I.addItem(201001, 100);

        }else if(ID == 10){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[8], EnemyData[6], null});

        }else if(ID == -10){
            GM_I.addItem(102005, 100);

        }else if(ID == 11){
            GM_I.addItem(201001, 100);

        }else if(ID == 113){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[1], null, null});

        }else if(ID == 16){
            GM_I.addItem(302001, 100);

        }else if(ID == 17){
            GM_I.addItem(104001, 100);

        }else if(ID == 21){
            GM_I.addItem(101003, 100);
            GM_I.addItem(106004, 101);
    
        }else if(ID == 22){
            EventRandomOneDamage(1);

        }else if(ID == 27){
            EventAllHeal(1);

        }else if(ID == 28){
            EventRandomOneDamage(1);

        }else if(ID == 30){
            EventRandomOneDamage(1);

        }else if(ID == 32){
            AddMember(PlayerData[2]);

        }else if(ID == 33){
            GM_I.addItem(302001, 100);

        }else if(ID == 34){
            GM_I.addItem(104001, 100);

        }else if(ID == 35){
            GM_I.addItem(106004, 100);

        }else if(ID == 38){
            GM_I.addItem(302002, 100);

        }else if(ID == 39){
            GM_I.addItem(302001, 100);

        }else if(ID == 43){
            GM_I.addItem(101006, 100);
            GM_I.addItem(106001, 101);

        }else if(ID == 44){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[7], EnemyData[5], null});

        }else if(ID == -44){
            GM_I.addItem(101006, 100);
            GM_I.addItem(106001, 101);

        }else if(ID == 47){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[1], EnemyData[1], null});

        }else if(ID == -47){
            GM_I.addItem(101006, 100);
            GM_I.addItem(106001, 101);

        }else if(ID == 48){
            EventAllHeal(1);

        }else if(ID == 52){
            GM_I.addItem(106002, 100);

        }else if(ID == 53){
            GM_I.addItem(302001, 100);

        }else if(ID == 55){
            OverrideEvent(9, 0, 57);
            OverrideEvent(9, 1, 57);
            OverrideEvent(9, 2, 57);
            OverrideEvent(9, 3, 57);
            OverrideEvent(9, 4, 57);

        }else if(ID == 56){
            OverrideEvent(9, 0, 58);
            OverrideEvent(9, 1, 58);
            OverrideEvent(9, 2, 58);
            OverrideEvent(9, 3, 58);
            OverrideEvent(9, 4, 58);

        }else if(ID == 63){
            EventAllDamage(1);

        }else if(ID == 64){
            GM_I.addItem(104001, 100);

        }else if(ID == 66){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[1], EnemyData[1], EnemyData[0]});

        }else if(ID == -66){
            GM_I.addItem(302002, 100);
            if(Random.Range(0,2) == 0){
                GM_I.addItem(102006, 100);
            }else{
                GM_I.addItem(101006, 100);
            }
        }else if(ID == 71){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[6], EnemyData[5], EnemyData[Random.Range(5,9)]});

        }else if(ID == 73){
            GM_I.addItem(101006, 100);
            GM_I.addItem(102005, 101);

        }else if(ID == 74){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[6], EnemyData[5], EnemyData[5]});

        }else if(ID == 76){
            GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 302001).getSortNo() );
            GM_I.addItem(101005, 100);
            GM_I.addItem(106004, 101);
        }else if(ID == 111){
            GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 302002).getSortNo() );
            GM_I.addItem(102007, 100);
            GM_I.addItem(106004, 101);
            GM_I.addItem(106004, 102);
        }else if(ID == 80){
            EventRandomOneDamage(2);
        }else if(ID == 81){
            GM_I.addItem(106004, 100);
        }else if(ID == 85){
            GM_I.addItem(101005, 100);
            GM_I.addItem(105001, 101);
            GM_I.addItem(302002, 102);
        }else if(ID == 86){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[4], EnemyData[0], EnemyData[0]});
        }else if(ID == -86){
            GM_I.addItem(101005, 100);
            GM_I.addItem(105001, 101);
            GM_I.addItem(302002, 102);
        }else if(ID == 87){
            if(Random.Range(0,2) == 0){
                GM_I.addItem(105001, 100);
                GM_I.addItem(302002, 101);
            }else{
                GM_I.addItem(101005, 100);
                GM_I.addItem(302002, 101);
            }
        }else if(ID == 90){
            GM_I.addItem(101008, 100);
            GM_I.addItem(302001, 101);
            GM_I.addItem(302001, 102);
            GM_I.addItem(106001, 103);
        }else if(ID == 92){
            if(GM_I.GetItemDataList().Count(item=> item.getID() == 104001 && item.getSortNo()>=6) >= 1){
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 104001 && item.getSortNo()>=6).getSortNo() );
            }else{
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 104001).getSortNo() );
            }
        }else if(ID == 112){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[5], EnemyData[5]});
        }else if(ID == -112){
            GM_I.addItem(101008, 100);
            GM_I.addItem(302001, 101);
            GM_I.addItem(302001, 102);
            GM_I.addItem(106001, 103);
        }else if(ID == 98){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[6], EnemyData[1], null});
        }else if(ID == -98){
            GM_I.addItem(302001, 100);
            GM_I.addItem(102008, 101);
        }else if(ID == 100){
            GM_I.addItem(101002, 100);
            GM_I.addItem(106002, 101);
        }else if(ID == 103){
            if(GM_I.GetItemDataList().Count(item=> item.getID() == 104001 && item.getSortNo()>=6) >= 1){
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 104001 && item.getSortNo()>=6).getSortNo() );
            }else{
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 104001).getSortNo() );
            }
            GM_I.addItem(101007, 100);
        }else if(ID == 104){
            if(GM_I.GetItemDataList().Count(item=> item.getID() == 104001 && item.getSortNo()>=6) >= 1){
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 104001 && item.getSortNo()>=6).getSortNo() );
            }else{
                GM_I.DeleteItem( GM_I.GetItemDataList().First(item=> item.getID() == 104001).getSortNo() );
            }
        }else if(ID == 107){
            GM_I.addItem(106004, 100);
            GM_I.addItem(106004, 101);
            GM_I.addItem(105001, 102);
        }else if(ID == 109){
            EventAllDamage(2);
        }else if(ID == 114){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[4], EnemyData[0], null});
        }else if(ID == -114){
            EventAllHeal(1);
        }else if(ID == 115){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
        }else if(ID == 1000){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[2], EnemyData[5], EnemyData[5]});
        }else if(ID == -1000){
            GM_I.addItem(302002, 100);
            GM_I.addItem(302002, 101);
            GM_I.addItem(302002, 102);
        }else if(ID == 1004){
            GM_I.addItem(101004, 100);
            GM_I.addItem(106004, 101);
        }else if(ID == 1005){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[7], EnemyData[5], null});
        }else if(ID == -1005){
            GM_I.addItem(101004, 100);
            GM_I.addItem(106004, 101);
        }else if(ID == 1006){
            GM_I.addItem(302001, 100);
        }else if(ID == 1007){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[7], EnemyData[5], EnemyData[5]});
        }else if(ID == 1009){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
        }else if(ID == 1013){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[6], EnemyData[8], null});
        }else if(ID == 1016){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[7], EnemyData[7], null});
        }else if(ID == -1016){
            GM_I.addItem(105001, 100);
            GM_I.addItem(106002, 101);
        }else if(ID == 1017){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[7], EnemyData[8], null});
        }else if(ID == -1017){
            if(Random.Range(0,2) == 0){
                GM_I.addItem(101007, 100);
            }else{
                GM_I.addItem(101008, 100);
            }
        }else if(ID == 1020){
            GM_I.addItem(201001, 100);
        }else if(ID == 1022){
            GM_I.addItem(302001, 101);
            GM_I.addItem(106001, 102);
        }else if(ID == 1024){
            GM_I.addItem(102006, 103);
        }else if(ID == 1026){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[6], EnemyData[7], EnemyData[8]});
        }else if(ID == 1027){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[7], EnemyData[8]});
        }else if(ID == 1028){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[7], null});
        }else if(ID == 1029){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
            GM_I.addItem(302001, 102);
        }else if(ID == 1030){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[5], null});
        }else if(ID == 1032){
            EventAllHeal(1);
        }else if(ID == 1034){
            GM_I.addItem(104001, 100);
        }else if(ID == 1035){
            GM_I.addItem(106002, 100);
            GM_I.addItem(106004, 100);
        }else if(ID == 1037){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[7], EnemyData[5], EnemyData[5]});
        }else if(ID == 1039){
            EventFlag.Add("diversion");
        }else if(ID == 1040){
            GM_I.addItem(101002, 100);
            GM_I.addItem(101002, 101);
            GM_I.addItem(101002, 102);
        }else if(ID == 1043){
            GM_I.addItem(201001, 100);
        }else if(ID == 1044){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[8], EnemyData[8], null});
        }else if(ID == -1044){
            GM_I.addItem(201001, 100);
        }else if(ID == 1048){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[6], EnemyData[8], null});
        }else if(ID == 1049){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[5], EnemyData[5]});
        }else if(ID == -1049){
            GM_I.addItem(106004, 100);
            GM_I.addItem(106004, 101);
        }else if(ID == 1054){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
            GM_I.addItem(102007, 102);
        }else if(ID == 1055){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[5], EnemyData[6], null});
        }else if(ID == -1055){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
            GM_I.addItem(102007, 102);
        }else if(ID == 1057){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[10], EnemyData[9], EnemyData[9]});
        }else if(ID == -1057){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
            GM_I.addItem(102009, 102);
        }else if(ID == 1060){
            EventRandomOneDamage(2);
        }else if(ID == 1063){
            GM_I.addItem(302002, 100);
            GM_I.addItem(104001, 101);
        }else if(ID == 1064){
            EventAllDamage(2);
        }else if(ID == 1067){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[10], EnemyData[10], EnemyData[12]});
        }else if(ID == -1067){
            GM_I.addItem(302001, 100);
            GM_I.addItem(302001, 101);
            if(Random.Range(0,2) == 0){
                GM_I.addItem(101009, 102);
            }else{
                GM_I.addItem(101010, 102);
            }
        }else if(ID == 1068){
            OverrideNextFloorEvent(GM_M.currentHierarchy, GM_M.currentLocale, 1069, 2);
        }else if(ID == 1069){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[9], EnemyData[5], EnemyData[5]});
        }else if(ID == -1069){
            EventAllHeal(1);
        }else if(ID == 1070){
            GM.AppearEnemy(new List<GameObject>(){EnemyData[11], EnemyData[12], EnemyData[10]});
        }else if(ID == 1071){
            AddMember(PlayerData[0]);
        }else if(ID == 1072){
            GM_I.addItem(106003, 100);
            GM_I.addItem(104001, 101);
        }else if(ID == 1073){
            EventAllHeal(5);
        }
        
        else if(ID == 100000){
            GM_T.unTimeStop(true);
            SceneManager.LoadScene("Title");
        }

        return false;
    }

}