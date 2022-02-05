using UnityEngine;

public class MonkZenState : PlayerState
{
    public float MoveFactor = 0.90f;

    public override void OnUpdateState()
    {
        player.MoveHorizontally(MoveFactor);
    }
}
