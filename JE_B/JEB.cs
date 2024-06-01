using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using DATA_CONVERTER;
using System.Reflection;
//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp;
using Jace;
using System.CodeDom.Compiler;
using System.Security.Policy;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Net;
using System.Runtime.InteropServices;
using jumpE_basic;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;
using static jumpE_basic.base_runner;
using System.Runtime.CompilerServices;

namespace jumpE_basic
{
    class jumpE_basic
    {
        static void Main(string[] args)
        {
            DATA_CONVERTER.Data data = new DATA_CONVERTER.Data();
            bool run = true;
            bool clear_lock = false;
            double floatingvar = 0;
            data.setI("LNT", 0);
            while (run)
            {
                data.setI("LN", 0);
                string hell = Console.ReadLine();
                if (hell == "end")
                {
                    break;
                }
                else if (hell == "clear lock")
                {
                    if (clear_lock)
                    {
                        clear_lock = false;
                        Console.WriteLine(clear_lock);
                    }
                    else
                    {
                        clear_lock = true;
                        Console.WriteLine(clear_lock);
                    }

                }
                else if (hell == "clear")
                {
                    if (clear_lock == false)
                    {
                        DATA_CONVERTER.Data datas = new DATA_CONVERTER.Data();
                        data = datas;
                        Console.WriteLine("CLEAR");
                        base_runner.hard_stop = false;
                    }
                }
                else if (hell == "debug")
                {
                    if(base_runner.debg == true)
                    {
                        base_runner.debg = false;
                    }
                    else if(base_runner.debg == false)
                    {
                        base_runner.debg = true;
                    }
                }
                else if (hell == "run")
                {
                    try
                    {
                        string hells = Console.ReadLine();
                        string fileName = @"" + hells;
                        using (StreamReader streamReader = File.OpenText(fileName))
                        {
                            string text = streamReader.ReadToEnd();
                            base_runner bases = new base_runner(text, data);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                /*else if (hell == "setD")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    Console.WriteLine("variable value");
                    double varval = Double.Parse(Console.ReadLine());
                    data.setD(varname, varval);
                }*/
                else if (hell == "help")
                {
                    Console.WriteLine("_-----------_ \n run \n clear \n refD \n refS \n add \n save \n clear lock \n ref \n folder \n_-----------_ ");
                }
                /*else if (hell == "setS")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    Console.WriteLine("variable value");
                    string varval = Console.ReadLine();
                    data.setS(varname, varval);
                }*/
                else if (hell == "refD")
                {

                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    if (data.indouble(varname))
                    {
                        floatingvar = data.referenceD(varname);
                        Console.WriteLine(floatingvar);
                    }
                    else
                    {
                        Console.WriteLine("not a double");
                    }

                }
                else if (hell == "refS")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    if (data.instring(varname))
                    {
                        Console.WriteLine(data.referenceS(varname));
                    }
                    else
                    {
                        Console.WriteLine("not a string");
                    }
                }
                else if (hell == "ref")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    if (data.isvar(varname))
                    {
                        Console.WriteLine(data.referenceVar(varname));
                    }
                    else
                    {
                        Console.WriteLine("not an initiallized variable");
                    }
                }
                /*else if (hell == "add")
                {
                    Console.WriteLine("name of variable");
                    string varname = Console.ReadLine();
                    double fla = data.referenceD(varname);
                    data.setD(varname, fla + floatingvar);
                }*/
                else if (hell == "folder")
                {
                    string folderPath = @"";
                    folderPath = Console.ReadLine();
                    if (Directory.Exists(folderPath))
                    {
                        string[] files = Directory.GetFiles(folderPath);
                        foreach (string filePath in files)
                        {
                            Console.WriteLine("File Name: " + Path.GetFileName(filePath));
                            Console.WriteLine("File Path: " + filePath);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("The specified folder does not exist.");
                    }
                }
                else if (hell == "save")
                {
                    Console.WriteLine("name of file");
                    string filename = Console.ReadLine();
                    filename += ".txt";
                    data.SaveToFile(filename);
                }
                else
                {
                    Console.WriteLine("NOT A COMMAND");
                }

            }

        }
    }
    public class base_runner
    {
        public Object return_value = null;
        public List<string> code = new List<string>();
        public List<string> lines = new List<string>();
        public string taken_in_string;
        public List<int> positions = new List<int>();
        public CommandRegistry commandRegistry = new CommandRegistry();
        DATA_CONVERTER.Data data;
        public List<DATA_CONVERTER.Data> datas = new List<Data>();
        DATA_CONVERTER.command_centralls interorouter = new DATA_CONVERTER.command_centralls();
        public int position;
        public bool run;
        private static int real_count = 0;
        public static bool debg = false;
        public static bool hard_stop = false;
        //public string data_storage = "@";
        private new pre_defined_variable f = new pre_defined_variable();

        public base_runner(string taken, DATA_CONVERTER.Data data)
        {
            this.taken_in_string = taken;
            this.lines = SimpleTokenizer.Linizer(this.taken_in_string);
            this.position = 0;
            this.run = true;
            datas.Add(data);
            data.setI("LNT", 0);

            while (this.run)
            {
                if (debg)
                {
                    Console.WriteLine(lines[position] + "   " + real_count);
                    Console.ReadLine();

                }
                if (hard_stop)
                {
                    this.run = false;
                    break;

                }
                //try
                {
                    this.code = SimpleTokenizer.Tokenizer(this.lines[this.position]);//get all commands in the line.
                }
                //catch { Console.WriteLine("Error: 2, Line not recognized"); }
                data.setI("LNC", this.position);// set line number for use as a variable in the code
                data.setI("LNT", data.referenceI("LNT") + 1);//total amount of lines that have been run per session of the data converter
                if (commandRegistry.ContainsCommand(this.code[0]))
                {
                    // distinguish between inner and outer commands, inner commands are commands that are built into the Basic interpreter (inner commands can allso be imported using useC),outer commands are commands that are imported using use.
                    interorouter = commandRegistry.GetCommandDefinition(this.code[0]);
                    if (interorouter is command_centrall)
                    {
                        ((command_centrall)(interorouter)).Execute(this.code, datas[datas.Count() - 1], this);
                    }
                    if (interorouter is outer_commands)
                    {
                        ((outer_commands)(interorouter)).Execute(this.code, datas[datas.Count() - 1]);
                    }


                }
                else if (datas[datas.Count() - 1].isvar(this.code[0]) && !(lines[position] == "{" || lines[position] == "}" || code[0] == "<<" || code[0] == ">>"))
                {
                    f.Execute(this.code, datas[datas.Count() - 1], this);
                }
                else
                {
                    //Console.WriteLine("Error: 1, command not recognized, Line " + position);
                }
                if (this.run == false)
                {
                    break;
                }
                //Debug.WriteLine(this.lines[this.position]);
                if (this.lines.Count >= this.position + 1)
                {
                    this.position++;
                    real_count++;
                }
                else
                {
                    break;
                }




            }
        }

        public void changePosition(int a)
        {
            this.position = a;
        }
        public int get_position()
        {
            return this.position;
        }
        public void STOP()
        {
            this.run = false;
        }
        public class SimpleTokenizer
        {
            public static List<string> Linizer(string input)
            {
                List<string> words = new List<string>();
                string[] lines = input.Split(new char[] { '\n', ';', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    words.Add(line);
                }

                return words;
            }
            public static List<string> Tokenizer(string input)
            {
                List<string> words = new List<string>();
                string[] tokens = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in tokens)
                {
                    words.Add(line);
                }

                return words;
            }
            public static List<string> comandizer(string input)
            {
                List<string> words = new List<string>();
                string[] tokens = input.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in tokens)
                {
                    words.Add(line);
                }

                return words;
            }
            // just removes all '\t' and ' ' from the string
            public static string no_tab_spaces(string input)
            {
                return Regex.Replace(input, @"[\t ]", "");
            }
            // this returns lists of all statments for if statments. thses include not, or, and, and but
            public static List<List<string>> Statementizer(List<string> input)
            {
                int start = 1; // Start from the second word
                List<List<string>> output = new List<List<string>>();

                for (int i = 1; i < input.Count; i++) // Start loop from 1
                {
                    if (input[i] == "not" || input[i] == "or" || input[i] == "and" || input[i] == "nor")
                    {
                        output.Add(input.GetRange(start, i - start));
                        start = i; // Move the start to the next index
                    }
                }

                // Add the last statement
                if (start < input.Count)
                {
                    output.Add(input.GetRange(start, input.Count - start));
                }

                return output;
            }

        }
        public class CommandRegistry
        {
            public Dictionary<string, DATA_CONVERTER.command_centralls> commands = new Dictionary<string, DATA_CONVERTER.command_centralls>();
            public CommandRegistry()
            {
                print print = new print();//prints to console, can use variables and strings by putting them in quotes(must be seperated by spaces)
                //whenD whend = new whenD();
                //whenS whens = new whenS();
                //setS setS = new setS();
                use use = new use(this);//imports a command from a .cs file, you can either use the file path or put it in the same file as the .exe, these commands only include logic and do not have access to the data converter, they can be used to create neouter commands
                //setD setD = new setD();
                //add add = new add();
                end end = new end();//end the program
                //subtract subtract = new subtract();
                //multiply multiply = new multiply();
                //divide divide = new divide();
                sideLayer sideLayer = new sideLayer();
                raise raise = new raise();
                push push = new push();
                pop pop = new pop();
                callLayer callLy = new callLayer();
                jump jump = new jump();//jumps to a line number or calls a function use "JP >> {function name}" to call a function
                inputD inputD = new inputD();//takes a double input and stores it in a variable, variable must allready be initalized
                comment comment = new comment();// used to comment out lines of code, can be used with // or #
                inputS inputS = new inputS();//takes a string input and stores it in a variable, variable must allready be initalized
                inputI inputI = new inputI();//takes a int input and stores it in a variable, variable must allready be initalized
                useC useC = new useC(this);//this is the full on stuff, this is used to just import code from a .cs file, can create both inner and outer commands, 
                line_number line_number = new line_number();// returns the current line number to pre initalize variables. this is an alternatite to LNC
                pre_defined_variable Math_equation = new pre_defined_variable();// this is not relivent to most, used for variables that have allready been initalzed, can be used to do math with variables
                double_func double_func = new double_func(Math_equation, this);//this initalizes a variable as a double, can be used to do math with variables
                string_func string_Func = new string_func(Math_equation, this);//this initalizes a variable as a string, can not be used to do math with variables
                int_func int_func = new int_func(Math_equation, this);// this initalizes a variable as a int, can be used to do math with variables
                when when = new when(Math_equation, this);//logic gate, can be used to create if statements, can be used to create while loops, can be used to create for loops, uses {}
                return_func return_Func = new return_func();// returns to the last jump point, can be used to return from a function
                commands.Add("return", return_Func); commands.Add("Return", return_Func); commands.Add("RETURN", return_Func); commands.Add("<<", return_Func);
                commands.Add("when", when); commands.Add("When", when); commands.Add("if", when);
                commands.Add("useC", useC); commands.Add("usec", useC);
                commands.Add("print", print); commands.Add("Print", print);
                commands.Add("inputI", inputI); commands.Add("inputi", inputI); commands.Add("InputI", inputI);
                //commands.Add("whenD", whend); commands.Add("WhenD", whend);
                commands.Add("inputS", inputS); commands.Add("inputs", inputS); commands.Add("InputS", inputS);
                //commands.Add("setS", setS); commands.Add("SetS", setS);
                commands.Add("string", string_Func); commands.Add("String", string_Func); commands.Add("STRING", string_Func);
                commands.Add("int", int_func); commands.Add("INT", int_func);
                //commands.Add("whenS", whens); commands.Add("WhenS", whens);
                commands.Add("jump", jump); commands.Add("jp", jump); commands.Add("JP", jump); commands.Add("JUMP", jump);
                commands.Add("double", double_func); commands.Add("DOUBLE", double_func); commands.Add("Double", double_func);
                /*commands.Add("subtract", subtract); commands.Add("sub", subtract);
                commands.Add("multiply", multiply); commands.Add("mult", multiply);
                commands.Add("divide", divide); commands.Add("div", divide);*/
                commands.Add("end", end); commands.Add("stop", end); commands.Add("END", end);
                commands.Add("inputD", inputD); commands.Add("inputd", inputD); commands.Add("InputD", inputD);
                commands.Add("use", use);
                commands.Add("line_number", line_number); commands.Add("ln", line_number); commands.Add("LN", line_number);
                commands.Add("comment", comment); commands.Add("//", comment); commands.Add("#", comment);
                commands.Add("raise", raise); commands.Add("push", push); commands.Add("pop", pop);
                commands.Add("IDD", new IDD()); commands.Add("IDT", new IDT());
                commands.Add("free", new free());
                commands.Add("skip", new Skip());
                commands.Add("sideLayer", sideLayer); commands.Add("remL", new remL()); commands.Add("callLayer", callLy); commands.Add("bring", new bring());
                commands.Add("raiseS", new raiseS()); commands.Add("raiseSA", new raiseSA());
                commands.Add("bringA", new bringA()); commands.Add("pushA", new pushA());
                commands.Add("pushDL", new pushDL());
                commands.Add("Line", new Line_func(Math_equation, this));
                commands.Add("File", new File_func());
                commands.Add("Function", new Function_func(Math_equation, this));
                commands.Add("bringDL", new bringDL());
                commands.Add("HS", new Hard_stop());
                commands.Add("save", new save());
                commands.Add("method", new Method_instantiate());
                // list all commands here :
                // return, Return , RETURN, <<, when, When, if, useC, usec, print, Print, inputI, inputi, InputI, inputS, inputs, InputS, string, String, STRING, int, INT, whenS, WhenS, jump, jp, JP, JUMP, double, DOUBLE, Double, end, stop, END, inputD, inputd, InputD, use, line_number, ln, LN, comment, //, #, raise, push, pop, IDD, IDT, free, skip, sideLayer, remL, callLayer, bring, raiseS, raiseSA, bringA, pushA, pushDL, Line, File, Function, bringDL, HS, 
                // list all commands that refer to sheets here : 
                // sideLayer, remL, callLayer, bring, raiseS, raiseSA, bringA, pushA, pushDL, raise, IDD, IDT, free, pushGOD, bringDL
                // List all eventing commands here :
                // Line, File, Function
                // List all variable declerations here :
                // double, DOUBLE, Double, string, String, STRING, int, INT, Line , File, Function
                // List all logic commands here :
                // when, When, if, skip
                // List all input commands here :
                // inputI, inputi, InputI, inputS, inputs, InputS, inputD, inputd, InputD
                // List all commands that are used to manipulate the stack here :
                // raise, push, pop, bring, raiseS, raiseSA, bringA, pushA, pushDL, callLayer, sideLayer, pushGOD, bringDL
                // List all commands that are used to manipulate the file system here :
                // Line, File, Function, use, useC, usec
                // List all commands that are used to manipulate the console here :
                // print, Print
                // List all commands that are used to manipulate the program here :
                // jump, jp, JP, JUMP, end, stop, END, return, Return , RETURN, <<, comment, //, #, HS
                // List all commands that are Alocate memory here :
                // sideLayer, remL, callLayer
                // List all commands that are used to manipulate the program flow here :
                // jump, jp, JP, JUMP, end, stop, END, return, Return , RETURN, <<, when, When, if, skip, HS
                // List all commands that are used to manipulate the data converter here :
                // IDD, IDT, free, raise, push, pop, bring, raiseS, raiseSA, bringA, pushA, pushDL pushGOD, bringDL


            }
            public void add_command(string name, command_centralls type)
            {
                if (commands.ContainsKey(name))
                {
                    commands.Remove(name);
                }
                commands.Add(name, type);
            }
            public bool ContainsCommand(string command)
            {
                if (commands.ContainsKey(command))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public command_centralls GetCommandDefinition(string commandName)
            {
                if (ContainsCommand(commandName))
                {
                    return commands[commandName];
                }
                else
                {
                    return null;
                }
            }
        }
        public class Hard_stop : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                base_runner.hard_stop = true;
            }
        }
        public class command_centrall : command_centralls
        {
            public virtual void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                Debug.WriteLine("eh");
            }
        }
        public class Skip : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                int w = 0;
                int q = Base.position + 1;
                while (true)
                {


                    if (SimpleTokenizer.no_tab_spaces(Base.lines[q]) == "{")
                    {
                        w++;
                    }
                    if (SimpleTokenizer.no_tab_spaces(Base.lines[q]) == "}")
                    {
                        if (w == 1)
                        {
                            Base.changePosition(q);
                            break;
                        }
                        else if (w != 0)
                        {
                            w--;
                        }
                    }
                    q++;
                }
            }
        }
        public class pushGOD : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (D.inint(code[1]))
                {
                    Base.datas[0].setI(code[1], D.referenceI(code[1])); //Base.commandRegistry.add_command(code[1], f); 
                }
                else if (D.indouble(code[1]))
                {
                    Base.datas[0].setD(code[1], D.referenceD(code[1])); //Base.commandRegistry.add_command(code[1], f); 
                }
                else if (D.instring(code[1]))
                {
                    Base.datas[0].setS(code[1], D.referenceS(code[1])); //Base.commandRegistry.add_command(code[1], f);                                                                                                          
                }
                else if (D.issheet(code[1]))
                {
                    Base.datas[0].setsheet(code[1], D.referenceSheet(code[1]));
                }
                else if (D.issheet(code[1] + "#"))
                {
                    Base.datas[0].setsheet(code[1], D.referenceSheet(code[1] + "#"));
                }
            }
        }
        public class comment : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
            }
        }
        public class print : command_centrall
        {

            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    //int color = 15;
                    string Message = "";

                    for (int i = 1; i < code.Count; i++)
                    {
                        if (code[i] == "\\Clear")
                        {
                            Console.Clear();
                        }
                        else if (code[i] == "\\n")
                        {
                            Console.WriteLine();
                        }
                        else if (code[i] == "\"" && code[i + 2] == "\"")
                        {
                            Message += (D.referenceVar(code[i + 1]));

                            i += 2;
                        }
                        else if (code[i] == "!S!")
                        {
                            Message += " ";
                        }
                        else if (code[i] == "\\!S!")
                        {
                            Message += "!S!";
                        }
                        else if (code[i] == "~|~")
                        {
                            int kk = 0;
                            if (D.isnumvar(code[i + 1]))
                            {
                                kk += (int)D.referenceVar(code[i + 1]);
                            }
                            else
                            {
                                kk += int.Parse(code[i + 1]);
                            }
                            if (kk == 0)
                                Console.ResetColor();
                            if (kk == 1)
                                Console.ForegroundColor = ConsoleColor.Black;
                            if (kk == 2)
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                            if (kk == 3)
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            if (kk == 4)
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                            if (kk == 5)
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            if (kk == 6)
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            if (kk == 7)
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            if (kk == 8)
                                Console.ForegroundColor = ConsoleColor.Gray;
                            if (kk == 9)
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            if (kk == 10)
                                Console.ForegroundColor = ConsoleColor.Blue;
                            if (kk == 11)
                                Console.ForegroundColor = ConsoleColor.Green;
                            if (kk == 12)
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            if (kk == 13)
                                Console.ForegroundColor = ConsoleColor.Red;
                            if (kk == 14)
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            if (kk == 15)
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            if (kk == 16)
                                Console.ForegroundColor = ConsoleColor.White;
                            i++;
                            Console.Write(Message);
                            Message = "";
                        }
                        else if (code[i] == "|~|")
                        {
                            int kk = 0;
                            if (D.isnumvar(code[i + 1]))
                            {
                                kk += (int)D.referenceVar(code[i + 1]);
                            }
                            else
                            {
                                kk += int.Parse(code[i + 1]);
                            }
                            if (kk == 0)
                                Console.ResetColor();
                            if (kk == 1)
                                Console.BackgroundColor = ConsoleColor.Black;
                            if (kk == 2)
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            if (kk == 3)
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                            if (kk == 4)
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
                            if (kk == 5)
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            if (kk == 6)
                                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            if (kk == 7)
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                            if (kk == 8)
                                Console.BackgroundColor = ConsoleColor.Gray;
                            if (kk == 9)
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            if (kk == 10)
                                Console.BackgroundColor = ConsoleColor.Blue;
                            if (kk == 11)
                                Console.BackgroundColor = ConsoleColor.Green;
                            if (kk == 12)
                                Console.BackgroundColor = ConsoleColor.Cyan;
                            if (kk == 13)
                                Console.BackgroundColor = ConsoleColor.Red;
                            if (kk == 14)
                                Console.BackgroundColor = ConsoleColor.Magenta;
                            if (kk == 15)
                                Console.BackgroundColor = ConsoleColor.Yellow;
                            if (kk == 16)
                                Console.BackgroundColor = ConsoleColor.White;
                            i++;
                            Console.Write(Message);
                            Message = "";
                        }
                        else if (code[i] == "M#" && code[i + 1] == "#")
                        {
                            string equation = "";
                            for (int ll = i; ll < code.Count; ll++)
                            {
                                if (code[ll] == "#" && code[ll + 1] == "#M")
                                {
                                    i = ll + 1;
                                    break;
                                }
                                double j;
                                if (Double.TryParse(code[ll], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (code[ll] == "+" || code[ll] == "-" || code[ll] == "/" || code[ll] == "*" || code[ll] == "sin" || code[ll] == "cos" || code[ll] == "%" || code[ll] == "tan" ||
                                code[ll] == "csc" || code[ll] == "sec" || code[ll] == "cot" || code[ll] == "root" || code[ll] == ")" || code[ll] == "(" || code[ll] == " ")
                                {
                                    equation += code[ll] + " ";
                                }
                                else if (D.isnumvar(code[ll]))
                                {
                                    equation += D.referenceVar(code[ll]) + " ";
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            Message += engine.Calculate(equation);
                        }
                        else
                        {
                            Message += (code[i]);
                        }

                    }
                    Console.Write(Message);
                }
                catch
                {
                    Console.WriteLine("Error: printing error, Line " + Base.position);
                }



            }
        }
        public class IDD : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (code[1] == "\"")
                {
                    D.identifier = D.referenceI(code[2]);
                }
                else
                {
                    D.identifier = int.Parse(code[1]);
                }
            }
        }
        public class IDT : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (code[1] == "\"")
                {
                    D.typeidentifier = D.referenceI(code[2]);
                }
                else
                {
                    D.typeidentifier = int.Parse(code[1]);
                }
            }
        }
        public class save : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                // save to file named at code[1]
                // if code[2] == "all" save all data
                // if code[2] = a variable save that variable
                // addd on the txt extension
                // the format for the save is as follows
                // variable name : variable value : variable type
                if (code[2] == "all")
                {
                    D.SaveToFile(code[2] + ".txt");
                }
                else if (D.isvar(code[2]))
                {
                    D.save_specific_var(code[2] + ".txt", code[1]);
                }


            }
        }
        public class read : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                // read from file named at code[1]
                // if code[2] == "all" read all data
                // if code[2] = a variable read that variable
                // addd on the txt extension
                // the format for the save is as follows
                // variable name : variable value : variable type
                if (code[2] == "all")
                {
                    D.ReadFromFile(code[2] + ".txt");
                }
            }
        }
        public class free : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                //try
                /*if (Base.commandRegistry.ContainsCommand(code[1]))
                {
                    Base.commandRegistry.commands.Remove(code[1]);

                }*/
                if (D.isvar(code[1]))
                {
                    D.remove(code[1]);
                }
                //} catch { Console.WriteLine("Error: 4, Unable to Free, Line"+Base.position); }
            }
        }
        public class raise : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                Base.datas.Add(D.Copy());

            }
        }
        public class raiseS : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                Data datas = new Data();
                for (int i = 1; i < code.Count; i++)
                {
                    if (D.isvar(code[i]))
                    {
                        if (D.inint(code[i]))
                        {
                            datas.setI(code[i], D.referenceI(code[i]));
                        }
                        else if (D.indouble(code[i]))
                        {
                            datas.setD(code[i], D.referenceD(code[i]));
                        }
                        else if (D.instring(code[i]))
                        {
                            datas.setS(code[i], D.referenceS(code[i]));
                        }
                        else if (D.issheet(code[i]))
                        {
                            datas.setsheet(code[i], D.referenceSheet(code[i]));
                        }
                    }
                }
                Base.datas.Add(datas);

            }
        }
        public class raiseSA : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                Data datas = new Data();
                for (int i = 1; i < code.Count; i += 2)
                {
                    if (D.isvar(code[i]))
                    {
                        if (D.inint(code[i]))
                        {
                            datas.setI(code[i + 1], D.referenceI(code[i]));
                        }
                        else if (D.indouble(code[i]))
                        {
                            datas.setD(code[i + 1], D.referenceD(code[i]));
                        }
                        else if (D.instring(code[i]))
                        {
                            datas.setS(code[i + 1], D.referenceS(code[i]));
                        }
                        else if (D.issheet(code[i]))
                        {
                            datas.setsheet(code[i + 1], D.referenceSheet(code[i]));
                        }
                    }
                    if (int.TryParse(code[i], out int ad))
                    {
                        if (ad == double.Parse(code[i]))
                        {
                            datas.setI(code[i + 1], ad);
                        }
                        else
                        {
                            datas.setD(code[i + 1], double.Parse(code[i]));
                        }
                    }
                }
                Base.datas.Add(datas);

            }
        }

        public class sideLayer : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                //try
                {
                    Data data = new Data();

                    if (code[1] == "\"" && code[3] == "\"")
                    {
                        D.setsheet(D.referenceS(code[2]) + "#", data);
                    }
                    else
                    {
                        D.setsheet(code[1], data);
                    }
                    Base.datas.Add(data);
                }
                //catch { Console.WriteLine("Error: 5, Sheet error, Line "+Base.position); }

            }
        }
        public class remL : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (code[1] == "\"" && code[3] == "\"")
                {
                    D.setsheet(D.referenceS(code[2]) + "#", D);
                }
                else
                {
                    D.setsheet(code[1], D);
                }
            }
        }
        public class callLayer : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (code[1] == "\"" && code[3] == "\"")
                {
                    Base.datas.Add(D.referenceSheet(D.referenceS(code[2]) + "#"));
                }
                else if (D.issheet(code[1]))
                {
                    Base.datas.Add(D.referenceSheet(code[1]));
                }
                else
                {
                    //Console.WriteLine("Error: 6, Sheet unable to be called, Line "+Base.position);
                }
                //Console.WriteLine(D.sheets);
            }
        }
        public class bring : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (Base.datas[Base.datas.Count - 2].inint(code[1]))
                {
                    D.setI(code[1], Base.datas[Base.datas.Count - 2].referenceI(code[1]));
                }
                else if (Base.datas[Base.datas.Count - 2].indouble(code[1]))
                {
                    D.setD(code[1], Base.datas[Base.datas.Count - 2].referenceD(code[1]));
                }
                else if (Base.datas[Base.datas.Count - 2].instring(code[1]))
                {
                    D.setS(code[1], Base.datas[Base.datas.Count - 2].referenceS(code[1]));
                }
                else if (Base.datas[Base.datas.Count - 2].issheet(code[1]))
                {
                    D.setsheet(code[1], Base.datas[Base.datas.Count - 2].referenceSheet(code[1]));
                }
                //adding lines functions and files

                //else { Console.WriteLine("Error: 7, unable to bring, Line "+Base.position); }
            }
        }
        public class bringA : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (Base.datas[Base.datas.Count - 2].inint(code[1]))
                {
                    D.setI(code[2], Base.datas[Base.datas.Count - 2].referenceI(code[1]));
                }
                else if (Base.datas[Base.datas.Count - 2].indouble(code[1]))
                {
                    D.setD(code[2], Base.datas[Base.datas.Count - 2].referenceD(code[1]));
                }
                else if (Base.datas[Base.datas.Count - 2].instring(code[1]))
                {
                    D.setS(code[2], Base.datas[Base.datas.Count - 2].referenceS(code[1]));
                }
                else if (Base.datas[Base.datas.Count - 2].issheet(code[1]))
                {
                    D.setsheet(code[2], Base.datas[Base.datas.Count - 2].referenceSheet(code[1]));
                }
                //else { Console.WriteLine("Error: 7, unable to bring, Line "+Base.position); }
            }
        }
        public class bringDL : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (Base.datas[Base.datas.Count - 2].instring(code[1]))
                {
                    if (Base.datas[Base.datas.Count - 2].issheet(Base.datas[Base.datas.Count - 2].referenceS(code[1]) + "#"))
                    {
                        D.setsheet(code[2], Base.datas[Base.datas.Count - 2].referenceSheet(Base.datas[Base.datas.Count - 2].referenceS(code[1]) + "#"));
                        return;
                    }
                }
                D.setsheet(code[2], Base.datas[Base.datas.Count - 2]);
            }
        }
        public class push : command_centrall
        {
            //pre_defined_variable f = new pre_defined_variable();
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (D.inint(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setI(code[1], D.referenceI(code[1])); //Base.commandRegistry.add_command(code[1], f); 
                }
                else if (D.indouble(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setD(code[1], D.referenceD(code[1])); //Base.commandRegistry.add_command(code[1], f); 
                }
                else if (D.instring(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setS(code[1], D.referenceS(code[1])); //Base.commandRegistry.add_command(code[1], f);                                                                                                          
                }
                else if (D.issheet(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setsheet(code[1], D.referenceSheet(code[1]));
                }
                else if (D.issheet(code[1] + "#"))
                {
                    Base.datas[Base.datas.Count() - 2].setsheet(code[1], D.referenceSheet(code[1] + "#"));
                }
                //else { Console.WriteLine("Error: 8, unable to push, Line "+Base.position); }
                //else if (D.isvar(code[1])) { Base.datas[Base.datas.Count - 1].se(code[1], D.referenceI(code[1])); }

            }
        }
        public class pushA : command_centrall
        {
            //pre_defined_variable f = new pre_defined_variable();
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (D.inint(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setI(code[2], D.referenceI(code[1])); //Base.commandRegistry.add_command(code[1], f); 
                }
                else if (D.indouble(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setD(code[2], D.referenceD(code[1])); //Base.commandRegistry.add_command(code[1], f); 
                }
                else if (D.instring(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setS(code[2], D.referenceS(code[1])); //Base.commandRegistry.add_command(code[1], f);                                                                                                          
                }
                else if (D.issheet(code[1]))
                {
                    Base.datas[Base.datas.Count() - 2].setsheet(code[2], D.referenceSheet(code[1]));
                }
                //else { Console.WriteLine("Error: 8, unable to push, Line "+Base.position); }
                //else if (D.isvar(code[1])) { Base.datas[Base.datas.Count - 1].se(code[1], D.referenceI(code[1])); }

            }
        }
        public class pushDL : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (D.instring(code[1]))
                {
                    if (D.issheet(D.referenceS(code[1] + "#")))
                    {
                        Base.datas[Base.datas.Count() - 2].setsheet(D.referenceS(code[1] + "#"), D);
                        return;
                    }
                }
                Base.datas[Base.datas.Count() - 2].setsheet(code[1], D);
            }
        }
        public class pop : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                //try
                {
                    Base.datas.RemoveAt(Base.datas.Count - 1);
                }
                //catch { Console.WriteLine("Error: 9, pop overflux, Line " + Base.position); }
            }
        }
        public class when : command_centrall
        {
            pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public when(pre_defined_variable j, CommandRegistry c)
            {
                this.Math_equation = j;
                this.commands = c;

            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {

                bool ors = false;
                bool ands = true;
                bool orsD = false;
                bool endresult = false;
                List<List<string>> statments = SimpleTokenizer.Statementizer(code);
                for (int iff = 0; iff < statments.Count(); iff++)
                {
                    //try
                    {
                        if (statments[iff].Count() < 2)
                        {
                            continue;
                        }
                        Boolean result = false;
                        string equation = "";
                        /*if (code.Count() == 2)
                        {
                            D.setI(code[1], 0);
                            this.commands.add_command(code[1], this.Math_equation);

                        }*/ // this will be for booleans when i get around to 
                            //else { }
                        if (statments[iff][1] == "str")
                        {
                            if (D.referenceS(statments[iff][2]).Equals(D.referenceS(statments[iff][3])))
                                result = true;
                            if (statments[iff][0] == "or" || statments[iff][0] == "nor")
                            {
                                orsD = true;
                            }
                            if (result && statments[iff][0] == "or" && !ors)
                            {
                                ors = true;
                            }
                            if (!result && statments[iff][0] == "nor" && !ors)
                            {
                                ors = true;
                            }
                            if (!result && ands && statments[iff][0] == "and")
                            {
                                ands = false;
                            }
                            if (result && ands && statments[iff][0] == "not")
                            {
                                ands = false;
                            }
                        }
                        else if (statments[iff][1] == "typ")
                        {
                            int a = 0;
                            int b = -1;
                            if (D.inint(statments[iff][2]))
                            {
                                a = D.referenceI(statments[iff][2]);
                            }
                            else if (D.issheet(statments[iff][2]))
                            {
                                a = D.referenceSheet(statments[iff][2]).typeidentifier;
                            }
                            else if (D.instring(statments[iff][2]))
                            {
                                if (D.issheet(D.referenceS(statments[iff][2]) + "#"))
                                {
                                    a = D.referenceSheet(D.referenceS(statments[iff][2]) + "#").typeidentifier;
                                }
                            }
                            else if (int.TryParse(statments[iff][2], out int ad))
                            {
                                a = ad;
                            }
                            if (D.inint(statments[iff][3]))
                            {
                                b = D.referenceI(statments[iff][3]);
                            }
                            else if (D.issheet(statments[iff][3]))
                            {
                                b = D.referenceSheet(statments[iff][3]).typeidentifier;
                            }
                            else if (D.instring(statments[iff][3]))
                            {
                                if (D.issheet(D.referenceS(statments[iff][3]) + "#"))
                                {
                                    b = D.referenceSheet(D.referenceS(statments[iff][3]) + "#").typeidentifier;
                                }
                            }
                            else if (int.TryParse(statments[iff][3], out int bd))
                            {
                                b = bd;
                            }
                            if (b == a)
                            {
                                result = true;
                            }
                            if (statments[iff][0] == "or" || statments[iff][0] == "nor")
                            {
                                orsD = true;
                            }
                            if (result && statments[iff][0] == "or" && !ors)
                            {
                                ors = true;
                            }
                            if (!result && statments[iff][0] == "nor" && !ors)
                            {
                                ors = true;
                            }
                            if (!result && ands && statments[iff][0] == "and")
                            {
                                ands = false;
                            }
                            if (result && ands && statments[iff][0] == "not")
                            {
                                ands = false;
                            }
                        }
                        else if (statments[iff][1] == "ver")
                        {
                            int a = 0;
                            int b = -1;
                            if (D.inint(statments[iff][2]))
                            {
                                a = D.referenceI(statments[iff][2]);
                            }
                            else if (D.issheet(statments[iff][2]))
                            {
                                a = D.referenceSheet(statments[iff][2]).identifier;
                            }
                            else if (D.instring(statments[iff][2]))
                            {
                                if (D.issheet(D.referenceS(statments[iff][2]) + "#"))
                                {
                                    a = D.referenceSheet(D.referenceS(statments[iff][2]) + "#").identifier;
                                }
                            }
                            else if (int.TryParse(statments[iff][2], out int ad))
                            {
                                a = ad;
                            }
                            if (D.inint(statments[iff][3]))
                            {
                                b = D.referenceI(statments[iff][3]);
                            }
                            else if (D.issheet(statments[iff][3]))
                            {
                                b = D.referenceSheet(statments[iff][3]).identifier;
                            }
                            else if (D.instring(statments[iff][3]))
                            {
                                if (D.issheet(D.referenceS(statments[iff][3]) + "#"))
                                {
                                    b = D.referenceSheet(D.referenceS(statments[iff][3]) + "#").identifier;
                                }
                            }
                            else if (int.TryParse(statments[iff][3], out int bd))
                            {
                                b = bd;
                            }
                            if (b == a)
                            {
                                result = true;
                            }
                            if (statments[iff][0] == "or" || statments[iff][0] == "nor")
                            {
                                orsD = true;
                            }
                            if (result && statments[iff][0] == "or" && !ors)
                            {
                                ors = true;
                            }
                            if (!result && statments[iff][0] == "nor" && !ors)
                            {
                                ors = true;
                            }
                            if (!result && ands && statments[iff][0] == "and")
                            {
                                ands = false;
                            }
                            if (result && ands && statments[iff][0] == "not")
                            {
                                ands = false;
                            }
                        }

                        else
                        {
                            for (int i = 1; i < statments[iff].Count(); i++)
                            {
                                double j;
                                if (Double.TryParse(statments[iff][i], out j))
                                {
                                    equation += j + " ";
                                }
                                else if (statments[iff][i] == "+" || statments[iff][i] == "-" || statments[iff][i] == "/" || statments[iff][i] == "*" || statments[iff][i] == "sin" || statments[iff][i] == "cos" || statments[iff][i] == "tan" ||
                                statments[iff][i] == "csc" || statments[iff][i] == "sec" || statments[iff][i] == "cot" || statments[iff][i] == "root" || statments[iff][i] == ")" || statments[iff][i] == "(" || statments[iff][i] == " " || statments[iff][i] == "==" || statments[iff][i] == "!=" || statments[iff][i] == ">" || statments[iff][i] == "<" ||
                                statments[iff][i] == "=>" || statments[iff][i] == "=<" || statments[iff][i] == "!")
                                {
                                    equation += statments[iff][i] + " ";
                                }
                                else if (D.isnumvar(statments[iff][i]))
                                {
                                    equation += D.referenceVar(statments[iff][i]) + " ";
                                }
                                else
                                {
                                    equation += statments[iff][i] + " ";
                                    //Debug.WriteLine("not recognized when statement");
                                }
                            }
                            CalculationEngine engine = new CalculationEngine();
                            result = Convert.ToBoolean(engine.Calculate(equation));
                            //Console.WriteLine(result);

                            if (statments[iff][0] == "or" || statments[iff][0] == "nor")
                            {
                                orsD = true;
                            }
                            if (result && (statments[iff][0] == "or" && !ors))
                            {
                                ors = true;
                            }
                            if (!result && (statments[iff][0] == "nor" && !ors))
                            {
                                ors = true;
                            }
                            if (!result && (ands && statments[iff][0] == "and"))
                            {
                                ands = false;
                            }
                            if (result && (ands && statments[iff][0] == "not"))
                            {
                                ands = false;
                            }
                        }
                    }
                }
                if (!orsD)
                {
                    ors = true;
                }
                if (ors && ands)
                    endresult = true;

                if (!endresult)
                {
                    int w = 0;
                    int q = Base.position + 1;
                    while (true)
                    {


                        if (SimpleTokenizer.no_tab_spaces(Base.lines[q]) == "{")
                        {
                            w++;
                        }
                        if (SimpleTokenizer.no_tab_spaces(Base.lines[q]) == "}")
                        {
                            if (w == 1)
                            {
                                Base.changePosition(q);
                                break;
                            }
                            else if (w != 0)
                            {
                                w--;
                            }
                        }
                        q++;
                    }
                }
                else
                {

                }

                /*catch
                {
                    Console.WriteLine("Error: 10, logic statement error, Line "+ Base.position);
                }*/
            }
        }

        public class useC : command_centrall
        {
            private CommandRegistry commandRegistry;

            public useC(CommandRegistry a)
            {
                this.commandRegistry = a;
            }

            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (code.Count >= 3)
                {
                    string filePath = code[1];

                    // Read the contents of the .cs file
                    string fileContent = File.ReadAllText(filePath);

                    // Dynamic code generation with namespace
                    string generatedCode = $@"
using DATA_CONVERTER;
using System;
using Jace;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Collections.Generic;
namespace Imported_commands 
{{ 
            {fileContent}
}}";
                    //Debug.WriteLine(generatedCode);
                    try
                    {
                        // Compile the code using CodeDom
                        CompilerResults compilerResults = CompileCode(generatedCode);

                        if (compilerResults.Errors.HasErrors)
                        {

                            foreach (CompilerError error in compilerResults.Errors)
                            {
                                Debug.WriteLine(error.ErrorText);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to initialize imports: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number of parameters for the 'use' command.");
                }
            }

            private CompilerResults CompileCode(string code)
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false
                };
                string assemblyNameD = "DATA_CONVERTER";

                // Get the assembly by name
                Assembly assemblyD = Assembly.Load(assemblyNameD);

                // Get the location (path) of the assembly
                string assemblyPathD = assemblyD.Location;




                compilerParameters.ReferencedAssemblies.Add(assemblyPathD);
                //compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\PresentationFramework.dll");
                return codeProvider.CompileAssemblyFromSource(compilerParameters, code);

            }
        }


        public class use : command_centrall
        {
            private CommandRegistry commandRegistry;

            public use(CommandRegistry a)
            {
                this.commandRegistry = a;
            }

            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (code.Count >= 3)
                {
                    string filePath = code[1];
                    string className = code[2];

                    // Read the contents of the .cs file
                    string fileContent = File.ReadAllText(filePath);

                    // Dynamic code generation with namespace
                    string generatedCode = $@"
using DATA_CONVERTER;
using System;
using System.Linq;
using System.Text;
using System.IO;
using Jace;
using System.Windows;
using System.Collections.Generic;
namespace Imported_commands 
{{ 
    public class {className} : DATA_CONVERTER.outer_commands 
    {{ 
        public override void Execute(List<string> code, Data D)
        {{
            {fileContent}
        }}
    }} 
}}";
                    //Debug.WriteLine(generatedCode);
                    try
                    {
                        // Compile the code using CodeDom
                        CompilerResults compilerResults = CompileCode(generatedCode);

                        if (compilerResults.Errors.HasErrors)
                        {

                            foreach (CompilerError error in compilerResults.Errors)
                            {
                                Debug.WriteLine(error.ErrorText);
                            }
                        }
                        else
                        {
                            // Get the compiled type
                            Type compiledType = compilerResults.CompiledAssembly.GetType($"Imported_commands.{className}");

                            // Create an instance of the compiled class
                            object initializedObject = Activator.CreateInstance(compiledType);

                            // Check if the result is an instance of DATA_CONVERTER.outer_commands
                            if (initializedObject is DATA_CONVERTER.outer_commands outerCommandsObject)
                            {
                                // Add the command to the commandRegistry
                                this.commandRegistry.add_command(className, outerCommandsObject);
                                Debug.WriteLine(className);

                            }
                            else
                            {
                                Console.WriteLine($"Failed to create an instance of {className}.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to create an instance of {className}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number of parameters for the 'use' command.");
                }
            }

            private CompilerResults CompileCode(string code)
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                CompilerParameters compilerParameters = new CompilerParameters
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false
                };
                string assemblyNameD = "DATA_CONVERTER";

                // Get the assembly by name
                Assembly assemblyD = Assembly.Load(assemblyNameD);

                // Get the location (path) of the assembly
                string assemblyPathD = assemblyD.Location;




                compilerParameters.ReferencedAssemblies.Add(assemblyPathD);
                //compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\PresentationFramework.dll");
                return codeProvider.CompileAssemblyFromSource(compilerParameters, code);

            }
        }

        public class inputD : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (D.indouble(code[1]))
                {
                    bool inputSuccess = false;

                    do
                    {
                        try
                        {
                            //Console.Write("Enter a double value: ");
                            string rans = Console.ReadLine();

                            if (double.TryParse(rans, out double ran))
                            {
                                D.setD(code[1], ran);
                                inputSuccess = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid double.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " Line: " + Base.get_position());
                        }
                    } while (!inputSuccess);
                }
            }
        }

        public class inputI : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (D.inint(code[1]))
                {
                    bool inputSuccess = false;

                    do
                    {
                        try
                        {
                            //Console.Write("Enter an integer value: ");
                            string rans = Console.ReadLine();

                            if (int.TryParse(rans, out int ran))
                            {
                                D.setI(code[1], ran);
                                inputSuccess = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid integer.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " Line: " + Base.get_position());
                        }
                    } while (!inputSuccess);
                }
            }
        }

        public class inputS : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                if (D.instring(code[1]))
                {
                    bool inputSuccess = false;

                    do
                    {
                        try
                        {
                            //Console.Write("Enter a string: ");
                            string rans = Console.ReadLine();
                            D.setS(code[1], rans);
                            inputSuccess = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + " Line: " + Base.get_position());
                        }
                    } while (!inputSuccess);
                }
            }
        }

        public class setS : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    string Message = "";

                    for (int i = 1; i < code.Count; i++)
                    {
                        if (code[i] == "ReFs")
                        {
                            string Messagee = D.referenceS(code[i + 1]);
                            Message += Messagee + " ";
                            i += 1;
                        }
                        else if (code[i] == "ReFd")
                        {
                            double Messagee = D.referenceD(code[i + 1]);
                            Message += Messagee + " ";
                            i += 1;
                        }
                        else
                        {
                            Message += code[i] + " ";
                        }

                    }
                    D.setS(code[1], Message);
                }
                catch
                {
                    Console.WriteLine("Setting error: line " + Base.get_position());
                }

            }
        }
        public class int_func : command_centrall
        {
            //pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public int_func(pre_defined_variable j, CommandRegistry c)
            {
                //this.Math_equation = j;
                this.commands = c;

            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {

                try
                {
                    string equation = "";
                    if (code.Count() == 2)
                    {
                        D.setI(code[1], 0);
                        //this.commands.add_command(code[1], this.Math_equation);

                    }
                    else if (code[2] == "=")
                    {
                        for (int i = 3; i < code.Count(); i++)
                        {
                            double j;
                            if (Double.TryParse(code[i], out j))
                            {
                                equation += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                            code[i] == "csc" || code[i] == "sec" || code[i] == "%" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                            {
                                equation += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equation += D.referenceVar(code[i]) + " ";
                            }
                        }
                        CalculationEngine engine = new CalculationEngine();
                        D.setI(code[1], (int)(engine.Calculate(equation, drict)));
                        /*if (!(Base.commandRegistry.ContainsCommand(code[1])))
                        {
                            Base.commandRegistry.add_command(code[1], this.Math_equation);
                        }*/

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Initialization error " + e);
                }
            }
        }
        public class Line_func : command_centrall
        {
            //pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public Line_func(pre_defined_variable j, CommandRegistry c)
            {
                //this.Math_equation = j;
                this.commands = c;

            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {

                try
                {
                    string equation = "";
                    if (code.Count() == 2)
                    {
                        D.setD(code[1], 0);
                        //this.commands.add_command(code[1], this.Math_equation);

                    }
                    else if (code[2] == "=")
                    {
                        for (int i = 3; i < code.Count(); i++)
                        {
                            double j;
                            if (Double.TryParse(code[i], out j))
                            {
                                equation += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                                           code[i] == "csc" || code[i] == "sec" || code[i] == "%" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                            {
                                equation += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equation += D.referenceVar(code[i]) + " ";
                            }
                        }
                        CalculationEngine engine = new CalculationEngine();
                        DATA_CONVERTER.Line u = new Line((int)(engine.Calculate(equation, drict)), D);
                        D.setLine(code[1], u);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Initialization error " + e);
                }
            }
        }

        public class Function_func : command_centrall
        {
            //pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public Function_func(pre_defined_variable j, CommandRegistry c)
            {
                //this.Math_equation = j;
                this.commands = c;

            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {

                try
                {
                    string equationa = "";
                    string equationb = "";
                    int ended = 0;
                    if (code.Count() == 2)
                    {
                        D.setD(code[1], 0);
                        //this.commands.add_command(code[1], this.Math_equation);

                    }
                    else if (code[2] == "=")
                    {
                        for (int i = 3; i < code.Count(); i++)
                        {
                            if (code[i] == ",")
                            {
                                ended = i;
                                break;
                            }
                            double j;
                            if (Double.TryParse(code[i], out j))
                            {
                                equationa += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                                           code[i] == "csc" || code[i] == "sec" || code[i] == "%" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                            {
                                equationa += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equationa += D.referenceVar(code[i]) + " ";
                            }
                        }
                        for (int i = ended; i < code.Count(); i++)
                        {
                            double j;
                            if (Double.TryParse(code[i], out j))
                            {
                                equationb += j + " ";
                            }
                            else if (code[i] == "+" || code[i] == "-" || code[i] == "/" || code[i] == "*" || code[i] == "sin" || code[i] == "cos" || code[i] == "tan" ||
                                                           code[i] == "csc" || code[i] == "sec" || code[i] == "%" || code[i] == "cot" || code[i] == "root" || code[i] == ")" || code[i] == "(" || code[i] == " ")
                            {
                                equationb += code[i] + " ";
                            }
                            else if (D.isnumvar(code[i]))
                            {
                                equationb += D.referenceVar(code[i]) + " ";
                            }
                        }

                        CalculationEngine engine = new CalculationEngine();
                        DATA_CONVERTER.Function u = new Function((int)(engine.Calculate(equationa, drict)), (int)(engine.Calculate(equationb, drict)), D);
                        D.setFunction(code[1], u);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Initialization error " + e);
                }
            }
        }

        public class File_func : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                if (code.Count() == 3)
                {
                    D.setFile(code[1], new file(code[2], D));
                }
                else if (code.Count() == 4)
                {
                    if (D.issheet(code[3]))
                    {
                        D.setFile(code[1], new file(code[2], D.referenceSheet(code[3])));
                    }
                    else if (D.instring(code[3]))
                    {
                        if (D.issheet(D.referenceS(code[3] + "#")))
                        {
                            D.setFile(code[1], new file(code[2], D.referenceSheet(D.referenceS(code[3] + "#"))));
                        }
                    }
                }
            }
        }








        public class pre_defined_variable : command_centrall
        {
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public override void Execute(List<string> code, Data D, base_runner Base)
            {

                if (D.inint(code[0]))
                {
                    try
                    {
                        double j = doMath(code.Skip(2).ToArray(), D);
                        if (code[1] == "=")
                        {
                            
                            D.setI(code[0], (int)j);

                        }
                        else if (code[1] == "+=")
                        {
                           
                            D.setI(code[0], D.referenceI(code[0]) + (int)j);

                        }
                        else if (code[1] == "-=")
                        {
                            
                            D.setI(code[0], D.referenceI(code[0]) - (int)j);

                        }
                        else if (code[1] == "*=")
                        {
                          
                            D.setI(code[0], D.referenceI(code[0]) * (int)j);

                        }
                        else if (code[1] == "/=")
                        {
                         
                            D.setI(code[0], D.referenceI(code[0]) / (int)j);

                        }
                        else if (code[1] == "++")
                        {
                            D.setI(code[0], D.referenceI(code[0]) + 1);
                        }
                        else if (code[1] == "--")
                        {
                            D.setI(code[0], D.referenceI(code[0]) - 1);
                        }
                        else if (code[1] == "**")
                        {
                            D.setI(code[0], D.referenceI(code[0]) * D.referenceI(code[0]));
                        }
                    }
                    catch
                    {
                        throw new Exception("Initialization error " + Base.position);
                    }
                }
                if (D.indouble(code[0]))
                {

                    {
                        try
                        {
                            double j = doMath(code.Skip(2).ToArray(), D);
                            
                            if (code[1] == "=")
                            {

                                D.setD(code[0], j);

                            }
                            else if (code[1] == "+=")
                            {

                                D.setD(code[0], D.referenceD(code[0]) + j);

                            }
                            else if (code[1] == "-=")
                            {

                                D.setD(code[0], D.referenceD(code[0]) - j);

                            }
                            else if (code[1] == "*=")
                            {

                                D.setD(code[0], D.referenceD(code[0]) * j);

                            }
                            else if (code[1] == "/=")
                            {

                                D.setD(code[0], D.referenceD(code[0]) / j);

                            }
                            else if (code[1] == "++")
                            {
                                D.setD(code[0], D.referenceD(code[0]) + 1);
                            }
                            else if (code[1] == "--")
                            {
                                D.setD(code[0], D.referenceD(code[0]) - 1);
                            }
                            else if (code[1] == "**")
                            {
                                D.setD(code[0], D.referenceD(code[0]) * D.referenceD(code[0]));
                            }
                            else if (code[1] == "/^")
                            {
                                D.setD(code[0], Math.Sqrt(D.referenceD(code[0])));
                            }
                        }
                        catch
                        {
                            throw new Exception("Initialization error");
                        }
                    }
                }
                if (D.instring(code[0]))
                {
                    string mesage = D.referenceS(code[0]);
                    if (code[1] == "=")
                    {
                        mesage = "";
                        for (int i = 2; i < code.Count(); i++)
                        {
                            if (code[i] == "\"" && code[i + 2] == "\"")
                            {
                                mesage += D.referenceVar(code[i + 1]);
                                i += 2;
                            }
                            else if (code[i] == "!S!")
                            {
                                mesage += " ";
                            }
                            else if (code[i] == "M#" && code[i + 1] == "#")
                            {
                                string equation = "";
                                for (int ll = i; ll < code.Count; ll++)
                                {
                                    if (code[ll] == "#" && code[ll + 1] == "#M")
                                    {
                                        i = ll + 1;
                                        break;
                                    }
                                    double j;
                                    if (Double.TryParse(code[ll], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[ll] == "+" || code[ll] == "-" || code[ll] == "/" || code[ll] == "*" || code[ll] == "sin" || code[ll] == "cos" || code[ll] == "%" || code[ll] == "tan" ||
                                    code[ll] == "csc" || code[ll] == "sec" || code[ll] == "cot" || code[ll] == "root" || code[ll] == ")" || code[ll] == "(" || code[ll] == " ")
                                    {
                                        equation += code[ll] + " ";
                                    }
                                    else if (D.isnumvar(code[ll]))
                                    {
                                        equation += D.referenceVar(code[ll]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                mesage += engine.Calculate(equation);
                            }
                            else
                            {
                                mesage += code[i];
                                i++;
                            }
                        }
                    }
                    else if (code[1] == "+=")
                    {
                        for (int i = 2; i < code.Count(); i++)
                        {
                            if (code[i] == "\"" && code[i + 2] == "\"")
                            {
                                mesage += D.referenceVar(code[i + 1]);
                                i += 2;
                            }
                            else if (code[i] == "!S!")
                            {
                                mesage += " ";
                            }
                            else if (code[i] == "M#" && code[i + 1] == "#")
                            {
                                string equation = "";
                                for (int ll = i; ll < code.Count; ll++)
                                {
                                    if (code[ll] == "#" && code[ll + 1] == "#M")
                                    {
                                        i = ll + 1;
                                        break;
                                    }
                                    double j;
                                    if (Double.TryParse(code[ll], out j))
                                    {
                                        equation += j + " ";
                                    }
                                    else if (code[ll] == "+" || code[ll] == "-" || code[ll] == "/" || code[ll] == "*" || code[ll] == "sin" || code[ll] == "cos" || code[ll] == "%" || code[ll] == "tan" ||
                                    code[ll] == "csc" || code[ll] == "sec" || code[ll] == "cot" || code[ll] == "root" || code[ll] == ")" || code[ll] == "(" || code[ll] == " ")
                                    {
                                        equation += code[ll] + " ";
                                    }
                                    else if (D.isnumvar(code[ll]))
                                    {
                                        equation += D.referenceVar(code[ll]) + " ";
                                    }
                                }
                                CalculationEngine engine = new CalculationEngine();
                                mesage += engine.Calculate(equation);
                            }
                            else
                            {
                                mesage += code[i] + " ";
                                i++;
                            }
                        }
                    }
                    D.setS(code[0], mesage);
                }
                if (D.isLine(code[0]))
                {

                    if (code.Count == 1)
                    {
                        D.referenceLine(code[0]).uses();
                    }
                    else if (code[1] == "inst")
                    {

                        D.referenceLine(code[0]).set_line_string(Base.lines[D.referenceLine(code[0]).get_line_number()] + "\nend");
                    }
                    else if (code[1] == "=")
                    {
                        if (D.isLine(code[2]))
                        {
                            D.referenceLine(code[0]).set_line_number(D.referenceLine(code[2]).get_line_number());
                        }
                        else if (D.inint(code[2]))
                        {
                            D.referenceLine(code[0]).set_line_number(D.referenceI(code[2]));
                        }
                        else if (D.instring(code[2]))
                        {
                            if (D.isLine(D.referenceS(code[2])))
                            {
                                D.referenceLine(code[0]).set_line_number(D.referenceLine(D.referenceS(code[2])).get_line_number());
                            }
                            else if (D.inint(D.referenceS(code[2])))
                            {
                                D.referenceLine(code[0]).set_line_number(D.referenceI(D.referenceS(code[2])));
                            }
                        }
                    }
                }
                if (D.isFunction(code[0]))
                {
                    if (code.Count == 1)
                    {
                        D.referenceFunction(code[0]).uses();
                    }
                    else if (code.Count == 2)
                    {
                        if (code[1] == "inst")
                        {
                            string mesage = "";
                            for (int i = D.referenceFunction(code[0]).get_start_int(); i < D.referenceFunction(code[0]).get_end_int(); i++)
                            {
                                mesage += Base.lines[i] + "\n";
                            }
                            mesage += "end";
                            D.referenceFunction(code[0]).Setfunction_string(mesage);
                        }
                    }
                    else if (code[1] == "=")
                    {
                        if (D.issheet(code[2]))
                        {
                            D.referenceFunction(code[0]).change_acsesed_data(D.referenceSheet(code[2]));
                        }
                        else if (D.instring(code[2]))
                        {
                            if (D.issheet(D.referenceS(code[2]) + "#"))
                            {
                                D.referenceFunction(code[0]).change_acsesed_data(D.referenceSheet(D.referenceS(code[2]) + "#"));
                            }
                        }
                    }

                }
                if (D.isFile(code[0]))
                {
                    if (code.Count == 1)
                    {
                        D.referenceFile(code[0]).uses();
                    }
                    else if (code.Count == 2)
                    {
                        if (code[1] == "inst")

                            try
                            {
                                string fileName = @"" + D.referenceFile(code[0]).get_file_path();
                                using (StreamReader streamReader = File.OpenText(fileName))
                                {
                                    string text = streamReader.ReadToEnd();
                                    D.referenceFile(code[0]).set_context(text);
                                }

                            }
                            catch
                            {
                                Console.WriteLine("File not found");
                            }
                    }

                    else if (code[1] == "=")
                    {
                        if (D.issheet(code[2]))
                        {
                            D.referenceFile(code[0]).change_acsesed_data(D.referenceSheet(code[2]));
                        }
                        else if (D.instring(code[2]))
                        {
                            if (D.issheet(D.referenceS(code[2]) + "#"))
                            {
                                D.referenceFile(code[0]).change_acsesed_data(D.referenceSheet(D.referenceS(code[2]) + "#"));
                            }
                        }
                    }
                }
                if (D.isMethod(code[0]))
                {
                    object[] args = new object[D.referenceMethod(code[0]).get_args().Count()];
                    for (int j = 1; j < D.referenceMethod(code[0]).get_args().Count()+1; j++)
                    {
                        args[j] = D.referenceVar(code[j]);
                    }
                    doMethod(D.referenceMethod(code[0]), args, D);
                }

            }
        }
        public class string_func : command_centrall
        {
            //pre_defined_variable varlee;
            CommandRegistry commands;
            public string_func(pre_defined_variable j, CommandRegistry c)
            {
                //this.varlee = j;
                this.commands = c;
            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                try
                {
                    string mesage = "";
                    if (code.Count() == 2)
                    {
                        D.setS(code[1], mesage);
                        //this.commands.add_command(code[1], this.varlee);
                    }
                    else if (code[2] == "=")
                    {
                        mesage = "";
                        for (int i = 3; i < code.Count(); i++)
                        {
                            if (code[i] == "\"" && code[i + 2] == "\"")
                            {
                                mesage += D.referenceVar(code[i + 1]);
                                i += 2;

                            }
                            else if (code[i] == "!S!")
                            {
                                mesage += " ";
                            }
                            else if (code[i] == "M#" && code[i + 1] == "#")
                            {
                                List<string> codes = new List<string>();
                                for (int ll = i; ll < code.Count; ll++)
                                {
                                    if (code[ll] == "#" && code[ll + 1] == "#M")
                                    {
                                        i = ll + 1;
                                        break;
                                    }
                                    codes.Add(code[ll]);
                                }
                                mesage += doMath(codes.ToArray(),D);
                            }
                            else
                            {
                                mesage += code[i];
                                i++;
                            }
                        }
                        D.setS(code[1], mesage);
                        /*if (!(Base.commandRegistry.ContainsCommand(code[1])))
                        {
                            Base.commandRegistry.add_command(code[1], this.varlee);
                        }*/


                    }
                }
                catch
                {
                    Console.WriteLine("Initialization error");
                }
            }
        }
        public class double_func : command_centrall
        {
            //pre_defined_variable Math_equation;
            CommandRegistry commands;
            IDictionary<string, double> drict = new Dictionary<string, double>();
            public double_func(pre_defined_variable j, CommandRegistry c)
            {
                //this.Math_equation = j;
                this.commands = c;
            }
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                try
                {
                    string equation = "";
                    if (code.Count() == 2)
                    {
                        D.setD(code[1], 0);
                        //this.commands.add_command(code[1], this.Math_equation);
                    }
                    else if (code[2] == "=")
                    {
                        String[] c = code.Skip(3).ToArray();
                        
                        D.setD(code[1], doMath(c, D));
                        /*if (!(Base.commandRegistry.ContainsCommand(code[1])))
                        {
                            Base.commandRegistry.add_command(code[1], this.Math_equation);
                        }*/

                    }

                }
                catch
                {
                    Console.WriteLine("Initialization error");
                }
            }
        }
        public class end : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                DATA_CONVERTER.Data DD = Base.datas[0];
                Base.datas.Clear();
                Base.datas.Add(DD);
                Base.STOP();
            }
        }
        public class return_func : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                Base.changePosition(Base.positions[Base.positions.Count - 1]);
                Base.positions.RemoveAt(Base.positions.Count - 1);
            }
        }
        public class jump : command_centrall
        {
            public override void Execute(List<string> code, DATA_CONVERTER.Data D, base_runner Base)
            {
                try
                {
                    int a;
                    if (D.inint(code[1]))
                    {
                        a = (D.referenceI(code[1]));
                        Base.changePosition(a);
                    }
                    else if (int.TryParse(code[1], out a))
                    {
                        Base.changePosition(a);
                    }
                    else if (code[1] == ">>")
                    {

                        foreach (string i in Base.lines)
                        {
                            //Console.WriteLine(i);Console.WriteLine(">> " + code[2]);
                            if (SimpleTokenizer.no_tab_spaces(i) == ">>" + code[2])
                            {

                                //D.setI(code[2], Base.get_position());
                                Base.positions.Add(Base.get_position());
                                Base.changePosition(Base.lines.IndexOf(i));
                                break;

                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }

            }
        }
        public class line_number : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                try
                {
                    if (D.inint(code[1]))
                    {
                        int x = Base.get_position();
                        D.setI(code[1], (x));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e + " Line: " + Base.get_position());
                }
            }
        }
        /*public class Data
        {
            Dictionary<string, string> strings = new Dictionary<string, string>();
            Dictionary<string, double> doubles = new Dictionary<string, double>();
            public string referenceS(string key)
            {
                if (strings.ContainsKey(key))
                {
                    return strings[key];
                }
                else
                {
                    return null;
                }
            }

            public double referenceD(string key)
            {
                if (doubles.ContainsKey(key))
                {
                    return doubles[key];
                }
                else
                {
                    return 0;
                }
            }
            public void setS(string key, string data)
            {
                if (strings.ContainsKey(key))
                {
                    strings.Remove(key);
                }
                strings.Add(key, data);
            }
            public void setD(string key, double data)
            {
                if (doubles.ContainsKey(key))
                {
                    doubles.Remove(key);
                }
                doubles.Add(key, data);
            }

        }*/



        public class Method_instantiate : command_centrall
        {
            public override void Execute(List<string> code, Data D, base_runner Base)
            {
                int w = 0;
                int q = Base.position + 1;
                List<String> args = new List<String>();
                while (true)
                {
                    args.Add(Base.lines[q]);

                    if (SimpleTokenizer.no_tab_spaces(Base.lines[q]) == "{")
                    {
                        w++;
                    }
                    if (SimpleTokenizer.no_tab_spaces(Base.lines[q]) == "}")
                    {
                        if (w == 1)
                        {
                            Base.changePosition(q);
                            break;
                        }
                        else if (w != 0)
                        {
                            w--;
                        }
                    }
                    q++;
                }
                Type t = getType(code[1]);
                Dictionary<string,Object> list = new Dictionary<string,Object>();
                for(int i = 4; i< code.Count; i+=2)
                {
                    list.Add(code[i-1], getType(code[i]));
                }   

                D.setMethod(code[1], args.ToArray(), t, list);
                
            }
        }
        
        public static Type getType(string type)
        {
            switch (type)
            {
                case ("int"):
                    return typeof(int);
                case ("double"):
                    return typeof(double);
                case ("string"):
                    return typeof(string);
                case ("sheet"):
                    return typeof(Data);
                case ("void"):
                    return typeof(void);
                default:
                    return typeof(void);
            }
        }
        public static double doMath(string[] equation, Data D)
        {
            IDictionary<string, double> drict = new Dictionary<string, double>();
            string equationa = "";
            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == "+" || equation[i] == "-" || equation[i] == "/" || equation[i] == "*" || equation[i] == "sin" || equation[i] == "cos" || equation[i] == "tan" ||
                                       equation[i] == "csc" || equation[i] == "sec" || equation[i] == "%" || equation[i] == "cot" || equation[i] == "root" || equation[i] == ")" || equation[i] == "(" || equation[i] == " ")
                {
                    equationa += equation[i] + " ";
                }
                else if (D.isnumvar(equation[i].ToString()))
                {
                    equationa += D.referenceVar(equation[i].ToString()) + " ";
                }
                else if (equation[i] == "!F!")
                {
                    object[] args = new object[D.referenceMethod(equation[i + 1]).get_args().Count()];
                    for(int j = i+2; j < D.referenceMethod(equation[i+1]).get_args().Count() + i + 2; j++)
                    {
                        args[j - i - 2] = D.referenceVar(equation[j]);
                    }
                    i = i + D.referenceMethod(equation[i + 1]).get_args().Count() + 1;
                    if (doMethod(D.referenceMethod(equation[i + 1]), args, D) == null)
                    {
                        continue;
                    }
                    equationa += doMethod(D.referenceMethod(equation[i + 1]), args, D);
                }
                else if (double.TryParse(equation[i],out double k))
                {
                    equationa += k + " ";
                }
                
            }
            CalculationEngine engine = new CalculationEngine();
            return engine.Calculate(equationa, drict);
        }

        public static Object doMethod(Method m, object[] args, Data D)
        {
            string full = "";
            foreach(string it in m.get_code())
            {
                full += it + "\n";
            }
            full += "end";
            Data DD = D.Copy();
            int i = 0;
            foreach(string key in m.get_args().Keys)
            {
                DD.SuperSet(key, args[i]);
                i ++;
            }
            
            base_runner Base = new base_runner(full,DD);
            return Base.return_value;

        }
    }



}
namespace DATA_CONVERTER
{
    public partial class Line
    {
        partial void Execute()
        {
            base_runner bases = new base_runner(this.line_string,this.acsesed_data);
        }

    }
    public partial class Function
    {
        partial void Execute()
        {
            base_runner bases = new base_runner(this.function_string,this.acsesed_data);
        }
    }
    public partial class file
    {
        partial void Execute()
        {
            base_runner bases = new base_runner(this.file_context,this.acsesed_data);
        }
    }
}
