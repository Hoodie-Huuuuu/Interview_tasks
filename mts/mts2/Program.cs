using System;
using System.Globalization;

class Program
{
	static readonly IFormatProvider _ifp = CultureInfo.InvariantCulture;

	class Number
	{
		readonly int _number;

		public Number(int number)
		{
			_number = number;
		}

		public static string operator+ (Number num1, string str_num2)
        {
			long result = num1._number + int.Parse(str_num2, _ifp);
			return result.ToString(_ifp);

		}

		public override string ToString()
		{
			return _number.ToString(_ifp);
		}
	}

	static void Main(string[] args)
	{
		int someValue1 = 100;
		int someValue2 = -3;

		string result = new Number(someValue1) + someValue2.ToString(_ifp);
		Console.WriteLine(result);
		Console.ReadKey();
	}
}

