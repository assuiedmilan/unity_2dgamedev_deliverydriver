using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Unity.DeliveryDriver.Editor.Build
{
    [InitializeOnLoad]
    public static class BuildSetup
    {
        static string playerName
        {
            get
            {
                return Application.platform switch
                {
                    RuntimePlatform.WindowsEditor => "delivery_driver.exe",
                    _ => throw new Exception($"Unsupported platform {Application.platform}")
                };
            }
        }
        
        static string defaultBuildPath => Path.Combine(Application.dataPath, "..", "build", playerName);
        
        static BuildSetup()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(OnBuild);
        }
        
        static void OnBuild(BuildPlayerOptions options)
        {
            BuildPipeline.BuildPlayer(options);
        }
        
        [UsedImplicitly]
        [MenuItem("DeliveryDriver/Build Player")]
        public static void RunBatchModeBuild()
        {
            OnBuild(ProcessBuildOptions());
        }
        
        static BuildPlayerOptions ProcessBuildOptions()
        {
            var buildOptions = new BuildPlayerOptions();
            
            buildOptions.assetBundleManifestPath = "";
            buildOptions.scenes = EditorBuildSettings.scenes.Select(p => p.path).ToArray();
            buildOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildOptions.targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            buildOptions.locationPathName = GetBuildPath(buildOptions);
            
            CreateBuildFolder(buildOptions.locationPathName);
            
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
            
            if (string.IsNullOrEmpty(locationPathName))
            {
                locationPathName = defaultBuildPath;
            }
            
            return locationPathName;
        }
    }
}