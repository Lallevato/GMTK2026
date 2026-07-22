using UnityEngine;

public class SilverBoltDrop : Drop
{
    protected override void Effect()
    {
        Debug.Log("Picked up silver bolt");
        AmmoCounter.instance.IncreaseSilverBolts(1);
        Destroy(gameObject);
    }
}
