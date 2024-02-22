// using System;
// using System.Collections;
// using System.Collections.Generic;
// using GplaySDK.BaseLib.RequireProperty;
// using GplaySDK.Core.BaseLib.Attribute;
// using GplaySDK.Core.BaseLib.Utils;
// using UnityEngine;
//
// public static class Utils
// {
//
//     public static int hintCount
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.GamePLayData.hintCount, 5);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.GamePLayData.hintCount, value);
//     }
//     
//     [CurrentLevel]
//     public static int levelIndex
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.GamePLayData.levelIndex, 0);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.GamePLayData.levelIndex, value);
//     }
//     
//     public static int sound
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.GamePLayData.sound, 1);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.GamePLayData.sound, value);
//     }
//     
//     public static int music
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.GamePLayData.music, 1);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.GamePLayData.music, value);
//     }
//     
//     public static int BGindex
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.GamePLayData.BGindex, 0);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.GamePLayData.BGindex, value);
//     }
//     
//     public static bool puzzleShowed
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.GamePLayData.puzzleShowed, 0) == 1;
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.GamePLayData.puzzleShowed, value ? 1 : 0);
//     }
//     
//     
//     public static void Save()
//     {
//         LocalStorageUtils.Custom.Save();
//     }
//     
//     [IsRemoveAds]
//     public static bool IsRemoveAds
//     {
//         get => LocalStorageUtils.Custom.GetBool(StringHelper.SdkAttribute.IsRemoveAdsAttribute, false);
//         set => LocalStorageUtils.Custom.SetBool(StringHelper.SdkAttribute.IsRemoveAdsAttribute, value);
//     }
//     [GdprValue]
//     public static bool GdprValue
//     {
//         get => LocalStorageUtils.Custom.GetBool(StringHelper.SdkAttribute.GdprValueAttribute, false);
//         set => LocalStorageUtils.Custom.SetBool(StringHelper.SdkAttribute.GdprValueAttribute, value);
//     }
//     [MinimumLevelShowInterstitial]
//     public static int MinimumLevelShowInterstitial
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.SdkAttribute.MinimumLevelShowInterstitialAttribute, 2);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.SdkAttribute.MinimumLevelShowInterstitialAttribute, value);
//     }
//     [Country]
//     public static string Country
//     {
//         get => LocalStorageUtils.Custom.GetString(StringHelper.SdkAttribute.CountryAttribute, "global");
//         set => LocalStorageUtils.Custom.SetString(StringHelper.SdkAttribute.CountryAttribute, value);
//     }
//     [CurrentLevelMode]
//     public static string CurrentLevelMode
//     {
//         get => LocalStorageUtils.Custom.GetString(StringHelper.SdkAttribute.CurrentLevelModeAttribute, "global");
//         set => LocalStorageUtils.Custom.SetString(StringHelper.SdkAttribute.CurrentLevelModeAttribute, value);
//     }
//     [DeviceId]
//     public static String DeviceId
//     {
//         get => LocalStorageUtils.Custom.GetString(StringHelper.SdkAttribute.DeviceIdAttribute, SystemInfo.deviceUniqueIdentifier);
//         set => LocalStorageUtils.Custom.SetString(StringHelper.SdkAttribute.DeviceIdAttribute, value);
//     }
//     [InterstitialCooldown]
//     public static int InterstitialCooldown
//     {
//         get => LocalStorageUtils.Custom.GetInt(StringHelper.SdkAttribute.InterstitialCooldownAttribute, 60);
//         set => LocalStorageUtils.Custom.SetInt(StringHelper.SdkAttribute.InterstitialCooldownAttribute, value);
//     }
//     
// }
