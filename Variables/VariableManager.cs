﻿using Bugtracker.Attributes;
using Bugtracker.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Win32;

namespace Bugtracker.Variables
{
    public class VariableManager
    {
        public Dictionary<string, (dynamic value, bool isDynamic)> VariableDictionary { get; set; } = new Dictionary<string, (dynamic value, bool isDynamic)>();
        //private RunningConfiguration rc;
        private readonly Object[] toLoadFrom = Array.Empty<object>(); 

        /// <summary>
        /// Loads 
        /// </summary>
        /// <param name="runningConfiguration"></param>
        public VariableManager(params Object[] toLoadFrom)
        {
            this.toLoadFrom = toLoadFrom;
            FullRefresh();
        }

        private void SetCustomKeyValues()
        {
            //Read KEYS created during program setup

            //disabled due to insufficients rights
            //VariableDictionary["configdest"] =
            //    (Registry.GetValue(@"HKEY_CURRENT_USER\Software\ManageMed", "CONFIGDEST", null), false);
            //VariableDictionary["serverdest"] =
            //    (Registry.GetValue(@"HKEY_CURRENT_USER\Software\ManageMed", "SERVERDEST", null), false);
            //VariableDictionary["targetdir"] =
            //    (Registry.GetValue(@"HKEY_CURRENT_USER\Software\ManageMed", "TARGETDIR", null), false);
            //VariableDictionary["firststartup"] =
            //    (Registry.GetValue(@"HKEY_CURRENT_USER\Software\ManageMed", "FIRSTSTARTUP", null), false);

            //hardcoded
            VariableDictionary["configdest"]    = ("\\10.74.10.100\\bugTracker\\", false);
            VariableDictionary["serverdest"]    = ("10.74.10.100", false);
            VariableDictionary["targetdir"]     = ("C:\\Bugtracker", false);
        }

        private void LoadInAllEnvironmentVariables()
        {
            foreach (DictionaryEntry dictEntry in Environment.GetEnvironmentVariables())
            {
                VariableDictionary[(string)dictEntry.Key] = (dictEntry.Value, false);
            }
        }

        private void ReloadSpecificAnnotatedKeyValuePair(Object objectToLoadFrom, string key)
        {
            foreach (PropertyInfo propertyInfo in objectToLoadFrom.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                {
                    KeyAttribute ka = (KeyAttribute)propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true);

                    if (ka.Name == key)
                    {
                        if (ka.Dynamic)
                            VariableDictionary[ka.Name] = (propertyInfo.GetValue(objectToLoadFrom) ?? "not set.", true);
                        else
                            VariableDictionary[ka.Name] = (propertyInfo.GetValue(objectToLoadFrom) ?? "not set.", false);

                        System.Diagnostics.Debug.WriteLine("key = " + ka.Name + "value: " + propertyInfo.GetValue(objectToLoadFrom));
                    }
                }
            }
        }

        private void LoadAllAnnotatedKeyValuePairs(Object[] objectsToLoadFrom)
        {
            foreach(Object obj in objectsToLoadFrom)
            {
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                    {
                        KeyAttribute ka = (KeyAttribute)propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true);

                        if (ka.Dynamic)
                            VariableDictionary[ka.Name] = (propertyInfo.GetValue(obj) ?? "not set.", true);
                        else
                            VariableDictionary[ka.Name] = (propertyInfo.GetValue(obj) ?? "not set.", false);

                        System.Diagnostics.Debug.WriteLine("key = " + ka.Name + "value: " + propertyInfo.GetValue(obj));
                    }
                }
            }
        }

        private void SetValuesAccordingToVariables(Object[] objectsToLoadFrom)
        {
            foreach(Object obj in objectsToLoadFrom)
            {
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                    {
                        KeyAttribute ka = (KeyAttribute)propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true);

                        foreach (var keyValuePair in VariableDictionary)
                        {
                            if (keyValuePair.Key == ka.Name)
                            {
                                if(propertyInfo.CanWrite)
                                {
                                    propertyInfo.SetValue(obj, keyValuePair.Value.value);
                                }
                            }
                                
                        }

                        System.Diagnostics.Debug.WriteLine("set value for key = " + ka.Name + " new value: " + propertyInfo.GetValue(obj));
                    }
                }
            }
            
        }


        private void PrintKeyValues()
        {
            foreach (var keyValuePair in VariableDictionary)
            {
                System.Diagnostics.Debug.WriteLine(".......Current Key Values:....... " + System.Environment.NewLine + "Key: " + keyValuePair.Key + ", Value: " + keyValuePair.Value.value);
            }
        }

        private void ReplaceKeysInValuesTillKeyless()
        {
            var keysAndNewValues = new List<(string Key, string NewValue)>();

            while (DoValuesContainKeys())
            {
                foreach (var keyValuePairX in VariableDictionary)
                {
                    foreach (var keyValuePairY in VariableDictionary)
                    {
                        if (keyValuePairX.ToString().Contains("%" + keyValuePairY.Key + "%"))
                        {
                            string newString = ((string) keyValuePairX.Value.value).Replace("%" + keyValuePairY.Key + "%",
                                keyValuePairY.Value.value);

                            keysAndNewValues.Add((keyValuePairX.Key, newString));


                            //((string)VariableDictionary[keyValuePairX.Key]).Replace("%" + keyValuePairY.Key + "%", keyValuePairY.Value);
                            System.Diagnostics.Debug.WriteLine("value to insert: " + (string)keyValuePairY.Value.value);
                            //System.Diagnostics.Debug.WriteLine("Replaced Key with value, new string: " + (string) keyValuePairX.Value);
                        }
                    }
                }

                foreach (var (Key, NewValue) in keysAndNewValues)
                {
                    System.Diagnostics.Debug.WriteLine("Try to change Dict var: key: " + Key + "new value" + NewValue);
                    VariableDictionary[Key] = (NewValue, false);
                }
            }
        }

        private bool DoValuesContainKeys()
        {
            foreach (var keyValuePair in VariableDictionary)
            {
                foreach (var keyValuePairY in VariableDictionary)
                {
                    if (keyValuePair.Value.ToString().Contains("%" + keyValuePairY.Key + "%"))
                    {
                        System.Diagnostics.Debug.WriteLine("Found Value with key inside: " + (string)keyValuePair.Value.value);
                        return true;
                    }
                }
            }

            return false;
        }

        public string ReplaceKeywords(string value)
        {
            string newValue = value;

            if (value != null)
            {

                foreach (var key in VariableDictionary.Keys)
                {
                    if (newValue.Contains("%" + key + "%"))
                    {
                        if (VariableDictionary[key].isDynamic)
                        {
                            foreach(Object obj in toLoadFrom)
                            {
                                ReloadSpecificAnnotatedKeyValuePair(obj, key);
                            }
                        }

                        newValue = newValue.Replace("%" + key + "%", VariableDictionary[key].value);
                        System.Diagnostics.Debug.WriteLine("Replaced " + "%" + key + "%" + ", New Value: " + newValue);
                        Logging.Logger.Log("Replaced " + "%" + key + "%" + ", New Value: " + newValue, Logging.LoggingSeverity.Info);
                    }
                        
                }
            }

            return newValue;
        }

        

        public void FullRefresh()
        {
            LoadInAllEnvironmentVariables();
            SetCustomKeyValues();
            LoadAllAnnotatedKeyValuePairs(toLoadFrom);
            ReplaceKeysInValuesTillKeyless();
            SetValuesAccordingToVariables(toLoadFrom);
        }
    }
}
