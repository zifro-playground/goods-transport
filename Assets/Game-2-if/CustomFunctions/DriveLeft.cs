using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class DriveLeft : ClrYieldingFunction
{
    public DriveLeft() : base("kör_vänster")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        SortedQueue.LEFT_QUEUE.Add(CarQueue.GetFirstCar());

        CarQueue.DriveQueueForward();
        CarQueue.DriveFirstCarLeft();
		SceneController2_2.carsSorted++;
    }
}
