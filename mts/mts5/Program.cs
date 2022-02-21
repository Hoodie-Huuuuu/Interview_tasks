using System;

//Программа выводит на экран строку «Муха»,
//а затем продолжает выполнять остальной код.
//Реализуйте метод TransformToElephant так,
//чтобы программа выводила на экран строку «Слон»,
//а затем продолжала выполнять остальной код,
//не выводя перед этим на экран строку «Муха».

using System.IO;

class Program
{
	static void Main(string[] args)
	{
		TransformToElephant();
		Console.WriteLine("Муха");
		//... custom application code
	}

	static void TransformToElephant()
	{
		if (Console.Out.NewLine != "Слон")
		{
			Console.WriteLine("Слон");
			Console.SetOut(new StringWriter() { NewLine = "Слон" });

			Main(null);
		}
	}

}	
