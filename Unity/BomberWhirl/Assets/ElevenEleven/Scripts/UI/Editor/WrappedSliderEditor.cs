﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace ElevenEleven {
    [CustomEditor(typeof(WrappedSlider), true)]
    [CanEditMultipleObjects]
    public class WrappedSliderEditor : SelectableEditor {
       
        SerializedProperty m_Direction;
        SerializedProperty m_FillRect;
        SerializedProperty m_HandleRect;
        SerializedProperty m_MinValue;
        SerializedProperty m_MaxValue;
        SerializedProperty m_WholeNumbers;
        SerializedProperty m_Value;
        SerializedProperty m_OnValueChanged;
        SerializedProperty m_wrapValues;
        SerializedProperty m_decrementButton;
        SerializedProperty m_incrementButton;

        protected override void OnEnable() {
            base.OnEnable();
            m_FillRect = serializedObject.FindProperty("m_FillRect");
            m_HandleRect = serializedObject.FindProperty("m_HandleRect");
            m_Direction = serializedObject.FindProperty("m_Direction");
            m_MinValue = serializedObject.FindProperty("m_MinValue");
            m_MaxValue = serializedObject.FindProperty("m_MaxValue");
            m_WholeNumbers = serializedObject.FindProperty("m_WholeNumbers");
            m_Value = serializedObject.FindProperty("m_Value");
            m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");
            m_wrapValues = serializedObject.FindProperty("m_wrapValues");
            m_decrementButton = serializedObject.FindProperty("m_decrementButton");
            m_incrementButton = serializedObject.FindProperty("m_incrementButton");
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_FillRect);
            EditorGUILayout.PropertyField(m_HandleRect);
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_Direction);
            if (EditorGUI.EndChangeCheck()) {
                Slider.Direction direction = (Slider.Direction)m_Direction.enumValueIndex;
                foreach (var obj in serializedObject.targetObjects) {
                    Slider slider = obj as Slider;
                    slider.SetDirection(direction, true);
                }
            }

            EditorGUILayout.PropertyField(m_wrapValues);
            EditorGUILayout.HelpBox("When using Wrap Values, the Max Value becomes equal to the Min Value. Keep this in mind if these values are tied to an array of objects.", MessageType.Info);

            EditorGUILayout.PropertyField(m_MinValue);
            EditorGUILayout.PropertyField(m_MaxValue);
            EditorGUILayout.PropertyField(m_WholeNumbers);
            EditorGUILayout.Slider(m_Value, m_MinValue.floatValue, m_MaxValue.floatValue);

            EditorGUILayout.PropertyField(m_decrementButton);
            EditorGUILayout.PropertyField(m_incrementButton);

            bool warning = false;
            foreach (var obj in serializedObject.targetObjects) {
                Slider slider = obj as Slider;
                Slider.Direction dir = slider.direction;
                if (dir == Slider.Direction.LeftToRight || dir == Slider.Direction.RightToLeft)
                    warning = (slider.navigation.mode != Navigation.Mode.Automatic && (slider.FindSelectableOnLeft() != null || slider.FindSelectableOnRight() != null));
                else
                    warning = (slider.navigation.mode != Navigation.Mode.Automatic && (slider.FindSelectableOnDown() != null || slider.FindSelectableOnUp() != null));
            }

            if (warning)
                EditorGUILayout.HelpBox("The selected slider direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);

            // Draw the event notification options
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_OnValueChanged);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}