using System;

class TransportTask
{
    static int[,] cost =
    {
        {40, 19, 25, 25, 35},
        {49, 26, 27, 18, 38},
        {46, 27, 36, 40, 45}
    };

    static void Main()
    {
        Console.WriteLine("Транспортная задача");
        Console.WriteLine("1 - Метод северо-западного угла");
        Console.WriteLine("2 - Метод минимальных элементов");
        Console.Write("Выберите метод: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Ошибка ввода");
            Console.ReadKey();
            return;
        }

        if (choice == 1)
            NorthWestCorner();
        else if (choice == 2)
            LeastCostMethod();
        else
            Console.WriteLine("Неверный выбор");

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void NorthWestCorner()
    {
        int[] supply = { 230, 250, 170 };
        int[] demand = { 140, 90, 160, 110, 150 };

        int[,] x = new int[3, 5];
        int i = 0, j = 0;

        while (i < 3 && j < 5)
        {
            int value = Math.Min(supply[i], demand[j]);
            x[i, j] = value;
            supply[i] -= value;
            demand[j] -= value;

            if (supply[i] == 0) i++;
            else j++;
        }

        PrintPlan(x, "Метод северо-западного угла");
    }

    static void LeastCostMethod()
    {
        int[] supply = { 230, 250, 170 };
        int[] demand = { 140, 90, 160, 110, 150 };

        int[,] x = new int[3, 5];

        while (true)
        {
            int min = int.MaxValue;
            int mi = -1, mj = -1;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 5; j++)
                    if (supply[i] > 0 && demand[j] > 0 && cost[i, j] < min)
                    {
                        min = cost[i, j];
                        mi = i;
                        mj = j;
                    }

            if (mi == -1) break;

            int value = Math.Min(supply[mi], demand[mj]);
            x[mi, mj] = value;
            supply[mi] -= value;
            demand[mj] -= value;
        }

        PrintPlan(x, "Метод минимальных элементов");
    }

    static void PrintPlan(int[,] x, string title)
    {
        Console.WriteLine("\n" + title);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
                Console.Write($"{x[i, j],5}");
            Console.WriteLine();
        }
    }
}
