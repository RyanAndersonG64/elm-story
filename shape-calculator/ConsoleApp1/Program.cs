class Shape
{

    public double perimeter;
    public double area;

    public Shape(string type)
    {

        if (type == "circle")
        {
                double diameter = ReadPositiveDouble("Enter Diameter: ");

              perimeter = Math.PI * diameter;
              area = Math.PI * Math.Pow(diameter / 2, 2);
        }

        else if (type == "rectangle")
        {
              double SideOne = ReadPositiveDouble("Enter First Side Length: ");

              double SideTwo = ReadPositiveDouble("Enter Second Side Length: ");

              perimeter = 2 * SideOne + 2 * SideTwo;
              area = SideOne * SideTwo;
        }

        else if (type == "triangle")
        {
              double SideOne = ReadPositiveDouble("Enter First Side Length: ");

              double SideTwo = ReadPositiveDouble("Enter Second Side Length: ");

              double SideThree = ReadPositiveDouble("Enter Third Side Length: ");

              perimeter = SideOne + SideTwo + SideThree;

              double s = 0.5 * perimeter;
              area = Math.Pow(s * (s - SideOne) * (s - SideTwo) * (s - SideThree), 0.5);
        }

        else
        {
            Console.WriteLine ("Invalid Input");
        }
    }

    private static double ReadPositiveDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double value) && value > 0)
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a positive number.");
        }
    }

}

class Program {
    static void Main()
    {
        Console.Write("Enter circle, rectangle, or triangle: ");
        string type = Console.ReadLine();
        Shape YourShape = new Shape(type);
        Console.WriteLine($"Perimter: {YourShape.perimeter}");
        Console.WriteLine($"Area: {YourShape.area}");
    }
}
