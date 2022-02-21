/// <summary>
/// <para> Отсчитать несколько элементов с конца </para>
/// <example> new[] {1,2,3,4}.EnumerateFromTail(2) = (1, ), (2, ), (3, 1), (4, 0)</example>
/// </summary> 
/// <typeparam name="T"></typeparam>
/// <param name="enumerable"></param>
/// <param name="tailLength">Сколько элеметнов отсчитать с конца  (у последнего элемента tail = 0)</param>
/// <returns></returns>

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public static void Main()
    {
        //example
        var e = new[] { 1, 2, 3, 4 };

        Console.WriteLine("1st collection:");
        foreach (var item in e)
            Console.Write(item + " ");

        Console.WriteLine("\n\ntailLength = 10");
        foreach (var item in e.EnumerateFromTail(10))
            Console.Write(item + " ");

        Console.WriteLine("\n\ntailLength = 2");
        foreach (var item in e.EnumerateFromTail(2))
            Console.Write(item + " ");

        
        Console.WriteLine("\n\nEmpty collection");
        var e1 = new int[0];
        Console.WriteLine("\ntailLength = 10");
        foreach (var item in e1.EnumerateFromTail(10))
            Console.Write(item + " ");
        
    }

}

static class MyExtension
{
    public static IEnumerable<(T item, int? tail)> EnumerateFromTail<T>
                            (this IEnumerable<T> enumerable, int? tailLength)
    {
        //пустая коллекция
        if (enumerable == null)
            throw new ArgumentNullException("collection is null");
        

        if (tailLength == null)
            throw new ArgumentNullException("arg \"tailLength\" is null");
        
            
        //здесь либо 0 проходов по коллекции, либо 1
        int count = enumerable.Count();

        //нумеруем коллекцию
        int cur_num = count - 1;
        foreach (var item in enumerable)
        {
            if (cur_num > (int)tailLength - 1)
                yield return (item, null);
            
            else
                yield return (item, cur_num);
            
            cur_num--;
        }
    }
}


//Возможно ли реализовать такой метод выполняя перебор
//значений перечисления только 1 раз?

//Ответ
//если enumareble реализует ICollection или ICollection<T>,
//то Count() вернет количество элементов как свойство(то есть выполнится только один проход в методе),
//иначе, он пробежит итератором по всей коллекции(и метод пробежится по коллекции 2 раза)