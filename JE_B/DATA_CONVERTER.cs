using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DATA_CONVERTER
{
    public class DATA_CONVERTER
    {
        public Data data = new Data();
        static void main(string[] args)
        {

        }
    }
    public class command_centralls
    {

    }
    public class outer_commands : command_centralls
    {
        public virtual void Execute(List<string> code, Data D)
        {

        }
    }
    public class Data
    {
        Dictionary<string, string> strings = new Dictionary<string, string>();
        Dictionary<string, double> doubles = new Dictionary<string, double>();
        Dictionary<string, int> integers = new Dictionary<string, int>();
        Dictionary<string, Dictionary<string, Object>> custom_types = new Dictionary<string, Dictionary<string, Object>>();
        public string referenceS(string key)
        {
            if (strings.ContainsKey(key))
            {
                return strings[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }
        }
        public void SaveToFile(string filePath)
        {
            string stringsData = DictionaryToString(strings);
            string doublesData = DictionaryToString(doubles);
            File.WriteAllText(filePath, stringsData + Environment.NewLine + doublesData);
        }

        private static string DictionaryToString<T>(Dictionary<string, T> dictionary)
        {
            List<string> keyValuePairs = new List<string>();
            foreach (var kvp in dictionary)
            {
                keyValuePairs.Add($"{kvp.Key}={kvp.Value}");
            }
            return string.Join(Environment.NewLine, keyValuePairs);
        }

        public double referenceD(string key)
        {
            if (doubles.ContainsKey(key))
            {
                return doubles[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }
        }
        public bool isnumvar(string key)
        {
            if (doubles.ContainsKey(key) || integers.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public bool isvar(string key)
        {
            if (doubles.ContainsKey(key) || integers.ContainsKey(key) || strings.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public bool inint(string key)
        {
            if (integers.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public bool instring(string key)
        {
            if (strings.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public bool indouble(string key)
        {
            if (doubles.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public int referenceI(string key)
        {
            if (integers.ContainsKey(key))
            {
                return integers[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }
        }
        public Object referenceVar(string key)
        {
            if (doubles.ContainsKey(key))
            {
                return doubles[key];
            }
            else if (strings.ContainsKey(key))
            {
                return strings[key];
            }
            else if (integers.ContainsKey(key))
            {
                return integers[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }

        }
        public void setS(string key, string data)
        {
            if (doubles.ContainsKey(key) || integers.ContainsKey(key))
            {
                Console.WriteLine("variable set to other type");
            }
            else
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (strings.ContainsKey(key))
                    {
                        strings.Remove(key);
                    }
                    strings.Add(key, data);
                }
            }
        }

        public void setD(string key, double data)
        {
            if (strings.ContainsKey(key) || integers.ContainsKey(key))
            {
                Console.WriteLine("variable set to other type");
            }
            else
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (doubles.ContainsKey(key))
                    {
                        doubles.Remove(key);
                    }
                    doubles.Add(key, data);
                }
            }
        }
        public void setI(string key, int data)
        {
            if (strings.ContainsKey(key) || doubles.ContainsKey(key))
            {
                Console.WriteLine("variable set to other type");
            }
            else
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (integers.ContainsKey(key))
                    {
                        integers.Remove(key);
                    }
                    integers.Add(key, data);
                }
            }
        }

    }
}