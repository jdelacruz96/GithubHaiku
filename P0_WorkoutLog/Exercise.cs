using System;


namespace Exercises
{

    // Exercise class to inherit from
    abstract class Exercise

    {// fields
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Difficulty { get; set; }
        public int CaloriesBurned { get; set; }
        public DateTime DatePerformed { get; set; }
    }

    // Weights class
    class Weights : Exercise
    {

        public int Reps { get; set; }
        public int Sets { get; set; }
        public double Pounds { get; set; }
        public string Muscle { get; set; }

        public Weights(string name, string startTime, string endTime, int reps, int sets, double pounds, DateTime datePerformed, string difficulty, string muscleGroup, int caloriesBurned)
        {
            this.CaloriesBurned = caloriesBurned;
            this.Name = name;
            this.Type = "Weights";
            this.Difficulty = difficulty;
            this.DatePerformed = datePerformed;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Reps = reps;
            this.Sets = sets;
            this.Pounds = pounds;
            this.Muscle = muscleGroup;
        }
    }

    // Cardio class
    class Cardio : Exercise
    {

        public Cardio(string name, DateTime datePerformed, string difficulty, string startTime, string endTime, int caloriesBurned, double Miles = 12.23, int Steps = 0)
        {
            this.Name = name;
            this.Type = "Cardio";
            this.Difficulty = difficulty;
            this.DatePerformed = datePerformed;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.CaloriesBurned = caloriesBurned;
        }

    }
}