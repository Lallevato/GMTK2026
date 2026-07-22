using UnityEngine;

public class HolyWaterDrop: Drop
{
    protected override void Effect()
    {
        Debug.Log("Picked up silver bolt");
        AmmoCounter.instance.IncreaseGrenades(1);
        Destroy(gameObject);
    }
}
