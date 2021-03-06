﻿using System;
namespace ProjectWebApi.Models.Project
{
    public class Project
    {
        public Int32 id { get; set; }
        public string subproject { get; set; }
        public string delivery { get; set; }
        public string subDel { get; set; }
        public string name { get; set; }
        public string objective { get; set; }
        public string classification { get; set; }
        public string state { get; set; }

        public string testState { get; set; }
        public bool deployOff { get; set; }
        public bool lossRelease { get; set; }
        public string lossReleaseReason { get; set; }

        public string currentRelease { get; set; }
        public int currentReleaseMonth { get; set; }
        public int currentReleaseYear { get; set; }

        public string clarityRelease { get; set; }
        public int clarityReleaseMonth { get; set; }
        public int clarityReleaseYear { get; set; }

        public string GP { get; set; }
        public string GP_N4 { get; set; }
        public string GP_N3 { get; set; }

        public string LT { get; set; }
        public string LT_N4 { get; set; }
        public string LT_N3 { get; set; }

        public string UN { get; set; }
        public string trafficLight { get; set; }
        public string rootCause { get; set; }
        public string actionPlan { get; set; }
        public string informative { get; set; }
        public string attentionPoints { get; set; }
        public string attentionPointsOfIndicators { get; set; }
        public string iterationsActive { get; set; }
        public string iterationsSelected { get; set; }
        public Int32 total { get; set; }
        public Int32 planned { get; set; }
        public Int32 realized { get; set; }
        public Int32 gap { get; set; }
    }
}