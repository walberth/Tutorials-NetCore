namespace LiskovSubstitutionPrinciple
{
    using System;

    class Program {
        private static int Area(Rectangle rect) => rect.Width * rect.Height;
         
        static void Main(string[] args) {
            Rectangle rec = new Rectangle(2,3);

            Console.WriteLine($"Rectangle has Area {Area(rec)}");

            Rectangle square = new Square();
            square.Width = 4;

            Console.WriteLine($"Square has Area {Area(square)}");

            Console.ReadLine();
        }
    }

    public class Rectangle {
        public Rectangle() {
        }

        public Rectangle(int width, int height) {
            Width = width;
            Height = height;
        }

        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
    }

    public class Square : Rectangle {
        public override int Width {
            set { base.Width = base.Height = value; }
        }

        public override int Height {
            set { base.Width = base.Height = value; }
        }
    }
}
