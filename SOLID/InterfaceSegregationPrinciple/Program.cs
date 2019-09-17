using System;
using InterfaceSegregationPrinciple . Interface;

namespace InterfaceSegregationPrinciple
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Printer : IPrint
    {
        public void Print(Document document) { throw new NotImplementedException(); }
    }

    public class OnlyPrinter : IMachine
    {
        public void Print(Document document) { throw new NotImplementedException(); }
        public void Scan(Document document) { throw new NotImplementedException(); }
        public void Fax(Document document) { throw new NotImplementedException(); }
    }

    public class MultiFunction : IMultiFunction {
        private IScan scan;
        private IPrint print;

        public MultiFunction(IScan scan, IPrint print) {
            this.scan = scan;
            this.print = print;
        }

        public void Print(Document document) => this.print.Print(document);
        public void Scan(Document document) => this.scan.Scan(document);
    }
}
