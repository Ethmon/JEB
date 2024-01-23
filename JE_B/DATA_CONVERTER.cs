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
        public Dictionary<string, Data> sheets = new Dictionary<string, Data>();
        Dictionary<string, Dictionary<string, Object>> custom_types = new Dictionary<string, Dictionary<string, Object>>();
        public int identifier = 0;
        public int typeidentifier = 0;
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
        public void remove(string key)
        {
            //remove from all dictionaries
            if (strings.ContainsKey(key))
            {
                strings.Remove(key);
            }
            if (doubles.ContainsKey(key))
            {
                doubles.Remove(key);
            }
            if (integers.ContainsKey(key))
            {
                integers.Remove(key);
            }
            if (sheets.ContainsKey(key))
            {
                sheets.Remove(key);
            }
            if (custom_types.ContainsKey(key))
            {
                custom_types.Remove(key);
            }

        }
        public Data Copy()
        {
            Data d = new Data();
            d.strings = new Dictionary<string, string>(strings);
            d.doubles = new Dictionary<string, double>(doubles);
            d.integers = new Dictionary<string, int>(integers);
            d.custom_types = new Dictionary<string, Dictionary<string,object>>(custom_types);
            foreach(string key in sheets.Keys)
            {
                d.sheets.Add(key, sheets[key].Copy());
            }
            return d;
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
        public Object referenceCustom(string key, string key2)
        {
            if (custom_types.ContainsKey(key))
            {
                if (custom_types[key].ContainsKey(key2))
                {
                    return custom_types[key][key2];
                }
                else
                {
                    throw new ArgumentException(key2 + " not initiallized");
                }
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }
        }
        public Data referenceSheet(string key)
        {
            if (sheets.ContainsKey(key))
            {
                return sheets[key];
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
            if (doubles.ContainsKey(key) || integers.ContainsKey(key) || strings.ContainsKey(key) || sheets.ContainsKey(key)||custom_types.ContainsKey(key))
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
        public bool issheet(string key)
        {
            if (sheets.ContainsKey(key))
            {
                return true;
            }
            return false;
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
            else if (sheets.ContainsKey(key))
            {
                return sheets[key];
            }
            else if (custom_types.ContainsKey(key))
            {
                return custom_types[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }

        }
        public void setS(string key, string data)
        {
            if (doubles.ContainsKey(key) || integers.ContainsKey(key) || sheets.ContainsKey(key) || custom_types.ContainsKey(key))
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
            if (strings.ContainsKey(key) || integers.ContainsKey(key) || sheets.ContainsKey(key) || custom_types.ContainsKey(key))
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
        public void setCustom(string key, Dictionary<string, Object> data)
        {
            if (strings.ContainsKey(key) || integers.ContainsKey(key) || sheets.ContainsKey(key) || doubles.ContainsKey(key))
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
                    if (custom_types.ContainsKey(key))
                    {
                        custom_types.Remove(key);
                    }
                    custom_types.Add(key, data);
                }
            }
        }

        public void setsheet(string key, Data data)
        {
            if (strings.ContainsKey(key) || integers.ContainsKey(key) || doubles.ContainsKey(key) || custom_types.ContainsKey(key))
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
                    if (sheets.ContainsKey(key))
                    {
                        sheets.Remove(key);
                    }
                    sheets.Add(key, data);
                }
            }
        }
        public void setI(string key, int data)
        {
            if (strings.ContainsKey(key) || doubles.ContainsKey(key) || sheets.ContainsKey(key) || custom_types.ContainsKey(key))
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