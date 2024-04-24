using System;
using System.Collections.Generic;

public enum GameEvent
{
    OnGameStarted,
    OnEnableItemInteraction,
    OnDisableItemInteraction,
    OnPlayerHit,
    OnPlayerDead,
    OnEnemyKill,
    OnMaxHealthSet,
    OnHealthUpdate,
    OnXPSet,
    OnXPUpdate,
    OnAmmoTextUpdate,
    OnKillCounterText,
    OnUIOpening,
    OnUIClosing,
    OnLevelUp,
    OnHighScoreUpdate
}

public static class EventManager
{
    private static Dictionary<GameEvent, Action<object>> eventTable
        = new Dictionary<GameEvent, Action<object>>();

    public static void AddHandler(GameEvent gameEvent, Action<object> action)
    {
        if (!eventTable.ContainsKey(gameEvent)) eventTable[gameEvent] = action;
        else eventTable[gameEvent] += action;
    }

    public static void RemoveHandler(GameEvent gameEvent, Action<object> action)
    {
        if (eventTable[gameEvent] != null)
            eventTable[gameEvent] -= action;
        if (eventTable[gameEvent] == null)
            eventTable.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent eventt, object value)
    {
        if (eventTable[eventt] != null)
            eventTable[eventt](value);
    }

}