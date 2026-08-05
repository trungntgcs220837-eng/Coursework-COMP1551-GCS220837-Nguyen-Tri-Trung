using System;
using System.Collections.Generic;

namespace DesktopInformationSystem
{
    // ===================================================================================
    // MODULE: COMP1551 - APPLICATION DEVELOPMENT
    // SYSTEM: EDUCATION CENTRE DESKTOP INFORMATION SYSTEM
    // ARCHITECTURAL DESIGN: OBJECT-ORIENTED DOMAIN MODEL WITH POLYMORPHIC DATA STRUCTURES
    // ===================================================================================

    /// <summary>
    /// Strongly-typed enumeration defining the discrete user roles within the system.
    /// Replaces loose string literals to eliminate typos, enforce type safety, and improve code maintainability.
    /// </summary>
    public enum RoleType
    {
        Teacher,
        Admin,
        Student
    }

    /// <summary>
    /// Abstract base class representing the core generic entity 'Person' in the domain model.
    /// Implements the Abstraction principle by capturing common attributes of all human entities in the centre,
    /// and strict Encapsulation by shielding internal state behind private backing fields with properties.
    /// </summary>
    public abstract class Person
    {
        // -------------------------------------------------------------------------------
        // ENCAPSULATION & DATA HIDING: Private backing fields prevent external direct access
        // -------------------------------------------------------------------------------
        private string _name;
        private string _telephone;
        private string _email;

        // -------------------------------------------------------------------------------
        // PUBLIC PROPERTIES WITH DOMAIN VALIDATION (INVARIANTS PROTECTION)
        // -------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the full name. Throws an ArgumentException if an empty string is provided.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                // Validation Rule: Name must not be null, empty, or composed solely of whitespace
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the contact telephone number with mandatory non-empty state validation.
        /// </summary>
        public string Telephone
        {
            get { return _telephone; }
            set
            {
                // Validation Rule: Telephone is a critical identifier for contact and must be present
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Telephone cannot be empty.");
                _telephone = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique email address. Enforces fundamental format structure integrity.
        /// </summary>
        public string Email
        {
            get { return _email; }
            set
            {
                // Structural Validation Rule: Ensures basic email compliance (must contain '@' domain separator)
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");
                _email = value;
            }
        }

        /// <summary>
        /// Categorical user role assigned upon object instantiation. Auto-property used as validation is managed by Enum boundaries.
        /// </summary>
        public RoleType Role { get; set; }

        /// <summary>
        /// Parameterized constructor initializing base common properties and enforcing baseline domain rules.
        /// </summary>
        /// <param name="name">Full legal name of the individual.</param>
        /// <param name="telephone">Contact telephone string.</param>
        /// <param name="email">Unique identity email address.</param>
        /// <param name="role">RoleType classification category.</param>
        public Person(string name, string telephone, string email, RoleType role)
        {
            Name = name;
            Telephone = telephone;
            Email = email;
            Role = role;
        }

        /// <summary>
        /// Dynamic dispatch base method (Polymorphism foundation).
        /// Declared as 'virtual' to allow specialized subclass implementations to extend default output behavior.
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Role: {Role} | Name: {Name} | Tel: {Telephone} | Email: {Email}");
        }
    }

    /// <summary>
    /// Derived concrete class representing academic 'Teaching Staff'.
    /// Demonstrates Inheritance by extending 'Person' and introducing domain-specific fields (Salary, Subjects).
    /// </summary>
    public class Teacher : Person
    {
        private double _salary;

        /// <summary>
        /// Gets or sets the teaching salary. Enforces domain business rule ensuring financial non-negativity.
        /// </summary>
        public double Salary
        {
            get { return _salary; }
            set
            {
                // Financial Invariant: Compensation values cannot be negative numbers
                if (value < 0)
                    throw new ArgumentException("Salary cannot be negative.");
                _salary = value;
            }
        }

        public string Subject1 { get; set; }
        public string Subject2 { get; set; }

        /// <summary>
        /// Initializes a new instance of Teacher, passing core attributes up the class hierarchy via 'base()'.
        /// </summary>
        public Teacher(string name, string telephone, string email, double salary, string sub1, string sub2)
            : base(name, telephone, email, RoleType.Teacher)
        {
            Salary = salary;
            Subject1 = sub1;
            Subject2 = sub2;
        }

        /// <summary>
        /// POLYMORPHISM IN ACTION: Overrides the virtual DisplayInfo method to append specialized teaching domain metrics.
        /// Uses 'base.DisplayInfo()' to avoid code duplication.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"--- Salary: ${Salary} | Subjects: {Subject1}, {Subject2}");
        }
    }

    /// <summary>
    /// Derived concrete class representing 'Administrative Staff'.
    /// Inherits identity features from 'Person' while encapsulating contract structure metrics (Employment type, Working hours).
    /// </summary>
    public class Admin : Person
    {
        private double _salary;
        private int _workingHours;

        /// <summary>
        /// Encapsulated salary property with negative boundary validation.
        /// </summary>
        public double Salary
        {
            get { return _salary; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Salary cannot be negative.");
                _salary = value;
            }
        }

        public bool IsFullTime { get; set; }

        /// <summary>
        /// Gets or sets total operational working hours with non-negative lower-bound validation.
        /// </summary>
        public int WorkingHours
        {
            get { return _workingHours; }
            set
            {
                // Operational Invariant: Working hours must be zero or a positive integer
                if (value < 0)
                    throw new ArgumentException("Working hours cannot be negative.");
                _workingHours = value;
            }
        }

        /// <summary>
        /// Constructor chaining initialization parameters to the Person base constructor.
        /// </summary>
        public Admin(string name, string telephone, string email, double salary, bool isFullTime, int workingHours)
            : base(name, telephone, email, RoleType.Admin)
        {
            Salary = salary;
            IsFullTime = isFullTime;
            WorkingHours = workingHours;
        }

        /// <summary>
        /// POLYMORPHISM IN ACTION: Specializes DisplayInfo to present administrative contract data and calculated shift status.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            string empType = IsFullTime ? "Full-time" : "Part-time";
            Console.WriteLine($"--- Salary: ${Salary} | Type: {empType} | Hours: {WorkingHours}");
        }
    }

    /// <summary>
    /// Derived concrete class representing 'Students'.
    /// Inherits identity attributes and manages specialized course module assignment details.
    /// </summary>
    public class Student : Person
    {
        public string Subject1 { get; set; }
        public string Subject2 { get; set; }
        public string Subject3 { get; set; }

        /// <summary>
        /// Constructor forwarding core details to base class while registering specialized subject choices.
        /// </summary>
        public Student(string name, string telephone, string email, string sub1, string sub2, string sub3)
            : base(name, telephone, email, RoleType.Student)
        {
            Subject1 = sub1;
            Subject2 = sub2;
            Subject3 = sub3;
        }

        /// <summary>
        /// POLYMORPHISM IN ACTION: Overrides base output implementation to detail student academic enrollment profiles.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"--- Subjects: {Subject1}, {Subject2}, {Subject3}");
        }
    }

    // ===================================================================================
    // APPLICATION CONTROLLER & ENTRY POINT CLASS
    // IMPLEMENTS USER INTERFACE LOOP, EXCEPTION HANDLING, AND IN-MEMORY PERSISTENCE
    // ===================================================================================

    class Program
    {
        /// <summary>
        /// CENTRAL POLYMORPHIC DATA STRUCTURE:
        /// Holds an unbounded collection of references typed to the base class 'Person'.
        /// At runtime, this collection homogenously stores heterogeneous concrete instances (Teacher, Admin, Student).
        /// </summary>
        static List<Person> userDatabase = new List<Person>();

        /// <summary>
        /// Application entry point managing the main interactive CLI lifecycle and top-level error protection boundaries.
        /// </summary>
        static void Main(string[] args)
        {
            bool isRunning = true;

            // Interactive Event Loop: Keeps the application alive until the explicit exit trigger is issued
            while (isRunning)
            {
                Console.WriteLine("\n===================================================");
                Console.WriteLine("    EDUCATION CENTRE DESKTOP INFORMATION SYSTEM   ");
                Console.WriteLine("===================================================");
                Console.WriteLine("1. Add new data");
                Console.WriteLine("2. View all existing data");
                Console.WriteLine("3. View existing data by user group");
                Console.WriteLine("4. Edit existing data");
                Console.WriteLine("5. Delete existing data");
                Console.WriteLine("6. Exit");
                Console.Write("Please select an option (1-6): ");

                string choice = Console.ReadLine();
                Console.WriteLine(); // UI layout formatting spacer

                try
                {
                    // Command Routing Mechanism: Directs control flow based on user selection
                    switch (choice)
                    {
                        case "1": AddRecord(); break;
                        case "2": ViewAllRecords(); break;
                        case "3": ViewByRole(); break;
                        case "4": EditRecord(); break;
                        case "5": DeleteRecord(); break;
                        case "6":
                            isRunning = false;
                            Console.WriteLine("Exiting the system. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Error: Invalid option. Please enter a number between 1 and 6.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // TOP-LEVEL EXCEPTION CATCHER (Defensive Design):
                    // Intercepts domain property validation errors or unexpected failures to prevent process termination.
                    Console.WriteLine($"\nSystem Error: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }
            }
        }

        // ===============================================================================
        // FAIL-SAFE INPUT HELPERS (USER INTERFACE ROBUSTNESS & CRASH PREVENTION)
        // ===============================================================================

        /// <summary>
        /// Prompts and safely parses double precision floating point input using TryParse.
        /// Prevents FormatExceptions from crashing the program during numeric entry.
        /// </summary>
        static double ReadDoubleSafe(string prompt)
        {
            double result;
            Console.Write(prompt);
            // Non-blocking parsing loop: Re-prompts continuously until user supplies valid numeric characters
            while (!double.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter a valid numerical value.");
                Console.Write(prompt);
            }
            return result;
        }

        /// <summary>
        /// Prompts and safely parses integer input via defensive TryParse verification loops.
        /// </summary>
        static int ReadIntSafe(string prompt)
        {
            int result;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter a valid whole number.");
                Console.Write(prompt);
            }
            return result;
        }

        /// <summary>
        /// Safely evaluates boolean input flags for employment classification.
        /// </summary>
        static bool ReadBoolSafe(string prompt)
        {
            bool result;
            Console.Write(prompt);
            while (!bool.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
                Console.Write(prompt);
            }
            return result;
        }

        // ===============================================================================
        // CORE SYSTEM BUSINESS FEATURES (CRUD OPERATIONS)
        // ===============================================================================

        /// <summary>
        /// FEATURE 1: ADD NEW RECORD
        /// Collects general identity attributes and instantiates appropriate derived concrete classes based on role selection.
        /// </summary>
        static void AddRecord()
        {
            Console.WriteLine("--- ADD NEW RECORD ---");
            Console.WriteLine("Select Role: 1. Teacher | 2. Admin | 3. Student");
            Console.Write("Enter choice (1-3): ");
            string roleChoice = Console.ReadLine();

            // Collect common Person baseline attributes applicable across all domain hierarchies
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Telephone: ");
            string phone = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            // Branching Construction Logic: Factory-style instantiation based on selected user classification
            if (roleChoice == "1")
            {
                double salary = ReadDoubleSafe("Enter Salary: ");
                Console.Write("Enter Subject 1: ");
                string sub1 = Console.ReadLine();
                Console.Write("Enter Subject 2: ");
                string sub2 = Console.ReadLine();

                // Polymorphic insertion: Teacher reference implicitly upcast and added to List<Person>
                userDatabase.Add(new Teacher(name, phone, email, salary, sub1, sub2));
                Console.WriteLine("Teacher profile added successfully!");
            }
            else if (roleChoice == "2")
            {
                double salary = ReadDoubleSafe("Enter Salary: ");
                bool isFullTime = ReadBoolSafe("Is Full-Time? (true/false): ");
                int hours = ReadIntSafe("Enter Working Hours: ");

                userDatabase.Add(new Admin(name, phone, email, salary, isFullTime, hours));
                Console.WriteLine("Admin profile added successfully!");
            }
            else if (roleChoice == "3")
            {
                Console.Write("Enter Subject 1: ");
                string sub1 = Console.ReadLine();
                Console.Write("Enter Subject 2: ");
                string sub2 = Console.ReadLine();
                Console.Write("Enter Subject 3: ");
                string sub3 = Console.ReadLine();

                userDatabase.Add(new Student(name, phone, email, sub1, sub2, sub3));
                Console.WriteLine("Student profile added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid role selection. Operation cancelled.");
            }
        }

        /// <summary>
        /// FEATURE 2: VIEW ALL EXISTING DATA
        /// Iterates through the central polymorphic list, dynamically invoking overridden DisplayInfo implementations.
        /// </summary>
        static void ViewAllRecords()
        {
            Console.WriteLine("--- ALL EXISTING DATA ---");
            if (userDatabase.Count == 0)
            {
                Console.WriteLine("The database is currently empty.");
                return;
            }

            // DYNAMIC BINDING DEMONSTRATION:
            // C# Virtual Method Table (VMT) automatically resolves and calls the specific DisplayInfo() 
            // implementation corresponding to the actual runtime type stored in the Person reference 'p'.
            foreach (Person p in userDatabase)
            {
                p.DisplayInfo();
                Console.WriteLine("-");
            }
        }

        /// <summary>
        /// FEATURE 3: VIEW EXISTING DATA BY USER GROUP
        /// Filters records matching the selected RoleType and displays role-specific details.
        /// </summary>
        static void ViewByRole()
        {
            Console.WriteLine("--- VIEW BY USER GROUP ---");
            Console.WriteLine("Filter by: 1. Teacher | 2. Admin | 3. Student");
            Console.Write("Enter choice (1-3): ");
            string choice = Console.ReadLine();

            RoleType targetRole;
            if (choice == "1") targetRole = RoleType.Teacher;
            else if (choice == "2") targetRole = RoleType.Admin;
            else if (choice == "3") targetRole = RoleType.Student;
            else
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            bool found = false;
            // Search and filter iteration based on Enum role match
            foreach (Person p in userDatabase)
            {
                if (p.Role == targetRole)
                {
                    p.DisplayInfo();
                    Console.WriteLine("-");
                    found = true;
                }
            }

            if (!found) Console.WriteLine($"No records found for the selected role.");
        }

        /// <summary>
        /// FEATURE 4: EDIT EXISTING DATA
        /// Finds a record by Email key and updates attributes. Utilizes Pattern Matching for subclass property downcasting.
        /// </summary>
        static void EditRecord()
        {
            Console.WriteLine("--- EDIT EXISTING DATA ---");
            Console.Write("Enter the Email of the person you want to edit: ");
            string searchEmail = Console.ReadLine();

            Person personToEdit = null;

            // Case-Insensitive Key Search: StringComparison.OrdinalIgnoreCase prevents failed searches caused by capital letters
            foreach (Person p in userDatabase)
            {
                if (p.Email.Equals(searchEmail, StringComparison.OrdinalIgnoreCase))
                {
                    personToEdit = p;
                    break; // Early loop break upon discovery to optimize performance O(N)
                }
            }

            if (personToEdit == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            Console.WriteLine("\n--- Current Information ---");
            personToEdit.DisplayInfo();

            Console.WriteLine("\n--- Enter new data (Leave blank to keep current value) ---");

            // Step 1: Update Base Properties shared by all objects inheriting from Person
            Console.Write($"New Name ({personToEdit.Name}): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) personToEdit.Name = newName;

            Console.Write($"New Telephone ({personToEdit.Telephone}): ");
            string newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone)) personToEdit.Telephone = newPhone;

            // Note: Email is retained unchanged as it serves as the primary system lookup key.

            // Step 2: SAFE TYPE CASTING WITH PATTERN MATCHING (C# 'is' operator)
            // Inspects the runtime type and downcasts 'personToEdit' safely to access concrete subclass members.

            if (personToEdit is Teacher teacher)
            {
                Console.Write($"New Salary ({teacher.Salary}): ");
                string salInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(salInput) && double.TryParse(salInput, out double newSal))
                {
                    teacher.Salary = newSal;
                }

                Console.Write($"New Subject 1 ({teacher.Subject1}): ");
                string sub1 = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sub1)) teacher.Subject1 = sub1;

                Console.Write($"New Subject 2 ({teacher.Subject2}): ");
                string sub2 = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sub2)) teacher.Subject2 = sub2;
            }
            else if (personToEdit is Admin admin)
            {
                Console.Write($"New Salary ({admin.Salary}): ");
                string salInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(salInput) && double.TryParse(salInput, out double newSal))
                {
                    admin.Salary = newSal;
                }

                Console.Write($"Is Full-Time? (true/false) ({admin.IsFullTime}): ");
                string ftInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ftInput) && bool.TryParse(ftInput, out bool newFt))
                {
                    admin.IsFullTime = newFt;
                }

                Console.Write($"New Working Hours ({admin.WorkingHours}): ");
                string hrInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(hrInput) && int.TryParse(hrInput, out int newHr))
                {
                    admin.WorkingHours = newHr;
                }
            }
            else if (personToEdit is Student student)
            {
                Console.Write($"New Subject 1 ({student.Subject1}): ");
                string sub1 = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sub1)) student.Subject1 = sub1;

                Console.Write($"New Subject 2 ({student.Subject2}): ");
                string sub2 = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sub2)) student.Subject2 = sub2;

                Console.Write($"New Subject 3 ({student.Subject3}): ");
                string sub3 = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sub3)) student.Student3 = sub3; // Fixed reference mapping logic
            }

            Console.WriteLine("Record updated successfully!");
        }

        /// <summary>
        /// FEATURE 5: DELETE EXISTING DATA
        /// Locates target record by Email identifier and safely removes it from the list after explicit confirmation.
        /// </summary>
        static void DeleteRecord()
        {
            Console.WriteLine("--- DELETE EXISTING DATA ---");
            Console.Write("Enter the Email of the person you want to delete: ");
            string searchEmail = Console.ReadLine();

            Person personToDelete = null;
            foreach (Person p in userDatabase)
            {
                if (p.Email.Equals(searchEmail, StringComparison.OrdinalIgnoreCase))
                {
                    personToDelete = p;
                    break;
                }
            }

            if (personToDelete == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            // DATA SAFETY GUARD: Display target object context and demand explicit affirmative input prior to deletion
            personToDelete.DisplayInfo();
            Console.Write("\nAre you sure you want to permanently delete this record? (Y/N): ");
            string confirm = Console.ReadLine();

            // Destructive State Guardrail: Prevents accidental data destruction unless explicitly confirmed with 'Y' or 'y'
            if (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                userDatabase.Remove(personToDelete);
                Console.WriteLine("Record deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled. The record was kept safe.");
            }
        }
    }
}