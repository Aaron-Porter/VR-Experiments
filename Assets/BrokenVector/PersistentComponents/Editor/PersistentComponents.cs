using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

namespace BrokenVector.PersistentComponents
{

    public class PersistentComponents
    {
        [MenuItem("GameObject/Make Persistent", false, 0)]
        public static void TogglePersistentGO(MenuCommand command)
        {
            var go = command.context as GameObject;
            if (go == null)
                return;

            Instance.WatchComponents(go.GetComponents<Component>());
            PersistentComponentsWindow.ShowWindow();
        }

        [MenuItem("CONTEXT/Component/Toggle Persistent", false, 50)]
        public static void TogglePersistent(MenuCommand command)
        {
            var target = (Component)command.context;
            if (Instance.IsComponentWatched(target))
            {
                Instance.ForgetComponent(target);
            }
            else
            {
                Instance.WatchComponent(target);
            }
            PersistentComponentsWindow.ShowWindow();
        }

        private static PersistentComponents instance;
        public static PersistentComponents Instance
        {
            get
            {
                if (instance == null)
                    instance = new PersistentComponents();
                return instance;
            }
        }

        public Dictionary<GameObject, List<Component>> WatchedComponents { get { return components; } }

        private Dictionary<GameObject, List<Component>> components = new Dictionary<GameObject, List<Component>>();
        private Dictionary<int, SerializedObject> serializedObjects = new Dictionary<int, SerializedObject>();

        private PersistencyData persistencyData;

        public PersistentComponents()
        {
            EditorApplication.playmodeStateChanged += OnPlaymodeChanged;

            //Has to be initialized regardless of ComponentsStayPersistent Setting
            persistencyData = AssetDatabase.LoadAssetAtPath<PersistencyData>(PersistencyData.GetAssetLocation());
            if (persistencyData == null)
                persistencyData = PersistencyData.CreateInstance(new Component[0]);

            if (Settings.ComponentsStayPersistent)
            {
                foreach(var comp in persistencyData.persistentComponents)
                {
                    if (comp == null)
                        continue;
                    if (!components.ContainsKey(comp.gameObject))
                        components[comp.gameObject] = new List<Component>();

                    components[comp.gameObject].Add(comp);
                }
            }
        }

        public void OnPlaymodeChanged()
        {
            if (EditorApplication.isPlaying)
            {
                UpdateAllComponents();
            }
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                OnExitPlayMode();
            }

            RememberComponents();
        }

        private void OnExitPlayMode()
        {
            Instance.ApplyModifiedProperties();
        }

        private void RememberComponents()
        {
            if (Settings.ComponentsStayPersistent)
            {
                List<Component> saveData = new List<Component>();
                foreach (var pair in components)
                    foreach (var comp in pair.Value)
                        if (comp != null)
                            saveData.Add(comp);

                persistencyData.persistentComponents = saveData.ToArray();
                persistencyData.Save();
            }
            else
            {
                ForgetEveryComponent();
            }
        }

        public void ApplyModifiedProperties()
        {
            List<int> remove = new List<int>();
            foreach (var so in serializedObjects)
            {
                if (so.Value.targetObject != null)
                    so.Value.ApplyModifiedProperties();
                else
                    remove.Add(so.Key);
            }
            foreach(var k in remove)
            {
                serializedObjects.Remove(k);
            }
        }

        public void ForgetComponent(Component comp)
        {
            components[comp.gameObject].Remove(comp);
            if (components[comp.gameObject].Count == 0)
                components.Remove(comp.gameObject);

            serializedObjects.Remove(comp.GetInstanceID());

            if (PersistentComponentsWindow.Instance != null)
                PersistentComponentsWindow.Instance.Repaint();
        }
        public void ForgetComponents(params Component[] comps)
        {
            foreach (var c in comps)
                ForgetComponent(c);
        }
        public void ForgetEveryComponent()
        {
            List<Component> toForget = new List<Component>();
            foreach (var pair in components)
                toForget.AddRange(pair.Value);

            ForgetComponents(toForget.ToArray());
        }

        public void WatchComponent(Component comp)
        {
            if (IsComponentWatched(comp))
                return;

            if (!components.ContainsKey(comp.gameObject))
                components[comp.gameObject] = new List<Component>();
            components[comp.gameObject].Add(comp);

            UpdateComponent(comp);

            if (PersistentComponentsWindow.Instance != null)
                PersistentComponentsWindow.Instance.Repaint();
        }
        public void WatchComponents(params Component[] comps)
        {
            foreach (var c in comps)
                WatchComponent(c);
        }

        public bool IsComponentWatched(Component comp)
        {
            return (components.ContainsKey(comp.gameObject) && components[comp.gameObject].Contains(comp));
        }

        public void UpdateComponent(Component comp)
        {
            if (!IsComponentWatched(comp))
                return;

            var instanceId = comp.GetInstanceID();
            if (!serializedObjects.ContainsKey(instanceId))
            {
                serializedObjects.Add(instanceId, new SerializedObject(comp));
            }

            var clone = new SerializedObject(comp);
            var original = serializedObjects[instanceId];

            SerializedProperty sp = clone.GetIterator();
            while (sp.NextVisible(true))
            {
                original.CopyFromSerializedProperty(sp);
            }
            sp.Reset();
        }
        public void UpdateComponents(params Component[] comps)
        {
            foreach (var c in comps)
                UpdateComponent(c);
        }
        public void UpdateAllComponents()
        {
            foreach (var go in components)
                foreach(var component in go.Value)
                    UpdateComponent(component);
        }

    }

}
