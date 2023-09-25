using System;
using Unity.DeliveryDriver.Editor.Enumerations;
using Unity.DeliveryDriver.Editor.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Unity.DeliveryDriver.Editor.Tags {
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

    public class GameTagsManager<T> where T : IComparable, IFormattable, IConvertible
    {
        IInternalEditorUtility m_InternalEditorUtility;
    
        public GameTagsManager(IInternalEditorUtility internalEditorUtility)
        {
            m_InternalEditorUtility = internalEditorUtility;
            EnumHelper.VerifyTypeIsEnum<T>();
        }
   
        public void CleanTags()
        {
            foreach (var tag in m_InternalEditorUtility.GetAllTags())
            {
                if (!Enum.IsDefined(typeof(UnityTags), tag))
                {
                    m_InternalEditorUtility.RemoveTag(tag);
                }
            }
        }

        public void InitializeTags()
        {
            foreach (var tag in (T[])Enum.GetValues(typeof(T)))
            {
                m_InternalEditorUtility.AddTag(tag.GetEnumAsString());
            }
        }
    }
}