//using DV;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
//using HarmonyLib;
//using UnityEngine;
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
public static class MyClass
{
    public static void Main()
    {
        // gradeEvent; // grade as a percent
        // gradeDecimal; // grade as a decimal
        // locomotiveWeight; // locomotive weight
        // adhesionFromGrade; // adhesion from grade
        // railAdhesionVar; // rail adhesion coefficient as %. to be replaced with weather
        // railAdhesionDecimal; // rail adhesion coefficient as decimal
        // railAdhesionCoefficient; = rail adhesion decimal. just-in-case futureproofing
        // dragForceFromGrade; drag from grade
        // gradeTractiveEffort; final equation


        
        /// locomotive weight
        var locos = trainset.traincars.Where(c => CarTypes.IsLocomotive(c.cartype));
        // ----------------------------------------------------
        // var sumWeight = locos.Sum(loco => loco.totalMass);
        // or:
        // ----------------------------------------------------
        float sumWeight = 0;
        foreach (var loco in locos)
        {
           sumWeight += loco.totalMass;
        }
        Console.WriteLine(); // TODO: remove write-to-console
        Console.WriteLine("sumWeight = {0}t", sumWeight);

        /// adhesion from grade
        /*   
        line 26-30, GeneralProviders.cs:
        car => {
            var invlination = car.transform.localEulerAngles.x;
            incination = inclination > 180 ? 360f - inclination : -inclination;
            return Mathf.Tan(inclination * Mathf.PI / 180) * 100;
        },
        // TODO: find what 'x' is defined as, double-check that 'car' isn't defined somewhere else


        line 68-71, TrackIndexer.cs:
        public static float Grade(EquiPointSet.Point point)
        {
            return Mathf.RountToInt(point.forward.y * 200) / 2f;
        }
        // TODO: see if point.forward.y is defined elsewhere?
        */
        float gradeEvent = 6.0f; // TODO: replace with actual grade code
        float gradeDecimal = gradeEvent / 100; // dividing by 100 for grade % as a decimal
        float adhesionFromGrade = 1 - 2 * Mathf.Atan(gradeDecimal) / 90; // does the actual equation
        Console.WriteLine(); // TODO: remove write-to-console
        Console.WriteLine("adhesionFromGrade  = {0}", adhesionFromGrade); // writes to console

        /* rail adhesion coefficient
        notes:
        rail adhesion coefficient is currently fixed, at 22.5%. in the future when weather is added and if when track wear is
        added it has an effect on rail adhesion, railAdhesionVar will be the variable used to get that.
        It also currently mimics a percentage to begin with, in case that is how adhesion is handled. */
        float railAdhesionVar = 22.5f;
        float railAdhesionDecimal = railAdhesionVar / 100;
        float railAdhesionCoefficient = railAdhesionDecimal;
        Console.WriteLine(); // TODO: remove write-to-console
        Console.WriteLine("railAdhesionCoefficient = {0}", railAdhesionCoefficient);
        
        // drag force from grade
        float dragForceFromGrade = 1 / gradeDecimal;
        Console.WriteLine(); // TODO: remove write-to-console
        Console.WriteLine("dragForceFromGrade = {0}", dragForceFromGrade);

        // grade tractive effort (final equation):
        float gradeTractiveEffort = locomotiveWeight * adhesionFromGrade * railAdhesionCoefficient * dragForceFromGrade;
        Console.WriteLine(); // TODO: remove write-to-console
        Console.WriteLine("gradeTractiveEffort = {0}", gradeTractiveEffort);
    }
}
/*
Locomotive Weight Ratings (printed):
DE2: 400t
SH282: 1000t
DE6: 1400t
2.3% grade, DE2: 383.21213848 t
*/