﻿using System;
using System.Collections.Generic;
using System.Linq;
using WorldOfWorms.Behaviors;
using WormsServer.Responses;

namespace WormsServer.Behaviors
{
    public class BehaviorSmart : IBehavior
    {
        private int totalSteps = 100;
        private int maxLifeforceWithoutKids = 60;
        private int stepsToReduceReproducing = 30;
        //public static Dictionary<IWorm, Position> wormFoodDictionary = new();
        public static int currentRun = -1;
        public static Dictionary<int, int> RunStepDictionary = new();
        //public static Dictionary<int, Dictionary<IWorm, Position>> wormFoodDictionary2 = new();
        public static List<int> runs=new();
        //public static List<int>

        public IResponse DoSomething(IWorld world, IWorm worm, int currentStep = 0, int run = 0)
        {
            // if (RunStepDictionary.ContainsKey(run))
            // {
            //     if (currentStep < RunStepDictionary[run])
            //     {
            //         wormFoodDictionary2[run].Clear();
            //     }
            // }
            // RunStepDictionary[run] = currentStep;

            var foods = world.Foods.ToList();
            // if (wormFoodDictionary2.ContainsKey(run)&&wormFoodDictionary2[run].ContainsKey(worm))
            // {
            //     wormFoodDictionary2[run].Remove(worm);
            // }

            Position foodPosition;
            int distance;
            while (true)
            {
                foodPosition = FindClosestFoodPosition(foods, worm);
                var food = foods.FirstOrDefault(f => f.FoodPosition.Equals(foodPosition));
                if (food is not null)
                {
                    foods.Remove(food);
                    distance = foodPosition.Distance(worm.WormPosition);
                    if (food.Lifeforce < distance ||
                        worm.Lifeforce < distance)
                    {
                        continue;
                    }

                    break;
                }

                // if (foodPosition.Equals(Position.InvalidPosition()) || !wormFoodDictionary2.ContainsKey(run)||
                //     !wormFoodDictionary2[run].ContainsValue(foodPosition))
                // {
                //     break;
                // }
            }

            if (foodPosition.Equals(Position.InvalidPosition()))
            {
                //foodPosition = new Position(0, 0);
                foodPosition = FindEmptiestZoneDirection(world);
            }
            else
            {
                // if (!wormFoodDictionary2.ContainsKey(run))
                // {
                //     wormFoodDictionary2[run] = new();
                // }
                //
                // wormFoodDictionary2[run][worm] = foodPosition;
            }


            var moveStep = GetMoveStep(foodPosition, worm);
            //distance = foodPosition.Distance(worm.WormPosition);
            var stepsLeft = totalSteps - currentStep-2;
            var newPosition = new Position(moveStep.X + worm.WormPosition.X, moveStep.Y + worm.WormPosition.Y);
            //stepsLeft = stepsLeft > 0 ? stepsLeft : 1;
            if (IsBlockedByWorm(world, newPosition))
            {
                moveStep = FindAvailableStep(world, worm.WormPosition);
                newPosition = new Position(moveStep.X + worm.WormPosition.X, moveStep.Y + worm.WormPosition.Y);
            }

            if (currentStep < stepsToReduceReproducing && worm.Lifeforce > IWorm.LIFEFORCE_TO_REPRODUCE+8 ||
                currentStep >= stepsToReduceReproducing && worm.Lifeforce > maxLifeforceWithoutKids ||
                worm.Lifeforce / IWorm.LIFEFORCE_TO_REPRODUCE > stepsLeft)
            {
                if (!moveStep.IsNothing()) //not blocked by worms
                {
                    // if (world.Worms.Count > 9&& worm.Lifeforce / Worm.LIFEFORCE_TO_REPRODUCE <= stepsLeft)
                    // {
                    //     return new ResponseMove(moveStep);
                    // }

                    if (worm.Lifeforce <= IWorm.LIFEFORCE_TO_REPRODUCE)
                    {
                        return new ResponseMove(moveStep);
                    }
                   // wormFoodDictionary2[run].Remove(worm);
                    if (IsBlockedByFood(world, newPosition))
                    {
                        if (worm.Lifeforce / IWorm.LIFEFORCE_TO_REPRODUCE > stepsLeft)
                        {
                            var freeStep = FindCleanStep(world, worm.WormPosition);
                            if (!freeStep.IsNothing())
                            {
                                return new ResponseReproduce(freeStep);
                            }
                        }

                        return new ResponseMove(moveStep); //cant reproduce because of food
                    }

                    return new ResponseReproduce(moveStep); //can reproduce here
                }

                return new ResponseNothing();

                // moveStep = FindAvailableStep(world, worm.WormPosition);
                // if (moveStep.IsNothing())
                // {
                //     return new ResponseNothing();
                // }
                //
                // return new ResponseReproduce(moveStep);
            }

            if (moveStep.IsNothing())
            {
                return new ResponseNothing();
            }

            return new ResponseMove(moveStep);
        }

        private static bool IsBlockedByFood(IWorld world, Position position)
        {
            return world.Foods.Any(f => f.FoodPosition.Equals(position));
        }

        private static Position FindEmptiestZoneDirection(IWorld world)
        {
            var q = new List<int> { 0, 0, 0, 0 };
            foreach (var worm in world.Worms)
            {
                if (worm.WormPosition.X < 0 && worm.WormPosition.Y <= 0)
                {
                    q[0]++;
                }

                if (worm.WormPosition.X <= 0 && worm.WormPosition.Y > 0)
                {
                    q[1]++;
                }

                if (worm.WormPosition.X > 0 && worm.WormPosition.Y >= 0)
                {
                    q[2]++;
                }

                if (worm.WormPosition.X >= 0 && worm.WormPosition.Y < 0)
                {
                    q[3]++;
                }
            }

            int x;
            int y;
            int baseX = 2;
            int baseY = 2;
            int max = q.Max();
            if (q[0] == max)
            {
                x = -baseX;
                y = -baseY;
            }
            else if (q[1] == max)
            {
                x = baseX;
                y = -baseY;
            }
            else if (q[3] == max)
            {
                x = baseX;
                y = baseY;
            }
            else
            {
                x = -baseX;
                y = baseY;
            }

            return new Position(x, y);
        }

        private static bool IsBlockedByWorm(IWorld world, Position position)
        {
            return world.Worms.Any(w => w.WormPosition.Equals(position));
        }

        private static Position FindAvailableStep(IWorld world, Position position)
        {
            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    if (!world.Worms.Any(w => w.WormPosition.Equals(new Position(position.X + x, position.Y + y))))
                    {
                        return new Position(x, y);
                    }
                }
            }

            return new Position(0, 0);
        }

        private static Position FindCleanStep(IWorld world, Position position)
        {
            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    var newPosition = new Position(position.X + x, position.Y + y);
                    if (!world.Worms.Any(w => w.WormPosition.Equals(newPosition)) &&
                        !world.Foods.Any(f => f.FoodPosition.Equals(newPosition)))
                    {
                        return new Position(x, y);
                    }
                }
            }

            return new Position(0, 0);
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

            if (worm.WormPosition.Y > foodPosition.Y)
            {
                return new Position(0, -1);
            }

            return new Position(0, 0);
        }

        private static Position FindClosestFoodPosition(List<IFood> foods, IWorm worm)
        {
            int minDistance = Int32.MaxValue;
            Position foodPosition = Position.InvalidPosition();
            foreach (var food in foods)
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