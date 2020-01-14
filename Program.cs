using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bandysoft
{

    

    class Program
    {
        public const int w_oper = 100;
        public const int w_parentheses = 1000;
        public const bool debugtrace = false;
        //  private string inputStr="";
        static Collection<operand> operandlist;

      
        static void Main(string[] args)
        {
            operandlist = new Collection<operand>();



            /*public operand*/
            operandlist.Add(new operand("+", 1, operand.fplus));
            operandlist.Add(new operand("-", 1, operand.fminus));
            operandlist.Add(new operand("*", 2, operand.milty));
            operandlist.Add(new operand("/", 1, operand.dev));

           



            Console.WriteLine("Добро пожаловать в калькулятор. Тестовое задание для BandySoft. Введите входную строку содержащую математическое выражение (целые и десятично-дробные числа, знаки +, -, *, / и скобки)");
            string inputStr = Console.ReadLine();
           // Console.WriteLine(inputStr);

            //разбиваем на простые радикалы.

            string[] separators = new string[] { "+", "-","(",")" };//
            char[] separators2 = new char[] { '+', '-', '(', ')' };//


           // Console.WriteLine(inputStr.LastIndexOfAny(separators2));

            Console.WriteLine(doaction(inputStr));
            
           

        }

        static string doaction(string _input)
        {
            double dres;
            if (Double.TryParse(_input, out dres))//выход из рекурсии если парсер может разобрать число 
                return _input;

            char[] separators2 = new char[] { '+', '-' };//
            string separatorstr = "";
            foreach (operand item in operandlist)
            {
                separatorstr += item.symbol;

            }
            separators2 = separatorstr.ToCharArray();

            Collection<deystviya> listdeystviy;
            listdeystviy = findorder(_input);

            if (listdeystviy.Count == 0) return _input; //выход из рекурсии если абракадабра без символов операций

            //поиск самого приорететного операнда
            int maxw = 0;
            int max_position = 0;
            operand maxoper = listdeystviy[0].oper;
            foreach (deystviya item in listdeystviy)
            {
                if (item.weigth > maxw)
                {
                    maxw = item.weigth;
                    max_position = item.position;
                    maxoper = item.oper;
                }

            }




            string leftstr = _input.Substring(0, max_position);
            string rigthstr = _input.Substring(max_position + 1, _input.Length - max_position - 1);


            int indexoflastleft = leftstr.LastIndexOfAny(separators2);
            int indexofirstRigth = rigthstr.IndexOfAny(separators2);


            string value1 = "", value2 = "";
            // if (indexoflastleft <= -1) indexoflastleft = 0;
            if (indexofirstRigth <= -1) indexofirstRigth = rigthstr.Length;

            value1 = leftstr.Substring(indexoflastleft + 1, leftstr.Length - indexoflastleft - 1);
            if (indexofirstRigth > -1) value2 = rigthstr.Substring(0, indexofirstRigth);

            leftstr = leftstr.Substring(0, indexoflastleft + 1);
            rigthstr = rigthstr.Substring(indexofirstRigth, rigthstr.Length - indexofirstRigth);

           
            value1 = value1.Replace("(", "");
            value1 = value1.Replace(")", "");
            value2 = value2.Replace("(", "");
            value2 = value2.Replace(")", "");



            double val1, val2;
            if (!Double.TryParse(value1, out val1))
                val1 = 0;
            if (!Double.TryParse(value2, out val2))
                val2 = 0;


            double res = maxoper.Funct(val1, val2);

            string resstring = leftstr + res.ToString() + rigthstr;

            if (debugtrace)
            {

                Console.WriteLine(max_position);
                Console.WriteLine(leftstr);
                Console.WriteLine(value1);
                Console.WriteLine(value2);
                Console.WriteLine(rigthstr);
                Console.WriteLine(resstring);
            }

            resstring=doaction(resstring);
            return resstring;
        }

        static  Collection<deystviya> findorder(string toorderstr)
        {
             Collection<deystviya> pordeystv = new Collection<deystviya>();

            int curweigth = 1000;
            for (int fori=0; fori<toorderstr.Length;fori++)
            {
                curweigth--;//каждую иттерацию мы уменшаем вес что бы приоритет был по порядку опреандов.

                if (toorderstr[fori] == '(')
                {
                    curweigth += w_parentheses;

                }
                if (toorderstr[fori] == ')')
                {
                    curweigth -= w_parentheses;

                }

                foreach (operand item in operandlist)
                {
                    if (toorderstr[fori] == item.symbol[0])
                    {
                        deystviya curdey = new deystviya();
                        curdey.position = fori;
                        curdey.oper = item;
                        curdey.weigth = curweigth + item.priority * w_oper;
                        pordeystv.Add(curdey);
                    }

                }

               

            }

            


            return pordeystv;
        }

       
    }
    class operand
    {
        public operand(string _symbol,int _priority,functdel _funct)
        {
            symbol = _symbol;
            priority = _priority;
            Funct = _funct;

        }

        public string symbol;
        public int priority; //не приоритет а вес операции чем больше - тем приоритетней
        public delegate double functdel(double val1, double val2);                     //public * funct;
        public functdel Funct;
        
        public static double fplus(double val1, double val2)
        {

            return val1 + val2;
        }

        public static double fminus(double val1, double val2)
        {

            return val1 - val2;
        }
        public static double milty(double val1, double val2)
        {

            return val1 * val2;
        }
        public static double dev(double val1, double val2)
        {

            return val1 / val2;
        }
    }

    class deystviya
    {
        public operand oper;
        public int weigth; //вес действия для определения порядка
        public int position;


    }

   

}
