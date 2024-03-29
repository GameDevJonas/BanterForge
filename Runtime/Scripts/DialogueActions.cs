﻿using System;
using System.Collections.Generic;
using System.Reflection;
using GDPanda.Data;
using UnityEngine;
using Object = System.Object;

namespace GDPanda.BanterForge
{
    public abstract class DialogueActionListener : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            DialogueActions.AddDialogueListener(this);
        }

        protected virtual void OnDisable()
        {
            DialogueActions.RemoveDialogueListener(this);
        }
    }
    
    public class DialogueActions : Singleton<DialogueActions>
    {
        [HideInInspector]
        [SerializeField]
        private List<DialogueActionListener> _additionalInstancesToCheckMethodsFrom;

        public static void AddDialogueListener(DialogueActionListener listener)
        {
            var instance = GetInstance();
            if(instance._additionalInstancesToCheckMethodsFrom.Contains(listener))
                return;
            
            instance._additionalInstancesToCheckMethodsFrom.Add(listener);
        }
        
        public static void RemoveDialogueListener(DialogueActionListener listener)
        {
            var instance = GetInstance();
            if(!instance._additionalInstancesToCheckMethodsFrom.Contains(listener))
                return;
            
            instance._additionalInstancesToCheckMethodsFrom.Remove(listener);
        }
        
        public static void OnDialogueAction(string actionString)
        {
            var instance = GetInstance() as object;
            Type thisType = instance.GetType();

            string[] methodName = actionString.Split("(");
            
            MethodInfo theMethod = thisType.GetMethod(methodName[0]);
            if (theMethod == null)
            {
                MethodInfo foundAdditionalMethod = null;
                foreach (var additionalInstance in GetInstance()._additionalInstancesToCheckMethodsFrom)
                {
                    var additionalInstanceType = additionalInstance.GetType();
                    foundAdditionalMethod = additionalInstanceType.GetMethod(methodName[0]);
                    if(foundAdditionalMethod == null)
                        continue;

                    theMethod = foundAdditionalMethod;
                    instance = additionalInstance;
                }
                
                if (theMethod == null)
                {
                    Debug.LogError("Trying to invoke dialogue action on null method!");
                    return;
                }
            }
            
            string[] cleanParameters = methodName[1].Split(")");
            string[] parameterStrings = cleanParameters[0].Split(",");
            
            List<string> parameters = new();
            for (int paramIndex = 0; paramIndex < parameterStrings.Length; paramIndex++)
            {
                var parameter = parameterStrings[paramIndex];
                parameters.Add(parameter);
            }

            List<object> inputParameters = new();
            var parameterInfos = theMethod.GetParameters();
            for (int methodParameterIndex = 0; methodParameterIndex < parameterInfos.Length; methodParameterIndex++)
            {
                if(methodParameterIndex >= parameters.Count || parameters.Count < methodParameterIndex || parameters.Count != parameterInfos.Length)
                    return;

                var inputParam = parameters[methodParameterIndex];
                
                var parameterInfo = parameterInfos[methodParameterIndex];
                var type = parameterInfo.ParameterType;
                
                var isBool = type == typeof(bool);
                var isInteger = type == typeof(int);
                var isFloat = type == typeof(float);
                var isString = type == typeof(string);
                if (isBool)
                {
                    try
                    {
                        var boolValue = bool.Parse(inputParam);
                        inputParameters.Add(boolValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
                else if (isInteger)
                {
                    try
                    {
                        var intValue = int.Parse(inputParam);
                        inputParameters.Add(intValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
                else if (isFloat)
                {
                    try
                    {
                        inputParam = inputParam.Replace(".", ",");
                        var floatValue = float.Parse(inputParam);
                        inputParameters.Add(floatValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
                else if (isString)
                {
                    inputParameters.Add(inputParam);
                }
                else return;
            }
            
            theMethod.Invoke(instance, inputParameters.ToArray());
        }

        public void Test()
        {
            Debug.Log("TEST");
        }

        public void ParamTest(bool boolean, int integer, float number, string text)
        {
            Debug.Log($"TEST {boolean}, {integer}, {number}, {text}");
        }
    }
}