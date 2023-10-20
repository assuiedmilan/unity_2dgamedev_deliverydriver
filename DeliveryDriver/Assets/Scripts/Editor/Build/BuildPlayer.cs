using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Unity.DeliveryDriver.Editor.Build
{
	public static class DirectoryInfoExtensions 
    { 
        public static void DeepCopy(this DirectoryInfo directory, string destinationDir) 
        { 
            Debug.Log($"Copying build from {directory} to {destinationDir}");
            
            foreach (var dir in Directory.GetDirectories(directory.FullName, "*", SearchOption.AllDirectories)) 
            {
                var dirToCreate = dir.Replace(directory.FullName, destinationDir); 
                Directory.CreateDirectory(dirToCreate); 
            } 
            
            foreach (string newPath in Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories)) 
            { 
                File.Copy(newPath, newPath.Replace(directory.FullName, destinationDir), true); 
            } 
        } 
    }
	
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
        /// Since UCB has unpredictable build output folder, the build will also be copied (do not move, for archive) at a pre-defined location.
        /// This allows to generate and archive a custom build on UCB as an artifact.
        /// </summary>
        [UsedImplicitly]
        public static void ReplaceUcDefaultBuildByBatchModeBuild(string pathToOriginalPlayer)
        {
            Debug.Log($"Received path to existing UCB Build: {pathToOriginalPlayer}");
            var buildOptions = BuildPlayer(pathToOriginalPlayer);
            
            var ucbBuildLocation = Directory.GetParent(pathToOriginalPlayer);
            var finalBuildLocation = ComputeBuildOutputFolder(buildOptions);
            
            ucbBuildLocation.DeepCopy(finalBuildLocation);
        }
        
        [UsedImplicitly]
        [MenuItem("DeliveryDriver/Build Player")]
        public static void RunBatchModeBuild()
        {
            BuildPlayer();
        }
        
        static BuildPlayerOptions BuildPlayer(string existingPlayerLocation = null)
        {
            var options = ProcessBuildOptions(existingPlayerLocation);
            BuildPipeline.BuildPlayer(options);
            return options;
        }
        
        private static string ComputeBuildOutputFolder(BuildPlayerOptions buildOptions) {
            var locationPathName = GetBuildPath(buildOptions);
            CreateBuildFolder(locationPathName);
            
            return locationPathName;
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
                buildOptions.locationPathName = ComputeBuildOutputFolder(buildOptions);
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