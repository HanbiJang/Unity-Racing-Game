using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState instance; //Singleton

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameState>();
                if (instance == null)
                {
                    GameObject tmp = new GameObject();
                    tmp.name = typeof(GameState).Name;
                    instance = tmp.AddComponent<GameState>();
                }
            }
            DontDestroyOnLoad(instance);
            return instance;
        }
    }

    //string ip = "10.88.164.52";
    string ip = "127.0.0.1";
    int portNum = 8888;

    ulong userId;
    ulong roomId;
    int userCount;
    List<KeyValuePair<ulong, ulong>> scoreLIst; //int,int는 :userID, Score순이다


    public ulong UserId { get { return userId; } set { userId = value; } }
    public ulong RoomId { get { return roomId; } set { roomId = value; } }

    public int UserCount { get { return userCount; } set { userCount = value; } }
    public List<KeyValuePair<ulong, ulong>> ScoreLIst { get { return scoreLIst; } set { scoreLIst = value; } }

    public string Ip { get { return ip; } set { ip = value; } }
    public int PortNum { get { return portNum; } set { portNum = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
