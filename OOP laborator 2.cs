using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum Domeniu
{
    INFORMATICA,
    INGINERIE_ELECTRICA,
    TEHNOLOGIE_ALIMENTARA,
    INGINERIE_MECANICA
}
public class Student
{
    public string IdentificatorUnic { get; set; }
    public string Nume { get; set; }
    public Domeniu Domeniu { get; set; }
    public bool EsteAbsolvent { get; set; }
}
public class Facultate
{
    public string Nume { get; set; }
    public Domeniu Domeniu { get; set; }
    public List<Student> Studenti { get; set; } = new List<Student>();
}
public class SistemManagementStudenti
{
    private List<Facultate> facultati = new List<Facultate>();
    public void CreareSiAtribuireStudent(string nume, string identificatorUnic, Domeniu domeniu, string numeFacultate)
    {
        var facultate = GetFacultateDupaNume(numeFacultate);
        if (facultate != null)
        {
            var student = new Student { Nume = nume, IdentificatorUnic = identificatorUnic, Domeniu = domeniu };
            facultate.Studenti.Add(student);
            Console.WriteLine($"{nume} a fost asignat la {numeFacultate}.");
        }
        else
        {
            Console.WriteLine($"Facultatea {numeFacultate} nu a fost găsită.");
        }
    }
    public void AbsolvireStudent(string identificatorUnic)
    {
        var student = GetStudentDupaIdentificator(identificatorUnic);
        if (student != null)
        {
            student.EsteAbsolvent = true;
            Console.WriteLine($"{student.Nume} a absolvit.");
        }
        else
        {
            Console.WriteLine($"Studentul cu ID-ul {identificatorUnic} nu a fost găsit.");
        }
    }
    public void AfisareStudentiInscrisiCurent()
    {
        var studentiInscrisi = facultati.SelectMany(f => f.Studenti.Where(s => !s.EsteAbsolvent));
        AfisareStudenti(studentiInscrisi, "Studenti Inscrisi Curent");
    }
    public void AfisareAbsolventi()
    {
        var absolventi = facultati.SelectMany(f => f.Studenti.Where(s => s.EsteAbsolvent));
        AfisareStudenti(absolventi, "Absolventi");
    }
    public bool VerificaDacaStudentulApartineFacultatii(string identificatorUnic, string numeFacultate)
    {
        var facultate = GetFacultateDupaNume(numeFacultate);
        return facultate?.Studenti.Any(s => s.IdentificatorUnic == identificatorUnic) ?? false;
    }
    public void CreareFacultateNoua(string nume, Domeniu domeniu)
    {
        var facultate = new Facultate { Nume = nume, Domeniu = domeniu };
        facultati.Add(facultate);
        Console.WriteLine($"Facultatea {nume} a fost creată.");
    }
    public string GasesteFacultateDupaIdentificatorulStudentului(string identificatorUnic)
    {
        var facultate = facultati.FirstOrDefault(f => f.Studenti.Any(s => s.IdentificatorUnic == identificatorUnic));
        return facultate?.Nume ?? "Facultatea nu a fost găsită";
    }
    public void AfisareFacultatiUniversitare()
    {
        AfisareFacultati(facultati, "Facultati Universitare");
    }
    public void AfisareFacultatiDupaDomeniu(Domeniu domeniu)
    {
        var facultatiFiltrate = facultati.Where(f => f.Domeniu == domeniu);
        AfisareFacultati(facultatiFiltrate, $"{domeniu} Facultati");
    }
    public void OperatieInscriereInGrupDinFisierText(string caleFisier)
    {
        try
        {
            var linii = File.ReadAllLines(caleFisier);
            foreach (var linie in linii)
            {
                var date = linie.Split(',');
                if (date.Length == 4)
                {
                    CreareSiAtribuireStudent(date[0], date[1], (Domeniu)Enum.Parse(typeof(Domeniu), date[2]), date[3]);
                }
                else
                {
                    Console.WriteLine($"Date nevalide în fișier: {linie}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la citirea fisierului: {ex.Message}");
        }
    }
    public void OperatieAbsolvireDinFisierText(string caleFisier)
    {
        try
        {
            var linii = File.ReadAllLines(caleFisier);
            foreach (var linie in linii)
            {
                var date = linie.Split(',');
                if (date.Length == 1)
                {
                    AbsolvireStudent(date[0]);
                }
                else
                {
                    Console.WriteLine($"Date nevalide în fișier: {linie}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la citirea fisierului: {ex.Message}");
        }
    }
    private Student GetStudentDupaIdentificator(string identificatorUnic)
    {
        return facultati.SelectMany(f => f.Studenti).FirstOrDefault(s => s.IdentificatorUnic == identificatorUnic);
    }
    private Facultate GetFacultateDupaNume(string numeFacultate)
    {
        return facultati.FirstOrDefault(f => f.Nume == numeFacultate);
    }
    private void AfisareStudenti(IEnumerable<Student> studenti, string titlu)
    {
        Console.WriteLine($"{titlu}:");
        foreach (var student in studenti)
        {
            Console.WriteLine($"- {student.Nume} ({student.IdentificatorUnic}) - {student.Domeniu}");
        }
    }
        private void AfisareFacultati(IEnumerable<Facultate> facultati, string titlu)
    {
        Console.WriteLine($"{titlu}:");
        foreach (var facultate in facultati)
        {
            Console.WriteLine($"- {facultate.Nume} - {facultate.Domeniu}");
        }
    }
}
public class OperationLoggingSystem
{
    public void LogOperation(string operatie)
    {
        Console.WriteLine($"Operația a fost înregistrată: {operatie}");
    }
}
class Program
{
    static void Main()
    {
        var sistemManagementStudenti = new SistemManagementStudenti();
        var operationLoggingSystem = new OperationLoggingSystem();
        while (true)
        {
            Console.WriteLine("\nAlegeți o operație:");
            Console.WriteLine("1. Creare și atribuire student la o facultate");
            Console.WriteLine("2. Absolvire student de la o facultate");
            Console.WriteLine("3. Afișare studenți înscriși curent");
            Console.WriteLine("4. Afișare absolvenți");
            Console.WriteLine("5. Verificare dacă un student aparține unei facultăți");
            Console.WriteLine("6. Creare facultate nouă");
            Console.WriteLine("7. Cautare facultate după identificatorul studentului");
            Console.WriteLine("8. Afișare facultăți universitare");
            Console.WriteLine("9. Afișare facultăți după domeniu");
            Console.WriteLine("10. Operație de înrolare în grup din fișier text");
            Console.WriteLine("11. Operație de absolvire din fișier text");
            Console.WriteLine("0. Ieșire");

            var optiune = Console.ReadLine();
            switch (optiune)
            {
                case "1":
                    Console.Write("Introduceți numele studentului: ");
                    var nume = Console.ReadLine();
                    Console.Write("Introduceți identificatorul unic al studentului: ");
                    var identificatorUnic = Console.ReadLine();
                    Console.Write("Introduceți domeniul studentului (INFORMATICA, INGINERIE_ELECTRICA, TEHNOLOGIE_ALIMENTARA, INGINERIE_MECANICA): ");
                    var domeniu = Enum.Parse<Domeniu>(Console.ReadLine());
                    Console.Write("Introduceți numele facultății: ");
                    var numeFacultate = Console.ReadLine();
                    sistemManagementStudenti.CreareSiAtribuireStudent(nume, identificatorUnic, domeniu, numeFacultate);
                    break;
                case "2":
                    Console.Write("Introduceți identificatorul unic al studentului de absolvit: ");
                    var idAbsolvire = Console.ReadLine();
                    sistemManagementStudenti.AbsolvireStudent(idAbsolvire);
                    break;
                case "3":
                    sistemManagementStudenti.AfisareStudentiInscrisiCurent();
                    break;
                case "4":
                    sistemManagementStudenti.AfisareAbsolventi();
                    break;
                case "5":
                    Console.Write("Introduceți identificatorul unic al studentului: ");
                    var idStudent = Console.ReadLine();
                    Console.Write("Introduceți numele facultății: ");
                    var numeFacultateVerificare = Console.ReadLine();
                    var apartineFacultatii = sistemManagementStudenti.VerificaDacaStudentulApartineFacultatii(idStudent, numeFacultateVerificare);
                    Console.WriteLine($"Studentul aparține facultății: {apartineFacultatii}");
                    break;
                case "6":
                    Console.Write("Introduceți numele noii facultăți: ");
                    var numeNouFacultate = Console.ReadLine();
                    Console.Write("Introduceți domeniul noii facultăți (INFORMATICA, INGINERIE_ELECTRICA, TEHNOLOGIE_ALIMENTARA, INGINERIE_MECANICA): ");
                    var domeniuNouFacultate = Enum.Parse<Domeniu>(Console.ReadLine());
                    sistemManagementStudenti.CreareFacultateNoua(numeNouFacultate, domeniuNouFacultate);
                    break;
                case "7":
                    Console.Write("Introduceți identificatorul unic al studentului: ");
                    var idStudentCautareFacultate = Console.ReadLine();
                    var facultateStudent = sistemManagementStudenti.GasesteFacultateDupaIdentificatorulStudentului(idStudentCautareFacultate);
                    Console.WriteLine($"Facultatea în care studentul este înscris: {facultateStudent}");
                    break;
                case "8":
                    sistemManagementStudenti.AfisareFacultatiUniversitare();
                    break;
                case "9":
                    Console.Write("Introduceți domeniul facultăților de afișat (INFORMATICA, INGINERIE_ELECTRICA, TEHNOLOGIE_ALIMENTARA, INGINERIE_MECANICA): ");
                    var domeniuAfisareFacultati = Enum.Parse<Domeniu>(Console.ReadLine());
                    sistemManagementStudenti.AfisareFacultatiDupaDomeniu(domeniuAfisareFacultati);
                    break;
                case "10":
                    Console.Write("Introduceți calea fișierului text pentru operația de înrolare în grup: ");
                    var caleFisierInrolare = Console.ReadLine();
                    sistemManagementStudenti.OperatieInscriereInGrupDinFisierText(caleFisierInrolare);
                    break;
                case "11":
                    Console.Write("Introduceți calea fișierului text pentru operația de absolvire: ");
                    var caleFisierAbsolvire = Console.ReadLine();
                    sistemManagementStudenti.OperatieAbsolvireDinFisierText(caleFisierAbsolvire);
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opțiune invalidă. Reîncercați.");
                    break;
            }
        }
    }
}

