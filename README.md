# ExceptionOr\<T\> - Another approach of error / exception handling 

## Intro
It's important to handle all the errors and occurance of exceptions properly, in a systematic way.

This small library approaches error handling in a different way. Some of us like Exceptions and others don't so much.

By no means I want to say that this way of handling errors & exceptions is the holy grail. But if you like ExceptionOr\<T\> then you are free to use it and if not then continue to use the traditional Exception-Handling way.

Check out the demo usages below and decide for yourself:

## Usages & examples

**1) Handling exceptions:**

```csharp
class Application
{
    public static void Main(string[] args)
    {
        ExceptionOrDemo(-10);
        ExceptionOrDemo(34);
        ExceptionOrDemo(70);
    }

    static void ExceptionOrDemo(int realAge)
    {
        dynamic newAgeCalc = Safe.Wrapper(new Demo());

        ExceptionOr<int> age = newAgeCalc.Calc(realAge);
        if (age.HasFailed())
        {
            age.Rethrow().If<OverflowException>();
            // or: age.Rethrow().If<OverflowException, NotSupportedException>();

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
    }
}
```

**2) Multiple exceptions:**

```csharp
class Application
{
    public static void Main(string[] args)
    {
        ExceptionOrDemo(-10);
        ExceptionOrDemo(34);
        ExceptionOrDemo(70);
    }

    static void ExceptionOrDemo(int realAge)
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

    public class Demo
    {
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
```

**Output:**

On the first line, you can see how the exception messages got concatenated by ", " when accessing (Get)Exception.Message:

```
Sorry, your age is not valid: Too young, 0 or negative age not supported, You are still too young
Congrats, your old age is valid
Sorry, your age is not valid: Too old
```
even if this gives you a nice summary you still can access each Exception individually by iterating over the List<Exception> returned by GetException().


## Important details
#### Why ```dynamic``` was used:

The ExceptionOr\<T\> is fully type safe but the ```dynamic``` was needed for the Safe.Wrapper(...) only in order to proxy the methods and intercept the Exceptions (if any occur). 

The other options would have been to declare all methods ```virtual``` or make all class inherit from ```MarshalByRefObject```. I personally found this rather ugly.

Since I wanted to make the library easy to use I decided to implement it the ```dynamic```-way.

Should I find a better solution then I will update the code. If you have a better way then please contact me.

## Motivation

I was mainly coding this library in order to learn more about the C# programming language and also because I personally (slightly) prefer to handle exceptions & errors this way. :-)

## Development status

This library is still in early alpha and should not be used in production. Also it is NOT backward compatible and might change with every new commit. If you would like to use it then use it as it is.


## License

The entire library is licensed under MIT License:

Copyright (c) 2017 Andreas Näpflin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
