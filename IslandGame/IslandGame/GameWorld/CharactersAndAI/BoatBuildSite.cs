﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace IslandGame.GameWorld.CharactersAndAI
{
    [Serializable]
    public class BoatBuildSite : ObjectBuildJobSite
    {

        public BoatBuildSite(BlockLoc newObjectLoc, IslandPathingProfile nProfile)
        {
            objectLoc = newObjectLoc;
            profile = nProfile;
        }

        public override void blockWasBuilt(BlockLoc loc) { }

        public override void blockWasDestroyed(BlockLoc loc) { }

        public override Job getJob(Character newWorker, Ray ray, IslandWorkingProfile workingProfile)
        {
            BlockLoc found = new BlockLoc();
            List<BlockLoc> containsBlockLocTarget = new List<BlockLoc>();
            containsBlockLocTarget.Add(getObjectLoc());

            PathHandler pathHandler = new PathHandler();
            Path path = pathHandler.getPathToMakeTheseBlocksAvaiable(getProfile(),
                new BlockLoc(newWorker.getFootLocation()), getProfile(),
                containsBlockLocTarget, 2,
                out found);
            return new TravelAlongPath(path, new ObjectBuildingJob(newWorker, this));

        }

        public override void draw(GraphicsDevice device, Effect effect, DisplayParameters parameters)
        {
            WorldMarkupHandler.addCharacter(ContentDistributor.getEmptyString()+@"boats\greenOnePersonBoat.chr",
            objectLoc.toWorldSpaceVector3() + new Vector3(.5f, 0, .5f),
            IslandGame.GameWorld.Boat.SCALE, .5f);
        }

        public override HashSet<BlockLoc> getAllBlocksInSite()
        {
            return new HashSet<BlockLoc>();
        }
    }
}
