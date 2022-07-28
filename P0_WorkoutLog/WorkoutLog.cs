using System;
using Exercises;
using WorkoutDay;
using WorkoutProgram;

namespace Test
{

    // struct to validate user input
    public struct UserInput
    {
        public bool valid;
        public string name;
    }

    // WorkoutPlan is a class that hold a list of Programs
    class WorkoutPlan
    {

        List<Program> programs = new List<Program>();

        public static void Main(string[] args)
        {
            // create workoutlog object
            WorkoutPlan wlog = new WorkoutPlan();


            int choice = 0;
            do
            {

                Console.WriteLine("Enter a Workout Program name to: Create/Print/Update/Delete/Retrieve From. Enter [exit] to exit the application."); // ask user for a program name
                UserInput output = wlog.getProgramName();
                if (output.valid == false)
                {
                    Console.WriteLine("Name is invalid");
                    break;
                }
                string name = output.name;

                // main menu
                Console.WriteLine("Enter a number based on the following options:\n[1] Create program\n[2] Print program\n[3] Update program (add exercises)\n[4] Delete program\n[5] Retrieve from");
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        // create program
                        if (wlog.checkProgramExistence(name)) { Console.WriteLine("Program exists already"); continue; } // check if program exists, if so then don't create another one with the same name
                        Console.WriteLine("Creating a program...");
                        wlog.createProgram(name);
                        Console.WriteLine("Creation complete");
                        wlog.printPrograms();
                        break;
                    case 2:
                        // print program
                        if (!wlog.checkProgramExistence(name)) { Console.WriteLine("Program isn't populated yet"); continue; } // check if program exists, if not then don't perform operation
                        Console.WriteLine("Printing program...");
                        wlog.displayProgram(name);
                        break;
                    case 3:
                        // update program
                        if (!wlog.checkProgramExistence(name)) { Console.WriteLine("Program isn't populated yet"); continue; } // check if program exists, if not then don't perform operation
                        Console.WriteLine("Updating program...");
                        wlog.updateProgram(name);
                        Console.WriteLine("Updates complete");
                        wlog.printPrograms();
                        break;
                    case 4:
                        // delete program
                        if (!wlog.checkProgramExistence(name)) { Console.WriteLine("Program isn't populated yet"); continue; } // check if program exists, if not then don't perform operation
                        Console.WriteLine("Deleting program...");
                        wlog.deleteProgram(name);
                        Console.WriteLine("Deletion complete");
                        wlog.printPrograms();
                        break;
                    case 5:
                        // retrieve from program
                        if (!wlog.checkProgramExistence(name)) { Console.WriteLine("Program isn't populated yet"); continue; } // check if program exists, if not then don't perform operation
                        Console.WriteLine("Retrieving program...");
                        wlog.retrieveOther(name);
                        Console.WriteLine("Retrieving complete");
                        break;
                    default:
                        Console.WriteLine("Input is not valid");
                        break;
                }
            } while (choice != -1);
        }



        // creates a program and add it to this list of programs
        public Program createProgram(string name)
        {
            Program? p = new Program(name);

            if (p == null)
            {
                Console.WriteLine($"Couldn't create program {name}");
            }
            else if (p.program.Count == 0)
            {
                Console.WriteLine("No days were added. Operation unsuccessful");
            }
            else
            {
                Console.WriteLine($"Successfully created program: {name}");
                programs.Add(p);
            }

            return p;
        }

        // updates a program
        public void updateProgram(string name)
        {

            Program? p = this.programs.Find(e => e.Name == name);
            if (p == null)
            {
                Console.WriteLine($"Couldn't find program with name {name}");
                return;
            }

            p.updateProgram();
        }

        // finds a program with [name] and delete it from this list of programs
        public void deleteProgram(string name)
        {
            int index = this.programs.FindIndex(p => p.Name == name);
            this.programs.RemoveAt(index);

        }


        //  method performs option 5 (retrieve) of the main menu, which get calories and difficult level of a workout plan
        public void retrieveOther(string name)
        {
            int index = this.programs.FindIndex(p => p.Name == name);// find the program in the list

            Console.WriteLine("Choose an option from the following list:\n[1] Get calories burned\n[2] See exercise difficulties");
            string? choice = Console.ReadLine();
            if (choice == null)
            {
                Console.WriteLine("Invalid input. Returning...");
                return;
            }

            while (choice != "1" && choice != "2")// keep asking the user for valid input
            {
                Console.WriteLine("Invalid input. Please select [1] or [2], or enter [exit] to exit");
                choice = Console.ReadLine();
                if (choice == "exit")
                {
                    Console.WriteLine("Operation aborted");
                    return;
                }
            }


            // based on user input, execute calorie calculation option, sort exercises by difficulty level or neither
            if (choice == "1")
            {
                int cals = 0;
                foreach (var p in this.programs[index].program)// looping the the days of the program
                {
                    foreach (var e in p.ExercisesToday)
                    {
                        cals += e.CaloriesBurned;
                    }
                }
                Console.WriteLine($"You burn {cals} calories a workout");
            }
            else if (choice == "2")
            {
                // create a hash map of difficulties that have a list of exercises that have that difficulty
                Dictionary<string, List<string>> table = new Dictionary<string, List<string>>();
                foreach (var p in this.programs[index].program)
                {
                    foreach (var ex in p.ExercisesToday)
                    {
                        string? diff = ex.Difficulty.ToLower();
                        if (table.ContainsKey(diff))
                        {
                            table[diff].Add(ex.Name);
                        }
                        else
                        {
                            table.Add(diff, new List<string>());
                            table[diff].Add(ex.Name);
                        }
                    }
                }

                // print out the difficult categories in a nice format
                int e = 0;
                int n = 0;
                int h = 0;
                if (table.ContainsKey("easy")) e = table["easy"].Count;
                if (table.ContainsKey("normal")) n = table["normal"].Count;
                if (table.ContainsKey("hard")) h = table["hard"].Count;

                int max = Math.Max(e, Math.Max(n, h));
                Console.WriteLine("EASY\t\t\tNORMAL\t\t\tHARD");
                int ein = 0, nin = 0, hin = 0;
                for (int i = 0; i < max; i++)
                {
                    if (ein < e)
                    {
                        Console.Write($"{table["easy"][ein++]}\t\t\t");
                    }
                    else
                    {
                        Console.Write("\t\t\t");
                    }

                    if (nin < n)
                    {
                        Console.Write($"{table["normal"][nin++]}\t\t\t");
                    }
                    else
                    {
                        Console.Write("\t\t\t");
                    }

                    if (hin < h)
                    {
                        Console.WriteLine($"{table["hard"][hin++]}");
                    }
                    else
                    {
                        Console.WriteLine("\t\t\t");
                    }
                }

            }
            else
            {
                Console.WriteLine("No option selected");
            }
        }

        // get valid user input
        public UserInput getProgramName()
        {
            UserInput choice;
            string? name;

            name = Console.ReadLine();
            if (name == null || name.ToLower() == "exit" || name == "")
            {
                choice.valid = false;
                choice.name = "";
                return choice;
            }

            name = name.ToLower();
            choice.valid = true;
            choice.name = name;
            return choice;
        }



        // displays an overview of all programs, their days, and exercises
        public void printPrograms()
        {
            foreach (var p in this.programs)
            {
                Console.WriteLine($"\t{p.Name}");
                foreach (var d in p.program)
                {
                    Console.WriteLine($"\t\t{d.Day}");
                    foreach (var e in d.ExercisesToday)
                    {
                        Console.WriteLine($"\t\t\t{e.Name}");
                    }
                }
            }
        }

        // prints an overview of a program, its days and exercises
        public void displayProgram(string name)
        {

            int index = this.programs.FindIndex(p => p.Name == name);

            Console.WriteLine("Program Name\t\tDays\t\tExercises");
            Console.WriteLine(name);

            foreach (var d in this.programs[index].program)
            {
                Console.WriteLine($"\t\t\t{d.Day}");
                foreach (var e in d.ExercisesToday)
                {
                    Console.WriteLine($"\t\t\t\t\t{e.Name}");
                }
            }
        }

        // checks the list of programs for a specific program name
        public bool checkProgramExistence(string name)
        {
            return this.programs.Find(e => e.Name == name) != null;
        }

    }
}