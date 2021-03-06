﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IslandGame.GameWorld
{
    [Serializable]
    class FetchResourceJob : MultiBlockOngoingJob
    {
        
        IslandWorkingProfile workingProfile;
        Character character;
        ResourceBlock.ResourceType typeToFetch;




        public FetchResourceJob(IslandWorkingProfile nworkingProfile, ResourceBlock.ResourceType nTypeToFetch,
            Character nCharacter, Job njobToReturnTo)
        {
            toReturnTo = njobToReturnTo;
            workingProfile = nworkingProfile;
            typeToFetch = nTypeToFetch;
            character = nCharacter;
            setJobType(JobType.none);

        }



        public override CharacterTask.Task getCurrentTask(CharacterTaskTracker taskTracker)
        {

            List<BlockLoc> goalsForBlockPickup = workingProfile.getBlocksToGetThisTypeFrom(typeToFetch).ToList();
            BlockLoc blockToPlaceResourceIn;
            PathHandler pathHandler = new PathHandler();

            foreach (BlockLoc test in taskTracker.blocksCurrentlyClaimed())
            {
                goalsForBlockPickup.Remove(test);
            }

            if (goalsForBlockPickup.Count > 0)
            {
                Path path = pathHandler.
                    getPathToMakeTheseBlocksAvaiable(workingProfile.getPathingProfile(), new BlockLoc(character.getFootLocation()),
                    workingProfile.getPathingProfile(), goalsForBlockPickup, 2, out blockToPlaceResourceIn);

                targetBlock = blockToPlaceResourceIn;
                TravelAlongPath travel = new TravelAlongPath(path,new PickUpResourceJob(typeToFetch,character,toReturnTo,workingProfile,blockToPlaceResourceIn));
                return new CharacterTask.SwitchJob(travel);
            }
            else
            {
                return new CharacterTask.SwitchJob(new UnemployedJob());
            }
            
        }




        public override bool isComplete()
        {
            return false;
        }

        public override bool isUseable()
        {
            return true;
        }




    }
}
