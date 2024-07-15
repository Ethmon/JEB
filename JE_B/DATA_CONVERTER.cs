using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Security.Cryptography;
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
    public partial class Data
    {
        Dictionary<string, string> strings = new Dictionary<string, string>();
        Dictionary<string, double> doubles = new Dictionary<string, double>();
        Dictionary<string, int> integers = new Dictionary<string, int>();
        public Dictionary<string, Data> sheets = new Dictionary<string, Data>();
        Dictionary<string, Line> lines = new Dictionary<string, Line>();
        Dictionary<string, Function> functions = new Dictionary<string, Function>();
        Dictionary<string, file> files = new Dictionary<string, file>();
        Dictionary<string, Method> methods = new Dictionary<string, Method>();
        Dictionary<string, UNIQ> UNIQs = new Dictionary<string, UNIQ>();
        Dictionary<string, list> lists = new Dictionary<string, list>();

        
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
            else if (doubles.ContainsKey(key))
            {
                doubles.Remove(key);
            }
            else if (integers.ContainsKey(key))
            {
                integers.Remove(key);
            }
            else if (sheets.ContainsKey(key))
            {
                sheets.Remove(key);
            }
            else if (custom_types.ContainsKey(key))
            {
                custom_types.Remove(key);
            }
            else if (lines.ContainsKey(key))
            {
                lines.Remove(key);
            }
            else if (functions.ContainsKey(key))
            {
                functions.Remove(key);
            }
            else if (files.ContainsKey(key))
            {
                files.Remove(key);
            }
            else if (methods.ContainsKey(key))
            {
                methods.Remove(key);
            }
            else if (UNIQs.ContainsKey(key))
            {
                UNIQs.Remove(key);
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
            d.lines = new Dictionary<string, Line>(lines);
            d.functions = new Dictionary<string, Function>(functions);
            d.files = new Dictionary<string, file>(files);
            return d;
        }
        public void SaveToFile(string filePath)
        {
            //put a | between each variable
            string stringsData = DictionaryToString(strings,"string");
            string doublesData = DictionaryToString(doubles,"double");
            string integersData = DictionaryToString(integers,"int");
            string customData = DictionaryToString(custom_types,"custom");
            string linesData = DictionaryToString(lines,"line");
            string functionsData = DictionaryToString(functions,"function");
            string filesData = DictionaryToString(files,"file");
            string sheetsData = "";
            foreach (var kvp in sheets)
            {
                sheetsData += kvp.Key + "=" + kvp.Value.identifier + Environment.NewLine;
                kvp.Value.SaveToFile(filePath + "_" + kvp.Key);
            }
            File.WriteAllText(filePath, stringsData + Environment.NewLine + doublesData + Environment.NewLine + integersData + Environment.NewLine + customData + Environment.NewLine + linesData + Environment.NewLine + functionsData + Environment.NewLine + filesData + Environment.NewLine + sheetsData);
        }
        public void save_specific_var(string key, string path)
        {
            //check type of var and save it to file
            // saves as follows
            // variable name : variable value : variable type
            if (strings.ContainsKey(key))
            {
                File.WriteAllText(path, strings[key]);
            }
            else if (doubles.ContainsKey(key))
            {
                File.WriteAllText(path, doubles[key]+"");
            }
            else if (integers.ContainsKey(key))
            {
                File.WriteAllText(path, integers[key] + "");
            }
            else if (sheets.ContainsKey(key))
            {
                sheets[key].SaveToFile(path);
            }
            /*else if (custom_types.ContainsKey(key))
            {
                File.WriteAllText(path, DictionaryToString(custom_types[key]) + ":custom|");
            }*/
            //else if (lines.ContainsKey(key))
            //{
            //    File.WriteAllText(path, lines[key].get_line_number() + "");
            //}
            //else if (functions.ContainsKey(key))
            //{
            //    File.WriteAllText(path, functions[key].get_start_int() + ":" + functions[key].get_end_int() + "");
            //}
            //else if (files.ContainsKey(key))
            //{
            //    File.WriteAllText(path, files[key].get_file_path());
            //}
        }
        public void StringToDictionary<T>(string data, Dictionary<string, T> dictionary)
        {
            string[] keyValuePairs = data.Split('|');
            foreach (string pair in keyValuePairs)
            {
                string[] parts = pair.Split('=');
                string key = parts[0];
                T value = (T)Convert.ChangeType(parts[1], typeof(T));
                dictionary.Add(key, value);
            }
        }
        public void ReadFromFile(string filePath)
        {
            string[] liness = File.ReadAllLines(filePath);
            StringToDictionary(liness[0], strings);
            StringToDictionary(liness[1], doubles);
            StringToDictionary(liness[2], integers);
            StringToDictionary(liness[3], custom_types);
            StringToDictionary(liness[4], lines);
            StringToDictionary(liness[5], functions);
            StringToDictionary(liness[6], files);
            for (int i = 7; i < liness.Length; i++)
            {
                string[] parts = liness[i].Split('=');
                string key = parts[0];
                int value = int.Parse(parts[1]);
                Data d = new Data();
                d.ReadFromFile(filePath + "_" + key);
                sheets.Add(key, d);
            }
        }

        private static string DictionaryToString<T>(Dictionary<string, T> dictionary,string type)
        {
            List<string> keyValuePairs = new List<string>();
            foreach (var kvp in dictionary)
            {
                keyValuePairs.Add($"{kvp.Key}={kvp.Value} " + ":"+type+"|");
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
        public Line referenceLine(string key)
        {
            if (lines.ContainsKey(key))
            {
                return lines[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }
        }
        public Function referenceFunction(string key)
        {
            if (functions.ContainsKey(key))
            {
                return functions[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }
        }
        public file referenceFile(string key)
        {
            if (files.ContainsKey(key))
            {
                return files[key];
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
            if (doubles.ContainsKey(key) || integers.ContainsKey(key) || strings.ContainsKey(key) || sheets.ContainsKey(key)||custom_types.ContainsKey(key) || lines.ContainsKey(key) || functions.ContainsKey(key) || files.ContainsKey(key)|| methods.ContainsKey(key)||UNIQs.ContainsKey(key))
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
        public bool isMethod(string key)
        {
            if (methods.ContainsKey(key))
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
        
        public bool isLine(string key)
        {
            if (lines.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public bool isFunction(string key)
        {
            if (functions.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public bool isFile(string key)
        {
            if (files.ContainsKey(key))
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
            else if (lines.ContainsKey(key))
            {
                return lines[key];
            }
            else if (functions.ContainsKey(key))
            {
                return functions[key];
            }
            else if (files.ContainsKey(key))
            {
                return files[key];
            }
            else if (lists.ContainsKey(key))
            {
                return lists[key];
            }
            else if (UNIQs.ContainsKey(key))
            {
                return UNIQs[key];
            }
            else
            {
                throw new ArgumentException(key + " not initiallized");
            }

        }
        public void setS(string key, string data)
        {
            if (doubles.ContainsKey(key) || integers.ContainsKey(key) || sheets.ContainsKey(key) || custom_types.ContainsKey(key) || lines.ContainsKey(key) || functions.ContainsKey(key) || files.ContainsKey(key))
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
        public void setMethod(string key, string[] code, Type t, Dictionary<string, Object> args)
        {
            if ((isMethod(key) && isvar(key)) || !isvar(key))
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (methods.ContainsKey(key))
                    {
                        methods.Remove(key);
                    }
                    methods.Add(key, new Method(code, t, args));
                }
            }
            else
            {
                Console.WriteLine("variable set to other type");
            }
        }
        public bool islist(string key)
        {
            if (lists.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public void setlist(string key, list data)
        {
            if ((isvar(key) && islist(key)) || !isvar(key))
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (lists.ContainsKey(key))
                    {
                        lists.Remove(key);
                    }
                    lists.Add(key, data);
                }
            }
            else
            {
                Console.WriteLine("variable set to other type");
            }
        }
        public void setLine(string key, Line data)
        {
            if (doubles.ContainsKey(key) || strings.ContainsKey(key)|| integers.ContainsKey(key) || sheets.ContainsKey(key) || custom_types.ContainsKey(key) || files.ContainsKey(key) || functions.ContainsKey(key))
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
                    if (lines.ContainsKey(key))
                    {
                        lines.Remove(key);
                    }
                    lines.Add(key, data);
                }
            }
        }
        public void setFunction(string key, Function data)
        {
            if ((isFunction(key) && isvar(key)) || !isvar(key))
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (functions.ContainsKey(key))
                    {
                        functions.Remove(key);
                    }
                    functions.Add(key, data);
                }
            }
            else
            {
                
                Console.WriteLine("variable set to other type");
            }
        }
        public void setFile(string key, file data)
        {
            if((isFile(key)&&isvar(key))||!isvar(key))
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (files.ContainsKey(key))
                    {
                        files.Remove(key);
                    }
                    files.Add(key, data);
                }
            }
            else
            {
                
                Console.WriteLine("variable set to other type");
            }
        }

        public void setD(string key, double data)
        {
            if ((indouble(key) && isvar(key)) || !isvar(key))
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
            else
            {
                Console.WriteLine("variable set to other type");
            }
        }
        public void setUNIQ(string key,UNIQ data)
        {
            if ((isUNIQ(key) && isvar(key)) || !isvar(key))
            {
                if (Double.TryParse(key, out _))
                {
                    Console.WriteLine("variable name contains only numbers");
                }
                else
                {
                    if (UNIQs.ContainsKey(key))
                    {
                        UNIQs.Remove(key);
                    }
                    UNIQs.Add(key, data);
                }
                
            }
            else
            {
                Console.WriteLine("variable set to other type");
            }
        }
        public bool isUNIQ(string key)
        {
            if(UNIQs.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        public void setCustom(string key, Dictionary<string, Object> data)
        {
            if (isvar(key))
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
            if ((issheet(key) && isvar(key)) || !isvar(key))
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
            else
            {
                
                Console.WriteLine("variable set to other type");
            }
        }
        public void setI(string key, int data)
        {
            if ((inint(key) && isvar(key)) || !isvar(key))
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
            else
            {
                Console.WriteLine("variable set to other type");
            }
        }
        public void SuperSet(string key, Object data)
        {
            remove(key);
            if (data is string)
            {
                setS(key, (string)data);
            }
            else if (data is double)
            {
                setD(key, (double)data);
            }
            else if (data is int)
            {
                setI(key, (int)data);
            }
            else if (data is Data)
            {
                setsheet(key, (Data)data);
            }
            else if (data is Dictionary<string, Object>)
            {
                setCustom(key, (Dictionary<string, Object>)data);
            }
            else if (data is Line)
            {
                setLine(key, (Line)data);
            }
            else if (data is Function)
            {
                setFunction(key, (Function)data);
            }
            else if (data is file)
            {
                setFile(key, (file)data);
            }
            else if (data is Method)
            {
                setMethod(key, ((Method)data).get_code(), ((Method)data).get_type(), ((Method)data).get_args());
            }
            else if(data is UNIQ)
            {
                setUNIQ(key, ((UNIQ)data));
            }
        }
        public Type getType(string key)
        {
            return referenceVar(key).GetType();
        }










        
        public Method referenceMethod(string key)
        {
            if(methods.ContainsKey(key))
            {
                return methods[key];
            }
            throw new ArgumentException(key + " not initiallized");
        }


    }
    public partial class Line
    {
        private int line_number;
        private string line_string;
        public string file_path;
        private Data acsesed_data;
        public Line()
        {
            acsesed_data = new Data();
            line_number = 0;
            file_path = "";
        }
        public Line(int line_num, string filepath)
        {
            acsesed_data = new Data();
            line_number = line_num;
            file_path = filepath;
        }
        public Line(int line_number,string filepath, Data acsesed_data) : this(line_number,filepath)
        {
            this.acsesed_data = acsesed_data;
        }
        public void set_line_number(int line_number)
        {
            this.line_number = line_number;
        }
        public int get_line_number()
        {
            return line_number;
        }
        public Data get_acsesed_data()
        {
            return acsesed_data;
        }
        public void change_acsesed_data(Data acsesed_data)
        {
           this.acsesed_data = acsesed_data;
        }
        public void set_line_string(string line_string)
        {
            this.line_string = line_string;
        }
        public void uses()
        {
            Execute();
        }
        partial void Execute();
    }
    public partial class Function
    {
        private int start_int;
        private int end_int;
        private string function_string;
        public string file_path;
        private Data acsesed_data;
        public Function()
        {
            acsesed_data = new Data();
            start_int = 0;
            end_int = 1;
            file_path = "";
        }
        public Function(int start_int, int end_int, string path)
        {
            acsesed_data = new Data();
            this.start_int = start_int;
            this.end_int = end_int;
            file_path = path;
        }
        public Function(int start_int, int end_int,string path, Data acsesed_data) : this(start_int, end_int, path)
        {
            this.acsesed_data = acsesed_data;
        }
        public void set_start_int(int start_int)
        {
            this.start_int = start_int;
        }
        public int get_start_int()
        {
            return start_int;
        }
        public void Setfunction_string(string function_string)
        {
            this.function_string = function_string;
        }
        public void set_end_int(int end_int)
        {
            this.end_int = end_int;
        }
        public int get_end_int()
        {
            return end_int;
        }
        public Data get_acsesed_data()
        {
            return acsesed_data;
        }
        public void change_acsesed_data(Data acsesed_data)
        {
            this.acsesed_data = acsesed_data;
        }
        public void uses()
        {
            Execute();
        }
        partial void Execute();
    }
    public partial class file
    {
        public string file_path;
        private string file_context;
        private Data acsesed_data;
        public file()
        {
            acsesed_data = new Data();
            file_path = "";
        }
        public file(string file_path)
        {
            acsesed_data = new Data();
            this.file_path = file_path;
        }
        public file(string file_path, Data acsesed_data) : this(file_path)
        {
            this.acsesed_data = acsesed_data;
        }
        public void set_file_path(string file_path)
        {
            this.file_path = file_path;
        }
        public string get_file_path()
        {
            return file_path;
        }
        public Data get_acsesed_data()
        {
            return acsesed_data;
        }
        public void set_context(string file_context)
        {
            this.file_context = file_context;
        }
        public void change_acsesed_data(Data acsesed_data)
        {
            this.acsesed_data = acsesed_data;
        }
        public void uses()
        {
            Execute();
        }
        partial void Execute();
        public static bool operator ==(file f1, file f2)
        {
            if(f1.file_path == f2.file_path)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(file f1, file f2)
        {
            if (f1.file_path != f2.file_path)
            {
                return true;
            }
            return false;
        }
        public static bool operator >(file f1, file f2)
        {
            if (f1.file_path.Length > f2.file_path.Length)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(file f1, file f2)
        {
            if (f1.file_path.Length < f2.file_path.Length)
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(file f1, file f2)
        {
            if (f1.file_path.Length >= f2.file_path.Length)
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(file f1, file f2)
        {
            if (f1.file_path.Length <= f2.file_path.Length)
            {
                return true;
            }
            return false;
        }
        public bool Equals(file f)
        {
            if(this.file_path == f.file_path)
            {
                return true;
            }
            return false;
        }
    }
    public partial class Method
    { 
        // ading in a true methods to the language
        public String[] code;
        public Type ty;
        public Dictionary<string,Object> args;
        public Method(String[] co, Type t, Dictionary<string,Object> Parameters)
        {
            this.code = co;
            this.ty = t;
            this.args = Parameters;
        }
        public string[] get_code()
        {
            return code;
        }
        public Type get_type()
        {
            return ty;
        }
        public Dictionary<string,Object> get_args()
        {
            return args;
        }
        public static bool operator ==(Method m1, Method m2)
        {
            if(m1.code == m2.code && m1.ty == m2.ty && m1.args == m2.args)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Method m1, Method m2)
        {
            if (m1.code != m2.code || m1.ty != m2.ty || m1.args != m2.args)
            {
                return true;
            }
            return false;
        }
        public static bool operator >(Method m1, Method m2)
        {
            if (m1.code.Length > m2.code.Length)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(Method m1, Method m2)
        {
            if (m1.code.Length < m2.code.Length)
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(Method m1, Method m2)
        {
            if (m1.code.Length >= m2.code.Length)
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(Method m1, Method m2)
        {
            if (m1.code.Length <= m2.code.Length)
            {
                return true;
            }
            return false;
        }
        
    }
    public partial class UNIQ
    {
        string code;
        Dictionary<string, Method> methods = new Dictionary<string, Method>();
        Data data = new Data();
        public UNIQ(string code)
        {
            this.code = code;
        }
        public void addMethod(string key, Method method)
        {
            methods.Add(key, method);
        }
        public Method getMethod(string key)
        {
            return methods[key];
        }
        

    }
    public class GB
    {
        public static string GetType(object obj)
        {
            string type = "void";
            if (obj.GetType() == typeof(int))
                type = "int";
            else if (obj.GetType() == typeof(double))
                type = "double";
            else if (obj.GetType() == typeof(string))
                type = "string";
            else if (obj.GetType() == typeof(list))
                type = "list";
            else if (obj.GetType() == typeof(UNIQ))
                type = "UNIQ";
            else if (obj.GetType() == typeof(Method))
                type = "method";


            return type;
        }
    }
    public partial class list : JEnumeral
    { 
        public string t;
        List<object> stuff;
        public list(string t)
        {
            this.t = t;
            stuff = new List<object>();
        }
        public void add(object obj)
        {
            if(GB.GetType(obj) == t)
            {
                stuff.Add(obj);
            }
            else
            {
                throw new ArgumentException("object is not of type " + t.ToString());
            }
        }
        public void remove(object obj)
        {
            if(GB.GetType(obj) == t)
            {
                stuff.Remove(obj);
            }
            else
            {
                throw new ArgumentException("object is not of type " + t.ToString());
            }
        }
        public void remove(int index)
        {
            stuff.RemoveAt(index);
        }
        public object get(int index)
        {
            return stuff[index];
        }
        public void set(int index, object obj)
        {
            if(GB.GetType(obj) != t)
            {
                throw new ArgumentException("object is not of type " + t.ToString());
            }
            stuff[index] = obj;
        }
        public int size()
        {
            return stuff.Count; 
        }
        public void sort()
        {
            stuff.Sort();
        }
        public int find(object obj)
        {
            if(GB.GetType(obj) != t)
            {
                throw new ArgumentException("object is not of type " + t.ToString());
            }
            return stuff.IndexOf(obj);
        }
        public void clear()
        {
            stuff.Clear();
        }
        public override String ToString()
        {
            string returned = "";
            foreach(object i in stuff)
            {
                returned += stuff.ToString() + " ";
            }
            return returned;
        }

        
        public static bool operator ==(list l1, list l2)
        {
            if(l1.size() != l2.size())
            {
                return false;
            }
            for(int i = 0; i < l1.size(); i++)
            {
                if(l1.get(i) != l2.get(i))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool operator !=(list l1, list l2)
        {
            if (l1.size() != l2.size())
            {
                return true;
            }
            for (int i = 0; i < l1.size(); i++)
            {
                if (l1.get(i) != l2.get(i))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool operator >(list l1, list l2)
        {
            if(l1.t!=l2.t)
            {
                throw new ArgumentException("lists are not of the same type");
            }
            switch(l1.t.ToString())
            {
                case ("int"):
                    if (l1.size() == l2.size())
                    {
                        for (int i = 0; i < l1.size(); i++)
                        {
                            if ((int)l1.get(i) < (int)l2.get(i))
                            {
                                return false;
                            }
                        }
                    }
                    if (l1.size() > l2.size())
                    {
                        return true;
                    }
                    break;
                case ("double"):
                    if (l1.size() == l2.size())
                    {
                        for (int i = 0; i < l1.size(); i++)
                        {
                            if ((double)l1.get(i) < (double)l2.get(i))
                            {
                                return false;
                            }
                        }
                    }
                    if (l1.size() > l2.size())
                    {
                        return true;
                    }
                    break;
                case ("string"):
                    if (l1.size() == l2.size())
                    {
                        for (int i = 0; i < l1.size(); i++)
                        {
                            if (String.Compare((string)l1.get(i), (string)l2.get(i)) < 0)
                            {
                                return false;
                            }
                        }
                    }
                    if (l1.size() > l2.size())
                    {
                        return true;
                    }
                    break;
                case ("list"):
                    if (l1.size() == l2.size())
                    {
                        for (int i = 0; i < l1.size(); i++)
                        {
                            if ((list)l1.get(i) < (list)l2.get(i))
                            {
                                return false;
                            }
                        }
                    }
                    if (l1.size() > l2.size())
                    {
                        return true;
                    }
                    break;


            }
            
            return false;
        }
        public static bool operator <(list l1, list l2)
        {
            if (l1.size() < l2.size())
            {
                return true;
            }
            return false;
        }
        public static bool operator >=(list l1, list l2)
        {
            if (l1.size() >= l2.size())
            {
                return true;
            }
            return false;
        }
        public static bool operator <=(list l1, list l2)
        {
            if (l1.size() <= l2.size())
            {
                return true;
            }
            return false;
        }
        public bool Equals(list l)
        {
            if(this == l)
            {
                return true;
            }
            return false;
        }
    }

    public interface JEnumeral
    {
         void add(object obj);
         void remove(object obj);
         void remove(int index);
         object get(int index);
         void set(int index, Object obj);
         int size();
    }
}