// HierarchyColours
// Purely aesthetic formatting for the Untiy Editor
// Free asset, unsure where I got it from but since its just aesthetic it doesn't matter
// Modified by Dima

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

/// Sets a background color for game objects in the Hierarchy tab
[UnityEditor.InitializeOnLoad]
public class HierarchyObjectColor
{
    // Offset to the left of Object Name
    private static Vector2 offset = new Vector2(2, 1);

    static HierarchyObjectColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            Color backgroundColor = Color.white;
            Color textColor = Color.white;
            Texture2D texture = null;

            /////// Start Here ////////

            void ColBlueInc(string objectName) // Colour Blue
            {
                if (obj.name.Contains(objectName))
                {
                    backgroundColor = new Color(0.2f, 0.3f, 0.6f);
                    textColor = new Color(0.9f, 0.9f, 0.9f);
                }
            }

            void ColGreenInc(string objectName) // Colour Green
            {
                if (obj.name.Contains(objectName))
                {
                    backgroundColor = new Color(0.2f, 0.6f, 0.2f);
                    textColor = new Color(0.9f, 0.9f, 0.9f);
                }
            }

            void ColRedInc(string objectName) // Colour Red
            {
                if (obj.name.Contains(objectName))
                {
                    backgroundColor = new Color(0.6f, 0.2f, 0.2f);
                    textColor = new Color(0.9f, 0.9f, 0.9f);
                }
            }

            void ColPurpleInc(string objectName) // Colour Purple
            {
                if (obj.name.Contains(objectName))
                {
                    backgroundColor = new Color(1f, 0f, 1f);
                    textColor = new Color(0.9f, 0.9f, 0.9f);
                }
            }

            // Header Folders

            ColBlueInc("--");

            // Sub Folders

            ColGreenInc("==");

            // Temporary or Idea Objects

            ColRedInc("?");
            ColPurpleInc("!");

            /////// End Here ////////

            if (backgroundColor != Color.white)
            {
                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

                EditorGUI.DrawRect(bgRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = textColor },
                    fontStyle = FontStyle.Bold
                }
                );

                if (texture != null)
                    EditorGUI.DrawPreviewTexture(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), texture);
            }
        }
    }
}

#endif