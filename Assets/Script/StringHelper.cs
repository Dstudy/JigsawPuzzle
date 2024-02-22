using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper 
{
    public static class GamePLayData
    {
        public static string hintCount = "hintCount";
        public static string levelIndex = "levelIndex";
        public static string sound = "sound";
        public static string music = "music";
        public static string BGindex = "BGindex";
        public static string puzzleShowed = "puzzleShowed";
    }

    public static class RewardAds
    {
        public static string hintAd = "hintAds";
    }
    
    public static class SdkAttribute
    {
        public const string CheckGdpr = "CheckGdpr";
        public const string IsRemoveAdsAttribute = "IsRemoveAdsAttribute";
        public const string GdprValueAttribute = "GdprValueAttribute";
        public const string MinimumLevelShowInterstitialAttribute = "MinimumLevelShowInterstitialAttribute";
        public const string CountryAttribute = "CountryAttribute";
        public const string CurrentLevelAttribute = "CurrentLevelAttribute";
        public const string CurrentLevelModeAttribute = "CurrentLevelModeAttribute";
        public const string DeviceIdAttribute = "DeviceIdAttribute";
        public const string InterstitialCooldownAttribute = "InterstitialCooldownAttribute";
    }
}
