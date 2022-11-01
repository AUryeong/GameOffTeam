
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(CameraController))]
//[CanEditMultipleObjects]
//public class CameraControllerEditor : Editor
//{
//    SerializedProperty _Type;
//    SerializedProperty _Target;

//    SerializedProperty _offsetX;
//    SerializedProperty _offsetY;
//    SerializedProperty _CameraDistance;
//    SerializedProperty _DelayTime;




//    protected void OnEnable()
//    {
//        _Type = serializedObject.FindProperty("Type");
//        _Target = serializedObject.FindProperty("target");


//        _offsetX = serializedObject.FindProperty("offsetX");
//        _offsetY = serializedObject.FindProperty("offsetY");

//        _CameraDistance = serializedObject.FindProperty("CameraDistance");
//        _DelayTime = serializedObject.FindProperty("DelayTime");

//    }

//    public override void OnInspectorGUI()
//    {
//        EditorGUILayout.PropertyField(_Type);
//        EditorGUILayout.PropertyField(_Target);
//        EditorGUI.indentLevel++;

//        if (_Type.intValue == (int)CameraController.CameraType.BackView)
//        {
//            EditorGUILayout.PropertyField(_offsetY);
//            EditorGUILayout.PropertyField(_CameraDistance);
//            EditorGUILayout.PropertyField(_DelayTime);
//        }
//        else if (_Type.intValue == (int)CameraController.CameraType.ShoulderView)
//        {
//            EditorGUILayout.PropertyField(_offsetX);
//            EditorGUILayout.PropertyField(_offsetY);
//            EditorGUILayout.PropertyField(_CameraDistance);
//            EditorGUILayout.PropertyField(_DelayTime);
//        }
//        else if (_Type.intValue == (int)CameraController.CameraType.QuarterView)
//        {

//        }
//        else if (_Type.intValue == (int)CameraController.CameraType.TopView)
//        {
//            EditorGUILayout.PropertyField(_offsetY);
//            EditorGUILayout.PropertyField(_CameraDistance);
//            EditorGUILayout.PropertyField(_DelayTime);
//        }


//        serializedObject.ApplyModifiedProperties();
//    }
//}

