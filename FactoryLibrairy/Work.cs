using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryLibrairy
{
    public class Work
    {
        public delegate void ThreadWorker(object emp);
        public void DoWork()
        {

            //Unite name
            Unite Unite1 = new Unite();
            Unite Unite2 = new Unite();
            Unite Unite3 = new Unite();
            Unite SUT1 = new Unite();
            Unite SUT2 = new Unite();
            Unite1.Name = "UT1";
            Unite1.ID = 1;
            Unite2.Name = "UT2";
            Unite2.ID = 2;
            Unite3.Name = "UT3";
            Unite3.ID = 3;
            SUT1.Name = "S-UT 1";
            SUT1.ID = 4;
            SUT2.ID = 5;
            SUT2.Name = "S-UT 2";

            //Object ID
            Object object1 = new Object();
            Object object2 = new Object();
            Object object3 = new Object();
            Object object4 = new Object();
            Object object5 = new Object();
            Object object6 = new Object();
            object1.ObjectID = 1;
            object2.ObjectID = 2;
            object3.ObjectID = 3;
            object4.ObjectID = 4;
            object5.ObjectID = 5;
            object6.ObjectID = 6;
            //Thread allocation
            Thread threadUnite1;
            Thread threadUnite2;
            Thread threadUnite3;
            int time = 0;
            int prod = 0;
            int prodtotal = 0;
            bool objetsuivant1 = false;
            bool objetsuivant2 = false;
            bool objetsuivant3 = false;
            bool objetsuivant4 = false;
            ThreadWorker threadwork = o =>
            {

                Unite currentUnit = (Unite)o;
                while (true)
                {

                    if (currentUnit.ID == 1)
                    {
                        try
                        {
                            if (Monitor.TryEnter(object1, 15000) && Monitor.TryEnter(object2, 15000))
                            {
                                Console.WriteLine(currentUnit.Name + " travaille sur les objets 1 et 2");
                                Console.WriteLine($"{currentUnit.Machinename1} travail sur l'objet");
                                Thread.Sleep(500);
                                Console.WriteLine($"{currentUnit.Machinename2} travail sur l'objet");
                                Thread.Sleep(500);
                                Console.WriteLine($"{currentUnit.Machinename3} travail sur l'objet");
                                Thread.Sleep(1000);
                                Console.WriteLine(currentUnit.Name + " à finis son travail");
                                objetsuivant1 = true;
                                objetsuivant2 = true;

                            }
                        }
                        finally
                        {
                            Monitor.Exit(object1);
                            Monitor.Exit(object2);

                            currentUnit.produitfini += 2;
                        }
                        if (objetsuivant1 == true && objetsuivant2 == true)
                        {
                            try
                            {
                                if (Monitor.TryEnter(object3, 15000) && Monitor.TryEnter(object4, 15000))
                                {
                                    Console.WriteLine(currentUnit.Name + " travaille sur les objets 3 et 4");
                                    Console.WriteLine($"{currentUnit.Machinename1} travail sur l'objet");
                                    Thread.Sleep(500);
                                    Console.WriteLine($"{currentUnit.Machinename2} travail sur l'objet");
                                    Thread.Sleep(500);
                                    Console.WriteLine($"{currentUnit.Machinename3} travail sur l'objet");
                                    Thread.Sleep(1000);
                                    Console.WriteLine(currentUnit.Name + " à finis son travail");
                                    prod += 2;
                                    objetsuivant3 = true;
                                    objetsuivant4 = true;
                                }
                            }
                            finally
                            {
                                Monitor.Exit(object3);
                                Monitor.Exit(object4);
                                currentUnit.produitfini += 2;
                            }
                        }
                    }
                    if (currentUnit.ID == 2)
                    {
                        if ((objetsuivant1 == true) && (objetsuivant2 == true))
                        {
                            try
                            {
                                if (Monitor.TryEnter(object1, 15000) && Monitor.TryEnter(object2, 15000))
                                {
                                    Console.WriteLine(currentUnit.Name + " travaille sur les objets 1 et 2");
                                    Console.WriteLine($"{currentUnit.Machinename} travaille sur l'objet");
                                    Thread.Sleep(3000);
                                    Console.WriteLine(currentUnit.Name + " à finis son travail");
                                    objetsuivant1 = false;
                                    objetsuivant2 = false;
                                }
                            }
                            finally
                            {
                                Monitor.Exit(object1);
                                Monitor.Exit(object2);
                                currentUnit.produitfini += 2;
                            }
                        }
                        if ((objetsuivant1 == false) && (objetsuivant2 == false) && (objetsuivant3 == true) && (objetsuivant4 == true))
                        {
                            try
                            {
                                if (Monitor.TryEnter(object3, 15000) && Monitor.TryEnter(object4, 15000))
                                {
                                    Console.WriteLine(currentUnit.Name + " travaille sur les objets 3 et 4");
                                    Console.WriteLine($"{currentUnit.Machinename} travaille sur l'objet");
                                    Thread.Sleep(3000);
                                    Console.WriteLine(currentUnit.Name + " à finis son travail");
                                    objetsuivant3 = false;
                                    objetsuivant4 = false;
                                }
                            }
                            finally
                            {
                                Monitor.Exit(object3);
                                Monitor.Exit(object4);

                                currentUnit.produitfini += 2;
                            }

                        }

                    }
                }
            };
            threadUnite1 = new(threadwork.Invoke);
            threadUnite2 = new(threadwork.Invoke);
            threadUnite1.Start(Unite1);
            threadUnite2.Start(Unite2);

            while (true)
            {
                Thread.Sleep(10000);
                time += 10;
                prodtotal = Unite1.produitfini + Unite2.produitfini + Unite3.produitfini;
                Console.WriteLine($"Etat production: \n ->{prodtotal}");
                Console.WriteLine("timer: " + time);
            }
        }
    }
}
