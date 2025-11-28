using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine.UIElements;

namespace Cali_4
{ 
    public class C4_InspectorInterface : MonoBehaviour
    {
        [Header("Manager Refs")]
        public C4_HostBrain inspBBrain;
        public C4_StateMachine inspStateMachine;
        public C4_StateDeterminant inspDeterminant;
        public C4_UIManager inspUIManager;
        public C4_AnimManager inspAnimManager;

        [Header("Script Component Refs")]
        public Actor inspActor;
        public Damager inspDamager;
        public Damageable inspDamageable;
        public Navigator inspNavigator;
        public NavMeshAgent inspNavMeshAgent;
        public Sensor inspMeleeSensor;

        [Header("Physical Component Refs")]
        public Rigidbody inspRigidbody;
        public Collider inspBossSensorCollider;
        public Collider inspBossDamagerCollider;

        public static C4_InspectorInterface Instance;

        protected List<Component> SerializedComponents = new List<Component>();
        
        //public delegate Func<Component, Component, bool> FindConnectedComponents = (targetObj, scopeObj) => (FindMatchies(targetObj, scopeObj));
        Func<string, Type> ReturnTypeFromString = (nameIn) => HelpMe(nameIn);

        private delegate void ComponentMatch();

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            var MatchComponents = new Action<Component,Component>((targetComp, scopeComp) => FindAttachedComponent(targetComp, scopeComp));
            //= (targetIn,scopeTarget) => FindAttachedComponent(targetIn,scopeTarget);
            CheckCustomStaticInstances();
            CheckUnassignedProperties();
        }

        private void CheckCustomStaticInstances()
        { 
            inspBBrain = C4_HostBrain.Buster;
            inspStateMachine = C4_StateMachine.Instance;
            inspDeterminant = C4_StateDeterminant.Instance;
            inspUIManager = C4_UIManager.Instance;
            inspAnimManager = C4_AnimManager.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckUnassignedProperties()
        {
            int propertyCount = 0;
            Component currentType = null;

            PropertyInfo[] scriptProperties = typeof(C4_InspectorInterface).GetProperties();

            foreach(PropertyInfo currentProperty in scriptProperties)
            {
                dynamic searchTypeRef = currentProperty.PropertyType;
                propertyCount++;
                print($"On property number {propertyCount}.");

                if(currentProperty == null)
                { 
                    if((currentProperty.PropertyType.Namespace.ToString() != "Cali_4"))
                    {  
                        print($"Property found null, and not in custom namespace: {currentProperty.Name}");
                        //C4_HostBrain.Buster.gameObject.GetComponent(currentProperty.Name);
                    }
                    else if(currentProperty.PropertyType.GetType() == typeof(Component))
                    {
                        print($"Property found null, and is a component: {currentProperty.Name}");
                        //currentProperty.SetValue(this,);
                        //typeof(C4_InspectorInterface).GetProp;
                    }
                    else
                    { 
                        print($"Property found null, and is not a component: {currentProperty.Name}");
                    }
                }
                else
                { 
                    print($"Property already set: {currentProperty.Name}");
                }
            }
            print($"CheckUnassignedProperties has counted {propertyCount} properties on C4_InspectorInterface's static instance.");
        }

        static private Type HelpMe(string typeNameBuh)
        {
            string fluB = new string("");

            Type outputType = null;

            List<Type> typesHehe = new List<Type>();

            PropertyInfo[] scriptProperties = typeof(C4_InspectorInterface).GetProperties();
            foreach(PropertyInfo grunk in scriptProperties)
                typesHehe.Add(grunk.PropertyType);

            outputType = typesHehe.Find(guh => guh.Name == typeNameBuh);

            return outputType;
        }

        static private T FindThatMotherfuckingComponent<T>() where T : Component
        {

            return null;    
        }

        public T GetSerializedRef<T>(T refIn) where T : Component
        { 
            return (T)SerializedComponents.Find(compRef => compRef.GetType() == refIn.GetType());
        }

        bool FindAttachedComponent(Component targetObj, Component targetAttachedObj)
        {
            List<Component> componentList = new();
            var componentArray = gameObject.GetComponents<MonoBehaviour>();
            foreach(var component in componentArray) { componentList.Add(component); }
            componentList.Find(listOut => FindMatchies(targetObj, targetAttachedObj, listOut));
            return false;
        }

        private static bool FindMatchies<T1,T2,T3>(T1 targetObj, T2 signifierObj, T3 listRef)
            where T1 : Component, T3
            where T2 : Component, T3
            where T3 : Component
        {
            bool resultBool = false;
            bool equalityCheck = (targetObj.GetType() == signifierObj.GetType());

            T1 firstHit = null;
            T2 secondHit = null;
            T3 secondaryObj = null;

            bool hitSuccess = false;

            if(equalityCheck)
            { 
                print("Two components of the same type were used as reference, please make sure to give objects of different types, but with both inheriting from Component.");
                return false;
            }
            else
            { 
                if(listRef.GetType() == targetObj.GetType())
                { 
                    firstHit = listRef as T1;
                    secondaryObj = listRef.GetComponent<T2>();
                    hitSuccess = true;
                }
                if(listRef.GetType() == signifierObj.GetType())
                { 
                    secondHit = listRef as T2;
                    secondaryObj = listRef.GetComponent<T1>();
                    hitSuccess = true;
                }
                else
                    hitSuccess = false;

                if(hitSuccess)
                {
                    if(secondaryObj != null)
                    { 
                        resultBool = true;
                    }
                }
            }

            return resultBool;
        }
    }
}
