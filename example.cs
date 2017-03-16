using System;

using Util.Exceptions;

public class A
{
	public int CallMe(string name)
	{
		Console.WriteLine("My name is totally: " + name);

        //throw new Exception("boom");
        //throw new NotSupportedException("boom");
        return 1337;
	}
}

class Program
{
	static void Main(string[] args)
    {
    	// Demo usage of ExceptionOr (version 1)
        Console.WriteLine("Example usage 1:");
        Console.WriteLine("====================================");
        ExceptionOrDemo(-10);
        ExceptionOrDemo(34);
        ExceptionOrDemo(70);

        // Demo usage of ExceptionOr (version 2)
        Console.WriteLine();
        Console.WriteLine("Example usage 2:");
        Console.WriteLine("====================================");
        ExceptionOrDemo2(-10);
        ExceptionOrDemo2(34);
        ExceptionOrDemo2(70);

        // Demo usage of ExceptionOr (version 3)
        Console.WriteLine();
        Console.WriteLine("Example usage 3:");
        Console.WriteLine("====================================");
        ExceptionOrDemo3(-10);
        ExceptionOrDemo3(34);
        ExceptionOrDemo3(70);

        // Demo usage of the "old try catch way":
        Console.WriteLine();
        Console.WriteLine("Example with try catch ('old way'):");
        Console.WriteLine("====================================");
        TryCatchDemo(-10);
        TryCatchDemo(34);
        TryCatchDemo(70);
    }

    static void ExceptionOrDemo(int realAge)
    {
        dynamic newAgeCalc = Safe.Wrapper(new Demo());

        ExceptionOr<int> age = newAgeCalc.Calc(realAge);
        if (age.HasFailed())
        {
            if (age.HasFailed<NotSupportedException, ArgumentException>())
            {
                Console.WriteLine("Please read the manual again, you simply use the class in a wrong way!");
                return;
            }

            Console.WriteLine("Please enter a correct age.");
            return;
        }

        Console.WriteLine("Congrats, your old age is {0}", age.GetValue());
        return;
    }

    static void ExceptionOrDemo2(int realAge)
    {
        var oldAgeCalculator = new Demo();

        ExceptionOr<int> age = Safe.Wrapper(oldAgeCalculator).Calc(realAge);
        if (age.HasFailed())
        {
            // Possibility to rethrow the Exception if it's one of the defined ones only:
            // age.Rethrow().If<OverflowException, NotSupportedException>();

            Console.WriteLine("An error has occured while trying to get the old age: " + age.GetException().Message);
            return;
        }

        Console.WriteLine("Congrats, your old age is {0}", age.GetValue());
        return;
    }

    static void ExceptionOrDemo3(int realAge)
    {
        var oldAgeCalculator = new Demo();

        ExceptionOr<bool> valid = Safe.Wrapper(oldAgeCalculator).Validate(realAge);
        if (valid.HasFailed())
        {
            Console.WriteLine("Sorry, your age is not valid: " + valid.GetException().Message);
            return;
        }

        Console.WriteLine("Congrats, your old age is valid");
        return;
    }

    static void TryCatchDemo(int realAge)
    {
        var oldAgeCalculator = new Demo();

        var age = 0;
        try
        {
            age = oldAgeCalculator.Calc(realAge);
        }
        catch (Exception e)
        {
            if (e is NotSupportedException || e is ArgumentException)
            {
                Console.WriteLine("Please read the manual again, you simply use the class in a wrong way!");
                return;
            }

            Console.WriteLine("Please enter a correct age.");
            return;
        }

        Console.WriteLine("Congrats, your old age is {0}", age);
        return;
    }

    public class Demo
    {
        public int Calc(int age)
        {
            if (age > 60)
            {
                throw new NotSupportedException("You are already old, we cannot make you older sorry!");
            }
            else if (age < 1)
            {
                throw new Exception("Invalid input, you are crazy!");
            }
            else if (age < 20)
            {
                throw new ArgumentException("You are too young, enjoy your life first.");
            }

            return age + 40;
        }

        public bool Validate(int age)
        {
            var e = new MultiException();

            if (age <= 10)
            {
                e.Add(new ArgumentException("Too young"));

                if (age <= 0)
                {
                    e.Add(new ArgumentException("0 or negative age not supported"));
                }
            }

            if (age < 30)
            {
                e.Add(new ArgumentException("You are still too young"));
            }

            if (age >= 40)
            {
                e.Add(new ArgumentException("Too old"));
            }

            e.ThrowIfFailed();

            return true;
        }
    }
}
