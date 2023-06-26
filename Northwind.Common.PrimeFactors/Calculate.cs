namespace Northwind.Web.Pages;

public static class Calculate
{
    public static string PrintPrimeFactors(int? number)//number <=1000
    {
        string result = $"Prime factors of {number} are: ";
        int SqrLimit,x2, y2, i, j, n, limit;
        if (number < 0)
        {
            result = "Negative numbers not calculate!";
            return result;
        }
        if (number>1000)
        {
            result="Numbers must be not greater 1000!";
            return result ;
        }
        if (!number.HasValue) 
        {
            result=$"Value of parameter {nameof(number)} can not be null!";
            return result ;
        }
        if(number<=5)
        {
            result = number switch
            {
                0 => "0 don`t have prime factors!",
                1 => "1 don`t have prime factors!",
                2 => "Prime factors of 2 are: 2",
                3 => "Prime factors of 3 are: 3",
                4 => "Prime factors of 4 are: 2 x 2",
                5 => "Prime factors of 5 are: 5",
                _=>"Some error!"
            };
            return result;
        }
        else
        {
            limit = (int)number / 2!;
            //Решето Аткина для поиска простых чисел(https://ru.wikipedia.org/wiki/%D0%A0%D0%B5%D1%88%D0%B5%D1%82%D0%BE_%D0%90%D1%82%D0%BA%D0%B8%D0%BD%D0%B0)//по идее надо бы в отдельный метод, но я пока хз как массив возращать
            bool[]? IsPrime = Array.Empty<bool>()!;
            bool numberIsPrime = true;
            Array.Resize<bool>(ref IsPrime, limit + 1);
            // Инициализация решета
            SqrLimit = (int)Math.Sqrt((long)limit);//(int)sqrt((long double)limit);
            for (i = 0; i <= limit; ++i)
            {
                IsPrime[i] = false;
            }
            IsPrime[2] = true;
            IsPrime[3] = true;
            // Предположительно простые — это целые с нечётным числом
            // представлений в данных квадратных формах.
            // x2 и y2 — это квадраты i и j (оптимизация).
            x2 = 0;
            for (i = 1; i <= SqrLimit; ++i)
            {
                x2 += 2 * i - 1;
                y2 = 0;
                for (j = 1; j <= SqrLimit; ++j)
                {
                    y2 += 2 * j - 1;
                    n = 4 * x2 + y2;
                    if ((n <= limit) && (n % 12 == 1 || n % 12 == 5))
                    {
                        IsPrime[n] = !IsPrime[n];
                    }
                    // n = 3 * x2 + y2; 
                    n -= x2; // Оптимизация
                    if ((n <= limit) && (n % 12 == 7))
                    {
                        IsPrime[n] = !IsPrime[n];
                    }
                    // n = 3 * x2 - y2;
                    n -= 2 * y2; // Оптимизация
                    if ((i > j) && (n <= limit) && (n % 12 == 11))
                    {
                        IsPrime[n] = !IsPrime[n];
                    }
                }
            }
            // Отсеиваем кратные квадратам простых чисел в интервале [5, sqrt(limit)].
            // (основной этап не может их отсеять)
            for (i = 5; i <= SqrLimit; ++i)
            {
                if (IsPrime[i])
                {
                    n = i * i;
                    for (j = n; j <= limit; j += n)
                    {
                        IsPrime[j] = false;
                    }
                }
            }
            // Вывод списка простых чисел в консоль. --storing in array
            //printf("2, 3, 5");
            int[] primeNumbersArray = { 2, 3, 5 };//store prime numbers < limit
            int primeNumbersArrayLength = 3;
            for (i = 6; i <= limit; ++i)
            {  // добавлена проверка делимости на 3 и 5. В оригинальной версии алгоритма потребности в ней нет.
                if ((IsPrime[i]) && (i % 3 != 0) && (i % 5 != 0))
                {
                    primeNumbersArrayLength++;
                    Array.Resize<int>(ref primeNumbersArray, primeNumbersArrayLength);
                    primeNumbersArray[primeNumbersArrayLength - 1] = i;
                    //printf(", %d", i);
                }
            }//переделать 111-118 строки!!!!
            for (i = primeNumbersArrayLength-1; i >= 0; i--)
            {
                if (number % primeNumbersArray[i] == 0)
                {
                    do
                    {
                        result += $"{primeNumbersArray[i]} x ";
                        number /= primeNumbersArray[i];
                    }
                    while (number % primeNumbersArray[i] == 0);                        
                    numberIsPrime = false;
                }
            }
            if(!numberIsPrime)
            {
                result = result.Remove(result.Length - 3);
            }
            else
            {
                primeNumbersArrayLength++;
                Array.Resize<int>(ref primeNumbersArray, primeNumbersArrayLength);
                primeNumbersArray[primeNumbersArrayLength - 1] = (int)number;
                result += $"{number}";
            }
            //print prime numbers
            //calculate prime factors
            //result += " " + primeNumbers[k]+" ";//add prime factor to string 
        }
        return result;
    }
}