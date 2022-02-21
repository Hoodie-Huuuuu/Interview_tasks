/// <summary>
///     Возвращает отсортированный по возрастанию поток чисел
/// </summary>
/// 
/// <param name="inputStream">
///     Поток чисел от 0 до maxValue.
///     Длина потока не превышает миллиарда чисел.
/// </param>
/// 
/// <param name="sortFactor">
///     Фактор упорядоченности потока. Неотрицательное число.
///     Если в потоке встретилось число x, то в нём
///     больше не встретятся числа меньше, чем (x - sortFactor).
/// </param>
/// 
/// <param name="maxValue">
///     Максимально возможное значение чисел в потоке.
///     Неотрицательное число, не превышающее 2000.
/// </param>
/// 
/// <returns>
///     Отсортированный по возрастанию поток чисел.
/// </returns>



using System;
using System.Collections.Generic;


//вспомогательный класс - кольцевой буфер
public class RingArray
{
    private int[] mas;
    private int firstIdx = 0;

    public RingArray(int size)
    {
        mas = new int[size];
    }

    public void ShiftLeft(int shiftSize)
    {
        firstIdx = (shiftSize + firstIdx) % mas.Length;
    }
    public int Length => mas.Length;
    public int this [int idx]
    {
        get => mas[(firstIdx + idx) % mas.Length];
        set => mas[(firstIdx + idx) % mas.Length] = value;
    }
}


//=============================== Program ==========================
class Program
{

    public static void Main()
    {
        //Тесты
        var m = new[] { 4, 6, 6, 5, 3, 4, 3, 3, 9, 6, 6, 6, 10 };
        var res = Sort1(m, 3, 10);

        Console.WriteLine("========= 1 example\ninitial:");
        foreach (var item in m) Console.Write(item + " ");

        Console.WriteLine("\nSorted:");
        foreach (var item in res) Console.Write(item + " ");



        var m2 = new[] { 10, 5, 0, 1, 2, -1, -2, -3 };
        var res2 = Sort1(m2, 20, 10);

        Console.WriteLine("\n\n========= 2 example\ninitial:");
        foreach (var item in m2) Console.Write(item + " ");

        Console.WriteLine("\nSorted:");
        foreach (var item in res2) Console.Write(item + " ");



        var m3 = new int[0];
        var res3 = Sort1(m3, 5, 10);

        Console.WriteLine("\n\n========= 3 example\ninitial:");
        foreach (var item in m3) Console.Write(item + " ");

        Console.WriteLine("\nSorted:");
        foreach (var item in res3) Console.Write(item + " ");



        var m4 = new[] { 5, 2, 4, 1, 3, 9, 6, 10, 8, 7, 110, 106, 108, 120 };
        var res4 = Sort1(m4, 4, 10);
        Console.WriteLine("\n\n========= 4 example\ninitial:");
        foreach (var item in m4) Console.Write(item + " ");

        Console.WriteLine("\nSorted:");
        foreach (var item in res4) Console.Write(item + " ");

        Console.WriteLine("\nSorted2:");
        foreach (var item in Sort2(m4, 4, 120)) Console.Write(item + " ");

    }


    //метод sort1
    public static IEnumerable<int> Sort1(IEnumerable<int> inputStream,
                                                   int sortFactor, int maxValue)
    {

        if (inputStream == null)
            throw new ArgumentNullException("inputStream is null");

        if (maxValue > 2000 || maxValue < 0)
            throw new ArgumentOutOfRangeException("maxValue must be between" +
                                                                  "0 and 2000");

        if (sortFactor < 0) throw new ArgumentOutOfRangeException("sortFactor" +
                                                        "must be not negative");


        //sortFactor + 1 = это максимально возможное количество
        //неотсортированных подряд идущих чисел (без учета дубликатов)
        //<example>
        //  srotFactor = 1
        //  enumerable = 1 5 5 4 6 5
        //</example
        //
        //здесь не больше 2ух подярд идущих чисел, идущих не по возрастанию
        //(если схлопнуть подряд идущие дубликаты в одно число)


        //заведем массив счетчиков элементов размера sortFactor + 1
        var countArr = new RingArray(sortFactor + 1);

        //минимальный элемент, который можно встретить далее в потоке
        int min = Int32.MinValue;

        //заведем соответсвующий счетчикам массив самих элементов
        var numArr = new RingArray(sortFactor + 1);
        //for (int i = 0; i < numArr.Length; ++i) numArr[i] = min;


        //читаем числа из потока
        foreach (int item in inputStream)
        {
            //проверяем минимальный элемент
            if (item - sortFactor > min)
            {
                min = item - sortFactor;

                //выгружаем на выход все числа, меньшие чем min
                //так как дальше меньших чисел уже не будет
                int i = 0;
                while (i < numArr.Length && numArr[i] < min)
                {
                    //выгружаем число столько раз, сколько оно встречалось
                    for (int j = 0; j < countArr[i]; ++j)
                        yield return numArr[i];
                    ++i;
                }

                //i - сколько освободили мест в массиве numArr
                //двигаем массив влево и заполняем новыми числами 
                numArr.ShiftLeft(i);
                countArr.ShiftLeft(i);


                //для первого прохода, так как все элементы массива 0
                //и неизвестен min элемент потока
                if (i == 0) i = numArr.Length;


                //заполняем с конца
                int tmp = item;
                for (int j = numArr.Length - 1; j >= numArr.Length - i; --j)
                {
                    numArr[j] = tmp--;
                    countArr[j] = 0;
                }
                //так как надо учесть текущий элемент item
                countArr[countArr.Length - 1]++;
            }
            else
            {
                countArr[item - min]++;
            }
        }

        //выгружаем оставшиеся
        for (int i = 0; i < numArr.Length; ++i)
            for (int j = 0; j < countArr[i]; ++j)
                yield return numArr[i];
    }


    //2ая реализация. Не использует sortFactor и работает только для положительных чисел(как в условии)
    public static IEnumerable<int> Sort2(IEnumerable<int> inputStream,
                                                   int sortFactor, int maxValue)
    {
        if (inputStream == null)
            throw new ArgumentNullException("inputStream is null");

        if (maxValue > 2000 || maxValue < 0)
            throw new ArgumentOutOfRangeException("maxValue must be between" +
                                                                  "0 and 2000");

        if (sortFactor < 0) throw new ArgumentOutOfRangeException("sortFactor" +
                                                        "must be not negative");
        //массив счетчиков
        var countArr = new int[maxValue + 1];

        foreach (var item in inputStream)
        {
            countArr[item]++;
        }

        for (int i = 0; i < countArr.Length; ++i)
            for (int j = 0; j < countArr[i]; ++j)
                yield return i;

    }

}



//Реализуйте метод Sort. Известно, что потребители метода зачастую не будут
//вычитывать данные до конца. Оптимально ли Ваше решение с точки зрения
//скорости выполнения? С точки зрения потребляемой памяти?

//Ответ
//Sort1 оптимален по памяти. По скорости - можно было бы не создавать класс RingArray,
//так как тратиться время на вызовы методов. Но тогда, на мой взгляд, пострадала
//бы читаемость кода. Так же Sort1 не просматривает всю коллекцию сразу, что позволяет
//не рассходовать лишние ресурсы

//Sort2 - решение в лоб, оно не оптимально ни по памяти ни по времени. Но зато очень понятное