using System;
using System.Collections.Generic;
using System.IO;

namespace app
{
    internal class Program
    {
        static List<user> users = new List<user>();
        static List<cars> cars = new List<cars>();
        static string usersFile = "users.txt";
        static string carsFile = "cars.txt";

        static void Main(string[] args)
        {
            LoadUserData();
            LoadCarData();

            string role;
            char op;

            while (true)
            {
                op = Menu();
                if (op == '1')
                {
                    Console.Clear();
                    SignUp();
                }
                else if (op == '2')
                {
                    Console.Clear();
                    role = SignIn();
                    if (role == "Admin" || role == "admin")
                    {
                        AdminConsole();
                    }
                    else
                    {
                        UserConsole();
                    }
                }
                else if (op == '3')
                {
                    Console.WriteLine("Exiting");
                    SaveUserData();
                    SaveCarData();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

        static char Menu()
        {
            Console.WriteLine("1. Sign Up");
            Console.WriteLine("2. Sign in");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            return Console.ReadLine()[0];
        }

        static void SignUp()
        {
            string name, role;
            int password;
            Console.WriteLine("Enter the details:");
            Console.Write("Enter the name: ");
            name = Console.ReadLine();
            Console.Write("Enter the password: ");
            password = int.Parse(Console.ReadLine());
            Console.Write("Enter the role: ");
            role = Console.ReadLine();

            if (!UserExists(name))
            {
                Console.WriteLine("Sign Up successful.");
                users.Add(new user(name, password, role));
            }
            else
            {
                Console.WriteLine("User already exists. Please choose a different name.");
            }
        }

        static string SignIn()
        {
            string name;
            int password;

            Console.WriteLine("Enter the name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter the password: ");
            password = int.Parse(Console.ReadLine());

            foreach (user user in users)
            {
                if (user.name == name && user.password == password)
                {
                    Console.WriteLine("Sign In successful.");
                    return user.role;
                }
            }

            Console.WriteLine("Login not successful");
            return null;
        }

        static void AdminConsole()
        {
            char choice;
            while (true)
            {
                Console.WriteLine("1. Enter car details.");
                Console.WriteLine("2. Remove car.");
                Console.WriteLine("3. View all cars.");
                Console.WriteLine("4. Logout.");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine()[0];
                if (choice == '1')
                {
                    AddCar();
                }
                else if (choice == '2')
                {
                    RemoveCar();
                }
                else if (choice == '3')
                {
                    ViewAllCars();
                }
                else if (choice == '4')
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }

        static void UserConsole()
        {
            char choice;
            while (true)
            {
                Console.WriteLine("1. View car details.");
                Console.WriteLine("2. Purchase car.");
                Console.WriteLine("3. Check news and events.");
                Console.WriteLine("4. Logout.");
                Console.Write("Enter your choice: ");
                choice = Console.ReadLine()[0];
                if (choice == '1')
                {
                    ViewAllCars();
                }
                else if (choice == '2')
                {
                    PurchaseCar();
                }
                else if (choice == '3')
                {
                    CheckNewsEvents();
                }
                else if (choice == '4')
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }

        static void AddCar()
        {
            string name, model, year;
            Console.WriteLine("Enter the car details:");
            Console.Write("Enter the name of the car: ");
            name = Console.ReadLine();
            Console.Write("Enter the model of the car: ");
            model = Console.ReadLine();
            Console.Write("Enter the year of the car: ");
            year = Console.ReadLine();

            if (!CarExists(name, model, year))
            {
                Console.WriteLine("Car added successfully.");
                cars.Add(new cars(name, model, year));
            }
            else
            {
                Console.WriteLine("This car already exists.");
            }
        }

        static void RemoveCar()
        {
            string name, model, year;
            Console.WriteLine("Enter the car details to remove:");
            Console.Write("Enter the name of the car: ");
            name = Console.ReadLine();
            Console.Write("Enter the model of the car: ");
            model = Console.ReadLine();
            Console.Write("Enter the year of the car: ");
            year = Console.ReadLine();

            cars carToRemove = cars.Find(c => c.name == name && c.model == model && c.year == year);
            if (carToRemove != null)
            {
                cars.Remove(carToRemove);
                Console.WriteLine("Car removed successfully.");
            }
            else
            {
                Console.WriteLine("Car not found.");
            }
        }

        static void ViewAllCars()
        {
            foreach (cars c in cars)
            {
                Console.WriteLine($"Name: {c.name}, Model: {c.model}, Year: {c.year}");
            }
        }

        static void PurchaseCar()
        {
            // Logic for purchasing car
            Console.WriteLine("Purchase car option selected.");
        }

        static void CheckNewsEvents()
        {
            // Logic for checking news and events
            Console.WriteLine("Check news and events option selected.");
        }

        static bool UserExists(string name)
        {
            foreach (user user in users)
            {
                if (user.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        static bool CarExists(string name, string model, string year)
        {
            foreach (cars car in cars)
            {
                if (car.name == name && car.model == model && car.year == year)
                {
                    return true;
                }
            }
            return false;
        }

        static void LoadUserData()
        {
            if (File.Exists(usersFile))
            {
                string[] lines = File.ReadAllLines(usersFile);
                foreach (string line in lines)
                {
                    string[] data = line.Split(',');
                    if (data.Length >= 3)
                    {
                        string name = data[0];
                        int password;
                        if (int.TryParse(data[1], out password)) // Parse password as an integer
                        {
                            string role = data[2];

                            users.Add(new user(name, password, role));
                        }
                        else
                        {
                            Console.WriteLine("Invalid password format for user.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid data format for user.");
                    }
                }
            }
        }

        static void SaveUserData()
        {
            using (StreamWriter writer = new StreamWriter(usersFile))
            {
                foreach (user user in users)
                {
                    writer.WriteLine($"{user.name},{user.password},{user.role}");
                }
            }
        }

        static void LoadCarData()
        {
            if (File.Exists(carsFile))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(carsFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] data = line.Split(',');
                            if (data.Length >= 3)
                            {
                                string name = data[0];
                                string model = data[1];
                                string year = data[2];

                                cars.Add(new cars(name, model, year));
                            }
                            else
                            {
                                Console.WriteLine("Invalid data format for car.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading car data: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Car data file does not exist.");
            }
        }

        static void SaveCarData()
        {
            using (StreamWriter writer = new StreamWriter(carsFile))
            {
                foreach (cars car in cars)
                {
                    writer.WriteLine($"{car.name},{car.model},{car.year}");
                }
            }
        }
    }

   

    
}
