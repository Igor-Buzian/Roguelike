using MalbersAnimations.Scriptables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
    [CreateAssetMenu(menuName = "Malbers Animations/Extras/Material Property Lerp", order = 2100)]
    public class MaterialPropertyLerpSO : ScriptableCoroutine
    {
        [Tooltip("Index of the Material")]
        public IntReference materialIndex = new IntReference();
        public FloatReference time = new FloatReference(1f);
        public AnimationCurve curve = new AnimationCurve(MTools.DefaultCurve);


        public StringReference propertyName;
        public MaterialPropertyType propertyType = MaterialPropertyType.Float;

        public FloatReference FloatValue = new(1f);
        public Color ColorValue = Color.white;
        [ColorUsage(true, true)]
        public Color ColorHDRValue = Color.white;
        public FloatReference StartMultiplier = new(1f);


        public void LerpMaterial(Component go) => LerpMaterial(go.gameObject);
        public void LerpMaterial(GameObject go)
        {
            var rootCore = go.FindInterface<IObjectCore>();
            var root = rootCore != null ? rootCore.transform : go.transform.root;

            var skinmesh = root.GetComponentsInChildren<SkinnedMeshRenderer>();
            var mesh = root.GetComponentsInChildren<MeshRenderer>();

            if (skinmesh.Length > 0) LerpMaterial(skinmesh);
            if (mesh.Length > 0) LerpMaterial(mesh);
        }

        internal override void Evaluate(MonoBehaviour mono, Transform target, float time, AnimationCurve curve = null)
        {
            var rootCore = target.FindInterface<IObjectCore>();
            var root = rootCore != null ? rootCore.transform : target.transform.root;

            var all = root.GetComponentsInChildren<SkinnedMeshRenderer>();
            var all2 = root.GetComponentsInChildren<MeshRenderer>();

            var curv = curve ?? this.curve;

            switch (propertyType)
            {
                case MaterialPropertyType.Float:
                    mono.StartCoroutine(LerperFloat(all, time, curv));
                    mono.StartCoroutine(LerperFloat(all2, time, curv));
                    break;
                case MaterialPropertyType.Color:
                    mono.StartCoroutine(LerperColor(all, ColorValue, time, curv));
                    mono.StartCoroutine(LerperColor(all2, ColorValue, time, curv));
                    break;
                case MaterialPropertyType.HDRColor:
                    mono.StartCoroutine(LerperColor(all, ColorHDRValue, time, curv));
                    mono.StartCoroutine(LerperColor(all2, ColorHDRValue, time, curv));
                    break;
                default:
                    break;

            }
        }

        public virtual void LerpMaterial(Renderer[] mesh)
        {
            IEnumerator ICoroutine = null;
            switch (propertyType)
            {
                case MaterialPropertyType.Float:
                    ICoroutine = LerperFloat(mesh, time, curve);
                    break;
                case MaterialPropertyType.Color:
                    ICoroutine = LerperColor(mesh, ColorValue, time, curve);
                    break;
                case MaterialPropertyType.HDRColor:
                    ICoroutine = LerperColor(mesh, ColorHDRValue, time, curve);
                    break;
                default:
                    break;

            }
            StartCoroutine(mesh[0], ICoroutine);
        }

        IEnumerator LerperFloat(Renderer[] mesh, float time, AnimationCurve curve)
        {
            float elapsedTime = 0;

            List<Material> allMats = new();
            for (int i = 0; i < mesh.Length; i++)
            {
                if (!mesh[i].gameObject.activeInHierarchy) continue;

                allMats.Add(mesh[i].materials[materialIndex]);
            }


            List<Material> matProperty = new();

            foreach (var m in allMats)
            {
                if (m.HasColor(propertyName))
                {
                    matProperty.Add(m);
                }
            }

            while (elapsedTime <= time)
            {
                float value = curve.Evaluate(elapsedTime / time);

                for (int i = 0; i < matProperty.Count; i++)
                {
                    matProperty[i].SetFloat(propertyName, value * FloatValue);
                }

                Debug.Log("value = " + value);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < matProperty.Count; i++)
            {
                matProperty[i].SetFloat(propertyName, curve.Evaluate(curve.keys[^1].time));
            }
            yield return null;
           // Stop(mesh[0]);
        }

        IEnumerator LerperColor(Renderer[] mesh, Color FinalColor, float time, AnimationCurve curve)
        {
            float elapsedTime = 0;

            List<Material> allMats = new();
            for (int i = 0; i < mesh.Length; i++)
            {
                allMats.Add(mesh[i].materials[materialIndex]);
            }

            List<Material> matProperty = new();
            List<Color> StartingColors = new();
            Color ElapsedColor;

            foreach (var m in allMats)
            {
                if (m.HasColor(propertyName))
                {
                    matProperty.Add(m);
                    StartingColors.Add(m.GetColor(propertyName) * StartMultiplier);
                }
            }

            if (time > 0)
            {
                while (elapsedTime <= time)
                {
                    float value = curve.Evaluate(elapsedTime / time);

                    for (int i = 0; i < matProperty.Count; i++)
                    {
                        ElapsedColor = Color.LerpUnclamped(StartingColors[i], FinalColor, value);
                        matProperty[i].SetColor(propertyName, ElapsedColor);
                    }

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }


            for (int i = 0; i < matProperty.Count; i++)
            {
                ElapsedColor = Color.LerpUnclamped(StartingColors[i], FinalColor, curve.Evaluate(1));
                matProperty[i].SetColor(propertyName, ElapsedColor);
            }

            yield return null;
            Stop(mesh[0]);
        }
    }

    [System.Serializable]
    public class MaterialPropertyInternal
    {
        public string propertyName;
        public MaterialPropertyType propertyType = MaterialPropertyType.Float;
        public float FloatValue = 1f;
        public Color ColorValue = Color.white;
        [ColorUsage(true, true)]
        public Color ColorHDRValue = Color.white;

        [HideInInspector] public bool isFloat;
        [HideInInspector] public bool isColor;
        [HideInInspector] public bool isColorHDR;
    }

    public enum MaterialPropertyType
    {
        Float,
        Color,
        HDRColor
    }

    //Inspector

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MaterialPropertyLerpSO)), UnityEditor.CanEditMultipleObjects]
    public class MaterialPropertyLerpSOEditor : UnityEditor.Editor
    {
        UnityEditor.SerializedProperty propertyName, materialIndex, propertyType, time, FloatValue, ColorValue, ColorHDRValue, ColorMultiplier, curve;//, UseMaterialPropertyBlock, shared;

        private void OnEnable()
        {
            propertyName = serializedObject.FindProperty("propertyName");
            materialIndex = serializedObject.FindProperty("materialIndex");
            propertyType = serializedObject.FindProperty("propertyType");
            time = serializedObject.FindProperty("time");
            FloatValue = serializedObject.FindProperty("FloatValue");
            ColorValue = serializedObject.FindProperty("ColorValue");
            ColorHDRValue = serializedObject.FindProperty("ColorHDRValue");
            ColorMultiplier = serializedObject.FindProperty("StartMultiplier");
            curve = serializedObject.FindProperty("curve");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UnityEditor.EditorGUILayout.PropertyField(propertyName);
            UnityEditor.EditorGUILayout.PropertyField(materialIndex);
            UnityEditor.EditorGUILayout.PropertyField(time);

            UnityEditor.EditorGUILayout.PropertyField(propertyType);

            var pType = (MaterialPropertyType)propertyType.intValue;

            switch (pType)
            {
                case MaterialPropertyType.Float:
                    UnityEditor.EditorGUILayout.PropertyField(FloatValue);
                    break;
                case MaterialPropertyType.Color:
                    UnityEditor.EditorGUILayout.PropertyField(ColorValue);
                    UnityEditor.EditorGUILayout.PropertyField(ColorMultiplier);
                    break;
                case MaterialPropertyType.HDRColor:
                    UnityEditor.EditorGUILayout.PropertyField(ColorHDRValue);
                    UnityEditor.EditorGUILayout.PropertyField(ColorMultiplier);
                    break;
                default:
                    break;
            }


            UnityEditor.EditorGUILayout.PropertyField(curve);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif


}

