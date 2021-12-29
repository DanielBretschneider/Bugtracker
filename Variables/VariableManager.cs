using Bugtracker.Attributes;
using Bugtracker.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Bugtracker.Variables
{
    public class VariableManager
    {
        public Dictionary<string, (dynamic value, bool isDynamic)> VariableDictionary { get; set; } = new Dictionary<string, (dynamic value, bool isDynamic)>();
        private RunningConfiguration rc;

        /// <summary>
        /// Loads 
        /// </summary>
        /// <param name="runningConfiguration"></param>
        public VariableManager(RunningConfiguration rc)
        {
            this.rc = rc;
            FullRefresh();
        }

        private void SetCustomKeyValues()
        {
            string hostname = rc.PcInfo.GetHostname();
            VariableDictionary["clientname"] = (hostname, false);
            VariableDictionary["computername"] = (hostname, false);
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

        private void LoadAllAnnotatedKeyValuePairs(Object objectToLoadFrom)
        {
            foreach (PropertyInfo propertyInfo in objectToLoadFrom.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                {
                    KeyAttribute ka = (KeyAttribute)propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true);

                    if (ka.Dynamic)
                        VariableDictionary[ka.Name] = (propertyInfo.GetValue(objectToLoadFrom) ?? "not set.", true);
                    else 
                        VariableDictionary[ka.Name] = (propertyInfo.GetValue(objectToLoadFrom) ?? "not set.", false);

                    System.Diagnostics.Debug.WriteLine("key = " + ka.Name + "value: " + propertyInfo.GetValue(objectToLoadFrom));
                }
            }
        }

        private void SetValuesAccordingToVariables(Object objectToLoadFrom)
        {
            foreach (PropertyInfo propertyInfo in objectToLoadFrom.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                {
                    KeyAttribute ka = (KeyAttribute)propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true);

                    foreach (var keyValuePair in VariableDictionary)
                    {
                        if (keyValuePair.Key == ka.Name)
                            propertyInfo.SetValue(objectToLoadFrom, keyValuePair.Value.value);
                    }

                    

                    System.Diagnostics.Debug.WriteLine("set value for key = " + ka.Name + " new value: " + propertyInfo.GetValue(objectToLoadFrom));
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

                foreach (var tuple in keysAndNewValues)
                {
                    System.Diagnostics.Debug.WriteLine("Try to change Dict var: key: " + tuple.Key + "new value" + tuple.NewValue);
                    VariableDictionary[tuple.Key] = (tuple.NewValue, false);
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
            //Refresh(true);

            string newValue = value;

            if (value != null)
            {

                foreach (var key in VariableDictionary.Keys)
                {
                    if (newValue.Contains("%" + key + "%"))
                    {
                        if (VariableDictionary[key].isDynamic)
                        {
                            ReloadSpecificAnnotatedKeyValuePair(rc, key);
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
            LoadAllAnnotatedKeyValuePairs(rc);
            ReplaceKeysInValuesTillKeyless();
            SetValuesAccordingToVariables(rc);
        }
    }
}
