using System;
using WormsServer;
using WormsServer.Behaviors;
using WormsServer.Responses;

namespace WorldOfWorms.Behaviors
{
    internal class BehaviorEatFood : IBehavior
    {
        public IResponse DoSomething(IWorld world, IWorm worm, int step = 0, int run = 0)
        {
            var foodPosition = FindClosestFoodPosition(world, worm);
            var moveStep = GetMoveStep(foodPosition, worm);
            if (moveStep.IsNothing())
            {
                return new ResponseNothing();
            }

            if (worm.Lifeforce > IWorm.LIFEFORCE_TO_REPRODUCE)
            {
                return new ResponseReproduce(moveStep);
            }

            return new ResponseMove(moveStep);
        }

        private static Position GetMoveStep(Position foodPosition, IWorm worm)
        {

            if (foodPosition.Equals(Position.InvalidPosition()))
            {
                return new Position(0, 0);
            }

            if (worm.WormPosition.X < foodPosition.X)
            {
                return new Position(1, 0);
            }

            if (worm.WormPosition.X > foodPosition.X)
            {
                return new Position(-1, 0);
            }

            if (worm.WormPosition.Y < foodPosition.Y)
            {
                return new Position(0, 1);
            }

            return new Position(0, -1);
        }

        private static Position FindClosestFoodPosition(IWorld world, IWorm worm)
        {
            int minDistance = Int32.MaxValue;
            Position foodPosition = Position.InvalidPosition();
            foreach (var food in world.Foods)
            {
                var distance = worm.WormPosition.Distance(food.FoodPosition);
                if (distance < minDistance)
                {
                    foodPosition = food.FoodPosition;
                    minDistance = distance;
                }
            }

            return foodPosition;
        }
    }
}