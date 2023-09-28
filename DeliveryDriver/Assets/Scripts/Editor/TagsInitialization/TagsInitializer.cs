using CodeBox.Tools.Editor.Interfaces;
using CodeBox.Tools.Editor.Tags;
using Unity.DeliveryDriver.Editor.Enumerations;
using UnityEditor;

namespace Unity.DeliveryDriver.Editor.TagsInitialization
{
    public static class TagsInitializer
    {
        [InitializeOnLoad]
        public static class Loader
        {
            class InternalEditorUtility : IInternalEditorUtility {}
            static GameTagsManager<GameTags> s_Manager = new(new InternalEditorUtility());
    
            static Loader()
            {
                s_Manager.CleanTags();
                s_Manager.InitializeTags();
            }
        }
    }
}
