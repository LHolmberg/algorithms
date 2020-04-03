using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

class Program
{
    TestClass[] arr = null;
    List<NobelPrizeWinner> nobelPrizes = new List<NobelPrizeWinner>();

    static void Main(string[] args) {
        new Program().Run();
    }
    
    void Run() {
        Startup();
        Startup(nobelPrizes);

        string input;
        while (true) {
            Console.Write("Command (use help if needed) > ");
            input = Console.ReadLine();

            if (input.Length > 5 && input.Substring(0, 5) == "model" && input.Split(" ").Length == 2) {

                Console.WriteLine("SORTING THE MODEL NAMES ALPHABETICALLY");
                BubbleSort(arr);

                PrintArray();

                int res = LinearSearch(arr, input.Split(" ")[1]);
                if (res == -1)
                    Console.WriteLine("Not found");
                else
                    Console.WriteLine("Found at index " + res + " after sort");
            }

            else if (input.Length > 2 && input.Substring(0, 2) == "id" && input.Split(" ").Length == 2) {

                Console.WriteLine("SORTING THE ID'S");
                SelectionSort(arr);

                PrintArray();

                int res = BinarySearch(arr, Convert.ToInt32(input.Split(" ")[1]));
                if (res == -1)
                    Console.WriteLine("Not found");
                else
                    Console.WriteLine("Found at index " + res + " after sort");
            }

            else if (input == "valfri") {
                Console.WriteLine("UNSORTED: ");
                PrintLinkedList();
                Console.WriteLine("SORTED: ");
                MergeSort(nobelPrizes, 0, nobelPrizes.Count - 1);
                PrintLinkedList();
            }

            else if (input == "print")
                PrintArray();

            else if (input == "help") {
                Console.WriteLine("###############");
                Console.WriteLine("Availible commands:");
                Console.WriteLine("print");
                Console.WriteLine("model xxxx");
                Console.WriteLine("id x");
                Console.WriteLine("valfri");
                Console.WriteLine("###############");
            }

            else
                Console.WriteLine("Unknown command");
        }
    }

    private void TryInput(string msg, out int val)
    {
        while (true) {
            try {
                Console.Write(msg);
                val = int.Parse(Console.ReadLine());
                break;
            }
            catch (Exception e) {
                Console.WriteLine("{0} : Wrong format", e.GetType().Name);
            }
        }
    }

    void PrintArray() {
        Console.WriteLine("");
        for (int i = 0; i < arr.Length; i++)
            Console.WriteLine("Index " + i + " > Model: " + arr[i].model + " ID: " + arr[i].id);
        Console.WriteLine("");
    }

    void PrintLinkedList() {
        Console.WriteLine("");
        for (int i = 0; i < nobelPrizes.Count; i++) {
            Console.WriteLine("Name: " + nobelPrizes[i].name + " Born: " + nobelPrizes[i].born);
        }
        Console.WriteLine("");
    }

    void Startup() {
        TryInput("Input a size of the array > ", out int arrSize);
        arr = new TestClass[arrSize];

        Console.Write("");
        TryInput("How long do you want the randomized string 'model' to be? > ", out int modelSize);

        for (int i = 0; i < arr.Length; i++)
            arr[i] = new TestClass(modelSize);
    }

    void Startup(List<NobelPrizeWinner> nP) {

        using (StreamReader file = File.OpenText(@"..\..\..\NobelPrize.json"))

        using (JsonTextReader reader = new JsonTextReader(file)) {
            JObject o = (JObject)JToken.ReadFrom(reader);

            int length = ((JArray)o["laureates"]).Count;
            int age;
            string name;

            for (int i = 0; i < length; i++) {
                if (o["laureates"][i]["prizes"][0]["category"].ToString() == "physics") {
                    age = Convert.ToInt32(o["laureates"][i]["born"].ToString().Split("-")[0]);
                    name = o["laureates"][i]["firstname"].ToString();
                    nP.Add(new NobelPrizeWinner(name, age));
                }
            }
        }
    }

    void Merge(List<NobelPrizeWinner> nP, int l, int m, int r) // O(n log n)
    {
        int i, j, k;
        int n1 = m - l + 1;
        int n2 = r - m;

        List<NobelPrizeWinner> L = new List<NobelPrizeWinner>();
        List<NobelPrizeWinner> R = new List<NobelPrizeWinner>();

        for (i = 0; i < n1; i++)
            L.Add(nP[l + i]);
        for (j = 0; j < n2; j++)
            R.Add(nP[m + 1 + j]);

        i = 0; 
        j = 0; 
        k = l;

        while (i < n1 && j < n2) {
            if (L[i].born <= R[j].born) {
                nP[k] = L[i];
                i++;
            }
            else {
                nP[k] = R[j];
                j++;
            }
            k++;
        }

        while (i < n1) {
            nP[k] = L[i];
            i++;
            k++;
        }

        while (j < n2) {
            nP[k] = R[j];
            j++;
            k++;
        }
    }

    void MergeSort(List<NobelPrizeWinner> nP, int l, int r) // O(n log n)
    {
        if (l < r) {
            int m = l + (r - l) / 2;

            MergeSort(nP, l, m);
            MergeSort(nP, m + 1, r);

            Merge(nP, l, m, r);
        }
    }
    void BubbleSort(TestClass[] arr) {
        bool swapped = true;
        int count = arr.Length;
        while (swapped) {
            swapped = false;
            for (int i = 0; i < count - 1; i++) {
                if (arr[i].model[0] > arr[i + 1].model[0]) {
                    TestClass t = arr[i];
                    arr[i] = arr[i + 1];
                    arr[i + 1] = t;
                    swapped = true;
                }
            }
            count--;
        }
    }

    void SelectionSort(TestClass[] arr) {
        for (int i = 0; i < arr.Length - 1; i++) {
            int t = i;
            for (int j = i + 1; j < arr.Length; j++)
                if (arr[j].id < arr[t].id)
                    t = j;

            TestClass temp = arr[t];
            arr[t] = arr[i];
            arr[i] = temp;
        }
    }

    int LinearSearch(TestClass[] arr, string val) { // O(n)
        for (int i = 0; i < arr.Length; i++) {
            if (arr[i].model == val)
                return i;
        }
        return -1;
    }

    int BinarySearch(TestClass[] arr, int x) { // O(log n)
        int n = 0, j = arr.Length - 1;
        while (n <= j) {
            int k = n + (j - n) / 2;

            if (arr[k].id == x)
                return k;

            if (arr[k].id < x)
                n = k + 1;

            else
                j = k - 1;
        }
            
        return -1;
    }
}
