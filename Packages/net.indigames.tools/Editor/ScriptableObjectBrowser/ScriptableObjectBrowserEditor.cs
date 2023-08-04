using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;
using Object = UnityEngine.Object;

namespace ScriptableObjectBrowser
{
    public abstract class ScriptableObjectBrowserEditor
    {
        public ScriptableObjectBrowser browser;
        protected Editor cachedEditor = null;

        public virtual void SetTargetObjects(Object[] objs)
        {
        }

        public virtual void RenderInspector()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory">Directory of input file</param>
        /// <param name="callback">callback for adding new ScriptableObject to ListView</param>
        public virtual void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
        }

        public bool CreateDataFolder { get; protected set; }

        public string DefaultStoragePath { get; protected set; }

        public GenericMenu ContextMenu { get; protected set; }
    }

    public abstract class ScriptableObjectBrowserEditor<T> : ScriptableObjectBrowserEditor where T : Object
    {
        T targetObject;

        public override void SetTargetObjects(Object[] objs)
        {
            if (objs == null || objs.Length <= 0) targetObject = null;
            else targetObject = (T)objs[0];

            Editor.CreateCachedEditor(objs, null, ref cachedEditor);
            if (cachedEditor != null) cachedEditor.ResetTarget();
        }

        public override void RenderInspector()
        {
            if (targetObject == null) return;
            CustomInspector(cachedEditor.serializedObject);
        }

        private void DrawDefaultInspector()
        {
            cachedEditor.OnInspectorGUI();
        }

        protected virtual void CustomInspector(SerializedObject obj)
        {
            DrawDefaultInspector();
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
        }
    }
}