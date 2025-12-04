using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameClientState { Lobby, Matching, Loading, InGame, Result }

public static class ClientState
{
    public static GameClientState State { get; private set; } = GameClientState.Lobby;
    public static void Set(GameClientState s) { State = s; Debug.Log($"[State] {s}"); }
}

