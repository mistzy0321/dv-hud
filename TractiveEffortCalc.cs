using DV;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
//namespace HeadsUpDisplay
//{

// 2.3% grade, DE2: 383.21213848 t
// <summary>
// LocoWeight * LocoAdhesionFromGrade * RailAdhesionCoefficient * DragForceFromGrade
// LocoAdhesionFromGrade: 1 - 2(arctangent(grade)) / 90
// RailAdhesionCoefficient: 0.225 (currently)
// DragForceFromGrade: 1 / grade
// </summary>
// DE2: 38t, 38000kg
// SH282: 100t (100000kg) locomotive, 50t (50000kg) tender
// DE6: 115t (115000kg)

// to display basic info about what it could do given different grades, might make an equation variable?
// or assign different variables that will run different numbers through the equation
namespace DvMod.HeadsUpDisplay
{
    public static class MyClass
    {
        public static void Main()
        {
            // function of code:
            // LocoWeight * LocoAdhesionFromGrade * RailAdhesion * DragForceFromGrade
            // LocoWeight . . . . . . . . . . lines 35-37
            // LocoAdhesionFromGrade: . . . . lines 40-48
            // RailAdhesion: lines  . . . . . lines 51-59
            // DragForceFromGrade . . . . . . lines 62-63
            // Final Equation . . . . . . . . lines 66-67

            /// locomotive weight
            var locos = trainset.traincars.Where(c => CarTypes.IsLocomotive(c.cartype));
            var sumWeight = locos.Sum(loco => loco.totalMass);

            /// adhesion from grade
            public static float GetCarGrade(TrainCar car)
            {
                var inclination = car.transform.localEulerAngles.x;
                incination = inclination > 180 ? 360f - inclination : -inclination;
                return Mathf.Tan(inclination * Mathf.PI / 180) * 100;
            },
            float gradeDecimal = gradeEvent / 100; // dividing by 100 for grade % as a decimal
            float adhesionFromGrade = 1 - 2 * Mathf.Atan(gradeDecimal) / 90; // does the actual equation

            //rail adhesion coefficient
            float railAdhesionVar = 22.5f; // defining the coefficient
            /* ---- notes ----
            the rail adhesion coefficient is currently fixed, at 22.5%. in the future, when weather is added,
            and if, when track wear is added, it has an affect on rail adhesion, railAdhesionVar will be the variable to replace.
            Additionally, it handles a percentage to begin with, in case that is how adhesion is handled.
            */
            float railAdhesionDecimal = railAdhesionVar / 100; // changing the percentage to a decimal (again, futureproofing)
            float railAdhesionCoefficient = railAdhesionDecimal; // rail adhesion decimal. additional, final variable, allowing for more equation flexibility in future

            // drag force from grade
            float dragForceFromGrade = 1 / gradeDecimal;

            // grade tractive effort (final equation)
            float gradeTractiveEffort = sumWeight * adhesionFromGrade * railAdhesionCoefficient * dragForceFromGrade;
        }
    }
}
/*
Locomotive Weight Ratings (printed):
DE2: 400t
SH282: 1000t
DE6: 1400t
2.3% grade, DE2: 383.21213848 t
*/