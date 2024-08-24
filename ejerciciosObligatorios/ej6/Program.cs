﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ej6
{
    internal class Program
    {

        static void Main(string[] args)
        {
            List<Libro> list = new List<Libro>();
            Libro l1 = new Libro(29029891831233, "El visitante", "Stephen King", 592);
            Libro l2 = new Libro(12901290930390, "IT", "Stephen King", 1504);

            list.Add(l1);
            list.Add(l2);

            Libro aux = new Libro();
            foreach (Libro l in list)
            { 
                l.MostrarDetalles();
                if(l.NumPaginas > aux.NumPaginas)
                {
                    aux.NumPaginas = l.NumPaginas;
                    aux.Titulo = l.Titulo;
                }
            }
            Console.WriteLine($"El libro con más paginas es {aux.Titulo}");
            Console.ReadKey();
        }
    }
}