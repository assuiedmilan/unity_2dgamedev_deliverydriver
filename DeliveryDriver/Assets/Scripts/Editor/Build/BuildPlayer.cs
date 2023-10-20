using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Unity.DeliveryDriver.Editor.Build
{
    public static class BuildSetup
    {
        static string playerName
        {
            get
            {
                return Application.platform switch
                {
                    RuntimePlatform.WindowsEditor => "delivery_driver_batchmode_build.exe",
                    _ => throw new Exception($"Unsupported platform {Application.platform}")
                };
            }
        }
        
        static string defaultBuildPath => Path.Combine(Application.dataPath, "..", "build", playerName);

        /// <summary>
        /// Method <c>ReplaceUcDefaultBuildByBatchModeBuild</c> generate the custom build and replace the existing UCB build with the updated one.
        /// This allows to generate and archive a custom build on UCB as an artifact.
        /// </summary>
        [UsedImplicitly]
        public static void ReplaceUcDefaultBuildByBatchModeBuild(string pathToOriginalPlayer)
        {
            Debug.Log($"Received path to existing UCB Build: {pathToOriginalPlayer}");
            BuildPlayer(pathToOriginalPlayer);
        }
        
        [UsedImplicitly]
        [MenuItem("DeliveryDriver/Build Player")]
        public static void RunBatchModeBuild()
        {
            BuildPlayer();
        }
        
        static void BuildPlayer(string existingPlayerLocation = null)
        {
            BuildPipeline.BuildPlayer((ProcessBuildOptions(existingPlayerLocation)));
        }
        
        static BuildPlayerOptions ProcessBuildOptions(string existingPlayerLocation = null)
        {
            var buildOptions = new BuildPlayerOptions();
            
            buildOptions.assetBundleManifestPath = "";
            buildOptions.scenes = EditorBuildSettings.scenes.Select(p => p.path).ToArray();
            buildOptions.scenes[0] = "Assets/Scenes/SampleScene2.unity";
            buildOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildOptions.targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            
            if (existingPlayerLocation == null)
            {
                buildOptions.locationPathName = GetBuildPath(buildOptions);
                CreateBuildFolder(buildOptions.locationPathName);
            }
            else
            {
                buildOptions.locationPathName = existingPlayerLocation;
            }
            
            Debug.Log($"Target is {buildOptions.target}, group {buildOptions.targetGroup} and location should be {buildOptions.locationPathName}");
            
            return buildOptions;
        }
        
        static void CreateBuildFolder(string locationPathName)
        {
            var directoryToManipulate = Path.GetDirectoryName(locationPathName);
            
            try
            {
                if (null == directoryToManipulate)
                {
                    throw new NullReferenceException("Build target directory is null, exiting, player will not be built");
                }
                
                if (Directory.Exists(directoryToManipulate))
                {
                    Directory.Delete(directoryToManipulate, true);
                    Debug.Log($"Removing directory {directoryToManipulate}");
                }

                Directory.CreateDirectory(directoryToManipulate);
            }
            catch (Exception ex)
            {
                Debug.Log($"An error occurred during build folder creation: {ex.Message}");
            }
        }
        
        static string GetBuildPath(BuildPlayerOptions options)
        {
            var locationPathName = EditorUserBuildSettings.GetBuildLocation(options.target);
            
            return string.IsNullOrEmpty(locationPathName) ? defaultBuildPath : Path.Combine(Path.GetDirectoryName(locationPathName) ?? string.Empty, playerName);
        }
    }
}