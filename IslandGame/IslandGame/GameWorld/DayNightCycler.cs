﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IslandGame.GameWorld
{
    class DayNightCycler
    {
        TimeOfDay[] timesOfDay;

        float timeLeftInCurrentTimeOfDayInSeconds = 0;
        int placeInList = 0;

        public DayNightCycler()
        {
            TimeOfDay morning = new TimeOfDay(new Vector4(.498f, .1843f, .0627f, 1), new Vector4(.0863f, .098f, .1569f, 1), .4f, 4);
            TimeOfDay broadDay = new TimeOfDay(new Vector4(.392f, .584f, .733f, 1), new Vector4(.019f, .243f, .549f, 1) * 1.7f, .9f, 4);
            TimeOfDay evening = new TimeOfDay(new Vector4(.694f, .416f, .306f, 1), new Vector4(.0588f, .2706f, .512f, 1), .4f, 4);
            TimeOfDay night = new TimeOfDay(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1), .2f, 4);

            timesOfDay = new TimeOfDay []{ morning, broadDay, evening,night };
        }

        public void update()
        {
            timeLeftInCurrentTimeOfDayInSeconds-= 1/60.0f;
            if (timeLeftInCurrentTimeOfDayInSeconds <= 0)
            {
                incrementPlaceInList();
                timeLeftInCurrentTimeOfDayInSeconds = getCurrentTimeOfDay().getLengthInSeconds();
            }
        }

        private void incrementPlaceInList()
        {
            placeInList = getNextPlaceInListFrom(placeInList);
        }

        private int getNextPlaceInListFrom(int startPos)
        {
            if (startPos >= timesOfDay.Length - 1)
            {
                 return 0;
            }
            else
            {
                return startPos + 1;
            }
        }

        public Vector4 getSkyHorizonColor()
        {
            float ratio = getSkyBlendRatio();
            Vector4 result = blendColors(getNextTimeOfDay().getSkyHorizonColor(), getCurrentTimeOfDay().getSkyHorizonColor(), ratio);

            return result;
        }

        private float getSkyBlendRatio()
        {
            float ratio = (getCurrentTimeOfDay().getLengthInSeconds() - timeLeftInCurrentTimeOfDayInSeconds) / getCurrentTimeOfDay().getLengthInSeconds();
            return ratio;
        }

        private static Vector4 blendColors( Vector4 goalColor,  Vector4 currentColor, float ratio)
        {

            Vector4 result = goalColor - currentColor;
            result *= ratio;
            result += currentColor;
            return result;
        }

        public Vector4 getSkyZenithColor()
        {
            float ratio = getSkyBlendRatio();
            Vector4 result = blendColors(getNextTimeOfDay().getSkyZenithColor(), getCurrentTimeOfDay().getSkyZenithColor(), ratio);

            return result;
        }

        public float getCurrentAmbientBrightness()
        {
            float result = getNextTimeOfDay().getAmbientBrightness() - getCurrentTimeOfDay().getAmbientBrightness();
            result *= getSkyBlendRatio();
            result += getCurrentTimeOfDay().getAmbientBrightness();
            return result;
        }

        private TimeOfDay getCurrentTimeOfDay()
        {
            return timesOfDay[placeInList];
        }

        private TimeOfDay getNextTimeOfDay()
        {
            return timesOfDay[getNextPlaceInListFrom(placeInList)];
        }

    }
}