using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Prroject
{
    public class variable
    {
        public string name;
        public string type;
        public string value;
        public variable(string name, string type, string value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
    public class array
    {
        public string name;
        public List<string> value = new List<string>();
    }
    public partial class Form4 : Form
    {
        string[] ids = new string[]
        {
                "int", "float", "string", "double", "bool", "char"
        };
        string[] reserved_word = new string[]
        {
                "for", "while", "if", "do", "return", "break", "continue", "end", "else", "switch","case", "default",":"
        };
        List<variable> usedVars = new List<variable>();

        Dictionary<string, char> reserved_words = new Dictionary<string, char>();
        public Form4()
        {
            InitializeComponent();
            this.Load += Form4_Load;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            reserved_words.Add("for", '(');
            reserved_words.Add("if", '(');
            reserved_words.Add("else", '{');
            reserved_words.Add("do", '{');
            reserved_words.Add("while", '(');
            reserved_words.Add("return", ';');
            reserved_words.Add("break", ';');
            reserved_words.Add("continue", ';');
            reserved_words.Add("end", ';');
            reserved_words.Add("switch", '(');
        }

        private void textBox1_TextChanged(object sender, EventArgs e)

        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string input = textBox1.Text;
            string[] words = input.Split(new[] { " ", "\n", "\r", "\t" }, StringSplitOptions.None);
            string[] substr;
            List<variable> usedVars = new List<variable>();
            List<array> arraysList = new List<array>();
            bool isUsed;
            string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int openParenthesesCount = 0;
            int closeParenthesesCount = 0;
            int openCurlyCount = 0;
            int closeCurlyCount = 0;
            bool flag;
            int caseCt = 0;
            int breakCt = 0;
            for (int i = 0; i < lines.Length; i++)//makes sure that every case has a break statement
            {
                substr = lines[i].Split(' ', '+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ':');
                for (int k = 0; k < substr.Length; k++)
                {
                    if (substr[k] == "case")
                    {
                        caseCt++;
                    }
                    if (substr[k] == "break")
                    {
                        breakCt++;
                    }
                }
                bool flag2 = false;
                substr = lines[i].Split(' ', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ':');
                for (int k = 0; k < substr.Length; k++)
                {
                    for (int j = 0; j < substr[k].Length; j++)
                    {
                        if (substr[k][j] == '+')
                        {
                            flag2 = true;
                            if (j < substr[k].Length - 1 && (substr[k][j + 1] != '+' || substr[k][j + 1] != '='))
                            {
                                flag2 = false;
                                break;
                            }
                        }
                        if (substr[k][j] == '-')
                        {
                            flag2 = true;
                            if (j < substr[k].Length - 1 && (substr[k][j + 1] != '-' || substr[k][j + 1] != '='))
                            {
                                flag2 = false;
                                break;
                            }
                        }
                    }
                }
                if (flag2)
                {
                    listBox1.Items.Add("Missing sign");
                }
            }
            if (caseCt != breakCt)
            {
                listBox1.Items.Add("Missing a break statement.");
            }
            foreach (string line in lines)
            {
                flag = false;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    substr = line.Split(' ', '+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
                    if (line[line.Length - 1] == ';' || line[line.Length - 1] == ')' || line[line.Length - 1] == '}' || line[line.Length - 1] == '{' || line[line.Length - 1] == ':')
                    {//checks if the line ends with a valid character or if it ends with : and has case in it
                        flag = true;
                        if (line[line.Length - 1] == ':')
                        {
                            flag = false;
                            for (int k = 0; k < substr.Length; k++)
                            {
                                if (substr[k] == "case")
                                {
                                    flag = true;
                                }
                            }
                        }
                    }
                    if (flag == false)
                    {
                        listBox1.Items.Add("Incorrect ending for the line.");
                    }
                    for (int k = 0; k < substr.Length; k++)//makes sure that every case ends with :
                    {
                        if (substr[k] == "case" && line[line.Length - 1] != ':')
                        {
                            listBox1.Items.Add("case should end with \":\"");
                        }
                    }
                    for (int p = 0; p < line.Length; p++)//makes sure that there is an expression between the brackets
                    {
                        if (line.Length - 1 > p && line[p] == '(' && line[p + 1] == ')')
                        {
                            listBox1.Items.Add("Missing Expression between the brackets.");
                        }
                    }
                    foreach (char c in input)//checks for mismatched brackets
                    {
                        if (c == '(')
                        {
                            openParenthesesCount++;
                        }
                        else if (c == ')')
                        {
                            closeParenthesesCount++;
                        }
                        else if (c == '{')
                        {
                            openCurlyCount++;
                        }
                        else if (c == '}')
                        {
                            closeCurlyCount++;
                        }

                    }
                    if (openCurlyCount != closeCurlyCount)
                    {
                        listBox1.Items.Add("Mismatched curly brackets");
                        break;
                    }
                    if (openParenthesesCount != closeParenthesesCount)
                    {
                        listBox1.Items.Add("Mismatched parentheses");
                        break;
                    }
                }

            }

            for (int i = 0; i < words.Length; i++)
            {
                isUsed = false;
                substr = words[i].Split('+', '-', '/', '%', '*', '(', ')', '{', '}', ',', ';', '<', '>', '=', '!', '&', '|', '[', ']', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ':','[',']');
                for (int k = 0; k < substr.Length; k++)
                {
                    isUsed = false;
                    for (int j = 0; j < reserved_word.Length; j++)//checks if the word is a reserved word
                    {
                        if (substr[k] == reserved_word[j])
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    for (int j = 0; j < ids.Length; j++)
                    {
                        if (substr[k] == ids[j])
                        {
                            if (words.Length > i + 1 && !isUsed && !string.IsNullOrWhiteSpace(words[i + 1]))
                            {
                                if ((words.Length > i + 2 && !isUsed && !string.IsNullOrWhiteSpace(words[i + 2]) && (words[i + 2] == ";" || words[i + 2] == "=")) || (words[i + 1][words[i + 1].Length - 1] == ';' || words[i + 1][words[i + 1].Length - 1] == '='))
                                {
                                    bool used2 = false;
                                    bool used3 = false;
                                    string[] juniorStr = words[i + 1].Split(';', '=');
                                    for (int l = 0; l < reserved_word.Length; l++)
                                    {
                                        if (juniorStr[0] == reserved_word[l])
                                        {
                                            used2 = true;
                                            break;
                                        }
                                    }
                                    for (int l = 0; l < ids.Length; l++)
                                    {
                                        if (juniorStr[0] == ids[l])
                                        {
                                            used2 = true;
                                            break;
                                        }
                                    }
                                    for (int l = 0; l < usedVars.Count; l++)
                                    {
                                        if (juniorStr[0] == usedVars[l].name)
                                        {
                                            listBox1.Items.Add(juniorStr[0] + " is already defined.");
                                            used3 = true;
                                            break;
                                        }
                                    }
                                    if (!used2 && !used3)
                                    {
                                        variable obj = new variable(juniorStr[0], substr[k], "null");
                                        usedVars.Add(obj);
                                    }
                                    else if (!used3)
                                    {
                                        listBox1.Items.Add(juniorStr[0] + " cannot be a variable.");
                                    }
                                    if (words[i + 1][words[i + 1].Length - 1] == '=')
                                    {
                                        listBox1.Items.Add("Line cannot end with '='.");
                                        isUsed = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    listBox1.Items.Add("Missing ; or invalid syntax");
                                }
                            }
                            isUsed = true;

                        }
                    }
                    for (int j = 0; j < usedVars.Count; j++)
                    {
                        if (usedVars[j].name == substr[k])
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    for (int j = 0; j < arraysList.Count; j++)
                    {
                        if (arraysList[j].name == substr[k])
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    if (!isUsed && !string.IsNullOrWhiteSpace(substr[k]))
                    {
                        listBox1.Items.Add("Undeclared Variable \"" + substr[k] + "\"");
                    }
                }


                for (int j = 0; j < substr.Length; j++)
                {
                    if (reserved_words.ContainsKey(substr[j]) == true)
                    {
                        if ((words.Length > i + 1 && !string.IsNullOrWhiteSpace(words[i + 1]) && words[i + 1][0] == reserved_words[substr[j]]) || (words[i].Length > substr[j].Length && words[i][substr[j].Length] == reserved_words[substr[j]]))
                        {
                            //No Errors
                        }
                        else
                        {
                            listBox1.Items.Add("Missing " + reserved_words[substr[j]]);
                        }
                    }

                }
            }

            words = input.Split(new[] { " ","\n", "\r", "\t", ";" }, StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < usedVars.Count; j++)
                {
                    string[] substr2 = words[i].Split(new[] { "=", ";", "+", "-", "*", "/", "++" }, StringSplitOptions.None);
                    if (words[i] == usedVars[j].name || substr2[0] == usedVars[j].name) // checks if changing variable with = or += or -=...etc
                    {
                        for (int k = 0; k < substr2.Length; k++)
                        {
                            if (substr2.Length > 1 && !string.IsNullOrWhiteSpace(substr2[1]))
                            {
                                usedVars[j].value = substr2[1];
                            }
                        }
                        float num;
                        float num2;
                        if (words.Length > i + 5 && words[i + 1] == "=")
                        {
                            if (words[i + 3] == "+")
                            {
                                if (float.TryParse(words[i + 2], out num))// int i = 2 + j;
                                {
                                    if (float.TryParse(words[i + 4], out num2))
                                    {
                                        float cal2 = num + num2;
                                        usedVars[j].value = cal2.ToString();
                                    }
                                    else
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                float cal = num + float.Parse(usedVars[va].value);
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (float.TryParse(words[i + 4], out num))
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal = float.Parse(usedVars[va].value) + num;
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        float cal = 0;
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                cal = float.Parse(usedVars[va].value);
                                            }
                                        }
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal2 = float.Parse(usedVars[va].value) + cal;
                                                usedVars[j].value = cal2.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            if (words[i + 3] == "-")
                            {
                                if (float.TryParse(words[i + 2], out num))// int i = 2 + j;
                                {
                                    if (float.TryParse(words[i + 4], out num2))
                                    {
                                        float cal2 = num - num2;
                                        usedVars[j].value = cal2.ToString();
                                    }
                                    else
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                float cal = num - float.Parse(usedVars[va].value);
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (float.TryParse(words[i + 4], out num))
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal = float.Parse(usedVars[va].value) - num;
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        float cal = 0;
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                cal = float.Parse(usedVars[va].value);
                                            }
                                        }
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal2 = float.Parse(usedVars[va].value) - cal;
                                                usedVars[j].value = cal2.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            if (words[i + 3] == "*")
                            {
                                if (float.TryParse(words[i + 2], out num))// int i = 2 + j;
                                {
                                    if (float.TryParse(words[i + 4], out num2))
                                    {
                                        float cal2 = num * num2;
                                        usedVars[j].value = cal2.ToString();
                                    }
                                    else
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                float cal = num * float.Parse(usedVars[va].value);
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (float.TryParse(words[i + 4], out num))
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal = float.Parse(usedVars[va].value) * num;
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        float cal = 0;
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                cal = float.Parse(usedVars[va].value);
                                            }
                                        }
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal2 = float.Parse(usedVars[va].value) * cal;
                                                usedVars[j].value = cal2.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            if (words[i + 3] == "/")
                            {
                                if (float.TryParse(words[i + 2], out num))// int i = 2 + j;
                                {
                                    if (float.TryParse(words[i + 4], out num2))
                                    {
                                        float cal2 = num / num2;
                                        usedVars[j].value = cal2.ToString();
                                    }
                                    else
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                float cal = num / float.Parse(usedVars[va].value);
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (float.TryParse(words[i + 4], out num))
                                    {
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal = float.Parse(usedVars[va].value) / num;
                                                usedVars[j].value = cal.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        float cal = 0;
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 4] == usedVars[va].name)
                                            {
                                                cal = float.Parse(usedVars[va].value);
                                            }
                                        }
                                        for (int va = 0; va < usedVars.Count; va++)
                                        {
                                            if (words[i + 2] == usedVars[va].name)
                                            {
                                                float cal2 = float.Parse(usedVars[va].value) / cal;
                                                usedVars[j].value = cal2.ToString();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else if (words.Length > i + 2 && words[i + 1] == "=")
                        {
                            usedVars[j].value = words[i + 2];
                        }
                        if (words.Length > i + 2 && words[i + 1] == "+=" && !string.IsNullOrWhiteSpace(words[i + 2]))
                        {
                            if (float.TryParse(words[i + 2], out num))
                            {
                                float cal = num + float.Parse(usedVars[j].value);
                                usedVars[j].value = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[i + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(usedVars[va].value) + float.Parse(usedVars[j].value);
                                        usedVars[j].value = cal.ToString();
                                    }
                                }
                            }
                        }
                        if (words.Length > i + 2 && words[i + 1] == "-=" && !string.IsNullOrWhiteSpace(words[i + 2]))
                        {
                            if (float.TryParse(words[i + 2], out num))
                            {
                                float cal = float.Parse(usedVars[j].value) - num;
                                usedVars[j].value = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[i + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(usedVars[j].value) - float.Parse(usedVars[va].value);
                                        usedVars[j].value = cal.ToString();
                                    }
                                }
                            }
                        }
                        if (words.Length > i + 2 && words[i + 1] == "*=" && !string.IsNullOrWhiteSpace(words[i + 2]))
                        {
                            if (float.TryParse(words[i + 2], out num))
                            {
                                float cal = num * float.Parse(usedVars[j].value);
                                usedVars[j].value = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[i + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(usedVars[j].value) * float.Parse(usedVars[va].value);
                                        usedVars[j].value = cal.ToString();
                                    }
                                }
                            }
                        }
                        if (words.Length > i + 2 && words[i + 1] == "/=" && !string.IsNullOrWhiteSpace(words[i + 2]))
                        {
                            if (float.TryParse(words[i + 2], out num))
                            {
                                float cal = float.Parse(usedVars[j].value) / num;//i += l;
                                usedVars[j].value = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[i + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(usedVars[j].value) / float.Parse(usedVars[va].value);
                                        usedVars[j].value = cal.ToString();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            for (int i = 0; i < usedVars.Count; i++)
            {
                for (int j = 0; j < usedVars[i].name.Length; j++)
                {
                    if (usedVars[i].name[j] == '[')
                    {
                        string[] tempStr = usedVars[i].name.Split('[', ']', ';', ',');
                        if (tempStr.Length > 3)
                        {
                            array tempArr = new array();
                            tempArr.name = tempStr[0];
                            for (int k = 1; k < tempStr.Length; k++)
                            {
                                if (!string.IsNullOrWhiteSpace(tempStr[k]))
                                {
                                    tempArr.value.Add(tempStr[k]);
                                }
                            }
                            arraysList.Add(tempArr);
                            usedVars.RemoveAt(i);
                            break;
                        }
                        else
                        {
                            usedVars.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            words = input.Split(new[] { " ", "\n", "\r", "\t" }, StringSplitOptions.None);
            for (int j = 0; j < words.Length; j++)
            {
                string[] subst = words[j].Split('[');
                for (int i = 0; i < arraysList.Count; i++)
                {

                    if (subst[0] == arraysList[i].name && words.Length > j + 2 && words[j + 1] == "=")
                    {
                        float num;
                        string[] tempStr = words[j].Split('[', ']', ',', ';');
                        if (tempStr.Length <= 3)
                        {
                            string[] subst2 = words[j + 2].Split(';');
                            if (float.TryParse(subst2[0], out num))
                            {
                                arraysList[i].value[int.Parse(tempStr[1])] = num.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (subst2[0] == usedVars[va].name)
                                    {
                                        arraysList[i].value[int.Parse(tempStr[1])] = usedVars[va].value;
                                    }
                                }
                            }
                        }
                        if (words.Length > j + 2 && words[j + 1] == "+=" && !string.IsNullOrWhiteSpace(words[j + 2]))
                        {
                            if (float.TryParse(words[j + 2], out num))
                            {
                                float cal = num + float.Parse(arraysList[i].value[int.Parse(tempStr[1])]);
                                arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[j + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(usedVars[va].value) + float.Parse(arraysList[i].value[int.Parse(tempStr[1])]);
                                        arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                                    }
                                }
                            }
                        }
                        if (words.Length > j + 2 && words[j + 1] == "-=" && !string.IsNullOrWhiteSpace(words[j + 2]))
                        {
                            if (float.TryParse(words[j + 2], out num))
                            {
                                float cal = float.Parse(arraysList[i].value[int.Parse(tempStr[1])]) - num;
                                arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[j + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(arraysList[i].value[int.Parse(tempStr[1])]) - float.Parse(usedVars[va].value);
                                        arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                                    }
                                }
                            }
                        }
                        if (words.Length > j + 2 && words[j + 1] == "*=" && !string.IsNullOrWhiteSpace(words[j + 2]))
                        {
                            if (float.TryParse(words[j + 2], out num))
                            {
                                float cal = num * float.Parse(arraysList[i].value[int.Parse(tempStr[1])]);
                                arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[j + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(arraysList[i].value[int.Parse(tempStr[1])]) * float.Parse(usedVars[va].value);
                                        arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                                    }
                                }
                            }
                        }
                        if (words.Length > j + 2 && words[j + 1] == "/=" && !string.IsNullOrWhiteSpace(words[j + 2]))
                        {
                            if (float.TryParse(words[j + 2], out num))
                            {
                                float cal = float.Parse(arraysList[i].value[int.Parse(tempStr[1])]) / num;//i += l;
                                arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                            }
                            else
                            {
                                for (int va = 0; va < usedVars.Count; va++)
                                {
                                    if (words[j + 2] == usedVars[va].name)
                                    {
                                        float cal = float.Parse(arraysList[i].value[int.Parse(tempStr[1])]) / float.Parse(usedVars[va].value);
                                        arraysList[i].value[int.Parse(tempStr[1])] = cal.ToString();
                                    }
                                }
                            }
                        }

                    }
                }
            }
            for (int i = 0; i < arraysList.Count; i++)
            {
                listBox2.Items.Add(arraysList[i].name);
                listBox2.Items.Add('[');
                for (int j = 0; j < arraysList[i].value.Count; j++)
                {
                    listBox2.Items.Add(arraysList[i].value[j]);
                }
                listBox2.Items.Add(']');
            }
            for (int i = 0; i < lines.Length; i++)
            {
                string[] ll = lines[i].Split('(');
                if (lines.Length > i + 1 && ll[0] == "for")
                {

                    int forstp = 1;
                    for (int l = 0; l < lines[i].Length; l++)//checks if there is a usedvar addition in a for loop
                    {
                        if (lines[i][l] == '<' && lines[i].Length > l + 1)
                        {
                            forstp = int.Parse(lines[i][l + 1].ToString());
                        }
                        if(forstp != 1)
                        {
                            for (int j = 0; j < usedVars.Count; j++)
                            {
                                if (lines[i].Length > l + 1)
                                {
                                    ll = lines[i + 1].Split(' ', '\n', '\r',';');
                                    if (ll[0] == (usedVars[0].name + "++"))
                                    {
                                        int cal = int.Parse(usedVars[j].value) + forstp;
                                        usedVars[j].value = cal.ToString();
                                        break;
                                    }
                                }
                            }
                            forstp = 1;
                        }
                    }
                }
            }
            for (int i = 0; i < usedVars.Count; i++)
            {
                listBox2.Items.Add(usedVars[i].name + " | " + usedVars[i].type + " | " + usedVars[i].value);
            }
        }
    }
}
