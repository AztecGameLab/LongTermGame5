﻿using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class CustomMenuItems : MonoBehaviour
    {
        [MenuItem("GameObject/LTG5/Trigger", false, 10)]
        private static GameObject CreateTrigger(MenuCommand menuCommand)
        {
            var newObject = CreateNewGameObject(menuCommand);

            newObject.name = "Trigger";
            var colliderComponent = newObject.AddComponent<BoxCollider2D>();
            var triggerComponent = newObject.AddComponent<Trigger>();
            triggerComponent.colliderComponent = colliderComponent;

            return newObject;
        }
        
        [MenuItem("GameObject/LTG5/Interactable", false, 10)]
        private static GameObject CreateInteractable(MenuCommand menuCommand)
        {
            var newObject = CreateTrigger(menuCommand);

            newObject.name = "Interactable";
            var interactableComponent = newObject.AddComponent<Interactable>();
            interactableComponent.trigger = newObject.GetComponent<Trigger>(); 

            return newObject;
        }

        private static GameObject CreateNewGameObject(MenuCommand menuCommand)
        {
            var newObject = new GameObject();
            GameObjectUtility.SetParentAndAlign(newObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(newObject, "Create " + newObject.name);
            Selection.activeObject = newObject;

            return newObject;
        }
    }
}