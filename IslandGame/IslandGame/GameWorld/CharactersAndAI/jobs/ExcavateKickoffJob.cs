﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IslandGame.GameWorld
{
    [Serializable]
    class ExcavateKickoffJob : MultiBlockOngoingJob
    {
        IslandWorkingProfile workingProfile;
        Character character;
        

        public ExcavateKickoffJob(IslandWorkingProfile nworkingProfile, Character nCharacter)
        {
            workingProfile = nworkingProfile;
            character = nCharacter;
            setJobType(JobType.mining);
        }

        public override CharacterTask.Task getCurrentTask(CharacterTaskTracker taskTracker)
        {

            List<BlockLoc> blocksToRemove = workingProfile.getExcavationSite().getBlocksToRemove();
            if (blocksToRemove.Count > 0)
            {
                BlockLoc toDestroy;
                foreach (BlockLoc claimed in taskTracker.blocksCurrentlyClaimed())
                {
                    blocksToRemove.Remove(claimed);

                }
                PathHandler pathHandler = new PathHandlerPreferringHigherBlocks();
                Path path = pathHandler.getPathToMakeTheseBlocksAvaiable(workingProfile.getPathingProfile(),
                    new BlockLoc(character.getFootLocation()), workingProfile.getPathingProfile(),
                    blocksToRemove, 2, out toDestroy);
                TravelAlongPath toSwitchTo = new TravelAlongPath(path, getWaitJobWithReturn(45, new DestroyBlockJob(character, workingProfile, toDestroy)));
                return new CharacterTask.SwitchJob(toSwitchTo);
            }
            else
            {
                return new CharacterTask.SwitchJob(new UnemployedJob());
            }

        }


        public override bool isComplete()
        {
            return workingProfile.getExcavationSite().getBlocksToRemove().Count == 0;
        }

        public override bool isUseable()
        {
            return true;
        }

    }
}
