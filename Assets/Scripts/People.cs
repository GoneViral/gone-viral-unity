using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.IMGUI.Controls;
using Random = System.Random;

public enum Status
{
    InHospital, InQuarantine, OutandAbout, Dead
}


public class People : MonoBehaviour
{
    //Chances to recover or die
    private static Double CtHealinHospital = 0.2;
    private static Double CtDieinHospital = 0.05;
    private static Double CtHealinQuarantine = 0.05;
    private static Double CtDie = 0.2;
    //chance to show symptoms
    private static Double CtshowSymptoms = 0.4;

    public int counter;
        
    
        public Boolean infected;
        public Boolean showsSymptoms;
        public Status status;
        public int age;
        public String type;
        
        public People(int age, String type)
        {
            this.age = age;
            this.type = type;
            infected = false;
            status = Status.OutandAbout;
        }

        public People()
        {
            infected = false;
            status = Status.OutandAbout;
        }

        //method to be called upon (every Tick?) to heal/kill infected
        void updateDisease() 
        {
            //should age be implemented
            int factor = 1;
            try
            {
                factor *= age / 100; //TODO very simple
            }
            catch (NullReferenceException e)
            {
                //do nothing 
            }

            if (this.infected)
            {
                //Person is infected and in Hospital will heal
                if (this.status == Status.InHospital)
                {
                    if (new Random().NextDouble() < CtHealinHospital * factor)
                    {
                        this.infected = false;
                        this.status = Status.OutandAbout;
                    }
                    //...and die
                    else if (new Random().NextDouble() < CtDieinHospital * factor)
                    {
                        this.status = Status.Dead;

                        //TODO remove player in some way
                    }
                }
                //Person is infected and im Quarantine will heal
                else if (new Random().NextDouble() < CtHealinQuarantine * factor)
                {
                    this.infected = false;
                    this.status = Status.OutandAbout;
                }
                //chance to die
                else if (new Random().NextDouble() < CtDie * factor)
                {
                    this.status = Status.Dead;

                    //TODO remove player in some way
                }


            }

        }

        void quarentine()
        {
            this.status = Status.InQuarantine;
        }

        void hospitalize()
        {
            this.status = Status.InHospital;
        }

        //calculates if this person shows symptoms
        public Boolean showingSymptoms()
        {
            if (new Random().NextDouble() < CtshowSymptoms)
            {
                return true;
            }

            return false;
        }

        public void setToInfected()
        {
            this.infected = true;
            if (showingSymptoms())
            {
                this.showsSymptoms = true;
            }
        }

        public Boolean getInfected()
        {
            return this.infected;
        }

        public void AddVirus(float fRange)
        {
            if (getInfected())
                return;
            
            setToInfected();
            GameLogic.instance.infectedCount++;
            gameObject.AddComponent<VirusPlayable>();
        }
}
