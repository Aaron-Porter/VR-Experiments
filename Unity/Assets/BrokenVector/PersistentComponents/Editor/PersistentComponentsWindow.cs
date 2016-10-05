using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BrokenVector.PersistentComponents.Utils;
using UnityEngine;
using UnityEditor;

namespace BrokenVector.PersistentComponents
{
	public class PersistentComponentsWindow : EditorWindow
	{
        // TODO: fix changingvalues from components with custom inspector.. -> dont check for changes, always change.. or sth like that

        public static PersistentComponentsWindow Instance { private set; get; }
	    private static readonly GUIContent removeButtonContent = new GUIContent("X");
	    private static EditorWindow window;

	    private static Texture2D windowIcon;

        private PersistentComponentsWindow()
        {
            Instance = this;
        }

        [MenuItem(Constants.WINDOW_PATH, false, Constants.WINDOW_PRIORITY), MenuItem(Constants.WINDOW_PATH_ALTERNATE)]
        public static void ShowWindow()
        {
            Init();
            window.Show();
        }

	    private static void Init()
	    {
            if (window == null)
                window = GetWindow(typeof(PersistentComponentsWindow));

            if (windowIcon == null)
                windowIcon = Base64.FromBase64(Constants.WINDOW_ICON);

            window.minSize = Constants.WINDOW_MIN_SIZE;
            window.titleContent = new GUIContent(Constants.WINDOW_NAME, windowIcon);
        }

	    private Vector2 currentScrollPos = Vector2.zero;
        void OnGUI()
        {
            Init();

            GUILayout.Space(10);
            EditorGUILayout.LabelField(Constants.CONTENT_TITLE, (GUIStyle)"BoldLabel");
            GUILayout.Space(5);

            if (Application.isPlaying || Settings.ComponentsStayPersistent)
                DrawComponentList();
            else
                GUILayout.TextArea("To mark Components as persistent while not in playmode, activate 'components stay persistent' in the settings");

            HandleDrop();
        }

        private void HandleDrop()
        {
            Rect dropArea = new Rect(0, 0, position.width, position.height);

            var currentEvent = Event.current;
            var currentEventType = currentEvent.type;

            if(currentEventType == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                DragAndDrop.AcceptDrag();
            }
            if(currentEventType == EventType.DragPerform)
            {
                foreach(var obj in DragAndDrop.objectReferences)
                {
                    if (obj is Component)
                        PersistentComponents.Instance.WatchComponent(obj as Component);
                    else if (obj is GameObject)
                        PersistentComponents.Instance.WatchComponents((obj as GameObject).GetComponents<Component>());
                }
            }
        }

	    private void DrawComponentList()
	    {
            if(GUILayout.Button("Add everything"))
            {
                PersistentComponents.Instance.WatchComponents(FindObjectsOfType<Component>());
            }
            if (GUILayout.Button("Remove everything"))
                PersistentComponents.Instance.ForgetEveryComponent();
            using (new GUILayout.HorizontalScope((GUIStyle)"ShurikenEffectBg"))
            {
                using (var scrollView = new GUILayout.ScrollViewScope(currentScrollPos, false, false))
                {
                    var toRemove = new List<Component>();
                    foreach (var go in PersistentComponents.Instance.WatchedComponents)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label(go.Key.name, GUILayout.ExpandWidth(false));

                            var alignmentBckp = GUIStyle.none.alignment;
                            GUIStyle.none.alignment = TextAnchor.MiddleCenter;
                            if (GUILayout.Button(EditorGUIUtility.FindTexture("winbtn_mac_close_a"), GUIStyle.none, GUILayout.Width(Constants.ICON_SIZE), GUILayout.Height(Constants.ICON_SIZE)))
                                toRemove.AddRange(go.Value);
                            GUIStyle.none.alignment = alignmentBckp;
                        }

                        foreach (var component in go.Value)
                        {
                            using (new GUILayout.HorizontalScope((GUIStyle)"HelpBox"))
                            {
                                Vector2 size = GUIStyle.none.CalcSize(removeButtonContent);
                                float inspectorWidth = EditorGUIUtility.currentViewWidth;
                                float buttonWidth = inspectorWidth - size.x - Constants.ICON_SIZE * 2 - 40; // 40 because of offsets and scrollbar

                                var content = EditorGUIUtility.ObjectContent(null, component.GetType()).image;
                                if (content == null)
                                    content = EditorGUIUtility.IconContent("cs Script Icon").image;
                                GUILayout.Label(content, GUILayout.MaxWidth(Constants.ICON_SIZE),
                                    GUILayout.MaxHeight(Constants.ICON_SIZE));

                                if (GUILayout.Button(component.gameObject.name + "." + component.GetType().Name,
                                    GUILayout.MaxWidth(buttonWidth), GUILayout.Width(buttonWidth),
                                    GUILayout.ExpandWidth(false)))
                                {
                                    EditorGUIUtility.PingObject(component);
                                    Selection.activeGameObject = component.gameObject;
                                }

                                var alignmentBckp = GUIStyle.none.alignment;
                                GUIStyle.none.alignment = TextAnchor.MiddleCenter;
                                if (GUILayout.Button(EditorGUIUtility.FindTexture("winbtn_mac_close_a"), GUIStyle.none, GUILayout.Width(Constants.ICON_SIZE), GUILayout.Height(Constants.ICON_SIZE)))
                                    toRemove.Add(component);
                                GUIStyle.none.alignment = alignmentBckp;

                            }
                        }
                    }

                    foreach (var removeable in toRemove)
                    {
                        PersistentComponents.Instance.ForgetComponent(removeable);
                    }

                    currentScrollPos = scrollView.scrollPosition;
                }
            }
        }

	    public static void MarkAsPersistent(Component component)
	    {
            PersistentComponents.Instance.WatchComponent(component);

            if (Instance != null)
                Instance.Repaint();
	    }

    }
}