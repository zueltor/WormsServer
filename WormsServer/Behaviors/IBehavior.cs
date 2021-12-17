using WormsServer.Models;
using WormsServer.Responses;

namespace WormsServer.Behaviors
{
    public interface IBehavior
    {
        IResponse DoSomething(IWorld world, IWorm worm, int step = 0, int run = 0);

        public static WormAction ParseResponse(IResponse response)
        {
            var action = new WormAction();
            if (response is ResponseNothing)
            {
                action.Direction = "Up";
                action.Split = false;
            }

            action.Direction = response.Step switch
            {
                { X: 1, Y: 0 } => "Right",
                { X: -1, Y: 0 } => "Left",
                { X: 0, Y: 1 } => "Up",
                { X: 0, Y: -1 } => "Down",
                _ => "Up"
            };

            action.Split = response is ResponseReproduce;
            return action;
        }
    }
}