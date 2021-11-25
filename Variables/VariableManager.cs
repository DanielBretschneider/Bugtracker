using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bugtracker.Attributes;
using Bugtracker.Configuration;
using Bugtracker.Console.Commands;

namespace Bugtracker.Variables
{
    public class VariableManager
    {
        public Dictionary<string, dynamic> VariableDictionary { get; set; } = new Dictionary<string, dynamic>();


        /// <summary>
        /// Loads 
        /// </summary>
        /// <param name="runningConfiguration"></param>
        public VariableManager(Object objectToLoadFrom)
        {
            LoadAllKeyValuePairs(objectToLoadFrom);
            ReplaceKeysInValuesTillKeyless();
            SetValuesAccordingToVariables(objectToLoadFrom);
        }


        private void LoadAllKeyValuePairs(Object objectToLoadFrom)
        {
            foreach (PropertyInfo propertyInfo in objectToLoadFrom.GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0)
                {
                    KeyAttribute ka = (KeyAttribute) propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true);

                    VariableDictionary.Add(ka.Name, propertyInfo.GetValue(objectToLoadFrom) ?? "not set.");

                    System.Diagnostics.Debug.WriteLine("key = " + ka.Name + "value: " +  propertyInfo.GetValue(objectToLoadFrom));
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
                            propertyInfo.SetValue(objectToLoadFrom, keyValuePair.Value);
                    }

                    

                    System.Diagnostics.Debug.WriteLine("set value for key = " + ka.Name + " new value: " + propertyInfo.GetValue(objectToLoadFrom));
                }
            }
        }


        private void PrintKeyValues()
        {
            foreach (var keyValuePair in VariableDictionary)
            {
                System.Diagnostics.Debug.WriteLine(".......Current Key Values:....... " + System.Environment.NewLine + "Key: " + keyValuePair.Key + ", Value: " + keyValuePair.Value);
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
                            string newString = ((string) keyValuePairX.Value).Replace("%" + keyValuePairY.Key + "%",
                                keyValuePairY.Value);

                            keysAndNewValues.Add((keyValuePairX.Key, newString));


                            //((string)VariableDictionary[keyValuePairX.Key]).Replace("%" + keyValuePairY.Key + "%", keyValuePairY.Value);
                            System.Diagnostics.Debug.WriteLine("value to insert: " + (string)keyValuePairY.Value);
                            //System.Diagnostics.Debug.WriteLine("Replaced Key with value, new string: " + (string) keyValuePairX.Value);
                        }
                    }
                }

                foreach (var tuple in keysAndNewValues)
                {
                    System.Diagnostics.Debug.WriteLine("Try to change Dict var: key: " + tuple.Key + "new value" + tuple.NewValue);
                    VariableDictionary[tuple.Key] = tuple.NewValue;
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
                        System.Diagnostics.Debug.WriteLine("Found Value with key inside: " + (string)keyValuePair.Value);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
